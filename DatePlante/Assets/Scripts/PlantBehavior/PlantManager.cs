using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance;
    public TMP_Dropdown wateringCanOptions;
    public ThemesList chosenTheme;
    public QuestionTypes chosenType;
    public WateringCanModule wateringCanModule;

    public Transform flowerTransform;

    public float maxRotation;
    public float maxYPos;
    public float minYPos; 
    [Range(0,1)]public float animSpeed = 0.05f;
    [Range(0,1)]public float lerpYPos;
    [Range(0,1)]public float aimedYLerp;
    [Range(0,1)]public float defaultYLerp;
    [Range(0,1)]public float keyboardYLerp;
    
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
        maxYPos = Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.height, -10)).y;
        minYPos = Camera.main.ScreenToWorldPoint(new Vector3(0,0, -10)).y;
        aimedYLerp = defaultYLerp;
        MainUIManager.instance.KeyboardEvent.AddListener(HandleKeyboardEvent);
    }

    private void HandleKeyboardEvent(bool shown)
    {
        aimedYLerp = shown ? keyboardYLerp : defaultYLerp;
    }

    // Update is called once per frame
    void Update()
    {
        flowerTransform.position = Vector3.Lerp(new Vector3(flowerTransform.position.x,minYPos,flowerTransform.position.z),
            new Vector3(flowerTransform.position.x,maxYPos,flowerTransform.position.z), lerpYPos);

        lerpYPos = Mathf.Lerp(lerpYPos, aimedYLerp, animSpeed);
    }

    public void ChooseWateringCan()
    {
        wateringCanModule.UpdateWateringCanType((QuestionTypes)wateringCanOptions.value + 1);
    }

    public void ValidateWateringCanChoice(QuestionTypes ChosenType)
    {
        chosenType = ChosenType;
        wateringCanModule.gameObject.SetActive(false);
        wateringCanOptions.gameObject.SetActive(false);
    }
}
