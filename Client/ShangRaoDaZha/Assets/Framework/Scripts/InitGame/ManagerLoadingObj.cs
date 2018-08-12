using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLoadingObj : MonoBehaviour
{
    float MaxTime = 2;
    float WaitTime = 2;
    void Update()
    {
        if (ConnServer.m_WaitServerMsgCount > 0 || (!ConnServer.m_IsConnectServer && GameUpdate.Instance == null && DisconnectServer.Instance == null))
        {
            if (WaitTime > 0)
            {
                WaitTime -= Time.deltaTime;
            }
            else
            {
                if (LoadingObj.Instance == null)
                {
                    WaitTime = MaxTime;
                    UIManager.Instance.ShowUIPanel(UIPaths.LoadingObj);
                }
            }
        }
        else
        {
            WaitTime = MaxTime;
            if (LoadingObj.Instance != null)
            {
                UIManager.Instance.HideUIPanel(UIPaths.LoadingObj);
            }
        }
    }
}
