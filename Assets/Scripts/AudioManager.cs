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

    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioSource _clockSound;

    public void StartClockSound()
    {
        _clockSound.Play();
    }

    public void StopClockSound()
    {
        _clockSound.Stop();
    }

    public void PlaySFXSound(AudioClip clip)
    {
        _sfx.PlayOneShot(clip);
    }
}
