using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TelegramUiHandler : MonoBehaviour
{
    public List<GameObject> TelegramUIObj;

    private void Start()
    {
        StartCoroutine(TelegramFunctionality());
    }

    IEnumerator TelegramFunctionality()
    {
        yield return new WaitForEndOfFrame();
        if (ConstantsHolder.xanaConstants.TelegramLoginBtnStatus)
        {
            // print("IS true " + walletFunctionalitybool);
            foreach (GameObject go in TelegramUIObj)
            {
                go.SetActive(true);
            }
        }
        else
        {
            //print("IS false " + walletFunctionalitybool);
            foreach (GameObject go in TelegramUIObj)
            {
                go.SetActive(false);
            }
        }
    }
}