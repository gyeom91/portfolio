using System;
using UnityEngine;

public class UILobbyCharacterSelectPage : UIPage
{
    [SerializeField] private UICharacterSelectToggle[] _toggles;

    protected override void OnShow()
    {
        base.OnShow();

        var toggle = Array.Find(_toggles, toggle => toggle.CharacterName == PlayerPrefs.GetString("CharacterName", "Archer"));
        if (toggle.IsNull())
            return;

        toggle.IsOn = true;
    }
}
