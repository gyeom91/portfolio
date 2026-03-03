using UnityEngine;

public abstract class Decorator : Leaf
{
    [SerializeField] protected Leaf _child;
}
