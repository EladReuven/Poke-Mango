using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraView : MonoBehaviour
{
    WebCamTexture backCam;
    Texture defaultBackground;
    WebCamDevice[] backCamDevArr;
    bool camAvailable;
    int camToggle = 0;
    float ratio;
    float scaleY;
    int orient;


    public RawImage background;
    public AspectRatioFitter ARFit;
    public TextMeshProUGUI camStatus;
    


    void Start()
    {
        defaultBackground = background.texture;
        InitBackCamDevices();
        if(camAvailable)
        {
            camStatus.text = "Camera Available";
            //backCam = backCamTextures[camToggle];
            backCam.Play();
            background.texture = backCam;
        }
        else
        {
            camStatus.text = "No Cameras Available";
        }
    }


    void Update()
    {
        if (!camAvailable)
            return;
        OperateCamera();
    }

    private void OperateCamera()
    {
        camStatus.text = "Current camera: " + (camToggle + 1).ToString();
        background.texture = backCam;
        
        ratio = (float)backCam.width / (float)backCam.height;
        ARFit.aspectRatio = ratio;

        scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        orient = -backCam.videoRotationAngle;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
        background.rectTransform.localEulerAngles = new Vector3(0f, 0f, orient);
    }

    private void InitBackCamDevices()
    {
        backCamDevArr = WebCamTexture.devices;
        WebCamTexture currentDevice;

        camStatus.text = backCamDevArr.Length.ToString(); ;
        int count = 0;
        for(int i = 0; i < backCamDevArr.Length; i++)
        {
            if (!backCamDevArr[i].isFrontFacing)
            {
                count++;
                currentDevice = new WebCamTexture(backCamDevArr[i].name, Screen.width, Screen.height);
                backCam = currentDevice;
            }
        }

        camAvailable = count > 0 ? true : false;
        camStatus.text = camAvailable ? "CamAvailable": "CamNotAvailable";

    }
}
