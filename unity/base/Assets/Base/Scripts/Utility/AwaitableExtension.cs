using System;
using UnityEngine;

public static class AwaitableExtension
{
    public static Awaitable WaitUntil(this Func<bool> callback, float interval)
    {
        var source = new AwaitableCompletionSource();

        async void Do()
        {
            while (callback() == false)
            {
                await Awaitable.WaitForSecondsAsync(interval);
            }

            source.SetResult();
        }

        Do();

        return source.Awaitable;
    }
}
