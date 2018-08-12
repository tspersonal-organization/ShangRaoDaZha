using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camertest : MonoBehaviour {

    private float height = 720;
    private float width = 1280;
    private float rate1 = 720 / 1280;
    private float rate2;

    //// Use this for initialization  
    //void Start()
    //{
    //    //if (Screen.height != height && Screen.width != width)
    //    //{
    //    //    rate2 = Screen.height / (float)Screen.width;
    //    //    gameObject.GetComponent<Camera>().fieldOfView *= 1 + (rate2 - rate1);
    //    //}

    //    int ManualWidth = 1280;   //首先记录下你想要的屏幕分辨率的宽
    //    int ManualHeight = 720;   //记录下你想要的屏幕分辨率的高        //普通安卓的都是 1280*720的分辨率
    //    int manualHeight;

    //    //然后得到当前屏幕的高宽比 和 你自定义需求的高宽比。通过判断他们的大小，来不同赋值
    //    //*其中Convert.ToSingle（）和 Convert.ToFloat() 来取得一个int类型的单精度浮点数（C#中没有 Convert.ToFloat() ）；
    //    if (Convert.ToSingle(Screen.height) / Screen.width > Convert.ToSingle(ManualHeight) / ManualWidth)
    //    {
    //        //如果屏幕的高宽比大于自定义的高宽比 。则通过公式  ManualWidth * manualHeight = Screen.width * Screen.height；
    //        //来求得适应的  manualHeight ，用它待求出 实际高度与理想高度的比率 scale
    //        manualHeight = Mathf.RoundToInt(Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
    //    }
    //    else
    //    {   //否则 直接给manualHeight 自定义的 ManualHeight的值，那么相机的fieldOfView就会原封不动
    //        manualHeight = ManualHeight;
    //    }

    //    Camera camera = GetComponent<Camera>();
    //    float scale = Convert.ToSingle(manualHeight * 1.0f / ManualHeight);
    //    camera.fieldOfView *= scale;                      //Camera.fieldOfView 视野:  这是垂直视野：水平FOV取决于视口的宽高比，当相机是正交时fieldofView被忽略
    //                                                      //把实际高度与理想高度的比率 scale乘加给Camera.fieldOfView。
    //                                                      //这样就能达到，屏幕自动调节分辨率的效果


    //}


    private Camera theCamera;

    //距离摄像机8.5米 用黄色表示
    public float upperDistance = 8.5f;
    //距离摄像机12米 用红色表示
    public float lowerDistance = 12.0f;

    private Transform tx;


    void Start()
    {
        if (!theCamera)
        {
            theCamera = Camera.main;
        }
        tx = theCamera.transform;

        if (Screen.height != height && Screen.width != width)
        {
            rate2 = Screen.height / (float)Screen.width;
            gameObject.GetComponent<Camera>().fieldOfView *= 1 + (rate2 - rate1);
        }

    }


    void Update()
    {
        FindUpperCorners();
        FindLowerCorners();
    }


    void FindUpperCorners()
    {
        Vector3[] corners = GetCorners(upperDistance);

        // for debugging
        Debug.DrawLine(corners[0], corners[1], Color.yellow); // UpperLeft -> UpperRight
        Debug.DrawLine(corners[1], corners[3], Color.yellow); // UpperRight -> LowerRight
        Debug.DrawLine(corners[3], corners[2], Color.yellow); // LowerRight -> LowerLeft
        Debug.DrawLine(corners[2], corners[0], Color.yellow); // LowerLeft -> UpperLeft
    }


    void FindLowerCorners()
    {
        Vector3[] corners = GetCorners(lowerDistance);

        // for debugging
        Debug.DrawLine(corners[0], corners[1], Color.red);
        Debug.DrawLine(corners[1], corners[3], Color.red);
        Debug.DrawLine(corners[3], corners[2], Color.red);
        Debug.DrawLine(corners[2], corners[0], Color.red);
    }


    Vector3[] GetCorners(float distance)
    {
        Vector3[] corners = new Vector3[4];

        float halfFOV = (theCamera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = theCamera.aspect;

        float height = distance * Mathf.Tan(halfFOV);
        float width = height * aspect;

        // UpperLeft
        corners[0] = tx.position - (tx.right * width);
        corners[0] += tx.up * height;
        corners[0] += tx.forward * distance;

        // UpperRight
        corners[1] = tx.position + (tx.right * width);
        corners[1] += tx.up * height;
        corners[1] += tx.forward * distance;

        // LowerLeft
        corners[2] = tx.position - (tx.right * width);
        corners[2] -= tx.up * height;
        corners[2] += tx.forward * distance;

        // LowerRight
        corners[3] = tx.position + (tx.right * width);
        corners[3] -= tx.up * height;
        corners[3] += tx.forward * distance;

        return corners;
    }
}
