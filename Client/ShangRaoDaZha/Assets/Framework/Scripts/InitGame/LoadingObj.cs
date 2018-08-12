using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingObj : UIBase<LoadingObj>
{
    void Start()
    {
        Invoke("ShowDisconnectServer", 5);

        UIManager.Instance.HideUIPanel(UIPaths.CreatRoomPanel);
        UIManager.Instance.HideUIPanel(UIPaths.JoinRoomPanel);
    }

    void ShowDisconnectServer()
    {
      //  UIManager.Instance.ShowUIPanel(UIPaths.DisconnectServer);
        ConnServer.m_WaitServerMsgCount = 0;
        UIManager.Instance.HideUIPanel(UIPaths.LoadingObj);
    }
}
