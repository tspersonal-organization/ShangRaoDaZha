using System;
using System.Collections;
using System.Collections.Generic;
using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using UnityEngine;

public class MainPanelHandler
{
    public MainPanelHandler()
    {
        Handler.addServerHandler(Opcodes.Server_RoomRecordBase, onRoomRecordList);//房间记录列表
        Handler.addServerHandler(Opcodes.Server_RoomRecordDetail, onRoomRecordDetail);//每局记录

        Handler.addServerHandler(Opcodes.Server_AgentRoomList,onAngentRoomList);//代理房间列表

        Handler.addServerHandler(Opcodes.PiPei_Info,this.GetPiPeiInfo);
    }


    /// <summary>
    /// 获取到匹配信息
    /// </summary>
    /// <param name="obj"></param>
    private void GetPiPeiInfo(NetworkMessage message)
    {
        JinBiDataControl.Instance.TaoShangLimitGold = message.readUInt32();
        JinBiDataControl.Instance.TaoSHangRate = message.readUInt32();

        JinBiDataControl.Instance.WDHLimitGold = message.readUInt32();
        JinBiDataControl.Instance.WDHRate = message.readUInt32();

        JinBiDataControl.Instance.ZBLimitGold = message.readUInt32();
        JinBiDataControl.Instance.ZBRate = message.readUInt32();

       // MainPanel.Instance.OpenPiPeiPanel();
        GameEventDispatcher.Instance.dispatchEvent(EventIndex.GetPiPeiInfo);

    }

    /// <summary>
    /// 接收到代理房间列表
    /// </summary>
    /// <param name="obj"></param>
    private void onAngentRoomList(NetworkMessage message)
    {
        GameData.AngentRoomList = new List<AngentRoomInfo>();
        int count = message.readInt32();
        for (int i = 0; i < count; i++)
        {
            AngentRoomInfo info = new AngentRoomInfo();
            info.RoomId = message.readUInt32();
            info.Roomtype = (RoomType)message.readUInt8();

            info.Time = message.readUInt32();
            info.RoomRound = message.readUInt32();
            info.PlayerCount = message.readInt32();
            info.HeadNames = new List<string>();



            for (int j = 0; j < info.PlayerCount; j++)
            {
                info.HeadNames.Add(message.readString());
            }

            GameData.AngentRoomList.Add(info);
        }

        //if (MyRoomPanel.Instance != null)
        //{
        //    MyRoomPanel.Instance.SetInfo();
        //}

        GameEventDispatcher.Instance.dispatchEvent(EventIndex.AngentRoomListUpdata);
    }

