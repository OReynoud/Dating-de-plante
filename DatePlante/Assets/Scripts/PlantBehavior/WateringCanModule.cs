using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class WateringCanModule : MonoBehaviour
{
    [BoxGroup("References")] public TextMeshProUGUI canText;
    [BoxGroup("References")] public ParticleSystem water;
    
    [BoxGroup("Behavior")] public float timeToValidateChoice;
    [HorizontalLine]
    [BoxGroup("Behavior")] public float xPosBuffer;
    [BoxGroup("Behavior")] public float xPosOffset;
    [BoxGroup("Behavior")] public float pouringRotation;
    [BoxGroup("Behavior")] [Range(0,1)]public float rotationLerp;
    
    [Foldout("Debug")] public float aimedRotation;
    [Foldout("Debug")] public float wateringTimer;
    [Foldout("Debug")] public QuestionTypes currentType;

    private bool isValidated;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DistanceFromFlower() < xPosBuffer)
        {
            PourWater();
        }
        else
        {
            DontPourWater();
        }
        transform.rotation = Quaternion.Euler(0,0,aimedRotation);
    }

    void PourWater()
    {
        aimedRotation = Mathf.Lerp(aimedRotation, pouringRotation, rotationLerp);
        if (wateringTimer < timeToValidateChoice)
        {
            wateringTimer += Time.deltaTime;
        }
        else if (!isValidated)
        {
            isValidated = true;
            PlantManager.instance.ValidateWateringCanChoice(currentType);
        }
        
        if (!water.isPlaying) 
            water.Play();
        
        
    }

    void DontPourWater()
    {
        aimedRotation = Mathf.Lerp(aimedRotation, 0, rotationLerp);
        if (water.isPlaying)
            water.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    float DistanceFromFlower()
    {
        return Mathf.Abs((PlantManager.instance.flowerTransform.position.x + xPosOffset) - transform.position.x);
    }

    public void UpdateWateringCanType(QuestionTypes waterType)
    {
        wateringTimer = 0;
        currentType = waterType;
        canText.text = waterType.ToString();
    }
}
