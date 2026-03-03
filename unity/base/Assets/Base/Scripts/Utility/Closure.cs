using System;
using UnityEngine;

public struct Closure
{
    private Delegate _delegate;
    private object _context;

    public Closure(Delegate del, object context = null)
    {
        _delegate = del;
        _context = context;
    }

    public void Invoke()
    {
        if (_delegate is Action action)
        {
            action();
        }
        else if (_delegate is Action<object> actionWithContext)
        {
            actionWithContext(_context);
        }
    }

    public void Invoke<T>()
    {
        if (_delegate is Func<T> func)
        {
            func();
        }
        else if (_delegate is Func<object, T> funcWithContext)
        {
            funcWithContext(_context);
        }
    }

    public static Closure Create(Action action) => new Closure(action);
    public static Closure Create<T>(Action<T> action, T obj) => new Closure(action, obj);
    public static Closure Create<T>(Func<T> func) => new Closure(func);
    public static Closure Create<T, TResult>(Func<T, TResult> func, T obj) => new Closure(func, obj);
}
