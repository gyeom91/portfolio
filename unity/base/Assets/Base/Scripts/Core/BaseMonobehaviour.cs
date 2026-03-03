using UnityEngine;

public class BaseMonobehaviour : MonoBehaviour
{
    public virtual bool IsActive { get { return gameObject.activeInHierarchy; } }

    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    protected virtual void Awake()
    {

    }
}
