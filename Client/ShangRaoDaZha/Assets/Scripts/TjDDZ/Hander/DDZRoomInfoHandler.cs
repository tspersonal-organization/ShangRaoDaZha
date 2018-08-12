using FrameworkForCSharp.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkForCSharp.NetWorks;
using System;

public class DDZRoomInfoHandler {

    public DDZRoomInfoHandler()
    {

        RoomManagerHandler.addServerHandler(RoomMessageType.RoomInfo, onRoomInfo);//房间基本信息
        //RoomManagerHandler.addServerHandler(RoomMessageType.PlayerEnter, onPlayerEnter);//玩家进入房间
        //RoomManagerHandler.addServerHandler(RoomMessageType.PlayerReadyForRoom, onPlayerReadyForRoom);//玩家准备开始游戏
        //RoomManagerHandler.addServerHandler(RoomMessageType.PlayerReadyForNext, onPlayerReadyForNext);//玩家准备下一局
        //RoomManagerHandler.addServerHandler(RoomMessageType.RoomActive, onRoomActive);//房间激活
        //RoomManagerHandler.addServerHandler(RoomMessageType.ZhuangPosition, onZhuangPosition);//定庄
        //RoomManagerHandler.addServerHandler(RoomMessageType.WaitPlayerChooseScore, onWaitPlayerChooseScore);//等待玩家选择积分
        //RoomManagerHandler.addServerHandler(RoomMessageType.PlayerChooseScore, onPlayerChooseScore);//玩家选择了分数
        //RoomManagerHandler.addServerHandler(RoomMessageType.PlayerHoldCards, onPlayerHoldCards);//开始发牌
        //RoomManagerHandler.addServerHandler(RoomMessageType.PlayerInCard, onPlayerInCard);//玩家摸牌
        //RoomManagerHandler.addServerHandler(RoomMessageType.playerOperate, onPlayerOperate);//玩家操作
        //RoomManagerHandler.addServerHandler(RoomMessageType.GameOver, onGameOver);//结算
        //RoomManagerHandler.addServerHandler(RoomMessageType.GameDispose, onGameDispose);//总结算
        //RoomManagerHandler.addServerHandler(RoomMessageType.PlayerOperateSuccess, onPlayerOperateSuccess);//操作成功
        //RoomManagerHandler.addServerHandler(RoomMessageType.PlayerQueryDisposeRoom, PlayerQueryDisposeRoom);//发起投票
        //RoomManagerHandler.addServerHandler(RoomMessageType.PlayerAgreeDisposeResult, PlayerAgreeDisposeResult);//投票结果
        //RoomManagerHandler.addServerHandler(RoomMessageType.PlayerOnForce, onPlayerOnForce);//获得焦点
        //RoomManagerHandler.addServerHandler(RoomMessageType.PlayerSpeak, onPlayerSpeak);//语音
        //RoomManagerHandler.addServerHandler(RoomMessageType.PlayerLeave, onPlayerLeave);//玩家离开房间
    }

