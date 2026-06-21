using Cysharp.Threading.Tasks;
using UnityEngine;

public class SoundManager : BaseMonoManager<SoundManager>
{
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioLowPassFilter _bgmLowPassFilter;

    [SerializeField] private float _sfxVolume = 0.5f;
    [SerializeField] private float _bgmVolume = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        _sfxSource.volume = _sfxVolume;
        _bgmSource.volume = _bgmVolume;
    }

    public async UniTaskVoid PlaySFX(string address, float volumeScale = 1f)
    {
        AudioClip clip = await LoadUtil.Async.LoadGenericAsync<AudioClip>(address);

        if (_sfxSource == null) return;

        if (clip == null) return;

        _sfxSource.PlayOneShot(clip, _sfxVolume * volumeScale);
    }

    public void StopSFX()
    {
        if (_sfxSource != null)
        {
            _sfxSource.Stop();
        }
    }
    public async UniTaskVoid PlayBGM(string address)
    {
        AudioClip clip = await LoadUtil.Async.LoadGenericAsync<AudioClip>(address);

        if (_bgmSource == null) return;

        if (clip == null) return;

        _bgmSource.clip = clip;
        _bgmSource.loop = true;
        _bgmSource.Play();
    }

    public void EnableMuffle()
    {
        if (_bgmLowPassFilter == null) return;
        _bgmLowPassFilter.cutoffFrequency = 1000f;
    }

    public void DisableMuffle()
    {
        if (_bgmLowPassFilter == null) return;
        _bgmLowPassFilter.cutoffFrequency = 22000f;
    }

    public void StopBGM()
    {
        if (_bgmSource != null)
        {
            _bgmSource.Stop();
        }
    }

    public float GetSFXVolume()
    {
        return _sfxVolume;
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVolume = volume;
        _sfxSource.volume = volume;
    }

    public void SetBGMVolume(float volume)
    {
        _bgmVolume = volume;
        _bgmSource.volume = volume;
    }
}