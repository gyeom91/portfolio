using System;
using UnityEngine;

public class SoundHandler : BaseMonobehaviour
{
    public static event Action<SoundHandler> OnStoppedSound;
    public string SoundID { get; private set; }

    private AudioSource _audioSource = null;

    public void Play(BaseSoundService.SoundModel soundModel, float volume)
    {
        SoundID = soundModel.SoundID;
        _audioSource.clip = soundModel.Clip;
        _audioSource.loop = soundModel.Loop;
        _audioSource.volume = volume;
        _audioSource.Play();
    }

    public void Play()
    {
        _audioSource.Play();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }

    public void SetLoop(bool loop)
    {
        _audioSource.loop = loop;
    }

    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    protected override void Awake()
    {
        base.Awake();

        _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        if (_audioSource.loop)
            return;

        if (_audioSource.isPlaying)
            return;

        OnStoppedSound?.Invoke(this);
    }
}
