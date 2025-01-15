using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereInfo : MonoBehaviour
{
    [Tooltip("Name Of Part")]
    public List<int> blendListIndex;
    public GameObject SiblingPoint;
}

public enum AxisType
{
    X_Axis,
    Y_Axis,
    X_Axis_andY_Axis
}

public enum Side
{
    Left,
    Right,
    Middle
}