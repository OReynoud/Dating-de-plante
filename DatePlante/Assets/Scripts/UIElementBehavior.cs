using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class UIElementBehavior : MonoBehaviour
{
    public bool listenToKeyboard;
    public bool listenToWateringStart;
    public bool listenToWateringEnd;
    public bool listenToFertilizingStart;
    public bool listenToFertilizingEnd;
    [BoxGroup("Behavior")] public bool useRectTransform;

    [BoxGroup("Behavior")] [ShowIf("useRectTransform")]
    public RectTransform elementTransform;
    [BoxGroup("Behavior")] [ShowIf("useRectTransform")]
    public float heightScalePercent;
    [BoxGroup("Behavior")] [ShowIf("useRectTransform")]
    public float widthScalePercent;

    [BoxGroup("Behavior")] public AnimationCurve showElementCurve;
    [BoxGroup("Behavior")] public AnimationCurve hideElementCurve;
    [BoxGroup("Behavior")] public float heightPosPercent;
    [BoxGroup("Behavior")] public float widthPosPercent;
    [BoxGroup("Behavior")] public float keyboardCurveTimer;
    [BoxGroup("Behavior")] public bool movementComplete;
    [BoxGroup("Behavior")] public AnchorType anchor;
    [BoxGroup("Behavior")] private Vector2 elementShownPos;
    [BoxGroup("Behavior")] private Vector2 elementHiddenPos;

    [BoxGroup("Debug")] public bool showElement;
    private Vector2 screenDimensions;
    void Start()
    {
        screenDimensions = new Vector2(Display.main.systemWidth,Display.main.systemHeight);
        if (useRectTransform)
        {
            elementTransform.sizeDelta = new Vector2(screenDimensions.x * widthScalePercent, screenDimensions.y * heightScalePercent);
        }
        elementShownPos = useRectTransform
            ? new Vector2(screenDimensions.x * widthPosPercent, screenDimensions.y * heightPosPercent)
            : Camera.main.ScreenToWorldPoint(new Vector3(screenDimensions.x * widthPosPercent, screenDimensions.y * heightPosPercent, -10));

        switch (anchor)
        {
            case AnchorType.Right:
                elementHiddenPos = useRectTransform
                    ? new Vector2(screenDimensions.x + screenDimensions.x * (1 - widthPosPercent), screenDimensions.y * heightPosPercent)
                    : Camera.main.ScreenToWorldPoint(new Vector3(screenDimensions.x + screenDimensions.x * (1 - widthPosPercent), screenDimensions.y * heightPosPercent, -10));
                break;
            case AnchorType.Left:
                elementHiddenPos = useRectTransform
                    ? new Vector2(-screenDimensions.x * (1 - widthPosPercent), screenDimensions.y * heightPosPercent)
                    : Camera.main.ScreenToWorldPoint(new Vector3(-screenDimensions.x * (1 - widthPosPercent), screenDimensions.y * heightPosPercent, -10));
                break;
            case AnchorType.Up:
                elementHiddenPos = useRectTransform
                    ? new Vector2(screenDimensions.x * widthPosPercent, screenDimensions.y + screenDimensions.y * (1 - heightPosPercent))
                    : Camera.main.ScreenToWorldPoint(new Vector3(screenDimensions.x * widthPosPercent, screenDimensions.y + screenDimensions.y * (1 - heightPosPercent), -10));
                break;
            case AnchorType.Down:
                elementHiddenPos = useRectTransform
                    ? new Vector2(screenDimensions.x * widthPosPercent, -screenDimensions.y * heightPosPercent)
                    : Camera.main.ScreenToWorldPoint(new Vector3(screenDimensions.x * widthPosPercent, -screenDimensions.y * heightPosPercent, -10));
                break;
        }

        keyboardCurveTimer = hideElementCurve.keys[^1].time - Time.deltaTime;
        MainUIManager.instance.KeyboardEvent.AddListener(HandleKeyboardEvent);
        PlantManager.instance.OnQuestionTypeValidate.AddListener(HandleQuestionTypeValidation);
        PlantManager.instance.OnQuestionTypeEnter.AddListener(HandleQuestionTypeEnter);
        
        PlantManager.instance.OnQuestionThemeEnter.AddListener(HandleQuestionThemeEnter);
        PlantManager.instance.OnQuestionThemeValidate.AddListener(HandleQuestionThemeValidation);
    }



    private Vector2 aimedPosition;

    // Update is called once per frame
    void Update()
    {
        if (showElement && !movementComplete)
        {
            if (keyboardCurveTimer < showElementCurve.keys[^1].time)
            {
                keyboardCurveTimer += Time.deltaTime;
                aimedPosition = Vector2.LerpUnclamped(elementHiddenPos, elementShownPos,
                    showElementCurve.Evaluate(keyboardCurveTimer));
            }
            else
            {
                movementComplete = true;
            }
        }
        else
        {
            if (keyboardCurveTimer < hideElementCurve.keys[^1].time)
            {
                keyboardCurveTimer += Time.deltaTime;
                aimedPosition = Vector2.LerpUnclamped(elementShownPos, elementHiddenPos,
                    hideElementCurve.Evaluate(keyboardCurveTimer));
            }
            else
            {
                movementComplete = true;
                Start();
            }
        }

        if (!movementComplete)
        {
            if (useRectTransform)
            {
                elementTransform.anchoredPosition = aimedPosition;
            }
            else
            {
                transform.position = aimedPosition;
            }
        }

    }

    private void HandleQuestionTypeValidation(bool arg0)
    {
        if (!listenToWateringEnd)
            return;

        keyboardCurveTimer = 0;
        movementComplete = false;
        showElement = arg0;
        elementShownPos = useRectTransform ? elementTransform.anchoredPosition : transform.position;
    }

    private void HandleQuestionTypeEnter(bool arg0)
    {
        if (!listenToWateringStart)
            return;

        keyboardCurveTimer = 0;
        movementComplete = false;
        showElement = arg0;
    }
    private void HandleQuestionThemeValidation(bool arg0)
    {
        if (!listenToFertilizingStart)
            return;

        keyboardCurveTimer = 0;
        movementComplete = false;
        showElement = arg0;
        elementShownPos = useRectTransform ? elementTransform.anchoredPosition : transform.position;
    }

    private void HandleQuestionThemeEnter(bool arg0)
    {
        if (!listenToFertilizingEnd)
            return;

        keyboardCurveTimer = 0;
        movementComplete = false;
        showElement = arg0;
    }
    private void HandleKeyboardEvent(bool arg0)
    {
        if (!listenToKeyboard)
            return;
    }
}