using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NFTColorCodes/ColorCodeAgainstColorName",menuName ="NFTColorCodes")]
public class NFTColorCodes : ScriptableObject
{
    public List<ColorCode> colorCodes;


    [System.Serializable]
    public class ColorCode
    {
        public string colorName;
        public Color colorCode;
        public Color updatedColor;
        //public Color Testcolor;
        //public Color Haircolor;
        public LightPresetNFT LightPresetNFT;
    }
}
