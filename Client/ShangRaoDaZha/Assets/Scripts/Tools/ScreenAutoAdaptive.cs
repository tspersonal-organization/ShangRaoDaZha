using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAutoAdaptive : MonoBehaviour {

    Dictionary<string, float> ScreenView = new Dictionary<string, float>();

	void Start () {
        ScreenView["1280x720"] = 32f;
        ScreenView["640x1136"] = 86f;
        ScreenView["1136x640"] = 32f;
        ScreenView["1366x768"] = 32f;
        ScreenView["512x512"] = 55f;
        ScreenView["1280x720"] = 32f;
        ScreenView["1920x1080"] = 32f;
        ScreenView["2048x1536"] = 41f;
     
        // Resolution[] resolutions = Screen.resolutions;
        // foreach (Resolution res in resolutions)
        // {
        //   //  print(res.width + "x" + res.height);

        // }
        //// Screen.SetResolution(resolutions[0].width, resolutions[0].height, true);
        // Debug.Log(resolutions[0].width + "x" + resolutions[0].height);
        Debug.Log(Screen.width + "x" + Screen.height);
      //  Camera.main.fieldOfView = getCameraFOV(50);
       // SetBasicValues();
    }
	
	// Update is called once per frame
	void Update () {
        Camera.main.fieldOfView = ScreenView[Screen.width + "x" + Screen.height];

    }

    static public float getCameraFOV(float currentFOV)
    {
        UIRoot root = GameObject.FindObjectOfType<UIRoot>();
        float scale = Convert.ToSingle(root.manualHeight / 720f);
        return currentFOV * scale;
    }

    private float width;
    private float height;

    void SetBasicValues()
    {

        float leftBorder;
        float rightBorder;
        float topBorder;
        float downBorder;
        //the up right corner
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(Camera.main.transform.position.z)));

        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        rightBorder = cornerPos.x;
        topBorder = cornerPos.y;
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        width = rightBorder - leftBorder;
        height = topBorder - downBorder;
        Debug.Log(width + "x" + height);
    }
}
