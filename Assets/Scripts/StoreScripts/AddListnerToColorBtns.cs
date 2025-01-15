using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddListnerToColorBtns : MonoBehaviour
{
    public int colorPanelNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(ColorBtnPressed);
    }
    private void ColorBtnPressed()
    {
        StoreStackHandler.obj.UpdateColorPanelStatus(colorPanelNum, true);
    }


}
