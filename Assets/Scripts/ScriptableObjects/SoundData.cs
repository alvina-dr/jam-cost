using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    public AudioClip[] audioClips;
    [Range(0, 3)] public float clipVolume;
    [Range(0, 3)] public float pitch;
    public float PitchBonus;
    public AudioMixerGroup Output;
}
