using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlendShapeDataClass
{
    public string itemName;
    public AxisType AxisType;
    public MorphPoints blendShapeName = MorphPoints.none;
    public int index;
    [Header("Blend Min Max Values")]
    public float minValue;
    public float maxValue;
    public bool reverseMyValue = false; // Has Sibling blend Shape If my value increase than sibling value decrease

    [Header("Bone Releated Fields")]
    public bool boneAvailable;
    //bones to Customise
    public GameObject maleBoneObj;  
    public GameObject femaleBoneObj;
    public List<BoneData> boneData;
}
[System.Serializable]
public enum MorphPoints { none, Face, Eyes, HeadBone, Lips, EyeBrow, Nose}

[System.Serializable]
public enum TransformDetails { none, scale, rotation, postion }

[System.Serializable]
public enum AxesDetails { none, x, y, z ,xy }

[System.Serializable]
public class BoneData
{
    public string boneName;
    public TransformDetails workingTransform;
    public AxesDetails workingAxes;
    public float minValue, maxValue;
    public bool reverseMyvalue = false;
}