using System.Collections;
using UnityEngine;

public class ElementHolder : MonoBehaviour
{
    [SerializeField] private Transform Icon;

    public void RotateIcon(float Angle, float Time)
    {
        Icon.transform.localRotation = Quaternion.Euler(0, 0, Angle);

        // StartCoroutine(RotateIconRoutine(Angle, Time));
    }

    private IEnumerator RotateIconRoutine(float Angle, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float interpolatedAngle = Mathf.Lerp(0, Angle, t);
            Icon.transform.localRotation = Quaternion.Euler(0, 0, interpolatedAngle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
