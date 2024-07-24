using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;
    private bool isPaused;

    [System.Serializable]

    public class AudioObject
    {
        public AudioClip AudioClip;
        [Range(0,1)] public float volume = 1f;
    }

    public AudioObject loseAudioClip;
    public AudioObject cleaningAudioClip;
    public AudioObject pauseAudioClip;
    public AudioObject bonusAudioClip;
    public AudioObject tutorialAudioClip;
    public AudioObject selectAudioClip;
    public AudioObject hitAudioClip;
    public AudioObject finishCleanAudioClip;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioObject clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip.AudioClip);
        }
    }

    public void PlayLoseSound() => PlaySound(loseAudioClip);
    public void PlayCleanSound() => PlaySound(cleaningAudioClip);
    public void PlayBonusSound() => PlaySound(bonusAudioClip);
    public void PlayPauseSound() => PlaySound(pauseAudioClip);
    public void PlayTutorialSound() => PlaySound(tutorialAudioClip);
    public void PlayHitSound() => PlaySound(hitAudioClip);
    public void PlaySelectSound() => PlaySound(selectAudioClip);
    public void PlayFinishCleanSound() => PlaySound(finishCleanAudioClip);

    public void PauseAudio()
    {
        Debug.Log("pause music");
        if (!audioSource.mute)
        {
            audioSource.Pause();
            isPaused = true;
        }
    }

    public void ResumeAudio()
    {
        if (isPaused)
        {
            audioSource.UnPause();
            isPaused = false;
        }
    }
    
    public void ToggleMute()
    {
        audioSource.mute = !audioSource.mute;
        isPaused = true;
    }
}
