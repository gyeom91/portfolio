using System.Collections.Generic;
using UnityEngine;

public class BaseSoundService : Service
{
    public struct SoundModel
    {
        public AudioClip Clip;
        public string SoundID;
        public bool Loop;
    }

    [SerializeField] private SoundHandler _handlerPrefab;
    [SerializeField] private SoundModel[] _soundModels;
    private Dictionary<string, SoundHandler> _handlerContainer = new();

    public void Play(string soundID, float volume, bool removeExisting = true)
    {
        SoundModel soundModel = GetSoundModel(soundID);
        if (soundModel.Equals(default(SoundModel)))
            return;

        if (_handlerContainer.ContainsKey(soundID))
        {
            var controller = _handlerContainer[soundID];
            controller.Stop();
            controller.Play();
        }
        else
        {
            var sceneController = SceneController.Instance;
            var poolService = sceneController.GetService<PoolService>();
            var controller = poolService.Spawn(_handlerPrefab);
            controller.Play(soundModel, volume);

            _handlerContainer.Add(soundID, controller);
        }
    }

    public void Stop(string soundID)
    {
        if (_handlerContainer.ContainsKey(soundID) == false)
            return;

        var controller = _handlerContainer[soundID];
        controller.Stop();

        var sceneController = SceneController.Instance;
        var poolService = sceneController.GetService<PoolService>();
        poolService.Despawn(controller);

        _handlerContainer.Remove(soundID);
    }

    public void StopAll()
    {
        var sceneController = SceneController.Instance;
        var poolService = sceneController.GetService<PoolService>();
        var soundPoolables = _handlerContainer.Values;
        foreach (var controller in soundPoolables)
        {
            controller.Stop();
            poolService.Despawn(controller);
        }

        _handlerContainer.Clear();
    }

    public void SetVolume(float volume)
    {
        var soundPoolables = _handlerContainer.Values;
        foreach (var controller in soundPoolables)
        {
            controller.SetVolume(volume);
        }
    }

    public void SetLoop(string soundID, bool loop)
    {
        if (_handlerContainer.ContainsKey(soundID) == false)
            return;

        var controller = _handlerContainer[soundID];
        controller.SetLoop(loop);
    }

    protected override void Awake()
    {
        base.Awake();

        SoundHandler.OnStoppedSound += OnStoppedSound;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        SoundHandler.OnStoppedSound -= OnStoppedSound;

        _handlerContainer.Clear();
        _handlerContainer = null;
    }

    protected virtual void OnStoppedSound(SoundHandler handler)
    {
        var sceneController = SceneController.Instance;
        var poolService = sceneController.GetService<PoolService>();
        poolService.Despawn(handler);

        _handlerContainer.Remove(handler.SoundID);
    }

    private SoundModel GetSoundModel(string soundID)
    {
        if (_soundModels.IsNull())
            return default(SoundModel);

        var length = _soundModels.Length;
        for (var i = 0; i < length; ++i)
        {
            var model = _soundModels[i];
            if (model.SoundID == soundID)
                return model;
        }

        return default(SoundModel);
    }
}
