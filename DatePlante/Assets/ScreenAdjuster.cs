using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAdjuster : MonoBehaviour
{
    private RectTransform elementTransform;    
    public float heightScalePercent;
    public float widthScalePercent;   
    public float heightPosPercent;
    public float widthPosPercent;
    
    private Vector2 elementShownPos;
    private Vector2 elementHiddenPos;
    // Start is called before the first frame update
    void Start()
    {
        elementTransform = GetComponent<RectTransform>();
        Vector2 screenDimensions = new Vector2(Display.main.systemWidth,Display.main.systemHeight);
        elementTransform.sizeDelta = new Vector2(screenDimensions.x * widthScalePercent, screenDimensions.y * heightScalePercent);
        elementTransform.anchoredPosition = new Vector2(screenDimensions.x * widthPosPercent, screenDimensions.y * heightPosPercent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
