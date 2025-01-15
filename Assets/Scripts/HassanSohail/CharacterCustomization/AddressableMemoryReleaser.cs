using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
//using Nethereum.Signer.Crypto;
using UnityEngine.InputSystem;

public class AddressableMemoryReleaser : MonoBehaviour
{
    [SerializeField]
    private List<MemoryObject> _memoryObjects = new List<MemoryObject>();
    public void AddToReferenceList(AsyncOperationHandle Obj,string refKey)
    {
        foreach(MemoryObject item in _memoryObjects)
        {
            if (item.Key.Equals(refKey))
            {
                return;
            }
        }
        MemoryObject refItem = new(refKey, Obj);
        _memoryObjects.Add(refItem);
        /*if (memoryObjects.Count > 0)
        {
            if (memoryObjects.First(x => x.Key.Equals(refKey)) == null)
            {
                MemoryObject item = new(refKey, Obj);
                memoryObjects.Add(item);
            }
        }
        else
        {
            MemoryObject item = new(refKey, Obj);
            memoryObjects.Add(item);
        }*/
    }
    public AsyncOperationHandle GetReferenceIfExist(string refKey,ref bool flag)
    {
        foreach (MemoryObject item in _memoryObjects)
        {
            if (item.Key.Equals(refKey))
            {
                flag = true;
                return item.HandlerObj;
            }
        }
        /*
        if (memoryObjects.Count > 0)
        {
            Debug.LogError(memoryObjects.Count);
            MemoryObject _mObj = memoryObjects.First(x => x.Key.Equals( refKey));
            if (_mObj != null)
            {
               // Obj = _mObj.HandlerObj;
               flag = true;
                return _mObj.HandlerObj;
            }
        }*/
        return default;
    }
    public void RemoveAllAddressables()
    {
        foreach (MemoryObject item in _memoryObjects)
        {
            if(item.HandlerObj.IsValid())
            {
                Addressables.ReleaseInstance(item.HandlerObj);
            }
        }
        _memoryObjects.Clear();
        GC.Collect();
        //AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
    }
    public void RemoveAddressable(string key)         // Added by Ali Hamza to release specific object based on key
    {
        for (int i = _memoryObjects.Count-1; i >=0 ; i--)
        {
            if (_memoryObjects[i].Key.Equals(key))
            {
                if (_memoryObjects[i].HandlerObj.IsValid())
                {
                    Addressables.ReleaseInstance(_memoryObjects[i].HandlerObj);
                }
                _memoryObjects.Remove(_memoryObjects[i]);
                break;
            }
        }
    }
}
[Serializable]
public class MemoryObject
{
    public string Key;
    public AsyncOperationHandle HandlerObj;
    public MemoryObject (string key,AsyncOperationHandle handler)
    {
        this.Key = key;
        this.HandlerObj = handler;
    }
}