using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using static InventoryManager;

public class SilderColorPicker : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image output;
    [SerializeField] TMP_Text outputTxt;
    [SerializeField] Image SliderBackground;
   
    CharacterBodyParts bodyParts;
    Button saveBtn;
    bool itemAlreadySaved = false;


    private Color currColor;
    private float hue;
    private float saturation;
    private float brightness;
    private Texture2D _cachedTexture;
    private float _silderBackgroundWidth;
    private int _silderBackgroundHeight;


    public SliderType sliderCategory;

    void Start()
    {
        if (SliderBackground != null)
        {
            _cachedTexture = SliderBackground.sprite.texture;
            _silderBackgroundWidth = (_cachedTexture.width - 1);
            _silderBackgroundHeight = (_cachedTexture.height / 2);
        }
    }

    public void Awake()
    {

        //bodyParts = GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>();
        saveBtn = InventoryManager.instance.saveButton.GetComponent<Button>();

        //Int();
        //slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });


        //Debug.Log("color slider awake");
    }

    private void OnEnable()
    {
        bodyParts = GameManager.Instance.mainCharacter.GetComponent<CharacterBodyParts>();

        SetRelatedData();
        if (sliderCategory.Equals(SliderType.Skin))
            CharacterBodyParts.OnSkinColorApply += ChangeSliderColor;
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        if (saveBtn.interactable)
            isSaveBtnEnable = true;

        SaveCurrentColor();
    }


    private void OnDisable()
    {
        if (sliderCategory.Equals(SliderType.Skin))
            CharacterBodyParts.OnSkinColorApply -= ChangeSliderColor;
        slider.onValueChanged.RemoveAllListeners();
        isSaveBtnEnable = false;
    }


    void Int()
    {
        ConvertColorToHex(bodyParts.GetBodyColor());
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {

        if (/*!UserPassManager.Instance.CheckSpecificItem(sliderCategory.ToString())*/ false)
        {
            //UserPassManager.Instance.PremiumUserUI.SetActive(true);

            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");

            itemAlreadySaved = false;
            //currColor = bodyParts.GetBodyColor();
            currColor = GetCurrentColor();

            // Making BaseColorLight, so color match with slider
            {
                if (sliderCategory != SliderType.Skin)
                {
                    currColor = new Color(0.7647f, 0.7019f, 0.5098f, 1.000f);
                }
            }


            Color tempColor;
            Color.RGBToHSV(currColor, out hue, out saturation, out brightness);
            tempColor =OnColorSliderChange(slider.value); //Color.HSVToRGB(slider.value, saturation, brightness);
            output.color = Color.HSVToRGB(slider.value, saturation + .2f, brightness);

            //CharcterBodyParts.instance.ChangeSkinColor(tempColor);

            ChangeColor(tempColor);

            //print("Color is : " + bodyParts.GetBodyColor());
            //outputTxt.text = ConvertColorToHex(bodyParts.GetBodyColor());

            //print("Color is : " + tempColor);
            outputTxt.text = ConvertColorToHex(tempColor);
            CheckIsColorChange();
        }
    }

    //static bool justOneTime = true;
    private Color initColor;       // AR changes
    void ChangeSliderColor(Color m_color)
    {
        initColor = m_color;       // AR changes

        Color.RGBToHSV(m_color, out hue, out saturation, out brightness);
        slider.value = hue;
        //Debug.Log("Change slider: " + sliderCategory);
    }


    Color OnColorSliderChange(float value)
    {
        if (_cachedTexture != null)
        {
            int x = Mathf.RoundToInt(value * _silderBackgroundWidth);
            Color pickedColor = _cachedTexture.GetPixel(x, _silderBackgroundHeight );
            return pickedColor;
        }
        return Color.white;
    }

    string ConvertColorToHex(Color color)
    {
        return ColorUtility.ToHtmlStringRGBA(color);
    }


    /// <summary>
    /// To enable Save button if color change's on slider
    /// </summary>


    bool isSaveBtnEnable = false;
    void CheckIsColorChange()
    {
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            bool valueChanged = false;

            switch (sliderCategory)
            {
                case SliderType.Skin:
                    if (_CharacterData.Skin != bodyParts.GetBodyColor())
                        valueChanged = true;
                    break;

                case SliderType.HairColor:
                    if (_CharacterData.HairColor != bodyParts.GetHairColor())
                        valueChanged = true;
                    break;

                case SliderType.EyeBrowColor:
                    if (_CharacterData.EyebrowColor != bodyParts.GetEyebrowColor())
                        valueChanged = true;
                    break;

                case SliderType.EyesColor:
                    if (_CharacterData.EyeColor != bodyParts.GetEyeColor())
                        valueChanged = true;
                    break;

                case SliderType.LipsColor:
                    if (_CharacterData.LipColor != bodyParts.GetLipColor())
                        valueChanged = true;
                    break;

                default:
                    break;
            }

            if (valueChanged)
                SaveBtnEnableDisable(true);
            else if (!isSaveBtnEnable)
                SaveBtnEnableDisable(false);
        }
    }

    void SaveBtnEnableDisable(bool _status)
    {
        InventoryManager.instance.SaveStoreBtn.SetActive(true);

        if (_status)
        {
            saveBtn.interactable = true;
            InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
            InventoryManager.instance.GreyRibbonImage.SetActive(false);
            InventoryManager.instance.WhiteRibbonImage.SetActive(true);
        }
        else
        {
            saveBtn.interactable = false;
            InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;
            InventoryManager.instance.GreyRibbonImage.SetActive(true);
            InventoryManager.instance.WhiteRibbonImage.SetActive(false);
        }
    }

    void ChangeSaveBtnStatus(bool _status)
    {
        if (_status) //
        {
            saveBtn.interactable = true;

            InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
            InventoryManager.instance.GreyRibbonImage.SetActive(false);
            InventoryManager.instance.WhiteRibbonImage.SetActive(true);
        }
        else
        {
            InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;
            InventoryManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = false;
            InventoryManager.instance.GreyRibbonImage.SetActive(true);
            InventoryManager.instance.WhiteRibbonImage.SetActive(false);
        }
        InventoryManager.instance.SaveStoreBtn.SetActive(true);
    }


    void SetRelatedData()
    {
        switch (sliderCategory)
        {
            case SliderType.Skin:
                ChangeSliderColor(bodyParts.GetBodyColor());
                outputTxt.text = ConvertColorToHex(bodyParts.GetBodyColor());
                break;

            case SliderType.HairColor:
                ChangeSliderColor(bodyParts.GetHairColor());
                outputTxt.text = ConvertColorToHex(bodyParts.GetHairColor());
                break;

            case SliderType.EyeBrowColor:
                ChangeSliderColor(bodyParts.GetEyebrowColor());
                outputTxt.text = ConvertColorToHex(bodyParts.GetEyebrowColor());
                break;

            case SliderType.EyesColor:
                ChangeSliderColor(bodyParts.GetEyeColor());
                outputTxt.text = ConvertColorToHex(bodyParts.GetEyeColor());
                break;

            case SliderType.LipsColor:
                ChangeSliderColor(bodyParts.GetLipColor());
                outputTxt.text = ConvertColorToHex(bodyParts.GetLipColor());
                break;

            default:
                break;
        }
    }

    Color previousColor; // For undoRedo Perpose
    private Color currentColor;   // AR changes

    void ChangeColor(Color _color)
    {
        //  Debug.Log("Change color call: " + _color);
        if (!StoreUndoRedo.obj.addToList)  //  UndoRedo.undoRedo.back == true
        {
            ChangeSliderColor(_color);
            output.color = Color.HSVToRGB(slider.value, saturation + .2f, brightness);
            outputTxt.text = ConvertColorToHex(_color);
            //UndoRedo.undoRedo.back = false;
            StoreUndoRedo.obj.addToList = true;
            Debug.Log("<color=red> Undo Redo back forcelly false </color>");
        }
        else
        {
            previousColor = GetCurrentColor();
        }
        switch (sliderCategory)
        {
            case SliderType.Skin:
                bodyParts.ChangeSkinColor(_color);
                break;

            case SliderType.HairColor:
                Debug.Log("Hair color");
                bodyParts.ChangeHairColor(_color);
                //InventoryManager.instance.itemData.hair_color = _color;
                break;

            case SliderType.EyeBrowColor:
                bodyParts.ChangeEyebrowColor(_color);
                break;

            case SliderType.EyesColor:
                bodyParts.ChangeEyeColor(_color);
                break;

            case SliderType.LipsColor:
                bodyParts.ChangeLipColor(_color);
                break;

            default:
                break;

        }
    }
    Color GetCurrentColor()
    {
        Color tempColor = Color.white;
        switch (sliderCategory)
        {
            case SliderType.Skin:
                tempColor = bodyParts.GetBodyColor();
                break;

            case SliderType.HairColor:
                tempColor = bodyParts.GetHairColor();
                break;

            case SliderType.EyeBrowColor:
                tempColor = bodyParts.GetEyebrowColor();
                break;

            case SliderType.EyesColor:
                tempColor = bodyParts.GetEyeColor();
                break;

            case SliderType.LipsColor:
                tempColor = bodyParts.GetLipColor();
                break;
                //default:
                //    return Color.white;
                //    break;
        }
        return tempColor;
    }

    public void CustomMouseStatus(bool isMouseUp)
    {
        if (isMouseUp)
        {
            Debug.Log("<color=red>Slider Up</color>");
            if (!StoreUndoRedo.obj.addToList)
                StoreUndoRedo.obj.addToList = true;
            else
            {
                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ChangeColor", StoreUndoRedo.ActionType.ChangeColorBySlider, previousColor, EnumClass.CategoryEnum.SliderColor);
                Debug.Log("<color=red> Set Default Hair </color>");
            }
            currentColor = previousColor;
        }
        else
        {
            SetPreviousColorValueForUndoRedo(currentColor);   // AR changes cistom method
            Debug.Log("<color=red>Slider Down</color>");
        }
    }

    private void SaveCurrentColor()
    {
        switch (sliderCategory)
        {
            case SliderType.Skin:
                currentColor = bodyParts.GetBodyColor();
                break;

            case SliderType.HairColor:
                currentColor = bodyParts.GetHairColor();
                break;

            case SliderType.EyeBrowColor:
                currentColor = bodyParts.GetEyebrowColor();
                break;

            case SliderType.EyesColor:
                Color tempColor = bodyParts.GetEyeColor();
                if (tempColor == Color.white)
                    ColorUtility.TryParseHtmlString("#D9D9D9", out tempColor);
                currentColor = tempColor;
                break;

            case SliderType.LipsColor:
                currentColor = bodyParts.GetLipColor();
                break;

            default:
                break;
        }
    }
    void SetPreviousColorValueForUndoRedo(Color colorParam)
    {
        if (!StoreUndoRedo.obj.addToList)
            StoreUndoRedo.obj.addToList = true;
        else
        {
            StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ChangeColor", StoreUndoRedo.ActionType.ChangeColorBySlider, colorParam, EnumClass.CategoryEnum.SliderColor);
            Debug.Log("<color=red> Set Default Hair </color>");
        }
    }


}

public enum SliderType
{
    Skin, HairColor, EyeBrowColor, EyesColor, LipsColor
}
