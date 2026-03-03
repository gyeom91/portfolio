using Unity.Netcode;
using UnityEngine;

public class BaseNetworkBehaviour : NetworkBehaviour
{
    public virtual bool IsActive { get { return gameObject.activeInHierarchy; } }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    protected virtual void Awake()
    {

    }
}
