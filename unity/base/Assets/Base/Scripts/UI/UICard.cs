using TMPro;
using UnityEngine;

public class UICard : UI
{
    [SerializeField] protected TextMeshProUGUI _slotIndex;

    public virtual void Create(int slotIndex)
    {
        _slotIndex.text = $"{slotIndex}";
    }
}
