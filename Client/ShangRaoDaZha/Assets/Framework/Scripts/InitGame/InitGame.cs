using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FrameworkForCSharp.Utils;
using System.Diagnostics;
using System;

public class InitGame : UIBase<InitGame>
{
    [HideInInspector]
    public bool isXMLDone = false;

    // Use this for initialization
    void Start()
    {
        InitData();
        GameSetting();//游戏设置
        LoadResources();//加载资源
        GameData.InitAllCardsInfo();
    }
    void InitData()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            //AndroidOrIOSResult.GetMask();
        }
    }
    void Update()
    {
        //if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        //{
        //    // if (QuitGame.Instance == null) UIManager.Instance.ShowUIPanel(UIPaths.QuitGame);
        //    Application.Quit();
        //}
    }

    void GameSetting()
    {
        //游戏不休眠
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //后台运行
        Application.runInBackground = true;
        //设置60
        Application.targetFrameRate = 60;
    }

    void LoadResources()
    {
        ResultCodeString.Init();
    }
    protected override void OnDestroy()
    {

    }
}
