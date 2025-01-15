using System.Collections;
using UnityEngine;

public class BodyFaceCustomizer : MonoBehaviour
{
    public static BodyFaceCustomizer Instance;

    public enum MorphType { Face, Body }  // To Set The "BodyCustomizerTrigger" To Modify Either The Face Or Body
    public MorphType m_MorphType;
    public float[] prev;
    

   //[HideInInspector]
    public GameObject m_CharacterFace;

    //[HideInInspector]
    public int[] m_EyesBlendShapeIndex;
    //[HideInInspector]
    public int[] m_NoseBlendShapeIndex;
    //[HideInInspector]
    public int[] m_LipsBlendShapeIndex;
    //[HideInInspector]
    public int[] m_EyeBrowsBlendShapeIndex;

    IEnumerator CoroutineCheck; 

   //[HideInInspector]
    public int[] m_FaceBlendShapeIndex;

    // ** Skin Mesh Renderer ** //
    SkinnedMeshRenderer m_FaceSkinMeshRenderer;

    int size;
    // ** Applied Morph Values ** //  if already a morph is applied
    int m_AppliedMorphOneIndex;
    int m_AppliedMorphTwoIndex;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        /*
        #region populating Body blend values from store manager script
        InventoryManager _sManager = FindObjectOfType<InventoryManager>();    // Getting InventoryManager Ref
        // Setting blend Shape values for morphs
        m_FaceBlendShapeIndex = new int[_sManager.faceAvatarButton.Length];   // populate the length of blend as per no of buttons
        for (int i = 0; i < _sManager.faceAvatarButton.Length; i++)       // 
        {
            m_FaceBlendShapeIndex[i] = _sManager.faceAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;  
        }
        //Eyebrow set indexing for blend shapes
        m_EyeBrowsBlendShapeIndex = new int[_sManager.eyeBrowsAvatarButton.Length];
        for (int i = 0; i < _sManager.eyeBrowsAvatarButton.Length; i++)
        {
            m_EyeBrowsBlendShapeIndex[i] = _sManager.eyeBrowsAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
        }
        //EYE
        m_EyesBlendShapeIndex = new int[_sManager.eyeAvatarButton.Length];
        for (int i = 0; i < _sManager.eyeAvatarButton.Length; i++)
        {
            m_EyesBlendShapeIndex[i] = _sManager.eyeAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
        }
        //LIP
        m_LipsBlendShapeIndex = new int[_sManager.lipAvatarButton.Length];
        for (int i = 0; i < _sManager.lipAvatarButton.Length; i++)
        {
            m_LipsBlendShapeIndex[i] = _sManager.lipAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
        }
        //Nose
        m_NoseBlendShapeIndex = new int[_sManager.noseAvatarButton.Length];
        for (int i = 0; i < _sManager. noseAvatarButton.Length; i++)
        {
            m_NoseBlendShapeIndex[i] = _sManager.noseAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
        }
         #endregion
        */
    }
    public void BodyCustomCallFromStore()
    {  
        //#region populating Body blend values from store manager script
        //InventoryManager _sManager = FindObjectOfType<InventoryManager>();    // Getting InventoryManager Ref

        //// Setting blend Shape values for morphs
        //m_FaceBlendShapeIndex = new int[_sManager.faceAvatarButton.Length];   // populate the length of blend as per no of buttons
        //for (int i = 0; i < _sManager.faceAvatarButton.Length; i++)       // 
        //{
        //    m_FaceBlendShapeIndex[i] = _sManager.faceAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
        //}
        ////Eyebrow set indexing for blend shapes
        //m_EyeBrowsBlendShapeIndex = new int[_sManager.eyeBrowsAvatarButton.Length];
        //for (int i = 0; i < _sManager.eyeBrowsAvatarButton.Length; i++)
        //{
        //    m_EyeBrowsBlendShapeIndex[i] = _sManager.eyeBrowsAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
        //}
        ////EYE
        //m_EyesBlendShapeIndex = new int[_sManager.eyeAvatarButton.Length];
        //for (int i = 0; i < _sManager.eyeAvatarButton.Length; i++)
        //{
        //    m_EyesBlendShapeIndex[i] = _sManager.eyeAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
        //}
        ////LIP
        //m_LipsBlendShapeIndex = new int[_sManager.lipAvatarButton.Length];
        //for (int i = 0; i < _sManager.lipAvatarButton.Length; i++)
        //{
        //    m_LipsBlendShapeIndex[i] = _sManager.lipAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
        //}
        ////Nose
        //m_NoseBlendShapeIndex = new int[_sManager.noseAvatarButton.Length];
        //for (int i = 0; i < _sManager.noseAvatarButton.Length; i++)
        //{
        //    m_NoseBlendShapeIndex[i] = _sManager.noseAvatarButton[i].GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
        //}
        //#endregion
    }

    public void save()
    {
        for (int i = 0; i < m_FaceSkinMeshRenderer.sharedMesh.blendShapeCount; i++)
        {
            prev[i] = m_FaceSkinMeshRenderer.GetBlendShapeWeight(i);
        }

    }
    public void apply_prev()
    {
        size = m_FaceSkinMeshRenderer.sharedMesh.blendShapeCount;
        for (int i = 0; i <size;i ++)
        {
            m_FaceSkinMeshRenderer.SetBlendShapeWeight(i, prev[i]);
        }
       
    }
    public void setprev()
    {
        if(m_FaceSkinMeshRenderer.sharedMesh.blendShapeCount< 0)
        {
            size = m_FaceSkinMeshRenderer.sharedMesh.blendShapeCount;
            prev = new float[size];

            for (int i = 0; i < size; i++)
            {
                prev[i] = m_FaceSkinMeshRenderer.GetBlendShapeWeight(i);
            }
        }
        
    }
    public void Resetbody()
    {
      for(int i=0;i< m_FaceSkinMeshRenderer.sharedMesh.blendShapeCount; i++)
        {
            m_FaceSkinMeshRenderer.SetBlendShapeWeight(i, 0);
        }
    }    

    void Start()
    {
        m_CharacterFace = GameManager.Instance.m_ChHead;
        m_FaceSkinMeshRenderer = m_CharacterFace.GetComponent<SkinnedMeshRenderer>();
        Initialization();

    }

    #region Initilization

    void Initialization()
    {
        m_AppliedMorphOneIndex = -1;
        m_AppliedMorphTwoIndex = -1;
             m_FaceSkinMeshRenderer = m_CharacterFace.GetComponent<SkinnedMeshRenderer>();
        
        LoadLastAppliedFaceBlendShape();
        setprev();
    }
    #endregion

    #region Apply Face Blend Shape

    public void ModifyCharacter_Face(int l_IndexOne, int l_IndexTwo, float l_AnimateTime, AnimationCurve l_AnimCurve, int l_ShapeIndex)
    {
        StopAllCoroutines();
        SaveCurrentApplieBlendShape(l_IndexOne, l_IndexTwo);
        StartCoroutine(FaceModificationAnimation(l_AnimateTime, l_IndexOne, l_IndexTwo, l_AnimCurve));
    }

    IEnumerator FaceModificationAnimation(float l_AnimateTime, int l_IndexOne, int l_IndexTwo, AnimationCurve l_AnimCurve)
    {
        float l_t = 0;
        ResetAllWeights();

        while (l_t <= l_AnimateTime)
        {
            l_t += Time.fixedDeltaTime;
            float blendWeight = Mathf.Lerp(0, 100, l_AnimCurve.Evaluate(l_t / l_AnimateTime));

            if (l_IndexOne != -1)
                m_FaceSkinMeshRenderer.SetBlendShapeWeight(l_IndexOne, blendWeight);

            if (l_IndexTwo != -1)
                m_FaceSkinMeshRenderer.SetBlendShapeWeight(l_IndexTwo, blendWeight);
            yield return null;
        }
    }

    #endregion


    #region Save And Load Face Blend Shapes

    private void SaveCurrentApplieBlendShape(int l_one, int l_two)
    {

        m_IsFaceBlendShapeApplied = true;
        PlayerPrefs.SetInt("FaceMorphIndexOne", l_one);
        PlayerPrefs.SetInt("FaceMorphIndexTwo", l_two);

        m_AppliedMorphOneIndex = l_one;
        m_AppliedMorphTwoIndex = l_two;
    }

    public void LoadLastAppliedFaceBlendShape()
    {
        if (!m_IsFaceBlendShapeApplied) return;

        m_AppliedMorphOneIndex = PlayerPrefs.GetInt("FaceMorphIndexOne");
        m_AppliedMorphTwoIndex = PlayerPrefs.GetInt("FaceMorphIndexTwo");

        if (m_AppliedMorphOneIndex != -1)
            m_FaceSkinMeshRenderer.SetBlendShapeWeight(m_AppliedMorphOneIndex, 100);

        if (m_AppliedMorphTwoIndex != -1)
            m_FaceSkinMeshRenderer.SetBlendShapeWeight(m_AppliedMorphTwoIndex, 100);
       

    }

    #endregion

    #region Properties

    public bool m_IsFaceBlendShapeApplied
    {
        get { return PlayerPrefs.GetInt("FaceBlendShapeApplied") == 1 ? true : false; }
        set { PlayerPrefs.SetInt("FaceBlendShapeApplied", value ? 1 : 0); }
    }

    public int m_BodyShapeIndex
    {
        get { return PlayerPrefs.GetInt("AppliedShapeIndexppp"); }
        set { PlayerPrefs.SetInt("AppliedShapeIndexppp", value); }
    }

    #endregion

    void ResetAllWeights()
    {
        int blends = m_FaceSkinMeshRenderer.sharedMesh.blendShapeCount;
        for (int i = 0; i < blends; i++)
        {
            m_FaceSkinMeshRenderer.SetBlendShapeWeight(i, 0);
        }
    }
    public void ApplyEyesBlendShapes(int l_IndexOne,int l_IndexTwo)
    {
        for(int i=0;i<m_EyesBlendShapeIndex.Length;i++)
        {
            m_FaceSkinMeshRenderer.SetBlendShapeWeight(m_EyesBlendShapeIndex[i], 0);
        }
        if (CoroutineCheck != null)
            StopCoroutine(CoroutineCheck);
          CoroutineCheck = AnimateBlendShape(0.1f, l_IndexOne, l_IndexTwo);
     
        StartCoroutine(CoroutineCheck);
   //     StartCoroutine(AnimateBlendShape(0.05f, l_IndexOne, l_IndexTwo));
    }

    public void ApplyNoseBlendShapes(int l_IndexOne, int l_IndexTwo)
    {
        for (int i = 0; i < m_NoseBlendShapeIndex.Length; i++)
        {
            m_FaceSkinMeshRenderer.SetBlendShapeWeight(m_NoseBlendShapeIndex[i], 0);
        }
        //   StartCoroutine(AnimateBlendShape(0.1f, l_IndexOne, l_IndexTwo));
        if (CoroutineCheck != null)
            StopCoroutine(CoroutineCheck);
        CoroutineCheck = AnimateBlendShape(0.1f, l_IndexOne, l_IndexTwo);

        StartCoroutine(CoroutineCheck);
    }

    public void ApplyLipsBlendShapes(int l_IndexOne, int l_IndexTwo)
    {
        for (int i = 0; i < m_LipsBlendShapeIndex.Length; i++)
        {
            m_FaceSkinMeshRenderer.SetBlendShapeWeight(m_LipsBlendShapeIndex[i], 0);
        }
        // StartCoroutine(AnimateBlendShape(0.1f, l_IndexOne, l_IndexTwo));
        if (CoroutineCheck != null)
            StopCoroutine(CoroutineCheck);
        CoroutineCheck = AnimateBlendShape(0.1f, l_IndexOne, l_IndexTwo);

        StartCoroutine(CoroutineCheck);
    }

    public void ApplyEyeBrowsBlendShapes(int l_IndexOne, int l_IndexTwo)
    {
        for (int i = 0; i < m_EyeBrowsBlendShapeIndex.Length; i++)
        {
            m_FaceSkinMeshRenderer.SetBlendShapeWeight(m_EyeBrowsBlendShapeIndex[i], 0);
        }
        //   StartCoroutine(AnimateBlendShape(0.1f, l_IndexOne, l_IndexTwo));
        if (CoroutineCheck != null)
            StopCoroutine(CoroutineCheck);
        CoroutineCheck = AnimateBlendShape(0.1f, l_IndexOne, l_IndexTwo);

        StartCoroutine(CoroutineCheck);
    }

    // WaqasAhmad
    public void ApplyEyeBrowsBlendShapes(Vector3 _value)
    {

        SkinnedMeshRenderer skinmesh = m_CharacterFace.GetComponent<SkinnedMeshRenderer>();
        skinmesh.SetBlendShapeWeight(16, _value.x); // EyeBrow_Outer_Up Old Index = 0
        skinmesh.SetBlendShapeWeight(17, _value.y); // EyeBrow_Mid_Down Old Index = 24
        skinmesh.SetBlendShapeWeight(21, _value.z); // EyeBrow_Mid_Up   Old Index = 35
        //   StartCoroutine(AnimateBlendShape(0.1f, l_IndexOne, l_IndexTwo));
        if (CoroutineCheck != null)
            StopCoroutine(CoroutineCheck);
        //CoroutineCheck = AnimateBlendShape(0.1f, l_IndexOne, l_IndexTwo);
        //StartCoroutine(CoroutineCheck);
    }

    public void ApplyFaceBlendShapes(int l_IndexOne, int l_IndexTwo)
    {
        for (int i = 0; i < m_FaceBlendShapeIndex.Length; i++)
        {
            m_FaceSkinMeshRenderer.SetBlendShapeWeight(m_FaceBlendShapeIndex[i], 0);
        }
        // StartCoroutine(AnimateBlendShape(0.1f, l_IndexOne, l_IndexTwo));
        if (CoroutineCheck != null)
            StopCoroutine(CoroutineCheck);
        CoroutineCheck = AnimateBlendShape(0.1f, l_IndexOne, l_IndexTwo);

        StartCoroutine(CoroutineCheck);
    }




    IEnumerator AnimateBlendShape(float l_TimeLimit,int l_IndexOne,int l_IndexTwo)
    {
        float l_T = 0;

        while(l_T<=l_TimeLimit)
        {
            l_T += Time.deltaTime;

            float l_BlendWeight = Mathf.Lerp(0, 100, (l_T / l_TimeLimit));

            if (l_IndexOne != -1)
                m_FaceSkinMeshRenderer.SetBlendShapeWeight(l_IndexOne, l_BlendWeight);

            if(l_IndexTwo!=-1)
                m_FaceSkinMeshRenderer.SetBlendShapeWeight(l_IndexTwo, l_BlendWeight);

            yield return null;
        }
    }
}
