using UnityEngine;

public class Actor : BaseMonobehaviour
{
    protected Renderer[] _renderers { get; private set; }

    public override void SetActive(bool active)
    {
        base.SetActive(active);

        var length = _renderers.Length;
        for (var i = 0; i < length; ++i)
        {
            _renderers[i].enabled = active;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _renderers = GetComponentsInChildren<Renderer>();
    }
}
