using UnityEngine;

[CreateAssetMenu(fileName = "Wait", menuName = PATH + "Wait")]
public class Wait : Leaf
{
    [SerializeField] private float _seconds;

    public override async Awaitable<EState> Execute(MonoBehaviour behaviour)
    {
        await Awaitable.WaitForSecondsAsync(_seconds);

        return EState.Succeed;
    }
}
