using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Screen = UnityEngine.Device.Screen;

public class MainUIManager : MonoBehaviour
{
    public static MainUIManager instance;
    [BoxGroup("Keyboard")]public RectTransform keyboardTransform;
    [BoxGroup("Keyboard")]public AnimationCurve showKeyboardCurve;
    [BoxGroup("Keyboard")]public AnimationCurve hideKeyboardCurve;
    [Range(0,1)][BoxGroup("Keyboard")]public float heightPercent;
    [BoxGroup("Keyboard")]public float keyboardCurveTimer;
    [BoxGroup("Keyboard")]private bool showKeyboard;
    [BoxGroup("Keyboard")]private Vector2 keyboardShownPos;
    [BoxGroup("Keyboard")]private Vector2 keyboardHiddenPos;

    public UnityEvent<bool> KeyboardEvent { get; set; } = new();
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
        }

        instance = this;
    }

    void Start()
    {
        keyboardTransform.sizeDelta = new Vector2(Screen.width, Screen.height * heightPercent);
        
        keyboardShownPos = Vector2.up * keyboardTransform.sizeDelta.y *0.5f;
        keyboardHiddenPos = Vector2.down * keyboardTransform.sizeDelta.y *0.5f;

        keyboardCurveTimer = hideKeyboardCurve.keys[^1].time - Time.deltaTime;

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
                keyboardAimedPosition = Vector2.LerpUnclamped(keyboardShownPos,keyboardHiddenPos,hideKeyboardCurve.Evaluate(keyboardCurveTimer));
            }
        }

        keyboardTransform.anchoredPosition = keyboardAimedPosition;
    }

    [Button]
    public void ShowKeyboard()
    {
        showKeyboard = true;
        keyboardCurveTimer = 0;
        if (KeyboardEvent != null)
            KeyboardEvent.Invoke(true);
        
    }
    
    [Button]
    public void HideKeyboard()
    {
        showKeyboard = false;
        keyboardCurveTimer = 0;
        if (KeyboardEvent != null)
            KeyboardEvent.Invoke(false);
    }
}
