using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementBehavior : MonoBehaviour
{
    
    void Start()
    {
        MainUIManager.instance.KeyboardEvent.AddListener(HandleKeyboardEvent);
    }

    private void HandleKeyboardEvent(bool arg0)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
