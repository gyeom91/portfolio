using UnityEngine;

public abstract class Composite : Leaf
{
    [SerializeField] protected Leaf[] _childs;
}
