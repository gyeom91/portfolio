using Unity.Netcode;
using UnityEngine;

public class Hero : BattleCharacter
{
    [SerializeField] private float _noiseAmplitude;
    [SerializeField] private float _noiseDuration;
    private NetworkAttributeAdapter _networkAttributeAdapter = null;

    public override void OnInitialize(NetworkObject networkObject)
    {
        base.OnInitialize(networkObject);

        if (networkObject.IsOwner == false)
            return;

        var sceneController = SceneController.Instance;
        var cinemachineService = sceneController.GetService<CinemachineService>();
        var noiseCinemachineHandler = cinemachineService.GetHandler<NoiseCinemachineHandler>();
        noiseCinemachineHandler.SetFollowTarget(transform);
        noiseCinemachineHandler.SetLookAtTarget(transform);

        cinemachineService.ChangeHandler(noiseCinemachineHandler);
    }

    protected override void OnBehaviour()
    {
        base.OnBehaviour();

        if (_networkObject.IsOwner == false)
            return;

        _moveDirection = InputService.MoveDirection;
    }

    protected override void OnFixedBehaviour()
    {
        if (_networkObject.IsOwner == false)
            return;

        base.OnFixedBehaviour();
    }

    protected override void Awake()
    {
        base.Awake();

        _networkAttributeAdapter = GetComponent<NetworkAttributeAdapter>();
        _networkAttributeAdapter.OnChangedNetworkAttributeList += OnChangedNetworkAttributeList;

        ASC.AddAttributeSet(new HeroAttributeSet(CharacterName));
    }

    private void Update()
    {
        if (SceneController.Instance is BattleSceneController sceneController == false || sceneController.Freeze)
            return;

        OnBehaviour();
    }

    private void FixedUpdate()
    {
        if (SceneController.Instance is BattleSceneController sceneController == false || sceneController.Freeze)
            return;

        OnFixedBehaviour();
    }

    private void OnDestroy()
    {
        _networkAttributeAdapter.OnChangedNetworkAttributeList -= OnChangedNetworkAttributeList;
    }

    private void OnChangedNetworkAttributeList(NetworkListEvent<NetworkAttributeData> networkListEvent, NetworkList<NetworkAttributeData> networkAttributeDatas)
    {
        if (_networkObject.IsOwner == false)
            return;

        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        var uIBattlePanel = uIService.Get<UIBattlePanel>();
        var networkAttributeData = networkListEvent.Value;
        var attributeName = networkAttributeData.AttributeName;
        switch (attributeName.ToString())
        {
            case SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_MaxHealth:
                {
                    uIBattlePanel.SetMaxHealth(networkAttributeData.CurrentValue);
                }
                break;

            case SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Health:
                {
                    uIBattlePanel.SetHealth(networkAttributeData.CurrentValue);
                }
                break;
        }
    }
}
