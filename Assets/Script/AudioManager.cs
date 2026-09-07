using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource audioBG;
    [SerializeField] private AudioSource audioSFX;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip BGMusic;
    [SerializeField] private AudioClip DropSFX;
    [SerializeField] private AudioClip MergeSFX;
    [SerializeField] private AudioClip UiSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioBG.clip = BGMusic;
        audioBG.Play();
    }

    public void PlayMergeSFX()
    {
        audioSFX.PlayOneShot(MergeSFX);
    }

    public void PlayDropSFX()
    {
        audioSFX.PlayOneShot(DropSFX);
    }
}
