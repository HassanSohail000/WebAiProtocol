using UnityEngine;
using UnityEngine.UI;
 
[ExecuteInEditMode]
[RequireComponent(typeof(GridLayoutGroup))]
public class AdjustGridLayoutCellSize : MonoBehaviour
{
    public enum Axis { X, Y };
    public enum RatioMode { Free, Fixed };
   
    [SerializeField] Axis expand;
    [SerializeField] RatioMode ratioMode;
    [SerializeField] float cellRatio = 1;
 
    RectTransform _recTransform;
    GridLayoutGroup grid;
 
    void Awake()
    {
        _recTransform = (RectTransform)base.transform;
        grid = GetComponent<GridLayoutGroup>();
    }
 
    void Start()
    {
        if (ConstantsHolder.xanaConstants!=null && ConstantsHolder.xanaConstants.screenType == ConstantsHolder.ScreenType.TabScreen)
            ratioMode = RatioMode.Fixed;
        UpdateCellSize();
    }
 /*
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
        grid = GetComponent<GridLayoutGroup>();
        UpdateCellSize();
    }
 */
 
    void UpdateCellSize()
    {
        if (!grid)
            return;
        var count = grid.constraintCount;
        if (expand == Axis.X)
        {
            float spacing = (count - 1) * grid.spacing.x;
            float contentSize = _recTransform.rect.width - grid.padding.left - grid.padding.right - spacing;
            float sizePerCell = contentSize / count;
            grid.cellSize = new Vector2(sizePerCell, ratioMode == RatioMode.Free ? grid.cellSize.y : sizePerCell * cellRatio);
           
        }
        else //if (expand == Axis.Y)
        {
            float spacing = (count - 1) * grid.spacing.y;
            float contentSize = _recTransform.rect.height - grid.padding.top - grid.padding.bottom -spacing;
            float sizePerCell = contentSize / count;
            grid.cellSize = new Vector2(ratioMode == RatioMode.Free ? grid.cellSize.x : sizePerCell * cellRatio, sizePerCell);
        }
    }
}