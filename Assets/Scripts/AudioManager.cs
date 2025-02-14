/*using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    [SerializeField] private AudioClip backgroundClip;
    [SerializeField] private AudioClip jumpClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayBackgroundMusic();
    }
    // nhạc nền 
    public void PlayBackgroundMusic()
    {
        backgroundAudioSource.clip = backgroundClip;
        backgroundAudioSource.Play();
    }
    // effect nhảy
    public void PlayJumpSound()
    {
        effectAudioSource.PlayOneShot(jumpClip);
    }
}
*/

using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip backgroundClip;
    [SerializeField] private AudioClip jumpClip;
    public Slider backgroundVolumeSlider;
    public Slider effectVolumeSlider;
    public Button backgroundMuteButton;
    public Button effectMuteButton;
    public Sprite soundOnIcon;
    public Sprite soundOffIcon;
    private bool isBackgroundMuted = false;
    private bool isEffectMuted = false;
    private float lastBackgroundVolume;
    private float lastEffectVolume;

    public bool isPaused = false; // Biến kiểm soát trạng thái tạm dừng

    void Start()
    {
        PlayBackgroundMusic();
        backgroundVolumeSlider.value = backgroundAudioSource.volume;
        effectVolumeSlider.value = effectAudioSource.volume;
        backgroundVolumeSlider.onValueChanged.AddListener(SetBackgroundVolume);
        effectVolumeSlider.onValueChanged.AddListener(SetEffectVolume);
        backgroundMuteButton.onClick.AddListener(ToggleBackgroundMute);
        effectMuteButton.onClick.AddListener(ToggleEffectMute);
        UpdateBackgroundMuteIcon();
        UpdateEffectMuteIcon();
    }

    public void PlayBackgroundMusic()
    {
        backgroundAudioSource.clip = backgroundClip;
        backgroundAudioSource.Play();
    }

    public void PlayJumpSound()
    {
        if (!isPaused) // Chỉ phát âm thanh nếu game không bị tạm dừng
        {
            effectAudioSource.PlayOneShot(jumpClip);
        }
    }

    void SetBackgroundVolume(float volume)
    {
        if (!isBackgroundMuted)
            backgroundAudioSource.volume = volume;

        UpdateBackgroundMuteIcon();
    }

    void SetEffectVolume(float volume)
    {
        if (!isEffectMuted)
            effectAudioSource.volume = volume;

        UpdateEffectMuteIcon();
    }

    void ToggleBackgroundMute()
    {
        isBackgroundMuted = !isBackgroundMuted;

        if (isBackgroundMuted)
        {
            lastBackgroundVolume = backgroundAudioSource.volume;
            backgroundAudioSource.volume = 0;
            backgroundVolumeSlider.value = 0;
        }
        else
        {
            backgroundAudioSource.volume = lastBackgroundVolume;
            backgroundVolumeSlider.value = lastBackgroundVolume;
        }

        backgroundAudioSource.mute = isBackgroundMuted;
        UpdateBackgroundMuteIcon();
    }

    void ToggleEffectMute()
    {
        isEffectMuted = !isEffectMuted;

        if (isEffectMuted)
        {
            lastEffectVolume = effectAudioSource.volume;
            effectAudioSource.volume = 0;
            effectVolumeSlider.value = 0;
        }
        else
        {
            effectAudioSource.volume = lastEffectVolume;
            effectVolumeSlider.value = lastEffectVolume;
        }

        effectAudioSource.mute = isEffectMuted;
        UpdateEffectMuteIcon();
    }

    void UpdateBackgroundMuteIcon()
    {
        bool isSilent = backgroundVolumeSlider.value == 0;
        backgroundMuteButton.image.sprite = isSilent ? soundOffIcon : soundOnIcon;
    }

    void UpdateEffectMuteIcon()
    {
        bool isSilent = effectVolumeSlider.value == 0;
        effectMuteButton.image.sprite = isSilent ? soundOffIcon : soundOnIcon;
    }
}

