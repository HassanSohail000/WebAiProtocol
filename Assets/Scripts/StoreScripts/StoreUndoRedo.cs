using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InventoryManager;

public class StoreUndoRedo : MonoBehaviour
{
    public static StoreUndoRedo obj;
    [Serializable]
    public class Data
    {
        public string name;
        public GameObject actionObject;
        public int parameter;
        public string methodName;
        public Color colorParam;
        public enum ObjectType { ChangeCategory, ChangePanel, ChangeItem, ChangeColor, ChangeColorBySlider };
        public ObjectType objectType;

        public enum CategoryType
        {
            Head, Face, Inner, Outer, Accesary, Bottom, Socks, Shoes, HairAvatar, HairAvatarColor, LipsAvatar,
            LipsAvatarColor, EyesAvatar, EyesAvatarColor, SkinToneAvatar, Presets, EyeBrowAvatar, EyeBrowAvatarColor,
            EyeLashesAvatar, Nose, Body, Makeup, Avatar, Wearable, SliderColor, AvatarBtns, WearableBtns, None
        }
        public CategoryType category;
    }

    [HideInInspector]
    public enum ActionType { ChangeCategory, ChangePanel, ChangeItem, ChangeColor, ChangeColorBySlider };
    public bool addToList = true;   // not add data into list when call function of selected gameobject for undo redo
    public Button undoBtn, redoBtn;
    [Space(5)]
    public Button[] avatar_Wearable_Btns;
    public enum PanelType { Avatar, Wearable };
    [Space(5)]
    public PanelType panelType;

    public int headIndex = -1;              // for testing purpose it is public otherwise it should private
    public int previousInd;                 // for testing purpose it is public otherwise it should private
    private const int defaultListSize = 2;    // some object add in list at start so default list size necessary to mention.
    private GameObject tempActionObject;
    private string tempMethodName;
    private int tempParam;
    private Color tempColor;
    private bool isDequeueData;
    private int difference;
    [Space(5)]
    public List<Data> data;

    public enum ButtonState { Undo, Redo ,none};
    public ButtonState currentButtonState = ButtonState.none;
    public bool calledOneTime = false;
    void Awake()
    {
        if (obj == null)
            obj = this;
        else if (obj != null)
            Destroy(this);

        data = new List<Data>();
    }

    private void Start()
    {
        undoBtn.onClick.AddListener(NextPressed);
        redoBtn.onClick.AddListener(BackPressed);
    }

    public void DestroyList()
    {
        data = null;
        headIndex = -1;
        previousInd = 0;
        difference = 0;
        isDequeueData = false;
        data = new List<Data>();
    }

    private void CustomDeleteList(int steps)
    {
        isDequeueData = false;
        int num = data.Count - 1;                 // Start del the list from end
        for (int i = 0; i <= steps && data.Count - 1 > defaultListSize; i++)
        {
            //Debug.Log("<color=blue> Dequeue Data: " + i + "</color>");
            data.RemoveAt(num);
            num--;
        }
    }

    public void ActionWithParametersAdd(GameObject dataObject, int paramInt, string method, ActionType actionEnum, Color colorParam, EnumClass.CategoryEnum categoryType)
    {
        if (!addToList) return;

        difference = (data.Count - 1) - headIndex;
        if (isDequeueData)
            CustomDeleteList(difference);

        int num = Array.IndexOf(Enum.GetValues(typeof(ActionType)), actionEnum);
        int num2 = Array.IndexOf(Enum.GetValues(typeof(EnumClass.CategoryEnum)), categoryType);

        data.Add(new Data
        {
            name = dataObject.name,
            actionObject = dataObject,
            parameter = paramInt,
            methodName = method,
            objectType = (Data.ObjectType)(int)num,
            colorParam = colorParam,
            category = (Data.CategoryType)(int)num2
        });

        headIndex++;
    }

    private bool performRedoAction = true;
    private void BackPressed()
    {
        if (!performRedoAction ) return;
        ReduActionPerform();
    }
    void ReduActionPerform()
    {
        performRedoAction = false;

        if (data.Count > defaultListSize)  // && headIndex >= defaultListSize
        {
            previousInd = headIndex - 1;

            StartCoroutine(SendMessage(data[previousInd].actionObject, data[previousInd].parameter, data[previousInd].methodName,
                    data[previousInd].colorParam, data[previousInd].objectType, data[previousInd].category));

            headIndex--;
            if (headIndex < 1)
                headIndex = 1;

            isDequeueData = true;
        }
        StartCoroutine(EnableBtnAction());

        if(currentButtonState == ButtonState.Redo && !calledOneTime)
        {
            headIndex++;
            calledOneTime = true;
        }
        if (currentButtonState != ButtonState.Redo)
        {
            currentButtonState = ButtonState.Redo;
            ReduActionPerform();
        }
    }