    /// <summary>
    /// 返回房间信息
    /// </summary>
    /// <param name="obj"></param>
    private void onRoomInfo(NetworkMessage message)
    {
        Log.Debug("房间基本信息");
        GameData.m_PlayerInfoList.Clear();
        GameData.m_HoldCardsList.Clear();
        GameDataFunc.ClearData();
        GameDataFunc.ClearDataObj();
        TableInfo info = new TableInfo();
        info.id = message.readUInt32();//房间号
        info.fangZhuGuid = message.readUInt64();//房主ID
        info.configRoundIndex = message.readUInt8();//几局
                                                    //  info.configFangChongIndex = message.readUInt8();
                                                    // info.configShengPaiIndex = message.readUInt8();
                                                    // info.configDaZiIndex = message.readUInt8();
        info.configPayIndex = message.readUInt8();
        info.configPlayerIndex = message.readUInt8();

        info.makerPos = message.readUInt8();
        info.lianZhuangCount = message.readUInt32();
        info.roomState = (RoomStatusType)message.readUInt8();
        info.curGameCount = message.readUInt32();
        info.isQueryLeaveRoom = message.readBool();
        if (info.isQueryLeaveRoom)
        {
            info.queryLeaveRoomWaitTime = message.readUInt32();
            byte count = message.readUInt8();
            for (int i = 0; i < count; i++)
                info.operateLeaveRoomList.Add(message.readUInt8());
        }

        byte pCount = message.readUInt8();//玩家个数
        for (int i = 0; i < pCount; i++)
        {
            PlayerInfo pInfo = new PlayerInfo();
            pInfo.pos = message.readUInt8();
            pInfo.N = message.readFloat();
            pInfo.E = message.readFloat();
            pInfo.guid = message.readUInt64();
            pInfo.sex = message.readUInt8();
            pInfo.isStartReady = message.readBool();
            pInfo.isNextReady = message.readBool();
            pInfo.isForce = message.readBool();
            pInfo.ip = message.readString();
            pInfo.mask = message.readString();
            pInfo.name = message.readString();
            pInfo.headID = message.readString();
            pInfo.changeScore = message.readInt32();
            pInfo.score = message.readInt32();
            pInfo.menCount = message.readUInt8();
            pInfo.huType = (HuType)message.readUInt8();
            pInfo.isOperateFangPao = message.readBool();
            pInfo.fangPaoScore = message.readUInt32();
            pInfo.totalHuCount = message.readUInt32();
            pInfo.totalMakerCount = message.readUInt32();
            byte oCount = message.readUInt8();
            for (int k = 0; k < oCount; k++)
            {
                OpreateCardInfo opInfo = new OpreateCardInfo();
                opInfo.pos = message.readUInt8();
                opInfo.opType = (CatchType)message.readUInt8();
                opInfo.opCard = message.readUInt32();
                pInfo.operateCardList.Add(opInfo);
            }
            oCount = message.readUInt8();
            for (int k = 0; k < oCount; k++)
                pInfo.localCardList.Add(message.readUInt32());
            oCount = message.readUInt8();
            for (int k = 0; k < oCount; k++)
                pInfo.outCardList.Add(message.readUInt32());
            oCount = message.readUInt8();
            for (int k = 0; k < oCount; k++)
                pInfo.limitPengCardList.Add(message.readUInt32());
            GameData.m_PlayerInfoList.Add(pInfo);
        }

        if (info.roomState == RoomStatusType.Play)
        {
            info.isWaitFangPao = !message.readBool();
            info.lastOutCardPos = message.readUInt8();
            info.waitOutCardPos = message.readUInt8();
            info.resCardCount = message.readUInt32();
            info.isOutCardInfo = message.readBool();
            if (info.isOutCardInfo)
            {
                info.outCardPos = message.readUInt8();
                info.outCardNumber = message.readUInt32();
            }

            info.isInCardInfo = message.readBool();
            if (info.isInCardInfo)
            {
                info.inCardPos = message.readUInt8();
                info.inCardNumber = message.readUInt32();
            }

            info.isQiangGangHu = message.readBool();
            if (info.isQiangGangHu)
            {
                info.qiangGangPos = message.readUInt8();
                info.qiangGangCard = message.readUInt32();
                byte qgCount = message.readUInt8();
                for (int i = 0; i < qgCount; i++)
                {
                    info.operateQiangGangList.Add(message.readUInt8());
                }
            }
        }
        else if (info.roomState == RoomStatusType.Over)
        {
            RoundOverInfo overInfo = new RoundOverInfo();
            overInfo.isHuPai = message.readBool();
            if (overInfo.isHuPai)
            {
                overInfo.huPos = message.readUInt8();
                overInfo.huCard = message.readUInt32();
                overInfo.isQiangGuangHu = message.readBool();
                if (overInfo.isQiangGuangHu)
                {
                    overInfo.qiangGangPos = message.readUInt8();
                    overInfo.qiangGangCard = message.readUInt32();
                }
                overInfo.isDianPaoHu = message.readBool();
                if (overInfo.isDianPaoHu)
                {
                    overInfo.dianPaoPos = message.readUInt8();
                    overInfo.dianPaoCard = message.readUInt32();
                }
            }
            GameData.m_RoundOverInfo = overInfo;
        }
        GameData.m_TableInfo = info;
        GameData.Dice1 = GameData.GenerateDice(1);
        GameData.Dice2 = GameData.GenerateDice(2);
        ManagerScene.Instance.LoadScene(SceneType.Game);
    }
}
