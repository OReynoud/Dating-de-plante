using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPlantStorage : MonoBehaviour
{
    public MeshRenderer mesh;
    public Material mat;
    private Material flowerMat;
    public ThemesList storedTheme;
    public QuestionTypes storedTime;
    
    // Start is called before the first frame update
    void Start()
    {
        mat = Instantiate(mesh.materials[1]);
        storedTheme = PlantManager.instance.chosenTheme;
        storedTime = PlantManager.instance.chosenType;
        mat.name = "Cloned mat";
        mat.color = MainUIManager.instance.fertilizerChoiceButtons[(int)PlantManager.instance.chosenTheme - 1].color;
        mesh.materials = new []{mesh.materials[0], mat};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