    IEnumerator EnableBtnAction()
    {
        yield return new WaitForSeconds(0.5f);
        performRedoAction = true;
    }


    private bool performUndoAction = true;
    private void NextPressed()
    {
        if (!performUndoAction) return;
        UndoActionPerform();
    }

    void UndoActionPerform()
    {
        performUndoAction = false;

        if (headIndex < data.Count && data.Count > defaultListSize)
        {
            if (headIndex == 1)
                headIndex++;
            if (headIndex >= data.Count)
                headIndex = data.Count - 1;

            Debug.Log("<color=red>Next Pressed</color>");
            StartCoroutine(SendMessage(data[headIndex].actionObject, data[headIndex].parameter, data[headIndex].methodName,
                data[headIndex].colorParam, data[headIndex].objectType, data[headIndex].category));

            if (headIndex != 1)
                headIndex++;

            isDequeueData = false;
        }
        StartCoroutine(EnableBtnAction2());
        if (currentButtonState != ButtonState.Undo)
        {
            currentButtonState = ButtonState.Undo;
            UndoActionPerform();
        }
    }
    IEnumerator EnableBtnAction2()
    {
        yield return new WaitForSeconds(0.5f);
        performUndoAction = true;
    }


    IEnumerator SendMessage(GameObject actionObject, int paramInt, string method, Color colorParam, Data.ObjectType objectType, Data.CategoryType category)
    {
        addToList = false;
        tempActionObject = actionObject;
        tempMethodName = method;
        tempParam = paramInt;
        tempColor = colorParam;
        StoreStackHandler.obj.ResetValue();                 // AR Changes

        if (!tempActionObject.activeInHierarchy)
        {
            CheckObjectStatus(tempActionObject, objectType, category);
            yield return new WaitForSeconds(0.1f);
        }


        if (tempColor != Color.white)
        {
            //Debug.Log("<color=red>tempActionObject1:: " + tempActionObject.name + "</color> : " + tempMethodName);
            tempActionObject.SendMessage(tempMethodName, tempColor);
        }
        else if (tempParam < 0)
        {
            //Debug.Log("<color=red>tempActionObject2:: " + tempActionObject.name + "</color> : " + tempMethodName);
            tempActionObject.SendMessage(tempMethodName);
        }
        else if (tempParam >= 0)
        {
            //Debug.Log("<color=red>tempActionObject3:: " + tempActionObject.name + "</color> : " + tempMethodName);
            tempActionObject.SendMessage(tempMethodName, tempParam);
        }
    }

