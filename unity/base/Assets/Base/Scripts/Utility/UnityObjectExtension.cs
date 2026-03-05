using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class UnityObjectExtension
{
    public static async void LoadScene(this UnityEngine.Object unityObject, int sceneIndex, LoadSceneMode loadSceneMode, Action<float> onUpdate = null, Action onCompleted = null)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
        if (asyncOperation.IsNull())
            return;

        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < .9f)
        {
            await Awaitable.NextFrameAsync();

            onUpdate?.Invoke(asyncOperation.progress);
        }
        onUpdate?.Invoke(1);

        asyncOperation.allowSceneActivation = true;

        while (asyncOperation.isDone == false)
            await Awaitable.NextFrameAsync();

        onCompleted?.Invoke();
    }

    public static async void LoadScene(this UnityEngine.Object unityObject, string sceneName, LoadSceneMode loadSceneMode, Action<float> onUpdate = null, Action onCompleted = null)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < .9f)
        {
            await Awaitable.NextFrameAsync();

            onUpdate?.Invoke(asyncOperation.progress);
        }
        onUpdate?.Invoke(1);

        asyncOperation.allowSceneActivation = true;

        while (asyncOperation.isDone == false)
            await Awaitable.NextFrameAsync();

        onCompleted?.Invoke();
    }

    public static Awaitable AsAwaitable<T>(this UnityEvent unityEvent)
    {
        var source = new AwaitableCompletionSource();
        UnityAction action = null;
        action = () =>
        {
            unityEvent.RemoveListener(action);
            source.TrySetResult();
        };

        unityEvent.AddListener(action);
        return source.Awaitable;
    }

    public static Awaitable<T> AsAwaitable<T>(this UnityEvent<T> unityEvent)
    {
        var source = new AwaitableCompletionSource<T>();
        UnityAction<T> action = null;
        action = value =>
        {
            unityEvent.RemoveListener(action);
            source.TrySetResult(value);
        };

        unityEvent.AddListener(action);
        return source.Awaitable;
    }
}
