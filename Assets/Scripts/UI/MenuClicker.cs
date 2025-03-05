using System.Collections;
using System.Collections.Generic;
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

    [Header("Opened Panel")]
    [SerializeField] private List<Image> Powers;
    [SerializeField] private Sprite UnlockedSprite;
    [SerializeField] private Sprite LockedSprite;
    [SerializeField] private Image BG;
    [SerializeField] private ElementDataSO DataSO;

    #region Unity
    private void OnEnable()
    {
        OrginalPosition = ButtonTransform.position;
        OriginalScale = this.transform.localScale;
        MenuPanel.color = new Color(1, 1, 1, 0);

        MenuPanel.transform.localScale = Vector3.zero;

        ElementHolder.ElementTypeChanged += OnClickedElement;
    }

    private void OnDisable()
    {
        ElementHolder.ElementTypeChanged -= OnClickedElement;

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
        Vector3 PanelScale;
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

                PanelScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

                endColor = Mathf.Lerp(0, 1, t);
            }
            else
            {
                endPos = Vector3.Lerp(EndPoint.position, OrginalPosition, t);
                endScale = Vector3.Lerp(EndScale, OriginalScale, t);

                PanelScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);

                endColor = Mathf.Lerp(1, 0, t);
            }

            ButtonTransform.position = endPos;
            this.transform.localScale = endScale;

            MenuPanel.transform.localScale = PanelScale;

            BG.color = new Color(1, 1, 1, endColor * 0.25f);

            MenuPanel.color = new Color(1, 1, 1, endColor);

            yield return null;
        }

        isTransitioning = false;
    }

    #endregion

    #region Callbacks
    private void OnClickedElement(Element Type)
    {
        ElementData data = DataSO.GetElementData(Type);

        if (data != null)
        {
            BG.sprite = data.sprite;

            for (int i = 0; i < Powers.Count; i++)
            {
                if (i < data.Value)
                {
                    Powers[i].sprite = UnlockedSprite;
                }
                else
                {
                    Powers[i].sprite = LockedSprite;
                }
            }
        }
    }
    #endregion
}