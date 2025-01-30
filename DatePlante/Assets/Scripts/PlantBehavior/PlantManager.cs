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
    public TMP_Dropdown wateringCanOptions;
    public Transform[] plantStates;
    public ParticleSystem growUpVfx;
    public ThemesList chosenTheme;
    public QuestionTypes chosenType;
    public WateringCanModule wateringCanModule;

    public Transform flowerTransform;

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

    public void ValidateWateringCanChoice(QuestionTypes ChosenType)
    {
        chosenType = ChosenType;
        //wateringCanModule.gameObject.SetActive(false);
        //wateringCanOptions.gameObject.SetActive(false);
        PlayerInputManager.instance.objectToDrag = null;
        GrowPlant();
        if (OnQuestionTypeValidate != null)
            OnQuestionTypeValidate.Invoke(false);
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