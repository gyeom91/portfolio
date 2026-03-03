using UnityEngine;

public class IKHead : BaseMonobehaviour
{
    public void LookAt(Vector3 position)
    {
        var direction = position - transform.position;
        direction.Normalize();

        var targetRotation = Quaternion.LookRotation(direction);
        var targetEuler = targetRotation.eulerAngles;
        targetEuler.z = 0f;

        transform.rotation = Quaternion.Euler(targetEuler);
    }
}
