using Unity.Netcode;
using UnityEngine;

public class Hero : BattleCharacter
{
    [SerializeField] private float _noiseAmplitude;
    [SerializeField] private float _noiseDuration;

    public override void OnInitialize(NetworkObject networkObject)
    {
        base.OnInitialize(networkObject);

        if (networkObject.IsOwner == false)
            return;

        var heroTable = TableManager.Instance.GetTable<HeroTable>();
        var characterName = name.Substring(name.IndexOf('_') + 1);
        if (heroTable.TryGetData(characterName, out var heroData) == false)
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

    private void Update()
    {
        if (SceneController.Instance is BattleSceneController sceneController && sceneController.Freeze)
            return;

        OnBehaviour();
    }

    private void FixedUpdate()
    {
        if (_networkObject.IsOwner == false)
            return;

        if (SceneController.Instance is BattleSceneController sceneController == false || sceneController.Freeze)
            return;

        OnFixedBehaviour();

        var worldService = sceneController.GetService<WorldService>();
        worldService.UpdatePosition(this);
    }
}
