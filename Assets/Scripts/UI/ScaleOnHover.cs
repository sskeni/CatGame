using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public float hoverScale;
    public float scaleDuration;
    public bool canClick = false;

    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = initialScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LerpScale(initialScale * hoverScale, scaleDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LerpScale(initialScale, scaleDuration);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canClick)
        {
            LerpScale(initialScale, scaleDuration);
        }
    }

    // Lerps the transform to a scale over a duration
    private void LerpScale(Vector3 scale, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(LerpScaleHelper(scale, duration));
    }

    // Scale coroutine helper
    private IEnumerator LerpScaleHelper(Vector3 scale, float duration)
    {
        float time = 0;
        Vector3 startScale = transform.localScale;

        while (time < duration)
        {
            transform.localScale = Vector3.Slerp(startScale, scale, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localScale = scale;
    }

}
