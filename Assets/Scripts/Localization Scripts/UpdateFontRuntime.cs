using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateFontRuntime : MonoBehaviour
{
    public TMP_Text TextForRef;
    public float FontSize;
    public float CharacterSpacing;
    public float LineSpacing;
    public float NewLeftMargin;
    public float NewTopMargin;
    // Start is called before the first frame update
    void Awake()
    {
        TextForRef = GetComponent<TextMeshProUGUI>();
        FontLoadOnRuntime();
    }

    // Update is called once per frame
    public void FontLoadOnRuntime()
    {
        if (LocalizationManager.forceJapanese || GameManager.currentLanguage == "ja")
        {
            TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>("Fonts/JP R");
            TextForRef.font = fontAsset;
            TextForRef.fontSize = FontSize;
            TextForRef.characterSpacing = CharacterSpacing;
            TextForRef.lineSpacing = LineSpacing;

            // Modify the left margin
            Vector4 currentMargin = TextForRef.margin;
            currentMargin.x = NewLeftMargin;
            TextForRef.margin = currentMargin;
            // Modify the Top margin
            currentMargin.y = NewTopMargin;
            TextForRef.margin = currentMargin;
            


        }
    }
}
