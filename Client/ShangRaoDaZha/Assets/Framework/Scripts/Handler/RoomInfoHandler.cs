using FrameworkForCSharp.Utils;
using FrameworkForCSharp.NetWorks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using S2CEntity;

public class RoomInfoHandler
{
    public RoomInfoHandler()
    {
        RoomManagerHandler.addServerHandler(RoomMessageType.BaoPai, OnBaoPaiAsk);//广播包牌
        RoomManagerHandler.addServerHandler(RoomMessageType.BaoPaiPosition, OnBaoPaiinfo);//广播包牌人信息


        RoomManagerHandler.addServerHandler(RoomMessageType.WhoQiangZhuang, OnWhoQiangZhuang);//广播牛有谁抢庄
        RoomManagerHandler.addServerHandler(RoomMessageType.ShowCard, OnNiuNiuShowCard);//广播牛牛亮牌
        RoomManagerHandler.addServerHandler(RoomMessageType.QiangZhuang, OnNiuNiuQiangZhuang);//广播开始抢庄
        RoomManagerHandler.addServerHandler(RoomMessageType.ChipInInfo, OnWhoChipIn);//广播谁下多少住


        RoomManagerHandler.addServerHandler(RoomMessageType.OnePlayerOver, OnPlayerFinish);//摸个玩家出牌结束 确定上中下游
        RoomManagerHandler.addServerHandler(RoomMessageType.RoomCreate, OnCreatRoomResult);//创建房间返回（正常创建  代理创建都会有返回）

        RoomManagerHandler.addServerHandler(RoomMessageType.OnTuoGuan, OnTuguanResult);//托管返回广播
        RoomManagerHandler.addServerHandler(RoomMessageType.FriendCard, onGetFriendCard);//开始广播朋友卡
        RoomManagerHandler.addServerHandler(RoomMessageType.FriendPresent, onShowFriendCard);//队友出朋友卡
        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerPass, onNextPlayer);//下个玩家出牌

        RoomManagerHandler.addServerHandler(RoomMessageType.RewardNumb, onJiangMa);//广播奖码

