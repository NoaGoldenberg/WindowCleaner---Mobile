using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private RectTransform safeAreaRectTransform;
    [SerializeField] private RectTransform unsafeAreaRectTransform;

    void Awake()
    {
        safeAreaRectTransform = GetComponent<RectTransform>();
        Debug.Log(safeAreaRectTransform);
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        safeAreaRectTransform.anchorMin = anchorMin;
        safeAreaRectTransform.anchorMax = anchorMax;
        ApplyUnsafeArea(anchorMax.y);
        
    }

    void ApplyUnsafeArea(float safeAreaMaxY)
    {
        unsafeAreaRectTransform.anchorMin = new Vector2(0, safeAreaMaxY);
        unsafeAreaRectTransform.anchorMax = new Vector2(1, 1);
    }
}