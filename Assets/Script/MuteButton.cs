using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [SerializeField] AudioSource SFXAudioSource;
    [SerializeField] AudioSource BGAudioSource;

    [SerializeField] Image MuteSprite;

    [SerializeField] Sprite OnMuteSprite;
    [SerializeField] Sprite NormalSprite;

    [SerializeField] bool isMuted;

    public void Mute()
    {
        if(!isMuted)
        {
            isMuted = true;
            MuteSprite.sprite = OnMuteSprite;
            BGAudioSource.volume = 0;
            SFXAudioSource.volume = 0;
        }
        else
        {
            isMuted = false;
            MuteSprite.sprite = NormalSprite;
            BGAudioSource.volume = 1;
            SFXAudioSource.volume = 0.4f;
        }
    }
}
