using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

public class MainUIManager : MonoBehaviour
{
    [BoxGroup("Keyboard")]public RectTransform keyboardTransform;
    [BoxGroup("Keyboard")]public AnimationCurve showKeyboardCurve;
    [BoxGroup("Keyboard")]public AnimationCurve hideKeyboardCurve;
    [Range(0,1)][BoxGroup("Keyboard")]public float heightPercent;
    [BoxGroup("Keyboard")]public float keyboardCurveTimer;
    [BoxGroup("Keyboard")]private bool showKeyboard;
    [BoxGroup("Keyboard")]private Vector2 keyboardShownPos;
    [BoxGroup("Keyboard")]private Vector2 keyboardHiddenPos;
    // Start is called before the first frame update
    void Start()
    {
        keyboardTransform.sizeDelta = new Vector2(Screen.width, Screen.height * heightPercent);
        
        keyboardShownPos = Vector2.up * keyboardTransform.sizeDelta.y *0.5f;
        keyboardHiddenPos = Vector2.down * keyboardTransform.sizeDelta.y *0.5f;
        
    }

    private Vector2 keyboardAimedPosition;
    // Update is called once per frame
    void Update()
    {
        if (showKeyboard)
        {
            if (keyboardCurveTimer < showKeyboardCurve.keys[^1].time)
            {
                keyboardCurveTimer += Time.deltaTime;
                keyboardAimedPosition = Vector2.LerpUnclamped(keyboardHiddenPos,keyboardShownPos,showKeyboardCurve.Evaluate(keyboardCurveTimer));
            }
        }
        else
        {
            if (keyboardCurveTimer < hideKeyboardCurve.keys[^1].time)
            {
                keyboardCurveTimer += Time.deltaTime;
                keyboardAimedPosition = Vector2.LerpUnclamped(keyboardShownPos,keyboardHiddenPos,showKeyboardCurve.Evaluate(keyboardCurveTimer));
            }
        }

        keyboardTransform.anchoredPosition = keyboardAimedPosition;
    }

    [Button]
    public void ShowKeyboard()
    {
        showKeyboard = true;
        keyboardCurveTimer = 0;
    }
    
    [Button]
    public void HideKeyboard()
    {
        showKeyboard = false;
        keyboardCurveTimer = 0;
    }
}
