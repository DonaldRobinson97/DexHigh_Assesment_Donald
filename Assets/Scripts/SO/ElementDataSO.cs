using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementDataSO", menuName = "ElementData", order = 0)]
public class ElementDataSO : ScriptableObject
{
    public ElementData[] Elements;

    public ElementData GetElementData(Element type)
    {
        foreach (var element in Elements)
        {
            if (element.Type == type)
            {
                return element;
            }
        }
        return null;
    }
}

[Serializable]
public class ElementData
{
    public Element Type;
    [Range(0, 6)] public int Value;
    public Sprite sprite;
}