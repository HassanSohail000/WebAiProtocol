using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeHolder : MonoBehaviour
{
    public static BlendShapeHolder instance;

    public SkinnedMeshRenderer blendHolder;
    public List<BlendShapeDataClass> allBlendShapes;

    int selectedBlendShapeIndex = -1;
    int selectedItemArrayIndex = -1;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public int GetBlendShapeIndex(MorphPoints blendShapeName)
    {
        foreach (var item in allBlendShapes)
        {
            if (item.blendShapeName == blendShapeName)
            {
                return selectedBlendShapeIndex = item.index;
            }
        }

        // Not Match
        return selectedBlendShapeIndex;
    }
    public int GetArrayIndex(MorphPoints blendShapeName)
    {
        foreach (var item in allBlendShapes)
        {
            if (item.blendShapeName == blendShapeName)
            {
                return selectedItemArrayIndex = allBlendShapes.IndexOf(item);
            }
        }

        // Not Match
        return selectedBlendShapeIndex;
    }

    // Call From Sliders
    public void SetBlendValue(int value)
    {
        if (selectedBlendShapeIndex == -1 || selectedBlendShapeIndex >= blendHolder.sharedMesh.blendShapeCount)
            return; // blendShape Selecded
        else
        {
            blendHolder.SetBlendShapeWeight(selectedBlendShapeIndex, value);
        }
    }
    public void SetBoneData(ObjectProperty objectProperty, Vector3 newValue)
    {
        if (selectedBlendShapeIndex != -1 || selectedBlendShapeIndex >= allBlendShapes.Count)
            return;


        Transform bone = null;
        if (CharacterHandler.instance.activePlayerGender == AvatarGender.Male)
        {
            bone = allBlendShapes[selectedBlendShapeIndex].maleBoneObj.transform;
        }
        else
        {
            bone = allBlendShapes[selectedBlendShapeIndex].femaleBoneObj.transform;
        }

        switch (objectProperty)
        {
            case ObjectProperty.position:
                bone.localPosition = newValue;
                break;

            case ObjectProperty.rotation:
                bone.localEulerAngles = newValue;
                break;

            case ObjectProperty.scale:
                bone.localScale = newValue;
                break;

            default:
                break;
        }
    }


    public void ResetEyeBrowBlendValues()
    {
        // EyeBrowBlendIndex = 16 17 18 19 21 22

        if (blendHolder == null)
            return;

        blendHolder.SetBlendShapeWeight(16, 0);
        blendHolder.SetBlendShapeWeight(17, 0);
        blendHolder.SetBlendShapeWeight(18, 0);
        blendHolder.SetBlendShapeWeight(19, 0);
        blendHolder.SetBlendShapeWeight(21, 0);
        blendHolder.SetBlendShapeWeight(22, 0);
    }


    //public bool updateName = false;
    //private void OnValidate()
    //{
    //    if (updateName)
    //    {
    //        updateName = false;
    //        foreach (var item in allBlendShapes)
    //        {
    //            if (!item.boneAvailable)
    //            {
    //                item.itemName = allBlendShapes.IndexOf(item)+ "." + item.blendShapeName.ToString() + " - Blend Ind :" + item.index;
    //            }
    //            else
    //            {
    //                for (int i = 0; i < item.boneData.Count; i++)
    //                {
    //                    item.boneData[i].boneName = item.boneData[i].workingTransform.ToString().ToUpper();
    //                }
    //            }
    //            //if (item.blendShapeName.ToString().Contains("Bone"))
    //            //    item.itemName = item.blendShapeName.ToString();
    //            //else
    //            //    item.itemName = item.blendShapeName.ToString() + " - Blend Ind :" + item.index;
    //        }
    //    }

    //}
}
public enum ObjectProperty { position, rotation, scale }