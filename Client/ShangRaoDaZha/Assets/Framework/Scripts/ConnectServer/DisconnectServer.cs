using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectServer : UIBase<DisconnectServer>
{
    public GameObject btnQiut;
    public GameObject btnReconnect;
    public UILabel LBDesc;

    float connectTime = 15;
    bool isConnect = true;
    // Use this for initialization
    void Start()
    {
        UIEventListener.Get(btnQiut).onClick = OnClick;
        UIEventListener.Get(btnReconnect).onClick = OnClick;
    }

    void OnClick(GameObject go)
    {
        if (go == btnQiut)
        {
            Application.Quit();
        }
        else if (go == btnReconnect)
        {
            if(isConnect)
            {
                isConnect = false;
                LBDesc.gameObject.SetActive(true);
                LBDesc.text = "正在连接中...";
                ConnServer.Instance.DisconnectServer();
                ConnServer.ConnectionServer(ToolsFunc.GetServerIP(ServerInfo.Data.ip), (ushort)ServerInfo.Data.port);
            }
        }
    }

    void Update()
    {
        if(!isConnect)
        {
            if(connectTime <= 0)
            {
                connectTime = 15;
                isConnect = true;
                LBDesc.text = "连接失败...";
            }
            else
            {
                connectTime -= Time.deltaTime;
            }
        }
    }
}