        RoomManagerHandler.addServerHandler(RoomMessageType.RoomInfo, onRoomInfo);//房间基本信息
        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerEnter, onPlayerEnter);//玩家进入房间
        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerReadyForRoom, onPlayerReadyForRoom);//玩家准备开始游戏
        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerReadyForNext, onPlayerReadyForNext);//玩家准备下一局
        RoomManagerHandler.addServerHandler(RoomMessageType.RoomActive, onRoomActive);//房间激活
        RoomManagerHandler.addServerHandler(RoomMessageType.ZhuangPosition, onZhuangPosition);//定庄
        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerInCard, onPlayerInCard);//玩家摸牌
        RoomManagerHandler.addServerHandler(RoomMessageType.WaitPlayerChooseScore, onWaitPlayerChooseScore);//等待玩家选择积分
        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerChooseScore, onPlayerChooseScore);//玩家选择了分数
        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerHoldCards, onPlayerHoldCards);//开始发牌
       

        RoomManagerHandler.addServerHandler(RoomMessageType.playerOperate, onPlayerOperate);//玩家操作
        RoomManagerHandler.addServerHandler(RoomMessageType.GameOver, onGameOver);//结算
        RoomManagerHandler.addServerHandler(RoomMessageType.GameDispose, onGameDispose);//总结算（房间解散）

        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerOperateSuccess, onPlayerOperateSuccess);//操作成功
        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerQueryDisposeRoom, PlayerQueryDisposeRoom);//发起投票


        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerAgreeDisposeResult, PlayerAgreeDisposeResult);//投票结果


        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerOnForce, onPlayerOnForce);//获得焦点

        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerSpeak, onPlayerSpeak);//语音


        RoomManagerHandler.addServerHandler(RoomMessageType.PlayerLeave, onPlayerLeave);//玩家离开房间

        RoomManagerHandler.addServerHandler(RoomMessageType.MagicCard, onMagicCard);//载宝骰子和面牌

        RoomManagerHandler.addServerHandler(RoomMessageType.ReduceGold, OnPlayerGoldReduce);//玩家金币扣除
    }

    /// <summary>
    /// 广播谁包牌啦
    /// </summary>
    /// <param name="message"></param>
    private void OnBaoPaiinfo(NetworkMessage message)
    {
        //message.writeUInt32(codeId);//房间号
        //message.writeUInt8((byte)position);//包牌的位置

        message.readUInt32();
       byte pos= message.readUInt8();//包牌人信息
        PartGameOverControl.instance.HelperPos = (int)pos;
        if (DzViewGame.Instance != null) DzViewGame.Instance.OnShowBaoPaiInfo( pos);


    }


    /// <summary>
    /// 询问是否包牌
    /// </summary>
    /// <param name="message"></param>
    private void OnBaoPaiAsk(NetworkMessage message)
    {
        //message.writeUInt32(codeId);//房间号
        //message.writeUInt8((byte)position);//询问的位置

        message.readUInt32();
      byte pos=  message.readUInt8();
        Debug.Log("广播包牌---"+ pos);
        if (pos == DzViewGame.Instance.LocalPos|| pos == DzViewGame.Instance.SelfPos)
        {
            DzViewGame.Instance.BaoPaiTipPanel.SetActive(true);
        }

    }

    /// <summary>
    /// 广播谁下多少注
    /// </summary>
    /// <param name="obj"></param>
    private void OnWhoChipIn(NetworkMessage message)
    {
        byte pos = message.readUInt8();//
        uint SelfChip = message.readUInt32();//给自己下的注
        int count = message.readInt32();//下 了多少其他玩家
        Dictionary<byte, uint> OtherChipDic = new Dictionary<byte, uint>();
        for (int i = 0; i < count; i++)
        {
            byte pos1 = message.readUInt8();//其他玩家的位置
            uint SelfChip1 = message.readUInt32();//给其他玩家下注下了多少
            OtherChipDic[pos1] = SelfChip1;
        }

        NiuNiuGame.Instance.PlayerChipIn(pos,SelfChip, OtherChipDic);

      //  Debug.LogError("-----------chipPos----------------"+ pos);
    }




  
    /// <summary>
    /// 广播有谁抢庄
    /// </summary>
    /// <param name="obj"></param>
    private void OnWhoQiangZhuang(NetworkMessage message)
    {
        byte pos = message.readUInt8();
        bool Operate = message.readBool();
        NiuNiuGame.Instance.OnWhoQiangZhuang(pos,Operate);

    }



    /// <summary>
    /// 牛牛广播亮牌
    /// </summary>
    /// <param name="message"></param>
    private void OnNiuNiuShowCard(NetworkMessage message)
    {
        uint pos = message.readUInt8();//玩家位置

        NNType PeiPaiType = (NNType)message.readUInt8();//牌型
        UInt32 FanBeiCount = message.readUInt32();//翻倍数

        List<List<uint>> PeiPaiInfo = new List<List<uint>>();//0 分数  1 三张牌  2 剩余两张牌  
        uint listcount = message.readUInt32();
        for (int l = 0; l < listcount; l++)
        {
            List<uint> PeiPaiItem = new List<uint>();
            uint listcount1 = message.readUInt32();
            for (int n = 0; n < listcount1; n++)
            {
                PeiPaiItem.Add(message.readUInt32());
            }
            PeiPaiInfo.Add(PeiPaiItem);
        }

      //  Debug.LogError("----------PeiPaiType-------------------"+ (int)PeiPaiType);
        for (int i = 0; i < PeiPaiInfo.Count; i++)
        {
            for (int j = 0; j < PeiPaiInfo[i].Count; j++)
            {
              //  Debug.LogError(PeiPaiInfo[i][j]);
            }
        }
        NiuNiuGame.Instance.PlayerShowCard(pos, PeiPaiType, FanBeiCount, PeiPaiInfo);//玩家亮牌信息

    }


    /// <summary>
    /// 牛牛抢庄广播
    /// </summary>
    /// <param name="obj"></param>
    private void OnNiuNiuQiangZhuang(NetworkMessage message)
    {
        NiuNiuGame.Instance.OnQiangZhuang();
    }

    /// <summary>
    /// 玩家金币扣除
    /// </summary>
    /// <param name="obj"></param>
    private void OnPlayerGoldReduce(NetworkMessage message)
    {
        uint ReduceGold = message.readUInt32();
        int count = message.readInt32();
        for (int i = 0; i < count; i++)
        {
            byte pos = message.readUInt8();
            GameDataFunc.GetPlayerInfo(pos).Gold = message.readInt64();
          
            //if (GameData.GlobleRoomType == RoomType.XYGQP)
            //{

            //    if (DDZJinBi.Instance != null) DDZJinBi.Instance.onPlayerEnter(GameDataFunc.GetPlayerInfo(pos));

            //}
             if (GameData.GlobleRoomType == RoomType.WDH)
            {
              
                    if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.onPlayerEnter(GameDataFunc.GetPlayerInfo(pos),true);
              

            }
            else if (GameData.GlobleRoomType == RoomType.ZB)
            {
              
                    if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.onPlayerEnter(GameDataFunc.GetPlayerInfo(pos),true);
              

            }
        }

        GameData.GlobleTipString = "消耗金币" + ReduceGold.ToString();
        UIManager.Instance.ShowUiPanel(UIPaths.GlobleTipPanel);//打开提示面板


    }



    /// <summary>
    /// 栽宝面牌和骰子
    /// </summary>
    /// <param name="message"></param>
    private void onMagicCard(NetworkMessage message)
    {
        ulong RoomGuid = message.readUInt64();
        uint DeviceOne = message.readUInt32();
        uint DeviceTwo = message.readUInt32();
        uint DeviceThree = message.readUInt32();
        uint DeviceFour = message.readUInt32();

        uint MainCard = message.readUInt32();
        uint MagicCard = message.readUInt32();
        GameData.Dice1 = (int)DeviceOne;
        GameData.Dice2 = (int)DeviceTwo;
        GameData.Dice3 = (int)DeviceThree;
        GameData.Dice4 = (int)DeviceFour;

        GameData.m_TableInfo.MianCard = MainCard;
        GameData.m_TableInfo.MagicCard = MagicCard;
        GameData.m_TableInfo.ForbiddenIndex = 135 - ((int)(GameData.Dice3 + GameData.Dice4) * 2) - 1;
        if (!IsPiPei)
        {
            if (ZaiBaiGame.Instance != null)
            {
                ZaiBaiGame.Instance.onDeciesShow(DeviceOne, DeviceTwo, DeviceThree, DeviceFour, MainCard, MagicCard);
            }
        }
        else
        {
            if (ZaiBaiGameJinBi.Instance != null)
            {
                ZaiBaiGameJinBi.Instance.onDeciesShow(DeviceOne, DeviceTwo, DeviceThree, DeviceFour, MainCard, MagicCard);
            }
        }
        
     
    }


    /// <summary>
    /// 无挡胡广播奖码
    /// </summary>
    /// <param name="obj"></param>
    private void onJiangMa(NetworkMessage message)
    {
        int count = message.readInt32();
        List<uint> JiangMaList = new List<uint>();
        for (int i = 0; i < count; i++)
        {
            JiangMaList.Add(message.readUInt32());
        }

        if (!IsPiPei)//匹配场
        {
            if (GameData.GlobleRoomType == RoomType.WDH)
            {
                if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.OnJiangMa(JiangMaList);
            }
        }
        else
        {
            if (GameData.GlobleRoomType == RoomType.WDH)
            {
                if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.OnJiangMa(JiangMaList);
            }
        }
        
    }

    /// <summary>
    /// 玩家出牌结束
    /// </summary>
    /// <param name="obj"></param>
    private void OnPlayerFinish(NetworkMessage message)
    {
        if (GameData.GlobleRoomType == RoomType.PK)
        {
            GameData.FinishPlayerPos = new List<FinishPlayer>();
            int playerCount = message.readInt32();//完成玩家人数
            for (int i = 0; i < playerCount; i++)
            {
                int pos1 = message.readUInt8();//位置
                int index = message.readUInt8();//第几个完成

                FinishPlayer player = new FinishPlayer();
                player.pos = pos1;
                player.index = index;
                GameData.FinishPlayerPos.Add(player);
            }
          

            if (!IsPiPei)
            {
                if (DzViewGame.Instance != null) DzViewGame.Instance.OnSetFinishPlayerIndex();
            }
            else
            {
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.OnSetFinishPlayerIndex();
              
            }
        }
        else if (GameData.GlobleRoomType == RoomType.WDH)
        {

        }
      

    }


    /// <summary>
    /// 创建房间返回
    /// </summary>
    /// <param name="obj"></param>
    private void OnCreatRoomResult(NetworkMessage message)
    {
        uint roomid = message.readUInt32();
        bool isAngentCreat = message.readBool();
        if (isAngentCreat)//是代理开房
        {
            UIManager.Instance.HideUiPanel(UIPaths.PanelCreatRoom);
            UIManager.Instance.ShowUiPanel(UIPaths.MyRoomPanel, OpenPanelType.MinToMax);

        }
    }

    /// <summary>
    /// 托管返回
    /// </summary>
    /// <param name="obj"></param>
    private void OnTuguanResult(NetworkMessage message)
    {
        if (GameData.GlobleRoomType == RoomType.PK)
        {
            int roomNum = (int)message.readUInt32();
            int pos = (int)message.readUInt8();
            bool isAi = message.readBool();


            if (!IsPiPei)
            {
                //  if (DzViewGame.Instance != null) DzViewGame.Instance.OnPlayerAi(roomNum, pos, isAi);
            }
            else
            {
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.OnPlayerAi(roomNum, pos, isAi);
            }
        }

        else if (GameData.GlobleRoomType == RoomType.WDH)
        {
            int roomNum = (int)message.readUInt32();
            int pos = (int)message.readUInt8();
            bool isAi = message.readBool();


            if (!IsPiPei)
            {
                //  if (DzViewGame.Instance != null) DzViewGame.Instance.OnPlayerAi(roomNum, pos, isAi);
            }
            else
            {
                if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.OnPlayerAi(roomNum, pos, isAi);
            }
        }
        else if (GameData.GlobleRoomType == RoomType.ZB)
        {
            int roomNum = (int)message.readUInt32();
            int pos = (int)message.readUInt8();
            bool isAi = message.readBool();


            if (!IsPiPei)
            {
                //  if (DzViewGame.Instance != null) DzViewGame.Instance.OnPlayerAi(roomNum, pos, isAi);
            }
            else
            {
                if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.OnPlayerAi(roomNum, pos, isAi);
            }
        }
       
    }


    /// <summary>
    /// 下个玩家出牌
    /// </summary>
    /// <param name="obj"></param>
    private void onNextPlayer(NetworkMessage message)
    {
        Debug.Log("广播由谁出牌");
        uint RoomId = message.readUInt32();
        PositionType Playerpos =(PositionType) message.readUInt8();
        int NextPlayerPos = message.readUInt8();

       //byte IsGiveToFriend = message.readUInt8();

        PuKeOperateType type = (PuKeOperateType)message.readUInt8();
        if (GameData.GlobleRoomType == RoomType.PK)
        {
            if (!IsPiPei)
            {
                if (DzViewGame.Instance != null) DzViewGame.Instance.ShowOperatePanle(NextPlayerPos, type);
            }
            else
            {
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.ShowOperatePanle(NextPlayerPos, type);
            }
            switch (type)
            {
                case PuKeOperateType.ChuPai:
                    Debug.Log(Playerpos+"/"+"出牌");
                    break;
                case PuKeOperateType.JiePai:
                    Debug.Log(Playerpos + "/" + "接牌");
                    break;

            }
           
        }
        if (GameData.GlobleRoomType == RoomType.WDH)
        {
          //  if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.onPlayerInCard(pos, card, resCards);
        }
           
    }


    /// <summary>
    /// 其他玩家出了friendcard
    /// </summary>
    /// <param name="message"></param>
    private void onShowFriendCard(NetworkMessage message)
    {
        if (GameData.GlobleRoomType == RoomType.PK)
        {
            int pos = (int)message.readUInt8();
            PartGameOverControl.instance.HelperPos = (int)pos;
            uint card = message.readUInt32();
           
            if (!IsPiPei)
            {
                PartGameOverControl.instance.HelperPos = (int)pos;
                if (DzViewGame.Instance != null) DzViewGame.Instance.OnShowFriendCard(pos, card);
            }
            else
            {
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.OnShowFriendCard(pos, card);

            }
        }
      
    }
    /// <summary>
    /// 获得friendcard
    /// </summary>
    /// <param name="obj"></param>
    uint FriendCard;
    private void onGetFriendCard(NetworkMessage message)
    {
        if (GameData.GlobleRoomType == RoomType.PK)
        {
            int pos = (int)message.readUInt8();
            uint card = message.readUInt32();
            this.FriendCard = card;
            GameData.m_TableInfo.FriendCard = card;
            if (!IsPiPei)
            {
                if (DzViewGame.Instance != null) DzViewGame.Instance.OnFriendCard(pos, GameData.m_TableInfo.FriendCard);
            }
            else
            {
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.OnFriendCard(pos, GameData.m_TableInfo.FriendCard);
               // if (DDZJinBi.Instance != null) DDZJinBi.Instance.onPlayerLeave((int)pos);

            }
        }
        
    }

    /// <summary>
    /// 玩家离开房间
    /// </summary>
    /// <param name="message"></param>
    private void onPlayerLeave(NetworkMessage message)
    {
        Log.Debug("玩家离开房间");
        uint roomID = message.readUInt32();
        byte pos = message.readUInt8();
        string addr = message.readString();
        if (GameData.GlobleRoomType == RoomType.PK)
        {
            if (!IsPiPei)
            {
                if (DzViewGame.Instance != null) DzViewGame.Instance.onPlayerLeave((int)pos);
            }
            else
            {
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.onPlayerLeave((int)pos);

            }
            // if (Game.Instance != null) Game.Instance.onPlayerLeave(pos);

        }

        //else if (GameData.GlobleRoomType == RoomType.WDH)
        //{
        //    if (!IsPiPei)
        //    {
        //        if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.onPlayerLeave(pos);
        //    }
        //    else
        //    {
        //        if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.onPlayerLeave(pos);
        //    }

        //}
        //else if (GameData.GlobleRoomType == RoomType.ZB)
        //{
        //    if (!IsPiPei)
        //    {
        //        if (ZaiBaiGame.Instance != null) ZaiBaiGame.Instance.onPlayerLeave(pos);
        //    }
        //    else
        //    {
        //        if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.onPlayerLeave(pos);
        //    }

        //}

        //else if (GameData.GlobleRoomType == RoomType.NN)
        //{

        //    NiuNiuGame.Instance.onPlayerLeave(pos);
        //    //if (!IsPiPei)
        //    //{
        //    //    if (ZaiBaiGame.Instance != null) NiuNiuGame.Instance.onPlayerLeave(pos);
        //    //}
        //    //else
        //    //{
        //    //    if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.onPlayerLeave(pos);
        //    //}

        //}

        GameDataFunc.RemovePlayerinfo(pos);//移除玩家

    }


    /// <summary>
    /// 玩家聊天语音
    /// </summary>
    /// <param name="message"></param>
    private void onPlayerSpeak(NetworkMessage message)
    {

        if (GameData.GlobleRoomType == RoomType.PK)
        {
            #region
            uint roomID = message.readUInt32();
            string connect = message.readString();
            string[] strs = connect.Split('@');
            if (strs[0] == "1")//语音
            {
                connect = roomID.ToString() + "@" + connect;
                GameData.m_VoiceQueue.Enqueue(connect);
            }
            else if (strs[0] == "2")//手输文字
            {
                // if (Game.Instance != null)
                //   Game.Instance.onPlayerChatTxt(connect);
                if (!IsPiPei)
                {
                    if (DzViewGame.Instance != null)
                        DzViewGame.Instance.onPlayerSendChatFace(roomID, connect);
                }
                else
                {
                    if (DDZJinBi.Instance != null)
                        DDZJinBi.Instance.onPlayerSendChatFace(roomID, connect);
                   
                }

               
            }
            else if (strs[0] == "3")//表情
            {
                if (!IsPiPei)
                {
                    if (DzViewGame.Instance != null)// "3@" + Player.Instance.guid + "@" + faceID;
                        DzViewGame.Instance.onPlayerSendFaceChatFace(roomID, connect);
                }
                else
                {
                    if (DDZJinBi.Instance != null)// "3@" + Player.Instance.guid + "@" + faceID;
                        DDZJinBi.Instance.onPlayerSendFaceChatFace(roomID, connect);
                 

                }
                //if (Game.Instance != null)
                //  Game.Instance.onPlayerSendChatFace(connect);
               
            }
            else if (strs[0] == "5")//快捷文字
            {

                if (!IsPiPei)
                {
                    if (DzViewGame.Instance != null)
                        DzViewGame.Instance.onPlayerSendChatFace(roomID, connect);
                }
                else
                {
                    if (DDZJinBi.Instance != null)
                        DDZJinBi.Instance.onPlayerSendChatFace(roomID, connect);
                   


                }
             
            }

            #endregion
        }
        else if (GameData.GlobleRoomType == RoomType.WDH)
        {

            #region
            uint roomID = message.readUInt32();
            string connect = message.readString();
            string[] strs = connect.Split('@');
            if (strs[0] == "1")//语音
            {
                connect = roomID.ToString() + "@" + connect;
                GameData.m_VoiceQueue.Enqueue(connect);
            }
            else if (strs[0] == "2")//手输文字
            {
                // if (Game.Instance != null)
                //   Game.Instance.onPlayerChatTxt(connect);
                if (!IsPiPei)
                {
                    if (WuDangHuGame.Instance != null)
                        WuDangHuGame.Instance.onPlayerSendChatFace(roomID, connect);
                }
                else
                {
                    if (WuDangHuGameJinBi.Instance != null)
                        WuDangHuGameJinBi.Instance.onPlayerSendChatFace(roomID, connect);
                }
              
            }
            else if (strs[0] == "3")//表情
            {
                //if (Game.Instance != null)
                //  Game.Instance.onPlayerSendChatFace(connect);
                if (!IsPiPei)
                {
                    if (WuDangHuGame.Instance != null)// "3@" + Player.Instance.guid + "@" + faceID;
                        WuDangHuGame.Instance.onPlayerSendChatFace(connect);
                }
                else
                {
                    if (WuDangHuGameJinBi.Instance != null)// "3@" + Player.Instance.guid + "@" + faceID;
                        WuDangHuGameJinBi.Instance.onPlayerSendChatFace(connect);
                }

               
            }
            else if (strs[0] == "6")//快捷文字
            {
                if (!IsPiPei)
                {
                    if (WuDangHuGame.Instance != null)
                        WuDangHuGame.Instance.onPlayerSendChatFace(roomID, connect);
                }
                else
                {
                    if (WuDangHuGameJinBi.Instance != null)
                        WuDangHuGameJinBi.Instance.onPlayerSendChatFace(roomID, connect);
                }
              
            }

            #endregion
        }

        else if (GameData.GlobleRoomType == RoomType.ZB)
        {
            uint roomID = message.readUInt32();
            string connect = message.readString();
            string[] strs = connect.Split('@');
            if (strs[0] == "1")//语音
            {
                connect = roomID.ToString() + "@" + connect;
                GameData.m_VoiceQueue.Enqueue(connect);
            }
            else if (strs[0] == "2")//手输文字
            {
                // if (Game.Instance != null)
                //   Game.Instance.onPlayerChatTxt(connect);
                if (!IsPiPei)
                {
                    if (ZaiBaiGame.Instance != null)
                        ZaiBaiGame.Instance.onPlayerSendChatFace(roomID, connect);
                }
                else
                {
                    if (ZaiBaiGameJinBi.Instance != null)
                        ZaiBaiGameJinBi.Instance.onPlayerSendChatFace(roomID, connect);
                }
               
            }
            else if (strs[0] == "3")//表情
            {
                //if (Game.Instance != null)
                //  Game.Instance.onPlayerSendChatFace(connect);
                if (!IsPiPei)
                {
                    if (ZaiBaiGame.Instance != null)// "3@" + Player.Instance.guid + "@" + faceID;
                        ZaiBaiGame.Instance.onPlayerSendChatFace(connect);
                }
                else
                {
                    if (ZaiBaiGameJinBi.Instance != null)// "3@" + Player.Instance.guid + "@" + faceID;
                        ZaiBaiGameJinBi.Instance.onPlayerSendChatFace(connect);
                }
               
            }
            else if (strs[0] == "6")//快捷文字
            {
                if (!IsPiPei)
                {
                    if (ZaiBaiGame.Instance != null)
                        ZaiBaiGame.Instance.onPlayerSendChatFace(roomID, connect);
                }
                else
                {
                    if (ZaiBaiGameJinBi.Instance != null)
                        ZaiBaiGameJinBi.Instance.onPlayerSendChatFace(roomID, connect);
                }
               
            }
        }

    }

    /// <summary>
    /// 玩家焦点操作
    /// </summary>
    /// <param name="message"></param>
    private void onPlayerOnForce(NetworkMessage message)
    {
        if (GameData.GlobleRoomType == RoomType.PK)
        {
            Log.Debug("焦点");
            uint roomID = message.readUInt32();
            byte pos = message.readUInt8();
            bool isForce = message.readBool();
            string ip = message.readString();
            string mask = message.readString();
            if (roomID == GameData.m_TableInfo.id)
            {
                //if (Game.Instance != null)
                // Game.Instance.onPlayerOnForce(pos, isForce);

                if (!IsPiPei)
                {
                    if (DzViewGame.Instance != null)
                        DzViewGame.Instance.onPlayerOnForce(pos, isForce);
                }
                else
                {
                    if (DDZJinBi.Instance != null)
                        DDZJinBi.Instance.onPlayerOnForce(pos, isForce);
                  

                }

              
            }
        }

        else if (GameData.GlobleRoomType == RoomType.WDH)
        {
            Log.Debug("焦点");
            uint roomID = message.readUInt32();
            byte pos = message.readUInt8();
            bool isForce = message.readBool();
            string ip = message.readString();
            string mask = message.readString();
            if (roomID == GameData.m_TableInfo.id)
            {
                if (!IsPiPei)
                {
                    if (WuDangHuGame.Instance != null)
                        WuDangHuGame.Instance.onPlayerOnForce(pos, isForce);
                }
                else
                {
                    if (WuDangHuGameJinBi.Instance != null)
                        WuDangHuGameJinBi.Instance.onPlayerOnForce(pos, isForce);
                }
               
            }

        }
        else if (GameData.GlobleRoomType == RoomType.ZB)
        {
            Log.Debug("焦点");
            uint roomID = message.readUInt32();
            byte pos = message.readUInt8();
            bool isForce = message.readBool();
            string ip = message.readString();
            string mask = message.readString();
            if (roomID == GameData.m_TableInfo.id)
            {
                if (!IsPiPei)
                {
                    if (ZaiBaiGame.Instance != null)
                        ZaiBaiGame.Instance.onPlayerOnForce(pos, isForce);
                }
                else
                {
                    if (ZaiBaiGameJinBi.Instance != null)
                        ZaiBaiGameJinBi.Instance.onPlayerOnForce(pos, isForce);
                }
               
            }

        }

        else if (GameData.GlobleRoomType == RoomType.NN)
        {
            Log.Debug("焦点");
            uint roomID = message.readUInt32();
            byte pos = message.readUInt8();
            bool isForce = message.readBool();
            string ip = message.readString();
            string mask = message.readString();
            if (roomID == GameData.m_TableInfo.id)
            {
                NiuNiuGame.Instance.onPlayerOnForce(pos, isForce);

            }

        }
    }


    /// <summary>
    /// 每次投票的结果返回
    /// </summary>
    /// <param name="message"></param>
    void PlayerAgreeDisposeResult(NetworkMessage message)
    {

        uint roomID = message.readUInt32();
        byte pos = message.readUInt8();
        bool isAgree = message.readBool();
        if (roomID == GameData.m_TableInfo.id)
        {
            if (isAgree)
            {
                GameData.m_TableInfo.operateLeaveRoomList.Add(pos);
                if (DzPanelDestoryRoom.Instance != null)
                    DzPanelDestoryRoom.Instance.Init();
            }
            else
            {
                UIManager.Instance.HideUiPanel(UIPaths.PanelDestoryRoom);
            }
        }

        #region
        //if (GameData.GlobleRoomType == RoomType.XYGQP)
        //{
        //    uint roomID = message.readUInt32();
        //    byte pos = message.readUInt8();
        //    bool isAgree = message.readBool();
        //    if (roomID == GameData.m_TableInfo.id)
        //    {
        //        if (isAgree)
        //        {
        //            GameData.m_TableInfo.operateLeaveRoomList.Add(pos);
        //            if (DzPanelDestoryRoom.Instance != null)
        //                DzPanelDestoryRoom.Instance.Init();
        //        }
        //        else
        //        {
        //            UIManager.Instance.HideUiPanel(UIPaths.PanelDestoryRoom);
        //        }
        //    }
        //}

        //else if(GameData.GlobleRoomType == RoomType.WDH)
        //{
        //    uint roomID = message.readUInt32();
        //    byte pos = message.readUInt8();
        //    bool isAgree = message.readBool();
        //    if (roomID == GameData.m_TableInfo.id)
        //    {
        //        if (isAgree)
        //        {
        //            GameData.m_TableInfo.operateLeaveRoomList.Add(pos);
        //            if (DzPanelDestoryRoom.Instance != null)
        //                DzPanelDestoryRoom.Instance.Init();
        //        }
        //        else
        //        {
        //            UIManager.Instance.HideUiPanel(UIPaths.PanelDestoryRoom);
        //        }
        //    }
        //}

        #endregion

    }

    /// <summary>
    /// 解散房间返回
    /// </summary>
    /// <param name="message"></param>
    void PlayerQueryDisposeRoom(NetworkMessage message)//
    {

        uint roomID = message.readUInt32();
        byte pos = message.readUInt8();
        if (roomID == GameData.m_TableInfo.id)
        {
            GameData.m_TableInfo.queryLeaveRoomWaitTime =180;
            GameData.m_TableInfo.operateLeaveRoomList.Clear();
            GameData.m_TableInfo.operateLeaveRoomList.Add(pos);
            UIManager.Instance.HideUiPanel(UIPaths.UIPanel_Setting);
            UIManager.Instance.ShowUiPanel(UIPaths.PanelDestoryRoom, OpenPanelType.MinToMax);
        }
      

           
    }

    /// <summary>
    /// 玩家操作
    /// </summary>
    /// <param name="message"></param>
    private void onPlayerOperateSuccess(NetworkMessage message)
    {
        if (GameData.GlobleRoomType == RoomType.PK )
        {

        }
        else if (GameData.GlobleRoomType == RoomType.WDH)
        {
            Log.Debug("玩家操作成功");
            uint roomID = message.readUInt32();
            byte pos = message.readUInt8();
            CardOperateType type = (CardOperateType)message.readUInt8();
            if (!IsPiPei)
            {
                if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.onPlayerOperateSuccess(pos, type);
            }
            else
            {
                if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.onPlayerOperateSuccess(pos, type);
            }
          
        }
        else if (GameData.GlobleRoomType == RoomType.ZB)
        {
            Log.Debug("玩家操作成功");
            uint roomID = message.readUInt32();
            byte pos = message.readUInt8();
            CardOperateType type = (CardOperateType)message.readUInt8();
            if (!IsPiPei)
            {
                if (ZaiBaiGame.Instance != null) ZaiBaiGame.Instance.onPlayerOperateSuccess(pos, type);
            }
            else
            {
                if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.onPlayerOperateSuccess(pos, type);
            }
          
        }

    }

    /// <summary>
    /// 玩家摸牌
    /// </summary>
    /// <param name="message"></param>
    private void onPlayerInCard(NetworkMessage message)
    {
        if (GameData.GlobleRoomType == RoomType.WDH)
        {
            Log.Debug("玩家摸牌");
            uint roomID = message.readUInt32();
            byte pos = message.readUInt8();
            uint card = message.readUInt32();
            uint resCards = message.readUInt32();
            GameData.m_TableInfo.resCardCount = resCards;
            if (!IsPiPei)
            {
                if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.onPlayerInCard(pos, card, resCards);
            }
            else
            {
                if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.onPlayerInCard(pos, card, resCards);
            }
           
        }

        if (GameData.GlobleRoomType == RoomType.ZB)
        {
            Log.Debug("玩家摸牌");
            uint roomID = message.readUInt32();
            byte pos = message.readUInt8();
            uint card = message.readUInt32();
            uint resCards = message.readUInt32();
            GameData.m_TableInfo.resCardCount = resCards;
            if (!IsPiPei)
            {
                ZaiBaiGame.Instance.onPlayerInCard(pos, card, resCards);
            }
            else
            {
                ZaiBaiGameJinBi.Instance.onPlayerInCard(pos, card, resCards);
            }
          
        }

    }
    /// <summary>
    /// 房间解散
    /// </summary>
    /// <param name="message"></param>
    private void onGameDispose(NetworkMessage message)
    {
        Log.Debug("房间销毁");
        uint roomID = message.readUInt32();
        uint endTime = message.readUInt32();
        RoomDisposeType type = (RoomDisposeType)message.readUInt8();
        byte count = message.readUInt8();

        PartGameOverControl.instance.TotalGameOverInfoList = new List<PlayerInfo>();
        for (int i = 0; i < count; i++)
        {

            byte pos = message.readUInt8();
            PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);
            info.guid = message.readUInt64();
            info.name = message.readString();
            info.headID = message.readString();
            info.score = message.readInt32();
            info.totalHuCount = message.readUInt32();
            info.totalMakerCount = message.readUInt32();
            long addGold = message.readInt64();//赠送的金币
            if (info.guid == Player.Instance.guid)
            {
                if (!IsPiPei)
                {
                    if (addGold != 0)
                    {
                        GameData.GlobleTipString = "赠送金币" + addGold;
                        UIManager.Instance.ShowUiPanel(UIPaths.GlobleTipPanel);
                    }
                   
                }
            }
            PartGameOverControl.instance.TotalGameOverInfoList.Add(info);
        }
        GameData.m_IsNormalOver = false;
        switch (type)
        {
            case RoomDisposeType.CreatorDispose:
                ManagerScene.Instance.LoadScene(SceneType.Main);
                break;
            case RoomDisposeType.RoundOver:
                GameData.m_IsNormalOver = true;
                if (GameData.GlobleRoomType == RoomType.PK)
                {
                    UIManager.Instance.ShowUiPanel(UIPaths.PanelGameOverBig, OpenPanelType.MinToMax);
                }
                break;
            case RoomDisposeType.PlayerQueryDispose:
               
                UIManager.Instance.HideUiPanel(UIPaths.PanelDestoryRoom);
                if (GameData.GlobleRoomType != RoomType.PK)
                {
                    UIManager.Instance.ShowUiPanel(UIPaths.UIPanel_TotalScore, OpenPanelType.MinToMax);
                }
                else
                {
                   UIManager.Instance.ShowUiPanel(UIPaths.PanelGameOverBig, OpenPanelType.MinToMax);
                }
              

                if (!IsPiPei)
                {
                    if (DzViewGame.Instance != null) DzViewGame.Instance.GameReset();
                }
                else
                {
                    if (DDZJinBi.Instance != null) DDZJinBi.Instance.GameReset();
                  
                }
               
                break;
            case RoomDisposeType.TimeOut:
                ManagerScene.Instance.LoadScene(SceneType.Main);
                break;
            case RoomDisposeType.ServerShutDown:
                ManagerScene.Instance.LoadScene(SceneType.Main);
                break;
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <param name="message"></param>
    private void onGameOver(NetworkMessage message)
    {
        Log.Debug("单局结算");
        RoomType overType = (RoomType) message.readUInt8();
        long length = message.readInt64(); //数据长度
        byte[] newbyte = new byte[length];
        message.readBytes(newbyte);

        GameOver overinfo = GameOver.Parser.ParseFrom(newbyte);

        PartGameOverControl.instance.ZhuangPos = (int) overinfo.ZhuangPosition;
        PartGameOverControl.instance.HelperPos = (int) overinfo.FriendPosition;

        int count = overinfo.GameOverInfo.Count;
        PartGameOverControl.instance.SettleInfoList = new List<SettleDownInfo>();
        for (int i = 0; i < count; i++)
        {
            bool isWin = overinfo.GameOverInfo[i].IsWinner;
            byte pos = (byte) overinfo.GameOverInfo[i].Position;
            int score = overinfo.GameOverInfo[i].Score;
            int changeScore = overinfo.GameOverInfo[i].ChangeScore;
            int baseScore = overinfo.GameOverInfo[i].BaseScore;
            int zadanScore = (int) overinfo.GameOverInfo[i].ZhanDanScore;
            int faWangScore = (int) overinfo.GameOverInfo[i].FaWangScore;

            //保存数据
            SettleDownInfo info = new SettleDownInfo();
            info.IsWin = isWin;
            info.Pos = pos;
            info.Score = score; //人物总积分
            info.BaseScore = baseScore;
            info.ChangeScore = changeScore; //总改变分数
            info.ZhaDanScore = zadanScore;
            info.FaWangScore = faWangScore;

            int leftCardCount = overinfo.GameOverInfo[i].Cards.Count; //剩余牌数
            List<uint> cardslist = new List<uint>();
            for (int j = 0; j < leftCardCount; j++)
            {
                cardslist.Add(overinfo.GameOverInfo[i].Cards[j]);
            }
            info.LeftCardList = cardslist; //剩余排数
            info.Index = 4;

            int outCardCount = overinfo.GameOverInfo[i].CatchedCard.Count; //总得出牌数
            info.TaoShangCardList = new List<List<uint>>();

            for (int j = 0; j < outCardCount; j++)
            {
                List<uint> taoShangCardList = new List<uint>();
                for (int k = 0; k < overinfo.GameOverInfo[i].CatchedCard[j].Card.Count; k++)
                {
                    taoShangCardList.Add(overinfo.GameOverInfo[i].CatchedCard[j].Card[k]);
                }

                info.TaoShangCardList.Add(taoShangCardList);
            }
            PartGameOverControl.instance.SettleInfoList.Add(info);
        }

        int playerCount = overinfo.FinishInfo.Count; //完成玩家人数
        for (int i = 0; i < playerCount; i++)
        {
            int pos1 = (int) overinfo.FinishInfo[i].FinishPosition; //位置
            int index = (int) overinfo.FinishInfo[i].FinishOrder; //第几个完成
            for (int j = 0; j < PartGameOverControl.instance.SettleInfoList.Count; j++)
            {
                if (pos1 == PartGameOverControl.instance.SettleInfoList[j].Pos)
                {
                    PartGameOverControl.instance.SettleInfoList[j].Index = index;
                }
            }
        }

        if (DzViewGame.Instance != null) DzViewGame.Instance.onPartGameOver();

        //当前局以及之前所有的记录
        List<List<SettleDownInfo>> list = new List<List<SettleDownInfo>>();
        for (var i = 0; i < overinfo.GameRecord.Count; i++)
        {
            List<SettleDownInfo> list1 = new List<SettleDownInfo>();
            for (var j = 0; j < overinfo.GameRecord[i].PlayerSocreInfo.Count; j++)
            {
                bool isWin = overinfo.GameOverInfo[j].IsWinner;
                byte pos = (byte) overinfo.GameOverInfo[j].Position;

                SettleDownInfo info = new SettleDownInfo();
                info.IsWin = isWin;
                info.Pos = pos;
                info.BaseScore = overinfo.GameRecord[i].PlayerSocreInfo[j].BaseScore;
                info.ZhaDanScore = overinfo.GameRecord[i].PlayerSocreInfo[j].BombScore;
                info.FaWangScore = overinfo.GameRecord[i].PlayerSocreInfo[j].FaWangScore;
                info.ChangeScore = overinfo.GameRecord[i].PlayerSocreInfo[j].ChangeScore;//改变分数
                info.Score = overinfo.GameRecord[i].PlayerSocreInfo[j].Score;//人物总积分
                list1.Add(info);
            }
            list.Add(list1);
        }
        PartGameOverControl.instance.ListGameOverSmall = list;
    }

    /// <summary>
    /// 玩家操作
    /// </summary>
    /// <param name="message"></param>
 
    private void onPlayerOperate(NetworkMessage message)
    {
        Log.Debug("玩家操作");
        uint roomID = message.readUInt32();
        byte pos = message.readUInt8();
        CardOperateType type = (CardOperateType)message.readUInt8();
        int count = message.readInt32();
        List<uint> OutCardList = new List<uint>();
        for (int i = 0; i < count; i++)
        {
            uint card = message.readUInt32();
            OutCardList.Add(card);
        }
        int PlayerLeftCard = message.readInt32();


        bool HaveScoreChanage = message.readBool();//是否有分数变化
        if (HaveScoreChanage)
        {
            int ChangeCount = message.readInt32();//改变的玩家个数
            for (int i = 0; i < ChangeCount; i++)
            {
              byte Changepos=  message.readUInt8();//位置
             // int  ChangeScore=  message.readInt32();//改变的分数
              int TotalScore= message.readInt32();//当前分(头像下面显示的)

                if (DzViewGame.Instance != null)
                {
                   // DzViewGame.Instance.SetTotalJifen(Changepos, TotalScore);//设置积分
                    DzViewGame.Instance.SetTaoShangFen(Changepos, TotalScore);

                    Debug.Log("----------TotalScore");
                }
            }
        }
       


        if (GameData.GlobleRoomType == RoomType.PK)
        {
            #region

           // int Tatalscore = message.readInt32();//讨赏分
            if (PlayerLeftCard < 6)
            {

                if (!IsPiPei)
                {
                    if (DzViewGame.Instance != null) DzViewGame.Instance.ShowLeftCardNum(pos, PlayerLeftCard);
                }
                else
                {
                    if (DDZJinBi.Instance != null) DDZJinBi.Instance.ShowLeftCardNum(pos, PlayerLeftCard);
                    

                }
            

            }

            for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
            {
                if (GameData.m_PlayerInfoList[i].pos == pos)
                {
                  //  if (DzViewGame.Instance != null) DzViewGame.Instance.onPlayerOperate(pos, type, OutCardList, PlayerLeftCard, GameData.m_PlayerInfoList[i].score, (int)GameData.m_PlayerInfoList[i].sex);
                    if (!IsPiPei)
                    {
                        if (DzViewGame.Instance != null) DzViewGame.Instance.onPlayerOperate(pos, type, OutCardList, PlayerLeftCard, GameData.m_PlayerInfoList[i].score, (int)GameData.m_PlayerInfoList[i].sex);
                    }
                    else
                    {
                        if (DDZJinBi.Instance != null) DDZJinBi.Instance.onPlayerOperate(pos, type, OutCardList, PlayerLeftCard, GameData.m_PlayerInfoList[i].score, (int)GameData.m_PlayerInfoList[i].sex);
                      


                    }
                  
                }
            }

            #endregion
        }
        else if (GameData.GlobleRoomType == RoomType.WDH)
        {
           
            byte outPos = message.readUInt8();
            bool isAnGang = message.readBool();
            if (!IsPiPei)
            {
                if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.onPlayerOperate(pos, type, OutCardList[0], outPos, isAnGang);
            }
            else
            {
                if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.onPlayerOperate(pos, type, OutCardList[0], outPos, isAnGang);

            }
         
        }
        else if (GameData.GlobleRoomType == RoomType.ZB)
        {
         
            byte outPos = message.readUInt8();
            bool isAnGang = message.readBool();
            if (!IsPiPei)
            {
                if (ZaiBaiGame.Instance != null) ZaiBaiGame.Instance.onPlayerOperate(pos, type, OutCardList[0], outPos, isAnGang);
            }
            else
            {
                if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.onPlayerOperate(pos, type, OutCardList[0], outPos, isAnGang);
            }
          
        }


    }

    /// <summary>
    /// 开始发牌
    /// </summary>
    /// <param name="message"></param>
    private void onPlayerHoldCards(NetworkMessage message)
    {
        Log.Debug("开始发牌");

        uint roomID = message.readUInt32();
        PlayerInfo info = GameDataFunc.GetPlayerInfo(Player.Instance.guid);
        info.localCardList.Clear();
        byte count = message.readUInt8();
        for (int i = 0; i < count; i++)
        {
            info.localCardList.Add(message.readUInt32());//101  
        }

        #region  add
        //add 
       bool haveDouble= message.readBool();//庄是不是有两张黑桃3 
       byte ZhuangPos=  message.readUInt8();//庄的位置(即之后第一个出牌人的位置)
       byte FriendPos= message.readUInt8();//朋友的位置（另一个黑桃3的位置，或者直接是


        GameData.m_TableInfo.makerPos = ZhuangPos;
        PartGameOverControl.instance.ZhuangPos = (int)ZhuangPos;
        GameData.m_TableInfo.ZhuangPos = ZhuangPos;
        #endregion
        if (GameData.GlobleRoomType == RoomType.PK)
        {
           
            // if (Game.Instance != null) Game.Instance.onPlayerHoldCards();
           

            if (!IsPiPei)
            {
                if (DzViewGame.Instance != null) DzViewGame.Instance.onPlayerHoldCards();
                if (DzViewGame.Instance != null) DzViewGame.Instance.onZhuangPosition();
                if (DzViewGame.Instance != null) DzViewGame.Instance.OnShowFriendCard(FriendPos, 0);
                if (DzViewGame.Instance != null) DzViewGame.Instance.onRoomActive();
                UIManager.Instance.HideUiPanel(UIPaths.PanelGameOverSmall);
            }
            else
            {
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.onPlayerHoldCards();
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.onRoomActive();
                UIManager.Instance.HideUiPanel(UIPaths.PanelGameOverSmall);


            }
        }
        else if (GameData.GlobleRoomType == RoomType.WDH)
        {
            GameData.Dice1 = GameData.GenerateDice(1);
            GameData.Dice2 = GameData.GenerateDice(2);
            if (!IsPiPei)
            {
                if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.onPlayerHoldCards();
            }
            else
            {
                if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.onPlayerHoldCards();
            }
          
        }
        else if (GameData.GlobleRoomType == RoomType.ZB)
        {
            if (!IsPiPei)
            {
                if (ZaiBaiGame.Instance != null) ZaiBaiGame.Instance.onPlayerHoldCards();
            }
            else
            {
                if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.onPlayerHoldCards();
            }
           
        }
        else if (GameData.GlobleRoomType == RoomType.NN)
        {
           
                if (NiuNiuGame.Instance != null) NiuNiuGame.Instance.onPlayerHoldCards();
           
        }
    }

    /// <summary>
    /// 玩家选分
    /// </summary>
    /// <param name="message"></param>
    private void onPlayerChooseScore(NetworkMessage message)
    {
        Log.Debug("玩家选择了分数");
        uint roomID = message.readUInt32();
        byte pos = message.readUInt8();
        uint chooseScore = message.readUInt32();
      //  if (Game.Instance != null) Game.Instance.onPlayerChooseScore(pos, chooseScore);
    }

    /// <summary>
    /// 等待玩家选分
    /// </summary>
    /// <param name="message"></param>
    private void onWaitPlayerChooseScore(NetworkMessage message)
    {
        Log.Debug("等待玩家选择积分");
        uint roomID = message.readUInt32();
      //  if (Game.Instance != null) Game.Instance.onWaitPlayerChooseScore();
    }

    /// <summary>
    /// 庄家pos  再发一个装的牌（确定帮手用的）
    /// </summary>
    /// <param name="message"></param>
    private void onZhuangPosition(NetworkMessage message)
    {
        Log.Debug("房间激活庄家位置");
        uint roomID = message.readUInt32();
        byte pos = message.readUInt8();
        uint lianZhuangCount = message.readUInt32();
        uint curGameCount = message.readUInt32();

        PartGameOverControl.instance.ZhuangPos = (int)pos;

        GameData.m_TableInfo.curGameCount = curGameCount;
        GameData.m_TableInfo.makerPos = pos;
        PartGameOverControl.instance.ZhuangPos = (int)pos;
        GameData.m_TableInfo.ZhuangPos = pos;
        GameData.m_TableInfo.lianZhuangCount = lianZhuangCount;
        if (GameData.GlobleRoomType == RoomType.PK)
        {

            if (!IsPiPei)
            {
                if (DzViewGame.Instance != null) DzViewGame.Instance.onZhuangPosition();
            }
            else
            {
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.onZhuangPosition();
                

            }
            // if (Game.Instance != null) Game.Instance.onZhuangPosition();//DzViewGame

         
        }
        else if (GameData.GlobleRoomType == RoomType.WDH)
        {
            if (!IsPiPei)
            {
                if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.onZhuangPosition();
            }
            else
            {
                if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.onZhuangPosition();
            }
           
        }

        else if (GameData.GlobleRoomType == RoomType.ZB)
        {
            if (!IsPiPei)
            {
                if (ZaiBaiGame.Instance != null) ZaiBaiGame.Instance.onZhuangPosition();
            }
            else
            {
                if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.onZhuangPosition();

            }
           
        }
        else if (GameData.GlobleRoomType == RoomType.NN)
        {
           
                if (NiuNiuGame.Instance != null) NiuNiuGame.Instance.onZhuangPosition();

           

        }

    }

    /// <summary>
    /// 房间激活
    /// </summary>
    /// <param name="message"></param>
    private void onRoomActive(NetworkMessage message)
    {
        Log.Debug("房间激活");
        uint roomID = message.readUInt32();
        if (GameData.GlobleRoomType == RoomType.PK)
        {

            // if (Game.Instance != null) Game.Instance.onRoomActive();

            if (!IsPiPei)
            {
                if (DzViewGame.Instance != null) DzViewGame.Instance.onRoomActive();
            }
            else
            {
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.onRoomActive();
               
            }
           
        }
        else if (GameData.GlobleRoomType == RoomType.WDH)
        {
            if (!IsPiPei)
            {
                if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.onRoomActive();
            }
            else
            {
                if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.onRoomActive();
            }
           
        }
        else if (GameData.GlobleRoomType == RoomType.ZB)
        {
            if (!IsPiPei)
            {
                if (ZaiBaiGame.Instance != null) ZaiBaiGame.Instance.onRoomActive();
            }
            else
            {
                if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.onRoomActive();
            }
           
        }

        else if (GameData.GlobleRoomType == RoomType.NN)
        {
            if (NiuNiuGame.Instance != null) NiuNiuGame.Instance.onRoomActive();
            //if (!IsPiPei)
            //{
            //    if (ZaiBaiGame.Instance != null) ZaiBaiGame.Instance.onRoomActive();
            //}
            //else
            //{
            //    if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.onRoomActive();
            //}

        }


    }

   /// <summary>
   /// 准备下一局
   /// </summary>
   /// <param name="message"></param>
    private void onPlayerReadyForNext(NetworkMessage message)
    {

        Log.Debug("玩家准备下一局");
        uint roomID = message.readUInt32();
        byte pos = message.readUInt8();
        if (GameData.GlobleRoomType == RoomType.PK)
        {
            if (!IsPiPei)
            {
                if (DzViewGame.Instance != null) DzViewGame.Instance.onPlayerReadyForRoom(pos);
            }
            else
            {
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.onPlayerReadyForRoom(pos);
               // if (DDZJinBi.Instance != null) DDZJinBi.Instance.onPlayerReadyForRoom(pos);
                // if (DDZJinBi.Instance != null) DDZJinBi.Instance.onPlayerEnter(info);
            }
            //  if (Game.Instance != null) Game.Instance.onPlayerReadyForNext(pos);
           
        }

        else if (GameData.GlobleRoomType == RoomType.WDH)
        {
            if (!IsPiPei)
            {
                if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.onPlayerReadyForNext(pos);
            }
            else
            {
                if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.onPlayerReadyForNext(pos);
            }
          
        }
        else if (GameData.GlobleRoomType == RoomType.ZB)
        {
            if (!IsPiPei)
            {
                if (ZaiBaiGame.Instance != null) ZaiBaiGame.Instance.onPlayerReadyForNext(pos);
            }
            else
            {
                if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.onPlayerReadyForNext(pos);
            }
          
        }
        else if (GameData.GlobleRoomType == RoomType.NN)
        {
           
                if (NiuNiuGame.Instance != null) NiuNiuGame.Instance.onPlayerReadyForRoom(pos);
           

        }

    }

    /// <summary>
    /// 准备开始游戏
    /// </summary>
    /// <param name="message"></param>
    private void onPlayerReadyForRoom(NetworkMessage message)
    {
        Log.Debug("玩家准备开始游戏");
        uint roomID = message.readUInt32();
        byte pos = message.readUInt8();
        if (GameData.GlobleRoomType == RoomType.PK)
        {
          
            // if (Game.Instance != null) Game.Instance.onPlayerReadyForRoom(pos);
          

            if (!IsPiPei)
            {
                if (DzViewGame.Instance != null) DzViewGame.Instance.onPlayerReadyForRoom(pos);
            }
            else
            {
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.onPlayerReadyForRoom(pos);
               // if (DDZJinBi.Instance != null) DDZJinBi.Instance.onPlayerEnter(info);
            }
        }
        else if (GameData.GlobleRoomType == RoomType.WDH)
        {
            if (!IsPiPei)
            {
                if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.onPlayerReadyForRoom(pos);
            }
            else
            {
                if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.onPlayerReadyForRoom(pos);
            }
          
        }
        else if (GameData.GlobleRoomType == RoomType.ZB)
        {
            if (!IsPiPei)
            {
                if (ZaiBaiGame.Instance != null) ZaiBaiGame.Instance.onPlayerReadyForRoom(pos);
            }
            else
            {
                if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.onPlayerReadyForRoom(pos);
            }
            
        }
        else if (GameData.GlobleRoomType == RoomType.NN)
        {
            
                if (NiuNiuGame.Instance != null) NiuNiuGame.Instance.onPlayerReadyForRoom(pos);
          
        }


    }

    /// <summary>
    /// 玩家进入
    /// </summary>
    /// <param name="message"></param>
    private void onPlayerEnter(NetworkMessage message)
    {

        Log.Debug("玩家进入房间");
        uint roomID = message.readUInt32();
        PlayerInfo info = new PlayerInfo();
        info.guid = message.readUInt64();
        info.pos = message.readUInt8();
        info.N = message.readFloat();//posx
        info.E = message.readFloat();//posy
        info.sex = message.readUInt8();
        info.name = message.readString();
        info.headID = message.readString();
        info.ip = message.readString();
        info.mask = message.readString();
        bool isPiPei = message.readBool();//是否为金币场
        if (isPiPei)
        {
            info.Gold = message.readInt64();
        }
        GameData.m_PlayerInfoList.Add(info);
        if (GameData.GlobleRoomType == RoomType.PK)
        {

            //  if (Game.Instance != null) Game.Instance.onPlayerEnter(info);
            if (!IsPiPei)
            {
                if (DzViewGame.Instance != null) DzViewGame.Instance.onPlayerEnter(info);
            }
            else
            {
                if (DDZJinBi.Instance != null) DDZJinBi.Instance.onPlayerEnter(info);
            }
          
        }
        else if (GameData.GlobleRoomType == RoomType.WDH)
        {
            if (!IsPiPei)
            {
                if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.onPlayerEnter(info);
            }
            else
            {
                if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.onPlayerEnter(info);
            }
          
        }
        else if (GameData.GlobleRoomType == RoomType.ZB)
        {
            if (!IsPiPei)
            {
                if (ZaiBaiGame.Instance != null) ZaiBaiGame.Instance.onPlayerEnter(info);
            }
            else
            {
                if (ZaiBaiGameJinBi.Instance != null) ZaiBaiGameJinBi.Instance.onPlayerEnter(info);
            }
           
        }
        else if (GameData.GlobleRoomType == RoomType.NN)
        {
            //if (!IsPiPei)
            //{
            //    if (ZaiBaiGame.Instance != null) ZaiBaiGame.Instance.onPlayerEnter(info);
            //}
            //else
            //{
                if (NiuNiuGame.Instance != null) NiuNiuGame.Instance.onPlayerEnter(info);
           // }

        }

    }

    RoomType roomType;//房间类型
   public  bool IsPiPei = false;//是否为金币场
    public bool IsDaiLiCreat = false;
   public   SendRoomInfo sendroominfo = null;//房间信息
    private void onRoomInfo(NetworkMessage message)
    {
        Log.Debug("房间基本信息");
        roomType = (RoomType)message.readUInt8();//房间类型
        long length = message.readInt64();//数据长度
        byte[] newbyte = new byte[length];
        message.readBytes(newbyte);

         sendroominfo = SendRoomInfo.Parser.ParseFrom(newbyte);

        GameData.sendroominfo = sendroominfo;

        GameData.m_PlayerInfoList.Clear();
        GameData.m_HoldCardsList.Clear();
        GameDataFunc.ClearData();
        GameDataFunc.ClearDataObj();

        #region  old  to new
      
        TableInfo info = new TableInfo();

        // roomType =(RoomType)message.readUInt8();//房间类型
        // IsDaiLiCreat = message.readBool();//是否为代理开房
        // IsPiPei = message.readBool();//是否为匹配

        //message.readUInt8();//开房类型creatRoomType
        GameData.GlobleRoomType = roomType;//全局房间类型

       
        switch (roomType)
        {
            case RoomType.PK:
                //OnTaoShnagRoomInfo(message);
                OnPKRoomInfo();
                break;
            case RoomType.WDH:
                OnWuTangHuRoomInfo(message);
                break;
            case RoomType.ZB:
                OnZaiBaoRoomInfo(message);
                break;
            case RoomType.NN:
                OnNiuNiuRoomInfo(message);
                break;
        }

      
        //if (info.roomState == RoomStatusType.Play)
        //{
        //    if (DzViewGame.Instance != null) DzViewGame.Instance.ReconnectServer();//重连
        //}

        #endregion
        // SendRoomInfo sendroominfo = new SendRoomInfo();//房间信息协议


        #region  old
        /*
        TableInfo info = new TableInfo();

         roomType =(RoomType)message.readUInt8();//房间类型
         IsDaiLiCreat = message.readBool();//是否为代理开房
         IsPiPei = message.readBool();//是否为匹配

        message.readUInt8();//开房类型creatRoomType
        GameData.GlobleRoomType = roomType;//全局房间类型

        switch (roomType)
        {
            case RoomType.PK:
                OnTaoShnagRoomInfo(message);
                break;
            case RoomType.WDH:
                OnWuTangHuRoomInfo(message);
                break;
            case RoomType.ZB:
                OnZaiBaoRoomInfo(message);
                break;
            case RoomType.NN:
                OnNiuNiuRoomInfo(message);
                break;
        }

        */
        //if (info.roomState == RoomStatusType.Play)
        //{
        //    if (DzViewGame.Instance != null) DzViewGame.Instance.ReconnectServer();//重连
        //}

        #endregion
    }
    /// <summary>
    ///上饶
    /// </summary>
    /// <param name="message"></param>
    private void OnPKRoomInfo()
    {
        GameData.m_PlayerInfoList.Clear();
        GameData.m_HoldCardsList.Clear();
        GameDataFunc.ClearData();
        GameDataFunc.ClearDataObj();
        TableInfo info = new TableInfo();

        info.isBawang = sendroominfo.IsBaWang;
        info.IsPiPei = IsPiPei;
        info.IsDaiLiCreat = IsDaiLiCreat;
        // RoomType roomType = (RoomType)message.readUInt8();//房间类型 这里不需要

        info.id = sendroominfo.RoomCodeId;//房间号
        info.fangZhuGuid = sendroominfo.CreatorGuid;//房主ID
        info.configRoundIndex = (byte)sendroominfo.GameCount;//几局

        info.configPlayerIndex =(byte) sendroominfo.PlayerCount;//人数config
       // info.configPayIndex = message.readUInt8();

        info.ZhuangPos = (byte)sendroominfo.ZhuangPosition;//庄家位置

        PartGameOverControl.instance.ZhuangPos = (int)info.ZhuangPos;
        info.lianZhuangCount = (uint)sendroominfo.ZhuangCount;//连庄次数

        info.roomState = (RoomStatusType)sendroominfo.RoomStatus;

        info.curGameCount = (uint)sendroominfo.CurGameCount;//当前局数
                                                          //info.configPlayerIndex = message.readUInt8();

        // info.makerPos = message.readUInt8();
        //bool HaveColdTime = message.readBool();//是否有房间冷却
        //if (HaveColdTime)
        //{
        //    uint time = message.readUInt32();
        //}


        info.isQueryLeaveRoom = sendroominfo.QueryDisposeRoom;//是否有请求离开的请求
        info.queryLeaveRoomWaitTime = sendroominfo.TimerRest;//倒计时
        if (info.isQueryLeaveRoom)
        {
            //info.queryLeaveRoomWaitTime = message.readUInt32();
            //byte count = (byte)message.readUInt8();

            for (int i = 0; i < sendroominfo.AgressPosition.Count; i++)
                info.operateLeaveRoomList.Add((byte)sendroominfo.AgressPosition[i]);



        }

       // byte pCount = (byte)message.readUInt8();//玩家个数
        for (int i = 0; i < sendroominfo.SitPlayerInfo.Count; i++)
        {
            PlayerInfo pInfo = new PlayerInfo();
            pInfo.pos =(byte) sendroominfo.SitPlayerInfo[i].Position;
            pInfo.N = sendroominfo.SitPlayerInfo[i].Positionx;
            pInfo.E = sendroominfo.SitPlayerInfo[i].Positiony;
            pInfo.guid = sendroominfo.SitPlayerInfo[i].Guid;

            pInfo.sex = (byte)sendroominfo.SitPlayerInfo[i].Sex;
            pInfo.isStartReady = sendroominfo.SitPlayerInfo[i].IsReadyForRoom;
            pInfo.isNextReady = sendroominfo.SitPlayerInfo[i].IsReadyForNextGame;

           // pInfo.IsAi = sendroominfo.SitPlayerInfo[i].IsReadyForNextGame;//是否托管
            pInfo.isForce = sendroominfo.SitPlayerInfo[i].OnForce;

            pInfo.ip = sendroominfo.SitPlayerInfo[i].ClientIp;
            pInfo.mask = sendroominfo.SitPlayerInfo[i].ClientMask;
            pInfo.name = sendroominfo.SitPlayerInfo[i].OtherName;
            pInfo.headID = sendroominfo.SitPlayerInfo[i].HeadIp;

            pInfo.changeScore = (int)sendroominfo.SitPlayerInfo[i].ChangeScore;

            pInfo.TSTaoShangScore = (uint)sendroominfo.SitPlayerInfo[i].TaoShangScore;
            pInfo.score = sendroominfo.SitPlayerInfo[i].Score;

            if (IsPiPei)//匹配的金币场
            {
              //  pInfo.Gold = message.readInt64();
            }

            pInfo.totalHuCount = sendroominfo.SitPlayerInfo[i].TotalWinCount;
            pInfo.totalMakerCount = sendroominfo.SitPlayerInfo[i].TotalZhuangCount;//当庄次数
            pInfo.OperateType = (CardOperateType)sendroominfo.SitPlayerInfo[i].LastOperateType;//操作类型
            #region

            #endregion
            // byte oCount = message.readInt32();//玩家手牌

            //int count = message.readInt32();
            pInfo.LeftCardNum = sendroominfo.SitPlayerInfo[i].HoldCards.Count;
            if (Player.Instance.guid == pInfo.guid)
            {
                for (int k = 0; k < sendroominfo.SitPlayerInfo[i].HoldCards.Count; k++)
                {
                    pInfo.localCardList.Add(sendroominfo.SitPlayerInfo[i].HoldCards[k]);
                }
            }
            else
            {
                //for (int k = 0; k < count; k++)
                //{
                //    pInfo.localCardList.Add(0);
                //}
            }


           // int oCount = message.readInt32();//丢去的牌  出的牌

            for (int k = 0; k < sendroominfo.SitPlayerInfo[i].DropCards.Count; k++)
            {
                pInfo.outCardList.Add(sendroominfo.SitPlayerInfo[i].DropCards[k]);
            }

            GameData.m_PlayerInfoList.Add(pInfo);
        }


        info.FriendPos = 0;//防止断线重连后位置有无
        if (sendroominfo.PlayStatusInfo != null) info.roomState = RoomStatusType.Play;
        if(sendroominfo.GameOverInfo != null) info.roomState = RoomStatusType.Over;
        if (info.roomState == RoomStatusType.Play)//断线回来有用
        {

            //uint FriendCard = sendroominfo.PlayStatusInfo;//朋友卡
            //uint friendpos = (uint)message.readUInt8();//朋友的位置
            info.FriendCard = FriendCard;
            info.FriendPos = (int)sendroominfo.PlayStatusInfo.FriendPosition;
            PartGameOverControl.instance.HelperPos = info.FriendPos;
            // info.isWaitFangPao = !message.readBool();
            info.lastOutCardPos = (byte)sendroominfo.PlayStatusInfo.LastDropCardPosition;//最后出牌的位置
            info.waitOutCardPos = (byte)sendroominfo.PlayStatusInfo.WaitDropPosition;//该出牌的位置



            // info.resCardCount = message.readUInt32();
            info.isOutCardInfo = sendroominfo.PlayStatusInfo.HasDropCard;//是否有出的牌
            if (info.isOutCardInfo)
            {

                //int count = message.readInt32();
                //for (int i = 0; i < count; i++)
                //{
                //    uint Card = message.readUInt32();
                //}
            }


            GameData.FinishPlayerPos = new List<FinishPlayer>();
            int playerCount = sendroominfo.PlayStatusInfo.FinishCount;//完成玩家人数
            for (int i = 0; i < sendroominfo.PlayStatusInfo.FinishInfo.Count; i++)
            {
                int pos1 = (int)sendroominfo.PlayStatusInfo.FinishInfo[i].FinishPosition;//位置
                int index = (int)sendroominfo.PlayStatusInfo.FinishInfo[i].FinishOrder;//第几个完成

                FinishPlayer player = new FinishPlayer();
                player.pos = pos1;
                player.index = index;
                GameData.FinishPlayerPos.Add(player);
            }


        }
        else if (info.roomState == RoomStatusType.Over)
        {
            PartGameOverControl.instance.ZhuangPos = (int)sendroominfo.GameOverInfo.ZhuangPosition;
            PartGameOverControl.instance.HelperPos = (int)sendroominfo.GameOverInfo.FriendPosition;

            PartGameOverControl.instance.SettleInfoList = new List<SettleDownInfo>();
          //  int Count = message.readInt32();
            for (int i = 0; i < sendroominfo.GameOverInfo.GameOverPlayerInfo.Count; i++)
            {


                #region
                bool IsWin = sendroominfo.GameOverInfo.GameOverPlayerInfo[i].IsWinner;
                byte pos = (byte)sendroominfo.GameOverInfo.GameOverPlayerInfo[i].Position;

                int Score = sendroominfo.GameOverInfo.GameOverPlayerInfo[i].Score;
                int ChangeScore = sendroominfo.GameOverInfo.GameOverPlayerInfo[i].ChangeScore;
                int HuiHeFen = sendroominfo.GameOverInfo.GameOverPlayerInfo[i].BaseScore;
                //  uint TaoShangScore = (uint)sendroominfo.GameOverInfo.GameOverPlayerInfo[i].TaoShangScore;

                int ZaDanScore = sendroominfo.GameOverInfo.GameOverPlayerInfo[i].ZhanDanScore;
                int FaWangScore = sendroominfo.GameOverInfo.GameOverPlayerInfo[i].FaWangScore;

                //保存数据
                SettleDownInfo info1 = new SettleDownInfo();
                info1.IsWin = IsWin;
                info1.Pos = (int)pos;
                info1.Score = Score;//人物总积分
                info1.BaseScore = HuiHeFen;
               // info1.TaoShangFen = (int)TaoShangScore;//讨赏分
                info1.ChangeScore = ChangeScore;//总改变分数

                info1.ZhaDanScore = ZaDanScore;//
                info1.FaWangScore = FaWangScore;//





                int leftCardCount = sendroominfo.GameOverInfo.GameOverPlayerInfo[i].Cards.Count;
                List<uint> Cardslist = new List<uint>();
                for (int j = 0; j < leftCardCount; j++)
                {
                    Cardslist.Add(sendroominfo.GameOverInfo.GameOverPlayerInfo[i].Cards[j]);
                }
                info1.LeftCardList = Cardslist;//剩余排数
                info1.Index = 4;
                PartGameOverControl.instance.SettleInfoList.Add(info1);

                #endregion

            }

            //int playerCount = message.readInt32();//完成玩家人数
            //for (int i = 0; i < playerCount; i++)
            //{
            //    int pos1 = message.readUInt8();//位置
            //    int index = message.readUInt8();//第几个完成
            //    for (int j = 0; j < PartGameOverControl.instance.SettleInfoList.Count; j++)
            //    {
            //        if (pos1 == PartGameOverControl.instance.SettleInfoList[j].pos)
            //        {
            //            PartGameOverControl.instance.SettleInfoList[j].Index = index;
            //        }
            //    }
            //}


        }


        GameData.m_TableInfo = info;

        GameData.Dice1 = GameData.GenerateDice(1);
        GameData.Dice2 = GameData.GenerateDice(2);
        UIManager.Instance.HideUiPanel(UIPaths.PanelCreatRoom);

        if (!IsPiPei)
        {
            ManagerScene.Instance.LoadScene(SceneType.Game);
        }
        else//金币场
        {
            ManagerScene.Instance.LoadScene(SceneType.GameJinBi);
        }



    }

    /// <summary>
    /// 牛牛房间信息处理
    /// </summary>
    /// <param name="message"></param>
    private void OnNiuNiuRoomInfo(NetworkMessage message)
    {
        GameData.m_PlayerInfoList.Clear();
        GameData.m_HoldCardsList.Clear();
        GameDataFunc.ClearData();
        GameDataFunc.ClearDataObj();
        TableInfo info = new TableInfo();

        info.IsPiPei = IsPiPei;
        info.IsDaiLiCreat = IsDaiLiCreat;
        // RoomType roomType = (RoomType)message.readUInt8();//房间类型 这里不需要

        info.id = message.readUInt32();//房间号
        info.fangZhuGuid = message.readUInt64();//房主ID
        info.configRoundIndex = (byte)message.readUInt32();//几局

        info.configPayIndex = message.readUInt8();

        info.ZhuangPos = message.readUInt8();//庄家位置

        PartGameOverControl.instance.ZhuangPos = (int)info.ZhuangPos;
        info.lianZhuangCount = (uint)message.readUInt32();//连庄次数

        info.roomState = (RoomStatusType)message.readUInt8();

        info.curGameCount = (uint)message.readUInt32();//当前局数
                                                       //info.configPlayerIndex = message.readUInt8();

        // info.makerPos = message.readUInt8();
        if (roomType == RoomType.NN)
        {
            info.NNYongPaiType =(NNYongPai) message.readUInt8();//用牌


            info.EnShunZhiNiu = message.readBool();
            info.EnBoomNiu = message.readBool();
            info.EnWuXiaoNiu = message.readBool();
            info.EnWuHuaNiu = message.readBool();
            info.EnXianJiaMaiMA = message.readBool();

            int ChipCount = message.readInt32();//可下注的底分
            info.CanChipList = new List<uint>();
            for (int i = 0; i < ChipCount; i++)
            {
                info.CanChipList.Add(message.readUInt32());
            }
        }

        bool HaveColdTime = message.readBool();//是否有房间冷却
        if (HaveColdTime)
        {
            uint time = message.readUInt32();
        }
           


        info.isQueryLeaveRoom = message.readBool();//是否有请求离开的请求
        if (info.isQueryLeaveRoom)
        {
            info.queryLeaveRoomWaitTime = message.readUInt32();
            byte count = (byte)message.readUInt8();

            for (int i = 0; i < count; i++)
                info.operateLeaveRoomList.Add((byte)message.readUInt8());
        }


        byte pCount = (byte)message.readUInt8();//玩家个数
        for (int i = 0; i < pCount; i++)
        {

            #region
            PlayerInfo pInfo = new PlayerInfo();
            pInfo.pos = message.readUInt8();
            pInfo.N = message.readFloat();
            pInfo.E = message.readFloat();
            pInfo.guid = message.readUInt64();

            pInfo.sex = message.readUInt8();
            pInfo.isStartReady = message.readBool();
            pInfo.isNextReady = message.readBool();

            pInfo.IsAi = message.readBool();//是否托管
            pInfo.isForce = message.readBool();

            pInfo.ip = message.readString();
            pInfo.mask = message.readString();
            pInfo.name = message.readString();
            pInfo.headID = message.readString();

            pInfo.changeScore = (int)message.readInt32();

          //  pInfo.TSTaoShangScore = message.readUInt32();
            pInfo.score = message.readInt32();

            if (IsPiPei)//匹配的金币场
            {
                pInfo.Gold = message.readInt64();
            }

            pInfo.totalHuCount = message.readUInt32();
            pInfo.totalMakerCount = message.readUInt32();//当庄次数
            pInfo.OperateType = (CardOperateType)message.readUInt8();//操作类型
            #region

            #endregion
            // byte oCount = message.readInt32();//玩家手牌

            int count = message.readInt32();//手牌
            pInfo.LeftCardNum = count;
            if (Player.Instance.guid == pInfo.guid)
            {
                for (int k = 0; k < count; k++)
                {
                    pInfo.localCardList.Add(message.readUInt32());
                }
            }
            else
            {
                for (int k = 0; k < count; k++)
                {
                    pInfo.localCardList.Add(0);
                }
            }


            int oCount = message.readInt32();//丢去的牌  出的牌

            //for (int k = 0; k < oCount; k++)
            //{
            //    pInfo.outCardList.Add(message.readUInt32());
            //}


            //配的牌
            if (roomType == RoomType.NN)
            {
                pInfo.PosAndChipDic = new Dictionary<byte, uint>();
                //其他玩家在该玩家上下的注
                int OtherCount = message.readInt32();
                for (int j = 0; j < OtherCount; j++)
                {
                  byte pos=  message.readUInt8();//下注位置
                  uint chip=  message.readUInt32();//下注多少
                    pInfo.PosAndChipDic[pos] = chip;
                }


                pInfo.PeiPaiInfo = new List<List<uint>>();
                uint listcount = message.readUInt32();
                for (int l = 0; l < listcount; l++)
                {
                    List<uint> PeiPaiItem = new List<uint>();
                    uint listcount1 = message.readUInt32();
                    for (int n= 0; n < listcount1; n++)
                    {
                        PeiPaiItem.Add(message.readUInt32());
                    }
                    pInfo.PeiPaiInfo.Add(PeiPaiItem);
                }

                pInfo. PeiPaiType =(NNType) message.readUInt8();
               pInfo. FanBeiCount = message.readUInt32();//翻倍数
            }

            GameData.m_PlayerInfoList.Add(pInfo);

            #endregion
        }


        info.FriendPos = 0;//防止断线重连后位置有无
        if (info.roomState == RoomStatusType.Play)//断线回来有用
        {
            info.IsQiangZhuangState = message.readBool();
            info.QiangZhuangPosList = new List<int>();
            if (info.IsQiangZhuangState)
            {
                int XiaZhuCount = message.readInt32();//抢庄人数
                for (int i = 0; i < XiaZhuCount; i++)
                {
                    uint pos = message.readUInt8();
                    bool xiazhu = message.readBool();
                    info.QiangZhuangPosList.Add((int)pos);
                }
            }

           
                info.IsXiaZhuState = message.readBool();
           
                info.PosAndChipDic = new Dictionary<int, int>();
                int XiaZhuCount1 = message.readInt32();//下注人数
                for (int i = 0; i < XiaZhuCount1; i++)
                {
                    uint pos = message.readUInt8();
                    bool xiazhu = message.readBool();
                    int chip = message.readInt32();//下的多少注

                    info.PosAndChipDic[(int)pos] = chip;
                }
           
          

            bool HavePeiPaiTime = message.readBool();
            if (HavePeiPaiTime)
            {
                uint time = message.readUInt32();//配牌剩余时间
            }
        

        }
        else if (info.roomState == RoomStatusType.Over)
        {
            PartGameOverControl.instance.ZhuangPos = (int)message.readUInt8();
            int Count = message.readInt32();
            for (int i = 0; i < Count; i++)
            {
                byte pos = message.readUInt8();

                int Score = message.readInt32();
                int ChangeScore = message.readInt32();
                int HuiHeFen = message.readInt32();

                if (roomType == RoomType.NN)
                {
                    List<List<uint>> PeiPaiInfo = new List<List<uint>>();
                    uint listcount = message.readUInt32();
                    for (int l = 0; l < listcount; l++)
                    {
                        List<uint> PeiPaiItem = new List<uint>();
                        uint listcount1 = message.readUInt32();
                        for (int n = 0; n < listcount1; n++)
                        {
                            PeiPaiItem.Add(message.readUInt32());
                        }
                        PeiPaiInfo.Add(PeiPaiItem);
                    }

                    NNType PeiPaiType = (NNType)message.readUInt8();
                    UInt32 FanBeiCount = message.readUInt32();//翻倍数
                }
            }
                #region

                /*
                PartGameOverControl.instance.HelperPos = (int)message.readUInt8();

                PartGameOverControl.instance.SettleInfoList = new List<SettleDownInfo>();
                int Count = message.readInt32();
                for (int i = 0; i < Count; i++)
                {


                    #region
                    bool IsWin = message.readBool();
                    byte pos = message.readUInt8();

                    int Score = message.readInt32();
                    int ChangeScore = message.readInt32();
                    int BaseScore = message.readInt32();
                    uint TaoShangScore = message.readUInt32();


                    //保存数据
                    SettleDownInfo info1 = new SettleDownInfo();
                    info1.IsWin = IsWin;
                    info1.pos = (int)pos;
                    info1.Score = Score;//人物总积分
                    info1.BaseScore = BaseScore;
                    info1.TaoShangFen = (int)TaoShangScore;//讨赏分
                    info1.ChangeScore = ChangeScore;//总改变分数





                    int leftCardCount = message.readInt32();
                    List<uint> Cardslist = new List<uint>();
                    for (int j = 0; j < leftCardCount; j++)
                    {
                        Cardslist.Add(message.readUInt32());
                    }
                    info1.LeftCardList = Cardslist;//剩余排数
                    info1.Index = 4;
                    PartGameOverControl.instance.SettleInfoList.Add(info1);

                    #endregion

                }

                int playerCount = message.readInt32();//完成玩家人数
                for (int i = 0; i < playerCount; i++)
                {
                    int pos1 = message.readUInt8();//位置
                    int index = message.readUInt8();//第几个完成
                    for (int j = 0; j < PartGameOverControl.instance.SettleInfoList.Count; j++)
                    {
                        if (pos1 == PartGameOverControl.instance.SettleInfoList[j].pos)
                        {
                            PartGameOverControl.instance.SettleInfoList[j].Index = index;
                        }
                    }
                }
                */
                #endregion
            }


        GameData.m_TableInfo = info;

        GameData.Dice1 = GameData.GenerateDice(1);
        GameData.Dice2 = GameData.GenerateDice(2);
        UIManager.Instance.HideUiPanel(UIPaths.PanelCreatRoom);

        ManagerScene.Instance.LoadScene(SceneType.NiuNiu);

        //if (!IsPiPei)
        //{
        //    ManagerScene.Instance.LoadScene(SceneType.Game);
        //}
        //else//金币场
        //{
        //    ManagerScene.Instance.LoadScene(SceneType.GameJinBi);
        //}



    }
    /// <summary>
    /// 讨赏房间信息处理
    /// </summary>
    /// <param name="message"></param>
    private void OnTaoShnagRoomInfo(NetworkMessage message)
    {
        GameData.m_PlayerInfoList.Clear();
        GameData.m_HoldCardsList.Clear();
        GameDataFunc.ClearData();
        GameDataFunc.ClearDataObj();
        TableInfo info = new TableInfo();

        info.IsPiPei = IsPiPei;
        info.IsDaiLiCreat = IsDaiLiCreat;
       // RoomType roomType = (RoomType)message.readUInt8();//房间类型 这里不需要

        info.id = message.readUInt32();//房间号
        info.fangZhuGuid = message.readUInt64();//房主ID
        info.configRoundIndex = (byte)message.readUInt32();//几局

        info.configPayIndex = message.readUInt8();

        info.ZhuangPos = message.readUInt8();//庄家位置

        PartGameOverControl.instance.ZhuangPos = (int)info.ZhuangPos;
        info.lianZhuangCount = (uint)message.readUInt32();//连庄次数

        info.roomState = (RoomStatusType)message.readUInt8();

        info.curGameCount = (uint)message.readUInt32();//当前局数
                                                       //info.configPlayerIndex = message.readUInt8();

        // info.makerPos = message.readUInt8();
        bool HaveColdTime = message.readBool();//是否有房间冷却
        if (HaveColdTime)
        {
            uint time = message.readUInt32();
        }


        info.isQueryLeaveRoom = message.readBool();//是否有请求离开的请求
        if (info.isQueryLeaveRoom)
        {
            info.queryLeaveRoomWaitTime = message.readUInt32();
            byte count = (byte)message.readUInt8();

            for (int i = 0; i < count; i++)
                info.operateLeaveRoomList.Add((byte)message.readUInt8());
        }

        byte pCount = (byte)message.readUInt8();//玩家个数
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

            pInfo.IsAi = message.readBool();//是否托管
            pInfo.isForce = message.readBool();

            pInfo.ip = message.readString();
            pInfo.mask = message.readString();
            pInfo.name = message.readString();
            pInfo.headID = message.readString();

            pInfo.changeScore =(int)message.readInt32();

            pInfo.TSTaoShangScore = message.readUInt32();
            pInfo.score = message.readInt32();

            if (IsPiPei)//匹配的金币场
            {
                pInfo.Gold = message.readInt64();
            }

            pInfo.totalHuCount = message.readUInt32();
            pInfo.totalMakerCount = message.readUInt32();//当庄次数
            pInfo.OperateType = (CardOperateType)message.readUInt8();//操作类型
            #region

            #endregion
            // byte oCount = message.readInt32();//玩家手牌

            int count = message.readInt32();
            pInfo.LeftCardNum = count;
            if (Player.Instance.guid == pInfo.guid)
            {
                for (int k = 0; k < count; k++)
                {
                    pInfo.localCardList.Add(message.readUInt32());
                }
            }
            else
            {
                for (int k = 0; k < count; k++)
                {
                    pInfo.localCardList.Add(0);
                }
            }


            int oCount = message.readInt32();//丢去的牌  出的牌

            for (int k = 0; k < oCount; k++)
            {
                pInfo.outCardList.Add(message.readUInt32());
            }

            GameData.m_PlayerInfoList.Add(pInfo);
        }

        info.FriendPos = 0;//防止断线重连后位置有无
        if (info.roomState == RoomStatusType.Play)//断线回来有用
        {

            uint FriendCard = message.readUInt32();//朋友卡
            uint friendpos = (uint)message.readUInt8();//朋友的位置
            info.FriendCard = FriendCard;
            info.FriendPos = (int)friendpos;
            PartGameOverControl.instance.HelperPos = info.FriendPos;
            // info.isWaitFangPao = !message.readBool();
            info.lastOutCardPos = message.readUInt8();//最后出牌的位置
            info.waitOutCardPos = message.readUInt8();//该出牌的位置

            

            // info.resCardCount = message.readUInt32();
            info.isOutCardInfo = message.readBool();//是否有出的牌
            if (info.isOutCardInfo)
            {
              
                int count = message.readInt32();
                for (int i = 0; i < count; i++)
                {
                    uint Card = message.readUInt32();
                }
            }


            GameData.FinishPlayerPos = new List<FinishPlayer>();
            int playerCount = message.readInt32();//完成玩家人数
            for (int i = 0; i < playerCount; i++)
            {
                int pos1 = message.readUInt8();//位置
                int index = message.readUInt8();//第几个完成

                FinishPlayer player = new FinishPlayer();
                player.pos = pos1;
                player.index = index;
                GameData.FinishPlayerPos.Add(player);
            }

        
        }
        else if (info.roomState == RoomStatusType.Over)
        {
            PartGameOverControl.instance.ZhuangPos = (int)message.readUInt8();
            PartGameOverControl.instance.HelperPos = (int)message.readUInt8();

            PartGameOverControl.instance.SettleInfoList = new List<SettleDownInfo>();
            int Count = message.readInt32();
            for (int i = 0; i < Count; i++)
            {


                #region
                bool IsWin = message.readBool();
                byte pos = message.readUInt8();

                int Score = message.readInt32();
                int ChangeScore = message.readInt32();
                int HuiHeFen = message.readInt32();
                uint TaoShangScore = message.readUInt32();


                //保存数据
                SettleDownInfo info1 = new SettleDownInfo();
                info1.IsWin = IsWin;
                info1.Pos = (int)pos;
                info1.Score = Score;//人物总积分
                info1.BaseScore = HuiHeFen;
                info1.TaoShangFen = (int)TaoShangScore;//讨赏分
                info1.ChangeScore = ChangeScore;//总改变分数





                int leftCardCount = message.readInt32();
                List<uint> Cardslist = new List<uint>();
                for (int j = 0; j < leftCardCount; j++)
                {
                    Cardslist.Add(message.readUInt32());
                }
                info1.LeftCardList = Cardslist;//剩余排数
                info1.Index = 4;
                PartGameOverControl.instance.SettleInfoList.Add(info1);

                #endregion

            }

            int playerCount = message.readInt32();//完成玩家人数
            for (int i = 0; i < playerCount; i++)
            {
                int pos1 = message.readUInt8();//位置
                int index = message.readUInt8();//第几个完成
                for (int j = 0; j < PartGameOverControl.instance.SettleInfoList.Count; j++)
                {
                    if (pos1 == PartGameOverControl.instance.SettleInfoList[j].Pos)
                    {
                        PartGameOverControl.instance.SettleInfoList[j].Index = index;
                    }
                }
            }


        }


        GameData.m_TableInfo = info;
      
        GameData.Dice1 = GameData.GenerateDice(1);
        GameData.Dice2 = GameData.GenerateDice(2);
        UIManager.Instance.HideUiPanel(UIPaths.PanelCreatRoom);

        if (!IsPiPei)
        {
            ManagerScene.Instance.LoadScene(SceneType.Game);
        }
        else//金币场
        {
            ManagerScene.Instance.LoadScene(SceneType.GameJinBi);
        }
     


    }

    /// <summary>
    /// 无挡胡房间信息处理
    /// </summary>
    /// <param name="message"></param>
    private void OnWuTangHuRoomInfo(NetworkMessage message)
    {
        GameData.m_PlayerInfoList.Clear();
        GameData.m_HoldCardsList.Clear();
        GameDataFunc.ClearData();
        GameDataFunc.ClearDataObj();
        TableInfo info = new TableInfo();
        info.IsPiPei = IsPiPei;
        info.IsDaiLiCreat = IsDaiLiCreat;
        //  RoomType roomType = (RoomType)message.readUInt8();//房间类型 这里不需要

        info.id = message.readUInt32();//房间号
        info.fangZhuGuid = message.readUInt64();//房主ID
        info.configRoundIndex = (byte)message.readUInt32();//几局

        info.configPayIndex = message.readUInt8();

        info.ZhuangPos = message.readUInt8();//庄家位置

        PartGameOverControl.instance.ZhuangPos = (int)info.ZhuangPos;
        info.lianZhuangCount = (uint)message.readUInt32();//连庄次数

        info.roomState = (RoomStatusType)message.readUInt8();

        info.curGameCount = (uint)message.readUInt32();//当前局数
                                                       //info.configPlayerIndex = message.readUInt8();
        info.IsJIangMa = message.readBool();//是否奖码
                                            // info.makerPos = message.readUInt8();

        bool HaveColdTime = message.readBool();//是否有房间冷却
        if (HaveColdTime)
        {
            uint time = message.readUInt32();
        }


        info.isQueryLeaveRoom = message.readBool();//是否有请求离开的请求
        if (info.isQueryLeaveRoom)
        {
            info.queryLeaveRoomWaitTime = message.readUInt32();
            byte count = (byte)message.readUInt8();

            for (int i = 0; i < count; i++)
                info.operateLeaveRoomList.Add((byte)message.readUInt8());
        }

        byte pCount = (byte)message.readUInt8();//玩家个数
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

            pInfo.IsAi = message.readBool();//是否托管
            pInfo.isForce = message.readBool();

            pInfo.ip = message.readString();
            pInfo.mask = message.readString();
            pInfo.name = message.readString();
            pInfo.headID = message.readString();

            pInfo.changeScore = message.readInt32();
            pInfo.score = message.readInt32();
            if (IsPiPei)//匹配的金币场
            {
                pInfo.Gold = message.readInt64();
            }

            pInfo.totalHuCount = message.readUInt32();
            pInfo.totalMakerCount = message.readUInt32();//当庄次数
            pInfo.OperateType = (CardOperateType)message.readUInt8();//操作类型
    
            // byte oCount = message.readInt32();//玩家手牌

            int count = message.readInt32();
            pInfo.LeftCardNum = count;
            if (Player.Instance.guid == pInfo.guid)
            {
                for (int k = 0; k < count; k++)
                {
                    pInfo.localCardList.Add(message.readUInt32());
                }
            }
            else
            {
                for (int j = 0; j < count; j++)//其他玩家  只有数量 没有具体值
                {
                    pInfo.localCardList.Add(0);
                }

            }


            int oCount = message.readInt32();//丢去的牌  出的牌

            for (int k = 0; k < oCount; k++)
            {
                pInfo.outCardList.Add(message.readUInt32());
            }

          


            //玩家吃碰杠的牌
            if (roomType == RoomType.WDH)
            {
                pInfo.operateCardList = new List<OpreateCardInfo>();


                pInfo.IsFirstChuPai = message.readBool();//是否为庄家第一次出牌
                pInfo.IsGangKai = message.readBool();//是否为杠开
                int OperateCardCount = message.readInt32();// 吃碰的牌
                for (int j = 0; j < OperateCardCount; j++)
                {
                    OpreateCardInfo operateinfo = new OpreateCardInfo();


                    byte Givenplayerpos = message.readUInt8();//谁丢出的（谁给的吃碰）
                    CatchType playerOperateType = (CatchType)message.readUInt8();

                    operateinfo.pos = Givenplayerpos;
                    operateinfo.opType = playerOperateType;

                    if (playerOperateType == CatchType.Chi)//没有
                    {
                        int ChiCount = message.readInt32();
                        for (int k = 0; k < ChiCount; k++)
                        {
                            uint ChiCard = message.readUInt32();
                          
                        }
                    }
                    else if (playerOperateType == CatchType.AnGang|| playerOperateType == CatchType.Gang|| playerOperateType == CatchType.BuGang)//没有
                    {
                        uint ChiCard = message.readUInt32();
                        operateinfo.opCard = ChiCard;
                    }
                    else if (playerOperateType == CatchType.Peng)
                    {
                        uint ChiCard = message.readUInt32();
                        operateinfo.opCard = ChiCard;
                    }


                    pInfo.operateCardList.Add(operateinfo);
                }
            }

            GameData.m_PlayerInfoList.Add(pInfo);

        }
        
        
        info.FriendPos = 0;//防止断线重连后位置有无
        if (info.roomState == RoomStatusType.Play)//断线回来有用
        {


            #region  讨赏
            //uint FriendCard = message.readUInt32();//朋友卡
            //uint friendpos = (uint)message.readUInt8();//朋友的位置
            //info.FriendCard = FriendCard;
            //info.FriendPos = (int)friendpos;
            //PartGameOverControl.instance.HelperPos = info.FriendPos;
            //// info.isWaitFangPao = !message.readBool();
            //info.lastOutCardPos = message.readUInt8();//最后出牌的位置
            //info.waitOutCardPos = message.readUInt8();//该出牌的位置



            //// info.resCardCount = message.readUInt32();
            //info.isOutCardInfo = message.readBool();//是否有出的牌
            //if (info.isOutCardInfo)
            //{

            //    int count = message.readInt32();
            //    for (int i = 0; i < count; i++)
            //    {
            //        uint Card = message.readUInt32();
            //    }
            //}


            //GameData.FinishPlayerPos = new List<FinishPlayer>();
            //int playerCount = message.readInt32();//完成玩家人数
            //for (int i = 0; i < playerCount; i++)
            //{
            //    int pos1 = message.readUInt8();//位置
            //    int index = message.readUInt8();//第几个完成

            //    FinishPlayer player = new FinishPlayer();
            //    player.pos = pos1;
            //    player.index = index;
            //    GameData.FinishPlayerPos.Add(player);
            //}

            #endregion

            #region


            
            // info.isWaitFangPao = !message.readBool();
            info.lastOutCardPos = message.readUInt8();
            info.waitOutCardPos = message.readUInt8();
            info.resCardCount = message.readUInt32();//牌桌上的剩余牌
            info.isOutCardInfo = message.readBool();
            if (info.isOutCardInfo)
            {
                info.outCardPos = message.readUInt8();
                info.outCardNumber = message.readUInt32();
            }

            info.isInCardInfo = message.readBool();//摸牌信息
            if (info.isInCardInfo)
            {
                info.inCardPos = message.readUInt8();
                info.inCardNumber = message.readUInt32();
            }

            //info.isQiangGangHu = message.readBool();
            //if (info.isQiangGangHu)
            //{
            //    info.qiangGangPos = message.readUInt8();
            //    info.qiangGangCard = message.readUInt32();
            //    byte qgCount = message.readUInt8();
            //    for (int i = 0; i < qgCount; i++)
            //    {
            //        info.operateQiangGangList.Add(message.readUInt8());
            //    }
            //}
            #endregion


        }
        else if (info.roomState == RoomStatusType.Over)
        {

            #region 讨赏
            //  PartGameOverControl.instance.ZhuangPos = (int)message.readUInt8();
            //PartGameOverControl.instance.HelperPos = (int)message.readUInt8();

            //PartGameOverControl.instance.SettleInfoList = new List<SettleDownInfo>();
            //int Count = message.readInt32();
            //for (int i = 0; i < Count; i++)
            //{


            //    #region
            //    bool IsWin = message.readBool();
            //    byte pos = message.readUInt8();

            //    int Score = message.readInt32();
            //    int ChangeScore = message.readInt32();
            //    int BaseScore = message.readInt32();
            //    uint TaoShangScore = message.readUInt32();


            //    //保存数据
            //    SettleDownInfo info1 = new SettleDownInfo();
            //    info1.IsWin = IsWin;
            //    info1.pos = (int)pos;
            //    info1.Score = Score;//人物总积分
            //    info1.BaseScore = BaseScore;
            //    info1.TaoShangFen = (int)TaoShangScore;//讨赏分
            //    info1.ChangeScore = ChangeScore;//总改变分数





            //    int leftCardCount = message.readInt32();
            //    List<uint> Cardslist = new List<uint>();
            //    for (int j = 0; j < leftCardCount; j++)
            //    {
            //        Cardslist.Add(message.readUInt32());
            //    }
            //    info1.LeftCardList = Cardslist;//剩余排数
            //    info1.Index = 4;
            //    PartGameOverControl.instance.SettleInfoList.Add(info1);

            //    #endregion

            //}

            //int playerCount = message.readInt32();//完成玩家人数
            //for (int i = 0; i < playerCount; i++)
            //{
            //    int pos1 = message.readUInt8();//位置
            //    int index = message.readUInt8();//第几个完成
            //    for (int j = 0; j < PartGameOverControl.instance.SettleInfoList.Count; j++)
            //    {
            //        if (pos1 == PartGameOverControl.instance.SettleInfoList[j].pos)
            //        {
            //            PartGameOverControl.instance.SettleInfoList[j].Index = index;
            //        }
            //    }
            //}

            #endregion


            #region
            //PartGameOverControl.instance.WuDangHuSettleInfoList = new List<WUHSettleDownInfo>();

            //int zhuangPos = message.readUInt8();//庄家位置

            //int Count = (int)message.readUInt8();//玩家数量
            //for (int i = 0; i < Count; i++)
            //{
            //    PlayerInfo WDHInfo = new PlayerInfo();
            //    WDHInfo.pos= message.readUInt8();

            //    WDHInfo. Score = message.readInt32();
            //    WDHInfo. ChangeScore = message.readInt32();
            //    WDHInfo .HuiHeScore = message.readInt32();
            //    WDHInfo. TaoShangScore = (int)message.readUInt32();//奖码分

            //    WDHInfo.CardHutype =(HuType) message.readUInt8();

            //    WDHInfo.GetCardCount = message.readUInt8();//碰的牌
            //    for (int j = 0; j < (int)WDHInfo.GetCardCount; j++)
            //    {
            //        GetCardInfo getcardinfo = new GetCardInfo();
            //        getcardinfo.Pos = message.readUInt8();
            //        getcardinfo.type = (CatchType)message.readUInt8();
            //        getcardinfo.Card =(uint) message.readUInt32();

            //        WDHInfo.GetCardInfoList.Add(getcardinfo);
            //    }

            //    PlayerInfo playerinfo = GameDataFunc.GetPlayerInfo(WDHInfo.pos);//恢复手牌
            //    int leftCardCount = (int)message.readUInt8();//剩余手牌
            //    for (int j = 0; j < leftCardCount; j++)
            //    {

            //        playerinfo.localCardList.Add(message.readUInt32());
            //    }

            //    PartGameOverControl.instance.WuDangHuSettleInfoList.Add(WDHInfo);

            // #endregion
            //  }
            #endregion

            int zhuangPos = message.readUInt8();//庄家位置

            uint LeftCardNum = message.readUInt32();

            int Count = (int)message.readUInt8();//玩家数量

            for (int i = 0; i < Count; i++)
            {
                byte pos = message.readUInt8();
                PlayerInfo info1 = GameDataFunc.GetPlayerInfo(pos);

                info1.score = message.readInt32();
                info1.changeScore = message.readInt32();
                info1.HuiHeScore = message.readInt32();
                info1.TaoShangScore = (int)message.readUInt32();//奖码分

                // info.menCount = message.readUInt8();
                info1.huType = (HuType)message.readUInt8();
                info1.prizeType = (PrizeType)message.readUInt8();//翻倍总类
                info1.GangKaiCount = (int)message.readUInt32();//杠开次数

                //吃碰杠的牌
                info1.operateCardList.Clear();
                byte oCount = message.readUInt8();
                for (int k = 0; k < oCount; k++)
                {
                    OpreateCardInfo opInfo = new OpreateCardInfo();
                    opInfo.pos = message.readUInt8();
                    opInfo.opType = (CatchType)message.readUInt8();

                    if (opInfo.opType == CatchType.Chi)
                    {
                        int chiCount = message.readInt32();
                        for (int j = 0; j < chiCount; j++)
                        {

                        }
                    }
                    else
                    {
                        //没有吃所以不用判断牌型
                        opInfo.opCard = message.readUInt32();
                        info1.operateCardList.Add(opInfo);
                    }

                }


                info1.localCardList.Clear();//手牌
                oCount = message.readUInt8();
                for (int k = 0; k < oCount; k++)
                {
                    info1.localCardList.Add(message.readUInt32());
                }
            }

          


            PartGameOverControl.instance.ISWDHHuPai =message.readBool();//是否有人胡牌
            if (PartGameOverControl.instance.ISWDHHuPai)
            {
                PartGameOverControl.instance.HuPos = message.readUInt8();
                PartGameOverControl.instance.WDHHuType =(HuType) message.readUInt32();
            }
           
            

        }


        GameData.m_TableInfo = info;
        GameData.m_TableInfo.makerPos = info.ZhuangPos;
        GameData.Dice1 = GameData.GenerateDice(1);
        GameData.Dice2 = GameData.GenerateDice(2);
        UIManager.Instance.HideUiPanel(UIPaths.PanelCreatRoom);


      

        if (!IsPiPei)
        {
            ManagerScene.Instance.LoadScene(SceneType.WDHGame);
        }
        else//金币场
        {
            ManagerScene.Instance.LoadScene(SceneType.WDHGameJinBi);
        }
    }

    /// <summary>
    /// 栽宝信息处理
    /// </summary>
    /// <param name="message"></param>
    private void OnZaiBaoRoomInfo(NetworkMessage message)
    {
        GameData.m_PlayerInfoList.Clear();
        GameData.m_HoldCardsList.Clear();
        GameDataFunc.ClearData();
        GameDataFunc.ClearDataObj();
        TableInfo info = new TableInfo();
        info.IsPiPei = IsPiPei;
        info.IsDaiLiCreat = IsDaiLiCreat;
        //  RoomType roomType = (RoomType)message.readUInt8();//房间类型 这里不需要

        info.id = message.readUInt32();//房间号
        info.fangZhuGuid = message.readUInt64();//房主ID
        info.configRoundIndex = (byte)message.readUInt32();//几局

        info.configPayIndex = message.readUInt8();

        info.ZhuangPos = message.readUInt8();//庄家位置

        PartGameOverControl.instance.ZhuangPos = (int)info.ZhuangPos;
        info.lianZhuangCount = (uint)message.readUInt32();//连庄次数

        info.roomState = (RoomStatusType)message.readUInt8();

        info.curGameCount = (uint)message.readUInt32();//当前局数
                                                       //info.configPlayerIndex = message.readUInt8();
                                                       // info.IsJIangMa = message.readBool();//是否奖码
                                                       // info.makerPos = message.readUInt8();

        bool HaveColdTime = message.readBool();//是否有房间冷却
        if (HaveColdTime)
        {
            uint time = message.readUInt32();
        }

        info.isQueryLeaveRoom = message.readBool();//是否有请求离开的请求
        if (info.isQueryLeaveRoom)
        {
            info.queryLeaveRoomWaitTime = message.readUInt32();
            byte count = (byte)message.readUInt8();

            for (int i = 0; i < count; i++)
                info.operateLeaveRoomList.Add((byte)message.readUInt8());
        }

        byte pCount = (byte)message.readUInt8();//玩家个数
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

            pInfo.IsAi = message.readBool();//是否托管
            pInfo.isForce = message.readBool();

            pInfo.ip = message.readString();
            pInfo.mask = message.readString();
            pInfo.name = message.readString();
            pInfo.headID = message.readString();

            pInfo.changeScore = message.readInt32();
            pInfo.score = message.readInt32();

            if (IsPiPei)//匹配的金币场
            {
                pInfo.Gold = message.readInt64();
            }

            pInfo.totalHuCount = message.readUInt32();
            pInfo.totalMakerCount = message.readUInt32();//当庄次数
            pInfo.OperateType = (CardOperateType)message.readUInt8();//操作类型

            // byte oCount = message.readInt32();//玩家手牌

            int count = message.readInt32();
            pInfo.LeftCardNum = count;
            if (Player.Instance.guid == pInfo.guid)
            {
                for (int k = 0; k < count; k++)
                {
                    pInfo.localCardList.Add(message.readUInt32());
                }
            }
            else
            {
                for (int j = 0; j < count; j++)//其他玩家  只有数量 没有具体值
                {
                    pInfo.localCardList.Add(0);
                }

            }


            int oCount = message.readInt32();//丢去的牌  出的牌

            for (int k = 0; k < oCount; k++)
            {
                
                pInfo.outCardList.Add(message.readUInt32());
            }




            //玩家吃碰杠的牌
            if (roomType == RoomType.ZB)
            {
                pInfo.operateCardList = new List<OpreateCardInfo>();


                pInfo.IsFirstChuPai = message.readBool();//是否为庄家第一次出牌
                pInfo.IsGangKai = message.readBool();//是否为杠开
                int OperateCardCount = message.readInt32();// 吃碰的牌 一碰为一
                for (int j = 0; j < OperateCardCount; j++)
                {
                    OpreateCardInfo operateinfo = new OpreateCardInfo();


                    byte Givenplayerpos = message.readUInt8();//谁丢出的（谁给的吃碰）
                    CatchType playerOperateType = (CatchType)message.readUInt8();

                    operateinfo.pos = Givenplayerpos;
                    operateinfo.opType = playerOperateType;

                    if (playerOperateType == CatchType.Chi)//没有
                    {
                        int ChiCount = message.readInt32();
                        for (int k = 0; k < ChiCount; k++)
                        {
                            uint ChiCard = message.readUInt32();

                        }
                    }
                    else if (playerOperateType == CatchType.AnGang || playerOperateType == CatchType.Gang || playerOperateType == CatchType.BuGang)//没有
                    {
                        uint ChiCard = message.readUInt32();
                        operateinfo.opCard = ChiCard;
                        info.GangCount++;//杠的次数
                    }
                    else if (playerOperateType == CatchType.Peng)
                    {
                        uint ChiCard = message.readUInt32();
                        operateinfo.opCard = ChiCard;
                    }


                    pInfo.operateCardList.Add(operateinfo);
                }
            }

            GameData.m_PlayerInfoList.Add(pInfo);

        }


        info.FriendPos = 0;//防止断线重连后位置有无
        if (info.roomState == RoomStatusType.Play)//断线回来有用
        {


            #region  讨赏
            //uint FriendCard = message.readUInt32();//朋友卡
            //uint friendpos = (uint)message.readUInt8();//朋友的位置
            //info.FriendCard = FriendCard;
            //info.FriendPos = (int)friendpos;
            //PartGameOverControl.instance.HelperPos = info.FriendPos;
            //// info.isWaitFangPao = !message.readBool();
            //info.lastOutCardPos = message.readUInt8();//最后出牌的位置
            //info.waitOutCardPos = message.readUInt8();//该出牌的位置



            //// info.resCardCount = message.readUInt32();
            //info.isOutCardInfo = message.readBool();//是否有出的牌
            //if (info.isOutCardInfo)
            //{

            //    int count = message.readInt32();
            //    for (int i = 0; i < count; i++)
            //    {
            //        uint Card = message.readUInt32();
            //    }
            //}


            //GameData.FinishPlayerPos = new List<FinishPlayer>();
            //int playerCount = message.readInt32();//完成玩家人数
            //for (int i = 0; i < playerCount; i++)
            //{
            //    int pos1 = message.readUInt8();//位置
            //    int index = message.readUInt8();//第几个完成

            //    FinishPlayer player = new FinishPlayer();
            //    player.pos = pos1;
            //    player.index = index;
            //    GameData.FinishPlayerPos.Add(player);
            //}

            #endregion

            #region

            info.RoomGuid = message.readUInt64();
           info.FirstDices = message.readUInt32();//骰子
           info.SecendDices= message.readUInt32();
            info.ThirdDices = message.readUInt32();//骰子
            info.FouthDices = message.readUInt32();

            GameData.Dice1 = (int)info.FirstDices;
            GameData.Dice2 = (int)info.SecendDices;
            GameData.Dice3 = (int)info.ThirdDices;
            GameData.Dice4 = (int)info.FouthDices;


            info.MianCard = message.readUInt32();//面牌
           info.MagicCard= message.readUInt32();//万能牌

         

            // info.isWaitFangPao = !message.readBool();
            info.lastOutCardPos = message.readUInt8();
            info.waitOutCardPos = message.readUInt8();
            info.resCardCount = message.readUInt32();//牌桌上的剩余牌
            info.isOutCardInfo = message.readBool();
            if (info.isOutCardInfo)
            {
                info.outCardPos = message.readUInt8();
                info.outCardNumber = message.readUInt32();
            }

            info.isInCardInfo = message.readBool();//摸牌信息
            if (info.isInCardInfo)
            {
                info.inCardPos = message.readUInt8();
                info.inCardNumber = message.readUInt32();
            }

            //info.isQiangGangHu = message.readBool();
            //if (info.isQiangGangHu)
            //{
            //    info.qiangGangPos = message.readUInt8();
            //    info.qiangGangCard = message.readUInt32();
            //    byte qgCount = message.readUInt8();
            //    for (int i = 0; i < qgCount; i++)
            //    {
            //        info.operateQiangGangList.Add(message.readUInt8());
            //    }
            //}
            #endregion


        }
        #region
        else if (info.roomState == RoomStatusType.Over)
        {
            

            int zhuangPos = message.readUInt8();//庄家位置





            info.FirstDices = message.readUInt32();//骰子
            info.SecendDices = message.readUInt32();
            info.ThirdDices = message.readUInt32();//骰子
            info.FouthDices = message.readUInt32();

            GameData.Dice1 = (int)info.FirstDices;
            GameData.Dice2 = (int)info.SecendDices;
            GameData.Dice3 = (int)info.ThirdDices;
            GameData.Dice4 = (int)info.FouthDices;

            info.MianCard = message.readUInt32();//面牌
            info.MagicCard = message.readUInt32();//万能牌
            info.resCardCount = message.readUInt32();//牌桌上的剩余牌


            int Count = (int)message.readUInt8();//玩家数量

            for (int i = 0; i < Count; i++)
            {
                byte pos = message.readUInt8();
                PlayerInfo info1 = GameDataFunc.GetPlayerInfo(pos);

                info1.score = message.readInt32();
                info1.changeScore = message.readInt32();
                info1.HuiHeScore = message.readInt32();
                info1.TaoShangScore = (int)message.readUInt32();//奖码分

                // info.menCount = message.readUInt8();
                info1.huType = (HuType)message.readUInt8();
                info1.prizeType = (PrizeType)message.readUInt8();//翻倍总类
                info1.GangKaiCount = (int)message.readUInt32();//杠开次数

                //吃碰杠的牌
                info1.operateCardList.Clear();
                byte oCount = message.readUInt8();
                for (int k = 0; k < oCount; k++)
                {
                    OpreateCardInfo opInfo = new OpreateCardInfo();
                    opInfo.pos = message.readUInt8();
                    opInfo.opType = (CatchType)message.readUInt8();

                    if (opInfo.opType == CatchType.Chi)
                    {
                        int chiCount = message.readInt32();
                        for (int j = 0; j < chiCount; j++)
                        {

                        }
                    }
                    else
                    {
                        //没有吃所以不用判断牌型
                        opInfo.opCard = message.readUInt32();
                        info1.operateCardList.Add(opInfo);
                    }

                }


                info1.localCardList.Clear();//手牌
                oCount = message.readUInt8();
                for (int k = 0; k < oCount; k++)
                {
                    info1.localCardList.Add(message.readUInt32());
                }
            }




            PartGameOverControl.instance.ISWDHHuPai = message.readBool();//是否有人胡牌
            if (PartGameOverControl.instance.ISWDHHuPai)
            {
                PartGameOverControl.instance.HuPos = message.readUInt8();
                PartGameOverControl.instance.WDHHuType = (HuType)message.readUInt32();

                uint MianCount = message.readUInt32();//吃面次数
                uint ZaiBaoCount = message.readUInt32();//载保次数
                uint GangCount = message.readUInt32();//杠的次数
            }



        }
        #endregion

        GameData.m_TableInfo = info;
        GameData.m_TableInfo.ForbiddenIndex = 135 - ((int)(GameData.Dice3 + GameData.Dice4) * 2) - 1;
        GameData.m_TableInfo.makerPos = info.ZhuangPos;
      //  GameData.Dice1 = GameData.GenerateDice(1);
      //  GameData.Dice2 = GameData.GenerateDice(2);
        UIManager.Instance.HideUiPanel(UIPaths.PanelCreatRoom);


        #region  吧玩家打出的面牌剔除 加入面牌列表里

        int ChiMianCount = 0;
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            List<uint> NewOutCardList = new List<uint>();
            for (int j = 0; j < GameData.m_PlayerInfoList[i].outCardList.Count; j++)
            {
                if (GameData.m_PlayerInfoList[i].outCardList[j] == GameData.m_TableInfo.MianCard  )
                {
                    ChiMianCount++;
                    GameData.m_PlayerInfoList[i].MianCardList.Add(GameData.m_PlayerInfoList[i].outCardList[j]);//加到面牌里面
                }
                else if (GameData.m_PlayerInfoList[i].outCardList[j] == GameData.m_TableInfo.MagicCard)
                {
                    GameData.m_PlayerInfoList[i].MianCardList.Add(GameData.m_PlayerInfoList[i].outCardList[j]);//加到面牌里面
                }
                else
                {
                    NewOutCardList.Add(GameData.m_PlayerInfoList[i].outCardList[j]);
                }
            }

            GameData.m_PlayerInfoList[i].outCardList = NewOutCardList;
        }

        GameData.m_TableInfo.ChiMianCount = ChiMianCount;//吃面次数


        #endregion

       

        if (!IsPiPei)
        {
            ManagerScene.Instance.LoadScene(SceneType.ZBGame);
        }
        else//金币场
        {
            ManagerScene.Instance.LoadScene(SceneType.ZBGameJinBi);
        }
    }
}

