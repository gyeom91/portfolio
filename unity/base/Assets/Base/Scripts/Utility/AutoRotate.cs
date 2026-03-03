using UnityEngine;

public class AutoRotate : BaseMonobehaviour
{
    [SerializeField] private Vector3 _eulers;

    private void Update()
    {
        transform.Rotate(_eulers * Time.unscaledDeltaTime);
    }
}
