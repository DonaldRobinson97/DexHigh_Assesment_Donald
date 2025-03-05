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
        
        elementButtons[MiddleIndex].OnSelect(null);
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
        int stepCount = Mathf.Abs(shiftAmount);
        bool isClockwise = shiftAmount < 0;

        StartCoroutine(PerformStepwiseRotation(isClockwise, stepCount));
    }

    private IEnumerator PerformStepwiseRotation(bool isClockwise, int steps)
    {
        for (int step = 0; step < steps; step++)
        {
            PerformSingleShift(isClockwise);

            yield return new WaitForSeconds(RotateDuration);
        }
    }

    private void PerformSingleShift(bool isClockwise)
    {
        if (isClockwise)
        {
            ElementHolder lastElement = elementButtons[elementButtons.Count - 1];
            elementButtons.RemoveAt(elementButtons.Count - 1);
            elementButtons.Insert(0, lastElement);
        }
        else
        {
            ElementHolder firstElement = elementButtons[0];
            elementButtons.RemoveAt(0);
            elementButtons.Add(firstElement);
        }

        for (int i = 0; i < elementButtons.Count; i++)
        {
            elementButtons[i].Index = i;
        }

        ReArrangeElements(true);
    }

    #endregion

    #region Callbacks
    private void OnClickedElement(int Index)
    {
        ReOrderElements(Index);
    }
    #endregion
}