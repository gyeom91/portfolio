using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattlePanel : UIPanel
{
    [SerializeField] private Button _optionBtn;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _healthTmp;
    [SerializeField] private Slider _expSlider;
    [SerializeField] private TextMeshProUGUI _levelTmp;
    [SerializeField] private Transform[] _abilitySlot;
    [SerializeField] private UIAbility _uIAbilityPrefab;
    private Dictionary<string, UIAbility> _uIAbilities = new();

    public void SetAbilityCoolTimer(string skillName, float cooltime)
    {
        if (_uIAbilities.ContainsKey(skillName) == false)
            return;

        _uIAbilities[skillName].SetFillAmount(cooltime);
    }

    public void CreateAbility(string skillName)
    {
        var index = _uIAbilities.Count;
        if (index >= Constants.BATTLE_ABILITY_MAX_SLOT)
            return;

        var sceneController = SceneController.Instance;
        var poolService = sceneController.GetService<PoolService>();
        var clone = poolService.Spawn(_uIAbilityPrefab, _abilitySlot[index]);
        _uIAbilities.Add(skillName, clone);
    }

    public void SetLevel(float level)
    {
        _levelTmp.text = $"{Mathf.RoundToInt(level)}";
    }

    public void SetExp(float exp)
    {
        _expSlider.value = exp;
    }

    public void SetMaxHealth(float maxHealth)
    {
        _healthSlider.maxValue = maxHealth;
    }

    public void SetHealth(float health)
    {
        _healthTmp.text = string.Format("{0:F0}%", health);
        _healthSlider.value = health;
    }

    protected override void Awake()
    {
        base.Awake();

        _optionBtn.onClick.AddListener(OnClickedOptionBtn);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private async void OnClickedOptionBtn()
    {
        await FSMModule.Trigger(Constants.BATTLE_OPTION);
    }
}
