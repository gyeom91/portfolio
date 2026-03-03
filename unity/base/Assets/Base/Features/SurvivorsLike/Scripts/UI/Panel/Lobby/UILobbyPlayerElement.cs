using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyPlayerElement : UI
{
    public string PlayerName { get; private set; }
    public int PlayerIndex { get; private set; }

    [SerializeField] private TextMeshProUGUI _playerNameTMPro;
    [SerializeField] private Image _readyImg;

    public void Setup(string playerName, int playerIndex, string ready)
    {
        PlayerName = playerName;
        PlayerIndex = playerIndex;
        _playerNameTMPro.text = $"{playerName}";
        _readyImg.enabled = bool.Parse(ready);
    }
}
