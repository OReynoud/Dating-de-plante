using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class FertilizerModule : Module
{
    public static FertilizerModule instance;
    [BoxGroup("References")] public Image icon;
    [BoxGroup("References")] public Sprite[] allThemeIcons;
    [BoxGroup("References")] public Transform topOfFertilizer;
    [BoxGroup("References")] public Transform startPoint;
    [BoxGroup("Behavior")] public float distanceToTriggerOpening;
    [Foldout("Debug")] public ThemesList currentTheme;
    [Foldout("Debug")] public bool openedFertilizer;
    [Foldout("Debug")] public bool isOpening;
    [Foldout("Debug")] public bool canOpen;
    [Foldout("Debug")] public float openingState;
    
    
    

    private UIElementBehavior componentUI;
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
        componentUI = GetComponent<UIElementBehavior>();
        
        PlayerInputManager.instance.OnTouchRelease.AddListener(ReleaseOpening);
    }

    private void ReleaseOpening()
    {
        if (openedFertilizer)
            return;
        openingState = 0;
        isOpening = false;
        canOpen = false;
    }

    void Update()
    {
        if (DistanceFromFlower() < xPosBuffer)
        {
            PourFertilizer();
        }
        else
        {
            DontPourFertilizer();
        }
        transform.rotation = Quaternion.Euler(0,0,aimedRotation);
        topOfFertilizer.rotation = Quaternion.Euler(0,0,Mathf.Lerp(0,-120,openingState));
        if (openingState >= 0.95f)
        {
            openingState = 1;
            openedFertilizer = true;
            isOpening = false;
        }

        if (isOpening)
        {
            if (!canOpen)
            {
                canOpen = Vector3.Distance(startPoint.position, PlayerInputManager.instance.touchWorldPos) <
                          distanceToTriggerOpening;
            }
            else
            {
                openingState = 1 - (topOfFertilizer.position.x - PlayerInputManager.instance.touchWorldPos.x) / topOfFertilizer.position.x;
            }
        }
    }

    void PourFertilizer()
    {
        aimedRotation = Mathf.Lerp(aimedRotation, pouringRotation, rotationLerp);
        if (pouringTimer < timeToValidateChoice)
        {
            pouringTimer += Time.deltaTime;
        }
        else if (!isValidated)
        {
            isValidated = true;
            PlantManager.instance.ValidateFertilizerChoice(currentTheme);
        }
        
        if (!vfx.isPlaying) 
            vfx.Play();
        
        
    }

    void DontPourFertilizer()
    {
        aimedRotation = Mathf.Lerp(aimedRotation, 0, rotationLerp);
        if (vfx.isPlaying)
            vfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }



    public void UpdateFertilizerType(ThemesList themeToChoose)
    {
        pouringTimer = 0;
        currentTheme = themeToChoose;
        icon.sprite = allThemeIcons[(int)themeToChoose - 1];
        openedFertilizer = false;
        openingState = 0;
        componentUI.keyboardCurveTimer = 0;
        componentUI.movementComplete = false;

    }
}
