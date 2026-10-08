using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioSource _clockSound;

    [Header("Sound Data Bank")]
    public SoundData OpenDialog;
    public SoundData CloseDialog;
    public SoundData CountItem;
    public SoundData ScoreAdd;
    public SoundData ScoreMultiply;
    public SoundData ItemHighlight;
    public SoundData NumberIncrease;
    
    public SoundData BonusHighlight;
    public SoundData BonusHover;
    public SoundData BonusShow;
    public SoundData BonusHide;

    public SoundData Combination;

    [Header("Parameters")]
    public float SoundScaler;

    public void StartClockSound()
    {
        //_clockSound.Play();
    }

    public void StopClockSound()
    {
        //_clockSound.Stop();
    }

    public void PlaySFXSound(AudioClip clip)
    {
        _sfx.PlayOneShot(clip);
    }

    public static void PlaySound(SoundData sound)
    {
        PlaySound(sound, 0);
    }

    public static void PlaySound(SoundData sound, float pitchBonus)
    {
        AudioClip newAudio;

        if (sound.audioClips.Length < 1)
        {
            return;
        }

        if (sound.audioClips.Length > 1)
        {
            newAudio = sound.audioClips[Random.Range(0, sound.audioClips.Length)];
        }
        else
        {
            newAudio = sound.audioClips[0];
        }

        Instance._sfx.clip = newAudio;
        Instance._sfx.volume = sound.clipVolume;
        if (Instance._sfx.volume == 0)
        {
            Debug.LogWarning("volume of " + newAudio + " is set to 0!!!");
        }
        Instance._sfx.pitch = sound.pitch + pitchBonus;
        //Instance._audioSource.outputAudioMixerGroup = GetAudioMixer(sound);
        Instance._sfx.PlayOneShot(newAudio, Instance.SoundScaler);
        //_lastSound = sound.soundName;
    }
}
