using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public int width;
    public int height;

    public string path;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            #if UNITY_EDITOR
            ScreenshotHandler.TakeScreenshot_Static(width,height, path);
            #endif
        }
    }
}
