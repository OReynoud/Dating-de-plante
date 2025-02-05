using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCam;
    Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    private void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            camAvailable = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                break;
            }
        }

        if(backCam == null)
        {
            Debug.Log("Unable to find back Camera");
            return;
        }

        backCam.Play();
        background.texture = backCam;

        camAvailable = true;
        StartCoroutine(AdjustAspectRatio());
    }

    private IEnumerator AdjustAspectRatio()
    {
        // Attendre que la caméra ait commencé à fonctionner
        while (backCam.width <= 16 || backCam.height <= 16)
        {
            yield return null;
        }

        // Une fois les dimensions disponibles, ajuster le ratio
        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        Debug.Log($"Camera resolution: {backCam.width}x{backCam.height}, Ratio: {ratio}");
    }


    // Update is called once per frame
    private void Update()
    {
        if (!camAvailable)
            return;

        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? 1f : -1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0,0,orient);
    }

    public void ToggleCameraMode()
    {
        if (background.texture != defaultBackground)
        {
            // Désactiver la caméra
            backCam.Stop();
            background.texture = defaultBackground;
            Debug.Log("Camera stopped");
        }
        else
        {
            // Activer la caméra
            backCam.Play();
            background.texture = backCam;
            Debug.Log("Camera started");
        }
    }

}

