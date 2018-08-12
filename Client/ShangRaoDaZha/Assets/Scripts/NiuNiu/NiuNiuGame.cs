/*
* 开发人：滕俊
* 项目：
* 功能描述：牛牛玩法核心
*
*
*/

using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NiuNiuGame : UIBase<NiuNiuGame>
{
    #region

   

    public GameObject CardObj;//牌的预制体
    public GameObject CardPoint;//卡牌的起点
  

    public UIButton InviteFriend;//邀请朋友
    public UIButton ReadyBtn;//准备按钮
    public UIButton ReadyForNext;//准备下一局按钮
    public UIButton StartGameBtn;//房主开始游戏按钮

    public GameObject QiangZhuangObj;
    public UIButton QiangZhuangBtn;//抢庄按钮
    public UIButton BuQiangBtn;//不抢按钮

    public GameObject ChuoPaiPanel;//搓牌界面
    public UIButton ChuoPaiBtn;//搓牌按钮
    public UIButton liangPaiBtn;//亮牌牌按钮
    public UIButton FanPaiBtn;//翻牌的按钮

    public GameObject ChipPanel;//下注panel
   

    public UIButton DisMissRoomBtn;//解散房间按钮
    public UIButton ChatBtn;

    public UILabel RoomIdLable;//房间号
    public UILabel ZhuangWeiLable;//庄位
    public UILabel DiFenLable;//底分
    public UILabel RoundLable;//局数

    public List<GameObject> PlayerList;//其他玩家列表
    public Dictionary<int, GameObject> IdAndPlayerDic;//玩家id和gameobj  其实就是pos1234

    public List<GameObject> MaiMaBtnList;
    Dictionary<int, GameObject> MaiMaBtnDic=new Dictionary<int, GameObject>();//玩家id和gameobj  其实就是pos1234

    public List<GameObject> LableBtnList;
    public Dictionary<int, GameObject> LableBtnDic = new Dictionary<int, GameObject>();//闲家买码

    public  int SelfPos;//自己的位置
 
    public GameObject TableMask;//桌布mask
 
    public UIButton SettingBtn;//设置按钮




    private int currentCount = 0;
   public  int CurrentCount//当前第几局
    {
        get {
            return currentCount;
        }
        set
        {

            currentCount = value;
            DiFenLable.text = "局数:[FBFF00FF]" + currentCount + "[-]/"+GameData.m_TableInfo.configRoundIndex;
        }
    }

    /// <summary>
    /// 房间激活
    /// </summary>
    public void onRoomActive()
    {
        CurrentCount = 1;
    }
    // Use this for initialization
    void Start()
    {
      

        GameData.m_IsNormalOver = false;//重置
    
        ChatBtn.onClick.Add(new EventDelegate(this.OpenChatPanel));
        //  AiButton.onClick.Add(new EventDelegate(this.SetAi));
        SettingBtn.onClick.Add(new EventDelegate(this.OpenSettingPanel));
        UIEventListener.Get(TableMask).onClick = this.CancleSelectCard;
        DisMissRoomBtn.onClick.Add(new EventDelegate(this.DisMissRoom));
        ReadyBtn.onClick.Add(new EventDelegate(this.ReadyForGame));
        ReadyForNext.onClick.Add(new EventDelegate(this.ReadyForNextGame));
        InviteFriend.onClick.Add(new EventDelegate(this.ShareInvite));
        ReadyBtn.gameObject.SetActive(true);
        InviteFriend.gameObject.SetActive(true);
        // QiangZhuangBtn.onClick.Add(new EventDelegate(this.SendQiangZhuang));//抢庄
        UIEventListener.Get(QiangZhuangBtn.gameObject).onClick = this.OnClick;
        UIEventListener.Get(BuQiangBtn.gameObject).onClick = this.OnClick;

        ChuoPaiBtn.onClick.Add(new EventDelegate(ChouPaiBtnClick));
        //{
        //   
        //  //  IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").gameObject.SetActive(false);
        //}));
        liangPaiBtn.onClick.Add(new EventDelegate(()=>
        {
            ClientToServerMsg.SendShowCard();
            liangPaiBtn.gameObject.SetActive(false);
           // IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").gameObject.SetActive(false);

        }));
        FanPaiBtn.onClick.Add(new EventDelegate(this.FanPaiAnim));//翻牌

        IdAndPlayerDic = new Dictionary<int, GameObject>();

        SetRoomInfo();

        SetPlayerListPos();


        if (Player.Instance.lastEnterRoomID != 0)//断线重连
        {
            ReconnectServer();//重连
        }
        else//中途加入
        {
            if (GameData.m_TableInfo.roomState == RoomStatusType.Play)
            {
                ShowJoinRoomInfo();//中途加入显示信息
            }
            else if (GameData.m_TableInfo.roomState == RoomStatusType.Over)
            {
                ReadyBtn.gameObject.SetActive(false);
                InviteFriend.gameObject.SetActive(false);
               // ReadyForNext.gameObject.SetActive(false);
                ReadyForNext.gameObject.SetActive(true);
            }
            ResetForce();
        }


    }

    /// <summary>
    /// 搓牌按钮点击
    /// </summary>
    void ChouPaiBtnClick()
    {
        StartCoroutine(HandsCardAnim());
    }

    IEnumerator HandsCardAnim()
    {

        PlayHandsCardAnim(false);
        yield return new WaitForSeconds(0.4f);
        ChuoPaiPanel.SetActive(true);
    }
    /// <summary>
    ///准备下一局
    /// </summary>
    private void ReadyForNextGame()
    {
        ClientToServerMsg.Send(Opcodes.Client_PlayerReady, GameData.m_TableInfo.id);//发送准备协议
        ReadyForNext.gameObject.SetActive(false);
        SetButtonClickSound();
    }


    /// <summary>
    /// 播放自己的翻牌动画
    /// </summary>
    public  void FanPaiAnim()
    {
        FanPaiBtn.gameObject.SetActive(false);
        ChuoPaiBtn.gameObject.SetActive(false);
        //to do  显示牌
        PlayerInfo info = GameDataFunc.GetPlayerInfo(Player.Instance.guid);
        for (int i = 0; i < info.localCardList.Count; i++)
        {
            IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card" + i.ToString()).GetComponent<UISprite>().spriteName = info.localCardList[i].ToString();
        }
        PlayHandsCardAnim(true);
        //播放完之后再显示亮牌按钮
        liangPaiBtn.gameObject.SetActive(true);
    }


    /// <summary>
    /// 手牌收拢和打开
    /// </summary>
    private void PlayHandsCardAnim(bool open,bool liangpai=false,int pos=0)
    {
        if (!liangpai)
        {
            if (open)
            {
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card0").gameObject, 0.2f, new Vector3(-260f, -25, 0));
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card1").gameObject, 0.2f, new Vector3(-140f, -25, 0));
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card2").gameObject, 0.2f, new Vector3(-20f, -25, 0));
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card3").gameObject, 0.2f, new Vector3(100f, -25, 0));
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card4").gameObject, 0.2f, new Vector3(220f, -25, 0));
            }
            else
            {
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card0").gameObject, 0.2f, new Vector3(-20f, -25, 0));
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card1").gameObject, 0.2f, new Vector3(-20f, -25, 0));
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card2").gameObject, 0.2f, new Vector3(-20f, -25, 0));
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card3").gameObject, 0.2f, new Vector3(-20f, -25, 0));
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card4").gameObject, 0.2f, new Vector3(-20f, -25, 0));
            }
        }
        else
        {
            if (pos == SelfPos)
            {
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card0").gameObject, 0.2f, new Vector3(-140f, -25, 0));
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card1").gameObject, 0.2f, new Vector3(-80f, -25, 0));
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card2").gameObject, 0.2f, new Vector3(-20f, -25, 0));
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card3").gameObject, 0.2f, new Vector3(100f, -25, 0));
                TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card4").gameObject, 0.2f, new Vector3(160f, -25, 0));
            }
            else
            {
                TweenPosition.Begin(IdAndPlayerDic[pos].transform.Find("cardPoint").Find("Card0").gameObject, 0.2f, new Vector3(-67f, 0, 0));
                TweenPosition.Begin(IdAndPlayerDic[pos].transform.Find("cardPoint").Find("Card1").gameObject, 0.2f, new Vector3(-38f, 0, 0));
                TweenPosition.Begin(IdAndPlayerDic[pos].transform.Find("cardPoint").Find("Card2").gameObject, 0.2f, new Vector3(-3f, 0, 0));
                TweenPosition.Begin(IdAndPlayerDic[pos].transform.Find("cardPoint").Find("Card3").gameObject, 0.2f, new Vector3(45f, 0, 0));
                TweenPosition.Begin(IdAndPlayerDic[pos].transform.Find("cardPoint").Find("Card4").gameObject, 0.2f, new Vector3(70f, 0, 0));
            }
        }
       
    }
    /// <summary>
    /// 设置房间信息
    /// </summary>
    private void SetRoomInfo()
    {

        RoomIdLable.text = "房号:" + GameData.m_TableInfo.id.ToString();
        ZhuangWeiLable.text = "底分:";
        for (int i = 0; i < GameData.m_TableInfo.CanChipList.Count; i++)
        {
            ZhuangWeiLable.text += " " + GameData.m_TableInfo.CanChipList[i];
        }
        // ZhuangWeiLable.text = "";
      //  DiFenLable.text = "当前第" + GameData.m_TableInfo.curGameCount + "局";
        DiFenLable.text = "局数:[FBFF00FF]" + GameData.m_TableInfo.curGameCount + "[-]/" + GameData.m_TableInfo.configRoundIndex;
        RoundLable.text = "";

    }

    /// <summary>
    /// 设置玩家的各个位置
    /// </summary>
    private void SetPlayerListPos()
    {

        #region  设置位置
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            Debug.Log(GameData.m_PlayerInfoList[i].guid + Player.Instance.guid);
            if (GameData.m_PlayerInfoList[i].guid == Player.Instance.guid)
            {
                switch (GameData.m_PlayerInfoList[i].pos)
                {
                    case 1:
                        IdAndPlayerDic[1] = PlayerList[0];
                        IdAndPlayerDic[2] = PlayerList[1];
                        IdAndPlayerDic[3] = PlayerList[2];
                        IdAndPlayerDic[4] = PlayerList[3];
                        IdAndPlayerDic[5] = PlayerList[4];
                        IdAndPlayerDic[6] = PlayerList[5];
                        IdAndPlayerDic[7] = PlayerList[6];
                        IdAndPlayerDic[8] = PlayerList[7];
                        SelfPos = 1;

                        break;
                    case 2:
                        IdAndPlayerDic[2] = PlayerList[0];
                        IdAndPlayerDic[3] = PlayerList[7];
                        IdAndPlayerDic[4] = PlayerList[6];
                        IdAndPlayerDic[5] = PlayerList[5];
                        IdAndPlayerDic[6] = PlayerList[4];
                        IdAndPlayerDic[7] = PlayerList[3];
                        IdAndPlayerDic[8] = PlayerList[2];
                        IdAndPlayerDic[1] = PlayerList[1];

                        SelfPos = 2;
                        break;
                    case 3:
                        IdAndPlayerDic[3] = PlayerList[0];
                        IdAndPlayerDic[4] = PlayerList[4];
                        IdAndPlayerDic[5] = PlayerList[7];
                        IdAndPlayerDic[6] = PlayerList[2];
                        IdAndPlayerDic[7] = PlayerList[5];
                        IdAndPlayerDic[8] = PlayerList[1];
                        IdAndPlayerDic[1] = PlayerList[3];
                        IdAndPlayerDic[2] = PlayerList[6];
                        SelfPos = 3;
                        break;
                    case 4:
                        IdAndPlayerDic[4] = PlayerList[0];
                        IdAndPlayerDic[5] = PlayerList[3];
                        IdAndPlayerDic[6] = PlayerList[6];
                        IdAndPlayerDic[7] = PlayerList[1];
                        IdAndPlayerDic[8] = PlayerList[4];
                        IdAndPlayerDic[1] = PlayerList[2];
                        IdAndPlayerDic[2] = PlayerList[7];
                        IdAndPlayerDic[3] = PlayerList[5];
                        SelfPos = 4;
                        break;
                    case 5:
                        IdAndPlayerDic[5] = PlayerList[0];
                        IdAndPlayerDic[6] = PlayerList[1];
                        IdAndPlayerDic[7] = PlayerList[7];
                        IdAndPlayerDic[8] = PlayerList[3];
                        IdAndPlayerDic[1] = PlayerList[5];
                        IdAndPlayerDic[2] = PlayerList[4];
                        IdAndPlayerDic[3] = PlayerList[6];
                        IdAndPlayerDic[4] = PlayerList[2];
                        SelfPos = 5;
                        break;
                    case 6:
                        IdAndPlayerDic[6] = PlayerList[0];
                        IdAndPlayerDic[7] = PlayerList[2];
                        IdAndPlayerDic[8] = PlayerList[6];
                        IdAndPlayerDic[1] = PlayerList[4];
                        IdAndPlayerDic[2] = PlayerList[5];
                        IdAndPlayerDic[3] = PlayerList[3];
                        IdAndPlayerDic[4] = PlayerList[7];
                        IdAndPlayerDic[5] = PlayerList[1];
                        SelfPos = 6;
                        break;
                    case 7:
                        IdAndPlayerDic[7] = PlayerList[0];
                        IdAndPlayerDic[8] = PlayerList[5];
                        IdAndPlayerDic[1] = PlayerList[7];
                        IdAndPlayerDic[2] = PlayerList[2];
                        IdAndPlayerDic[3] = PlayerList[4];
                        IdAndPlayerDic[4] = PlayerList[1];
                        IdAndPlayerDic[5] = PlayerList[6];
                        IdAndPlayerDic[6] = PlayerList[3];
                        SelfPos = 7;
                        break;
                    case 8:
                        IdAndPlayerDic[8] = PlayerList[0];
                        IdAndPlayerDic[1] = PlayerList[6];
                        IdAndPlayerDic[2] = PlayerList[3];
                        IdAndPlayerDic[3] = PlayerList[1];
                        IdAndPlayerDic[4] = PlayerList[5];
                        IdAndPlayerDic[5] = PlayerList[2];
                        IdAndPlayerDic[6] = PlayerList[7];
                        IdAndPlayerDic[7] = PlayerList[4];
                        SelfPos = 8;
                        break;
                }

              

            }
        }
        #endregion

        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            //  Debug.LogError(GameData.m_PlayerInfoList[i].pos);


            IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].gameObject.SetActive(true);

            string str= GameDataFunc.GetPlayerInfo(GameData.m_PlayerInfoList[i].pos).name.ToString();

            IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("TaoShangSprite/NameLabel").GetComponent<UILabel>().text = GameDataFunc.GetPlayerInfo(GameData.m_PlayerInfoList[i].pos).name.ToString();
            IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("TaoShangSprite/IconLable/Label").GetComponent<UILabel>().text = "0";
            // IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.FindChild("JiFenLabel").GetComponent<UILabel>().text = "讨赏分：" + 0.ToString();// GameDataFunc.GetPlayerInfo(GameData.m_PlayerInfoList[i].pos).name.ToString();
            DownloadImage.Instance.Download(IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("HeadPanel").Find("HeadSprite").GetComponent<UITexture>(), GameData.m_PlayerInfoList[i].headID);

            SetTotalJifen(GameData.m_PlayerInfoList[i].pos, 0);//设置积分


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


          SetMaiMaBtnDic();//设置买码的btn


    }

    /// <summary>
    /// 设置买码的btn
    /// </summary>
    private void SetMaiMaBtnDic()
    {
        switch (SelfPos)
        {
            case 1:
              

                MaiMaBtnDic[1] = MaiMaBtnList[0];
                MaiMaBtnDic[2] = MaiMaBtnList[1];
                MaiMaBtnDic[3] = MaiMaBtnList[2];
                MaiMaBtnDic[4] = MaiMaBtnList[3];
                MaiMaBtnDic[5] = MaiMaBtnList[4];
                MaiMaBtnDic[6] = MaiMaBtnList[5];
                MaiMaBtnDic[7] = MaiMaBtnList[6];
                MaiMaBtnDic[8] = MaiMaBtnList[7];

                LableBtnDic[1] = LableBtnList[0];
                LableBtnDic[2] = LableBtnList[1];
                LableBtnDic[3] = LableBtnList[2];
                LableBtnDic[4] = LableBtnList[3];
                LableBtnDic[5] = LableBtnList[4];
                LableBtnDic[6] = LableBtnList[5];
                LableBtnDic[7] = LableBtnList[6];
                LableBtnDic[8] = LableBtnList[7];

                break;
            case 2:
                LableBtnDic[2] = LableBtnList[0];
                LableBtnDic[3] = LableBtnList[7];
                LableBtnDic[4] = LableBtnList[6];
                LableBtnDic[5] = LableBtnList[5];
                LableBtnDic[6] = LableBtnList[4];
                LableBtnDic[7] = LableBtnList[3];
                LableBtnDic[8] = LableBtnList[2];
                LableBtnDic[1] = LableBtnList[1];

                MaiMaBtnDic[2] = MaiMaBtnList[0];
                MaiMaBtnDic[3] = MaiMaBtnList[7];
                MaiMaBtnDic[4] = MaiMaBtnList[6];
                MaiMaBtnDic[5] = MaiMaBtnList[5];
                MaiMaBtnDic[6] = MaiMaBtnList[4];
                MaiMaBtnDic[7] = MaiMaBtnList[3];
                MaiMaBtnDic[8] = MaiMaBtnList[2];
                MaiMaBtnDic[1] = MaiMaBtnList[1];


                break;
            case 3:
                LableBtnDic[3] = LableBtnList[0];
                LableBtnDic[4] = LableBtnList[4];
                LableBtnDic[5] = LableBtnList[7];
                LableBtnDic[6] = LableBtnList[2];
                LableBtnDic[7] = LableBtnList[5];
                LableBtnDic[8] = LableBtnList[1];
                LableBtnDic[1] = LableBtnList[3];
                LableBtnDic[2] = LableBtnList[6];

                MaiMaBtnDic[3] = MaiMaBtnList[0];
                MaiMaBtnDic[4] = MaiMaBtnList[4];
                MaiMaBtnDic[5] = MaiMaBtnList[7];
                MaiMaBtnDic[6] = MaiMaBtnList[2];
                MaiMaBtnDic[7] = MaiMaBtnList[5];
                MaiMaBtnDic[8] = MaiMaBtnList[1];
                MaiMaBtnDic[1] = MaiMaBtnList[3];
                MaiMaBtnDic[2] = MaiMaBtnList[6];

                break;
            case 4:
                LableBtnDic[4] = LableBtnList[0];
                LableBtnDic[5] = LableBtnList[3];
                LableBtnDic[6] = LableBtnList[6];
                LableBtnDic[7] = LableBtnList[1];
                LableBtnDic[8] = LableBtnList[4];
                LableBtnDic[1] = LableBtnList[2];
                LableBtnDic[2] = LableBtnList[7];
                LableBtnDic[3] = LableBtnList[5];

                MaiMaBtnDic[4] = MaiMaBtnList[0];
                MaiMaBtnDic[5] = MaiMaBtnList[3];
                MaiMaBtnDic[6] = MaiMaBtnList[6];
                MaiMaBtnDic[7] = MaiMaBtnList[1];
                MaiMaBtnDic[8] = MaiMaBtnList[4];
                MaiMaBtnDic[1] = MaiMaBtnList[2];
                MaiMaBtnDic[2] = MaiMaBtnList[7];
                MaiMaBtnDic[3] = MaiMaBtnList[5];

                break;
            case 5:
                LableBtnDic[5] = LableBtnList[0];
                LableBtnDic[6] = LableBtnList[1];
                LableBtnDic[7] = LableBtnList[7];
                LableBtnDic[8] = LableBtnList[3];
                LableBtnDic[1] = LableBtnList[5];
                LableBtnDic[2] = LableBtnList[4];
                LableBtnDic[3] = LableBtnList[6];
                LableBtnDic[4] = LableBtnList[2];

                MaiMaBtnDic[5] = MaiMaBtnList[0];
                MaiMaBtnDic[6] = MaiMaBtnList[1];
                MaiMaBtnDic[7] = MaiMaBtnList[7];
                MaiMaBtnDic[8] = MaiMaBtnList[3];
                MaiMaBtnDic[1] = MaiMaBtnList[5];
                MaiMaBtnDic[2] = MaiMaBtnList[4];
                MaiMaBtnDic[3] = MaiMaBtnList[6];
                MaiMaBtnDic[4] = MaiMaBtnList[2];

                break;
            case 6:
                LableBtnDic[6] = LableBtnList[0];
                LableBtnDic[7] = LableBtnList[2];
                LableBtnDic[8] = LableBtnList[6];
                LableBtnDic[1] = LableBtnList[4];
                LableBtnDic[2] = LableBtnList[5];
                LableBtnDic[3] = LableBtnList[3];
                LableBtnDic[4] = LableBtnList[7];
                LableBtnDic[5] = LableBtnList[1];

                MaiMaBtnDic[6] = MaiMaBtnList[0];
                MaiMaBtnDic[7] = MaiMaBtnList[2];
                MaiMaBtnDic[8] = MaiMaBtnList[6];
                MaiMaBtnDic[1] = MaiMaBtnList[4];
                MaiMaBtnDic[2] = MaiMaBtnList[5];
                MaiMaBtnDic[3] = MaiMaBtnList[3];
                MaiMaBtnDic[4] = MaiMaBtnList[7];
                MaiMaBtnDic[5] = MaiMaBtnList[1];

                break;
            case 7:
                LableBtnDic[7] = LableBtnList[0];
                LableBtnDic[8] = LableBtnList[5];
                LableBtnDic[1] = LableBtnList[7];
                LableBtnDic[2] = LableBtnList[2];
                LableBtnDic[3] = LableBtnList[4];
                LableBtnDic[4] = LableBtnList[1];
                LableBtnDic[5] = LableBtnList[6];
                LableBtnDic[6] = LableBtnList[3];

                MaiMaBtnDic[7] = MaiMaBtnList[0];
                MaiMaBtnDic[8] = MaiMaBtnList[5];
                MaiMaBtnDic[1] = MaiMaBtnList[7];
                MaiMaBtnDic[2] = MaiMaBtnList[2];
                MaiMaBtnDic[3] = MaiMaBtnList[4];
                MaiMaBtnDic[4] = MaiMaBtnList[1];
                MaiMaBtnDic[5] = MaiMaBtnList[6];
                MaiMaBtnDic[6] = MaiMaBtnList[3];

                break;
            case 8:
                LableBtnDic[8] = LableBtnList[0];
                LableBtnDic[1] = LableBtnList[6];
                LableBtnDic[2] = LableBtnList[3];
                LableBtnDic[3] = LableBtnList[1];
                LableBtnDic[4] = LableBtnList[5];
                LableBtnDic[5] = LableBtnList[2];
                LableBtnDic[6] = LableBtnList[7];
                LableBtnDic[7] = LableBtnList[4];

                MaiMaBtnDic[8] = MaiMaBtnList[0];
                MaiMaBtnDic[1] = MaiMaBtnList[6];
                MaiMaBtnDic[2] = MaiMaBtnList[3];
                MaiMaBtnDic[3] = MaiMaBtnList[1];
                MaiMaBtnDic[4] = MaiMaBtnList[5];
                MaiMaBtnDic[5] = MaiMaBtnList[2];
                MaiMaBtnDic[6] = MaiMaBtnList[7];
                MaiMaBtnDic[7] = MaiMaBtnList[4];

                break;
        }

        foreach (var item in MaiMaBtnDic)
        {
            item.Value.transform.GetComponent<XianJiaMaiMaBtnControl>().SetValue(item.Key);


        }
    }

    #region  OnHander

    private List<int> QiangZhuangPosList = new List<int>();//抢庄的list
    /// <summary>
    ///  广播有谁抢庄
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="QiangZhuang"></param>
    public void OnWhoQiangZhuang(byte pos, bool QiangZhuang)
    {
        Debug.Log(pos + "/抢庄");
        IdAndPlayerDic[pos].transform.Find("OtherPanel/QianZhuangLabel").GetComponent<QiangZhuangLable>().SetValue(QiangZhuang);
        if (QiangZhuang)
        {
            QiangZhuangPosList.Add((int)pos);
        }
    }

    /// <summary>
    /// 玩家下注
    /// </summary>
    /// <param name="pos">自己的位置</param>
    /// <param name="Chip">给自己下了多少注</param>
    /// <param name="OtherChipDic">给他玩家下了多少注</param>
    private Dictionary<int, List<int>> PlayerChipOther = new Dictionary<int, List<int>>();//给其他人下的注
    public void PlayerChipIn(byte pos, uint Chip, Dictionary<byte, uint> OtherChipDic)
    {
        if (Chip != 0)//有给自己的下注(自己和别人)
        {
            if (IdAndPlayerDic[pos].transform.Find("TaoShangSprite/IconLable/Label").GetComponent<UILabel>().text != Chip.ToString())
            {
                if (pos == SelfPos)
                {
                    IdAndPlayerDic[pos].transform.Find("TaoShangSprite/IconLable").gameObject.SetActive(true);
                }
                IdAndPlayerDic[pos].transform.Find("TaoShangSprite/Move0").GetComponent<ChipAnimControl>().PlayAnim();
            }
            IdAndPlayerDic[pos].transform.Find("TaoShangSprite/IconLable/Label").GetComponent<UILabel>().text = Chip.ToString();

            if (pos == SelfPos)
            {
                ChipPanel.transform.Find("Chip0").gameObject.SetActive(false);
                ChipPanel.transform.Find("Chip1").gameObject.SetActive(false);
                ChipPanel.transform.Find("Chip2").gameObject.SetActive(false);
            }
        }
       

        //to do 播放下注动画
        //RoundAnimControl.instance.SetLocalPath(IdAndPlayerDic[pos].transform.FindChild("TaoShangSprite/StartPoint").localPosition, IdAndPlayerDic[pos].transform.FindChild("TaoShangSprite/IconLable").localPosition, IdAndPlayerDic[pos].transform.FindChild("TaoShangSprite"));
      

        if (OtherChipDic.Count != 0)//有给其他玩家下注
        {
            foreach (var item in OtherChipDic)
            {
                if (!PlayerChipOther.ContainsKey((int)pos))
                {
                    RoundAnimControl.instance.SetPath(IdAndPlayerDic[pos].transform.localPosition, LableBtnDic[(int)item.Key].transform.localPosition);
                    LableBtnDic[(int)item.Key].SetActive(true);
                    int a = int.Parse(LableBtnDic[(int)item.Key].transform.Find("Label").GetComponent<UILabel>().text);
                    LableBtnDic[(int)item.Key].transform.Find("Label").GetComponent<UILabel>().text =(a+ item.Value).ToString();
                    if (pos == SelfPos)
                    {
                        MaiMaBtnList[(int)item.Key].SetActive(false);
                    }
                  //  MaiMaBtnList[(int)item.Key].SetActive(false);
                    List<int> posList = new List<int>();
                    posList.Add((int)item.Key);
                    PlayerChipOther[(int)pos] = posList;
                }
                else
                {
                    if (!PlayerChipOther[(int)pos].Contains((int)item.Key))
                    {
                        RoundAnimControl.instance.SetPath(IdAndPlayerDic[pos].transform.localPosition, LableBtnDic[(int)item.Key].transform.localPosition);
                        LableBtnDic[(int)item.Key].SetActive(true);
                        int a = int.Parse(LableBtnDic[(int)item.Key].transform.Find("Label").GetComponent<UILabel>().text);
                        LableBtnDic[(int)item.Key].transform.Find("Label").GetComponent<UILabel>().text = (a + item.Value).ToString();

                        if (pos == SelfPos)
                        {
                            MaiMaBtnList[(int)item.Key].SetActive(false);
                        }
                        // MaiMaBtnList[(int)item.Key].SetActive(false);
                        PlayerChipOther[(int)pos].Add((int)item.Key);
                    }
                }
              
                // IdAndPlayerDic[pos].transform.FindChild("TaoShangSprite/Move0 (1)").GetComponent<ChipAnimControl>().PlayeXianJiaMaiMa(IdAndPlayerDic[pos].transform.FindChild("TaoShangSprite/StartPoint").gameObject, IdAndPlayerDic[(int)item.Key].transform.FindChild("TaoShangSprite/OtherIconLable").gameObject);
            }
           
        }
      
        //if (pos == SelfPos)
        //{
        //    // ChipPanel.gameObject.SetActive(false);//隐藏自己的下注面板
        //    ChipPanel.transform.FindChild("Chip0").gameObject.SetActive(false);
        //    ChipPanel.transform.FindChild("Chip1").gameObject.SetActive(false);
        //    ChipPanel.transform.FindChild("Chip2").gameObject.SetActive(false);
        //    for (int i = 0; i < MaiMaBtnList.Count; i++)
        //    {
        //        MaiMaBtnList[i].SetActive(false);

        //    }
        //}
    }

    /// <summary>
    /// 单局结算
    /// </summary>
    public void onGameOver()
    {
        // to do 播放结算动画
        // to do 清理桌上的东西
        ClearTable();
        //准备下一局
       
    }

    /// <summary>
    /// 开始广播抢庄抢庄
    /// </summary>
    public void OnQiangZhuang()
    {
        QiangZhuangObj.SetActive(true);
        TimeCountDownPanelControl.Instance.Init(GameData.TIME_QIANGZHUANG,"等待抢庄");
        RestPlayerState();
    }

    #endregion





    /// <summary>
    /// 结算的时候清理桌子
    /// </summary>
    public void ClearTable()
    {
        TimeCountDownPanelControl.Instance.HideTimer();
        ChipPanelControl.Instance.OtherChipDic = new Dictionary<byte, uint>();//买码数据清空
        IdAndPlayerDic[SelfPos].transform.Find("TaoShangSprite/IconLable").gameObject.SetActive(false);
        ChuoPaiPanel.SetActive(false);
       // PlayOverAnim();
        StartCoroutine(PlayOverAnim());
       
    }


    /// <summary>
    /// 播放单局结算的动画
    /// </summary>
   IEnumerator PlayOverAnim()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < NiuNiuPartOverInfo.LosePosList.Count; i++)
        {
            try
            {
                RoundAnimControl.instance.SetPath(IdAndPlayerDic[NiuNiuPartOverInfo.LosePosList[i]].transform.localPosition, IdAndPlayerDic[GameData.m_TableInfo.ZhuangPos].transform.localPosition);
            }
            catch (Exception e)
            {
                Debug.LogError("输家位置有问题");
            }
         
        }
        if (NiuNiuPartOverInfo.LosePosList.Count != 0)
        {
            yield return new WaitForSeconds(2f);
        }

        for (int i = 0; i < NiuNiuPartOverInfo.WinPosList.Count; i++)
        {
            try
            {
                RoundAnimControl.instance.SetPath(IdAndPlayerDic[GameData.m_TableInfo.ZhuangPos].transform.localPosition, IdAndPlayerDic[NiuNiuPartOverInfo.WinPosList[i]].transform.localPosition);
            }
            catch (Exception e)
            {
                Debug.LogError("赢家位置有问题");
            }
         
        }
        SetOverScoreValue();//积分改变
        //变化的分数
        foreach (var item in NiuNiuPartOverInfo.PosAndChangeScore)
        {
            IdAndPlayerDic[item.Key].transform.Find("NumAnim").GetComponent<NumAnimControl>().SetValue(item.Value);
        }
        yield return new WaitForSeconds(3f);

        NiuNiuPartOverReset();
    }


    /// <summary>
    /// 设置积分
    /// </summary>
    private void SetOverScoreValue()
    {
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("TaoShangSprite/LeftLabel").GetComponent<UILabel>().text = "[FFF800FF]" + GameData.m_PlayerInfoList[i].score+"[-]";
        }
    }
    /// <summary>
    /// 单局结束的重置
    /// </summary>
    private void NiuNiuPartOverReset()
    {
        foreach (var item in IdAndPlayerDic)
        {
            if (item.Key == GameData.m_TableInfo.ZhuangPos)//显示地主图标
            {
                item.Value.transform.Find("OtherPanel").Find("LandSprite").gameObject.SetActive(false);
            }
        }

        for (int i = 1; i < 9; i++)
        {
            IdAndPlayerDic[i].transform.Find("cardPoint").gameObject.SetActive(false);
            IdAndPlayerDic[i].transform.Find("cardPoint/CardTypeSprite").GetComponent<NiuNiuCardTypeAnim>().ResetToBegin();
            IdAndPlayerDic[i].transform.Find("cardPoint/CardTypeSprite").gameObject.SetActive(false);
            IdAndPlayerDic[i].transform.Find("TaoShangSprite/IconLable/Label").GetComponent<UILabel>().text = "0";
            IdAndPlayerDic[i].transform.Find("TaoShangSprite/OtherIconLable").transform.gameObject.SetActive(false);
            LableBtnDic[i].SetActive(false);
            LableBtnDic[i].transform.Find("Label").GetComponent<UILabel>().text = 0.ToString();
            MaiMaBtnDic[i].SetActive(false);
        }
        ChipPanelControl.Instance.ChipChose = 0;
        //   IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").gameObject.SetActive(false);
        liangPaiBtn.gameObject.SetActive(false);

        ReadyForNext.gameObject.SetActive(true);
        CurrentCount++;//当前第几局
        PlayerChipOther = new Dictionary<int, List<int>>();
    }
    /// <summary>
    /// 物体点击
    /// </summary>
    /// <param name="go"></param>
    private void OnClick(GameObject go)
    {
        if (go == QiangZhuangBtn.gameObject)
        {
            ClientToServerMsg.SendQiangZhuang(true,1);
        }
        else if (go == BuQiangBtn.gameObject)
        {
            ClientToServerMsg.SendQiangZhuang(false, 1);
        }
        QiangZhuangObj.SetActive(false);
    }

    /// <summary>
    /// 玩家亮牌
    /// </summary>
    public void PlayerShowCard(uint pos, NNType PeiPaiType, UInt32 FanBeiCount, List<List<uint>> PeiPaiInfo)
    {
      
        List<uint> CardList = new List<uint>();
        for (int i = 0; i < PeiPaiInfo[1].Count; i++)
        {
            CardList.Add(PeiPaiInfo[1][i]);
        }
        for (int i = 0; i < PeiPaiInfo[2].Count; i++)
        {
            CardList.Add(PeiPaiInfo[2][i]);
        }
        for (int i = 0; i < CardList.Count; i++)
        {
            IdAndPlayerDic[(int)pos].transform.Find("cardPoint").Find("Card" + i.ToString()).GetComponent<UISprite>().spriteName = CardList[i].ToString();
            IdAndPlayerDic[(int)pos].transform.Find("cardPoint").gameObject.SetActive(true);
        }
        IdAndPlayerDic[(int)pos].transform.Find("cardPoint/CardTypeSprite").GetComponent<NiuNiuCardTypeAnim>().SetCardValue(PeiPaiType, FanBeiCount);//播放牌型动画
        if (pos == SelfPos)
        {
           

            liangPaiBtn.gameObject.SetActive(false);
            FanPaiBtn.gameObject.SetActive(false);
            ChuoPaiBtn.gameObject.SetActive(false);
            ChuoPaiPanel.gameObject.SetActive(false);
        }

        PlayHandsCardAnim(false,true,(int)pos);
        //to do  播放牌型动画
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            if ((uint)GameData.m_PlayerInfoList[i].pos == pos)
            {
                SoundControl.Instance.PlayNiuNiuCardType((int)GameData.m_PlayerInfoList[i].sex, PeiPaiType);//播放音效
            }
        }
      
    }

    

    /// <summary>
    /// 重置玩家的一些狀態
    /// </summary>
    public void RestPlayerState()
    {
        ReadyBtn.gameObject.SetActive(false);
        InviteFriend.gameObject.SetActive(false);
        ReadyForNext.gameObject.SetActive(false);
        for (int i = 1; i < 9; i++)
        {
            IdAndPlayerDic[i].transform.Find("OtherPanel").Find("ReadySprite").gameObject.SetActive(false);
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
        Vector3 vecPos = IdAndPlayerDic[(int)pos].transform.Find("HeadPanel").Find("HeadSprite").position;
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
        // jishu = (int)GameData.m_TableInfo.configPayIndex;
        switch ((int)GameData.m_TableInfo.configPayIndex)
        {
           
            case 0:
               
                AuthorizeOrShare.Instance.ShareRoomID(GameData.m_TableInfo.id, "一起来玩闲娱狗吧！" + ((int)GameData.m_TableInfo.configRoundIndex).ToString() + "局!" + "  房主支付!", "闲娱狗牛牛");
               
                break;
            case 1:
               
                AuthorizeOrShare.Instance.ShareRoomID(GameData.m_TableInfo.id, "一起来玩闲娱狗吧！" + ((int)GameData.m_TableInfo.configRoundIndex).ToString() + "局!"  + "!" + "  平摊支付!", "闲娱狗牛牛");
               
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
        UIManager.Instance.ShowUIPanel(UIPaths.ChatFace, OpenPanelType.MinToMax);
        SetButtonClickSound();
    }

    /// <summary>
    /// 发送表情聊天信息
    /// </summary>
    /// <param name="content"></param>
    public void onPlayerSendFaceChatFace(uint roomid, string content)
    {
        StartCoroutine(AsynCreateChatFace(content));
    }
    IEnumerator AsynCreateChatFace(string content)
    {
        string[] strs = content.Split('@');
        PlayerInfo info = GameDataFunc.GetPlayerInfo(ulong.Parse(strs[1]));
        GameObject preObj = Resources.Load<GameObject>("Face/Face" + strs[2]);
        GameObject face = Instantiate<GameObject>(preObj);
        face.transform.parent = IdAndPlayerDic[info.pos].transform.Find("HeadPanel");
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
        //if (roomNum == GameData.m_TableInfo.id)
        //{
        //    if (isAi)
        //    {
        //        IdAndPlayerDic[pos].transform.FindChild("OtherPanel").FindChild("AISprite").gameObject.SetActive(true);



        //    }
        //    else
        //    {
        //        IdAndPlayerDic[pos].transform.FindChild("OtherPanel").FindChild("AISprite").gameObject.SetActive(false);

        //    }
        //    if (pos == SelfPos)
        //    {
        //        if (isAi)
        //        {
        //            AiButton.transform.FindChild("AingSprite").gameObject.SetActive(true);
        //        }
        //        else
        //        {
        //            AiButton.transform.FindChild("AingSprite").gameObject.SetActive(false);
        //        }
        //        IsTuoGuan = isAi;
        //    }
        //}
    }
    /// <summary>
    /// 设置托管
    /// </summary>
    private void SetAi()
    {
        //if (AiButton.transform.FindChild("AingSprite").gameObject.activeSelf)
        //{
        //    SendTuoGuan(false);
        //    AiButton.transform.FindChild("AingSprite").gameObject.SetActive(false);
        //}
        //else
        //{
        //    SendTuoGuan(true);
        //    AiButton.transform.FindChild("AingSprite").gameObject.SetActive(true);
        //}
        //SetButtonClickSound();
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
        UIManager.Instance.ShowUIPanel(UIPaths.SettingPanel2, OpenPanelType.MinToMax);
        SetButtonClickSound();
    }

    /// <summary>
    /// 取消选中的牌
    /// </summary>
    /// <param name="go"></param>
    private void CancleSelectCard(GameObject go)
    {
        //for (int i = 0; i < CurChooseCardsObjList.Count; i++)
        //{
        //    CurChooseCardsObjList[i].transform.localPosition = new Vector3(CurChooseCardsObjList[i].transform.localPosition.x, 0, CurChooseCardsObjList[i].transform.localPosition.z);
        //}
        //NoticeClickCount = -1;
        //CurChooseCardsObjList = new List<GameObject>();
        //SetButtonClickSound();
        //TipSeperateBoomObj.SetActive(false);
    }


    #region  断线重连

    /// <summary>
    /// 中途加入显示房间的信息
    /// </summary>
    public void ShowJoinRoomInfo()
    {
        ReadyBtn.gameObject.SetActive(false);
        InviteFriend.gameObject.SetActive(false);
        ReadyForNext.gameObject.SetActive(false);
        if (GameData.m_TableInfo.IsQiangZhuangState)//抢庄状态
        {
            OnQiangZhuang();

        }
        else if (GameData.m_TableInfo.IsXiaZhuState)//下注状态
        {
            ResetChip();

        }
        else//配牌阶段
        {
           // ResetChip();
            for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
            {
                if (GameData.m_PlayerInfoList[i].pos == SelfPos) continue;//自己不处理
                IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("cardPoint").gameObject.SetActive(true);
                for (int j = 0; j < 5; j++)
                {
                    IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("cardPoint").Find("Card" + j.ToString()).GetComponent<UISprite>().spriteName = "bei2";
                    IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("cardPoint").Find("Card" + j.ToString()).gameObject.SetActive(true);
                    //IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.FindChild("cardPoint").FindChild("Card" + j.ToString()).GetComponent<TweenScale>().ResetToBeginning();
                    //IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.FindChild("cardPoint").FindChild("Card" + j.ToString()).gameObject.SetActive(false);
                    // TweenScale.Begin(IdAndPlayerDic[SelfPos].transform.FindChild("cardPoint").FindChild("Card" + i.ToString()).gameObject,);
                }
                if (GameData.m_PlayerInfoList[i].PeiPaiInfo.Count != 0)
                {
                    PlayerShowCard(GameData.m_PlayerInfoList[i].pos, GameData.m_PlayerInfoList[i].PeiPaiType, GameData.m_PlayerInfoList[i].FanBeiCount, GameData.m_PlayerInfoList[i].PeiPaiInfo);//玩家亮牌信息
                }
               
            }

            foreach (var item in IdAndPlayerDic)//恢复庄图标
            {
                if (item.Key == GameData.m_TableInfo.ZhuangPos)//显示地主图标
                {
                    item.Value.transform.Find("OtherPanel").Find("LandSprite").gameObject.SetActive(true);
                }
            }
            foreach (var item in GameData.m_TableInfo.PosAndChipDic)
            {
                for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
                {
                    if (item.Key == GameData.m_PlayerInfoList[i].pos)
                    {
                        PlayerChipIn((byte)item.Key, (uint)item.Value, GameData.m_PlayerInfoList[i].PosAndChipDic);
                    }
                }
            }
        }
        GameData.m_TableInfo.IsQiangZhuangState = false;
        GameData.m_TableInfo.IsXiaZhuState = false;
    }

    /// <summary>
    /// 重连回来
    /// </summary>
    public void ReconnectServer()
    {
            switch (GameData.m_TableInfo.roomState)
            {
                case RoomStatusType.None:
                    RoomNone();
                    break;
                case RoomStatusType.Play:
                    RoomPlay();
                    break;
                case RoomStatusType.Over:
                 //   RoomPlay();
                    RoomOver();
                    break;
            }
      

    }


    void RoomNone()
    {
        ReadyBtn.gameObject.SetActive(true);
        InviteFriend.gameObject.SetActive(true);
        // ReadyBtn.gameObject.SetActive(true);
        //InviteFriend.gameObject.SetActive(false);
        //ReadyForNext.gameObject.SetActive(true);
    }

    void RoomPlay()
    {
        ReadyBtn.gameObject.SetActive(false);
        InviteFriend.gameObject.SetActive(false);
        ReadyForNext.gameObject.SetActive(false);
        if (GameData.m_TableInfo.IsQiangZhuangState)//抢庄状态
        {
            if (!GameData.m_TableInfo.QiangZhuangPosList.Contains(SelfPos))
            {
                OnQiangZhuang();
            }
          
        }
        else if (GameData.m_TableInfo.IsXiaZhuState)//下注状态
        {
            ResetChip();
            
        }
        else//配牌阶段
        {
            ResetChip();
            HideChipPanel();
            ResetHandCard();
          
        }
        ResetForce();


    }

    /// <summary>
    /// 恢复是否为断线状态
    /// </summary>
    void ResetForce()
    {
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)//恢复是否为断线
        {
            onPlayerOnForce(GameData.m_PlayerInfoList[i].pos, GameData.m_PlayerInfoList[i].isForce);
        }
    }
    //恢复下注
    void ResetChip()
    {

        ShowChipPanel();
        foreach (var item in IdAndPlayerDic)//恢复庄图标
        {
            if (item.Key == GameData.m_TableInfo.ZhuangPos)//显示地主图标
            {
                item.Value.transform.Find("OtherPanel").Find("LandSprite").gameObject.SetActive(true);
            }
        }
        foreach (var item in GameData.m_TableInfo.PosAndChipDic)
        {
            for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
            {
                if (item.Key == GameData.m_PlayerInfoList[i].pos)
                {
                    PlayerChipIn((byte)item.Key, (uint)item.Value, GameData.m_PlayerInfoList[i].PosAndChipDic);
                }
            }
        }
    }

    //恢复手牌信息
    void ResetHandCard()
    {
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("cardPoint").gameObject.SetActive(true);
            for (int j = 0; j < 5; j++)
            {
                IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("cardPoint").Find("Card" + j.ToString()).GetComponent<UISprite>().spriteName = "bei2";
                IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("cardPoint").Find("Card" + j.ToString()).gameObject.SetActive(true);
                //IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.FindChild("cardPoint").FindChild("Card" + j.ToString()).GetComponent<TweenScale>().ResetToBeginning();
                //IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.FindChild("cardPoint").FindChild("Card" + j.ToString()).gameObject.SetActive(false);
                // TweenScale.Begin(IdAndPlayerDic[SelfPos].transform.FindChild("cardPoint").FindChild("Card" + i.ToString()).gameObject,);
            }
            if (GameData.m_PlayerInfoList[i].PeiPaiInfo.Count != 0)
            {
                PlayerShowCard(GameData.m_PlayerInfoList[i].pos, GameData.m_PlayerInfoList[i].PeiPaiType, GameData.m_PlayerInfoList[i].FanBeiCount, GameData.m_PlayerInfoList[i].PeiPaiInfo);//玩家亮牌信息
            }
            if (GameData.m_PlayerInfoList[i].pos == SelfPos)//自己没有亮牌
            {
                if (GameData.m_PlayerInfoList[i].PeiPaiInfo.Count == 0)
                {
                    ShowOperatePanel();
                }
            }
        }
    }

    void RoomOver()
    {
        ReadyForNext.gameObject.SetActive(true);
    }

    #endregion
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
 
   

    public void SetButtonClickSound()
    {
        SoundManager.Instance.PlaySound(UIPaths.BUTTONCLICK);
    }
    /// <summary>
    /// 一局结束
    /// </summary>
    public void onPartGameOver()
    {
        //SetGameOverSound();
        //for (int i = 1; i < 5; i++)
        //{
        //    SetTaoShangFen(i, 0);//设置讨赏分
        //    HideOperatePanle(i);
        //    ClearPlayedCard(i);
        //    ShowOrHideDontPlayCard(i, false);//隐藏不出

        //    SetTotalJifen(PartGameOverControl.instance.SettleInfoList[i - 1].pos, PartGameOverControl.instance.SettleInfoList[i - 1].Score);//显示积分
        //}

        //CurChooseCardsObjList = new List<GameObject>();
        //PosAndPlayedCard = new Dictionary<int, List<GameObject>>();
        //CanPressCardList = new List<List<uint>>();
        //LargestCard = new List<uint>();
        //LargestPos = 0;
        //ClearHoldCard();//清除玩家手牌

        //ResetLeftCardNum();
        //HideZhuangAndHelper();

        //FriendCard.SetActive(false);
        //ResetFinishPlayerIndex();
        //UIManager.Instance.ShowUIPanel(UIPaths.GameOverPanel);


        //ResetCardSortType();//重置排序
        //                    //  StartCoroutine(GameOverTimeDely());
    }

    IEnumerator GameOverTimeDely()
    {
        yield return new WaitForSeconds(2f);
        ResetLeftCardNum();
       // HideZhuangAndHelper();
        ClearHoldCard();//清除玩家手牌

        for (int i = 1; i < 5; i++)
        {
            SetTaoShangFen(i, 0);//设置讨赏分
            HideOperatePanle(i);
            ClearPlayedCard(i);
        }
        UIManager.Instance.ShowUIPanel(UIPaths.GameOverPanel);
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void GameReset()
    {
        //for (int i = 1; i < 5; i++)
        //{
        //    HideOperatePanle(i);
        //    ClearPlayedCard(i);
        //    ShowOrHideDontPlayCard(i, false);//隐藏不出

        //    //  SetTotalJifen(PartGameOverControl.instance.SettleInfoList[i - 1].pos, PartGameOverControl.instance.SettleInfoList[i - 1].Score);//显示积分
        //}

        //CurChooseCardsObjList = new List<GameObject>();
        //PosAndPlayedCard = new Dictionary<int, List<GameObject>>();
        //CanPressCardList = new List<List<uint>>();
        //LargestCard = new List<uint>();
        //ClearHoldCard();//清除玩家手牌

        //HideZhuangAndHelper();
        ////ReadyBtn.gameObject.SetActive(true);
        ////  AiButton.gameObject.SetActive(false);
        //FriendCard.SetActive(false);

    }

  
    /// <summary>
    /// 解散房间
    /// </summary>
    private void DisMissRoom()
    {
        ClientToServerMsg.Send(Opcodes.Client_PlayerLeaveRoom, GameData.m_TableInfo.id, true);
        SetButtonClickSound();
    }

   


    

    #region  toserver
    /// <summary>
    /// 发送准备
    /// </summary>
    private void ReadyForGame()
    {

        ClientToServerMsg.Send(Opcodes.Client_PlayerReadyStart, GameData.m_TableInfo.id);//发送准备协议
        ReadyBtn.gameObject.SetActive(false);
        SetButtonClickSound();
    }
    

   
    
    #endregion




   

    

    

    /// <summary>
    /// 设置玩家积分
    /// </summary>
    /// <param name="pos"></param>
    public void SetTotalJifen(int pos, int Score)
    {
        IdAndPlayerDic[pos].transform.Find("TaoShangSprite").Find("LeftLabel").GetComponent<UILabel>().text = ""+Score.ToString();
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
            IdAndPlayerDic[(int)pos].transform.Find("OffLine").gameObject.SetActive(false);
        }
        else
        {
            IdAndPlayerDic[(int)pos].transform.Find("OffLine").gameObject.SetActive(true);

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
        IdAndPlayerDic[info.pos].transform.Find("TaoShangSprite/NameLabel").GetComponent<UILabel>().text = GameDataFunc.GetPlayerInfo(info.pos).name.ToString();
        IdAndPlayerDic[info.pos].transform.Find("TaoShangSprite/IconLable/Label").GetComponent<UILabel>().text = "";
        DownloadImage.Instance.Download(IdAndPlayerDic[info.pos].transform.Find("HeadPanel").Find("HeadSprite").GetComponent<UITexture>(), info.headID);
        SetTotalJifen(info.pos, 0);//设置积分
       // SetTaoShangFen(info.pos, 0);//设置讨赏分
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
    /// 开始发牌
    /// </summary>
    public void onPlayerHoldCards()
    {
        TimeCountDownPanelControl.Instance.HideTimer();
        transform.Find("BianKuangSprite").gameObject.SetActive(false);
        HideChipPanel();
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("cardPoint").gameObject.SetActive(true);
            for (int j = 0; j < 5; j++)
            {
                IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("cardPoint").Find("Card" + j.ToString()).GetComponent<UISprite>().spriteName = "bei2";
                IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("cardPoint").Find("Card" + j.ToString()).GetComponent<TweenScale>().ResetToBeginning();
                IdAndPlayerDic[GameData.m_PlayerInfoList[i].pos].transform.Find("cardPoint").Find("Card" + j.ToString()).gameObject.SetActive(false);
                // TweenScale.Begin(IdAndPlayerDic[SelfPos].transform.FindChild("cardPoint").FindChild("Card" + i.ToString()).gameObject,);
            }

            StartCoroutine(PlayerFaPaiAnim(GameData.m_PlayerInfoList[i].pos));
            //to do 播放发牌动画 结束后再执行下面操作
            MaiMaBtnDic[GameData.m_PlayerInfoList[i].pos].SetActive(false);

        }


        ShowOperatePanel();
        //  StartCoroutine(PlayFaPaiAnim());
        //IdAndPlayerDic[SelfPos].transform.FindChild("cardPoint").gameObject.SetActive(true);
        //for (int i = 0; i < 5; i++)
        //{
        //    IdAndPlayerDic[SelfPos].transform.FindChild("cardPoint").FindChild("Card" + i.ToString()).GetComponent<UISprite>().spriteName = "bei2";
        //    IdAndPlayerDic[SelfPos].transform.FindChild("cardPoint").FindChild("Card" + i.ToString()).GetComponent<TweenScale>().ResetToBeginning();
        //    IdAndPlayerDic[SelfPos].transform.FindChild("cardPoint").FindChild("Card" + i.ToString()).gameObject.SetActive(false);
        //   // TweenScale.Begin(IdAndPlayerDic[SelfPos].transform.FindChild("cardPoint").FindChild("Card" + i.ToString()).gameObject,);
        //}
        ////to do 播放发牌动画 结束后再执行下面操作
        //StartCoroutine(PlayFaPaiAnim());
    }

    IEnumerator PlayerFaPaiAnim(int pos)
    {
        if (pos == SelfPos)
        {
            TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card0").gameObject, 0.2f, new Vector3(-260f, -25, 0));
            TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card1").gameObject, 0.2f, new Vector3(-140f, -25, 0));
            TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card2").gameObject, 0.2f, new Vector3(-20f, -25, 0));
            TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card3").gameObject, 0.2f, new Vector3(100f, -25, 0));
            TweenPosition.Begin(IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card4").gameObject, 0.2f, new Vector3(220f, -25, 0));
        }
        else
        {
            TweenPosition.Begin(IdAndPlayerDic[pos].transform.Find("cardPoint").Find("Card0").gameObject, 0.2f, new Vector3(-67f, 0, 0));
            TweenPosition.Begin(IdAndPlayerDic[pos].transform.Find("cardPoint").Find("Card1").gameObject, 0.2f, new Vector3(-38f, 0, 0));
            TweenPosition.Begin(IdAndPlayerDic[pos].transform.Find("cardPoint").Find("Card2").gameObject, 0.2f, new Vector3(-3f, 0, 0));
            TweenPosition.Begin(IdAndPlayerDic[pos].transform.Find("cardPoint").Find("Card3").gameObject, 0.2f, new Vector3(26f, 0, 0));
            TweenPosition.Begin(IdAndPlayerDic[pos].transform.Find("cardPoint").Find("Card4").gameObject, 0.2f, new Vector3(51f, 0, 0));
        }
        for (int i = 0; i < 5; i++)
        {
          
            IdAndPlayerDic[pos].transform.Find("cardPoint").Find("Card" + i.ToString()).gameObject.SetActive(true);
            IdAndPlayerDic[pos].transform.Find("cardPoint").Find("Card" + i.ToString()).GetComponent<TweenScale>().PlayForward();
           
            yield return new WaitForSeconds(0.2f);
            // TweenScale.Begin(IdAndPlayerDic[SelfPos].transform.FindChild("cardPoint").FindChild("Card" + i.ToString()).gameObject,);
        }
        TimeCountDownPanelControl.Instance.Init(GameData.TIME_LIANGPAI, "等待亮牌");
    }

    //播放发牌动画
    IEnumerator PlayFaPaiAnim()
    {


        for (int i = 0; i < 5; i++)
        {
            IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card" + i.ToString()).gameObject.SetActive(true);
            IdAndPlayerDic[SelfPos].transform.Find("cardPoint").Find("Card" + i.ToString()).GetComponent<TweenScale>().PlayForward();
            yield return new WaitForSeconds(0.2f);
            // TweenScale.Begin(IdAndPlayerDic[SelfPos].transform.FindChild("cardPoint").FindChild("Card" + i.ToString()).gameObject,);
        }
        ShowOperatePanel();
    }

    /// <summary>
    /// 显示操作界面
    /// </summary>
    public void ShowOperatePanel()
    {
        // IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").gameObject.SetActive(true);
        ChuoPaiBtn.gameObject.SetActive(true);
        FanPaiBtn.gameObject.SetActive(true);
    }
  
    
 
    /// <summary>
    /// 庄家位置
    /// </summary>
    bool getZhuangPos = false;
    public void onZhuangPosition()
    {
        CurrentCount =(int) GameData.m_TableInfo.curGameCount;
        TimeCountDownPanelControl.Instance.HideTimer();
        // getZhuangPos = true;
        StartCoroutine(RandomZhuangShow());
    }

    /// <summary>
    /// 定庄随机闪烁
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomZhuangShow()
    {
        if (QiangZhuangPosList.Count > 1)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < QiangZhuangPosList.Count; i++)
                {
                    IdAndPlayerDic[QiangZhuangPosList[i]].transform.Find("ChoseCircle").gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.1f);
                    IdAndPlayerDic[QiangZhuangPosList[i]].transform.Find("ChoseCircle").gameObject.SetActive(false);
                }
            }
           
        }
        else
        {
            yield return new WaitForSeconds(1f);
          
        }

        QiangZhuangObj.SetActive(false);
        foreach (var item in IdAndPlayerDic)
        {
            if (item.Key == GameData.m_TableInfo.ZhuangPos)//显示地主图标
            {
                item.Value.transform.Find("OtherPanel").Find("LandSprite").gameObject.SetActive(true);
            }
        }

        ShowChipPanel();
        TimeCountDownPanelControl.Instance.Init(GameData.TIME_XIAZHU, "等待下注");
        QiangZhuangPosList = new List<int>();//重置谁抢庄的list
    }


   

    /// <summary>
    /// 显示下注panel
    /// 
    /// </summary>
    public void ShowChipPanel()
    {
        if (SelfPos == GameData.m_TableInfo.ZhuangPos) return;
        else
        {
            ChipPanel.gameObject.SetActive(true);
            for (int i = 0; i < GameData.m_TableInfo.CanChipList.Count; i++)//可下那些基础分
            {
                ChipPanel.transform.Find("Chip" + i.ToString()).gameObject.SetActive(true);
                ChipPanel.transform.Find("Chip" + i.ToString()).Find("Label").GetComponent<UILabel>().text = GameData.m_TableInfo.CanChipList[i].ToString() + "分";
            }
            if (GameData.m_TableInfo.CanChipList.Count == 2)
            {
                ChipPanel.transform.Find("Chip0").localPosition = new Vector3(-64,-163,0);
                ChipPanel.transform.Find("Chip1").localPosition = new Vector3(75,-163,0);
            }

            if (GameData.m_TableInfo.EnXianJiaMaiMA)//是否为闲家买码
            {
                ChipPanel.transform.Find("xianjiaMa").gameObject.SetActive(true);
               transform.Find("BianKuangSprite").gameObject.SetActive(true);
                for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
                {


                    //if (GameData.m_PlayerInfoList[i].pos == SelfPos)
                    //{
                    //    continue;
                    //}
                    //  MaiMaBtnDic[GameData.m_PlayerInfoList[i].pos].SetActive(true);
                    if (GameData.m_PlayerInfoList[i].pos != GameData.m_TableInfo.ZhuangPos)
                    {
                        MaiMaBtnDic[GameData.m_PlayerInfoList[i].pos].gameObject.SetActive(true);
                    }

                }
            }
        }
       
    }

    /// <summary>
    /// 影藏闲家买码panel
    /// </summary>
    public void HideChipPanel()
    {
        transform.Find("BianKuangSprite").gameObject.SetActive(false);
        foreach (var item in MaiMaBtnDic)
        {
           
          //  item.Value.transform.GetComponent<XianJiaMaiMaBtnControl>().Reset();
            item.Value.SetActive(false);
        }
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
        //if (LargestPos == pos)//当前玩家为最大玩家
        //{
        //    for (int i = 1; i < 5; i++)
        //    {
        //        ShowOrHideDontPlayCard(i, false);

        //        ClearPlayedCard(i);//一轮出牌结束  清除所有手牌
        //    }
        //}
        //else
        //{

        //}
        //if (type == PuKeOperateType.ChuPai)
        //{
        //    for (int i = 1; i < 5; i++)
        //    {
        //        ShowOrHideDontPlayCard(i, false);
        //        ClearPlayedCard(i);//一轮出牌结束  清除所有手牌
        //    }
        //}
    }
    /// <summary>
    /// 检查自己是不是第一家  或最大家
    /// </summary>
    public void CheckOperateShow(PuKeOperateType type)
    {
        /*
        if (type == PuKeOperateType.ChuPai)
        {
            IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("DontPlayOperateSprite").gameObject.SetActive(true);
            IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("NoticeSprite").gameObject.SetActive(true);
            IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("PlaySprite").gameObject.SetActive(true);
            IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("YaoBuYiSprite").gameObject.SetActive(false);
            LargestPos = 0;


            #region  
            IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("DontPlayOperateSprite").gameObject.SetActive(false);
            IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("NoticeSprite").gameObject.SetActive(false);
            IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("PlaySprite").localPosition = IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("NoticeSprite").localPosition;
            LargestPos = 0;
            for (int i = 1; i < 5; i++)
            {
                IdAndPlayerDic[i].transform.FindChild("DontPlaySprite").gameObject.SetActive(false);
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

                CanPressList = myQiPaiHelper.Instance.getFitCards(LargestCard, uintCard);
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
                IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("DontPlayOperateSprite").gameObject.SetActive(false);
                IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("NoticeSprite").gameObject.SetActive(false);
                IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("PlaySprite").gameObject.SetActive(false);
                IdAndPlayerDic[SelfPos].transform.FindChild("OperatePanel").FindChild("YaoBuYiSprite").gameObject.SetActive(true);
                IdAndPlayerDic[SelfPos].transform.FindChild("NoLargeCardNoticeSprite").gameObject.SetActive(true);
            }


        }
        */
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
        //if (CurChooseCardsObjList.Count != 0)//因为有自动打牌
        //{
        //    for (int i = 0; i < CurChooseCardsObjList.Count; i++)
        //    {
        //        CurChooseCardsObjList[i].transform.localPosition = new Vector3(CurChooseCardsObjList[i].transform.localPosition.x, 0, CurChooseCardsObjList[i].transform.localPosition.z);
        //    }
        //    CurChooseCardsObjList = new List<GameObject>();
        //}
        //List<GameObject> NeedDestroy = new List<GameObject>();
        //List<GameObject> NewLocalCardsObjList = new List<GameObject>();
        //for (int i = 0; i < CardList.Count; i++)
        //{
        //    for (int j = 0; j < LocalCardsObjList.Count; j++)
        //    {
        //        if (uint.Parse(LocalCardsObjList[j].transform.GetComponent<UISprite>().spriteName) == CardList[i])
        //        {
        //            if (!NeedDestroy.Contains(LocalCardsObjList[j]))
        //            {
        //                NeedDestroy.Add(LocalCardsObjList[j]);
        //                CurChooseCardsObjList.Add(LocalCardsObjList[j]);
        //                break;
        //            }
        //            else
        //            {
        //                continue;
        //            }

        //        }

        //    }
        //}

        //DestroyChoseCard();


    }
    /// <summary>
    /// 除掉选中的牌(出牌返回)
    /// </summary>

    public void DestroyChoseCard()
    {
        //List<GameObject> NewCard = new List<GameObject>();
        //List<GameObject> NeedDestroycard = new List<GameObject>();
        //List<CardData> NeedDisCardDataList = new List<CardData>();

        //for (int i = 0; i < LocalCardsObjList.Count; i++)
        //{
        //    if (!CurChooseCardsObjList.Contains(LocalCardsObjList[i]))
        //    {
        //        NewCard.Add(LocalCardsObjList[i]);
        //    }
        //    else
        //    {
        //        NeedDestroycard.Add(LocalCardsObjList[i]);

        //        for (int j = 0; j < CardDataList.Count; j++)
        //        {
        //            if ((CardDataList[j].data == (uint.Parse(LocalCardsObjList[i].transform.GetComponent<UISprite>().spriteName))) &&
        //               CardDataList[j].Index == LocalCardsObjList[i].transform.GetComponent<Card>().index)
        //            {
        //                CardData data = CardDataList[j];
        //                NeedDisCardDataList.Add(data);//去掉数据 排序用
        //            }
        //        }
        //        //  CardDataList.Remove(uint .Parse(LocalCardsObjList[i].transform.GetComponent<UISprite>().spriteName));
        //    }
        //}

        //for (int i = 0; i < NeedDisCardDataList.Count; i++)
        //{
        //    CardDataList.Remove(NeedDisCardDataList[i]);
        //}

        //LocalCardsObjList = new List<GameObject>();
        //LocalCardsObjList = NewCard;
        //int count = NeedDestroycard.Count;
        //for (int i = 0; i < count; i++)
        //{
        //    Destroy(NeedDestroycard[i]);
        //}


        //CardSort();


        //CurChooseCardsObjList = new List<GameObject>();
    }
    /// <summary>
    /// 生成打出的牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="cardlist"></param>
    public void CreatPlayedCard(int pos, List<uint> cardlist)
    {
        //ClearPlayedCard(pos);//清除打出的牌

        //if (SelfPos - pos == -1 || SelfPos - pos == 3)//右边玩家
        //{
        //    IdAndPlayerDic[pos].transform.FindChild("PlayedCardPoint").localPosition = new Vector3(-240 - cardlist.Count * 12, 25, 0);
        //}
        //else if (SelfPos - pos == 1 || SelfPos - pos == -3)//左边玩家
        //{

        //}
        //else if (Mathf.Abs(SelfPos - pos) == 2)//对面玩家
        //{
        //    //  IdAndPlayerDic[pos].transform.FindChild("PlayedCardPoint").localPosition = new Vector3(217 - cardlist.Count * 12, -111, 0);
        //}
        //else if (SelfPos == pos)//自己
        //{
        //    IdAndPlayerDic[pos].transform.FindChild("PlayedCardPoint").localPosition = new Vector3(557 - cardlist.Count * 12, 80, 0);
        //}

        //PosAndPlayedCard[pos] = new List<GameObject>();

        //cardlist = CardTools.JokerInsteadForNum(cardlist);
        //cardlist = CardTools.CardValueSort(cardlist);//按值排序
        //for (int i = 0; i < cardlist.Count; i++)
        //{
        //    GameObject card = Instantiate(CardObj, IdAndPlayerDic[pos].transform.FindChild("PlayedCardPoint"));
        //    card.transform.localScale = new Vector3(0.7f, 0.7f, 0);
        //    if (i > 11)
        //    {
        //        card.transform.localPosition = new Vector3((i - 12) * 25, -50, 0);
        //    }
        //    else
        //    {
        //        card.transform.localPosition = new Vector3(i * 25, 0, 0);
        //        //  card.transform.GetComponent<UISprite>().depth=
        //    }

        //    card.transform.GetComponent<Card>().SetValue(cardlist[i]);
        //    PosAndPlayedCard[pos].Add(card);
        //}

        //for (int i = 0; i < PosAndPlayedCard[pos].Count; i++)
        //{
        //    if (i > 0)
        //    {
        //        PosAndPlayedCard[pos][i].transform.GetComponent<UISprite>().depth = PosAndPlayedCard[pos][0].transform.GetComponent<UISprite>().depth * 2 * i;
        //        PosAndPlayedCard[pos][i].transform.FindChild("Sprite").GetComponent<UISprite>().depth = PosAndPlayedCard[pos][i].transform.GetComponent<UISprite>().depth + 1;
        //    }

        //}
    }

    /// <summary>
    /// 清除打出的牌
    /// </summary>
    public void ClearPlayedCard(int pos)
    {
        //try
        //{
        //    if (PosAndPlayedCard[pos] == null || PosAndPlayedCard[pos].Count != 0)
        //    {
        //        for (int i = 0; i < PosAndPlayedCard[pos].Count; i++)
        //        {
        //            Destroy(PosAndPlayedCard[pos][i]);
        //        }
        //    }

        //    PosAndPlayedCard[pos] = new List<GameObject>();
        //}

        //catch (Exception e)
        //{
        //    Debug.Log(e.Message);

        //}

    }

    /// <summary>
    /// 清空掉所有的手牌（一局结束后）
    /// </summary>
    public void ClearHoldCard()
    {
        //for (int i = 0; i < LocalCardsObjList.Count; i++)
        //{
        //    Destroy(LocalCardsObjList[i]);
        //}

        //LocalCardsObjList = new List<GameObject>();
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
