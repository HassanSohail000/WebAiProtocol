
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
//using WebSocketSharp;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager _instance;
    public string LocalizeURL,LocalizeDateStamp;
    private string _path;
    private Coroutine prevCoroutine;
    [SerializeField]
    private bool _forceJapanese;
    public static bool forceJapanese;

    public static Dictionary<string , RecordsLanguage> localisationDict;
    public static bool IsReady;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        //DontDestroyOnLoad(this);
        forceJapanese = _forceJapanese;
        _path = Application.persistentDataPath + "/Localization.dat";
        if (!File.Exists(_path))
        {
            File.Create(_path);
        }

        prevCoroutine = StartCoroutine(GetLocalizationDataFromSheet());  
          
        GameManager.currentLanguage = GetLanguage();   
    }

    IEnumerator CheckIfSheetUpdated()
    {
        var www = UnityWebRequest.Get(LocalizeDateStamp);
        www.SendWebRequest();
        while(!www.isDone)
        {
            yield return null; 
        }
        if (www.result==UnityWebRequest.Result.ConnectionError || www.result==UnityWebRequest.Result.ProtocolError) 
        {
            Debug.Log(www.error);
            StopAllCoroutines();
            StartCoroutine(CheckIfSheetUpdated());
            //Awake();
        }
        else
        {
            var json = www.downloadHandler.text;
            var dateTime = DateTime.Parse(json);
            
            if (DateTime.Parse(PlayerPrefs.GetString("DateTime")) < dateTime || !PlayerPrefs.HasKey("DateTime"))
            {
                PlayerPrefs.SetString("DateTime", json);
                StartCoroutine(GetLocalizationDataFromSheet());
            }
            else
            {
                print("Not Updated USe previous sheet");
            }
        }

    }
    
    IEnumerator GetLocalizationDataFromSheet()
    {
     //   yield return new WaitForSeconds(2);   
        
       var www = UnityWebRequest.Get(LocalizeURL);
       www.SendWebRequest();
        while(!www.isDone)
        {
            yield return null;
        }
       if (www.result==UnityWebRequest.Result.ConnectionError || www.result!=UnityWebRequest.Result.Success)
       {
           IsReady = false;
           
           Coroutine current = StartCoroutine(GetLocalizationDataFromSheet());
           if (prevCoroutine != null)
           {
               StopCoroutine(prevCoroutine);
           }

           prevCoroutine = current;
       }
       else
       {
            var json = www.downloadHandler.text;

           if (!string.IsNullOrEmpty(json))
            {
               // var writer = new StreamWriter(_path, false);
               // writer.Write(json);
               // writer.Close();
               
               RecordsLanguage[] localisationSheet = CSVSerializer.Deserialize<RecordsLanguage>(json);
               localisationDict = localisationSheet.GroupBy(p => p.Keys, StringComparer.OrdinalIgnoreCase)
                   .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase); //Remove Duplicate entries
               IsReady = true;
           }
           else
           {
               Debug.Log("Json is empty");
           }
       }
    }

    #region Unused Functions

    void  ReadDataFromFile()
    {
        //    StreamReader reader = new StreamReader(path);
        //    Debug.Log(reader.ReadToEnd());
        //      string json = reader.ReadToEnd();
        // reader.Close();
        //  Debug.Log(json);
//        RecordsLanguage[] avc = CSVSerializer.Deserialize<RecordsLanguage>(json);

    }



    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);
        file.Close();
    }

    #endregion

    public bool IsJapanese(string text) //Detect Japanese characters //sohaib
    {
        int count = 0;
        foreach (char c in text)
        {
            // Check if the character is in the Japanese Hiragana, Katakana, or Kanji ranges
            if ((c >= '\u3040' && c <= '\u309F') || // Hiragana
                (c >= '\u30A0' && c <= '\u30FF') || // Katakana
                (c >= '\u4E00' && c <= '\u9FAF'))   // Kanji
            {
                // If any Japanese character is found, return true
                count++;
                if (count >= 5)
                {
                    return true;
                }
            }
        }

        // If no Japanese characters are found, return false
        return false;
    }
    public static string GetLanguage()
    {
#if UNITY_EDITOR || UNITY_WEBGL
        string newLanguage = Application.systemLanguage.ToString();

        if (newLanguage == "English")
        {
            return "en";
        }
        else if (newLanguage == "Japanese")
        {
            return "ja";
        }
        else
        {
            return "";
        }
#elif UNITY_ANDROID
        try
        {
            var locale = new AndroidJavaClass("java.util.Locale");
            var localeInst = locale.CallStatic<AndroidJavaObject>("getDefault");
            var name = localeInst.Call<string>("getLanguage");
            return name;
        }
        catch (System.Exception e)
        {
            return "Error";
        }
#elif UNITY_IOS
        string newLanguage = Application.systemLanguage.ToString();

        if (newLanguage == "English")
        {
            return "en";
        }
        else if (newLanguage == "Japanese")
        {
            return "ja";
        }
        else
        {
            return "";
        }
#else
        return "en"; // Default return value for other platforms
#endif
    }
    // returns "en" / "deu" / ...

    public static string GetISO3Language()
    {
#if UNITY_EDITOR || UNITY_WEBGL
        string newLanguage = Application.systemLanguage.ToString();

        if (newLanguage == "English")
        {
            return "en";
        }
        else if (newLanguage == "Japanese")
        {
            return "ja";
        }
        else
        {
            return "";
        }
#elif UNITY_ANDROID
        try
        {
            var locale = new AndroidJavaClass("java.util.Locale");
            var localeInst = locale.CallStatic<AndroidJavaObject>("getDefault");
            var name = localeInst.Call<string>("getISO3Language");
            return name;
        }
        catch (System.Exception e)
        {
            return "Error";
        }
#elif UNITY_IOS
        string newLanguage = Application.systemLanguage.ToString();

        if (newLanguage == "English")
        {
            return "en";
        }
        else if (newLanguage == "Japanese")
        {
            return "ja";
        }
        else
        {
            return "";
        }
#else
        return "en"; // Default return value for other platforms
#endif
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            GameManager.currentLanguage = GetLanguage();

            TextLocalization[] arrayOfTextLocale = FindObjectsOfTypeAll(typeof(TextLocalization)) as TextLocalization[];
            foreach (TextLocalization tl in arrayOfTextLocale)
            {
                tl.LocalizeTextText();
            }
        }
    }
}


public class RecordsLanguage
{
    public  string Keys;
    public string English;
    public string Japanese;
    public string Korean;
    public string Chinese;
}