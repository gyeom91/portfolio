using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PoolService : Service
{
    protected Dictionary<string, (AsyncOperationHandle, UnityEngine.Object)> _handlePools = new();
    protected Dictionary<string, Queue<GameObject>> _pools = new();

    [SerializeField] private string[] _assetLabels = null;

    public virtual GameObject Spawn(GameObject prefab, Transform parent = null, int defaultSpawnCount = 10)
    {
        var key = prefab.name;
        if (_pools.TryGetValue(key, out var queue) == false)
        {
            queue = new();

            _pools.Add(key, queue);
        }

        if (queue.Count == 0)
        {
            for (var i = 0; i < defaultSpawnCount; ++i)
            {
                var go = Instantiate(prefab, this.transform);
                go.name = key;
                go.SetActive(false);

                queue.Enqueue(go);
            }
        }

        var clone = queue.Dequeue();
        clone.SetActive(true);

        var cloneTransform = clone.transform;
        cloneTransform.SetParent(parent);
        cloneTransform.localPosition = Vector3.zero;
        cloneTransform.localRotation = Quaternion.identity;
        cloneTransform.localScale = Vector3.one;

        return clone;
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null, int defaultSpawnCount = 10)
    {
        var clone = Spawn(prefab, parent, defaultSpawnCount);
        clone.transform.localPosition = position;
        clone.transform.localRotation = rotation;
        clone.transform.localScale = prefab.transform.localScale;

        return clone;
    }

    public T Spawn<T>(GameObject prefab, Transform parent = null, int defaultSpawnCount = 10)
    {
        return Spawn(prefab, parent, defaultSpawnCount).GetComponent<T>();
    }

    public T Spawn<T>(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null, int defaultSpawnCount = 10)
    {
        return Spawn(prefab, position, rotation, parent, defaultSpawnCount).GetComponent<T>();
    }

    public T Spawn<T>(T prefab, Transform parent = null, int defaultSpawnCount = 10) where T : MonoBehaviour
    {
        return Spawn<T>(prefab.gameObject, parent, defaultSpawnCount);
    }

    public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null, int defaultSpawnCount = 10) where T : MonoBehaviour
    {
        return Spawn<T>(prefab.gameObject, position, rotation, parent, defaultSpawnCount);
    }

    public virtual async Awaitable<GameObject> SpawnAsync(GameObject prefab, Transform parent = null, int defaultSpawnCount = 10)
    {
        var key = prefab.name;
        if (_pools.TryGetValue(key, out var queue) == false)
        {
            queue = new();

            _pools.Add(key, queue);
        }

        if (queue.Count == 0)
        {
            var gameObjects = await InstantiateAsync(prefab, defaultSpawnCount, this.transform);
            for (var i = 0; i < defaultSpawnCount; ++i)
            {
                var go = gameObjects[i];
                go.name = key;
                go.SetActive(false);

                queue.Enqueue(go);
            }
        }

        var clone = queue.Dequeue();
        clone.SetActive(true);

        var cloneTransform = clone.transform;
        cloneTransform.SetParent(parent);
        cloneTransform.localPosition = Vector3.zero;
        cloneTransform.localRotation = Quaternion.identity;
        cloneTransform.localScale = Vector3.one;

        return clone;
    }

    public async Awaitable<GameObject> SpawnAsync(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null, int defaultSpawnCount = 10)
    {
        var clone = await SpawnAsync(prefab, parent, defaultSpawnCount);
        clone.transform.localPosition = position;
        clone.transform.localRotation = rotation;
        clone.transform.localScale = prefab.transform.localScale;

        return clone;
    }

    public async Awaitable<T> SpawnAsync<T>(GameObject prefab, Transform parent = null, int defaultSpawnCount = 10)
    {
        var clone = await SpawnAsync(prefab, parent, defaultSpawnCount);

        return clone.GetComponent<T>();
    }

    public async Awaitable<T> SpawnAsync<T>(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null, int defaultSpawnCount = 10)
    {
        var clone = await SpawnAsync(prefab, position, rotation, parent, defaultSpawnCount);

        return clone.GetComponent<T>();
    }

    public async Awaitable<T> SpawnAsync<T>(T prefab, Transform parent = null, int defaultSpawnCount = 10) where T : MonoBehaviour
    {
        var clone = await SpawnAsync(prefab.gameObject, parent, defaultSpawnCount);

        return clone.GetComponent<T>();
    }

    public async Awaitable<T> SpawnAsync<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null, int defaultSpawnCount = 10) where T : MonoBehaviour
    {
        var clone = await SpawnAsync(prefab.gameObject, position, rotation, parent, defaultSpawnCount);

        return clone.GetComponent<T>();
    }

    public async Awaitable<GameObject> SpawnAsync(string addressableName, Transform parent = null)
    {
        var asset = await LoadAsync<GameObject>(addressableName);

        return await SpawnAsync(asset, parent);
    }

    public async Awaitable<GameObject> SpawnAsync(string addressableName, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var asset = await LoadAsync<GameObject>(addressableName);

        return await SpawnAsync(asset, position, rotation, parent);
    }

    public async Awaitable<T> SpawnAsync<T>(string addressableName, Transform parent = null)
    {
        var key = string.IsNullOrEmpty(addressableName) ? typeof(T).Name : addressableName;
        var asset = await LoadAsync<GameObject>(key);

        return await SpawnAsync<T>(asset, parent);
    }

    public async Awaitable<T> SpawnAsync<T>(string addressableName, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        var key = string.IsNullOrEmpty(addressableName) ? typeof(T).Name : addressableName;
        var asset = await LoadAsync<GameObject>(key);

        return await SpawnAsync<T>(asset, position, rotation, parent);
    }

    public virtual async void Despawn(GameObject target, float delay = 0)
    {
        await Awaitable.WaitForSecondsAsync(delay);

        if (target.IsNull())
            return;

        if (_pools.TryGetValue(target.name, out var queue) == false)
        {
            Destroy(target);
            return;
        }

        target.SetActive(false);

        var targetTransform = target.transform;
        targetTransform.SetParent(transform);

        queue.Enqueue(target);
    }

    public virtual void Despawn(MonoBehaviour monoBehaviour, float delay = 0)
    {
        if (monoBehaviour.IsNull())
            return;

        Despawn(monoBehaviour.gameObject, delay);
    }

    public async Awaitable<T> LoadAsync<T>(string addressableName = "") where T : UnityEngine.Object
    {
        var key = string.IsNullOrEmpty(addressableName) ? typeof(T).Name : addressableName;
        if (_handlePools.TryGetValue(key, out var output) == false)
        {
            var handle = Addressables.LoadAssetAsync<T>(key);
            await handle.Task;

            var result = handle.Result;
            if (_handlePools.ContainsKey(key))
            {
                Addressables.Release(handle);
            }
            else
            {
                _handlePools.Add(key, (handle, result));
            }
        }
        var item = _handlePools[key];

        return item.Item2 as T;
    }

    public async Awaitable Initialize(Action<long, long, float> onUpdate, Action onSucceeded, Action<string> onFailed)
    {
        await Addressables.InitializeAsync().Task;

        if (_assetLabels.IsNull() == false)
        {
            var length = _assetLabels.Length;
            for (var i = 0; i < length; ++i)
            {
                var label = _assetLabels[i];
                var sizeHandler = Addressables.GetDownloadSizeAsync(label);
                var downloadSize = await sizeHandler.Task;
                Addressables.Release(sizeHandler);
                if (downloadSize == 0)
                    continue;

                var asyncOperation = Addressables.DownloadDependenciesAsync(label, false);
                while (asyncOperation.IsDone == false)
                {
                    var downloadStatus = asyncOperation.GetDownloadStatus();
                    onUpdate?.Invoke(downloadStatus.TotalBytes, downloadStatus.DownloadedBytes, downloadStatus.Percent);

                    await Awaitable.NextFrameAsync();
                }

                var status = asyncOperation.Status;
                var exception = asyncOperation.OperationException;
                var message = string.Empty;
                if (exception.IsNull() == false)
                    message = exception.Message;

                Addressables.Release(asyncOperation);

                if (status == AsyncOperationStatus.Failed)
                {
                    onFailed?.Invoke(message);
                    return;
                }
            }
        }

        onSucceeded?.Invoke();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        foreach (var pools in _pools)
        {
            var pool = pools.Value;
            pool.Clear();
        }
        _pools.Clear();
        _pools = null;

        foreach (var pools in _handlePools)
        {
            var items = pools.Value;
            Addressables.Release(items.Item1);
        }
        _handlePools.Clear();
        _handlePools = null;
    }
}
