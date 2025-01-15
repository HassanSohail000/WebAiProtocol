using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SubBottons : MonoBehaviour
{
    public GameObject[] TotalBtns;
    public Color NormalColor;
    public Color HighlightedColor;
    public bool ClothBool;
    public bool AvatarBool;

    private AvatarCustomizationUIHandler customizationUIManager;

    // Start is called before the first frame update
    void Start()
    {
        customizationUIManager = FindObjectOfType<AvatarCustomizationUIHandler>();
    }

    private int currentSelectedCategoryIndex;

    public void ClickBtnFtn(int m_Index)
    {
        //if(customizationUIManager == null)
        //    customizationUIManager = CharacterCustomizationUIManager.Instance;

        for (int i = 0; i < TotalBtns.Length; i++)
        {
            TotalBtns[i].GetComponentInChildren<ButtonScript>().BtnTxt.color = NormalColor;
            TotalBtns[i].transform.GetChild(0).GetComponent<Text>().fontStyle = FontStyle.Normal;
        }

        TotalBtns[m_Index].GetComponentInChildren<ButtonScript>().BtnTxt.color = HighlightedColor;
        TotalBtns[m_Index].transform.GetChild(0).GetComponent<Text>().fontStyle = FontStyle.Bold;


        if (ClothBool)
        {
            InventoryManager.instance.OpenClothContainerPanel(m_Index);
            if (m_Index == 2)
            {
                AvatarCustomizationUIHandler.Instance.LoadMyFaceCustomizationPanel();
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(1);
            }
            else
            {
                AvatarCustomizationUIHandler.Instance.LoadMyClothCustomizationPanel();
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(2);
            }
        }
        else if (AvatarBool)
        {
            InventoryManager.instance.OpenAvatarContainerPanel(m_Index);
            currentSelectedCategoryIndex = m_Index;

            if (m_Index == 10 || m_Index == 6)
            {
                AvatarCustomizationUIHandler.Instance.LoadMyClothCustomizationPanel();
            }
            else
            {
                AvatarCustomizationUIHandler.Instance.LoadMyFaceCustomizationPanel();
            }
        }
        // print(m_Index);
    }


}
