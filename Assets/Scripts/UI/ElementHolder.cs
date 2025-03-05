using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ElementHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Image Icon;
    [SerializeField] private Sprite DefaultSprite;
    [SerializeField] private Sprite HighlightSprite;
    [SerializeField] private Element Type;
    private bool isOpen = false;
    private bool isMain = false;
    public int Index = -1;
    public static event Action<int> OnElementClicked;
    public static event Action<Element> ElementTypeChanged;

    #region Unity
    void Start()
    {
        ToggleSprite(false);
    }
    #endregion

    public void RotateIcon(float Angle, float Time, float scaleFactor, bool Opened)
    {
        isOpen = Opened;
        StartCoroutine(RotateIconRoutine(Angle, Time, scaleFactor));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isMain && isOpen)
        {
            ToggleSprite(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isMain && isOpen)
        {
            ToggleSprite(false);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnElementClicked?.Invoke(Index);
        ElementTypeChanged?.Invoke(Type);
    }

    public void OnDeselect(BaseEventData eventData)
    {

    }

    public void ToggleMain(bool Main)
    {
        isMain = Main;
        ToggleSprite(isMain);
    }

    private IEnumerator RotateIconRoutine(float targetAngle, float duration, float scaleFactor)
    {
        float elapsedTime = 0;
        float startAngle = Icon.transform.localRotation.eulerAngles.z;

        Vector3 startScale = Icon.transform.localScale;
        Vector3 endScale = new Vector3(scaleFactor, scaleFactor, 1f);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            float interpolatedAngle = Mathf.Lerp(startAngle, targetAngle, t);

            Icon.transform.localRotation = Quaternion.Euler(0, 0, interpolatedAngle);
            Icon.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Icon.transform.localRotation = Quaternion.Euler(0, 0, targetAngle);
        Icon.transform.localScale = endScale;
    }

    private void ToggleSprite(bool isHighlight)
    {
        if (isHighlight)
        {
            Icon.sprite = HighlightSprite;
        }
        else
        {
            Icon.sprite = DefaultSprite;
        }
    }
}
