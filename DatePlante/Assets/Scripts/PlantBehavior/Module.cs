using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class Module : MonoBehaviour
{
    [BoxGroup("References")] public ParticleSystem vfx;
    
    [BoxGroup("Behavior")] public float timeToValidateChoice;
    [HorizontalLine]
    [BoxGroup("Behavior")] public float xPosBuffer;
    [BoxGroup("Behavior")] public float xPosOffset;
    [BoxGroup("Behavior")] public float pouringRotation;
    [BoxGroup("Behavior")] [Range(0,1)]public float rotationLerp;
    
    [Foldout("Debug")] public float aimedRotation;
    [Foldout("Debug")] public float pouringTimer;

    public bool isValidated;
    
    public float DistanceFromFlower()
    {
        return Mathf.Abs((PlantManager.instance.flowerTransform.position.x + xPosOffset) - transform.position.x);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
