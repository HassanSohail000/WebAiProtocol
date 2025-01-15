//with the exception of some minor additions, this is not the work of GameDevStudent. The script was downloaded from here: https://github.com/masterprompt/ModelStitching 

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Stitcher
{
    /// <summary>
    /// Stitch clothing onto an avatar.  Both clothing and avatar must be instantiated however clothing may be destroyed after.
    /// </summary>
    /// <param name="sourceClothing"></param>
    /// <param name="targetAvatar"></param>
    /// <returns>Newly created clothing on avatar</returns>
    CharacterBodyParts body;


    public GameObject Stitch(GameObject sourceClothing, GameObject targetAvatar)
    {
        TransformCatalog boneCatalog = new TransformCatalog(targetAvatar.transform);
        SkinnedMeshRenderer[] skinnedMeshRenderers = sourceClothing.GetComponentsInChildren<SkinnedMeshRenderer>();
        GameObject targetClothing = AddChild(sourceClothing, targetAvatar.transform);
        foreach (SkinnedMeshRenderer sourceRenderer in skinnedMeshRenderers)
        {
            SkinnedMeshRenderer targetRenderer = AddSkinnedMeshRenderer(sourceRenderer, targetClothing);
            targetRenderer.bones = TranslateTransforms(sourceRenderer.bones, boneCatalog);
        }
        return targetClothing;
    }

    public GameObject Stitch_Vtuber(GameObject sourceClothing, GameObject targetAvatar, CharacterBodyParts charBody)
    {
        TransformCatalog boneCatalog = new TransformCatalog(targetAvatar.transform);
        SkinnedMeshRenderer[] skinnedMeshRenderers = sourceClothing.GetComponentsInChildren<SkinnedMeshRenderer>();
        GameObject targetClothing = AddChild(sourceClothing, targetAvatar.transform);
        
        if(charBody != null) body = charBody;

        foreach (SkinnedMeshRenderer sourceRenderer in skinnedMeshRenderers)
        {
            SkinnedMeshRenderer targetRenderer = AddSkinnedMeshRenderer(sourceRenderer, targetClothing);
            targetRenderer.bones = TranslateTransforms_Vtuber(sourceRenderer.bones, boneCatalog, targetAvatar.transform);
        }
        return targetClothing;
    }


    private GameObject AddChild(GameObject source, Transform parent)
    {
        GameObject target = new GameObject(source.name);
        target.transform.parent = parent;
        target.transform.localPosition = source.transform.localPosition;
        target.transform.localRotation = source.transform.localRotation;
        target.transform.localScale = source.transform.localScale;
        return target;
    }

    private SkinnedMeshRenderer AddSkinnedMeshRenderer(SkinnedMeshRenderer source, GameObject parent)
    {
        SkinnedMeshRenderer target = parent.AddComponent<SkinnedMeshRenderer>();
        target.sharedMesh = source.sharedMesh;
        target.materials = source.sharedMaterials;
        return target;
    }

    private Transform[] TranslateTransforms(Transform[] sources, TransformCatalog transformCatalog)
    {
        Transform[] targets = new Transform[sources.Length];
        for (int index = 0; index < sources.Length; index++)
        {
            targets[index] = DictionaryExtensions.Find(transformCatalog, sources[index].name);
            if (targets[index] == null)
            {
                Debug.LogWarning($"Transform '{sources[index].name}' not found in target avatar.");
            }
        }
        return targets;
    }
    private Transform[] TranslateTransforms_Vtuber(Transform[] sources, TransformCatalog transformCatalog, Transform targetRoot)
    {
        Transform[] targets = new Transform[sources.Length];
        for (int index = 0; index < sources.Length; index++)
        {
            targets[index] = DictionaryExtensions.Find(transformCatalog, sources[index].name);
            if (targets[index] == null)
            {
                Debug.LogWarning($"Transform '{sources[index].name}' not found in target avatar. Creating a new transform.");
                GameObject newBone = new GameObject(sources[index].name);
                newBone.transform.parent = FindOrCreateParentTransform(sources[index].parent, transformCatalog, targetRoot);
                newBone.transform.localPosition = sources[index].localPosition;
                newBone.transform.localRotation = sources[index].localRotation;
                newBone.transform.localScale = sources[index].localScale;
                CopyDynamicBoneComponent(sources[index].gameObject, newBone);
                targets[index] = newBone.transform;
                transformCatalog.Add(newBone.name, newBone.transform); // Add the new bone to the catalog

             //   if (body.dynamicBone_Dress.Contains(newBone) == false)
                    body.dynamicBone_Dress.Add(newBone);
            }
        }
        return targets;
    }

    private void CopyDynamicBoneComponent(GameObject source, GameObject target)
    {
        var sourceDynamicBone = source.GetComponent<DynamicBone>();
        if (sourceDynamicBone != null)
        {
            System.Type type = sourceDynamicBone.GetType();
            Component targetComponent = target.AddComponent(type);
            foreach (var field in type.GetFields())
            {
                field.SetValue(targetComponent, field.GetValue(sourceDynamicBone));
            }

            target.GetComponent<DynamicBone>().m_Root = target.transform.parent;
        }
    }


    private Transform FindOrCreateParentTransform(Transform sourceParent, TransformCatalog transformCatalog, Transform targetRoot)
    {
        if (sourceParent == null)
        {
            return targetRoot;
        }

        Transform targetParent = DictionaryExtensions.Find(transformCatalog, sourceParent.name);
        if (targetParent == null)
        {
            Debug.LogWarning($"Parent transform '{sourceParent.name}' not found. Creating a new parent transform.");
            GameObject newParent = new GameObject(sourceParent.name);
            newParent.transform.parent = FindOrCreateParentTransform(sourceParent.parent, transformCatalog, targetRoot);
            newParent.transform.localPosition = sourceParent.localPosition;
            newParent.transform.localRotation = sourceParent.localRotation;
            newParent.transform.localScale = sourceParent.localScale;
            targetParent = newParent.transform;
            transformCatalog.Add(newParent.name, newParent.transform); // Add the new parent to the catalog
        }
       
        return targetParent;
    }


    #region TransformCatalog
    private class TransformCatalog : Dictionary<string, Transform>
    {
        #region Constructors
        public TransformCatalog(Transform transform)
        {
            Catalog(transform);
        }
        #endregion

        #region Catalog
        private void Catalog(Transform transform)
        {
            if (ContainsKey(transform.name))
            {
                Remove(transform.name);
                Add(transform.name, transform);
            }
            else
                Add(transform.name, transform);
            foreach (Transform child in transform)
                Catalog(child);
        }
        #endregion
    }
    #endregion


    #region DictionaryExtensions
    private class DictionaryExtensions
    {
        public static TValue Find<TKey, TValue>(Dictionary<TKey, TValue> source, TKey key)
        {
            TValue value;
            source.TryGetValue(key, out value);
            return value;
        }
    }
    #endregion

}

