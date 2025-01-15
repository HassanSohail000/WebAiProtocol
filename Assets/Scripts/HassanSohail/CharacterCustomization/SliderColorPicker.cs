using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
public class SliderColorPicker : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image output;
    [SerializeField] TMP_Text outputTxt;
    CharacterBodyParts bodyParts;
    Button saveBtn;
    bool itemAlreadySaved = false;
    public void Start()
    {
        bodyParts = GameManager.Instance.mainCharacter.GetComponent<CharacterBodyParts>();
        saveBtn = InventoryManager.instance.saveButton.GetComponent<Button>();
        Int();
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
    void Int()
    {
        UpdateColorInTextBox();
       
    }
    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        if (/*!UserPassManager.Instance.CheckSpecificItem("")*/false)
        {
            //UserPassManager.Instance.PremiumUserUI.SetActive(true);
            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");
            itemAlreadySaved = false;
            Color tempColor = Color.HSVToRGB(slider.value, 1, 1);
            bodyParts.ChangeSkinColor(tempColor);
            UpdateColorInTextBox();
            CheckIsColorChange();
        }
    }
    string ConvertColorToHex(Color color)
    {
        return ColorUtility.ToHtmlStringRGBA(color);
    }
    /// <summary>
    /// To enable Save button if color change's on slider
    /// </summary>
    void CheckIsColorChange()
    {
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
            if (_CharacterData.Skin != bodyParts.GetBodyColor()) //
            {
                saveBtn.interactable = true;
                InventoryManager.instance.SaveStoreBtn.SetActive(true);
                InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
                InventoryManager.instance.GreyRibbonImage.SetActive(false);
                InventoryManager.instance.WhiteRibbonImage.SetActive(true);
            }
            else
            {
                InventoryManager.instance.SaveStoreBtn.SetActive(true);
                InventoryManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = false;
                InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;
                InventoryManager.instance.GreyRibbonImage.SetActive(true);
                InventoryManager.instance.WhiteRibbonImage.SetActive(false);
            }
        }
    }
   
    /// <summary>
    /// Change Textbox data according to selected item
    /// </summary>
    public void UpdateColorInTextBox()
    {
        print("Color is : " + bodyParts.GetBodyColor());
        outputTxt.text = ConvertColorToHex(bodyParts.GetBodyColor());
        output.color = bodyParts.GetBodyColor();
    }
}