using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using System;

public class LoginHandler
{
    public LoginHandler()
    {
        Handler.addServerHandler(Opcodes.Server_Character_Info,onPlayerInfo);//玩家信息
        Handler.addServerHandler(Opcodes.Server_Character_LivingDataChanged, onPlayerDataChange);//玩家数据变化
    }
    void onPlayerDataChange(NetworkMessage message)
    {
        byte count = message.readUInt8();
        for (int i = 0; i < count; i++)
        {
            LivingType type = (LivingType)message.readUInt8();
            switch (type)
            {

                case LivingType.has_email://x有信息
                    Player.Instance.HaveEmail = message.readBool();
                    MainPanel.Instance.InitInviteClubMessage();
                  
                    break;
                case LivingType.clubInviteChange://邀请列表变化
                    int Messagecount = message.readInt32();
                    GameData.InviteClubIdAndName = new List<ClubInfo>();
                    for (int j = 0; j < Messagecount; j++)
                    {
                        ClubInfo info = new ClubInfo();
                        info.Id =(int) message.readUInt32();
                        info.ClubName = message.readString();
                        GameData.InviteClubIdAndName.Add(info);
                    }
                    if (ClubInvitePanelControl.Instance == null)
                    {
                        UIManager.Instance.ShowUiPanel(UIPaths.ClubInvitePlayerPanel);
                    }
                    else
                    {
                        if (ClubInvitePanelControl.Instance.gameObject.activeSelf)
                        {
                            ClubInvitePanelControl.Instance.InitData();
                        }
                        else
                        {
                            UIManager.Instance.ShowUiPanel(UIPaths.ClubInvitePlayerPanel);
                        }
                       
                    }
                    break;
                case LivingType.card:
                    Player.Instance.RoomCard = message.readInt64();
                    break;
                case LivingType.otherName:
                    break;
                case LivingType.headId:
                    break;
                case LivingType.gold:
                    Player.Instance.Gold = message.readInt64();
                    break;
                case LivingType.diamond:
                    Player.Instance.money = message.readInt64();
                    break;
                case LivingType.clubChange://俱乐部信息改变
                    GameData.PKClubInfoList = new List<PkClubInfo>();
                    int  count1 = message.readInt32();//俱乐部个数
                    for (int j = 0; j < count1; j++)
                    {
                        PkClubInfo info = new PkClubInfo();
                        info.ClubId= message.readUInt32();//clubid
                      info.ClubName=  message.readString();//俱乐部名字
                       message.readString();//者的名字
                        message.readInt32();//俱乐部玩家个数

                        info.RoomCount = 0;
                        GameData.PKClubInfoList.Add(info);
                    }

                    MainPanel.Instance.CreatPkClubList();
                    break;
            }
        }

        GameEventDispatcher.Instance.dispatchEvent(EventIndex.PlayerLivingDataChange);//玩家数据改变
    }
    void onPlayerInfo(NetworkMessage message)
    {
        Log.Debug("个人信息");
        Player.Instance.guid = message.readUInt64();
        Player.Instance.account = message.readString();
        Player.Instance.otherName = message.readString();
        Player.Instance.headID = message.readString();

        Player.Instance.RoomCard = message.readInt64();//房卡
        Player.Instance.money = message.readInt64();//钻石
        Player.Instance.Gold = message.readInt64();//金币

        Player.Instance.shareUrl = message.readString();
        Player.Instance.lastEnterRoomID = message.readUInt32();//（0,房间id）
        Player.Instance.sex = message.readUInt8();

        UIPaths.CHAT_UPLOAD_SOUND_PATH = message.readString();
        UIPaths.CHAT_DOWN_SOUND_PATH = message.readString();
        Player.Instance.isDaiLi = message.readBool();


        GameData.m_PaoMaDengList.Clear();
        uint count = message.readUInt32();
        for (int i = 0; i < count; i++)
        {
            string content = message.readString();
             GameData.m_PaoMaDengList.Add(content);
        }


        Player.Instance.content = message.readString();
        ServerInfo.Data.version = message.readString();
        ServerInfo.Data.update_android_url = message.readString();
        ServerInfo.Data.update_ios_url = message.readString();
        ServerInfo.Data.update_message = message.readString();
        ServerInfo.Data.login_with_device = message.readBool();


        string[] strs = ServerInfo.Data.update_message.Split('@');
        ServerInfo.Data.update_message = strs[0];
        //GameData.IsAppStore = strs[1] == "AppStore" ? true : false;
        //GameData.ShareImageURL = strs[2];
        //GameData.RechargeURL = strs[3];


        Player.Instance.everydayShareCount = message.readUInt32();
        Player.Instance.InviteGuid = message.readUInt64();

        bool HaveEmail = message.readBool();//是否有邮件
        Player.Instance.HaveEmail = HaveEmail;

        uint ClubCount = message.readUInt32();//加入的俱乐部个数
        GameData.PKClubInfoList = new List<PkClubInfo>();
        for (int i = 0; i < ClubCount; i++)
        {
            bool exist = message.readBool();
            if (exist)
            {
                PkClubInfo info = new PkClubInfo();
                info.ClubId= message.readUInt32();//俱乐部id
                info.ClubName=message.readString();//俱乐部名称
                info.RoomCount = message.readInt32();//俱乐部房间数

                GameData.PKClubInfoList.Add(info);
            }
            else
            {
                message.readBool();
                continue;
            }
        }

        /*
        int Messagecount = message.readInt32();//邀请玩家加入俱乐部的消息列表
        GameData.InviteClubIdAndName = new List<ClubInfo>();
        for (int i = 0; i < Messagecount; i++)
        {
            ClubInfo info = new ClubInfo();
            uint InviteClubId = message.readUInt32();
            string ClubName = message.readString();
            info.Id = (int)InviteClubId;
            info.ClubName = ClubName;
            GameData.InviteClubIdAndName.Add(info);
        }
        */


        if (Player.Instance.lastEnterRoomID != 0)
        {
            Debug.Log("duanlian的房间"+ Player.Instance.lastEnterRoomID);
            ClientToServerMsg.Send(Opcodes.Client_PlayerEnterRoom, Player.Instance.lastEnterRoomID, Input.location.lastData.latitude, Input.location.lastData.longitude);
        }
        else
        {

            ManagerScene.Instance.LoadScene(SceneType.Main);
            //if (Player.Instance.shareRoomID != 0)
            //{
            //    ClientToServerMsg.Send(Opcodes.Client_PlayerEnterRoom, Player.Instance.shareRoomID, Input.location.lastData.latitude, Input.location.lastData.longitude);
            //    Player.Instance.shareRoomID = 0;
            //}
            //else
            //{
            //    ManagerScene.Instance.LoadScene(SceneType.Main);
            //}
        }

     

        Player.Instance.isLogin = true;
    }
}
