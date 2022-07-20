using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Screenshoter : MonoBehaviour {


    public static void TakeScreenshot(string fileName) {

        DirectoryInfo screenshotDirectory = Directory.CreateDirectory("Screenshots");
        string fullPath = Path.Combine(screenshotDirectory.FullName, fileName);
    
        ScreenCapture.CaptureScreenshot(fullPath + ".png");

        Debug.Log("He hecho una foto con nombre " + fileName);
       
    }
}

