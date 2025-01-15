using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SavedPlayerDataJson : MonoBehaviour
{
    public string id;
    //public string playername;
    public string avatarJson;
    public string avatarThumbnailLink;
    public TMP_Text playerName;
    public GameObject ImageBG;
    public GameObject ImageObject;
    public GameObject ImageDownloadingLoader; // Download loader to show downloading
    //public Color[] color;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => SetJson());
        //ImageBG.GetComponent<RawImage>().color = color[(Random.Range(0,color.Length))];
    }

    public void SetJson()
    {
        LoadPlayerAvatar.avatarId = id;
        LoadPlayerAvatar.avatarName = playerName.text;
        LoadPlayerAvatar.avatarJson = avatarJson;
        LoadPlayerAvatar.currentSelected = this.gameObject;
    }
}
