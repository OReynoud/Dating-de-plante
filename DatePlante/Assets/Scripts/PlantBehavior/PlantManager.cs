using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance;
    [BoxGroup("References")]public TMP_Dropdown wateringCanOptions;
    [BoxGroup("References")]public Transform[] plantStates;
    [BoxGroup("References")]public ParticleSystem growUpVfx;
    public ThemesList chosenTheme;
    public QuestionTypes chosenType;
    [BoxGroup("References")]public WateringCanModule wateringCanModule;
    [BoxGroup("References")]public FertilizerModule fertilizerModule;
    [BoxGroup("References")]public TextMeshProUGUI questionText;
    [BoxGroup("References")]public QuestionListSo allQuestions;
   

    [BoxGroup("References")]public Transform flowerTransform;
    
    [BoxGroup("References")]public Transform potTransform;
    
    [BoxGroup("References")]public Vector3 potOffset;

    public float rotationSpeedMultiplier;
    [Range(0, 1)] public float animSpeed = 0.05f;
    [Range(0, 1)] public float defaultYLerp;
    [Range(0, 1)] public float keyboardYLerp;

    
    private float maxYPos;
    private float minYPos;
    [Range(0, 1)] private float lerpYPos;
    [Range(0, 1)] private float aimedYLerp;
    public bool isRotating { get; set; }
    private float spinValue;
    
    [BoxGroup("Growth Behavior")] public AnimationCurve growthAnim;
    [BoxGroup("Growth Behavior")] public int growthState;    
    [BoxGroup("Growth Behavior")] private float growthAnimTimer;

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
        maxYPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Display.main.systemHeight, -10)).y;
        minYPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -10)).y;
        aimedYLerp = defaultYLerp;
        MainUIManager.instance.KeyboardEvent.AddListener(HandleKeyboardEvent);
        PlayerInputManager.instance.OnTouchRelease.AddListener(PlantReleaseSpin);
    }

    private void HandleKeyboardEvent(bool shown)
    {
        aimedYLerp = shown ? keyboardYLerp : defaultYLerp;
    }

    // Update is called once per frame
    void Update()
    {
        flowerTransform.position = Vector3.Lerp(
            new Vector3(flowerTransform.position.x, minYPos, flowerTransform.position.z),
            new Vector3(flowerTransform.position.x, maxYPos, flowerTransform.position.z), lerpYPos);
        
        potTransform.position = flowerTransform.position + potOffset;

        lerpYPos = Mathf.Lerp(lerpYPos, aimedYLerp, animSpeed);
        if (isRotating)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
                transform.eulerAngles.y + (PlayerInputManager.instance.previousPos.x - PlayerInputManager.instance.touchWorldPos.x) * rotationSpeedMultiplier
                , transform.eulerAngles.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
                transform.eulerAngles.y + spinValue
                , transform.eulerAngles.z);
            spinValue = Mathf.Lerp(spinValue, 0, 0.01f);
        }

        if (growthAnimTimer < growthAnim.keys[^1].time)
        {
            growthAnimTimer += Time.deltaTime;
            if (growthState>=0)
            {
                plantStates[growthState].localScale = Vector3.one * growthAnim.Evaluate(growthAnimTimer);
            }
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
    }

    public void PlantReleaseSpin()
    {
        if (!isRotating)
            return;
        isRotating = false;
        spinValue = (PlayerInputManager.instance.previousPos.x - PlayerInputManager.instance.touchWorldPos.x) * rotationSpeedMultiplier;
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
            return;
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