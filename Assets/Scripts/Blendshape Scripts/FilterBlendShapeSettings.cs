
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FilteredBlendShapeSettings", menuName = "ScriptableObjects/BlendShapeScriptableObject",
    order = 1)]

public class FilterBlendShapeSettings : ScriptableObject
{
    [SerializeField] private List<int> AllowedFaceBlendShapesList;
    [SerializeField] private List<int> AllowedNoseBlendShapesList;
    [SerializeField] private List<int> AllowedEyesBlendShapesList;
    [SerializeField] private List<int> AllowedEyeBrowsBlendShapesList;
    [SerializeField] private List<int> AllowedLipsBlendShapesList;

    public bool ContainsIndex(int i)
    {
        return AllowedEyesBlendShapesList.Contains(i) || AllowedFaceBlendShapesList.Contains(i) ||
               AllowedNoseBlendShapesList.Contains(i) || AllowedEyeBrowsBlendShapesList.Contains(i) ||
               AllowedLipsBlendShapesList.Contains(i);
    }

    public List<int> GetIndices(ListType searchKey)
    {

        switch(searchKey)
        {
            case ListType.Eyes:
                return AllowedEyesBlendShapesList;
            case ListType.Nose:
                return AllowedNoseBlendShapesList ;
            case ListType.Eyebrows:
                return AllowedEyeBrowsBlendShapesList;
            case ListType.Lips:
                return AllowedLipsBlendShapesList;
            case ListType.Face:
                return AllowedFaceBlendShapesList;

        }
        return null;
    }

    public enum ListType
    {
        Eyes,
        Nose,
        Lips,
        Eyebrows,
        Face
    }
}