    void CheckObjectStatus(GameObject btnObj, Data.ObjectType objectType, Data.CategoryType category)
    {
        bool enableColorPalette = true;
        if (objectType.Equals(Data.ObjectType.ChangeColorBySlider))
            enableColorPalette = false;
        else
            enableColorPalette = true;

        int num = Array.IndexOf(Enum.GetValues(typeof(Data.CategoryType)), category);
        Debug.Log("<color=red>ItemCategoryIndex: " + num + "</color>");
        switch (category)
        {
            case Data.CategoryType.Head:              // 1
                {
                    CheckWearablePanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.Face:              // 2
                {
                    if (objectType.Equals(Data.ObjectType.ChangePanel))
                    {
                        if (panelType.Equals(PanelType.Avatar))
                        {
                            panelType = PanelType.Avatar;
                            CheckWearablePanelStatus();
                        }
                        else if (panelType.Equals(PanelType.Wearable))
                        {
                            panelType = PanelType.Wearable;
                            CheckAvatarPanelStatus();
                        }
                    }
                    else if (objectType.Equals(Data.ObjectType.ChangeItem))
                    {
                        CheckAvatarPanelStatus();
                    }
                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.Inner:             // 3
                {
                    CheckWearablePanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.Outer:             // 4
                {
                    CheckWearablePanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.Accesary:          // 5
                {
                    CheckWearablePanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.Bottom:            // 6
                {
                    CheckWearablePanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.Socks:             // 7
                {
                    CheckWearablePanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.Shoes:             // 8
                {
                    CheckWearablePanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.HairAvatar:        // 9
                {
                    CheckAvatarPanelStatus();

                    if (btnObj.GetComponent<ItemDetail>().name == "ColorButton")
                    {
                        if (InventoryManager.instance.CheckColorPanelEnabled(ConstantsHolder.xanaConstants.currentButtonIndex))
                        {
                            //Debug.Log("<color=blue> Hair Color Panel already enabled");
                            previousInd--;
                            tempActionObject = data[previousInd].actionObject;
                            tempMethodName = data[previousInd].methodName;
                            tempParam = data[previousInd].parameter;
                            tempColor = data[previousInd].colorParam;
                        }
                        else
                            Debug.Log("<color=blue> Hair Color Panel not enable");
                    }
                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.HairAvatarColor:   // 10 set
                {
                    CheckAvatarPanelStatus();

                    avatar_Wearable_Btns[num - 1].GetComponent<ButtonScript>().BtnClicked();
                    addToList = false;
                    if (enableColorPalette)
                    {
                        avatar_Wearable_Btns[num].GetComponent<ItemDetail>().ItemBtnClicked();
                        addToList = false;
                    }
                }
                break;
            case Data.CategoryType.LipsAvatar:        // 11
                {
                    CheckAvatarPanelStatus();

                    if (btnObj.GetComponent<ItemDetail>() != null)
                    {
                        if (InventoryManager.instance.CheckColorPanelEnabled(ConstantsHolder.xanaConstants.currentButtonIndex))
                        {
                            //Debug.Log("<color=blue> Lips Color Panel already enabled");
                            previousInd--;
                            tempActionObject = data[previousInd].actionObject;
                            tempMethodName = data[previousInd].methodName;
                            tempParam = data[previousInd].parameter;
                            tempColor = data[previousInd].colorParam;
                        }
                        else
                            Debug.Log("<color=blue> Lips Color Panel not enable");
                    }
                    else if (btnObj.GetComponent<AvatarBtn>() != null)
                    {
                        previousInd--;
                        tempActionObject = data[previousInd].actionObject;
                        tempMethodName = data[previousInd].methodName;
                        tempParam = data[previousInd].parameter;
                        tempColor = data[previousInd].colorParam;
                    }
                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.LipsAvatarColor:   // 12
                {
                    CheckAvatarPanelStatus();

                    avatar_Wearable_Btns[num - 1].GetComponent<ButtonScript>().BtnClicked();
                    addToList = false;
                    if (enableColorPalette)
                    {
                        avatar_Wearable_Btns[num].GetComponent<ItemDetail>().ItemBtnClicked();
                        addToList = false;
                    }
                }
                break;
            case Data.CategoryType.EyesAvatar:        // 13
                {
                    CheckAvatarPanelStatus();
                    if (btnObj.GetComponent<ItemDetail>() != null)
                    {
                        if (btnObj.GetComponent<ItemDetail>().name == "ColorButton")
                        {
                            if (InventoryManager.instance.CheckColorPanelEnabled(ConstantsHolder.xanaConstants.currentButtonIndex))
                            {
                                //Debug.Log("<color=blue> Eye Color Panel already enabled");
                                previousInd--;
                                tempActionObject = data[previousInd].actionObject;
                                tempMethodName = data[previousInd].methodName;
                                tempParam = data[previousInd].parameter;
                                tempColor = data[previousInd].colorParam;
                            }
                            else
                                Debug.Log("<color=blue> Eye Color Panel not enable");
                        }
                    }
                    else if (btnObj.GetComponent<AvatarBtn>() != null)
                    {
                        previousInd--;
                        tempActionObject = data[previousInd].actionObject;
                        tempMethodName = data[previousInd].methodName;
                        tempParam = data[previousInd].parameter;
                        tempColor = data[previousInd].colorParam;
                    }
                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.EyesAvatarColor:   // 14
                {
                    CheckAvatarPanelStatus();

                    avatar_Wearable_Btns[num - 1].GetComponent<ButtonScript>().BtnClicked();
                    addToList = false;
                    if (enableColorPalette)
                    {
                        avatar_Wearable_Btns[num].GetComponent<ItemDetail>().ItemBtnClicked();
                        addToList = false;
                    }
                }
                break;
            case Data.CategoryType.SkinToneAvatar:    // 15
                {
                    CheckAvatarPanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.Presets:           // 16
                {
                    CheckAvatarPanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.EyeBrowAvatar:     // 17
                {
                    CheckAvatarPanelStatus();

                    if (btnObj.GetComponent<ItemDetail>().name == "ColorButton")
                    {
                        if (InventoryManager.instance.CheckColorPanelEnabled(ConstantsHolder.xanaConstants.currentButtonIndex))
                        {
                            //Debug.Log("<color=blue> EyeBrow Color Panel already enabled");
                            previousInd--;
                            tempActionObject = data[previousInd].actionObject;
                            tempMethodName = data[previousInd].methodName;
                            tempParam = data[previousInd].parameter;
                            tempColor = data[previousInd].colorParam;
                        }
                        else
                            Debug.Log("<color=blue> EyeBrow Color Panel not enable");
                    }
                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.EyeBrowAvatarColor: // 18
                {
                    CheckAvatarPanelStatus();

                    avatar_Wearable_Btns[num - 1].GetComponent<ButtonScript>().BtnClicked();
                    addToList = false;
                    if (enableColorPalette)
                    {
                        avatar_Wearable_Btns[num].GetComponent<ItemDetail>().ItemBtnClicked();
                        addToList = false;
                    }
                }
                break;
            case Data.CategoryType.EyeLashesAvatar:    // 19
                {
                    CheckAvatarPanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.Nose:    // 20
                {
                    CheckAvatarPanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.Body:    // 21
                {
                    CheckAvatarPanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.Makeup:    // 22
                {
                    CheckAvatarPanelStatus();

                    PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.AvatarBtns:    // 22
                {
                    CheckAvatarPanelStatus();

                    //PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.WearableBtns:    // 22
                {
                    CheckWearablePanelStatus();

                    //PressedAvatarAndWearableBtns(num);
                }
                break;
            case Data.CategoryType.SliderColor:  // 23
                {
                    CheckAvatarPanelStatus();

                    //Debug.Log("Slider Color Case: " + btnObj.GetComponent<SilderColorPicker>().sliderCategory);
                    switch (btnObj.GetComponent<SilderColorPicker>().sliderCategory)
                    {
                        case SliderType.HairColor:
                            {
                                bool enableColorScreen = InventoryManager.instance.CheckColorPanelEnabled(0) ? true : false;
                                avatar_Wearable_Btns[8].GetComponent<ButtonScript>().BtnClicked();
                                addToList = false;
                                if (enableColorScreen)
                                {
                                    Debug.Log("<color=blue> Hair Color Panel is Active </color>");
                                    avatar_Wearable_Btns[9].GetComponent<ItemDetail>().ItemBtnClicked();
                                    addToList = false;
                                }
                            }
                            break;
                        case SliderType.EyeBrowColor:
                            {
                                bool enableColorScreen = InventoryManager.instance.CheckColorPanelEnabled(2) ? true : false;
                                avatar_Wearable_Btns[16].GetComponent<ButtonScript>().BtnClicked();
                                addToList = false;
                                if (enableColorScreen)
                                {
                                    Debug.Log("<color=blue> Eye Brow Panel is Active </color>");
                                    avatar_Wearable_Btns[17].GetComponent<ItemDetail>().ItemBtnClicked();
                                    addToList = false;
                                }
                            }
                            break;
                        case SliderType.EyesColor:
                            {
                                bool enableColorScreen = InventoryManager.instance.CheckColorPanelEnabled(3) ? true : false;
                                avatar_Wearable_Btns[12].GetComponent<ButtonScript>().BtnClicked();
                                addToList = false;
                                if (enableColorScreen)
                                {
                                    Debug.Log("<color=blue> Eye Color Panel is Active </color>");
                                    avatar_Wearable_Btns[13].GetComponent<ItemDetail>().ItemBtnClicked();
                                    addToList = false;
                                }
                            }
                            break;
                        case SliderType.LipsColor:
                            {
                                bool enableColorScreen = InventoryManager.instance.CheckColorPanelEnabled(5) ? true : false;
                                avatar_Wearable_Btns[10].GetComponent<ButtonScript>().BtnClicked();
                                addToList = false;
                                if (enableColorScreen)
                                {
                                    Debug.Log("<color=blue> Lips Color Panel is Active </color>");
                                    avatar_Wearable_Btns[11].GetComponent<ItemDetail>().ItemBtnClicked();
                                    addToList = false;
                                }
                            }
                            break;
                        case SliderType.Skin:
                            {
                                avatar_Wearable_Btns[14].GetComponent<ButtonScript>().BtnClicked();
                                addToList = false;
                            }
                            break;
                    }
                }
                break;
        }
    }

    private void PressedAvatarAndWearableBtns(int num)
    {
        avatar_Wearable_Btns[num].GetComponent<ButtonScript>().BtnClicked();
        addToList = false;
    }

    private void CheckAvatarPanelStatus()
    {
        if (panelType.Equals(PanelType.Wearable))
        {
            //Debug.Log("<color=red> Active Avatar Panel </color>");
            InventoryManager.instance.SelectPanel(1);
        }
    }

    private void CheckWearablePanelStatus()
    {
        if (panelType.Equals(PanelType.Avatar))
        {
            //Debug.Log("<color=red> Active Wearable Panel </color>");
            InventoryManager.instance.SelectPanel(0);
        }
    }


}

