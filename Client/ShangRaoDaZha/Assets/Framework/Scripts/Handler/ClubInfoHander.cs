﻿/*
* 开发人：滕俊
* 项目：
* 功能描述：
*
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using UnityEngine;

public class ClubInfoHander : MonoBehaviour
{

    public ClubInfoHander()
    {

        Handler.addServerHandler(Opcodes.Client_Query_Club_RoomList_Act, onClubRoomListBack);//俱乐部房间列表返回
        Handler.addServerHandler(Opcodes.Client_Query_Club_Player_Info_Ack, onClubMemInfoBack);//俱乐部成员信息返回

        Handler.addServerHandler(Opcodes.Save_Club_Info, onClubListBack);//俱乐部列表返回
        Handler.addServerHandler(Opcodes.Client_Query_Club_Info_Ack, onClubInfoBack);//俱乐部信息返回
    }

    /// <summary>
    /// 俱乐部的房间列表返回
    /// </summary>
    /// <param name="obj"></param>
    private void onClubRoomListBack(NetworkMessage message)
    {
        //   message.writeInt32(room_list.Count);
        //   foreach (XYGQiPaiRoom item in room_list.Values)
        //   {
        //       message.writeUInt32(item.codeId);//房间id
        //       message.writeUInt8((byte)item.playType);//玩法类型，普通、包牌
        //       message.writeUInt32(item.getRoomConfig().playerCount);//玩家人数
        //       message.writeUInt32(item.getRoomConfig().gameCount);//局数
        //       message.writeBool(item.getRoomConfig().wuZhaTp);//无炸摊牌
        //       message.writeBool(item.getRoomConfig().faWangTp);//罚王摊牌
        //       message.writeInt32(item.getRoomConfig().jiangMa.Count);//奖码数
        //       foreach (var sub_item in item.getRoomConfig().jiangMa)
        //       {
        //           message.writeUInt32(sub_item);//奖码
        //       }
        //       message.writeString(create_player.name);//开房人名称
        //       message.writeUInt64(create_player.guid);//开房人guid
        //（//如果改玩家不存在则会返回以下数据
        //    message.writeString("无该人物数据");//开房人名称
        //       message.writeUInt64(0);//开房人guid ）
        //   }
        //message.writeInt32(item.sitPlayerDict.Count);//在座玩家个数
        //foreach (var player_item in item.sitPlayerDict.Values)
        //{
        //    message.writeUInt8((byte)player_item.position);//玩家位置
        //    message.writeUInt64(player_item.guid);//玩家的guid
        //    message.writeString(player_item.playerBase.otherName);//玩家昵称
        //    message.writeString(player_item.playerBase.headId);//玩家头像


        int count = message.readInt32();
        GameData.PKClubRoomList = new List<PKClubRoomInfo>();
        for (int i = 0; i < count; i++)
        {
            PKClubRoomInfo info = new PKClubRoomInfo();
            info.codeId = message.readUInt32();//房间id
            info.playType = message.readUInt8();//玩法类型，普通、包牌
            info.playerCount = message.readUInt32();//玩家人数
            info.gameCount = message.readUInt32();//局数
            info.wuZhaTp = message.readBool();//无炸摊牌
            info.faWangTp = message.readBool();
            info.BaWang = message.readBool();//是否八王
            int count1 = message.readInt32();//开奖数
            info.JiangMa = new List<uint>();
            for (int j = 0; j < count1; j++)
            {
                info.JiangMa.Add(message.readUInt32());
            }
            info.create_playername = message.readString();
            info.create_playerguid = message.readUInt64();

            int playerCount = message.readInt32();
            info.PKClubPlayerInfoList = new List<PKClubPlayerInfo>();
            for (int j = 0; j < playerCount; j++)
            {
                PKClubPlayerInfo pkinfo = new PKClubPlayerInfo();
                pkinfo.pos = message.readUInt8();//玩家位置
                pkinfo.Guid = message.readUInt64();//玩家的guid
                pkinfo.OtherName = message.readString();//玩家昵称
                pkinfo.HeadId = message.readString();//玩家头像

                info.PKClubPlayerInfoList.Add(pkinfo);
            }

            GameData.PKClubRoomList.Add(info);

        }
      ;
        DzViewMain.Instance.CreatClubRoomList();
        // DzViewMain.Instance.CreatClubRoomList();
    }

    /// <summary>
    /// 俱乐部成员  申请信息返回
    /// </summary>
    /// <param name="message"></param>
    private void onClubMemInfoBack(NetworkMessage message)
    {
        GameData.CurrentClubInfo.ApplyMemList = new List<MemInfo>();
        bool bApply = message.readBool();
        if (bApply) //有申请信息
        {
            int ClubCount = message.readInt32(); //俱乐部个数
            for (int j = 0; j < ClubCount; j++)
            {
                uint clubid = message.readUInt32();
                string ClubName = message.readString();
                int count = message.readInt32(); //申请的人数

                for (int i = 0; i < count; i++)
                {
                    MemInfo info = new MemInfo();
                    info.ClubId = clubid;
                    info.ClubName = ClubName;
                    info.Name = message.readString();
                    info.Guid = message.readUInt64();
                    info.HeadId = message.readString();
                    GameData.CurrentClubInfo.ApplyMemList.Add(info);
                }
            }
            int count1 = message.readInt32(); //邀请信息
            GameData.CurrentClubInfo.InviteList = new List<MemInfo>();
            for (int i = 0; i < count1; i++)
            {
                MemInfo inviteM = new MemInfo();

                inviteM.Guid = message.readUInt64(); //邀请人guid
                inviteM.Name = message.readString(); //邀请人名字
                inviteM.ClubId = message.readUInt32(); //俱乐部id
                inviteM.ClubName = message.readString(); //俱乐部名字

                GameData.CurrentClubInfo.InviteList.Add(inviteM);
            }
        }
        else
        {
            int count2 = message.readInt32(); //邀请信息
            GameData.CurrentClubInfo.InviteList = new List<MemInfo>();
            for (int i = 0; i < count2; i++)
            {
                MemInfo info = new MemInfo();
                info.Guid = message.readUInt64();
                info.Name = message.readString();
                info.ClubId = message.readUInt32(); //俱乐部id
                info.HeadId = message.readString();
                GameData.CurrentClubInfo.InviteList.Add(info);
            }
        }
        if (DzPanelMessage.Instance != null)
        {
            DzPanelMessage.Instance.CreatData();
        }
        UIManager.Instance.ShowUiPanel(UIPaths.PanelMessage, OpenPanelType.MinToMax);

    }


    /// <summary>
    /// 俱乐部信息返回
    /// </summary>
    /// <param name="obj"></param>
    private void onClubInfoBack(NetworkMessage message)
    {
        ClubInfo info = new ClubInfo();
        info.RoomConfigList = new List<RoomConfig>();
        info.ActiveRoomInfoList = new List<ActiveRoomInfo>();
        info.HistoryDataList = new List<HistoryData>();

        info.Id = (int)message.readUInt32();
        info.ClubName = message.readString();
        info.CreatorName = message.readString();
        info.CreatorGuid = message.readUInt64();
        info.CreatorPhoneNum = message.readString();
        info.Gold = message.readInt64();
        info.creatPower = (ClubCreatePower)message.readUInt8();//开房权限
        info.PlayerCount = message.readInt32();//俱乐部人数

        //所有的玩家信息 包括俱乐部群主 以及管理员
        info.AllMemList = new List<MemInfo>();
        int count = message.readInt32();
        for (int i = 0; i < count; i++)
        {
            MemInfo memInfo = new MemInfo();
            memInfo.Name = message.readString();
            memInfo.Guid = message.readUInt64();
            memInfo.HeadId = message.readString();
            memInfo.Sex = message.readUInt8();
            memInfo.Ip = message.readString();
            memInfo.Adress = message.readString();
            memInfo.PlayCount = message.readUInt32();

            info.AllMemList.Add(memInfo);
        }

        info.MemMasterList = new List<MemInfo>();//管理员
        int count1 = message.readInt32();
        for (int i = 0; i < count1; i++)
        {
            MemInfo masterInfo = new MemInfo();
            masterInfo.Name = message.readString();
            masterInfo.Guid = message.readUInt64();
            masterInfo.HeadId = message.readString();
            info.MemMasterList.Add(masterInfo);
        }

        //设置群主、管理员、普通会员数据
        info.NormalMemList = new List<MemInfo>();//玩家信息
        for (var i = 0; i < info.AllMemList.Count; i++)
        {
            var data = info.AllMemList[i];
            bool bNormal = true;
            if (data.Guid == info.CreatorGuid)
            {
                bNormal = false;
                info.CreatorMemInfo = data;
            }
            else
            {
                for (var j = 0; j < info.MemMasterList.Count; j++)
                {
                    var data1 = info.MemMasterList[j];
                    if (data1.Guid == data.Guid)
                    {
                        bNormal = false;
                        info.MemMasterList[j] = data;
                    }
                }
            }
            if (bNormal)
            {
                info.NormalMemList.Add(data);
            }
        }

        GameData.CurrentClubInfo = info;//当前申请的俱乐部
        if (DzPanelMomentInfo.Instance != null && DzPanelMomentInfo.Instance.gameObject.activeSelf)
        {
            DzPanelMomentInfo.Instance.InitData();
        }
        UIManager.Instance.ShowUiPanel(UIPaths.PanelMomentInfo, OpenPanelType.MinToMax);
    }


    /// <summary>
    /// 俱乐部列表返回
    /// </summary>
    /// <param name="message"></param>
    private void onClubListBack(NetworkMessage message)
    {
        GameData.ClubInfoList = new List<ClubInfo>();
        int count = message.readInt32();
        for (int i = 0; i < count; i++)
        {
            ClubInfo info = new ClubInfo();
            info.Id = (int)message.readUInt32();//俱乐部id
            info.ClubName = message.readString();//俱乐部名称
            info.CreatorName = message.readString();//俱乐部管理人
            info.PlayerCount = message.readInt32();//俱乐部人数

            GameData.ClubInfoList.Add(info);
        }
        UIManager.Instance.ShowUiPanel(UIPaths.ClubListPanel, OpenPanelType.MinToMax);
        if (ClubListPanelControl.Instance != null)
            ClubListPanelControl.Instance.InitData();
    }
}

//俱乐部信息
public class ClubInfo
{
    public string ClubName;
    public string CreatorName;
    public ulong CreatorGuid;
    public string CreatorPhoneNum;
    public long Gold;//俱乐部资金
    public int PlayerCount;
    public int Id;
    public ClubCreatePower creatPower;//开房权限

    public List<RoomConfig> RoomConfigList = new List<RoomConfig>();//房间配置的类型
    public List<ActiveRoomInfo> ActiveRoomInfoList = new List<ActiveRoomInfo>();//已经激活的房间
    public List<HistoryData> HistoryDataList = new List<HistoryData>();//历史数据

    public List<MemInfo> AllMemList = new List<MemInfo>();//成员列表
    public MemInfo CreatorMemInfo = new MemInfo();//群主信息
    public List<MemInfo> NormalMemList = new List<MemInfo>();//成员列表
    public List<MemInfo> MemMasterList = new List<MemInfo>();//管理员列表
    public List<MemInfo> InviteList = new List<MemInfo>();//邀请列表
    public List<MemInfo> ApplyMemList = new List<MemInfo>();//申请的列表
}

//局头配置的房间
public class RoomConfig
{
    public RoomType roomType;//房间类型
    public ClubRoomType clubRoomType;//
    public int GameCount;//房间局数
    public JiangMaType IsJiangMa;//是否奖码
    public NNYongPai YongPaiType;//牛牛用牌类型 

    public List<uint> NiuNiuChips = new List<uint>();//底分
    public bool ShunZhiNiu = false;
    public bool ZhaDanNiu = false;
    public bool wuXiaoNiu = false;
    public bool WuHuaNiu = false;
    public bool XianJiaMaiMa = false;

}

//配置激活的房间
public class ActiveRoomInfo
{
    public RoomStatusType RoomStatus;//房间状态
    public uint RoomId;//房间号
    public RoomType roomtype;//房间类型
    public ClubRoomType clubRoomType;//
    public uint RoomCount;//房间局数
    public JiangMaType IsJiangMa;//是否奖码
    public NNYongPai YongPaiType;//牛牛用牌类型 
    public List<string> PlayerCountHeadList = new List<string>();//玩家头像列表

    public List<uint> NiuNiuChips = new List<uint>();//底分
    public bool ShunZhiNiu = false;
    public bool ZhaDanNiu = false;
    public bool wuXiaoNiu = false;
    public bool WuHuaNiu = false;
    public bool XianJiaMaiMa = false;

}

/// <summary>
/// 历史数据记录
/// </summary>
public class HistoryData
{
    public string Time;
    public Dictionary<ClubRoomType, uint> RoomTypeAndDownCount = new Dictionary<ClubRoomType, uint>();
}

/// <summary>
/// 成员信息
/// </summary>
public class MemInfo
{
    public string Name;
    public ulong Guid;
    public string HeadId;
    public int Sex;
    public string Ip;
    public string Adress;
    public uint PlayCount;//玩的次数
    public uint ClubId;
    public string ClubName;//所在俱乐部的名字（邀请玩家加入的时候有用）
}

