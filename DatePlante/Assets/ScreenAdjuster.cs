using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAdjuster : MonoBehaviour
{
    private RectTransform elementTransform;    
    public float heightScalePercent;
    public float widthScalePercent;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 screenDimensions = new Vector2(Display.main.systemWidth,Display.main.systemHeight);
        elementTransform.sizeDelta = new Vector2(screenDimensions.x * widthScalePercent, screenDimensions.y * heightScalePercent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
