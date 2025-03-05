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
    }
    #endregion

    #region Private
    private void ReArrangeElements()
    {
        float radAngle = 360 / iconCount;

        for (int i = 0; i < iconCount; i++)
        {
            float angle = radAngle * i;
            elementButtons[i].transform.rotation = Quaternion.Euler(0, 0, angle);
            elementButtons[i].RotateIcon(-angle, RotateDuration);
        }
    }
    #endregion
}