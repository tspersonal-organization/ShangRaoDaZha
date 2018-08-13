using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DzViewGame : UIBase<DzViewGame>
{

    #region  包牌相关
    public GameObject BaoPaiTipPanel;
    public UIButton BaoPaiButton;
    public UIButton CancleBaoPaiButton;
    #endregion



    #region

    public byte LocalPos;//自己玩家当前位置

    public GameObject CardObj;//牌的预制体
    public GameObject CardPoint;//卡牌的起点
    List<GameObject> CurChooseCardsObjList = new List<GameObject>();//选中的牌

    List<uint> CardList = new List<uint>();//手牌数据
    List<GameObject> LocalCardsObjList = new List<GameObject>();//手牌

    public UIButton InviteFriend;//邀请朋友
    public UIButton ReadyBtn;//准备按钮
    public UIButton DisposRoomBtn;//
    public UIButton QuiteRoomBtn;//准备按钮

    public UIButton DontPlayBtn;//不出按钮
    public UIButton YaoBuQiBtn;//要不起按钮
    public UIButton NoticeBtn;//提示按钮
    public UIButton PlayCardBtn;//出牌按钮
    public UIButton DisMissRoomBtn;//解散房间按钮
    public UIButton ChatBtn;

    public UILabel RoomIdLable;//房间号
    public UILabel RuleLable;//规则
    public UILabel DetailRuleLable;//详细规则

    public List<GameObject> PlayerList;//其他玩家列表
    public Dictionary<int, GameObject> IdAndPlayerDic;//玩家id和gameobj  其实就是pos1234
    public Dictionary<int, List<GameObject>> PosAndPlayedCard = new Dictionary<int, List<GameObject>>();//各个玩家打出的牌
    public List<uint> LargestCard = new List<uint>();//当前牌桌上出的最大牌
    List<List<uint>> CanPressCardList = new List<List<uint>>();//提示能打过的牌

    private uint LargestPos = 0;
    public  int SelfPos;//自己的位置
    public int FriendPos = 0;

    public bool IsTuoGuan = false;//玩家是否托管
    public GameObject TableMask;//桌布mask
    public UIButton AiButton;//托管按钮
    public GameObject FriendCard;//朋友牌
    public UIButton SettingBtn;//设置按钮
    public UIButton SortBtn;//排序按钮

    public GameObject TipSeperateBoomObj;//拆炸弹提示框
    public UIButton SureSeperateBoom;
    public UIButton CancleSeperateBoom;

    #region  self
    // public UISprite SelfHeadSprite;//自己的图像id

    #endregion

    #region  playerOne

    #endregion

    #region  playerTwo

    #endregion

    #region  playerThree

    #endregion


    // Use this for initialization
    void Start()
    {
        BaoPaiButton.onClick.Add(new EventDelegate(()=>
        {
            BaoPaiTipPanel.SetActive(false);
            ClientToServerMsg.SendBaoPaiInfo((uint)GameData.m_TableInfo.id,true);
        }));
        CancleBaoPaiButton.onClick.Add(new EventDelegate(() =>
        {
            BaoPaiTipPanel.SetActive(false);
            ClientToServerMsg.SendBaoPaiInfo((uint)GameData.m_TableInfo.id, false);
        }));
        SureSeperateBoom.onClick.Add(new EventDelegate(TipBoomPlayCard));
        CancleSeperateBoom.onClick.Add(new EventDelegate(TipBoomCanclePlayCard));

        GameData.m_IsNormalOver = false;//重置
        SortBtn.onClick.Add(new EventDelegate(this.SetCardSortType));
        ChatBtn.onClick.Add(new EventDelegate(this.OpenChatPanel));
      //  AiButton.onClick.Add(new EventDelegate(this.SetAi));
        SettingBtn.onClick.Add(new EventDelegate(this.OpenSettingPanel));
        UIEventListener.Get(TableMask).onClick = this.CancleSelectCard;
        DisMissRoomBtn.onClick.Add(new EventDelegate(this.DisMissRoom));//DisposRoomBtn
        DisposRoomBtn.onClick.Add(new EventDelegate(this.DisposRoom));//QuiteRoomBtn
        QuiteRoomBtn.onClick.Add(new EventDelegate(this.DisMissRoom));//QuiteRoomBtn
        ReadyBtn.onClick.Add(new EventDelegate(this.ReadyForGame));
        InviteFriend.onClick.Add(new EventDelegate(this.ShareInvite));

        DontPlayBtn.onClick.Add(new EventDelegate(this.DontPlayCard));//  public UIButton YaoBuQiBtn;//要不起按钮
        YaoBuQiBtn.onClick.Add(new EventDelegate(this.DontPlayCard));
        NoticeBtn.onClick.Add(new EventDelegate(this.NoticePlayCard));
        PlayCardBtn.onClick.Add(new EventDelegate(this.PlayCard));

        for (int i = 1; i < 5; i++)
        {
            PosAndPlayedCard[i] = new List<GameObject>();
        }
        IdAndPlayerDic = new Dictionary<int, GameObject>();

        SetRoomInfo();

        SetPlayerListPos();


        if (Player.Instance.lastEnterRoomID != 0)//断线重连
        {
            ReconnectServer();//重连
        }


    }


    private void DisposRoom()
    {
        ClientToServerMsg.Send(Opcodes.Client_DisposeRoom, GameData.m_TableInfo.id);
        SoundManager.Instance.PlaySound(UIPaths.BUTTONCLICK);
    }


    /// <summary>
    /// 设置玩家上中下
    /// </summary>
    public void OnSetFinishPlayerIndex()
    {
        for (int i = 0; i < GameData.FinishPlayerPos.Count; i++)
        {
            if (GameData.FinishPlayerPos[i].index == 1)
            {
                IdAndPlayerDic[GameData.FinishPlayerPos[i].pos].transform.Find("IndexSprite").gameObject.SetActive(true);
                IdAndPlayerDic[GameData.FinishPlayerPos[i].pos].transform.Find("IndexSprite").GetComponent<UISprite>().spriteName = "UI_game_icon_1";
            }
            else if (GameData.FinishPlayerPos[i].index == 2)
            {
                IdAndPlayerDic[GameData.FinishPlayerPos[i].pos].transform.Find("IndexSprite").gameObject.SetActive(true);
                IdAndPlayerDic[GameData.FinishPlayerPos[i].pos].transform.Find("IndexSprite").GetComponent<UISprite>().spriteName = "UI_game_icon_2";
            }
        }
    }

    /// <summary>
    /// 重置上中下
    /// </summary>
    public void ResetFinishPlayerIndex()
    {
        for (int i = 1; i < 5; i++)
        {
            IdAndPlayerDic[i].transform.Find("IndexSprite").gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 播放语音动画
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="length"></param>
    public void onShowSoundAnimation(ulong guid, float length)
    {
        byte pos = GameDataFunc.GetPlayerInfo(guid).pos;
        Vector3 vecPos = IdAndPlayerDic[(int)pos].transform.Find("HeadSprite").position;
        StartCoroutine(AsynSoundAnimation((int)pos, length));
    }

    /// <summary>
    /// 显示玩家说话
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="timeLength"></param>
    /// <returns></returns>
    public IEnumerator AsynSoundAnimation(int pos, float timeLength)
    {
        Transform tran = IdAndPlayerDic[(int)pos].transform.Find("WhoSound");
        //Transform tran = transform.FindChild("Chat").FindChild("WhoSound");
        //tran.position = pos;
        tran.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeLength);
        tran.gameObject.SetActive(false);
        yield break;
    }

    /// <summary>
    /// 邀请好友
    /// </summary>

    void ShareInvite()
    {
        AuthorizeOrShare.Instance.ShareRoomID(GameData.m_TableInfo.id, "一起来玩儿欢乐上饶打炸！", "欢乐上饶打炸") ;// + ((int)GameData.m_TableInfo.configRoundIndex).ToString() + "局!" + (GameData.m_PlayerInfoList.Count).ToString() + "缺" + (4 - GameData.m_PlayerInfoList.Count).ToString() + "!" + "  房主支付!", "闲娱狗讨赏");

        return;
        // jishu = (int)GameData.m_TableInfo.configPayIndex;
        switch ((int)GameData.m_TableInfo.configPayIndex)
        {
           // jishu = (int)GameData.m_TableInfo.configPayIndex;
            case 0:
               // jishu =1111;
                AuthorizeOrShare.Instance.ShareRoomID(GameData.m_TableInfo.id, "一起来玩闲娱狗吧！"+ ((int)GameData.m_TableInfo.configRoundIndex).ToString() + "局!"+ (GameData.m_PlayerInfoList.Count).ToString() + "缺" +(4 - GameData.m_PlayerInfoList.Count).ToString() +"!"+"  房主支付!","闲娱狗讨赏");
                // RuleLable.text = "局数：" + GameData.m_TableInfo.curGameCount.ToString() + "/" + GameData.m_TableInfo.configRoundIndex.ToString() + "  房主支付";
              //  jishu = 11112222;
                break;
            case 1:
              //  jishu = 2222;
                AuthorizeOrShare.Instance.ShareRoomID(GameData.m_TableInfo.id, "一起来玩闲娱狗吧！" + ((int)GameData.m_TableInfo.configRoundIndex).ToString() + "局!" + (GameData.m_PlayerInfoList.Count).ToString() + "缺" + (4 - GameData.m_PlayerInfoList.Count).ToString() + "!" + "  平摊支付!", "闲娱狗讨赏");
                // RuleLable.text = "局数：" + GameData.m_TableInfo.curGameCount.ToString() + "/" + GameData.m_TableInfo.configRoundIndex.ToString() + "  平均支付";
               // jishu = 22223333;
                break;
        }
      

    }


  
    #region  聊天相关
    //聊天内容
    List<string> ChatList = new List<string>() {
        "快些的出牌啊 ，做什么去了",
        "这个牌你还拽什么拽，你跑天上去",
        "我不是看天面 给你炸了",
        "你也真是哼时  舞的一把里个好牌",
        "里也真要望天爱  舞只好伙里就好",
        "管耶哦  舞的一只好亲家",
        "到屋里去汗哦   你也参与社会赌博",
        "你是帅哥还是美女哦   舞完的留只电话撒 " };

    /// <summary>
    /// 玩家聊天信息 发送格式为  fileName = "5@" + Player.Instance.guid + "@" + txtIndex;
    /// </summary>
    /// <param name="roomid"></param>
    /// <param name="content"></param>
    public void onPlayerSendChatFace(uint roomid, string content)
    {
        if (roomid == GameData.m_TableInfo.id)
        {
            string[] strs = content.Split('@');
            if (strs[0] == "5")//快捷文字聊天
            {
                for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
                {
                    if (GameData.m_PlayerInfoList[i].guid == ulong.Parse(strs[1]))
                    {
                        IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("ChatSprite").GetComponent<ChatControl>().SetValue(ChatList[int.Parse(strs[2])]);
                        SoundControl.Instance.PlayChatSound((int)GameData.m_PlayerInfoList[i].sex, int.Parse(strs[2]));
                    }
                }
            }
            if (strs[0] == "2")//输入的文字
            {
                for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
                {
                    if (GameData.m_PlayerInfoList[i].guid == ulong.Parse(strs[1]))
                    {
                        IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("ChatSprite").GetComponent<ChatControl>().SetValue(strs[2]);
                       // SoundControl.Instance.PlayChatSound((int)GameData.m_PlayerInfoList[i].sex, int.Parse(strs[2]));
                    }
                }
            }
           
        }
    }
    /// <summary>
    /// 打开聊天面板
    /// </summary>
    private void OpenChatPanel()
    {
        UIManager.Instance.ShowUiPanel(UIPaths.PanelChat, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.BUTTONCLICK);
    }

    /// <summary>
    /// 发送表情聊天信息
    /// </summary>
    /// <param name="content"></param>
    public void onPlayerSendFaceChatFace(uint roomid,string content)
    {
        StartCoroutine(AsynCreateChatFace(content));
    }
    IEnumerator AsynCreateChatFace(string content)
    {
        string[] strs = content.Split('@');
        PlayerInfo info = GameDataFunc.GetPlayerInfo(ulong.Parse(strs[1]));
        GameObject preObj = Resources.Load<GameObject>("Common/Face/Face" + strs[2]);
        GameObject face = Instantiate<GameObject>(preObj);
        face.transform.parent = IdAndPlayerDic[info.pos].transform.Find("HeadSprite");
        face.transform.localScale = Vector3.one;
        face.transform.localPosition = Vector3.zero;
        //  SoundManager.Instance.PlaySound(UIPaths.SOUND_CHAT_FACE + strs[2], info.sex);
        yield return new WaitForSeconds(3f);
        Destroy(face);
        yield break;
    }
    #endregion

    #region  托管相关
    /// <summary>
    /// 设置玩家托管
    /// </summary>
    /// <param name="roomNum"></param>
    /// <param name="pos"></param>
    /// <param name="isAi"></param>
    public void OnPlayerAi(int roomNum, int pos, bool isAi)
    {
        if (roomNum == GameData.m_TableInfo.id)
        {
            if (isAi)
            {
                IdAndPlayerDic[pos].transform.Find("OtherPanel").Find("AISprite").gameObject.SetActive(true);



            }
            else
            {
                IdAndPlayerDic[pos].transform.Find("OtherPanel").Find("AISprite").gameObject.SetActive(false);

            }
            if (pos == SelfPos)
            {
                if (isAi)
                {
                    AiButton.transform.Find("AingSprite").gameObject.SetActive(true);
                }
                else
                {
                    AiButton.transform.Find("AingSprite").gameObject.SetActive(false);
                }
                IsTuoGuan = isAi;
            }
        }
    }
    /// <summary>
    /// 设置托管
    /// </summary>
    private void SetAi()
    {
        if (AiButton.transform.Find("AingSprite").gameObject.activeSelf)
        {
            SendTuoGuan(false);
            AiButton.transform.Find("AingSprite").gameObject.SetActive(false);
        }
        else
        {
            SendTuoGuan(true);
            AiButton.transform.Find("AingSprite").gameObject.SetActive(true);
        }
        SoundManager.Instance.PlaySound(UIPaths.BUTTONCLICK);
    }



    /// <summary>
    /// 发送托管
    /// </summary>
    private void SendTuoGuan(bool isAi)
    {
        ClientToServerMsg.Send(Opcodes.Client_PlayerTuoGuan, GameData.m_TableInfo.id, isAi);
    }
    /// <summary>
    /// 断线重连重置玩家的托管
    /// </summary>
    public void ResetPlayerAI()
    {
       // AiButton.gameObject.SetActive(true);
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            OnPlayerAi((int)GameData.m_TableInfo.id, GameData.m_PlayerInfoList[i].pos, GameData.m_PlayerInfoList[i].IsAi);
        }
    }
    #endregion
    /// <summary>
    /// 打开设置按钮
    /// </summary>
    private void OpenSettingPanel()
    {
        UIManager.Instance.ShowUiPanel(UIPaths.PanelSetting2, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.BUTTONCLICK);
    }

    /// <summary>
    /// 取消选中的牌
    /// </summary>
    /// <param name="go"></param>
    private void CancleSelectCard(GameObject go)
    {
        for (int i = 0; i < CurChooseCardsObjList.Count; i++)
        {
            CurChooseCardsObjList[i].transform.localPosition = new Vector3(CurChooseCardsObjList[i].transform.localPosition.x, 0, CurChooseCardsObjList[i].transform.localPosition.z);
        }
        NoticeClickCount = -1;
        CurChooseCardsObjList = new List<GameObject>();
        TipSeperateBoomObj.SetActive(false);
        SoundManager.Instance.PlaySound(UIPaths.BUTTONCLICK);
    }

    /// <summary>
    /// 朋友出了朋友卡
    /// </summary>
    public void OnShowFriendCard(int pos, uint CardValue)
    {
        if (pos != 0)
        {
            for (int i = 1; i < IdAndPlayerDic.Count+1; i++)
            {
                //
                IdAndPlayerDic[pos].transform.Find("OtherPanel").Find("LandSprite").gameObject.SetActive(false);
                IdAndPlayerDic[pos].transform.Find("OtherPanel").Find("HelperSprite").gameObject.SetActive(false);
            }
            if (GameData.m_TableInfo.ZhuangPos == SelfPos || pos == SelfPos)
            {
                if (GameData.m_TableInfo.configPlayerIndex == 4)
                {
                    IdAndPlayerDic[GameData.m_TableInfo.ZhuangPos].transform.Find("OtherPanel").Find("HelperSprite")
                        .gameObject.SetActive(true);
                    IdAndPlayerDic[pos].transform.Find("OtherPanel").Find("HelperSprite").gameObject.SetActive(true);
                }
            }
            else
            {
                for (int i = 1; i < IdAndPlayerDic.Count + 1; i++)
                {
                    if (i == GameData.m_TableInfo.ZhuangPos || i == pos) continue;
                    else
                    {
                        if (GameData.m_TableInfo.configPlayerIndex == 4)
                            IdAndPlayerDic[i].transform.Find("OtherPanel").Find("HelperSprite").gameObject
                                .SetActive(true);
                    }
                }

            }

            // IdAndPlayerDic[pos].transform.FindChild("FriendCard").gameObject.SetActive(true);
           // IdAndPlayerDic[pos].transform.FindChild("FriendCard").GetComponent<Card>().SetValue(CardValue);
        }
        //  if(SelfPos==)
        IdAndPlayerDic[SelfPos].transform.Find("OtherPanel").Find("HelperSprite").gameObject.SetActive(false);

    }
    /// <summary>
    /// 朋友出了朋友卡
    /// </summary>
    public void OnShowBaoPaiInfo(int pos)
    {
        for (int i = 0; i < IdAndPlayerDic.Count; i++)
        {
            //LandSprite
            IdAndPlayerDic[i+1].transform.Find("OtherPanel").Find("HelperSprite").gameObject.SetActive(false);
            IdAndPlayerDic[i + 1].transform.Find("OtherPanel").Find("LandSprite").gameObject.SetActive(false);

           // IdAndPlayerDic[i + 1].transform.FindChild("OtherPanel").FindChild("HelperSprite").gameObject.SetActive(true);
        }
        if (pos != 0)
        {
            if (pos == SelfPos)//自己包牌 只看的建自己
            {
                if (GameData.m_TableInfo.configPlayerIndex == 4)
                {
                    IdAndPlayerDic[pos].transform.Find("OtherPanel").Find("HelperSprite").gameObject.SetActive(true);
                }
                //for (int i = 0; i < IdAndPlayerDic.Count; i++)
                //{
                //    if ((i + 1) != pos) continue;
                //    IdAndPlayerDic[i + 1].transform.FindChild("OtherPanel").FindChild("HelperSprite").gameObject.SetActive(false);
                //}
            }
            else//别人包牌
            {
                for (int i = 0; i < IdAndPlayerDic.Count; i++)
                {
                    if ((i + 1) == pos) continue;
                    if (GameData.m_TableInfo.configPlayerIndex == 4)
                    {
                        IdAndPlayerDic[i + 1].transform.Find("OtherPanel").Find("HelperSprite").gameObject
                            .SetActive(true);
                    }
                }
              //  IdAndPlayerDic[pos].transform.FindChild("OtherPanel").FindChild("HelperSprite").gameObject.SetActive(false);
            }
           // IdAndPlayerDic[pos].transform.FindChild("OtherPanel").FindChild("HelperSprite").gameObject.SetActive(false);
          //  IdAndPlayerDic[pos].transform.FindChild("OtherPanel").FindChild("LandSprite").gameObject.SetActive(false);
            // IdAndPlayerDic[pos].transform.FindChild("FriendCard").gameObject.SetActive(true);
            // IdAndPlayerDic[pos].transform.FindChild("FriendCard").GetComponent<Card>().SetValue(CardValue);
        }
        //  if(SelfPos==)


    }
    /// <summary>
    /// 获得朋友卡
    /// </summary>
    public void OnFriendCard(int pos, uint CardValue)
    {
        FriendCard.SetActive(true);
        FriendCard.transform.GetComponent<Card>().SetValue(CardValue);
        if (pos != 0)
        {
            //  IdAndPlayerDic[pos].transform.FindChild("FriendCard").gameObject.SetActive(true);
            // IdAndPlayerDic[pos].transform.FindChild("FriendCard").GetComponent<Card>().SetValue(CardValue);
        }
        // IdAndPlayerDic[pos].transform.FindChild("HelperSprite").gameObject.SetActive(true);


    }
    /// <summary>
    /// 重连回来
    /// </summary>
    public void ReconnectServer()
    {
        PlayerInfo selfinfo = GameDataFunc.GetPlayerInfo(Player.Instance.guid);

       
        IdAndPlayerDic = new Dictionary<int, GameObject>();
        #region   设置位置

        if (GameData.m_TableInfo.configPlayerIndex == 4)//四人
        {
            for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
            {
                if (GameData.m_PlayerInfoList[i].guid == Player.Instance.guid)
                {
                    switch (GameData.m_PlayerInfoList[i].pos)
                    {
                        case 1:
                            IdAndPlayerDic[1] = PlayerList[0];
                            IdAndPlayerDic[2] = PlayerList[1];
                            IdAndPlayerDic[3] = PlayerList[2];
                            IdAndPlayerDic[4] = PlayerList[3];
                            SelfPos = 1;

                            break;
                        case 2:
                            IdAndPlayerDic[2] = PlayerList[0];
                            IdAndPlayerDic[3] = PlayerList[1];
                            IdAndPlayerDic[4] = PlayerList[2];
                            IdAndPlayerDic[1] = PlayerList[3];
                            SelfPos = 2;
                            break;
                        case 3:
                            IdAndPlayerDic[3] = PlayerList[0];
                            IdAndPlayerDic[4] = PlayerList[1];
                            IdAndPlayerDic[1] = PlayerList[2];
                            IdAndPlayerDic[2] = PlayerList[3];
                            SelfPos = 3;
                            break;
                        case 4:
                            IdAndPlayerDic[4] = PlayerList[0];
                            IdAndPlayerDic[1] = PlayerList[1];
                            IdAndPlayerDic[2] = PlayerList[2];
                            IdAndPlayerDic[3] = PlayerList[3];
                            SelfPos = 4;
                            break;
                    }
                }
            }
        }
        else if (GameData.m_TableInfo.configPlayerIndex == 2)//两个人
        {
            for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
            {
                if (GameData.m_PlayerInfoList[i].guid == Player.Instance.guid)
                {
                    switch (GameData.m_PlayerInfoList[i].pos)
                    {
                        case 1:
                            IdAndPlayerDic[1] = PlayerList[0];
                            IdAndPlayerDic[2] = PlayerList[2];
                            IdAndPlayerDic[3] = PlayerList[1];
                            IdAndPlayerDic[4] = PlayerList[3];
                            SelfPos = 1;

                            break;
                        case 2:
                            IdAndPlayerDic[2] = PlayerList[0];
                            IdAndPlayerDic[3] = PlayerList[1];
                            IdAndPlayerDic[4] = PlayerList[3];
                            IdAndPlayerDic[1] = PlayerList[2];
                            SelfPos = 2;
                            break;
                        case 3:
                            IdAndPlayerDic[3] = PlayerList[0];
                            IdAndPlayerDic[4] = PlayerList[1];
                            IdAndPlayerDic[1] = PlayerList[2];
                            IdAndPlayerDic[2] = PlayerList[3];
                            SelfPos = 3;
                            break;
                        case 4:
                            IdAndPlayerDic[4] = PlayerList[0];
                            IdAndPlayerDic[1] = PlayerList[1];
                            IdAndPlayerDic[2] = PlayerList[2];
                            IdAndPlayerDic[3] = PlayerList[3];
                            SelfPos = 4;
                            break;
                    }
                }
            }
        }
        //switch (selfinfo.pos)
        //{
        //    case 1:
        //        IdAndPlayerDic[1] = PlayerList[0];
        //        IdAndPlayerDic[2] = PlayerList[1];
        //        IdAndPlayerDic[3] = PlayerList[2];
        //        IdAndPlayerDic[4] = PlayerList[3];
        //        SelfPos = 1;
        //        break;
        //    case 2:
        //        IdAndPlayerDic[2] = PlayerList[0];
        //        IdAndPlayerDic[3] = PlayerList[1];
        //        IdAndPlayerDic[4] = PlayerList[2];
        //        IdAndPlayerDic[1] = PlayerList[3];
        //        SelfPos = 2;
        //        break;
        //    case 3:
        //        IdAndPlayerDic[3] = PlayerList[0];
        //        IdAndPlayerDic[4] = PlayerList[1];
        //        IdAndPlayerDic[1] = PlayerList[2];
        //        IdAndPlayerDic[2] = PlayerList[3];
        //        SelfPos = 3;
        //        break;
        //    case 4:
        //        IdAndPlayerDic[4] = PlayerList[0];
        //        IdAndPlayerDic[1] = PlayerList[1];
        //        IdAndPlayerDic[2] = PlayerList[2];
        //        IdAndPlayerDic[3] = PlayerList[3];
        //        SelfPos = 4;
        //        break;
        //}

        #endregion



        //  onZhuangPosition();//庄的位置

      

      //  ResetPlayerAI();//重置托管


        if (GameData.m_TableInfo.roomState == RoomStatusType.Active)
        {
            InviteFriend.gameObject.SetActive(false);//DisposRoomBtn
            DisposRoomBtn.gameObject.SetActive(false);//DisposRoomBtn
            QuiteRoomBtn.gameObject.SetActive(false);
            ReadyBtn.gameObject.SetActive(false);
        }
        else if (GameData.m_TableInfo.roomState == RoomStatusType.Play)
        {
            foreach (var item in IdAndPlayerDic)
            {
                if (item.Key == GameData.m_TableInfo.ZhuangPos)//显示地主图标
                {
                    item.Value.transform.Find("OtherPanel").Find("LandSprite").gameObject.SetActive(true);
                }
            }
            // onPlayerHoldCards();//生成手牌
            ReconnectCreatHoldCard();

            LargestPos = GameData.m_TableInfo.lastOutCardPos;
            for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
            {
                if (GameData.m_PlayerInfoList[i].pos == LargestPos)
                {
                    LargestCard = GameData.m_PlayerInfoList[i].outCardList;
                  
                }

                SetTotalJifen(GameData.m_PlayerInfoList[i].pos, GameData.m_PlayerInfoList[i].score);//重置总积分
                SetTaoShangFen(GameData.m_PlayerInfoList[i].pos, (int)GameData.m_PlayerInfoList[i].TSTaoShangScore);//重置讨赏分
              //  ShowLeftCardNum(GameData.m_PlayerInfoList[i].pos, GameData.m_PlayerInfoList[i].LeftCardNum);//重置剩余多少张手牌
              //  IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos]
            }


            ResetPlayedCard();//生成打出的牌

            if (LargestPos == 0 || GameData.m_TableInfo.lastOutCardPos == GameData.m_TableInfo.waitOutCardPos)
            {
                ShowOperatePanle(GameData.m_TableInfo.waitOutCardPos, PuKeOperateType.ChuPai);
            }
            else
            {
                ShowOperatePanle(GameData.m_TableInfo.waitOutCardPos, PuKeOperateType.JiePai);
            }
          

            InviteFriend.gameObject.SetActive(false);
            DisposRoomBtn.gameObject.SetActive(false);//DisposRoomBtn
            QuiteRoomBtn.gameObject.SetActive(false);
            ReadyBtn.gameObject.SetActive(false);
            OnFriendCard(GameData.m_TableInfo.ZhuangPos, GameData.m_TableInfo.FriendCard);
            OnShowFriendCard(GameData.m_TableInfo.FriendPos, GameData.m_TableInfo.FriendCard);


            //重置是否有解散房间信息
            if (GameData.m_TableInfo.isQueryLeaveRoom)
            {
                UIManager.Instance.ShowUiPanel(UIPaths.PanelDestoryRoom, OpenPanelType.MinToMax);
            }


            OnSetFinishPlayerIndex();//出现上中下标志
            onResetPlayerOnForce();//断线的重置


            if (GameData.sendroominfo.PlayStatusInfo.BaoPaiPosition == (uint)PositionType.None)//没有包牌
            {
                for (int i = 0; i < IdAndPlayerDic.Count; i++)
                {
                    //LandSprite
                    IdAndPlayerDic[i + 1].transform.Find("OtherPanel").Find("HelperSprite").gameObject.SetActive(false);
                    IdAndPlayerDic[i + 1].transform.Find("OtherPanel").Find("LandSprite").gameObject.SetActive(false);

                  //  IdAndPlayerDic[i + 1].transform.FindChild("OtherPanel").FindChild("HelperSprite").gameObject.SetActive(true);
                }
                if (GameData.m_TableInfo.ZhuangPos == SelfPos || GameData.m_TableInfo.FriendPos == SelfPos)
                {
                    if (GameData.m_TableInfo.configPlayerIndex == 4)
                    {
                        IdAndPlayerDic[GameData.m_TableInfo.ZhuangPos].transform.Find("OtherPanel").Find("HelperSprite")
                            .gameObject.SetActive(true);
                        IdAndPlayerDic[GameData.m_TableInfo.FriendPos].transform.Find("OtherPanel").Find("HelperSprite")
                            .gameObject.SetActive(true);
                    }
                }
                else
                {
                    for (int i = 1; i < IdAndPlayerDic.Count+1; i++)
                    {
                        if (GameData.m_TableInfo.configPlayerIndex == 4)
                        {
                            if (GameData.m_TableInfo.ZhuangPos == i || GameData.m_TableInfo.FriendPos == i) continue;
                            //LandSprite
                            IdAndPlayerDic[i].transform.Find("OtherPanel").Find("HelperSprite").gameObject
                                .SetActive(true);
                        }
                        // IdAndPlayerDic[i].transform.FindChild("OtherPanel").FindChild("LandSprite").gameObject.SetActive(false);

                        //  IdAndPlayerDic[i + 1].transform.FindChild("OtherPanel").FindChild("HelperSprite").gameObject.SetActive(true);
                    }
                }
                IdAndPlayerDic[SelfPos].transform.Find("OtherPanel").Find("HelperSprite").gameObject.SetActive(false);
                // IdAndPlayerDic[(int)GameData.sendroominfo.PlayStatusInfo.FriendPosition].transform.FindChild("OtherPanel").FindChild("HelperSprite").gameObject.SetActive(true);
                // IdAndPlayerDic[(int)GameData.sendroominfo.PlayStatusInfo.AnotherFriendPosition].transform.FindChild("OtherPanel").FindChild("HelperSprite").gameObject.SetActive(true);
            }
            else//包牌
            {
                //if(GameData.sendroominfo.PlayStatusInfo.BaoPaiPosition!=SelfPos)
                //    for (int i = 1; i < IdAndPlayerDic.Count+1; i++)
                //    {

                //    }
                OnShowBaoPaiInfo((int)GameData.sendroominfo.PlayStatusInfo.BaoPaiPosition);
                IdAndPlayerDic[SelfPos].transform.Find("OtherPanel").Find("HelperSprite").gameObject.SetActive(false);
            }

        }
        else if (GameData.m_TableInfo.roomState == RoomStatusType.Over)//在结算时期
        {
            InviteFriend.gameObject.SetActive(false);
            DisposRoomBtn.gameObject.SetActive(false);//DisposRoomBtn
            QuiteRoomBtn.gameObject.SetActive(false);
            ReadyBtn.gameObject.SetActive(false);
            onPartGameOver();

            //重置是否有解散房间信息
            if (GameData.m_TableInfo.isQueryLeaveRoom)
            {
                UIManager.Instance.ShowUiPanel(UIPaths.PanelDestoryRoom, OpenPanelType.MinToMax);
            }
        }
        else if (GameData.m_TableInfo.roomState == RoomStatusType.Dispose)
        {
            InviteFriend.gameObject.SetActive(false);
            DisposRoomBtn.gameObject.SetActive(false);//DisposRoomBtn
            QuiteRoomBtn.gameObject.SetActive(false);
            ReadyBtn.gameObject.SetActive(false);
        }
        else if (GameData.m_TableInfo.roomState == RoomStatusType.None)
        {
            if (selfinfo.isNextReady || selfinfo.isStartReady)
            {
                ReadyBtn.gameObject.SetActive(false);
            }
        }



    }


    /// <summary>
    /// 玩家断线标志
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="isForce"></param>
    public void onResetPlayerOnForce()
    {
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            onPlayerOnForce(GameData.m_PlayerInfoList[i].pos, GameData.m_PlayerInfoList[i].isForce);
        }
       
    }
    /// <summary>
    /// 重连生成打出的牌
    /// </summary>
    public void ResetPlayedCard()
    {
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            if (GameData.m_PlayerInfoList[i].outCardList.Count != 0)
            {
                CreatPlayedCard(GameData.m_PlayerInfoList[i].pos, GameData.m_PlayerInfoList[i].outCardList);
            }
            else
            {
                if (GameData.m_PlayerInfoList[i].OperateType == CardOperateType.GuoPai)
                {
                    IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("DontPlaySprite").gameObject.SetActive(true);
                }
                else//没轮到出牌
                {

                }

            }
        }
    }
    /// <summary>
    /// 游戏结束音效
    /// </summary>
    public void SetGameOverSound()
    {
        for (int i = 0; i < PartGameOverControl.instance.SettleInfoList.Count; i++)
        {
            if (PartGameOverControl.instance.SettleInfoList[i].pos == SelfPos)
            {
                if (PartGameOverControl.instance.SettleInfoList[i].IsWin)
                {
                    SoundManager.Instance.PlaySound(UIPaths.GAMEWIN);
                }
                else
                {
                    SoundManager.Instance.PlaySound(UIPaths.GAMELOSE);
                }
            }
        }
    }

    public void SetButtonClickSound()
    {
        SoundManager.Instance.PlaySound(UIPaths.BUTTONCLICK);
    }

    /// <summary>
    /// 一局结束
    /// </summary>
    public void onPartGameOver()
    {
        Debug.Log("--------------------onPartGameOver");
        SetGameOverSound();

        if (GameData.m_TableInfo.configPlayerIndex == 4)
        {
            for (int i = 1; i < 5; i++)
            {
                SetTaoShangFen(i, 0);//设置讨赏分
                HideOperatePanle(i);
                ClearPlayedCard(i);
                ShowOrHideDontPlayCard(i, false);//隐藏不出

                SetTotalJifen(PartGameOverControl.instance.SettleInfoList[i - 1].pos, PartGameOverControl.instance.SettleInfoList[i - 1].Score);//显示积分
            }

        }
        else if (GameData.m_TableInfo.configPlayerIndex == 2)
        {
            for (int i = 1; i <3; i++)
            {
                SetTaoShangFen(i, 0);//设置讨赏分
                HideOperatePanle(i);
                ClearPlayedCard(i);
                ShowOrHideDontPlayCard(i, false);//隐藏不出

                SetTotalJifen(PartGameOverControl.instance.SettleInfoList[i - 1].pos, PartGameOverControl.instance.SettleInfoList[i - 1].Score);//显示积分
            }
        }

        CurChooseCardsObjList = new List<GameObject>();
        PosAndPlayedCard = new Dictionary<int, List<GameObject>>();
        CanPressCardList = new List<List<uint>>();
        LargestCard = new List<uint>();
        LargestPos = 0;
        ClearHoldCard();//清除玩家手牌

        ResetLeftCardNum();
        HideZhuangAndHelper();
     
        FriendCard.SetActive(false);
        ResetFinishPlayerIndex();

        UIManager.Instance.HideUiPanel(UIPaths.PanelDestoryRoom);
        UIManager.Instance.ShowUiPanel(UIPaths.PanelShowCard);


        ResetCardSortType();//重置排序
      //  StartCoroutine(GameOverTimeDely());
    }

    IEnumerator GameOverTimeDely()
    {
        yield return new WaitForSeconds(2f);
        ResetLeftCardNum();
        HideZhuangAndHelper();
        ClearHoldCard();//清除玩家手牌

        for (int i = 1; i < 5; i++)
        {
            SetTaoShangFen(i, 0);//设置讨赏分
            HideOperatePanle(i);
            ClearPlayedCard(i);
        }
        UIManager.Instance.ShowUiPanel(UIPaths.PanelGameOverSmall);
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void GameReset()
    {
        for (int i = 1; i < 5; i++)
        {
            HideOperatePanle(i);
            ClearPlayedCard(i);
            ShowOrHideDontPlayCard(i, false);//隐藏不出

            //  SetTotalJifen(PartGameOverControl.instance.SettleInfoList[i - 1].pos, PartGameOverControl.instance.SettleInfoList[i - 1].Score);//显示积分
        }

        CurChooseCardsObjList = new List<GameObject>();
        PosAndPlayedCard = new Dictionary<int, List<GameObject>>();
        CanPressCardList = new List<List<uint>>();
        LargestCard = new List<uint>();
        ClearHoldCard();//清除玩家手牌

        HideZhuangAndHelper();
        //ReadyBtn.gameObject.SetActive(true);
      //  AiButton.gameObject.SetActive(false);
        FriendCard.SetActive(false);

    }

    /// <summary>
    /// 隐藏庄和帮手 和朋友牌
    /// </summary>
    public void HideZhuangAndHelper()
    {
        foreach (var item in IdAndPlayerDic)
        {
            item.Value.transform.Find("FriendCard").gameObject.SetActive(false);
            item.Value.transform.Find("OtherPanel").Find("LandSprite").gameObject.SetActive(false);
            item.Value.transform.Find("OtherPanel").Find("HelperSprite").gameObject.SetActive(false);
        }
        //IdAndPlayerDic[pos].transform.FindChild("FriendCard").gameObject.SetActive(true);
    }
    /// <summary>
    /// 退出房间
    /// </summary>
    private void DisMissRoom()
    {
        SoundManager.Instance.PlaySound(UIPaths.BUTTONCLICK);
        ClientToServerMsg.Send(Opcodes.Client_PlayerLeaveRoom, GameData.m_TableInfo.id, true);
    }

    /// <summary>
    /// 玩家出牌
    /// </summary>
    private void PlayCard()
    {
        if (!IsTuoGuan)
        {
            NoticeClickCount = -1;
            ChuPai();
            SoundManager.Instance.PlaySound(UIPaths.SOUND_CHUPAI);
        }

    }

    /// <summary>
    /// 提示出牌
    /// </summary>
    bool IsSeperateBoom = false;//是否踩炸弹
    public int NoticeClickCount = -1;
    private void NoticePlayCard()
    {
        if (!IsTuoGuan)
        {

            if (LargestCard.Count == 0)
            {
                return;
            }
            NoticeClickCount++;
            CanPressCardList = new List<List<uint>>();
            List<uint> uintCard = new List<uint>();
            for (int i = 0; i < LocalCardsObjList.Count; i++)
            {
                uintCard.Add(uint.Parse(LocalCardsObjList[i].transform.GetComponent<UISprite>().spriteName));
            }
            if (LargestCard.Count != 0)
            {
                // CanPressCardList = myQiPaiHelper.Instance.getFitCards(LargestCard, uintCard, ref IsSeperateBoom);

                CanPressCardList = myQiPaiHelper.Instance.getFitCards(LargestCard, uintCard, GameData.m_TableInfo.isBawang);
                SetNoticeCardShow();
            }
            else
            {
                Debug.LogError("当前没有最大牌");
            }
        }

    }

    /// <summary>
    /// 提示的牌显示
    /// </summary>
    public void SetNoticeCardShow()
    {
        for (int i = 0; i < CurChooseCardsObjList.Count; i++)
        {
            CurChooseCardsObjList[i].transform.localPosition = new Vector3(CurChooseCardsObjList[i].transform.localPosition.x, 0, 0);
        }
        CurChooseCardsObjList = new List<GameObject>();

        if (CanPressCardList.Count == 0)//没有大过的牌
        {
            DontPlayCard();
            NoticeClickCount = -1;
            return;

        }

        if (NoticeClickCount == CanPressCardList.Count)//第二轮提示
        {
            NoticeClickCount = 0;
        }


        for (int j = 0; j < CanPressCardList[NoticeClickCount].Count; j++)
        {
            for (int i = 0; i < LocalCardsObjList.Count; i++)
            {
                if (uint.Parse(LocalCardsObjList[i].transform.GetComponent<UISprite>().spriteName) == CanPressCardList[NoticeClickCount][j])
                {
                    if (!CurChooseCardsObjList.Contains(LocalCardsObjList[i]))
                    {
                        CurChooseCardsObjList.Add(LocalCardsObjList[i]);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                //    if (CanPressCardList[ClickIndex].Contains(uint.Parse(LocalCardsObjList[i].transform.GetComponent<UISprite>().spriteName)))
                //{
                //    CurChooseCardsObjList.Add(LocalCardsObjList[i]);
                //}
            }
        }


        for (int i = 0; i < CurChooseCardsObjList.Count; i++)
        {
            CurChooseCardsObjList[i].transform.localPosition = new Vector3(CurChooseCardsObjList[i].transform.localPosition.x, 20, 0);
        }
    }
    /// <summary>
    /// 玩家不出
    /// </summary>
    private void DontPlayCard()
    {
        if (!IsTuoGuan)
        {
            NoticeClickCount = -1;
            List<uint> cards = new List<uint>();
            //  ClientToServerMsg.Send(Opcodes.Client_PlayerOperate, GameData.m_TableInfo.id, (byte)CardOperateType.GuoPai,0, cards.ToArray());
            ClientToServerMsg.SendTaoShangGuo(cards);
        }

    }

    #region  toserver
    /// <summary>
    /// 发送准备
    /// </summary>
    private void ReadyForGame()
    {

        ClientToServerMsg.Send(Opcodes.Client_PlayerReadyStart, GameData.m_TableInfo.id);//发送准备协议
        ReadyBtn.gameObject.SetActive(false);
        SoundManager.Instance.PlaySound(UIPaths.BUTTONCLICK);
    }
    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="isChuPai"></param>
    void ChuPai(bool isChuPai = true)
    {
        List<uint> cards = new List<uint>();
        if (isChuPai)
        {
            if (CurChooseCardsObjList.Count == 0)
            {
                GlobalModule.Instance.OnOpenBubblingHint("请选择手牌");
                return;
            }
            // myQiPaiHelper
            for (int i = 0; i < CurChooseCardsObjList.Count; i++)
            {
                // if (LocalCardsObjList[i].transform.localPosition.y == 20)
                // {
                cards.Add(uint.Parse(CurChooseCardsObjList[i].transform.GetComponent<UISprite>().spriteName));
                // }
            }
        }

        #region 生成手牌数据
        List<uint> handcard = new List<uint>();
        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {
            handcard.Add(uint.Parse(LocalCardsObjList[i].transform.GetComponent<UISprite>().spriteName));
        }

        #endregion


        object[] pms = new object[3 + cards.Count];
        pms[0] = GameData.m_TableInfo.id;
        pms[1] = (byte)CardOperateType.ChuPai;
        pms[2] = cards.Count;
        for (int i = 0; i < cards.Count; i++) pms[i + 3] = cards[i];
        ClientToServerMsg.Send(Opcodes.Client_PlayerOperate, pms);
        return;

        if (!CardTools.IsSeperateBoom(cards, handcard))//有没有拆砸蛋
        {

            ClientToServerMsg.Send(Opcodes.Client_PlayerOperate, pms);
            //  ClientToServerMsg.Send(Opcodes.Client_PlayerOperate, GameData.m_TableInfo.id, (byte)CardOperateType.ChuPai, cards.Count, cards.ToArray().GetValue);

        }
        else
        {
            TipSeperateBoomObj.SetActive(true);
           pams = pms;
        }

       
        // ClientToServerMsg.SendChuPai(GameData.m_DDZTableInfo.id, cards);
    }


   public  object[] pams;
    /// <summary>
    /// 提示拆炸弹出牌
    /// </summary>
    public void TipBoomPlayCard()
    {
        TipSeperateBoomObj.SetActive(false);
        ClientToServerMsg.Send(Opcodes.Client_PlayerOperate, pams);
    }
    /// <summary>
    /// 取消拆炸弹出牌
    /// </summary>
    public void TipBoomCanclePlayCard()
    {
        TipSeperateBoomObj.SetActive(false);
        pams = null;
        for (int i = 0; i < CurChooseCardsObjList.Count; i++)
        {
            CurChooseCardsObjList[i].transform.localPosition = new Vector3(CurChooseCardsObjList[i].transform.localPosition.x, 0, CurChooseCardsObjList[i].transform.localPosition.z);
        }
        NoticeClickCount = -1;
        CurChooseCardsObjList = new List<GameObject>();
    }
    #endregion




    // Update is called once per frame
    void Update()
    {
        if (MoveEnd)
        {
            MoveEnd = false;
            CardSort();

        }
    }

    /// <summary>
    /// 设置房间信息
    /// </summary>
    private void SetRoomInfo()
    {

        RoomIdLable.text = "房间号:" + GameData.m_TableInfo.id.ToString();
        switch (GameData.m_TableInfo.configPayIndex)
        {
            case 0:
                RuleLable.text = "局数：" + GameData.m_TableInfo.curGameCount.ToString() + "/" + GameData.m_TableInfo.configRoundIndex.ToString() + "";
                break;
            case 1:
                RuleLable.text = "局数：" + GameData.m_TableInfo.curGameCount.ToString() + "/" + GameData.m_TableInfo.configRoundIndex.ToString() + "";
                break;
        }
        currentCount = (int)GameData.m_TableInfo.curGameCount;
        string showStr = "";
        if (GameData.sendroominfo.IsBaWang) showStr += "  八王  ";
        if (GameData.sendroominfo.JiangMa.Count > 0)
        {
            for (int i = 0; i < GameData.sendroominfo.JiangMa.Count; i++)
            {
                if (GameData.sendroominfo.JiangMa[i] == 15) continue;
                switch (GameData.sendroominfo.JiangMa[i])
                {
                    case 7:
                        showStr += " " + GameData.sendroominfo.JiangMa[i].ToString();
                        break;
                    case 11:
                        showStr += " J" ;
                        break;
                    case 13:
                        showStr += " K" ;
                        break;
                    case 14:
                        showStr += " A" ;
                        break;
                }
              
            }
            showStr += "开奖";
        }
        if (GameData.sendroominfo.FaWangTp) showStr +="  罚王摊牌";

        if (GameData.sendroominfo.WuZhaTp) showStr += "  无炸摊牌";

       

        DetailRuleLable.text = showStr;
    }

    /// <summary>
    /// 设置玩家的各个位置
    /// </summary>
    private void SetPlayerListPos()
    {

        if (GameData.m_TableInfo.configPlayerIndex == 4)//四人
        {
            for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
            {
                if (GameData.m_PlayerInfoList[i].guid == Player.Instance.guid)
                {
                    switch (GameData.m_PlayerInfoList[i].pos)
                    {
                        case 1:
                            IdAndPlayerDic[1] = PlayerList[0];
                            IdAndPlayerDic[2] = PlayerList[1];
                            IdAndPlayerDic[3] = PlayerList[2];
                            IdAndPlayerDic[4] = PlayerList[3];
                            SelfPos = 1;

                            break;
                        case 2:
                            IdAndPlayerDic[2] = PlayerList[0];
                            IdAndPlayerDic[3] = PlayerList[1];
                            IdAndPlayerDic[4] = PlayerList[2];
                            IdAndPlayerDic[1] = PlayerList[3];
                            SelfPos = 2;
                            break;
                        case 3:
                            IdAndPlayerDic[3] = PlayerList[0];
                            IdAndPlayerDic[4] = PlayerList[1];
                            IdAndPlayerDic[1] = PlayerList[2];
                            IdAndPlayerDic[2] = PlayerList[3];
                            SelfPos = 3;
                            break;
                        case 4:
                            IdAndPlayerDic[4] = PlayerList[0];
                            IdAndPlayerDic[1] = PlayerList[1];
                            IdAndPlayerDic[2] = PlayerList[2];
                            IdAndPlayerDic[3] = PlayerList[3];
                            SelfPos = 4;
                            break;
                    }
                }
            }
        }
        else if (GameData.m_TableInfo.configPlayerIndex == 2)//两个人
        {
            for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
            {
                if (GameData.m_PlayerInfoList[i].guid == Player.Instance.guid)
                {
                    switch (GameData.m_PlayerInfoList[i].pos)
                    {
                        case 1:
                            IdAndPlayerDic[1] = PlayerList[0];
                            IdAndPlayerDic[2] = PlayerList[2];
                            IdAndPlayerDic[3] = PlayerList[1];
                            IdAndPlayerDic[4] = PlayerList[3];
                            SelfPos = 1;

                            break;
                        case 2:
                            IdAndPlayerDic[2] = PlayerList[0];
                            IdAndPlayerDic[3] = PlayerList[1];
                            IdAndPlayerDic[4] = PlayerList[3];
                            IdAndPlayerDic[1] = PlayerList[2];
                            SelfPos = 2;
                            break;
                        case 3:
                            IdAndPlayerDic[3] = PlayerList[0];
                            IdAndPlayerDic[4] = PlayerList[1];
                            IdAndPlayerDic[1] = PlayerList[2];
                            IdAndPlayerDic[2] = PlayerList[3];
                            SelfPos = 3;
                            break;
                        case 4:
                            IdAndPlayerDic[4] = PlayerList[0];
                            IdAndPlayerDic[1] = PlayerList[1];
                            IdAndPlayerDic[2] = PlayerList[2];
                            IdAndPlayerDic[3] = PlayerList[3];
                            SelfPos = 4;
                            break;
                    }
                }
            }
        }
      

        #region
        //switch (GameData.m_PlayerInfoList.Count)//设置玩家的显示
        //{
        //    case 1:
        //        IdAndPlayerDic[1] = PlayerList[0];
        //        IdAndPlayerDic[2] = PlayerList[1];
        //        IdAndPlayerDic[3] = PlayerList[2];
        //        IdAndPlayerDic[4] = PlayerList[3];
        //        SelfPos = 1;

        //        break;
        //    case 2:
        //        IdAndPlayerDic[2] = PlayerList[0];
        //        IdAndPlayerDic[3] = PlayerList[1];
        //        IdAndPlayerDic[4] = PlayerList[2];
        //        IdAndPlayerDic[1] = PlayerList[3];
        //        SelfPos = 2;
        //        break;
        //    case 3:
        //        IdAndPlayerDic[3] = PlayerList[0];
        //        IdAndPlayerDic[4] = PlayerList[1];
        //        IdAndPlayerDic[1] = PlayerList[2];
        //        IdAndPlayerDic[2] = PlayerList[3];
        //        SelfPos =3;
        //        break;
        //    case 4:
        //        IdAndPlayerDic[4] = PlayerList[0];
        //        IdAndPlayerDic[1] = PlayerList[1];
        //        IdAndPlayerDic[2] = PlayerList[2];
        //        IdAndPlayerDic[3] = PlayerList[3];
        //        SelfPos = 4;
        //        break;
        //}

        #endregion
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            //  Debug.LogError(GameData.m_PlayerInfoList[i].pos);


            IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].gameObject.SetActive(true);
            IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("NameLabel").GetComponent<UILabel>().text = GameDataFunc.GetPlayerInfo(GameData.m_PlayerInfoList[i].pos).name.ToString();
            IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("TaoShangSprite/NameLable").GetComponent<UILabel>().text = GameDataFunc.GetPlayerInfo(GameData.m_PlayerInfoList[i].pos).name.ToString();
            // IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.FindChild("JiFenLabel").GetComponent<UILabel>().text = "讨赏分：" + 0.ToString();// GameDataFunc.GetPlayerInfo(GameData.m_PlayerInfoList[i].pos).name.ToString();
            DownloadImage.Instance.Download(IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("HeadSprite").GetComponent<UITexture>(), GameData.m_PlayerInfoList[i].headID);

            SetTotalJifen(GameData.m_PlayerInfoList[i].pos, 0);//设置积分
            SetTaoShangFen(GameData.m_PlayerInfoList[i].pos, 0);//设置讨赏分

            if (GameData.m_TableInfo.curGameCount == 0)//第一局
            {
                if (GameData.m_PlayerInfoList[i].isStartReady)
                {
                    IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("OtherPanel").Find("ReadySprite").gameObject.SetActive(true);
                }
            }
            else
            {
                if (GameData.m_PlayerInfoList[i].isNextReady)
                {
                    IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("OtherPanel").Find("ReadySprite").gameObject.SetActive(true);
                }
            }

        }





    }

    /// <summary>
    /// 设置玩家积分
    /// </summary>
    /// <param name="pos"></param>
    public void SetTotalJifen(int pos, int Score)
    {
        IdAndPlayerDic[pos].transform.Find("JiFenLabel").GetComponent<UILabel>().text = Score.ToString();
    }

    /// <summary>
    /// 设置讨赏分
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="TaoShangFen"></param>
    public void SetTaoShangFen(int pos, int TaoShangFen)
    {
        IdAndPlayerDic[pos].transform.Find("TaoShangSprite").Find("Label").GetComponent<UILabel>().text = TaoShangFen.ToString();// GameDataFunc.GetPlayerInfo(GameData.m_PlayerInfoList[i].pos).name.ToString();
    }


    #region  handler中调用

    /// <summary>
    /// 玩家断线标志
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="isForce"></param>
    public void onPlayerOnForce(byte pos, bool isForce)
    {
        if (isForce)
        {
            IdAndPlayerDic[(int)pos].transform.Find("OtherPanel").Find("OffLineSprite").gameObject.SetActive(false);
        }
        else
        {
            IdAndPlayerDic[(int)pos].transform.Find("OtherPanel").Find("OffLineSprite").gameObject.SetActive(true);

        }
    }
    /// <summary>
    /// 玩家离开房间
    /// </summary>
    public void onPlayerLeave(int pos)
    {
        if (pos == SelfPos)
        {
            ManagerScene.Instance.LoadScene(SceneType.Main);
        }
        else
        {
            IdAndPlayerDic[pos].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 玩家进入
    /// </summary>
    /// <param name="info"></param>
    public void onPlayerEnter(PlayerInfo info)
    {
        IdAndPlayerDic[info.pos].gameObject.SetActive(true);
        IdAndPlayerDic[info.pos].transform.Find("NameLabel").GetComponent<UILabel>().text = GameDataFunc.GetPlayerInfo(info.pos).name.ToString();
        IdAndPlayerDic[info.pos].transform.Find("TaoShangSprite/NameLable").GetComponent<UILabel>().text = GameDataFunc.GetPlayerInfo(info.pos).name.ToString();

        DownloadImage.Instance.Download(IdAndPlayerDic[info.pos].transform.Find("HeadSprite").GetComponent<UITexture>(), info.headID);
        SetTotalJifen(info.pos, 0);//设置积分
        SetTaoShangFen(info.pos, 0);//设置讨赏分
    }

    /// <summary>
    /// 玩家准备
    /// </summary>
    /// <param name="pos"></param>
    public void onPlayerReadyForRoom(byte pos)
    {
        PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);
        if (info == null) return;
        info.isStartReady = true;

        IdAndPlayerDic[pos].transform.Find("OtherPanel").Find("ReadySprite").gameObject.SetActive(true);
        //PlayerObjList[(int)info.LVD].transform.Find("ready").gameObject.SetActive(info.isStartReady);
        //if (info.pos == LocalPos)
        //{
        //    btnReady.GetComponent<BoxCollider2D>().enabled = false;
        //    btnReady.GetComponent<UISprite>().spriteName = "UI_game_btn_ready_Pressed";
        //}
        SoundManager.Instance.PlaySound(UIPaths.SOUND_READY);
    }

    /// <summary>
    /// 游戏开局  房间激活
    /// </summary>
    public void onRoomActive()
    {

        InviteFriend.gameObject.SetActive(false);
        DisposRoomBtn.gameObject.SetActive(false);//DisposRoomBtn
        QuiteRoomBtn.gameObject.SetActive(false);
        ReadyBtn.gameObject.SetActive(false);
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("OtherPanel").Find("ReadySprite").gameObject.SetActive(false);
            // PlayerObjList[(int)GameData.m_PlayerInfoList[i].LVD].transform.Find("ready").gameObject.SetActive(false);
        }
        GameData.m_TableInfo.roomState = RoomStatusType.Active;
      
        SetRoomInfo();//更新房间信息
        #region
        //if (GameData.m_TableInfo.configPlayerIndex == 2)
        //{
        //    for (int i = 0; i < 2; i++)
        //    {
        //        int index = LocalPos + i;
        //        if (index > 2) index -= 2;
        //        int pindex = index;
        //        index = (int)GetLVD((byte)index);
        //        if (pindex == LocalPos)
        //        {
        //            TweenPosition.Begin(PlayerObjList[index], 0.2f, PlayerStartPos2[0]);
        //        }
        //        else
        //        {
        //            TweenPosition.Begin(PlayerObjList[index], 0.2f, PlayerStartPos2[1]);
        //        }
        //    }
        //}
        //else if (GameData.m_TableInfo.configPlayerIndex == 1)
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        int index = LocalPos + i;
        //        if (index > 3) index -= 3;
        //        int pindex = index;
        //        index = (int)GetLVD((byte)index);
        //        TweenPosition.Begin(PlayerObjList[index], 0.2f, PlayerStartPos4[index]);
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < PlayerObjList.Count; i++)
        //    {
        //        int index = LocalPos + i;
        //        if (index > 4) index -= 4;
        //        int pindex = index;
        //        index = (int)GetLVD((byte)index);
        //        TweenPosition.Begin(PlayerObjList[index], 0.2f, PlayerStartPos4[index]);
        //    }
        //}

        #endregion
    }


    /// <summary>
    /// 断线回来生成手牌
    /// </summary>
    public void ReconnectCreatHoldCard()
    {
        PlayerInfo info = GameDataFunc.GetPlayerInfo(Player.Instance.guid);
        CardDataList = new List<CardData>();
        CardList = info.localCardList;//还原手牌数据
        for (int i = 0; i < info.localCardList.Count; i++)
        {
            CardData carddata = new CardData();
            carddata.SetData(info.localCardList[i]);
            CardDataList.Add(carddata);//用于排序


            GameObject item = GameObject.Instantiate(CardObj, CardPoint.transform);
            item.transform.localPosition = new Vector3(0, 0, 0);
            item.transform.localScale = Vector3.one;
            item.transform.GetComponent<Card>().SetValue(info.localCardList[i]);
            item.SetActive(true);
            LocalCardsObjList.Add(item);//增加手牌
                                        // UIEventListener.Get(item).onPress = OnPressCard;
            UIEventListener.Get(item).onDragOver = OnDragOver;
            UIEventListener.Get(item).onClick = OnCardClick;
            UIEventListener.Get(item).onDragEnd = OnCardDragEnd;




        }

        CardSort();
        SortBtn.gameObject.SetActive(true);//显示排序按钮
    }
    /// <summary>
    /// 开始发牌
    /// </summary>
    int currentCount = 0;//当前第几句
    public void onPlayerHoldCards()
    {
        GameData.m_TableInfo.curGameCount++;
        RuleLable.text = "局数：" + GameData.m_TableInfo.curGameCount.ToString() + "/" + GameData.m_TableInfo.configRoundIndex.ToString() + "";

        InviteFriend.gameObject.SetActive(false);
        DisposRoomBtn.gameObject.SetActive(false);//DisposRoomBtn
        QuiteRoomBtn.gameObject.SetActive(false);
        ReadyBtn.gameObject.SetActive(false);
        EffectCount = 4;//重置有效玩家数
        
        PlayerInfo info = GameDataFunc.GetPlayerInfo(Player.Instance.guid);
        CardDataList = new List<CardData>();
        CardList = info.localCardList;//还原手牌数据
        for (int i = 0; i < info.localCardList.Count; i++)
        {
            CardData carddata = new CardData();
            carddata.SetData(info.localCardList[i]);
            CardDataList.Add(carddata);//用于排序


            GameObject item = GameObject.Instantiate(CardObj, CardPoint.transform);
            item.transform.localPosition = new Vector3(0, 0, 0);
            item.transform.localScale =new Vector3(1.3f,1.3f,0);
            item.transform.GetComponent<Card>().SetValue(info.localCardList[i]);
            item.SetActive(false);
            LocalCardsObjList.Add(item);//增加手牌
                                        // UIEventListener.Get(item).onPress = OnPressCard;
            UIEventListener.Get(item).onDragOver = OnDragOver;
            UIEventListener.Get(item).onClick = OnCardClick;
            UIEventListener.Get(item).onDragEnd = OnCardDragEnd;


        }

        // CardSort();
        SoundManager.Instance.PlaySound(UIPaths.GAMESTART);
        SoundManager.Instance.PlaySound(UIPaths.FAPAI);
        StartCoroutine(AsynDealCard());

    }

    public float StartCardPoint = -600f;//开始的位置
    public float ScreenWith = 1230f;//屏幕宽度>20

    public float StartCardPoint1 = -490f;//开始的位置
    public float ScreenWith1 = 980f;//屏幕宽度15 20

    public float StartCardPoint2 = -390f;//开始的位置
    public float ScreenWith2 = 780;//屏幕宽度10 15

    public float StartCardPoint3 = -200f;//开始的位置
    public float ScreenWith3 = 400;//屏幕宽度5 10

    public float StartCardPoint4 = -90f;//开始的位置
    public float ScreenWith4 = 180;//屏幕宽度2 5
    //发牌
    private IEnumerator AsynDealCard()
    {
        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {
            LocalCardsObjList[i].transform.localPosition = new Vector3(-590f + (ScreenWith / LocalCardsObjList.Count) * i, 0, 0);
            LocalCardsObjList[i].SetActive(true);
            LocalCardsObjList[i].transform.GetComponent<Card>().ScaleMove();
            yield return new WaitForSeconds(0.1f);
        }
        ResetPos();
        yield return new WaitForSeconds(0.3f);
        MoveEnd = true;



    }




  
    bool MoveEnd = false;
    /// <summary>
    /// 往中间走
    /// </summary>
    private void ResetPos()
    {
        Vector3 v = CardPoint.transform.position;
        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {

            AnimSystem.Instance.MoveTo(LocalCardsObjList[i], v);
        }

        if (getZhuangPos)//晚点出险打牌界面
        {
            getZhuangPos = false;
            OnZhuang();
        }
    }


    public enum SortType
    {
        ValueSort,//值排序
        NumSort//数量排序

    };
     SortType CardSortType = SortType.ValueSort;//牌排序的方式
    bool NumSort = false;//是否数量排序
    /// <summary>
    /// 设置排序
    /// </summary>
    public void SetCardSortType()
    {
        NumSort = !NumSort;
        if (NumSort)
        {
            CardSortType = SortType.NumSort;
        }
        else
        {
            CardSortType = SortType.ValueSort;
        }
        CardSort();
    }

    /// <summary>
    /// 重置排序
    /// </summary>
    public void ResetCardSortType()
    {
        NumSort = false;
        CardSortType = SortType.ValueSort;//牌排序的方式
        SortBtn.gameObject.SetActive(false);
    }
    /// <summary>
    /// 玩家手牌排序
    /// </summary>
   // public List<uint> SelfCardList = new List<uint>();//自己的手牌
    public List<CardData> CardDataList = new List<CardData>();//自己的手牌数据
    public void CardSort()
    {
        // LocalCardsObjList.Sort();
        // CardDataList.Sort();

        #region 
        if (CardSortType == SortType.ValueSort)
        {
            List<CardData> newlist = CardDataSort(CardDataList);
            for (int i = 0; i < newlist.Count; i++)
            {
                LocalCardsObjList[i].transform.GetComponent<Card>().SetValue(newlist[i].data, newlist[i].Index);
            }

        }
        else
        {
            List<CardData> a = CardDataNumSort(CardDataList);

            for (int i = 0; i < a.Count; i++)

            {
                LocalCardsObjList[i].transform.GetComponent<Card>().SetValue(a[i].data, a[i].Index);
            }
        }


        #region
        //    if (LocalCardsObjList.Count >= 20)//25张的排序
        //    {
        //        float startDistance = 32f * (LocalCardsObjList.Count / 2) + 64f;
        //        for (int i = 0; i < LocalCardsObjList.Count; i++)
        //        {
        //            //  Vector3 v = LocalCardsObjList[i].transform.position;
        //            //if (i == 0)
        //            //{
        //            //    LocalCardsObjList[i].transform.localPosition = new Vector3(-StartCardPoint - (ScreenWith / (float)LocalCardsObjList.Count) * i , 0, 0);
        //            //}
        //            //else
        //            //{
        //            //    LocalCardsObjList[i].transform.localPosition = new Vector3(-StartCardPoint - (ScreenWith / (float)LocalCardsObjList.Count) * i - 30, 0, 0);
        //            //}
        //            LocalCardsObjList[i].transform.localPosition = new Vector3(20-StartCardPoint - (ScreenWith / (float)LocalCardsObjList.Count) * i, 0, 0);
        //            // AnimSystem.Instance.MoveTo(LocalCardsObjList[i], new Vector3((startDistance - 32 * i) / 350f, CardPoint.transform.position.y, 0), iTween.EaseType.easeOutBack);
        //            // LocalCardsObjList[i].transform.localPosition = new Vector3((startDistance -40  * i)/320f, CardPoint.transform.position.y, 0);
        //            if (i != 0)
        //            {
        //                LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //            }
        //            else
        //            {
        //                LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //            }
        //        }
        //    }
        //   else if (LocalCardsObjList.Count < 20 && LocalCardsObjList.Count > 15)
        //    {

        //        for (int i = 0; i < LocalCardsObjList.Count; i++)
        //        {
        //            //  Vector3 v = LocalCardsObjList[i].transform.position;
        //            LocalCardsObjList[i].transform.localPosition = new Vector3(-StartCardPoint1 - (ScreenWith1 / LocalCardsObjList.Count) * i, 0, 0);
        //            // AnimSystem.Instance.MoveTo(LocalCardsObjList[i], new Vector3((startDistance - 32 * i) / 350f, CardPoint.transform.position.y, 0), iTween.EaseType.easeOutBack);
        //            // LocalCardsObjList[i].transform.localPosition = new Vector3((startDistance -40  * i)/320f, CardPoint.transform.position.y, 0);
        //            if (i != 0)
        //            {
        //                LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //            }
        //            else
        //            {
        //                LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //            }
        //        }
        //        #region  
        //        //if (LocalCardsObjList.Count % 2 == 1)//基数张
        //        //{
        //        //    for (int i = 0; i < LocalCardsObjList.Count; i++)
        //        //    {
        //        //        LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count - 1) / 2) * 50 - 50 * i, CardPoint.transform.position.y, 0);
        //        //      //  LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count - 1) / 2) * 35- 35 * i, CardPoint.transform.position.y, 0);
        //        //        if (i != 0)
        //        //        {
        //        //            LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //        //        }
        //        //        else
        //        //        {
        //        //            LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //        //        }
        //        //    }


        //        //}

        //        //else //偶数张
        //        //{
        //        //    for (int i = 0; i < LocalCardsObjList.Count; i++)
        //        //    {
        //        //        LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count ) / 2) * 50 - 50 * i - 50, CardPoint.transform.position.y, 0);
        //        //        if (i != 0)
        //        //        {
        //        //            LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //        //        }
        //        //        else
        //        //        {
        //        //            LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //        //        }
        //        //    }
        //        //}

        //        #endregion
        //    }

        //    else if (LocalCardsObjList.Count <= 15 && LocalCardsObjList.Count > 10)
        //    {
        //        for (int i = 0; i < LocalCardsObjList.Count; i++)
        //        {
        //            //  Vector3 v = LocalCardsObjList[i].transform.position;
        //            LocalCardsObjList[i].transform.localPosition = new Vector3(-StartCardPoint2 - (ScreenWith2 / LocalCardsObjList.Count) * i, 0, 0);
        //            // AnimSystem.Instance.MoveTo(LocalCardsObjList[i], new Vector3((startDistance - 32 * i) / 350f, CardPoint.transform.position.y, 0), iTween.EaseType.easeOutBack);
        //            // LocalCardsObjList[i].transform.localPosition = new Vector3((startDistance -40  * i)/320f, CardPoint.transform.position.y, 0);
        //            if (i != 0)
        //            {
        //                LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //            }
        //            else
        //            {
        //                LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //            }
        //        }
        //        #region
        //        //if (LocalCardsObjList.Count % 2 == 1)//基数张
        //        //{
        //        //    for (int i = 0; i < LocalCardsObjList.Count; i++)
        //        //    {
        //        //        LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count - 1) / 2) * 60 - 60 * i, CardPoint.transform.position.y, 0);
        //        //        if (i != 0)
        //        //        {
        //        //            LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //        //        }
        //        //        else
        //        //        {
        //        //            LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //        //        }
        //        //    }


        //        //}

        //        //else //偶数张
        //        //{
        //        //    for (int i = 0; i < LocalCardsObjList.Count; i++)
        //        //    {
        //        //        LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count - 1) / 2) * 60 - 60 * i -50, CardPoint.transform.position.y, 0);
        //        //        if (i != 0)
        //        //        {
        //        //            LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //        //        }
        //        //        else
        //        //        {
        //        //            LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //        //        }
        //        //    }
        //        //}
        //        #endregion
        //    }
        //    else  if (LocalCardsObjList.Count <= 10 && LocalCardsObjList.Count > 5)
        //        {
        //        for (int i = 0; i < LocalCardsObjList.Count; i++)
        //        {
        //            //  Vector3 v = LocalCardsObjList[i].transform.position;
        //            LocalCardsObjList[i].transform.localPosition = new Vector3(-StartCardPoint3 -(ScreenWith3 / LocalCardsObjList.Count) * i, 0, 0);
        //            // AnimSystem.Instance.MoveTo(LocalCardsObjList[i], new Vector3((startDistance - 32 * i) / 350f, CardPoint.transform.position.y, 0), iTween.EaseType.easeOutBack);
        //            // LocalCardsObjList[i].transform.localPosition = new Vector3((startDistance -40  * i)/320f, CardPoint.transform.position.y, 0);
        //            if (i != 0)
        //            {
        //                LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //            }
        //            else
        //            {
        //                LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //            }
        //        }
        //        #region
        //        /*
        //            if (LocalCardsObjList.Count % 2 == 1)//基数张
        //            {
        //                for (int i = 0; i < LocalCardsObjList.Count; i++)
        //                {
        //                    LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count - 1) / 2) * 65 - 65 * i, CardPoint.transform.position.y, 0);
        //                    if (i != 0)
        //                    {
        //                        LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //                    }
        //                    else
        //                    {
        //                        LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //                    }
        //                }


        //            }

        //            else //偶数张
        //            {
        //                for (int i = 0; i < LocalCardsObjList.Count; i++)
        //                {
        //                    LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count - 1) / 2) * 55 -50 * i +50, CardPoint.transform.position.y, 0);
        //                    if (i != 0)
        //                    {
        //                        LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //                    }
        //                    else
        //                    {
        //                        LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //                    }
        //                }
        //            }

        //*/

        //        #endregion
        //    }


        //    else  if (LocalCardsObjList.Count <= 5 && LocalCardsObjList.Count > 1)
        //        {
        //        for (int i = 0; i < LocalCardsObjList.Count; i++)
        //        {
        //            //  Vector3 v = LocalCardsObjList[i].transform.position;
        //            LocalCardsObjList[i].transform.localPosition = new Vector3(-StartCardPoint4 - (ScreenWith4 / LocalCardsObjList.Count) * i, 0, 0);
        //            // AnimSystem.Instance.MoveTo(LocalCardsObjList[i], new Vector3((startDistance - 32 * i) / 350f, CardPoint.transform.position.y, 0), iTween.EaseType.easeOutBack);
        //            // LocalCardsObjList[i].transform.localPosition = new Vector3((startDistance -40  * i)/320f, CardPoint.transform.position.y, 0);
        //            if (i != 0)
        //            {
        //                LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //            }
        //            else
        //            {
        //                LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //            }
        //        }
        //        #region
        //        /*
        //            if (LocalCardsObjList.Count % 2 == 1)//基数张
        //            {
        //                for (int i = 0; i < LocalCardsObjList.Count; i++)
        //                {
        //                    LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count - 1) / 2) * 60 - 60 * i, CardPoint.transform.position.y, 0);
        //                    if (i != 0)
        //                    {
        //                        LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //                    }
        //                    else
        //                    {
        //                        LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //                    }
        //                }


        //            }

        //            else //偶数张
        //            {
        //                for (int i = 0; i < LocalCardsObjList.Count; i++)
        //                {
        //                    LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count - 1) / 2) * 60 - 60 * i + 50, CardPoint.transform.position.y, 0);
        //                    if (i != 0)
        //                    {
        //                        LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
        //                    }
        //                    else
        //                    {
        //                        LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
        //                    }
        //                }
        //            }

        //*/

        //        #endregion
        //    }

        //    else if ( LocalCardsObjList.Count == 1)
        //    {
        //        LocalCardsObjList[0].transform.localPosition = Vector3.zero;
        //    }

        #endregion

        CardDistanceControl();
        SortBtn.gameObject.SetActive(true);//显示排序按钮


        #endregion

        // AiButton.gameObject.SetActive(true);//显示托管按钮(发完牌后显示)
        // SortBtn.gameObject.SetActive(true);//显示托管按钮(发完牌后显示)

    }

    private float StandardScreenWidth = 1180f;//屏幕宽度
    private float NewStartPoint = 590f;
    /// <summary>
    /// 牌间距的控制
    /// </summary>
    public void CardDistanceControl()
    {
        if (LocalCardsObjList.Count >= 20)//25张的排序
        {

            float newStartIndex = (ScreenWith / (float)LocalCardsObjList.Count);//牌间距
            for (int i = 0; i < LocalCardsObjList.Count; i++)
            {
                if (LocalCardsObjList.Count % 2 == 0)//偶数张
                {
                    LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count / 2) * newStartIndex - (newStartIndex / 2f)) - newStartIndex * i, 0, 0);
                }
                else
                {
                    LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count - 1) / 2) * newStartIndex - newStartIndex * i, 0, 0);
                }
                // LocalCardsObjList[i].transform.localPosition = new Vector3(NewStartPoint - (ScreenWith / ((float)LocalCardsObjList.Count - 1)) * i, 0, 0);

                if (i != 0)
                {
                    LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
                }
                else
                {
                    LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
                }
            }
            //for (int i = 0; i < LocalCardsObjList.Count; i++)
            //{

            //    LocalCardsObjList[i].transform.localPosition = new Vector3( NewStartPoint - (ScreenWith / ((float)LocalCardsObjList.Count-1f)) * i, 0, 0);

            //    if (i != 0)
            //    {
            //        LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
            //    }
            //    else
            //    {
            //        LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
            //    }
            //}
        }
        else
        {
            float newStartIndex = (ScreenWith / (19f));//牌间距
            for (int i = 0; i < LocalCardsObjList.Count; i++)
            {
                if (LocalCardsObjList.Count % 2 == 0)//偶数张
                {
                    LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count/2) * newStartIndex-(newStartIndex/2f)) - newStartIndex * i, 0, 0);
                }
                else
                {
                    LocalCardsObjList[i].transform.localPosition = new Vector3(((LocalCardsObjList.Count-1) / 2) * newStartIndex  - newStartIndex * i, 0, 0);
                }
               // LocalCardsObjList[i].transform.localPosition = new Vector3(NewStartPoint - (ScreenWith / ((float)LocalCardsObjList.Count - 1)) * i, 0, 0);

                if (i != 0)
                {
                    LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = LocalCardsObjList[i - 1].transform.GetComponent<UISprite>().depth - 1;
                }
                else
                {
                    LocalCardsObjList[i].transform.GetComponent<UISprite>().depth = 30;
                }
            }
        }
    }
    /// <summary>
    /// 牌数据的排序
    /// </summary>
    /// <returns></returns>
    public List<CardData> CardDataSort(List<CardData> cardlist)
    {

        for (int i = 0; i < cardlist.Count; i++)
        {
            uint min = cardlist[i].data % 100;
            int minindex = i;

            for (int j = i; j < cardlist.Count; j++)
            {
                if (cardlist[j].data % 100 < min)
                {
                    min = cardlist[j].data % 100;
                    minindex = j;
                }
            }

            if (minindex != i)
            {
                uint change = cardlist[i].data;


                cardlist[i].data = cardlist[minindex].data;
                cardlist[minindex].data = change;

            }
        }

        List<CardData> newListIndex = new List<CardData>();
        List<CardData> newList = new List<CardData>();
        //黑红梅方排序
        #region
        for (int i = 0; i < cardlist.Count; i++)
        {
            if (i == 0)
            {
                newList.Add(cardlist[i]);
            }
            else
            {
                if (cardlist[i].data % 100 == newList[0].data % 100)
                {
                    newList.Add(cardlist[i]);
                    if (i == cardlist.Count - 1)
                    {
                        if (newList.Count > 1)
                        {
                            newList.Sort((a, b) =>
                            {
                                return -((int)a.data - (int)b.data);
                            });
                        }

                        newListIndex.AddRange(newList);
                        newList = new List<CardData>();
                        newList.Add(cardlist[i]);
                    }

                }

               
                else
                {
                    if (newList.Count > 1)
                    {
                        newList.Sort((a, b) =>
                        {
                            return -((int)a.data - (int)b.data);
                        });
                    }
                   
                    newListIndex.AddRange(newList);
                    newList = new List<CardData>();
                    newList.Add(cardlist[i]);


                    if (i == cardlist.Count - 1)
                    {
                        if (newList.Count > 1)
                        {
                            newList.Sort((a, b) =>
                            {
                                return -((int)a.data - (int)b.data);
                            });
                        }

                        newListIndex.AddRange(newList);
                        newList = new List<CardData>();
                        newList.Add(cardlist[i]);
                    }
                }
            }
        }

        #endregion

        cardlist = newListIndex;
        for (int i = 0; i < cardlist.Count; i++)
        {
            cardlist[i].Index = i;
        }
        
        return cardlist;
    }

    /// <summary>
    /// 拍数据数量排序
    /// </summary>
    /// <param name="cardlist"></param>
    /// <returns></returns>
    public List<CardData> CardDataNumSort(List<CardData> cardlist)
    {
        List<uint> newlist = new List<uint>();
        for (int i = 0; i < cardlist.Count; i++)
        {
            newlist.Add(cardlist[i].data);
        }
        newlist = CardTools.CardNumSort(newlist);
        List<CardData> newCardDataList = new List<CardData>();
       // cardlist = new List<CardData>();
        //for (int i = 0; i < newlist.Count; i++)
        //{
        //    CardData data = new CardData();
        //    data.data = newlist[i];
        //    data.Index = i;
        //    newCardDataList.Add(data);
        //    cardlist[i] = data;
        //    //newCardDataList[i].data = newlist[i];
        //    //newCardDataList[i].Index = i;
        //}

        for (int i = newlist.Count-1; i >=0; i--)
        {
            CardData data = new CardData();
            data.data = newlist[i];
            data.Index = i;
            newCardDataList.Add(data);
            cardlist[i] = data;
            //newCardDataList[i].data = newlist[i];
            //newCardDataList[i].Index = i;
        }

        List<CardData> newListIndex = new List<CardData>();
        List<CardData> newList = new List<CardData>();
        //黑红梅方排序
        #region
        for (int i = 0; i < newCardDataList.Count; i++)
        {
            if (i == 0)
            {
                newList.Add(newCardDataList[i]);
            }
            else
            {
                if (newCardDataList[i].data % 100 == newList[0].data % 100)
                {
                    newList.Add(newCardDataList[i]);
                    if (i == newCardDataList.Count - 1)
                    {
                        if (newList.Count > 1)
                        {
                            newList.Sort((a, b) =>
                            {
                                return -((int)a.data - (int)b.data);
                            });
                        }

                        newListIndex.AddRange(newList);
                        newList = new List<CardData>();
                        newList.Add(newCardDataList[i]);
                    }

                }


                else
                {
                    if (newList.Count > 1)
                    {
                        newList.Sort((a, b) =>
                        {
                            return -((int)a.data - (int)b.data);
                        });
                    }

                    newListIndex.AddRange(newList);
                    newList = new List<CardData>();
                    newList.Add(newCardDataList[i]);


                    if (i == newCardDataList.Count - 1)
                    {
                        if (newList.Count > 1)
                        {
                            newList.Sort((a, b) =>
                            {
                                return -((int)a.data - (int)b.data);
                            });
                        }

                        newListIndex.AddRange(newList);
                        newList = new List<CardData>();
                        newList.Add(newCardDataList[i]);
                    }
                }
            }
        }

        #endregion
        newCardDataList = newListIndex;
        //for (int i = 0; i < newCardDataList.Count; i++)
        //{
        //    LocalCardsObjList[i].transform.GetComponent<Card>().SetValue(newCardDataList[i].data, newCardDataList[i].Index);
        //}
        return newCardDataList;
    }

    #region  牌的操作

    /// <summary>
    /// 牌的滑动
    /// </summary>
    /// <param name="go"></param>
    private void OnCardDragEnd(GameObject go)
    {
        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {
            LocalCardsObjList[i].GetComponent<UISprite>().color = Color.white;
        }
    }

    /// <summary>
    /// 牌的点击
    /// </summary>
    /// <param name="go"></param>
    private void OnCardClick(GameObject go)
    {
        if (go.transform.localPosition.y == 20)
        {
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, 0);
            if (CurChooseCardsObjList.Contains(go))
            {
                CurChooseCardsObjList.Remove(go);
            }
        }
        else
        {
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, 20);
            if (!CurChooseCardsObjList.Contains(go))
            {
                CurChooseCardsObjList.Add(go);
            }
        }
        // CurChooseCardsObjList.Add(go);
    }


    //牌的点击和拖动
    void OnPressCard(GameObject go, bool isPress)
    {
        if (isPress)
        {
            if (go.transform.localPosition.y == 20)
            {
                go.transform.localPosition = new Vector3(go.transform.localPosition.x, 0);
                if (CurChooseCardsObjList.Contains(go))
                {
                    CurChooseCardsObjList.Remove(go);
                }
            }
            else
            {
                go.transform.localPosition = new Vector3(go.transform.localPosition.x, 20);
                if (!CurChooseCardsObjList.Contains(go))
                {
                    CurChooseCardsObjList.Add(go);
                }
            }
        }


        //if (isPress)
        //{
        //  CurChooseCardsObjList.Clear();

        //}
        //else
        //{
        //    if (CurChooseCardsObjList.Count == 0)
        //    {
        //        if (go.transform.localPosition.y == 20) go.transform.localPosition = new Vector3(go.transform.localPosition.x, 0);
        //        else go.transform.localPosition = new Vector3(go.transform.localPosition.x, 20);
        //    }
        //    else
        //    {
        //        for (int i = 0; i < CurChooseCardsObjList.Count; i++)
        //        {
        //            if (CurChooseCardsObjList[i].transform.localPosition.y == 20) CurChooseCardsObjList[i].transform.localPosition = new Vector3(CurChooseCardsObjList[i].transform.localPosition.x, 0);
        //            else CurChooseCardsObjList[i].transform.localPosition = new Vector3(CurChooseCardsObjList[i].transform.localPosition.x, 20);
        //            CurChooseCardsObjList[i].GetComponent<UISprite>().color = Color.white;
        //        }
        //    }
        //}
    }
    void OnDragOver(GameObject go)
    {
        go.GetComponent<UISprite>().color = Color.gray;
        //  CurChooseCardsObjList.Add(go);

        if (go.transform.localPosition.y == 20)
        {
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, 0, 0);
            if (CurChooseCardsObjList.Contains(go))
            {
                CurChooseCardsObjList.Remove(go);
            }
        }
        else
        {
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, 20, 0);
            if (!CurChooseCardsObjList.Contains(go))
            {
                CurChooseCardsObjList.Add(go);
            }
        }
    }



    #endregion



    /// <summary>
    /// 玩家的操作
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="opType"></param>
    /// <param name="opCard"></param>
    /// <param name="outPos"></param>
    /// <param name="isAnGang"></param>

    public void onPlayerOperate(byte pos, CardOperateType opType, List<uint> cards, int leftCardCount, int TaoShangScore, int sex)
    {



        // HideOperateBtn();
        PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);
       // SetTaoShangFen(pos, TaoShangScore);//设置讨赏分
        NoticeClickCount = -1;//提示相关
        // IdAndPlayerDic[pos].transform.FindChild("TaoShangSprite").FindChild("Label").GetComponent<UILabel>().text = TaoShangScore.ToString();
        switch (opType)
        {
            case CardOperateType.ChuPai:
                LargestCard = cards;
                LargestPos = pos;//记录最大牌和位置
                HideOperatePanle(pos);//隐藏操作界面
                ShowPlayedCard(pos, cards);
                ShowOrHideDontPlayCard(pos, false);

                #region  NewTurnControl
                if (counting)
                {
                    counting = false;
                    index = 0;
                }
                #endregion

                break;

            case CardOperateType.GuoPai:

                HideOperatePanle(pos);//隐藏操作界面
                ShowOrHideDontPlayCard(pos, true);

                #region  NewTurnControl
                if (counting)
                {
                    index++;
                    if (index == EffectCount)
                    {
                        index = 0;
                        counting = false;
                        LargestCard = new List<uint>();
                        LargestPos = 0;
                    }
                }
                #endregion
                break;

        }

        if (leftCardCount == 0)//有玩家出完牌(为了下次帮手顺风出牌)
        {
            counting = true;
            EffectCount -= 1;
        }



        SoundControl.Instance.CheckOperateSoundType(cards, sex);//播放出牌yin效
    }

    /// <summary>
    /// 新一轮 出牌开始 有人出完
    /// </summary>
    bool counting = false;
    int index = 0;
    int EffectCount = 4;//有效玩家数量
    public void NewTurnControl()
    {

    }
    /// <summary>
    /// 玩家不出
    /// </summary>
    /// <param name="pos"></param>
    public void ShowDontPlay(int pos)
    {
        if (pos == SelfPos)
        {
            return;
        }
        else
        {
            IdAndPlayerDic[pos].transform.Find("OperatePanel").Find("DontPlaySprite").gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 庄家位置
    /// </summary>
    bool getZhuangPos = false;
    public void onZhuangPosition()
    {
        getZhuangPos = true;

    }

    /// <summary>
    /// 设置庄
    /// </summary>
    public void OnZhuang()
    {
        foreach (var item in IdAndPlayerDic)
        {
            if (item.Key == GameData.m_TableInfo.ZhuangPos)//显示地主图标
            {
                item.Value.transform.Find("OtherPanel").Find("LandSprite").gameObject.SetActive(true);
            }
        }
      //  ShowOperatePanle(GameData.m_TableInfo.ZhuangPos, PuKeOperateType.ChuPai);


        Debug.Log("庄家位置返回");
    }
    /// <summary>
    /// 显示操作界面
    /// </summary>
    public void ShowOperatePanle(int pos, PuKeOperateType type)
    {

        ClearPlayedCard(pos);//清除掉前面打出的牌
                             //  
        if (pos == 0)
        {
            Debug.Log("准备阶段（非打牌阶段）");
        }
        else
        {
            if (pos != SelfPos)
            {
                IdAndPlayerDic[pos].transform.Find("OperatePanel").gameObject.SetActive(true);
                // IdAndPlayerDic[pos].transform.FindChild("OperatePanel").FindChild("DontPlaySprite").gameObject.SetActive(false);
                IdAndPlayerDic[pos].transform.Find("OperatePanel").Find("TimerSprite").gameObject.SetActive(true);
                IdAndPlayerDic[pos].transform.Find("OperatePanel").Find("TimerSprite").GetComponent<TimeCountDown>().SetTimeCountDown(15);
                CheckOtherIsLargest(pos, type);//检查其他玩家是否为最大  清除不出
            }
            else//玩家自己
            {
                IdAndPlayerDic[pos].transform.Find("OperatePanel").gameObject.SetActive(true);
                IdAndPlayerDic[pos].transform.Find("OperatePanel").Find("TimerSprite").GetComponent<TimeCountDown>().SetTimeCountDown(15);
                CheckOperateShow(type);
            }
        }

        ShowOrHideDontPlayCard(pos, false);//隐藏不出图标

    }

    /// <summary>
    /// 检查其他玩家是不是最大玩家  消除桌面上的不出
    /// </summary>
    /// <param name="pos">该出牌的位置</param>
    public void CheckOtherIsLargest(int pos, PuKeOperateType type)
    {
        if (LargestPos == pos)//当前玩家为最大玩家
        {
            for (int i = 1; i < 5; i++)
            {
                ShowOrHideDontPlayCard(i, false);

                ClearPlayedCard(i);//一轮出牌结束  清除所有手牌
            }
        }
        else
        {

        }
        if (type==PuKeOperateType.ChuPai)
        {
            for (int i = 1; i < 5; i++)
            {
                ShowOrHideDontPlayCard(i, false);
                ClearPlayedCard(i);//一轮出牌结束  清除所有手牌
            }
        }
    }
    /// <summary>
    /// 检查自己是不是第一家  或最大家
    /// </summary>
    public void CheckOperateShow(PuKeOperateType type)
    {

        if (type==PuKeOperateType.ChuPai)
        {
            IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("DontPlayOperateSprite").gameObject.SetActive(true);
            IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("NoticeSprite").gameObject.SetActive(true);
            IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("PlaySprite").gameObject.SetActive(true);
            IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("YaoBuYiSprite").gameObject.SetActive(false);
            LargestPos = 0;


            #region  
            IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("DontPlayOperateSprite").gameObject.SetActive(false);
            IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("NoticeSprite").gameObject.SetActive(false);
            IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("PlaySprite").localPosition = IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("NoticeSprite").localPosition;
            LargestPos = 0;
            for (int i = 1; i < 5; i++)
            {
                IdAndPlayerDic[i].transform.Find("DontPlaySprite").gameObject.SetActive(false);
                ClearPlayedCard(i);
            }

            #endregion
        }
        else
        {
            //if ((int)this.LargestPos == SelfPos || LargestPos == 0)//自己为最大出牌玩家
            //{
            //    IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("DontPlayOperateSprite").gameObject.SetActive(false);
            //    IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("NoticeSprite").gameObject.SetActive(false);
            //    IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("PlaySprite").localPosition = IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("NoticeSprite").localPosition;
            //    LargestPos = 0;
            //    for (int i = 1; i < 5; i++)
            //    {
            //        IdAndPlayerDic[i].transform.FindChild("DontPlaySprite").gameObject.SetActive(false);
            //        ClearPlayedCard(i);
            //    }
            //}
            //else
            //{
                #region 判断有没有大过的牌
                List<uint> uintCard = new List<uint>();
                List<List<uint>> CanPressList = new List<List<uint>>();
                for (int i = 0; i < LocalCardsObjList.Count; i++)
                {
                    uintCard.Add(uint.Parse(LocalCardsObjList[i].transform.GetComponent<UISprite>().spriteName));
                }
                if (LargestCard.Count != 0)
                {
                    // CanPressCardList = myQiPaiHelper.Instance.getFitCards(LargestCard, uintCard, ref IsSeperateBoom);

                    CanPressList = myQiPaiHelper.Instance.getFitCards(LargestCard, uintCard, GameData.m_TableInfo.isBawang);
                    //  SetNoticeCardShow(NoticeClickCount);
                }
                else
                {
                    Debug.LogError("当前没有最大牌");
                }
                #endregion

                if (CanPressList.Count > 0)//有大过的牌
                {

                }
                else//没有大过的牌
                {
                    IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("DontPlayOperateSprite").gameObject.SetActive(false);
                    IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("NoticeSprite").gameObject.SetActive(false);
                    IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("PlaySprite").gameObject.SetActive(false);
                    IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("YaoBuYiSprite").gameObject.SetActive(true);
                    IdAndPlayerDic[SelfPos].transform.Find("NoLargeCardNoticeSprite").gameObject.SetActive(true);
                }


            }
        }

   // }

    /// <summary>
    /// 隐藏操作界面
    /// </summary>
    public void HideOperatePanle(int pos)
    {
        ClearPlayedCard(pos);//清除打出的牌
        if (pos != SelfPos)
        {
            // IdAndPlayerDic[pos].transform.FindChild("OperatePanel").FindChild("DontPlaySprite").gameObject.SetActive(false);
            IdAndPlayerDic[pos].transform.Find("OperatePanel").Find("TimerSprite").gameObject.SetActive(false);
            // IdAndPlayerDic[pos].transform.FindChild("OperatePanel").gameObject.SetActive(false);
        }
        else
        {
            IdAndPlayerDic[pos].transform.Find("OperatePanel").gameObject.SetActive(false);
            IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("PlaySprite").localPosition = new Vector3(745, 180, 0);
            IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("DontPlayOperateSprite").gameObject.SetActive(true);
            IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("NoticeSprite").gameObject.SetActive(true);
            IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("PlaySprite").gameObject.SetActive(true);
            IdAndPlayerDic[SelfPos].transform.Find("OperatePanel").Find("YaoBuYiSprite").gameObject.SetActive(false);
            IdAndPlayerDic[SelfPos].transform.Find("NoLargeCardNoticeSprite").gameObject.SetActive(false);
        }




    }

    /// <summary>
    /// 显示不出
    /// </summary>
    /// <param name=""></param>
    public void ShowOrHideDontPlayCard(int pos, bool isShow)
    {
        if (pos == 0)
        {
            return;
        }
        if (isShow)
        {
            IdAndPlayerDic[pos].transform.Find("DontPlaySprite").gameObject.SetActive(true);
        }
        else
        {
            IdAndPlayerDic[pos].transform.Find("DontPlaySprite").gameObject.SetActive(false);
        }

    }

    /// <summary>
    ///  显示打出的牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="cardlist"></param>
    public void ShowPlayedCard(int pos, List<uint> cardlist)
    {

        if (pos == SelfPos)//自己打的牌
        {
            if (cardlist.Count == 0)//不出
            {
                // IdAndPlayerDic[pos].transform.FindChild("").gameObject.SetActive(true);
            }
            else
            {
                //销毁选中的牌CurChooseCardsObjList
                //手牌重新排序
                //  DestroyChoseCard();
                ClearSelfPlayedCard(cardlist);
                //todo  生成打出的牌
                CreatPlayedCard(pos, cardlist);
            }
        }
        else
        {
            if (cardlist.Count == 0)//不出
            {
                IdAndPlayerDic[pos].transform.Find("DontPlaySprite").gameObject.SetActive(true);
            }
            else
            {
                //todo  生成打出的牌
                CreatPlayedCard(pos, cardlist);
            }
        }
    }



    /// <summary>
    /// 删除自己打出的牌
    /// </summary>
    /// <param name="CardList"></param>
    public void ClearSelfPlayedCard(List<uint> CardList)
    {
        if (CurChooseCardsObjList.Count != 0)//因为有自动打牌
        {
            for (int i = 0; i < CurChooseCardsObjList.Count; i++)
            {
                CurChooseCardsObjList[i].transform.localPosition = new Vector3(CurChooseCardsObjList[i].transform.localPosition.x, 0, CurChooseCardsObjList[i].transform.localPosition.z);
            }
            CurChooseCardsObjList = new List<GameObject>();
        }
        List<GameObject> NeedDestroy = new List<GameObject>();
        List<GameObject> NewLocalCardsObjList = new List<GameObject>();
        for (int i = 0; i < CardList.Count; i++)
        {
            for (int j = 0; j < LocalCardsObjList.Count; j++)
            {
                if (uint.Parse(LocalCardsObjList[j].transform.GetComponent<UISprite>().spriteName) == CardList[i])
                {
                    if (!NeedDestroy.Contains(LocalCardsObjList[j]))
                    {
                        NeedDestroy.Add(LocalCardsObjList[j]);
                        CurChooseCardsObjList.Add(LocalCardsObjList[j]);
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }

            }
        }

        DestroyChoseCard();


    }
    /// <summary>
    /// 除掉选中的牌(出牌返回)
    /// </summary>
  
    public void DestroyChoseCard()
    {
        List<GameObject> NewCard = new List<GameObject>();
        List<GameObject> NeedDestroycard = new List<GameObject>();
        List<CardData> NeedDisCardDataList = new List<CardData>();

        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {
            if (!CurChooseCardsObjList.Contains(LocalCardsObjList[i]))
            {
                NewCard.Add(LocalCardsObjList[i]);
            }
            else
            {
                NeedDestroycard.Add(LocalCardsObjList[i]);

                for (int j = 0; j < CardDataList.Count; j++)
                {
                    if ((CardDataList[j].data == (uint.Parse(LocalCardsObjList[i].transform.GetComponent<UISprite>().spriteName))) &&
                       CardDataList[j].Index == LocalCardsObjList[i].transform.GetComponent<Card>().index)
                    {
                        CardData data = CardDataList[j];
                        NeedDisCardDataList.Add(data);//去掉数据 排序用
                    }
                }
                //  CardDataList.Remove(uint .Parse(LocalCardsObjList[i].transform.GetComponent<UISprite>().spriteName));
            }
        }

        for (int i = 0; i < NeedDisCardDataList.Count; i++)
        {
            CardDataList.Remove(NeedDisCardDataList[i]);
        }

        LocalCardsObjList = new List<GameObject>();
        LocalCardsObjList = NewCard;
        int count = NeedDestroycard.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(NeedDestroycard[i]);
        }
      
       
            CardSort();
       
    
        CurChooseCardsObjList = new List<GameObject>();
    }
    /// <summary>
    /// 生成打出的牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="cardlist"></param>
    public void CreatPlayedCard(int pos, List<uint> cardlist)
    {
        ClearPlayedCard(pos);//清除打出的牌

        if (GameData.m_TableInfo.configPlayerIndex == 4)//四人
        {
            if (SelfPos - pos == -1 || SelfPos - pos == 3)//右边玩家
            {
                IdAndPlayerDic[pos].transform.Find("PlayedCardPoint").localPosition = new Vector3(-240 - cardlist.Count * 12, 0, 0);
            }
        }
       
         if (SelfPos - pos == 1 || SelfPos - pos == -3)//左边玩家
        {

        }
        else if (Mathf.Abs(SelfPos - pos) == 2)//对面玩家
        {
            //  IdAndPlayerDic[pos].transform.FindChild("PlayedCardPoint").localPosition = new Vector3(217 - cardlist.Count * 12, -111, 0);
        }
        else if (SelfPos == pos)//自己
        {
            IdAndPlayerDic[pos].transform.Find("PlayedCardPoint").localPosition = new Vector3(557 - cardlist.Count * 12, 34, 0);
        }

        PosAndPlayedCard[pos] = new List<GameObject>();

        // cardlist = CardTools.JokerInsteadForNum(cardlist);
        // cardlist = CardTools.CardValueSort(cardlist);//按值排序CardNumSort

        cardlist = CardTools.CardNumSort(cardlist);
        for (int i = 0; i < cardlist.Count; i++)
        {
            GameObject card = Instantiate(CardObj, IdAndPlayerDic[pos].transform.Find("PlayedCardPoint"));
            card.transform.localScale = new Vector3(0.7f, 0.7f, 0);
            if (i > 11)
            {
                card.transform.localPosition = new Vector3((i - 12) * 25, -50, 0);
            }
            else
            {
                card.transform.localPosition = new Vector3(i * 25, 0, 0);
              //  card.transform.GetComponent<UISprite>().depth=
            }

            card.transform.GetComponent<Card>().SetValue(cardlist[i]);
            PosAndPlayedCard[pos].Add(card);
        }

        for (int i = 0; i < PosAndPlayedCard[pos].Count; i++)
        {
            if (i > 0)
            {
                PosAndPlayedCard[pos][i].transform.GetComponent<UISprite>().depth = PosAndPlayedCard[pos][0].transform.GetComponent<UISprite>().depth * 2 * i;
                PosAndPlayedCard[pos][i].transform.Find("Sprite").GetComponent<UISprite>().depth = PosAndPlayedCard[pos][i].transform.GetComponent<UISprite>().depth + 1;
            }
           
        }
    }

    /// <summary>
    /// 清除打出的牌
    /// </summary>
    public void ClearPlayedCard(int pos)
    {
        try
        {
            if (PosAndPlayedCard[pos] == null || PosAndPlayedCard[pos].Count != 0)
            {
                for (int i = 0; i < PosAndPlayedCard[pos].Count; i++)
                {
                    Destroy(PosAndPlayedCard[pos][i]);
                }
            }

            PosAndPlayedCard[pos] = new List<GameObject>();
        }

        catch (Exception e)
        {
            Debug.Log(e.Message);

        }

    }

    /// <summary>
    /// 清空掉所有的手牌（一局结束后）
    /// </summary>
    public void ClearHoldCard()
    {
        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {
            Destroy(LocalCardsObjList[i]);
        }

        LocalCardsObjList = new List<GameObject>();
    }

    /// <summary>
    /// 显示剩余的牌
    /// </summary>

    public void ShowLeftCardNum(int pos, int leftCardNum)
    {

        if (pos != SelfPos)
        {
            if (leftCardNum < 6)
            {
                IdAndPlayerDic[pos].transform.Find("LeftCard").gameObject.SetActive(true);
                IdAndPlayerDic[pos].transform.Find("LeftCard").Find("Label").GetComponent<UILabel>().text = leftCardNum.ToString();
            }
        }
        
       
    }

    /// <summary>
    /// 重置剩余的牌
    /// </summary>
    public void ResetLeftCardNum()
    {
        for (int i = 1; i < 5; i++)
        {
            IdAndPlayerDic[i].transform.Find("LeftCard").gameObject.SetActive(false);
        }
    }



    #endregion

    #endregion
}
