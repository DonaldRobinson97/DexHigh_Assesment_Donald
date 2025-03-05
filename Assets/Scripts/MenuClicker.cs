using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuClicker : MonoBehaviour
{
    [SerializeField] private Image MenuPanel;
    [SerializeField] private RectTransform ButtonTransform;
    [SerializeField] private RectTransform EndPoint;
    [SerializeField, Range(0.1f, 0.5f)] private float toggleDuration = 0.15f;
    [SerializeField] private Vector3 EndScale;
    private Vector2 OrginalPosition;
    private Vector3 OriginalScale;
    private bool isTransitioning = false;
    private bool isOpen = false;

    #region Unity
    private void OnEnable()
    {
        OrginalPosition = ButtonTransform.position;
        OriginalScale = this.transform.localScale;
        MenuPanel.color = new Color(1, 1, 1, 0);
    }
    #endregion

    public void OpenPanelButton()
    {
        if (isTransitioning)
            return;

        TogglePanel();
    }

    #region Private
    private void TogglePanel()
    {
        StartCoroutine(PanelToggleRoutine());
    }

    private IEnumerator PanelToggleRoutine()
    {
        isTransitioning = true;
        isOpen = !isOpen;

        Vector2 endPos;
        Vector3 endScale;
        float endColor;
        float t;

        float elapsedTime = 0f;

        while (elapsedTime < toggleDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / toggleDuration;

            if (isOpen)
            {
                endPos = Vector3.Lerp(OrginalPosition, EndPoint.position, t);
                endScale = Vector3.Lerp(OriginalScale, EndScale, t);

                endColor = Mathf.Lerp(0, 1, t);
            }
            else
            {
                endPos = Vector3.Lerp(EndPoint.position, OrginalPosition, t);
                endScale = Vector3.Lerp(EndScale, OriginalScale, t);

                endColor = Mathf.Lerp(1, 0, t);
            }

            ButtonTransform.position = endPos;
            this.transform.localScale = endScale;

            MenuPanel.color = new Color(1, 1, 1, endColor);

            yield return null;
        }

        isTransitioning = false;
    }
    #endregion
}