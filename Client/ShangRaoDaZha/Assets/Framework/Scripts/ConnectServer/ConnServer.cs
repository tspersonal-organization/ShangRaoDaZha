using UnityEngine;
using System;
using System.Collections;
using FrameworkForCSharp.NetWorks;
using System.Collections.Generic;
using FrameworkForCSharp.Utils;
using LitJson;


public class ConnServer : UIBase<ConnServer>
{

    public static class global
    {
        public static TcpClient Tcp_gateway;        //作为客户端 连接 GatewayServer 
    }
    public class GatewayClient : TcpClient
    {
        protected override void onConnectedFailed(string ip, ushort port)
        {
            connect<GatewayConnection>(ip, port);
        }
    }
    public static bool m_IsConnectServer = false;
    public static int m_WaitServerMsgCount = 0;//消息计数
    public static bool m_IsIpv6 = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //ConnectionServer("192.168.39.166", 20060);
        StartCoroutine(GetServerConfig());
    }
    void Update()
    {
        Connection.update();
    }
    #region 获取服务器配置
    IEnumerator GetServerConfig()
    {
        //string url = "http://xcmj.cn-newworld.com:97/ClientQuery.aspx?method=queryServerAddress&gameId={0}&clientVersion={1}";
      // string url = "http://game.youthgamer.com:97/ClientQuery.aspx?method=queryServerAddress&gameId={0}&clientVersion={1}";
      // string url = "http://xianyugou.youthgamer.com:97/ClientQuery.aspx?method=queryServerAddress&gameId={0}&clientVersion={1}";
        //http://game.youthgamer.com:97/ClientQuery.aspx?method=queryServerAddress&gameId=xygqp&clientVersion=1.0.0
        string url = "http://hw389.cn:97/ClientQuery.aspx?method=queryServerAddress&gameId={0}&clientVersion={1}";
        url = string.Format(url, "ljsrdz", Application.version);

        //url = string.Format(url, "xincmj",Application.version);
       WWW www = new WWW(url);
       yield return www;
       if (www.error == null)
       {
            ServerInfo.Data = JsonUtility.FromJson<ServerInfo>(www.text);
            if(ServerInfo.Data.statusCode == "Success")
            {
                if(ServerInfo.Data.version == Application.version)
                {
                    ConnectionServer(ToolsFunc.GetServerIP(ServerInfo.Data.ip), (ushort)ServerInfo.Data.port);
                    if (!ServerInfo.Data.login_with_device) GetGPS.Instance.InitGPS();
                }
                else
                {
                    UIManager.Instance.ShowUiPanel(UIPaths.GameUpdate, OpenPanelType.MinToMax);
                }
            }
        }
        yield break;
    }
    #endregion
    #region 连接游戏服务器
    public static void ConnectionServer(string ip, ushort port)
    {
        global.Tcp_gateway = new GatewayClient();//
        if (m_IsIpv6)
            global.Tcp_gateway.connectIpv6<GatewayConnection>(ip, port);
        else
            global.Tcp_gateway.connect<GatewayConnection>(ip, port);
    }

    public void DisconnectServer()
    {
        if (global.Tcp_gateway != null)
        {
            Connection conn = global.Tcp_gateway[0];
            if (conn != null)
                conn.close();
        }

      //  m_IsConnectServer = false;
    }
    #endregion
    protected override void OnDestroy()
    {
        DisconnectServer();
    }
}
