using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingObj : UIBase<LoadingObj>
{
    void Start()
    {
        Invoke("ShowDisconnectServer", 5);

        UIManager.Instance.HideUiPanel(UIPaths.PanelCreatRoom);
        UIManager.Instance.HideUiPanel(UIPaths.PanelJoinRoom);
    }

    void ShowDisconnectServer()
    {
      //  UIManager.Instance.ShowUiPanel(UIPaths.DisconnectServer);
        ConnServer.m_WaitServerMsgCount = 0;
        UIManager.Instance.HideUiPanel(UIPaths.LoadingObj);
    }
}
