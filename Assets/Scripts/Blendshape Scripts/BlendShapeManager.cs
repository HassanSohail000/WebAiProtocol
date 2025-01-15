using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class BlendShapeManager : MonoBehaviour
{
    //public static BlendShapeManager Instance;

    [Header("Mesh Renderer for Blend Shapes")]
    public SkinnedMeshRenderer m_Renderer;

    [Header("Object to be Placed on BlendShape")]
    public GameObject NosePoint;
    public GameObject LipsPoints, EyesPoints, EyeBrowPoints, FacePoints,/* Body,*/ HeadPoints;

    [Header("Animator and Object Parent")]
    //public Animator anim;
    public Transform PlacedObjectsParent;
    public Transform ReferenceParent;

    [Header("UI Sliders & Options")]
    public Slider SliderX;
    public Slider SliderY;
    [Tooltip("Side to turn off when Side view is selected")]
    public Side TurnOffSide;

    [Header("Blend Filter Options")]
    public FilterBlendShapeSettings FilterSettings;
    public bool FilterAllowed;

    [Tooltip("Min Distance at which corresponding blendshapes are placed")]
    [Header("Distance Settings")]
    public float AllowedDistance;

    public string SelectedPart = "";
    private int ListIndex;
    private int CurrentBlendShapeInd;
    private bool Allowed;
    private int SelectedObjIndex;

    public SphereInfo CurrentSelectedObject;
    private List<CorrespondingVertices> Objects;


    public Sprite defaultIcon, selectedIcon;

    #region Unity Events


    private void Awake()
    {
        //Instance = this;
    }

    private void Start()
    {
        // PlacedPrefab.GetComponent<SpriteRenderer>().material.renderQueue = 5000;
        Objects = new List<CorrespondingVertices>();
    }

    public void SetAllColors(bool active)
    {
        foreach (CorrespondingVertices vert in Objects)
        {
            vert.SetColors(active);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Camera.main != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                SphereInfo info = hit.collider.gameObject.GetComponent<SphereInfo>();
                if (info)
                {
                    // Change All Point Sprites
                    foreach (Transform child in info.transform.parent)
                    {
                        if (child.GetComponent<SpriteRenderer>())
                        {
                            child.GetComponent<SpriteRenderer>().sprite = defaultIcon;
                        }
                    }

                    info.GetComponent<SpriteRenderer>().sprite = selectedIcon;
                    if (info.SiblingPoint != null)
                    {
                        if (info.SiblingPoint.GetComponent<SpriteRenderer>())
                        {
                            info.SiblingPoint.GetComponent<SpriteRenderer>().sprite = selectedIcon;
                        }
                    }

                    CurrentSelectedObject = info;

                    SliderX.gameObject.SetActive(false);
                    SliderY.gameObject.SetActive(false);

                    SliderX.onValueChanged.RemoveAllListeners(); 
                    SliderY.onValueChanged.RemoveAllListeners();
                    GameManager.Instance.mainCharacter.GetComponent<CharacterBodyParts>().ValuesForSliderXY(info.blendListIndex);
                   // CharacterBodyParts.instance.ValuesForSliderXY(info.blendListIndex);
                }
            }
        }
    }


    #endregion

    void ChangeSphareSpriteToDefault(Transform parentObj)
    {
        for (int i = 0; i < parentObj.childCount; i++)
        {
            parentObj.GetChild(i).GetComponent<SpriteRenderer>().sprite = defaultIcon;
        }
       
        SliderX.gameObject.SetActive(false);
        SliderY.gameObject.SetActive(false);
    }

    #region Main Important Functions

    private void BuildInitialObjects()
    {
        //int count = m_Renderer.sharedMesh.blendShapeCount;

        //Stop animation for our mesh calculation
        if (Objects != null && Objects.Count > 0)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                Destroy(Objects[i].FirstObject);
                Destroy(Objects[i].SecondObject);
            }
        }

        Objects = new List<CorrespondingVertices>();
        for (int i = 0; i < Objects.Count; i++)
        {
            Objects[i].FirstObject.transform.SetParent(PlacedObjectsParent, true);
            if (Objects[i].SecondObject)
            {
                Objects[i].SecondObject.transform.SetParent(PlacedObjectsParent, true);
            }
        }
    }
    public void PlaceObjectOnBlendShapes(GameObject obj, int BlendShapeIndex)
    {
        Mesh sharedMesh = m_Renderer.sharedMesh;
        Vector3[] arr = new Vector3[sharedMesh.vertexCount];
        Vector3[] bakedDelta = new Vector3[sharedMesh.vertexCount];

        List<Vector3> Verts = new List<Vector3>();

        //m_Renderer.SetBlendShapeWeight (BlendShapeIndex, 0); //reset Blend Shape value for selected index
        Mesh mesh = new Mesh();
        m_Renderer.BakeMesh(mesh); //Bake new mesh to get updated value
        m_Renderer.sharedMesh.GetBlendShapeFrameVertices(BlendShapeIndex, 0, arr, null, null);

        mesh.AddBlendShapeFrame("tempShape", m_Renderer.GetBlendShapeWeight(BlendShapeIndex), arr, null, null); //Add frame to baked mesh

        mesh.GetBlendShapeFrameVertices(0, 0, bakedDelta, null, null);
        mesh.GetVertices(Verts);

        List<Vector3> DeltaList = bakedDelta.ToList();

        MaxVector FirstMaxVector; //Value to save maximum vector
        FirstMaxVector.max = 0;
        FirstMaxVector.ind = 0;

        for (int i = 0; i < DeltaList.Count; i++)
        {
            if (DeltaList[i] != Vector3.zero)
            {
                if (DeltaList[i].magnitude > FirstMaxVector.max)
                {
                    FirstMaxVector.max = DeltaList[i].magnitude;
                    FirstMaxVector.ind = i;
                }
            }

        }

        //GameObject Sphere = Instantiate(obj);
        //Sphere.transform.position = ReferenceParent.transform.TransformPoint(Verts[FirstMaxVector.ind]);
        //SphereInfo info = Sphere.GetComponent<SphereInfo>();
        //info.BlendShapeIndex = BlendShapeIndex;
        //info.AffectedVertexInd = FirstMaxVector.ind;

        //if (DeltaList[FirstMaxVector.ind].x > DeltaList[FirstMaxVector.ind].y)
        //{
        //    info.AxisType = AxisType.X_Axis;
        //}
        //else
        //{
        //    info.AxisType = AxisType.Y_Axis;
        //}


        DeltaList[FirstMaxVector.ind] = Vector3.zero; //Reset Max Vertex to find second highest Vertex

        FirstMaxVector.max = 0;
        FirstMaxVector.ind = 0;

        for (int i = 0; i < DeltaList.Count; i++)
        {
            if (DeltaList[i] != Vector3.zero)
            {
                if (DeltaList[i].magnitude > FirstMaxVector.max)
                {
                    FirstMaxVector.max = DeltaList[i].magnitude;
                    FirstMaxVector.ind = i;
                }
            }

        }

        Vector3 OtherPos = new Vector3(Verts[FirstMaxVector.ind].x, Verts[FirstMaxVector.ind].y, Verts[FirstMaxVector.ind].z); //Instantiate Object at most changed vertex
        obj.transform.position = ReferenceParent.transform.TransformPoint(OtherPos);


        //GameObject Sphere1 = Instantiate(obj);
        //Vector3 OtherPos = new Vector3(Verts[FirstMaxVector.ind].x, Verts[FirstMaxVector.ind].y, Verts[FirstMaxVector.ind].z); //Instantiate Object at most changed vertex
        //Sphere1.transform.position = ReferenceParent.transform.TransformPoint(OtherPos);
        //SphereInfo info1 = Sphere1.GetComponent<SphereInfo>();
        //info1.BlendShapeIndex = BlendShapeIndex;
        //info1.AffectedVertexInd = FirstMaxVector.ind;
        //info1.ObjectSide = Side.Right;

        //if (Mathf.Abs(DeltaList[FirstMaxVector.ind].x) > Mathf.Abs(DeltaList[FirstMaxVector.ind].y))
        //{
        //    info1.AxisType = AxisType.X_Axis;
        //}
        //else
        //{
        //    info1.AxisType = AxisType.Y_Axis;
        //}

        //if (Vector3.Distance(Sphere1.transform.position, Sphere.transform.position) > AllowedDistance)
        //{

        //    CorrespondingVertices Result = new CorrespondingVertices(Sphere, Sphere1 , TurnOffSide); //Save two most affected vertices found

        //    Objects.Add(Result);
        //    info.MyListIndex = Objects.Count - 1;
        //    info1.MyListIndex = Objects.Count - 1;
        //    if (Sphere.transform.localPosition.x > Sphere1.transform.localPosition.x)
        //    {
        //        info.ObjectSide = Side.Right;
        //        info1.ObjectSide = Side.Left;
        //    }
        //    else
        //    {
        //        info1.ObjectSide = Side.Right;
        //        info.ObjectSide = Side.Left;
        //    }
        //}
        //else
        //{
        //    Destroy(Sphere1);
        //    CorrespondingVertices Result = new CorrespondingVertices(Sphere, null , TurnOffSide); //Save one most affected vertices found

        //    Objects.Add(Result);
        //    info.MyListIndex = Objects.Count - 1;
        //    info.ObjectSide = Side.Middle;
        //}
    }

    #endregion

    #region Slider Change Events
    private void ChangeTriggerPos() //Same as above but now it calculates only when slider changes 
    {
        // SphereInfo FirstObjectInfo = Objects[selectedObjIndex].FirstObject.GetComponent<SphereInfo>();
        //SphereInfo SecondObjectInfo = null;
        //if (Objects[selectedObjIndex].SecondObject)
        //{
        //    SecondObjectInfo = Objects[selectedObjIndex].SecondObject.GetComponent<SphereInfo>();
        //}

        //Mesh tmp = new Mesh();
        //m_Renderer.BakeMesh(tmp);

        //Vector3 [] arr = new Vector3[m_Renderer.sharedMesh.vertexCount];

        //List<Vector3> Verts = new List<Vector3>();
        //m_Renderer.sharedMesh.GetBlendShapeFrameVertices(FirstObjectInfo.BlendShapeIndex, 0, arr, null, null);

        //tmp.AddBlendShapeFrame("newShape" , 1 , arr , null , null);
        //tmp.GetVertices(Verts);

        //Vector3 pos = ReferenceParent.transform.TransformPoint(Verts[FirstObjectInfo.AffectedVertexInd]);
        //Objects[CurrentSelectedObject.MyListIndex].FirstObject.transform.position = new Vector3(pos.x , pos.y , pos.z);

        //if (SecondObjectInfo)
        //{

        //    Vector3 pos1 = ReferenceParent.transform.TransformPoint(Verts[SecondObjectInfo.AffectedVertexInd]);
        //    Objects[CurrentSelectedObject.MyListIndex].SecondObject.transform.position =
        //        new Vector3(pos1.x, pos1.y, pos1.z);
        //}

    }

    #endregion

    #region On Click for Part Selection

    public void MorphTypeSelected(string MorphName)
    {
        print("13131313 --- Hello : " + MorphName);
        SelectedPart = MorphName;
        // BuildInitialObjects(); //Build and Place Triggers on First Selection
    }

    #endregion

    #region Avatar Panel Events

    public void OnLeftSide()
    {
        BuildInitialObjects();
        if (Objects.Count > 0 && Objects != null)
        {
            foreach (CorrespondingVertices Obj in Objects)
            {
                Obj.TurnOffSideObjects();
            }
        }

        // WaqasAhmad
        EnableRespectedSidePoints(SelectedPart, false, true);
    }

    public void OnFrontSide()
    {
        BuildInitialObjects();
        if (Objects.Count > 0 && Objects != null)
        {
            foreach (CorrespondingVertices Obj in Objects)
            {
                Obj.TurnOnAllObjects();
            }
        }

        // WaqasAhmad
        EnableRespectedSidePoints(SelectedPart, true, false);
    }

    /// <summary>
    /// Made by WaqasAhmad
    /// On side change enable respected side points
    /// </summary>
    void EnableRespectedSidePoints(string name, bool status_1, bool status_2)
    {
        print("Yes Calling : " + name + " - " + status_1 + " - " + status_2);
        int childInd = -1;

        if (status_1)
            childInd = 0;
        if(status_2)
            childInd = 1;

        switch (name)
        {
            case "FaceMorph":
                //case "Head":
                FacePoints.transform.GetChild(0).gameObject.SetActive(status_1);
                FacePoints.transform.GetChild(1).gameObject.SetActive(status_2);

                ChangeSphareSpriteToDefault(FacePoints.transform.GetChild(childInd).transform);
                break;

            case "EyeBrowMorph":
            case "EyeBrow":
                EyeBrowPoints.transform.GetChild(0).gameObject.SetActive(status_1);
                EyeBrowPoints.transform.GetChild(1).gameObject.SetActive(status_2);

                ChangeSphareSpriteToDefault(EyeBrowPoints.transform.GetChild(childInd).transform);
                break;

            case "EyesMorph":
            case "eye":
                EyesPoints.transform.GetChild(0).gameObject.SetActive(status_1);
                EyesPoints.transform.GetChild(1).gameObject.SetActive(status_2);

                ChangeSphareSpriteToDefault(EyesPoints.transform.GetChild(childInd).transform);
                break;


            case "LipsMorph":
            case "Lips":
                LipsPoints.transform.GetChild(0).gameObject.SetActive(status_1);
                LipsPoints.transform.GetChild(1).gameObject.SetActive(status_2);

                ChangeSphareSpriteToDefault(LipsPoints.transform.GetChild(childInd).transform);
                break;

            case "NoseMorph":
            case "Nose":
                NosePoint.transform.GetChild(0).gameObject.SetActive(status_1);
                NosePoint.transform.GetChild(1).gameObject.SetActive(status_2);

                ChangeSphareSpriteToDefault(NosePoint.transform.GetChild(childInd).transform);
                break;

            case "HeadMorph":
            case "Head":
                HeadPoints.transform.GetChild(0).gameObject.SetActive(status_1);
                HeadPoints.transform.GetChild(1).gameObject.SetActive(status_2);

                ChangeSphareSpriteToDefault(HeadPoints.transform.GetChild(childInd).transform);
                break;
        }
    }

    #endregion

    #region Customize Navigations

    public void DismissPoints()
    {
        GameManager.Instance.mainCharacter.GetComponent<Animator>().SetBool("Customization", false);
       // PatchForStore.isCustomizationPanelOpen = false;
        SliderX.gameObject.SetActive(false);
        SliderY.gameObject.SetActive(false);
        PlacedObjectsParent.gameObject.SetActive(false);

        NosePoint.SetActive(false);
        LipsPoints.SetActive(false);
        EyesPoints.SetActive(false);
        EyeBrowPoints.SetActive(false);
        FacePoints.SetActive(false);
        HeadPoints.SetActive(false);
    }

    public void TurnOnPoints(string name)
    {
        print("Selected Point : " + SelectedPart + "  /  " + name);
        switch (name)
        {
            case "FaceMorph":
                TurnOffAllObjects();
                FacePoints.SetActive(true);
                break;

            case "EyeBrowMorph":
                TurnOffAllObjects();
                EyeBrowPoints.SetActive(true);
                break;

            case "EyesMorph":
                TurnOffAllObjects();
                EyesPoints.SetActive(true);
                break;

            case "LipsMorph":
                TurnOffAllObjects();
                LipsPoints.SetActive(true);
                break;

            case "NoseMorph":
                TurnOffAllObjects();
                NosePoint.SetActive(true);
                break;

            case "HeadMorph":
                TurnOffAllObjects();
                HeadPoints.SetActive(true);
                break;
        }
        EnableRespectedSidePoints(name, true, false);
    }

    public void TurnOffAllObjects()
    {
        FacePoints.SetActive(false);
        EyeBrowPoints.SetActive(false);
        EyesPoints.SetActive(false);
        LipsPoints.SetActive(false);
        NosePoint.SetActive(false);
        HeadPoints.SetActive(false);
    }

    #endregion

    #region Private Classes

    private struct MaxVector
    {
        public int ind;
        public float max;
    }

    private class CorrespondingVertices
    {
        public GameObject FirstObject;
        public GameObject SecondObject;
        public Side CheckSide;


        public CorrespondingVertices(GameObject firstObject, GameObject secondObject, Side checkSide)
        {
            FirstObject = firstObject;
            SecondObject = secondObject;
            CheckSide = checkSide;
        }

        public void SetColors(bool selected)
        {
            if (selected)
            {
                ChangeObjectColor(Color.green);
            }
            else
            {
                ChangeObjectColor(Color.gray);
            }
        }

        public void TurnOffSideObjects()
        {
            //if (FirstObject.GetComponent<SphereInfo>().ObjectSide == CheckSide)f
            //{
            //    FirstObject.SetActive(false);
            //}
            //else if (SecondObject && SecondObject.GetComponent<SphereInfo>().ObjectSide == CheckSide)
            //{
            //    SecondObject.SetActive(false);
            //}
        }

        public void TurnOnAllObjects()
        {
            FirstObject.SetActive(true);
            if (SecondObject)
            {
                SecondObject.SetActive(true);
            }
        }

        private void ChangeObjectColor(Color color)
        {
            FirstObject.GetComponent<SpriteRenderer>().color = color;
            if (SecondObject)
            {
                SecondObject.GetComponent<SpriteRenderer>().color = color;
            }
        }

    }

    #endregion

}