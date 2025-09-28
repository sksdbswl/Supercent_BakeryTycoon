using UnityEngine;
using UnityEngine.UI;

public enum SoundType
{
    Get,
    Put,
    Cashier,
    Money,
    Open,
    CleanUp,
}

public class SoundManager : Singleton<SoundManager>
{
    [Header("Effect Sounds")]
    public AudioClip getClip;
    public AudioClip putClip;
    public AudioClip cashierClip;
    public AudioClip moneyClip;
    public AudioClip openClip;
    public AudioClip cleanUpClip;

    [Header("Source")]
    private AudioSource sfxSource;

    [Header("Sound Value")]
    public float sfxVolume = 1f;

    [Header("Sound Controller")]
    public Slider sfxVolumeSlider;

    private void Awake()
    {
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;
    }

    private void Start()
    {
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = sfxVolume;
            sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
        }
    }

    public void PlaySound(SoundType soundType)
    {
        AudioClip clip = null;

        switch (soundType)
        {
            case SoundType.Get:
                clip = getClip;
                break;
            case SoundType.Put:
                clip = putClip;
                break;
            case SoundType.Cashier:
                clip = cashierClip;
                break;
            case SoundType.Money:
                clip = moneyClip;
                break;
            case SoundType.Open:
                clip = openClip;
                break;
            case SoundType.CleanUp:
                clip = cleanUpClip;
                break;
            default:
                Debug.LogWarning("Unhandled SoundType: " + soundType);
                return;
        }

        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = value;
        if (sfxSource != null)
            sfxSource.volume = value;
    }
}
