
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoneDataContainer { 
    public string Name;
    public GameObject Obj;
    public Vector3 Pos;
    public Vector3 Rotation;
    public Vector3 Scale;

    public BoneDataContainer() { }

    /// <summary>
    /// Constructor to Set bone position, rotation, scale for saving
    /// </summary>
    public  BoneDataContainer(string _name, GameObject _obj )
    {
        Name = _name;
        Obj = _obj;
        //Pos =  new Vector3(Mathf.Round( _pos.x), Mathf.Round(_pos.y), Mathf.Round(_pos.z)) ;
        ////Rotation = new Vector3(Mathf.Round(_rot.x), Mathf.Round(_rot.y), Mathf.Round(_rot.z));
        //Scale = new Vector3(Mathf.Round(_scale.x), Mathf.Round(_scale.y), Mathf.Round(_scale.z));
    }


    public BoneDataContainer(string _name,  Vector3 _pos, Vector3 _rot, Vector3 _scale)
    {
        Name = _name;
        Pos = new Vector3(_pos.x, _pos.y, _pos.z);
        Rotation = new Vector3(_rot.x, _rot.y, _rot.z);
        Scale = new Vector3(_scale.x, _scale.y, _scale.z);
    }

    public BoneDataContainer(string _name, GameObject _obj, Vector3 _pos, Vector3 _rot, Vector3 _scale)
    {
        Name = _name;
        Obj = _obj;
        Pos = new Vector3(_pos.x, _pos.y, _pos.z);
        Rotation = new Vector3(_rot.x, _rot.y, _rot.z);
        Scale = new Vector3(_scale.x, _scale.y, _scale.z);
    }
}




