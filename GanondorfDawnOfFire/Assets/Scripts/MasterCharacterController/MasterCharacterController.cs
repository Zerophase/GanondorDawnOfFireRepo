using UnityEngine;
using System.Collections;

public class MasterCharacterController
{
    
    public static void TwoD_Camera()
    {
        createCamera();

        if(!Camera.main.isOrthoGraphic)
        {
            Camera.main.orthographic = true;
            Camera.main.orthographicSize = 5;
            Camera.main.depth = -1;
        }
    }

    public static void ThreeD_Camera()
    {
        createCamera();

        if(Camera.main.isOrthoGraphic)
        {
            Camera.main.orthographic = false;
            Camera.main.fieldOfView = 60;
        }
    }

    private static void createCamera()
    {
        GameObject tempCamera;
        if (Camera.main != null)
            tempCamera = Camera.main.gameObject;
        else if(Camera.main == null)
        {
            tempCamera = new GameObject("MainCamera");
            tempCamera.AddComponent<Camera>();
            tempCamera.tag = "MainCamera";
        }
    }
}
