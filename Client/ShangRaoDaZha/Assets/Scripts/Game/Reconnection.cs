using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FrameworkForCSharp.NetWorks;

/// <summary>
/// 重新刷新牌局
/// </summary>
public class Reconnection : MonoBehaviour 
{
    DateTime startTime;
    DateTime endTime;
    void OnApplicationFocus(bool isClose)
    {
        if (isClose)//获得焦点
        {
            if (ConnServer.m_IsConnectServer)
            {
                ClientToServerMsg.Send(Opcodes.Client_PlayerOnForce, GameData.m_TableInfo.id, true);
                endTime = DateTime.Now;
                TimeSpan temp = endTime - startTime;
                if (temp.Seconds > 1)
                {
                    ClientToServerMsg.Send(Opcodes.Client_PlayerEnterRoom,GameData.m_TableInfo.id, Input.location.lastData.latitude, Input.location.lastData.longitude);
                }
            }
        }
        else//失去焦点
        {
            startTime = DateTime.Now;
             Player.Instance.lastEnterRoomID = GameData.m_TableInfo.id;
            ClientToServerMsg.Send(Opcodes.Client_PlayerOnForce, GameData.m_TableInfo.id,false);
        }
    }
}
