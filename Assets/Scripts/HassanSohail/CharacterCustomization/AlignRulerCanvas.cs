using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignRulerCanvas : MonoBehaviour
{
    [SerializeField] GameObject obj;
    public void AlignCanvas() {
        if (obj.activeInHierarchy)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.SetActive(true);
        }
    }
}
