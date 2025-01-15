using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EyesBlinking : MonoBehaviour
{
    public SkinnedMeshRenderer blendHolder;
    public List<EyeBlendShape> AllEyeBlendShapes = new List<EyeBlendShape>();

    [HideInInspector]
    public float blinkingRate;
    [HideInInspector]
    public bool AreEyesClosed = false;
    [Range(2f, 3f)]
    public float waitTime;
    [Range(0.1f, 0.2f)]
    public float blinkingSpeed;
    [HideInInspector]
    public bool isBlinking = true;

    public bool isCoroutineRunning = true;
    private float currentBlendWeight = 0f;


    [Serializable]
    public class EyeBlendShape
    {
        public int index;
        public float value;
    }

    private void Awake()
    {
            for (int i = 0; i < blendHolder.sharedMesh.blendShapeCount; i++)   // Get all the blend shapes that are related to eyes
            {
                string blendShapeName = blendHolder.sharedMesh.GetBlendShapeName(i);
                if (blendShapeName.Contains("eye"))
                {
                    AllEyeBlendShapes.Add(new EyeBlendShape { index = i });
                }
            }

        waitTime = UnityEngine.Random.Range(2f, 3f);            // Randomize the wait time
        blinkingSpeed = UnityEngine.Random.Range(0.1f, 0.2f);   // Randomize the blinking speed
    }
    public void StoreBlendShapeValues()
    {
        if (blendHolder != null)
        {
            isBlinking = false;
            blendHolder.SetBlendShapeWeight(8, 0);
            blendHolder.SetBlendShapeWeight(9, 0);

            for (int i = 0; i < AllEyeBlendShapes.Count; i++)
            {
                AllEyeBlendShapes[i].value = blendHolder.GetBlendShapeWeight(AllEyeBlendShapes[i].index);
            }
            isBlinking = true;
        }
    }

    public IEnumerator BlinkingStartRoutine()
    {
        if (isBlinking && AllEyeBlendShapes.Count != 0)
        {
            while (!AreEyesClosed)
            {
                UpdateBlendShapes(0);
                currentBlendWeight += Time.deltaTime * blinkingRate;
                if (currentBlendWeight >= 100f)
                {
                    currentBlendWeight = 100f;
                    AreEyesClosed = true;
                }

                if (name.Contains("Vtuber"))
                {
                    blendHolder.SetBlendShapeWeight(37, currentBlendWeight);
                }
                else
                {
                    blendHolder.SetBlendShapeWeight(8, currentBlendWeight);  // Set the blend shape weight for the left eye
                    blendHolder.SetBlendShapeWeight(9, currentBlendWeight);  // Set the blend shape weight for the right eye
                }
            }
            yield return new WaitForSeconds(blinkingSpeed);
            while (AreEyesClosed)
            {
                currentBlendWeight -= Time.deltaTime * blinkingRate;
                if (currentBlendWeight <= 0f)
                {
                    currentBlendWeight = 0f;
                    AreEyesClosed = false;
                }
                UpdateBlendShapesToOriginal();
                if (name.Contains("Vtuber"))
                {
                    blendHolder.SetBlendShapeWeight(37, currentBlendWeight);
                }
                else
                {
                    blendHolder.SetBlendShapeWeight(8, currentBlendWeight);
                    blendHolder.SetBlendShapeWeight(9, currentBlendWeight);
                }
            }
            isCoroutineRunning = true;
            yield return new WaitForSeconds(waitTime);
            if (gameObject.activeInHierarchy)
                StartCoroutine(BlinkingStartRoutine());
        }
        isCoroutineRunning = false;
    }

    private void UpdateBlendShapes(float targetWeight)
    {
        foreach (var blendShape in AllEyeBlendShapes)
        {
            if (blendShape.index == 8 || blendShape.index == 9) continue;
            float currentWeight = blendHolder.GetBlendShapeWeight(blendShape.index);
            blendHolder.SetBlendShapeWeight(blendShape.index, Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * blinkingRate));
        }
    }
    private void UpdateBlendShapesToOriginal()
    {
        foreach (var blendShape in AllEyeBlendShapes)
        {
            if (blendShape.index == 8 || blendShape.index == 9) continue;
            float currentWeight = blendHolder.GetBlendShapeWeight(blendShape.index);
            blendHolder.SetBlendShapeWeight(blendShape.index, Mathf.Lerp(currentWeight, blendShape.value, Time.deltaTime * blinkingRate));
        }
    }
    private void OnDisable()
    {
        isCoroutineRunning = false;
    }
}