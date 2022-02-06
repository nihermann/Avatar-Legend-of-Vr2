using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
 
public class ScreenshotHandler: MonoBehaviour
{
    public Camera myCamera;
    private static ScreenshotHandler instance;

    private bool takeScreenshotOnNextFrame;

    private string path;
    
    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
        
    }

    private void Update()
    {

        if (takeScreenshotOnNextFrame)
        {
            
            Debug.Log("In post render");
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult =
                new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            Byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/"+path+".png", byteArray);
            Debug.Log("Image was saved at: "+ Application.dataPath + "/"+path+".png");

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
            

        }
    }

    private void TakeScreenshot(int width, int height, string fileName){
        Debug.Log("Made a Screenshot");
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
        path = fileName;
    }

    public static void TakeScreenshot_Static(int width, int height, string fileName)
    {
        instance.TakeScreenshot(width, height, fileName);
    }
}

#endif