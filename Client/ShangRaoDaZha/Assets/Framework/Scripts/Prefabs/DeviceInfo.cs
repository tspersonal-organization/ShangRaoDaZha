using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceInfo : MonoBehaviour
{
    public UILabel LBTime;
    public UISprite WIFIImg;
    public UISprite BatteryImg;
    [Header("时间更新间隔")]
    public float timeUpdateTime = 60;
    [Header("信号更新间隔")]
    public float wifiUpdateTime = 5;
    [Header("电量更新间隔")]
    public float batteryUpdateTime = 60;

	void Start ()
    {
        InvokeRepeating("TimeUpdate",0,timeUpdateTime);
        InvokeRepeating("WIFIUpdate", 0, wifiUpdateTime);
        InvokeRepeating("BatteryUpdate", 0, batteryUpdateTime);
    }

    void TimeUpdate()
    {
        LBTime.text = DateTime.Now.ToString("HH:mm");
    }

    void WIFIUpdate()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)//没网
            WIFIImg.spriteName = "UI_game_icon_NoSignal";
        else if(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)//数据
            WIFIImg.spriteName = "UI_game_icon_4g";
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)//wifi
            WIFIImg.spriteName = "UI_game_icon_wifi";
        WIFIImg.MakePixelPerfect();
    }

    void BatteryUpdate()
    {
        BatteryImg.fillAmount = SystemInfo.batteryLevel;
    }
}
