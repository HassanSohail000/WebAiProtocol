using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BodyTextures
{
    [Header("Texture Maps")]
    public Texture2D baseColor;
    //public Texture2D heightMap;
    //public Texture2D metallicMap;
    //public Texture2D normalMap;
    //public Texture2D roughnessMap;

    [Header("Icon")]
    public Sprite attributeIcon;
}

public class BodyColorCustomization : MonoBehaviour
{
    public CharacterBodyParts characterBodyParts;
    public Renderer head;
    public Renderer[] eyeball;

    public List<BodyTextures> bodyTextures;
    public List<BodyTextures> lipsTextures;
    public List<BodyTextures> makeupTextures;

    public Texture[] eyeBallTextures;
    public Sprite[] eyeBallSprites;

    private void Awake()
    {
    }

    public void SkinTones(int index)
    {
        // Commented By Talha Now use texture for Body
        //foreach (GameObject _renderer in characterBodyParts.m_BodyParts)
        //{
        //    _renderer.GetComponent<Renderer>().material.mainTexture = bodyTextures[index].baseColor;
        //}

        //characterBodyParts.m_BodyParts[0].GetComponent<SkinnedMeshRenderer>().sharedMaterial.mainTexture = skinTextures[index];
    }

    public void ChangeLipsTextures(int index)
    {
        head.materials[1].mainTexture = lipsTextures[index].baseColor;
    }

    public void ChangeFaceTexture(int index)
    {
        head.materials[0].mainTexture = makeupTextures[index].baseColor;
    }

    public void EyeBallTextures(int index)
    {
        foreach (Renderer _mat in eyeball)
        { 
            _mat.material.mainTexture = eyeBallTextures[index];
        }

    }
}
