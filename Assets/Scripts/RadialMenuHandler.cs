using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RadialMenuHandler : MonoBehaviour
{
    [SerializeField] private List<ElementHolder> elementButtons;
    [SerializeField, Range(0.2f, 1f)] private float RotateDuration;
    [SerializeField] private float scaleFactor = 0.25f;
    private bool ishalfCircle = false;
    private int iconCount = 0;
    private int MiddleIndex = 0;

    #region Unity
    private void OnEnable()
    {
        ElementHolder.OnElementClicked += OnClickedElement;
    }

    private void OnDisable()
    {
        ElementHolder.OnElementClicked -= OnClickedElement;
    }

    private void Start()
    {
        iconCount = elementButtons.Count;
        MiddleIndex = iconCount / 2;
        ReArrangeElements(false);
    }

    #endregion

    #region Public
    public void OnMainButtonClicked()
    {
        ishalfCircle = !ishalfCircle;
        ReArrangeElements();
    }
    #endregion

    #region Private
    private void ReArrangeElements(bool IsAnimated = true)
    {
        float radAngle = (ishalfCircle ? 180 : 360) / (ishalfCircle ? iconCount - 1 : iconCount);

        for (int i = 0; i < iconCount; i++)
        {
            elementButtons[i].Index = i;

            float targetAngle = radAngle * i;

            StartCoroutine(RotateElementSmoothly(elementButtons[i].transform, targetAngle, IsAnimated ? RotateDuration : 0f));

            elementButtons[i].RotateIcon(-targetAngle, IsAnimated ? RotateDuration : 0f, GetScaleFactor(i), ishalfCircle);

            elementButtons[i].ToggleMain(i == MiddleIndex && ishalfCircle);
        }
    }

    private IEnumerator RotateElementSmoothly(Transform target, float targetAngle, float duration)
    {
        Quaternion startRotation = target.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, targetAngle);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            target.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.rotation = endRotation;
    }

    private float GetScaleFactor(int index)
    {
        if (!ishalfCircle)
        {
            return 1f;
        }

        int center = iconCount / 2;
        int distanceFromCenter = Mathf.Abs(index - center);

        return Mathf.Pow(scaleFactor, distanceFromCenter);
    }

    private void ReOrderElements(int ClickedIndex)
    {
        if (ClickedIndex == MiddleIndex) return;

        int shiftAmount = ClickedIndex - MiddleIndex;

        List<ElementHolder> newList = new List<ElementHolder>(elementButtons);

        for (int i = 0; i < iconCount; i++)
        {
            int newIndex = (i - shiftAmount + iconCount) % iconCount;
            elementButtons[newIndex] = newList[i];
        }

        for (int i = 0; i < iconCount; i++)
        {
            elementButtons[i].Index = i;
        }

        ReArrangeElements();
    }
    #endregion

    #region Callbacks
    private void OnClickedElement(int Index)
    {
        Debug.Log("Clicked on element " + Index);
        ReOrderElements(Index);
    }
    #endregion
}