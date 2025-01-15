using UnityEngine;
using UnityEngine.UI;
 
[ExecuteInEditMode]
[RequireComponent(typeof(HorizontalLayoutGroup))]
public class AdjustHorizontalLayoutCellSize : MonoBehaviour
{
    public enum Axis { X, Y };
    public enum RatioMode { Free, Fixed };

    [SerializeField] Axis expand;
    [SerializeField] RatioMode ratioMode;
    [SerializeField] float cellRatio = 1;

    private RectTransform transform;
    private HorizontalLayoutGroup horizontalLayout;

    void Awake()
    {
        transform = (RectTransform)base.transform;
        horizontalLayout = GetComponent<HorizontalLayoutGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateCellSize();
    }

    void OnRectTransformDimensionsChange()
    {
        UpdateCellSize();
    }

#if UNITY_EDITOR
    [ExecuteAlways]
    void Update()
    {
        UpdateCellSize();
    }
#endif

    void OnValidate()
    {
        transform = (RectTransform)base.transform;
        horizontalLayout = GetComponent<HorizontalLayoutGroup>();
        UpdateCellSize();
    }

    void UpdateCellSize()
    {
        if (!horizontalLayout)
            return;

        int itemCount = transform.childCount;

        if (itemCount == 0)
            return;

        float spacing = (itemCount - 1) * horizontalLayout.spacing;

        if (expand == Axis.X)
        {
            float contentWidth = transform.rect.width - horizontalLayout.padding.left - horizontalLayout.padding.right - spacing;
            float sizePerItem = contentWidth / itemCount;
            horizontalLayout.childForceExpandWidth = false;
            horizontalLayout.childControlWidth = false;
            horizontalLayout.childScaleWidth = false;
            horizontalLayout.childForceExpandHeight = false;
            horizontalLayout.childControlHeight = false;
            horizontalLayout.childScaleHeight = false;
            horizontalLayout.spacing = (int)horizontalLayout.spacing;
            horizontalLayout.childAlignment = TextAnchor.UpperLeft;
            for (int i = 0; i < itemCount; i++)
            {
                RectTransform child = transform.GetChild(i) as RectTransform;
                child.sizeDelta = new Vector2(ratioMode == RatioMode.Free ? child.sizeDelta.x : sizePerItem * cellRatio, child.sizeDelta.y);
            }
        }
        else //if (expand == Axis.Y)
        {
            // Adjust for the Y axis as needed.
        }
    }
}