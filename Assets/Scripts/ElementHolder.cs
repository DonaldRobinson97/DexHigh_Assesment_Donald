using System.Collections;
using UnityEngine;

public class ElementHolder : MonoBehaviour
{
    [SerializeField] private Transform Icon;

    public void RotateIcon(float Angle, float Time, float scaleFactor)
    {
        StartCoroutine(RotateIconRoutine(Angle, Time, scaleFactor));
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
}
