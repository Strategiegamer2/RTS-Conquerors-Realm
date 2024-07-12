using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;

    public AudioClip mainTheme;
    public AudioClip[] backgroundTracks;

    private int currentTrackIndex = -1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        LoadSettings();
        PlayMainTheme();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("SoundVolume", volume);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        float volume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        audioSource.volume = volume;
    }

    public void PlayMainTheme()
    {
        if (mainTheme != null)
        {
            audioSource.clip = mainTheme;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % backgroundTracks.Length;
        PlayTrack(currentTrackIndex);
    }

    public void PlayTrack(int index)
    {
        if (index >= 0 && index < backgroundTracks.Length)
        {
            audioSource.clip = backgroundTracks[index];
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
