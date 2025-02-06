using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance;
    public ThemesList chosenTheme;
    public QuestionTypes chosenType;

    [Foldout("References")] public GameObject newPlantButton;
    [Foldout("References")] public TMP_Dropdown wateringCanOptions;
    [Foldout("References")] public Transform[] plantStates;
    [Foldout("References")] public QuestionPlantStorage[] questionPlants;
    [Foldout("References")] public ParticleSystem growUpVfx;
    [Foldout("References")] public WateringCanModule wateringCanModule;
    [Foldout("References")] public FertilizerModule fertilizerModule;
    [Foldout("References")] public TextMeshProUGUI questionText;
    [Foldout("References")] public QuestionListSo allQuestions;
    [Foldout("References")] public Transform flowerTransform;
    [Foldout("References")] public Transform potTransform;
    [Foldout("References")] public Vector3 potOffset;

    public float rotationSpeedMultiplier;
    [Range(0, 1)] public float animSpeed = 0.05f;
    [Range(0, 1)] public float defaultYLerp;
    [Range(0, 1)] public float keyboardYLerp;

    private float[] questionPlantsTimers;
    private float maxPos;
    private float minPos;
    [Range(0, 1)] private float lerpYPos;
    [Range(0, 1)] private float aimedYLerp;
    public bool isRotating { get; set; }
    private float spinValue;

    [BoxGroup("Growth Behavior")] private float growthAnimTimer;
    [BoxGroup("Growth Behavior")] public TextMeshProUGUI cooldownText;
    [BoxGroup("Growth Behavior")] public AnimationCurve growthAnim;

    [Foldout("Debug")] public float screenXPosPercent;
    [Foldout("Debug")] public int growthState;
    [Foldout("Debug")] public int fullyGrownPlants;
    [Foldout("Debug")] public float cooldownTimer;
    [Foldout("Debug")] public bool cooldownActive;

    public UnityEvent<bool> OnQuestionTypeValidate { get; set; } = new();
    public UnityEvent<bool> OnQuestionTypeEnter { get; set; } = new();
    public UnityEvent<bool> OnQuestionThemeValidate { get; set; } = new();
    public UnityEvent<bool> OnQuestionThemeEnter { get; set; } = new();

    private List<QuestionSo> tempQuestionsHolder = new List<QuestionSo>();

    


    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        minPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -10)).x;
        maxPos = Camera.main.ScreenToWorldPoint(new Vector3( Display.main.systemWidth, 0, -10)).x;
        
        flowerTransform.position = new Vector3(Mathf.Lerp(minPos, maxPos, screenXPosPercent),
            flowerTransform.position.y, flowerTransform.position.z);
        
        maxPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Display.main.systemHeight, -10)).y;
        minPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -10)).y;
        aimedYLerp = defaultYLerp;
        MainUIManager.instance.KeyboardEvent.AddListener(HandleKeyboardEvent);
        PlayerInputManager.instance.OnTouchRelease.AddListener(PlantReleaseSpin);

        questionPlantsTimers = new float[questionPlants.Length];
    }

    private void HandleKeyboardEvent(bool shown)
    {
        aimedYLerp = shown ? keyboardYLerp : defaultYLerp;
    }

    private void FixedUpdate()
    {
        GrowthUpdate();
        PlantScaleAndPos();
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.fixedDeltaTime;

            cooldownText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", 
                Mathf.FloorToInt(cooldownTimer / 3600) % 24,
                Mathf.FloorToInt(cooldownTimer * 0.016666f) % 60, 
                Mathf.FloorToInt(cooldownTimer) % 60);
        }
        else if(cooldownActive)
        {
            questionText.transform.parent.gameObject.SetActive(false);
            MainUIManager.instance.cooldownResetButton.interactable = false;
            cooldownActive = false;
            newPlantButton.SetActive(true);
            chosenTheme = ThemesList.None;
            chosenType = QuestionTypes.None;
            wateringCanModule.isValidated = false;
            fertilizerModule.isValidated = false;
            cooldownText.text = "New plant is ready!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlantRotation();
    }

    private void GrowthUpdate()
    {
        if (growthAnimTimer < growthAnim.keys[^1].time)
        {
            growthAnimTimer += Time.fixedDeltaTime;
            if (growthState >= 0)
            {
                plantStates[growthState].localScale = Vector3.one * growthAnim.Evaluate(growthAnimTimer);
            }
        }

        if (fullyGrownPlants > 0)
        {
            if (questionPlantsTimers[fullyGrownPlants - 1] < growthAnim.keys[^1].time)
            {
                questionPlantsTimers[fullyGrownPlants - 1] += Time.fixedDeltaTime;
                questionPlants[fullyGrownPlants - 1].transform.localScale =
                    Vector3.one * growthAnim.Evaluate(questionPlantsTimers[fullyGrownPlants - 1]);
            }
        }
    }

    private void PlantScaleAndPos()
    {
        flowerTransform.position = Vector3.Lerp(
            new Vector3(flowerTransform.position.x, minPos, flowerTransform.position.z),
            new Vector3(flowerTransform.position.x, maxPos, flowerTransform.position.z), lerpYPos);

        potTransform.position = flowerTransform.position + potOffset;

        lerpYPos = Mathf.Lerp(lerpYPos, aimedYLerp, animSpeed);
    }

    private void PlantRotation()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
                transform.eulerAngles.y +
                (PlayerInputManager.instance.previousPos.x - PlayerInputManager.instance.touchWorldPos.x) *
                rotationSpeedMultiplier
                , transform.eulerAngles.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
                transform.eulerAngles.y + spinValue
                , transform.eulerAngles.z);
            spinValue = Mathf.Lerp(spinValue, 0, 0.01f);
        }
    }

    public void ChooseWateringCan()
    {
        wateringCanModule.UpdateWateringCanType((QuestionTypes)wateringCanOptions.value + 1);
    }

    public void ChooseFertilizer(int index)
    {
        fertilizerModule.UpdateFertilizerType((ThemesList)index);
    }

    public void ValidateWateringCanChoice(QuestionTypes ChosenType)
    {
        chosenType = ChosenType;
        PlayerInputManager.instance.objectToDrag = null;
        GrowPlant();
        if (OnQuestionTypeValidate != null)
            OnQuestionTypeValidate.Invoke(false);
        if (chosenTheme == ThemesList.None)
            MainUIManager.instance.fertilizerButton.SetActive(true);


        if (chosenType != QuestionTypes.None && chosenTheme != ThemesList.None)
            AskQuestion();
    }

    public void ValidateFertilizerChoice(ThemesList ChosenTheme)
    {
        chosenTheme = ChosenTheme;
        PlayerInputManager.instance.objectToDrag = null;
        GrowPlant();
        if (OnQuestionThemeValidate != null)
            OnQuestionThemeValidate.Invoke(false);
        if (chosenType == QuestionTypes.None)
            MainUIManager.instance.wateringButton.SetActive(true);

        if (chosenType != QuestionTypes.None && chosenTheme != ThemesList.None)
            AskQuestion();
    }

    private void AskQuestion()
    {
        tempQuestionsHolder.Clear();
        foreach (var question in allQuestions.allQuestions)
        {
            if (question.questionTheme != chosenTheme)
                continue;
            if (question.questionType != chosenType)
                continue;
            tempQuestionsHolder.Add(question);
        }

        int rand = Random.Range(0, tempQuestionsHolder.Count);
        questionText.transform.parent.gameObject.SetActive(true);
        questionText.text = tempQuestionsHolder[rand].questionContent;
        MainUIManager.instance.cooldownResetButton.interactable = true;
        StartCooldown();
    }

    private void StartCooldown()
    {
        cooldownTimer = 24 * 60 * 60 - 1;
        cooldownText.transform.parent.gameObject.SetActive(true);
        cooldownActive = true;
    }

    public void PlantReleaseSpin()
    {
        if (!isRotating)
            return;
        isRotating = false;
        spinValue = (PlayerInputManager.instance.previousPos.x - PlayerInputManager.instance.touchWorldPos.x) *
                    rotationSpeedMultiplier;
    }

    public void StartWatering()
    {
        if (OnQuestionTypeEnter != null)
            OnQuestionTypeEnter.Invoke(true);
        ChooseWateringCan();
    }

    public void StartFertilizing()
    {
        if (OnQuestionThemeEnter != null)
            OnQuestionThemeEnter.Invoke(true);
        ChooseFertilizer(1);
    }

    public void GrowPlant()
    {
        if (growthState == plantStates.Length - 1)
        {
            plantStates[growthState].gameObject.SetActive(false);
            growthState = -1;
            questionPlants[fullyGrownPlants].gameObject.SetActive(true);
            fullyGrownPlants++;
            return;
        }

        growthState++;
        plantStates[growthState].gameObject.SetActive(true);
        if (growthState > 0)
        {
            plantStates[growthState - 1].gameObject.SetActive(false);
        }

        growUpVfx.Play();
        growthAnimTimer = 0;
    }
}