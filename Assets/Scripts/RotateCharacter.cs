using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateCharacter : MonoBehaviour
{
    public void CharacterRotate()
    {
        //if(AvatarCustomizationManager.Instance.checkInternet.ispopUpClose || SceneManager.GetActiveScene().name.Contains("InventoryScene 1"))
            AvatarCustomizationManager.Instance.RotateAvatar();
    }
}
