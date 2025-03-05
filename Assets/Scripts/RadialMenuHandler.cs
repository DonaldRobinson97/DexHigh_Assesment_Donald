using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuHandler : MonoBehaviour
{
    [SerializeField] private List<ElementHolder> elementButtons;
    [SerializeField, Range(0.2f, 1f)] private float RotateDuration;
    private bool ishalfCircle = false;
    private int iconCount = 0;

    #region Unity
    private void Start()
    {
        iconCount = elementButtons.Count;
        ReArrangeElements();
    }

    #endregion

    #region Public
    public void OnMainButtonClicked()
    {
        ishalfCircle = !ishalfCircle;

        Debug.Log("The circle is half?" + ishalfCircle);

        ReArrangeElements();
    }
    #endregion

    #region Private
    private void ReArrangeElements()
    {
        float radAngle = (ishalfCircle ? 180 : 360) / (ishalfCircle ? iconCount - 1 : iconCount);

        for (int i = 0; i < iconCount; i++)
        {
            float targetAngle = radAngle * i;
            StartCoroutine(RotateElementSmoothly(elementButtons[i].transform, targetAngle, RotateDuration));
            elementButtons[i].RotateIcon(-targetAngle, RotateDuration);
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

        target.rotation = endRotation; // Ensure it finishes exactly at the target rotation
    }

    #endregion
}