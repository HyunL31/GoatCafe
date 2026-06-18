using UnityEngine;

public class SoundManager : BaseMonoManager<SoundManager>
{
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _bgmSource;

    [SerializeField] private float _sfxVolume = 0.5f;
    [SerializeField] private float _bgmVolume = 0.5f;


}
