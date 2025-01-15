using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBG : MonoBehaviour
{
    public GameObject mobileBG;
    public GameObject tabBG;
    // Start is called before the first frame update
    void Start()
    {
        if (ConstantsHolder.xanaConstants.screenType == ConstantsHolder.ScreenType.TabScreen)
        {
            tabBG.SetActive(true);
            mobileBG.SetActive(false);
        }
        else
        {
            mobileBG.SetActive(true);
            tabBG.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
