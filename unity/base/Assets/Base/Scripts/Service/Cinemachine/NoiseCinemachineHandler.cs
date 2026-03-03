using Unity.Cinemachine;
using UnityEngine;

public class NoiseCinemachineHandler : CinemachineHandler
{
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin = null;

    public async Awaitable Noise(float amplitude, float totalDelay)
    {
        for (var delay = 0f; delay < totalDelay; delay += Time.deltaTime)
        {
            _cinemachineBasicMultiChannelPerlin.AmplitudeGain = Mathf.Lerp(amplitude, 0, delay / totalDelay);

            await Awaitable.NextFrameAsync();
        }

        _cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0;
    }

    protected override void Awake()
    {
        base.Awake();

        _cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
        _cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0;
    }
}
