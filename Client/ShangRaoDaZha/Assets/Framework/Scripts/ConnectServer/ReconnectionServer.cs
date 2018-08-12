using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 断线重连
/// </summary>
public class ReconnectionServer : MonoBehaviour
{
    public static float intervalTime = 3.0f;//重新连接时间
    static NetworkReachability CurNetworkType = NetworkReachability.NotReachable;

    void Start()
    {
        CurNetworkType = Application.internetReachability;
    }
    // Update is called once per frame
    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable || GameUpdate.Instance != null)
        {
            if (CurNetworkType != NetworkReachability.NotReachable)
            {
              //  UIManager.Instance.ShowUIPanel(UIPaths.ReconectTipPanel);
            }
          
                CurNetworkType = NetworkReachability.NotReachable;
          
           // UIManager.Instance.ShowUIPanel(UIPaths.ReconectTipPanel);
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
           

            if (CurNetworkType != NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                ConnServer.Instance.DisconnectServer();
               // AndroidOrIOSResult.GetMask();
                intervalTime = 0;
            }
            CurNetworkType = NetworkReachability.ReachableViaCarrierDataNetwork;
            if (!ConnServer.m_IsConnectServer)//断开服务器
            {
                if (intervalTime <= 0)
                {
                    intervalTime = 3;
                    ConnServer.m_WaitServerMsgCount = 0;
                    if(ServerInfo.Data.ip != null)
                    {
                        ConnServer.ConnectionServer(ToolsFunc.GetServerIP(ServerInfo.Data.ip), ServerInfo.Data.port);
                    }
                }
                else
                {
                    intervalTime -= Time.deltaTime;
                }
            }

          //  UIManager.Instance.HideUIPanel(UIPaths.ReconectTipPanel);
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {

          
            if (CurNetworkType != NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                ConnServer.Instance.DisconnectServer();
              //  AndroidOrIOSResult.GetMask();
                intervalTime = 0;
            }
            CurNetworkType = NetworkReachability.ReachableViaLocalAreaNetwork;
            if (!ConnServer.m_IsConnectServer)//断开服务器
            {
                if (intervalTime <= 0)
                {
                    intervalTime = 3;
                    ConnServer.m_WaitServerMsgCount = 0;
                    if (ServerInfo.Data.ip != null)
                    {
                        ConnServer.ConnectionServer(ToolsFunc.GetServerIP(ServerInfo.Data.ip), ServerInfo.Data.port);
                    }
                }
                else
                {
                    intervalTime -= Time.deltaTime;
                }
            }

        //    UIManager.Instance.HideUIPanel(UIPaths.ReconectTipPanel);
        }
    }

    //string STR = "";
    //void OnGUI()
    //{
    //    if (GUILayout.Button(STR))
    //    {

    //    }
    //}
}