    /// <summary>
    /// 具体那一房间的的回放信息（包含多局）
    /// </summary>
    /// <param name="message"></param>
    private void onRoomRecordDetail(NetworkMessage message)
    {
        GameData.m_RecordRoundList.Clear();
        GameData.m_PlayerInfoList.Clear();
        ulong guid = message.readUInt64();
        bool isData = message.readBool();//是否有数据

        if (!isData) return;
        GameData.m_TableInfo.id = message.readUInt32();
        GameData.m_TableInfo.configRoundIndex = (byte)message.readUInt32();
        GameData.m_TableInfo.configPayIndex = message.readUInt8();
        //if (!isData) return;

        //GameData.m_TableInfo.id = message.readUInt32();
        //GameData.m_TableInfo.configRoundIndex = message.readUInt8();
        //GameData.m_TableInfo.configFangChongIndex = message.readUInt8();
        //GameData.m_TableInfo.configShengPaiIndex = message.readUInt8();
        //GameData.m_TableInfo.configDaZiIndex = message.readUInt8();
        //GameData.m_TableInfo.configPlayerIndex = message.readUInt8();
        //GameData.m_TableInfo.configPayIndex = message.readUInt8();

        byte count = message.readUInt8();//有多少局
        #region
        for (int i = 0; i < count; i++)
        {
            RecordListInfo info = new RecordListInfo();
            info.startTime = myFunction.Instance.fromSecondsFromGameBegin(message.readUInt32());//开始时间
            info.endTime = myFunction.Instance.fromSecondsFromGameBegin(message.readUInt32());//结束时间
            ulong makerGui = message.readUInt64();//庄的guid
            info.ZhuangGuid = makerGui;//庄的guid

            string makerName = message.readString();//庄的昵称
            byte pCount = message.readUInt8();//玩家个数
            for (int k = 0; k < pCount; k++)
            {
                #region 原来
                //string strs = message.readString();
                //strs += ":" + message.readInt32();
                //info.playerInfo.Add(strs);//guid|name|socre|headid
                #endregion

                ulong playerguid = message.readUInt64();
                string otherName = message.readString();
                string HeadId = message.readString();
                int ChangeScore = message.readInt32();
                int BaseScore = message.readInt32();
                int JiangMaScore = message.readInt32();
                int TotalScore = message.readInt32();

                string str = playerguid.ToString() + "@" + otherName.ToString() + "@" + ChangeScore.ToString();
                info.playerInfo.Add(str);//guid|name|socre|headid

                bool HaveCard = message.readBool();//是否有手牌
                if (HaveCard)
                {
                    int CardCount = message.readInt32();//手牌
                    for (int j = 0; j < CardCount; j++)
                    {
                        uint card = message.readUInt32();
                    }
                }
              

            }
            GameData.m_RecordRoundList.Add(info);//每局的信息
        }
        #endregion


        GameData.m_HuiFangList.Clear();//

        byte tCount = message.readUInt8();//有多少条录像

        for (int i = 0; i < tCount; i++)
        {
            HuiFangInfo hInfo = new HuiFangInfo();
            byte dataVersion = message.readUInt8();

            if (GameData.GlobleRoomType == RoomType.ZB)//载宝才传
            {
                hInfo. MianCard = message.readUInt32();//面牌
                hInfo. MagicCard = message.readUInt32();
            }



            GameData.m_PlayerInfoList = new List<PlayerInfo>();
            byte playerCount = message.readUInt8();//玩家个数
            for (int k = 0; k < playerCount; k++)
            {
                PlayerInfo pInfo = new PlayerInfo();
                pInfo.guid = message.readUInt64();

                #region
                string[] strs = GetPlayerInfo(pInfo.guid).Split('@');
                pInfo.name = strs[1];
                pInfo.headID = strs[3];
                #endregion


                pInfo.pos = message.readUInt8();
                pInfo.score = message.readInt32();

                if (GameData.GlobleRoomType == RoomType.NN)
                {
                    Int32 basescore= message.readInt32();//牛牛下注的分数
                }
               // pInfo.fangPaoScore = message.readUInt32();
                byte cardCount = message.readUInt8();//手牌
                for (int j = 0; j < cardCount; j++)
                    pInfo.localCardList.Add(message.readUInt32());


                GameData.m_PlayerInfoList.Add(pInfo);
                hInfo.playerList.Add(pInfo);
            }


            uint opCount = (uint)message.readInt32();//操作数
            uint roomID = 0;
            for (int j = 0; j < opCount; j++)
            {
                RoomMessageType type = (RoomMessageType)message.readUInt8();
                string strs = Enum.GetName(typeof(RoomMessageType), type);
              
                if (type == RoomMessageType.PlayerInCard)
                {
                 

                    roomID = message.readUInt32();
                    byte pos = message.readUInt8();
                    uint card = message.readUInt32();
                    uint resCardCount = message.readUInt32();
                    strs += ":" + pos;
                    strs += ":" + card;
                    strs += ":" + resCardCount;
                    hInfo.operateList.Add(strs);
                }
                if (type == RoomMessageType.playerOperate)
                {
                    //roomID = message.readUInt32();
                    //byte pos = message.readUInt8();
                    //CardOperateType opType = (CardOperateType)message.readUInt8();
                    //uint opCard = message.readUInt32();
                    //byte outPos = message.readUInt8();
                    //uint needCardCount = message.readUInt32();
                    //strs += ":" + pos;
                    //strs += ":" + Enum.GetName(typeof(CardOperateType), opType);
                    //strs += ":" + opCard;
                    //strs += ":" + outPos;
                    //hInfo.operateList.Add(strs);


                     roomID = message.readUInt32();
                    byte pos = message.readUInt8();
                    CardOperateType opType = (CardOperateType)message.readUInt8();
                    int count1 = message.readInt32();
                    List<uint> OutCardList = new List<uint>();
                    for (int k = 0; k < count1; k++)
                    {
                        uint card = message.readUInt32();
                        OutCardList.Add(card);
                    }
                    int PlayerLeftCard = message.readInt32();
                    byte outPos = message.readUInt8();
                    //uint needCard = message.readUInt32();

                    bool isAnGang = message.readBool();

                    strs += ":" + pos;
                    strs += ":" + Enum.GetName(typeof(CardOperateType), opType);
                    strs += ":" + OutCardList[0];
                    strs += ":" + outPos;
                    hInfo.operateList.Add(strs);
                }
            
            }
            GameData.m_HuiFangList.Add(hInfo);
        }
        UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_ZhanJiRoundInfo, OpenPanelType.MinToMax);
    }

    /// <summary>
    /// 战绩 请求回调
    /// </summary>
    /// <param name="message"></param>
    private void onRoomRecordList(NetworkMessage message)
    {
        GameData.m_RecordList.Clear();
        uint totalCount = message.readUInt32();
        uint startIndex = message.readUInt32();
        uint count = message.readUInt32();
        for (int i = 0; i < count; i++)
        {
            RecordListInfo info = new RecordListInfo();

           
            byte roomType = message.readUInt8();

            RoomType t = (RoomType)roomType;

            info.roomType = t;//房间类型
            info.guid = message.readUInt64();//每条录像的唯一标识
            info.id = message.readUInt32();
            info.fangZhuGuid = message.readUInt64();
            info.fangZhuName = message.readString();
            info.startTime = myFunction.Instance.fromSecondsFromGameBegin(message.readUInt32());
            info.endTime = myFunction.Instance.fromSecondsFromGameBegin(message.readUInt32());

            byte pCount = message.readUInt8();
            for (int k = 0; k < pCount; k++)
            {
                string strs = message.readUInt64().ToString();
                strs += "@" + message.readString();
                strs += "@" + message.readInt32();
                strs += "@" + message.readString();
                info.playerInfo.Add(strs);//guid@name@socre@headid
            }
            GameData.m_RecordList.Add(info);
        }

        if (UIZhanJiList.Instance != null) UIZhanJiList.Instance.ShowItems();
    }


    string GetPlayerInfo(ulong guid)
    {
        string str = "";
        for (int i = 0; i < GameData.m_ChooseRecordListInfo.playerInfo.Count; i++)
        {
            string[] strs = GameData.m_ChooseRecordListInfo.playerInfo[i].Split('@');
            if (guid == ulong.Parse(strs[0]))
            {
                str = GameData.m_ChooseRecordListInfo.playerInfo[i];
                break;
            }
        }
        return str;
    }
}
