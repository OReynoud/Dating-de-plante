using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class WateringCanModule : Module
{

    
    [BoxGroup("References")] public TextMeshProUGUI canText;
    [Foldout("Debug")] public QuestionTypes currentType;
    
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
        if (pouringTimer < timeToValidateChoice)
        {
            pouringTimer += Time.deltaTime;
        }
        else if (!isValidated)
        {
            isValidated = true;
            PlantManager.instance.ValidateWateringCanChoice(currentType);
        }
        
        if (!vfx.isPlaying) 
            vfx.Play();
        
        
    }

    void DontPourWater()
    {
        aimedRotation = Mathf.Lerp(aimedRotation, 0, rotationLerp);
        if (vfx.isPlaying)
            vfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }



    public void UpdateWateringCanType(QuestionTypes waterType)
    {
        pouringTimer = 0;
        currentType = waterType;
        canText.text = waterType.ToString();
    }
}
