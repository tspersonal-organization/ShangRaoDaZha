using UnityEngine;
using System;
using FrameworkForCSharp.NetWorks;

public class GatewayConnection:AuthConnectionClient
{
	
	public GatewayConnection ()
	{
        protocolSecurity = true;
        //ForWebSocket = false;
		 //addHandler((ushort)Opcodes.SMSG_CHAR_LOGOUT, ClientResponse.onLogout);
	}
    protected override void OnConnected()
    {
        base.OnConnected();
    }
    protected override void OnAuthSuccessed()
	{
        Log.Debug("連接服務器成功...");
        ConnServer.m_IsConnectServer = true;

        if(!Player.Instance.isLogin)
        {
            ManagerScene.Instance.LoadScene(SceneType.Login);
        }
        else
        {
            if(Player.Instance.isLogin)
            {
                ClientToServerMsg.Send(Opcodes.Client_Character_Create, Player.Instance.openID, Player.Instance.otherName, Player.Instance.headID,(byte)Player.Instance.sex);
            }
        }

	}
	protected override void OnDisconnected()
	{
        ConnServer.m_IsConnectServer = false;
        Log.Debug("断开服務器成功...");
	}
	protected override void DefaultHandleMessage(NetworkMessage message)
	{
		Handler.dispatchMessage(message);
        if(ConnServer.m_WaitServerMsgCount > 0)
            ConnServer.m_WaitServerMsgCount--;
    }
}


