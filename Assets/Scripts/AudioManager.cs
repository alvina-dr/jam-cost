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

    public void PlaySFXSound(AudioClip clip)
    {
        _sfx.PlayOneShot(clip);
    }
}
