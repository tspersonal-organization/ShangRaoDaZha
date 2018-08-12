using FrameworkForCSharp.NetWorks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FrameworkForCSharp.Utils;

public class RoomManagerHandler
{
    #region 注册房间消息
    static RoomManagerHandler()
    {
        new RoomInfoHandler();
      //  new DDZRoomInfoHandler();
    }
    private static Action<NetworkMessage>[] message_room_handlers = new Action<NetworkMessage>[512];
    public static void addServerHandler(RoomMessageType type, Action<NetworkMessage> handler)
    {
        message_room_handlers[(UInt16)type] = handler;
    }
    public static void dispatchMessage(RoomMessageType type,NetworkMessage message)
    {
        if(message_room_handlers[(UInt16)type] != null)
            message_room_handlers[(UInt16)type](message);
    }
    #endregion

    #region 消息入口
    public RoomManagerHandler()
    {
        Handler.addServerHandler(Opcodes.Server_Room_Info, onRoomMsg);
       // Handler.addServerHandler(Opcodes.Player_Finish,this.OnPlayerFinish);
    }

   

    void onRoomMsg(NetworkMessage message)
    {

       
        RoomMessageType type = (RoomMessageType)message.readUInt8();
        dispatchMessage(type,message);
    }
    #endregion

}
