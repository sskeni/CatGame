using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] private AudioClip hoverSoundClip;
    [SerializeField] private float hoverVolume;
    [SerializeField] private float hoverPitchRange;
    [SerializeField] private AudioClip pressedSoundClip;
    [SerializeField] private float pressedVolume;
    [SerializeField] private float pressedPitchRange;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundFXHandler.Instance.PlaySoundFXClip(hoverSoundClip, transform, hoverVolume, hoverPitchRange, hoverPitchRange);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundFXHandler.Instance.PlaySoundFXClip(pressedSoundClip, transform, pressedVolume, pressedPitchRange, pressedPitchRange);
    }

}
