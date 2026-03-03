using TMPro;
using UnityEngine;

public class LobbyActor : Actor
{
    [SerializeField] private TextMeshProUGUI _name;

    private Canvas _canvas = null;

    public void SetEnableCanvas(bool enable)
    {
        _canvas.enabled = enable;
    }

    public void Setup(string playerName)
    {
        _name.text = playerName;
    }

    protected override void Awake()
    {
        base.Awake();

        _canvas = GetComponentInChildren<Canvas>();
    }
}
