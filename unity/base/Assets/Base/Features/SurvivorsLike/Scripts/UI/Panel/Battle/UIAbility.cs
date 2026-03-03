using UnityEngine;
using UnityEngine.UI;

public class UIAbility : UI
{
    [SerializeField] private Image _filledImage;

    public void SetFillAmount(float amount)
    {
        _filledImage.fillAmount = amount;
    }
}
