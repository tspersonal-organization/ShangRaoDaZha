using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using System;

public class ZaiBaiGameJinBi : UIBase<ZaiBaiGameJinBi>
{


    public UILabel LBRoomID;
    public UILabel LBGameCount;
    public UILabel LBRoomDesc;
    public UILabel LBResCardCount;
    // public UILabel LBFangchong3D;//
    public UILabel JiangMa3d;//是否奖码
                             // public UILabel LBShengpai3D;

    public GameObject btnInvite;
    public GameObject btnReady;
    public GameObject btnGuo;
    public GameObject btnPeng;
    public GameObject btnGang;
    public GameObject btnHu;

    //  public Transform TranFPBtn;
    public Transform LocalParent;
    public Transform RightParent;
    public Transform UpParent;
    public Transform LeftParent;
    public Transform OutParent;

    public GameObject ObjMakerImg;//庄 
    public GameObject ObjCurOutImg;//当前操作图标
    public GameObject ObjOutSign;//标志位
    public GameObject ObjFangZhuImg;//房主标志

    public GameObject OutCountDown;//出牌倒计时
    public GameObject TopLift;//对家麻将桌升降板
    public GameObject SelfLift;//自家麻将桌升降板
    public GameObject LeftLift;//上家麻将桌升降板
    public GameObject RightLift;//下家麻将桌升降板
    public GameObject SelfOnDesk;//自己面前的未被摸走的牌堆
    public GameObject TopOnDesk;//对家面前的未被摸走的牌堆
    public GameObject LeftOnDesk;//上家面前的未被摸走的牌堆
    public GameObject RightOnDesk;//下家面前的未被摸走的牌堆
                                  // public GameObject HuKouObj;//显示听牌胡口
    public Transform SelfParent3D;//自己的3D父级
    public Transform RightParent3D;//下家的3D父级
    public Transform LeftParent3D;//上家的3D父级
    public Transform TopParent3D;//对家的3D父级
    public Transform MJPaiParent;//用来放在内存里复制用的所有麻将牌的父物体
    public Transform LocationMarkParent;//东南西北的标记位

    public GameObject CPGOnDuiJia;
    public GameObject CPGOnShangJia;
    public GameObject CPGOnXiaJia;
    public GameObject CPGOnLocal;//碰杠生成的牌的预制体

    public Vector3[] DicePointRList;
    public Transform DiceParent;//骰子父物体
    public GameObject Dice1;
    public GameObject Dice2;
    public List<GameObject> effectObjList = new List<GameObject>();
    public float localCardWidth;//3D麻将牌的宽度

    public float outMarkHeight;//出牌标记的漂浮高度
    public int TotalCardsCount;//总共多少张牌
    public int OutInterval = 20;//出牌倒计时的间隔
    public float LocalLeftCardStartPos = -570;//最左的牌起始相对坐标
    public Material NormalM;
    public Material ClickedM;

    GameObject[] AllCardsOnDesk;//指向所有未抓的牌的数组

    GameObject pbCardImage;//自己手牌预制体
    GameObject pbLocalItem;
    GameObject pbUpItem;
    GameObject pbEffectItem;


    Transform RightOnHand;
    Transform RightCPG;
    Transform RightChu;
    Transform RightHu;
    Transform RightOver;
    Transform SelfOnHand;
    Transform SelfCPG;
    Transform SelfChu;
    Transform SelfHu;//自己出牌放的位置
    Transform SelfOver;//胡牌放的位置
    Transform LeftOnHand;
    Transform LeftCPG;
    Transform LeftChu;
    Transform LeftHu;
    Transform LeftOver;
    Transform TopOnHand;
    Transform TopCPG;
    Transform TopChu;
    Transform TopHu;
    Transform TopOver;

    byte LocalPos;//当前自己位置
    byte InCardPos;//进牌位置
    uint InCardNumber;//进的牌
    byte curOutWhoOperate;//当前出牌位置
    bool IsDealCardOver;//发牌结束
    uint outCardNumber;//打出的牌
    public GameObject GangPaiPanel;//杠牌panel
    GameObject ObjPlayerChooseCard;
    GameObject ObjEffect;
    GameObject itemObj = null;
    string roomDesc;
    bool isDragOutCard = true;//是否可以拖动出牌
    bool isCurDragOutCard = false;//是否在拖动出牌
    Vector3 dragStartPos;
    List<GameObject> PlayerObjList = new List<GameObject>();
    List<GameObject> outCards = new List<GameObject>();
    int leftAndRightHideOffset = 28;
    int outCardOffsetX = 44;
    int outCardOffsetY = 54;
    int totalGangCount;//总共杠的数量

    int leftDeskStartStack;//上家一开始面前的牌堆有多少对
    int rightDeskStartStack;
    int topDeskStartStack;
    int selfDeskStartStack;
    Color32 huPaiCardColor = new Color32(253, 191, 56, 255);

    Vector3 originalLeftOnDeskLocalPos;//桌面上没有摸的牌组位置
    Vector3 originalRightOnDeskLocalPos;
    Vector3 originalTopOnDeskLocalPos;
    Vector3 originalSelfOnDeskLocalPos;

    Vector3[] PlayerStartPos4 = new Vector3[] { new Vector3(-570, -280), new Vector3(560, 70), new Vector3(-460, 300), new Vector3(-569, 70) };//四个玩家开始的位置
    Vector3[] PlayerStartPos2 = new Vector3[] { new Vector3(-570, -280), new Vector3(-460, 300) };
    Vector3[] PlayerDownPos4 = new Vector3[] { new Vector3(-12, -75), new Vector3(140, 70), new Vector3(-12, 190), new Vector3(-160, 70) };
    Vector3[] PlayerDownPos2 = new Vector3[] { new Vector3(-12, -75), new Vector3(-12, 190) };





    public UIButton LeaveBtn;
    public UIButton AiButton;//托管按钮
    //解散房间
    public void LeaveRoom()
    {
        ClientToServerMsg.Send(Opcodes.Client_PlayerLeaveRoom, GameData.m_TableInfo.id, true);
    }
    // Use this for initialization
    void Start()
    {
        AiButton.gameObject.SetActive(false);
        GameEventDispatcher.Instance.addEventListener(EventIndex.TingDropCardClick, this.ShowTingNotice);//听牌点击
        GameEventDispatcher.Instance.addEventListener(EventIndex.GangCardClick, this.NoticeGang);//听牌点击
        LeaveBtn.onClick.Add(new EventDelegate(this.LeaveRoom));

        GameData.m_IsNormalOver = false;
        foreach (Transform item in transform.Find("PlayerBase"))
            PlayerObjList.Add(item.gameObject);
        foreach (Transform item in transform.Find("BtnBase"))
            UIEventListener.Get(item.gameObject).onClick = OnClickBtn;
        //foreach (Transform item in TranFPBtn)
        //    UIEventListener.Get(item.gameObject).onClick = OnClickBtn;
        UIEventListener.Get(btnGuo).onClick = OnClickBtn;
        UIEventListener.Get(btnHu).onClick = OnClickBtn;
        UIEventListener.Get(btnPeng).onClick = OnClickBtn;
        UIEventListener.Get(btnGang).onClick = OnClickBtn;
        AiButton.onClick.Add(new EventDelegate(this.SetAi));
        for (int i = 0; i < PlayerObjList.Count; i++)
            UIEventListener.Get(PlayerObjList[i]).onClick = OnClickPlayer;
        UIEventListener.Get(transform.Find("BG").gameObject).onClick = OnClickBtn;

        SelfOnHand = SelfParent3D.transform.Find("UserOnHand");
        SelfCPG = SelfParent3D.transform.Find("UserCPG");



        #region  确定各种位置  
        if (GameData.m_TableInfo.configPlayerIndex == 2)//确定出牌放的位置
        {
            SelfChu = SelfParent3D.transform.Find("UserChuWhen2");
        }
        else
        {
            SelfChu = SelfParent3D.transform.Find("UserChu");
        }
        SelfHu = SelfParent3D.transform.Find("UserHu");
        SelfOver = SelfParent3D.transform.Find("UserOver");
        TopOnHand = TopParent3D.transform.Find("UserOnHand");
        TopCPG = TopParent3D.transform.Find("UserCPG");
        if (GameData.m_TableInfo.configPlayerIndex == 2)//确定出牌放的位置
        {
            TopChu = TopParent3D.transform.Find("UserChuWhen2");
        }
        else
        {
            TopChu = TopParent3D.transform.Find("UserChu");
        }
        TopHu = TopParent3D.transform.Find("UserHu");//胡牌位置
        TopOver = TopParent3D.transform.Find("UserOver");
        LeftOnHand = LeftParent3D.transform.Find("UserOnHand");
        LeftCPG = LeftParent3D.transform.Find("UserCPG");
        LeftChu = LeftParent3D.transform.Find("UserChu");
        LeftHu = LeftParent3D.transform.Find("UserHu");
        LeftOver = LeftParent3D.transform.Find("UserOver");
        RightOnHand = RightParent3D.transform.Find("UserOnHand");
        RightCPG = RightParent3D.transform.Find("UserCPG");
        RightChu = RightParent3D.transform.Find("UserChu");
        RightHu = RightParent3D.transform.Find("UserHu");
        RightOver = RightParent3D.transform.Find("UserOver");


        #endregion


        InitRoomData();
        InitPlayerData();
        ShowFangZhuImg();
        InitReconnection();
    }

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
            PlayerInfo info = GameDataFunc.GetPlayerInfo((byte)pos);
            if (isAi)
            {

                PlayerObjList[(int)info.LVD].transform.Find("AI").gameObject.SetActive(true);



            }
            else
            {
                PlayerObjList[(int)info.LVD].transform.Find("AI").gameObject.SetActive(false);

            }
            if (info.guid == Player.Instance.guid)
            {
                if (isAi)
                {
                    AiButton.transform.Find("AingSprite").gameObject.SetActive(true);
                }
                else
                {
                    AiButton.transform.Find("AingSprite").gameObject.SetActive(false);
                }
                // IsTuoGuan = isAi;
            }

            AiButton.gameObject.SetActive(true);
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
        SetButtonClickSound();
    }
    public void SetButtonClickSound()
    {
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


    /// <summary>
    /// 一局结束重置ai
    /// </summary>
    public void PartOverResetAI()
    {
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            GameData.m_PlayerInfoList[i].IsAi = false;
            OnPlayerAi((int)GameData.m_TableInfo.id, GameData.m_PlayerInfoList[i].pos, GameData.m_PlayerInfoList[i].IsAi);
        }
    }
    #endregion

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameEventDispatcher.Instance.removeEventListener(EventIndex.TingDropCardClick, this.ShowTingNotice);//听牌点击
        GameEventDispatcher.Instance.removeEventListener(EventIndex.GangCardClick, this.NoticeGang);//听牌点击
    }
    /// <summary>
    /// button点击事件
    /// </summary>
    /// <param name="go"></param>
    void OnClickBtn(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        switch (go.name)
        {
            case "btnInvite":
                ClientToServerMsg.Send(Opcodes.Client_PiPei_ChangeDesk, (byte)GameData.GlobleRoomType, GameData.m_TableInfo.id, Input.location.lastData.latitude, Input.location.lastData.longitude);
                //switch ((int)GameData.m_TableInfo.configPayIndex)
                //{
                //    case 0:
                //        AuthorizeOrShare.Instance.ShareRoomID(GameData.m_TableInfo.id, "一起来玩闲娱狗吧！" + ((int)GameData.m_TableInfo.configRoundIndex).ToString() + "局" + "  房主支付!", "闲娱狗栽宝");
                //        break;
                //    case 1:
                //        AuthorizeOrShare.Instance.ShareRoomID(GameData.m_TableInfo.id, "一起来玩闲娱狗吧！" + ((int)GameData.m_TableInfo.configRoundIndex).ToString() + "局" + "  平摊支付!", "闲娱狗栽宝");
                //        break;
                //}
                break;
            case "btnChat":
                UIManager.Instance.ShowUIPanel(UIPaths.ChatFace, OpenPanelType.MinToMax);
                break;
            case "btnSetting":
                UIManager.Instance.ShowUIPanel(UIPaths.SettingPanel2, OpenPanelType.MinToMax);
                // UIManager.Instance.ShowUIPanel(UIPaths.SettingPanel, OpenPanelType.MinToMax);
                break;
            case "btnReady":
                ClientToServerMsg.Send(Opcodes.Client_PlayerReadyStart, GameData.m_TableInfo.id);
                break;
            case "btnFP0":
            case "btnFP1":
            case "btnFP2":
            case "btnFP3":
            case "btnFP4":
                uint fpScore = uint.Parse(go.name.Substring(5));
                ClientToServerMsg.Send(Opcodes.Client_PlayerSetFangPaoScore, GameData.m_TableInfo.id, fpScore);
                break;
            case "btnGuo":
                // ClientToServerMsg.Send(Opcodes.Client_PlayerOperate, GameData.m_TableInfo.id, (byte)CardOperateType.GuoPai, 0);
                if (curOutWhoOperate == LocalPos)
                {
                    HideOperateBtn();
                }
                else
                {
                    List<uint> cardlist = new List<uint>();
                    ClientToServerMsg.SendWuDangHuGuo(cardlist);
                }

                break;
            case "btnHu":
                List<uint> Hucardlist = new List<uint>();
                ClientToServerMsg.SendWuDangHuHuPai(Hucardlist);
                //   ClientToServerMsg.Send(Opcodes.Client_PlayerOperate, GameData.m_TableInfo.id, (byte)CardOperateType.HuPai, outCardNumber);
                break;
            case "btnPeng":
                List<uint> cardlist1 = new List<uint>();
                cardlist1.Add(outCardNumber);
                ClientToServerMsg.SendWuDangHuPeng(cardlist1);
                // ClientToServerMsg.Send(Opcodes.Client_PlayerOperate, GameData.m_TableInfo.id, (byte)CardOperateType.PengPai, outCardNumber);
                break;
            case "btnGang":
                if (curOutWhoOperate == LocalPos)
                {
                    if (GetGangList().Count == 1)//只有一个杠
                    {
                        List<uint> cardlist2 = new List<uint>();
                        cardlist2.Add(GetGangList()[0]);
                        ClientToServerMsg.SendWuDangHuGang(cardlist2);
                    }
                    else
                    {
                        ShowGangPanel();
                    }

                    //  ClientToServerMsg.Send(Opcodes.Client_PlayerOperate, GameData.m_TableInfo.id, (byte)CardOperateType.GangPai, GetGangList()[0]);
                }
                else
                {
                    List<uint> cardlist3 = new List<uint>();
                    cardlist3.Add(outCardNumber);
                    ClientToServerMsg.SendWuDangHuGang(cardlist3);
                    //  ClientToServerMsg.Send(Opcodes.Client_PlayerOperate, GameData.m_TableInfo.id, (byte)CardOperateType.GangPai, outCardNumber);
                }
                break;
            case "BG":
                if (ObjPlayerChooseCard != null)
                {
                    ObjPlayerChooseCard.transform.localPosition = new Vector3(ObjPlayerChooseCard.transform.localPosition.x, 0, 0);
                    ObjPlayerChooseCard = null;
                    HideOutCards();



                }

                if (curOutWhoOperate == LocalPos)
                {
                    HideGangPanel();//隐藏杠那些牌的提示
                    AfterOperateCheckGangHu();
                }

                ResetTingNotice();
                break;
        }
    }


    #region   杠提示相关
    List<GameObject> ShowGangPanelList = new List<GameObject>();//杠提示生成的牌
    /// <summary>
    /// 展示杠牌提示
    /// </summary>
    public void ShowGangPanel()
    {
        GangPaiPanel.SetActive(true);
        GangPaiPanel.transform.localPosition = new Vector3(-0, -120, 0);
        HideOperateBtn();//影藏操作按钮

        GangPaiPanel.transform.Find("BgSprite").GetComponent<UISprite>().width = GetGangList().Count * 4 * 60;
        if (GetGangList().Count == 1)//基数个
        {
            for (int i = 0; i < 4; i++)
            {
                if (pbCardImage == null) pbCardImage = Resources.Load<GameObject>("Item/CardImage");
                GameObject g = GameObject.Instantiate(pbCardImage, GangPaiPanel.transform.Find("BgSprite"));
                g.transform.localScale = new Vector3(0.7f, 0.7f, 0);
                g.transform.localPosition = new Vector3(30 + 60 * i, 0, 0);
                g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + GetGangList()[0].ToString();
                g.name = GetGangList()[0].ToString();
                ShowGangPanelList.Add(g);
                UIEventListener.Get(g).onClick = GangCardClick;

            }
        }
        else if (GetGangList().Count == 2)
        {
            for (int i = 0; i < 8; i++)
            {
                if (pbCardImage == null) pbCardImage = Resources.Load<GameObject>("Item/CardImage");
                GameObject g = GameObject.Instantiate(pbCardImage, GangPaiPanel.transform.Find("BgSprite"));
                g.transform.localScale = new Vector3(0.7f, 0.7f, 0);

                if (i < 4)
                {
                    g.transform.localPosition = new Vector3(30 + 55 * i, 0, 0);
                    g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + GetGangList()[0].ToString();
                    g.name = GetGangList()[0].ToString();
                }
                else
                {
                    g.transform.localPosition = new Vector3(210 + 55 * (i - 3), 0, 0);
                    g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + GetGangList()[1].ToString();
                    g.name = GetGangList()[1].ToString();
                }



                ShowGangPanelList.Add(g);
                UIEventListener.Get(g).onClick = GangCardClick;
            }
        }


        else if (GetGangList().Count == 3)
        {
            GangPaiPanel.transform.localPosition = new Vector3(-200, -120, 0);
            for (int i = 0; i < 12; i++)
            {
                if (pbCardImage == null) pbCardImage = Resources.Load<GameObject>("Item/CardImage");
                GameObject g = GameObject.Instantiate(pbCardImage, GangPaiPanel.transform.Find("BgSprite"));
                g.transform.localScale = new Vector3(0.7f, 0.7f, 0);

                if (i < 4)
                {
                    g.transform.localPosition = new Vector3(30 + 55 * i, 0, 0);
                    g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + GetGangList()[0].ToString();
                    g.name = GetGangList()[0].ToString();
                }
                else if (i >= 4 && i < 8)
                {
                    g.transform.localPosition = new Vector3(210 + 55 * (i - 3), 0, 0);
                    g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + GetGangList()[1].ToString();
                    g.name = GetGangList()[1].ToString();
                }
                else
                {
                    g.transform.localPosition = new Vector3(450 + 55 * (i - 7), 0, 0);
                    g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + GetGangList()[2].ToString();
                    g.name = GetGangList()[2].ToString();
                }


                ShowGangPanelList.Add(g);
                UIEventListener.Get(g).onClick = GangCardClick;
            }
        }
    }

    /// <summary>
    /// 隐藏杠panel
    /// </summary>
    public void HideGangPanel()
    {
        if (ShowGangPanelList.Count != 0)
        {
            for (int i = 0; i < ShowGangPanelList.Count; i++)
            {
                Destroy(ShowGangPanelList[i]);
            }

        }
        ShowGangPanelList = new List<GameObject>();
        GangPaiPanel.SetActive(false);
    }

    /// <summary>
    /// 提示杠牌的点击
    /// </summary>
    /// <param name="go"></param>
    public void GangCardClick(GameObject go)
    {
        GameEventDispatcher.Instance.dispatchEvent(EventIndex.GangCardClick, go.name);
        HideGangPanel();
    }

    /// <summary>
    ///杠牌
    /// </summary>
    /// <param name="cardname"></param>
    private void NoticeGang(object cardname)
    {
        uint gangCard = uint.Parse(cardname.ToString());
        List<uint> cardlist = new List<uint>();
        cardlist.Add(gangCard);
        ClientToServerMsg.SendWuDangHuGang(cardlist);
    }

    /// <summary>
    /// 碰杠后的检测杠胡 还有听
    /// </summary>
    public void AfterOperateCheckGangHu()
    {
        bool isGang = false;
        PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalPos);
        //List<List<uint>> operateList = new List<List<uint>>();
        //for (int i = 0; i < info.operateCardList.Count; i++)
        //{
        //    List<uint> opCards = new List<uint>();
        //    if (info.operateCardList[i].opType == CatchType.Peng)
        //    {
        //        opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard);
        //    }
        //    else if (info.operateCardList[i].opType == CatchType.Gang || info.operateCardList[i].opType == CatchType.AnGang || info.operateCardList[i].opType == CatchType.BuGang)
        //    {
        //        opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard);
        //        // SelfHaveGone = true;
        //    }
        //    operateList.Add(opCards);
        //}

        //bool isHu = myXYHelper.Instance.ZBCheckHu(operateList, info.localCardList, GameData.m_TableInfo.MagicCard);
        if (GetGangList().Count > 0) isGang = true;
        int index = 0;
        if (isGang)
        {
            btnGuo.SetActive(true);
            btnGuo.transform.localPosition = new Vector3(-140 * index + 300, 0, 0);
            index++;
            btnGang.SetActive(true);
            btnGang.transform.localPosition = new Vector3(-140 * index + 300, 0, 0);
        }

        //  CheckTing();

    }

    #endregion



    /// <summary>
    /// 初始化房间数据
    /// </summary>
    void InitRoomData()
    {
        roomDesc = "";
        LBRoomID.text = "房间:" + GameData.m_TableInfo.id;
        LBGameCount.text = "局数:" + (GameData.m_TableInfo.curGameCount) + "/" + GameData.m_TableInfo.configRoundIndex;// gameRoundCount[GameData.m_TableInfo.configRoundIndex];
                                                                                                                     // LBFangchong3D.text = (GameData.m_TableInfo.configFangChongIndex == 0 ? "缺一不可放冲" : "缺一可以放冲");
                                                                                                                     //  LBShengpai3D.text = (GameData.m_TableInfo.configShengPaiIndex == 0 ? "不剩牌" : "剩7墩");
                                                                                                                     //  roomDesc += "打子:" + daZiString[GameData.m_TableInfo.configDaZiIndex] + "\n";
                                                                                                                     //  roomDesc += "人数:" + playerString[GameData.m_TableInfo.configPlayerIndex] + "\n";
        if (GameData.m_TableInfo.IsJIangMa)
        {
            JiangMa3d.text = "带奖码  ";
        }
        else
        {
            JiangMa3d.text = "不带奖码";
        }
        roomDesc += "支付:" + (GameData.m_TableInfo.configPayIndex == 0 ? "房主支付" : "房费均分");
        //LBRoomDesc.text = roomDesc.Replace("\\n", "\n");
        LBRoomDesc.text = roomDesc;

        //初始数据
        for (int i = 0; i < MJPaiParent.childCount; i++)
        {
            TableCardInfo tmpInfo = GameDataFunc.GetTableCardInfoByID(uint.Parse(MJPaiParent.GetChild(i).name));

            tmpInfo.ShownCount = 0;
            tmpInfo.RemainedCount = tmpInfo.TotalCount;

        }


        TotalCardsCount = 136;
        originalLeftOnDeskLocalPos = LeftOnDesk.transform.localPosition;//牌堆位置
        originalRightOnDeskLocalPos = RightOnDesk.transform.localPosition;
        originalSelfOnDeskLocalPos = SelfOnDesk.transform.localPosition;
        originalTopOnDeskLocalPos = TopOnDesk.transform.localPosition;
    }

    /// <summary>
    /// 初始玩家数据
    /// </summary>
    void InitPlayerData()
    {
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            if (GameData.m_PlayerInfoList[i].guid == Player.Instance.guid)
            {
                LocalPos = GameData.m_PlayerInfoList[i].pos;
                break;
            }
        }

        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            PlayerInfo info = GameData.m_PlayerInfoList[i];
            info.LVD = GetLVD(info.pos);//确定玩家在那个方位  right left up local
            GameObject obj = PlayerObjList[(int)info.LVD];
            obj.SetActive(true);
            obj.transform.Find("name").GetComponent<UILabel>().text = info.name;
            obj.transform.Find("score").GetComponent<UILabel>().text = info.score.ToString();
            obj.transform.Find("score").GetComponent<UILabel>().text = info.Gold.ToString();
            DownloadImage.Instance.Download(obj.transform.Find("headImage").GetComponent<UITexture>(), info.headID);
            if (GameData.m_TableInfo.roomState == RoomStatusType.None)
                obj.transform.Find("ready").gameObject.SetActive(info.isStartReady);
            else if (GameData.m_TableInfo.roomState == RoomStatusType.Over)
                obj.transform.Find("ready").gameObject.SetActive(info.isNextReady);
            obj.transform.Find("online").gameObject.SetActive(!info.isForce);

        }
        LocalViewSitDown();
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            HoldCardsObj obj = new HoldCardsObj();//无挡胡数据
            obj.pos = GameData.m_PlayerInfoList[i].pos;
            obj.LVD = GameData.m_PlayerInfoList[i].LVD;
            GameData.m_HoldCardsList.Add(obj);
        }
        SetLocations(0);//设置东南西北方位
    }

    /// <summary>
    /// s设置玩家的位置
    /// </summary>
    void LocalViewSitDown()
    {
        if (GameData.m_TableInfo.roomState == RoomStatusType.None)
        {
            if (GameData.m_TableInfo.configPlayerIndex == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    int index = LocalPos + i;
                    if (index > 2) index -= 2;
                    int pindex = index;
                    index = (int)GetLVD((byte)index);
                    if (pindex == LocalPos)
                    {
                        PlayerObjList[index].transform.localPosition = PlayerDownPos2[0];
                    }
                    else
                    {
                        PlayerObjList[index].transform.localPosition = PlayerDownPos2[1];
                    }
                }
            }
            else if (GameData.m_TableInfo.configPlayerIndex == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    int index = LocalPos + i;
                    if (index > 3) index -= 3;
                    int pindex = index;
                    index = (int)GetLVD((byte)index);
                    PlayerObjList[index].transform.localPosition = PlayerDownPos4[index];
                }
            }
            else
            {
                for (int i = 0; i < PlayerObjList.Count; i++)
                {
                    int index = LocalPos + i;
                    if (index > 4) index -= 4;
                    int pindex = index;
                    index = (int)GetLVD((byte)index);
                    PlayerObjList[index].transform.localPosition = PlayerDownPos4[index];
                }
            }
        }
        else
        {
            if (GameData.m_TableInfo.configPlayerIndex == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    int index = LocalPos + i;
                    if (index > 2) index -= 2;
                    int pindex = index;
                    index = (int)GetLVD((byte)index);
                    if (pindex == LocalPos)
                    {
                        PlayerObjList[index].transform.localPosition = PlayerStartPos2[0];
                    }
                    else
                    {
                        PlayerObjList[index].transform.localPosition = PlayerStartPos2[1];
                    }
                }
            }
            else if (GameData.m_TableInfo.configPlayerIndex == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    int index = LocalPos + i;
                    if (index > 3) index -= 3;
                    int pindex = index;
                    index = (int)GetLVD((byte)index);
                    PlayerObjList[index].transform.localPosition = PlayerStartPos4[index];
                }
            }
            else
            {
                for (int i = 0; i < PlayerObjList.Count; i++)
                {
                    int index = LocalPos + i;
                    if (index > 4) index -= 4;
                    int pindex = index;
                    index = (int)GetLVD((byte)index);
                    PlayerObjList[index].transform.localPosition = PlayerStartPos4[index];
                }
            }
        }

    }

    /// <summary>
    /// 设置东南西北方位  显示牌堆
    /// </summary>
    /// <param name="ActiveIndex"></param>
    void SetLocations(int ActiveIndex)
    {
        if (ActiveIndex == 0)
        {
            //RightOnDesk.SetActive(false);
            //TopOnDesk.SetActive(false);
            //LeftOnDesk.SetActive(false);
            //SelfOnDesk.SetActive(false);
            SetActiveChildPaiCount(RightOnDesk.transform, 34);
            SetActiveChildPaiCount(LeftOnDesk.transform, 34);
            SetActiveChildPaiCount(TopOnDesk.transform, 34);
            SetActiveChildPaiCount(SelfOnDesk.transform, 34);
            rightDeskStartStack = 17;
            leftDeskStartStack = 17;
            topDeskStartStack = 17;
            selfDeskStartStack = 17;
            for (int i = 0; i < LocationMarkParent.childCount; i++)//东南西北张图片
            {
                LocationMarkParent.GetChild(i).gameObject.SetActive(false);
            }
            switch (GameData.m_PlayerInfoList[0].LVD)//默认第一家为庄  
            {
                case LocalViewDirection.UP:
                    LocationMarkParent.localEulerAngles = new Vector3(90, 0, 0);

                    break;
                case LocalViewDirection.LOCAL:
                    LocationMarkParent.localEulerAngles = new Vector3(90, 180, 0);
                    break;
                case LocalViewDirection.RIGHT:
                    LocationMarkParent.localEulerAngles = new Vector3(90, 90, 0);
                    break;
                case LocalViewDirection.LEFT:
                    LocationMarkParent.localEulerAngles = new Vector3(90, 270, 0);
                    break;
            }
        }
        else if (ActiveIndex <= 4)
        {
            if (ActiveIndex == 2 && GameData.m_TableInfo.configPlayerIndex == 2)
            {
                ActiveIndex = 3;
            }
            for (int i = 1; i < 5; i++)
            {
                if (i == ActiveIndex)
                {
                    LocationMarkParent.Find(i.ToString()).gameObject.SetActive(true);
                }
                else
                {
                    LocationMarkParent.Find(i.ToString()).gameObject.SetActive(false);
                }
            }
        }
        else
        {

        }
    }

    /// <summary>
    /// 点击玩家头像
    /// </summary>
    /// <param name="go"></param>
    void OnClickPlayer(GameObject go)
    {
        if (!ServerInfo.Data.login_with_device)
            UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_Distance, OpenPanelType.MinToMax);
    }

    #region 牌的操作
    void OnDragEnd(GameObject go)
    {
        if (go.GetComponent<UISprite>().color == Color.gray) return;
        if (curOutWhoOperate != LocalPos || GameDataFunc.GetPlayerInfo(LocalPos).localCardList.Count % 3 == 1) return;
        float d = Math.Abs(Vector3.Distance(go.transform.localPosition, new Vector3(dragStartPos.x, 0, 0)));
        if (d > 50)
            OutCard(uint.Parse(go.name), go);
        else
            go.transform.localPosition = new Vector3(dragStartPos.x, 0, 0);
        ObjPlayerChooseCard = null;
    }
    void OnDrag(GameObject go, Vector2 delta)
    {
        if (go.GetComponent<UISprite>().color == Color.gray) return;
        if (curOutWhoOperate != LocalPos || GameDataFunc.GetPlayerInfo(LocalPos).localCardList.Count % 3 == 1) return;
        if (go == ObjPlayerChooseCard)
        {
            if (isCurDragOutCard)
                go.transform.localPosition += new Vector3(delta.x, delta.y);
            else
                OutCard(uint.Parse(go.name), go);
        }
        else
        {
            if (ObjPlayerChooseCard != null) ObjPlayerChooseCard.transform.localPosition = new Vector3(go.transform.localPosition.x, 0, 0);
            dragStartPos = go.transform.localPosition;
            isCurDragOutCard = true;
            ObjPlayerChooseCard = go;
        }
    }
    void OnClickCard(GameObject go)
    {
        if (go.GetComponent<UISprite>().color == Color.gray) return;
        if (curOutWhoOperate != LocalPos || GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.Count % 3 == 1) return;

        if (go == ObjPlayerChooseCard)
        {
            uint card = uint.Parse(go.name);
            OutCard(card, ObjPlayerChooseCard);
        }
        else
        {
            if (ObjPlayerChooseCard != null)
            {
                ObjPlayerChooseCard.transform.localPosition = new Vector3(ObjPlayerChooseCard.transform.localPosition.x, 0, ObjPlayerChooseCard.transform.localPosition.z);
            }

            ObjPlayerChooseCard = go;
            ShowOutCards(uint.Parse(go.name));

            if (IsTingPai)
            {
                bool clickTingCard = false;
                foreach (var item in TingDic)
                {
                    if (item.Key.ToString() == go.name)
                    {
                        GameEventDispatcher.Instance.dispatchEvent(EventIndex.TingDropCardClick, go.name);
                        clickTingCard = true;
                    }
                }
                if (!clickTingCard)
                {
                    ResetTingNotice();
                }
            }

            isDragOutCard = true;
            isCurDragOutCard = false;

        }

        #region

        //if (!IsTingPai)
        //{
        //    if (go == ObjPlayerChooseCard)
        //    {
        //        uint card = uint.Parse(go.name);
        //        OutCard(card, ObjPlayerChooseCard);
        //    }
        //}
        //else
        //{
        //    ObjPlayerChooseCard = go;
        //    if (!IsTingPai)//没有听牌
        //    {
        //        ShowOutCards(uint.Parse(go.name));
        //    }
        //    else//听牌后显示听那张牌
        //    {
        //        foreach (var item in TingDic)
        //        {
        //            if (item.Key.ToString() == go.name)
        //            {
        //                GameEventDispatcher.Instance.dispatchEvent(EventIndex.TingDropCardClick, go.name);
        //            }
        //        }

        //    }

        //    isDragOutCard = true;
        //    isCurDragOutCard = false;
        //    //   SoundManager.Instance.PlaySound(UIPaths.SOUND_CHOOSE);
        //}

        #endregion
    }
    void OnPressCard(GameObject go, bool state)
    {
        if (go.GetComponent<UISprite>().color == Color.gray) return;
        if (curOutWhoOperate != LocalPos || GameDataFunc.GetPlayerInfo(LocalPos).localCardList.Count % 3 == 1) return;
        if (go == ObjPlayerChooseCard) return;
        if (ObjPlayerChooseCard != null)
        {
            ObjPlayerChooseCard.transform.localPosition = new Vector3(ObjPlayerChooseCard.transform.localPosition.x, 0);
        }
        if (!isCurDragOutCard)
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, 20);
        isCurDragOutCard = state;
    }

    #endregion

    #region 断线重连
    void InitReconnection()
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
                RoomPlay();
                RoomOver();
                break;
        }
    }

    void RoomNone()
    {
        PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalPos);
        if (info.isStartReady)
        {
            btnReady.GetComponent<BoxCollider2D>().enabled = false;
            btnReady.GetComponent<UISprite>().spriteName = "UI_game_btn_ready_Pressed";
        }
    }
    void RoomPlay()
    {
        btnInvite.SetActive(false);
        btnReady.SetActive(false);
        ShowFangPaoScore();
        IsDealCardOver = true;
        ShowMakerImg();
        if (GameData.m_TableInfo.isQueryLeaveRoom) UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_AskDismissRoom);

        DestroyAllOutCards();//清理所有桌面上的出牌
        SetActiveChildPaiCount(SelfChu, 0);
        SetActiveChildPaiCount(LeftChu, 0);
        SetActiveChildPaiCount(RightChu, 0);
        SetActiveChildPaiCount(TopChu, 0);

        if (GameData.m_TableInfo.isInCardInfo && !GameData.m_TableInfo.isWaitFangPao)
        {
            InCardPos = GameData.m_TableInfo.inCardPos;
            InCardNumber = GameData.m_TableInfo.inCardNumber;
            GameDataFunc.RemoverHoldCard(InCardNumber, InCardPos);
        }
        //本地设置
        PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL);

        #region 把万能牌放在最左边
        List<uint> MagicCardList = new List<uint>();
        List<uint> OtherCardList = new List<uint>();
        for (int i = 0; i < GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.Count; i++)
        {
            if (GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList[i] == GameData.m_TableInfo.MagicCard)
            {
                MagicCardList.Add(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList[i]);
            }
            else
            {
                OtherCardList.Add(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList[i]);
            }
        }
        OtherCardList.Sort((a, b) =>
        {

            return ((int)a - (int)b);
        });


        GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList = new List<uint>();
        GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.AddRange(MagicCardList);
        GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.AddRange(OtherCardList);
        #endregion

        if (pbCardImage == null) pbCardImage = Resources.Load<GameObject>("Item/CardImage");
        int startPos = (int)LocalLeftCardStartPos + (13 - info.localCardList.Count) * 75;
        //info.localCardList.Sort((a, b) =>
        //{
        //    return (int)a - (int)b;
        //});
        for (int j = 0; j < info.localCardList.Count; j++)//本地牌
        {
            GameObject card = Instantiate<GameObject>(pbCardImage);
            card.transform.parent = LocalParent;
            card.transform.localScale = Vector3.one;
            card.transform.localPosition = new Vector3(startPos + j * 75, 0, 0);
            card.name = info.localCardList[j].ToString();
            card.AddComponent<BoxCollider2D>().size = new Vector3(75, 122, 0);
            // card.GetComponent<UISprite>().spriteName = (GameData.m_TableInfo.curGameCount % 2 == 0) ? "card_local_max_" + info.localCardList[j].ToString() : "cardblue_local_max_" + info.localCardList[j].ToString();
            card.GetComponent<UISprite>().spriteName = "card_local_max_" + info.localCardList[j].ToString();

            //如果是面牌  标红
            if (info.localCardList[j] == GameData.m_TableInfo.MagicCard)
            {
                card.GetComponent<UISprite>().color = new Color(1, 0.8f, 0.8f, 1);
            }

            UIEventListener.Get(card).onPress = OnPressCard;
            UIEventListener.Get(card).onClick = OnClickCard;
            UIEventListener.Get(card).onDrag = OnDrag;
            UIEventListener.Get(card).onDragEnd = OnDragEnd;
            GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Add(card);
            GameDataFunc.GetTableCardInfoByID(info.localCardList[j]).RemainedCount -= 1;
        }
        for (int i = 0; i < info.outCardList.Count; i++)
        {
            GameObject item = null;
            item = Instantiate<GameObject>(MJPaiParent.Find(info.outCardList[i].ToString()).gameObject);
            HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL);
            SetActiveChildPaiCount(SelfChu, i + 1);
            StartCoroutine(AsyncSetMJPai(SelfChu.Find(i.ToString()).Find("Content"), item));
            item.name = info.outCardList[i].ToString();
            GameDataFunc.GetTableCardInfoByID(info.outCardList[i]).RemainedCount -= 1;
            infoObj.outObjList.Add(item);
        }
        for (int i = 0; i < info.operateCardList.Count; i++)
        {
            switch (info.operateCardList[i].opType)
            {

                case CatchType.AnGang:
                    StartCoroutine(AsynPengGang(info.guid, GetPengGangType(info.operateCardList[i].opType), info.operateCardList[i].opCard, info.operateCardList[i].pos, true, false));
                    break;
                case CatchType.Peng:
                case CatchType.Gang:
                case CatchType.BuGang:
                    StartCoroutine(AsynPengGang(info.guid, GetPengGangType(info.operateCardList[i].opType), info.operateCardList[i].opCard, info.operateCardList[i].pos, false, false));
                    break;
            }
        }

        CreatPlayerSendMianCard(info);//重置打出的面牌
        //if (GameData.m_TableInfo.configPlayerIndex == 1 || GameData.m_TableInfo.configPlayerIndex == 0)
        //{
        info = GameDataFunc.GetPlayerInfo(LocalViewDirection.RIGHT);
        if (info != null)
        {
            for (int j = 0; j < info.localCardList.Count; j++)//下家
            {
                GameObject card = RightOnHand.Find(j.ToString()).gameObject;
                SetActiveChildPaiCount(RightOnHand, j + 1);
                GameDataFunc.GetHoldCardObj(LocalViewDirection.RIGHT).holdObjList.Add(card);
            }


            for (int i = 0; i < info.outCardList.Count; i++)
            {
                GameObject item = null;
                item = Instantiate<GameObject>(MJPaiParent.Find(info.outCardList[i].ToString()).gameObject);
                HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(LocalViewDirection.RIGHT);
                SetActiveChildPaiCount(RightChu, i + 1);
                StartCoroutine(AsyncSetMJPai(RightChu.Find(i.ToString()).Find("Content"), item));
                item.name = info.outCardList[i].ToString();
                infoObj.outObjList.Add(item);
                GameDataFunc.GetTableCardInfoByID(info.outCardList[i]).RemainedCount -= 1;
            }
            for (int i = 0; i < info.operateCardList.Count; i++)
            {
                switch (info.operateCardList[i].opType)
                {

                    case CatchType.AnGang:
                        StartCoroutine(AsynPengGang(info.guid, GetPengGangType(info.operateCardList[i].opType), info.operateCardList[i].opCard, info.operateCardList[i].pos, true, false));
                        break;
                    case CatchType.Peng:
                    case CatchType.Gang:
                    case CatchType.BuGang:
                        StartCoroutine(AsynPengGang(info.guid, GetPengGangType(info.operateCardList[i].opType), info.operateCardList[i].opCard, info.operateCardList[i].pos, false, false));
                        break;
                }

            }

            CreatPlayerSendMianCard(info);//重置打出的面牌
        }
        //}
        info = GameDataFunc.GetPlayerInfo(LocalViewDirection.UP);
        if (info != null)
        {
            for (int j = 0; j < info.localCardList.Count; j++)//对家
            {
                GameObject card = TopOnHand.Find(j.ToString()).gameObject;
                SetActiveChildPaiCount(TopOnHand, j + 1);
                GameDataFunc.GetHoldCardObj(LocalViewDirection.UP).holdObjList.Add(card);
            }
            for (int i = 0; i < info.outCardList.Count; i++)
            {
                GameObject item = null;
                item = Instantiate<GameObject>(MJPaiParent.Find(info.outCardList[i].ToString()).gameObject);

                HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(LocalViewDirection.UP);
                SetActiveChildPaiCount(TopChu, i + 1);
                StartCoroutine(AsyncSetMJPai(TopChu.Find(i.ToString()).Find("Content"), item));

                item.name = info.outCardList[i].ToString();
                infoObj.outObjList.Add(item);
                GameDataFunc.GetTableCardInfoByID(info.outCardList[i]).RemainedCount -= 1;
            }
            for (int i = 0; i < info.operateCardList.Count; i++)
            {
                switch (info.operateCardList[i].opType)
                {
                    case CatchType.AnGang:
                        StartCoroutine(AsynPengGang(info.guid, GetPengGangType(info.operateCardList[i].opType), info.operateCardList[i].opCard, info.operateCardList[i].pos, true, false));
                        break;
                    case CatchType.Peng:
                    case CatchType.Gang:
                    case CatchType.BuGang:
                        StartCoroutine(AsynPengGang(info.guid, GetPengGangType(info.operateCardList[i].opType), info.operateCardList[i].opCard, info.operateCardList[i].pos, false, false));
                        break;
                }
            }

            CreatPlayerSendMianCard(info);//重置打出的面牌
        }
        //if (GameData.m_TableInfo.configPlayerIndex == 0)
        //{
        info = GameDataFunc.GetPlayerInfo(LocalViewDirection.LEFT);
        if (info != null)
        {
            for (int j = 0; j < info.localCardList.Count; j++)//上家
            {
                GameObject card = LeftOnHand.Find(j.ToString()).gameObject;
                SetActiveChildPaiCount(LeftOnHand, j + 1);
                GameDataFunc.GetHoldCardObj(LocalViewDirection.LEFT).holdObjList.Add(card);
            }
            for (int i = 0; i < info.outCardList.Count; i++)
            {
                GameObject item = null;
                item = Instantiate<GameObject>(MJPaiParent.Find(info.outCardList[i].ToString()).gameObject);
                HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(LocalViewDirection.LEFT);
                SetActiveChildPaiCount(LeftChu, i + 1);
                StartCoroutine(AsyncSetMJPai(LeftChu.Find(i.ToString()).Find("Content"), item));
                item.name = info.outCardList[i].ToString();
                infoObj.outObjList.Add(item);
                GameDataFunc.GetTableCardInfoByID(info.outCardList[i]).RemainedCount -= 1;
            }
            for (int i = 0; i < info.operateCardList.Count; i++)
            {
                switch (info.operateCardList[i].opType)
                {

                    case CatchType.AnGang:
                        StartCoroutine(AsynPengGang(info.guid, GetPengGangType(info.operateCardList[i].opType), info.operateCardList[i].opCard, info.operateCardList[i].pos, true, false));
                        break;
                    case CatchType.Peng:
                    case CatchType.BuGang:
                    case CatchType.Gang:
                        StartCoroutine(AsynPengGang(info.guid, GetPengGangType(info.operateCardList[i].opType), info.operateCardList[i].opCard, info.operateCardList[i].pos, false, false));
                        break;
                }
            }

            CreatPlayerSendMianCard(info);//重置打出的面牌
        }
        //}
        if (GameData.m_TableInfo.isWaitFangPao)
        {
            if (!GameDataFunc.GetPlayerInfo(LocalPos).isOperateFangPao)
            {
                // onWaitPlayerChooseScore();
            }
        }
        else
        {
            if (GameData.m_TableInfo.lastOutCardPos != 0)
            {
                StartCoroutine(AsyncSetOutSignMark());
            }
            InitCardsOnDesk();//吧所有的牌放到AllCardsOnDesk中去  （LeftOnDesk。。）
            ResCardCount = GameData.m_TableInfo.resCardCount;//排座上剩余牌数



            if (GameData.m_TableInfo.isInCardInfo)
            {
                curOutWhoOperate = InCardPos;
                CreateInCard(InCardPos, InCardNumber);
                Debug.Log(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.Count);
                if (InCardPos == LocalPos)
                    ShowLocalOperateState(InCardPos, InCardNumber);
            }
            if (GameData.m_TableInfo.isOutCardInfo)
            {
                curOutWhoOperate = GameData.m_TableInfo.outCardPos;
                outCardNumber = GameData.m_TableInfo.outCardNumber;
                ShowLocalOperateState(curOutWhoOperate, outCardNumber, false);
            }
            if (GameData.m_TableInfo.waitOutCardPos != 0)
            {
                curOutWhoOperate = GameData.m_TableInfo.waitOutCardPos;
                ShowOutCardPlayer(GameData.m_TableInfo.waitOutCardPos);

                if (LocalPos == GameData.m_TableInfo.waitOutCardPos)//该自己出牌  看看需不需要杠
                {
                    AfterOperateCheckGangHu();//
                    if (btnHu.activeSelf != true)
                    {
                        CheckTing();
                    }
                }
            }
        }

        //重置骰子
        SetDice(Dice1, GameData.Dice3);
        SetDice(Dice2, GameData.Dice4);
        DiceParent.gameObject.SetActive(false);
        Debug.Log(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.Count);

        ResetPlayerAI();
    }
    void RoomOver()
    {
        btnInvite.SetActive(false);
        btnReady.SetActive(false);
        //   ShowFangPaoScore();
        ViewHu();
        StartCoroutine(AsynCreateOtherCards());
        PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalPos);
        if (!info.isNextReady) UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_RoundOver);
    }
    #endregion

    #region 服务器回调




    /// <summary>
    /// 玩家进入
    /// </summary>
    /// <param name="info"></param>
    public void onPlayerEnter(PlayerInfo info,bool ReduceGold=false)
    {
        info.LVD = GetLVD(info.pos);
        GameObject obj = PlayerObjList[(int)info.LVD];
        obj.SetActive(true);
        obj.transform.Find("name").GetComponent<UILabel>().text = info.name;
        obj.transform.Find("score").GetComponent<UILabel>().text = info.score.ToString();
        obj.transform.Find("score").GetComponent<UILabel>().text = info.Gold.ToString();
        DownloadImage.Instance.Download(obj.transform.Find("headImage").GetComponent<UITexture>(), info.headID);

        Log.Debug(Enum.GetName(typeof(LocalViewDirection), info.LVD));
        HoldCardsObj objHold = new HoldCardsObj();
        objHold.pos = info.pos;
        objHold.LVD = info.LVD;
        if(!ReduceGold)
        GameData.m_HoldCardsList.Add(objHold);
        //  SoundManager.Instance.PlaySound(UIPaths.SOUND_SITDOWN);
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
        PlayerObjList[(int)info.LVD].transform.Find("ready").gameObject.SetActive(info.isStartReady);
        if (info.pos == LocalPos)
        {
            btnReady.GetComponent<BoxCollider2D>().enabled = false;
            btnReady.GetComponent<UISprite>().spriteName = "UI_game_btn_ready_Pressed";
        }
        SoundManager.Instance.PlaySound(UIPaths.SOUND_READY);
    }

    /// <summary>
    /// 准备下一局
    /// </summary>
    /// <param name="pos"></param>
    public void onPlayerReadyForNext(byte pos)
    {
        if (pos == LocalPos) UIManager.Instance.HideUIPanel(UIPaths.UIPanel_RoundOver);
        PlayerObjList[(int)GetLVD(pos)].transform.Find("ready").gameObject.SetActive(true);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_READY);
    }

    /// <summary>
    /// 房间激活
    /// </summary>
    public void onRoomActive()
    {
        btnInvite.SetActive(false);
        btnReady.SetActive(false);
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            PlayerObjList[(int)GameData.m_PlayerInfoList[i].LVD].transform.Find("ready").gameObject.SetActive(false);
        }
        GameData.m_TableInfo.roomState = RoomStatusType.Active;


        if (GameData.m_TableInfo.configPlayerIndex == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                int index = LocalPos + i;
                if (index > 2) index -= 2;
                int pindex = index;
                index = (int)GetLVD((byte)index);
                if (pindex == LocalPos)
                {
                    TweenPosition.Begin(PlayerObjList[index], 0.2f, PlayerStartPos2[0]);
                }
                else
                {
                    TweenPosition.Begin(PlayerObjList[index], 0.2f, PlayerStartPos2[1]);
                }
            }
        }
        else if (GameData.m_TableInfo.configPlayerIndex == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                int index = LocalPos + i;
                if (index > 3) index -= 3;
                int pindex = index;
                index = (int)GetLVD((byte)index);
                TweenPosition.Begin(PlayerObjList[index], 0.2f, PlayerStartPos4[index]);
            }
        }
        else
        {
            for (int i = 0; i < PlayerObjList.Count; i++)
            {
                int index = LocalPos + i;
                if (index > 4) index -= 4;
                int pindex = index;
                index = (int)GetLVD((byte)index);
                TweenPosition.Begin(PlayerObjList[index], 0.2f, PlayerStartPos4[index]);
            }
        }
    }

    /// <summary>
    /// 庄的位置
    /// </summary>
    public void onZhuangPosition()
    {
        LBGameCount.text = "局数:" + GameData.m_TableInfo.curGameCount + "/" + GameData.m_TableInfo.configRoundIndex;// gameRoundCount[GameData.m_TableInfo.configRoundIndex];
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            PlayerObjList[(int)GameData.m_PlayerInfoList[i].LVD].transform.Find("ready").gameObject.SetActive(false);
        }
        ClearTable();
        ShowMakerImg();
    }

    #region  打骰子操作  add by tj
    /// <summary>
    /// 骰子的展示  
    /// </summary>
    /// <param name="decOne">骰子</param>
    /// <param name="deTwo"></param>
    /// <param name="MianCard">面牌</param>
    /// <param name="MagicCard">宝</param>

    public GameObject MianCardPanel;
    public void onDeciesShow(uint decOne, uint deTwo, uint decThree, uint deFour, uint MianCard, uint MagicCard)
    {
        GameData.m_TableInfo.ForbiddenIndex = 135 - ((int)(decThree + deFour) * 2) - 1;

        InitCardsOnDesk();// 把桌上的牌放到 AllCardsOnDesk中
        StartCoroutine(AsynShowTableCard());

        // CreatMianCard();
    }

    private GameObject MianPaiCard;//面牌
    /// <summary>
    /// 吧面牌翻过来
    /// </summary>
    public void CreatMianCard()
    {
        if (MianPaiCard == null)
        {
            MianPaiCard = Instantiate<GameObject>(MJPaiParent.Find(GameData.m_TableInfo.MianCard.ToString()).gameObject);
        }
        else
        {
            Destroy(MianPaiCard);
            MianPaiCard = Instantiate<GameObject>(MJPaiParent.Find(GameData.m_TableInfo.MianCard.ToString()).gameObject);
        }


        MianPaiCard.SetActive(true);
        MianPaiCard.transform.SetParent(AllCardsOnDesk[GameData.m_TableInfo.ForbiddenIndex].transform.parent);
        MianPaiCard.transform.localScale = AllCardsOnDesk[GameData.m_TableInfo.ForbiddenIndex].transform.localScale;
        MianPaiCard.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        MianPaiCard.transform.localRotation = AllCardsOnDesk[GameData.m_TableInfo.ForbiddenIndex].transform.localRotation;
        MianPaiCard.transform.localPosition = AllCardsOnDesk[GameData.m_TableInfo.ForbiddenIndex].transform.localPosition;
        MianPaiCard.transform.localEulerAngles = new Vector3(MianPaiCard.transform.localEulerAngles.x, 180, MianPaiCard.transform.localEulerAngles.z);
        AllCardsOnDesk[GameData.m_TableInfo.ForbiddenIndex].SetActive(false);

        MianPaiCard.transform.SetParent(MJPaiParent);
        MianPaiCard.SetActive(true);

    }

    //展现桌上的牌
    IEnumerator AsynShowTableCard()
    {

        #region 挪开挡板
        SoundManager.Instance.PlaySound(UIPaths.SOUND_START_GAME);
        //挪开挡板
        TweenPosition.Begin(TopLift, 0.5f, new Vector3(0, 0, -0.04f));
        TweenPosition.Begin(SelfLift, 0.5f, new Vector3(0, 0, -0.04f));
        TweenPosition.Begin(LeftLift, 0.5f, new Vector3(0, 0, -0.04f));
        TweenPosition.Begin(RightLift, 0.5f, new Vector3(0, 0, -0.04f));
        yield return new WaitForSeconds(0.5f);
        SetLocations(0);//东南西北


        //重置牌堆，并将牌堆移上挡板
        SelfOnDesk.gameObject.SetActive(true);
        RightOnDesk.gameObject.SetActive(true);
        TopOnDesk.gameObject.SetActive(true);
        LeftOnDesk.gameObject.SetActive(true);
        SelfOnDesk.transform.localPosition = new Vector3(0.2f, 0, 0.02f);
        RightOnDesk.transform.localPosition = new Vector3(0.2f, 0, 0.02f);
        TopOnDesk.transform.localPosition = new Vector3(0.2f, 0, 0.02f);
        LeftOnDesk.transform.localPosition = new Vector3(0.2f, 0, 0.02f);

        TweenPosition.Begin(SelfOnDesk, 0.5f, new Vector3(originalSelfOnDeskLocalPos.x, originalSelfOnDeskLocalPos.y, 0.017f));
        TweenPosition.Begin(RightOnDesk, 0.5f, new Vector3(originalRightOnDeskLocalPos.x, originalRightOnDeskLocalPos.y, 0.017f));
        TweenPosition.Begin(LeftOnDesk, 0.5f, new Vector3(originalLeftOnDeskLocalPos.x, originalLeftOnDeskLocalPos.y, 0.017f));
        TweenPosition.Begin(TopOnDesk, 0.5f, new Vector3(originalTopOnDeskLocalPos.x, originalTopOnDeskLocalPos.y, 0.017f));

        yield return new WaitForSeconds(0.5f);

        //将挡板和麻将牌一起上移
        // ResCardCount = (uint)TotalCardsCount;
        SetCardOnDesk();
        TweenPosition.Begin(SelfOnDesk, 0.5f, new Vector3(originalSelfOnDeskLocalPos.x, originalSelfOnDeskLocalPos.y, 0.065f));
        TweenPosition.Begin(RightOnDesk, 0.5f, new Vector3(originalRightOnDeskLocalPos.x, originalRightOnDeskLocalPos.y, 0.065f));
        TweenPosition.Begin(LeftOnDesk, 0.5f, new Vector3(originalLeftOnDeskLocalPos.x, originalLeftOnDeskLocalPos.y, 0.065f));
        TweenPosition.Begin(TopOnDesk, 0.5f, new Vector3(originalTopOnDeskLocalPos.x, originalTopOnDeskLocalPos.y, 0.065f));
        TweenPosition.Begin(TopLift, 0.5f, Vector3.zero);
        TweenPosition.Begin(SelfLift, 0.5f, Vector3.zero);
        TweenPosition.Begin(LeftLift, 0.5f, Vector3.zero);
        TweenPosition.Begin(RightLift, 0.5f, Vector3.zero);
        yield return new WaitForSeconds(1f);

        #endregion


        #region  打骰子
        DiceParent.gameObject.SetActive(true);
        Dice1.gameObject.SetActive(true);
        Dice2.gameObject.SetActive(true);

        DiceParent.localEulerAngles = Vector3.zero;
        iTween.RotateFrom(DiceParent.gameObject, iTween.Hash("y", -180, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        Dice1.transform.localPosition = new Vector3(0.2f, 0, 0);

        Dice2.transform.localPosition = new Vector3(-0.2f, 0, 0);
        iTween.MoveFrom(Dice1, iTween.Hash("x", 0.875, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        iTween.MoveFrom(Dice2, iTween.Hash("x", -0.875, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        iTween.RotateBy(Dice1, iTween.Hash("x", UnityEngine.Random.Range(540, 1080), "y", UnityEngine.Random.Range(540, 1080), "z", UnityEngine.Random.Range(540, 1080), "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        iTween.RotateBy(Dice2, iTween.Hash("x", UnityEngine.Random.Range(540, 1080), "y", UnityEngine.Random.Range(540, 1080), "z", UnityEngine.Random.Range(540, 1080), "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        OutCountDown.SetActive(false);
        yield return new WaitForSeconds(1.05f);
        SetDice(Dice1, GameData.Dice1);
        SetDice(Dice2, GameData.Dice2);

        ShowMianCard();//显示面牌
        CreatMianCard();
        yield return new WaitForSeconds(2f);


        //第二次打骰子
        DiceParent.localEulerAngles = Vector3.zero;
        iTween.RotateFrom(DiceParent.gameObject, iTween.Hash("y", -180, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        Dice1.transform.localPosition = new Vector3(0.2f, 0, 0);

        Dice2.transform.localPosition = new Vector3(-0.2f, 0, 0);
        iTween.MoveFrom(Dice1, iTween.Hash("x", 0.875, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        iTween.MoveFrom(Dice2, iTween.Hash("x", -0.875, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        iTween.RotateBy(Dice1, iTween.Hash("x", UnityEngine.Random.Range(540, 1080), "y", UnityEngine.Random.Range(540, 1080), "z", UnityEngine.Random.Range(540, 1080), "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        iTween.RotateBy(Dice2, iTween.Hash("x", UnityEngine.Random.Range(540, 1080), "y", UnityEngine.Random.Range(540, 1080), "z", UnityEngine.Random.Range(540, 1080), "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        OutCountDown.SetActive(false);
        yield return new WaitForSeconds(1.05f);
        SetDice(Dice1, GameData.Dice3);
        SetDice(Dice2, GameData.Dice4);


        yield return new WaitForSeconds(2f);
        #endregion

        #region  发牌
        if (CanDealCard)//是否传了发牌协议
        {
            int inCardCount = 3;
            int cardCount = 4;

            int index = 0;
            int indexRight = 0;
            int indexUp = 0;
            int indexLeft = 0;
            int tmpGivenCards = 0;
            if (pbCardImage == null) pbCardImage = Resources.Load<GameObject>("Item/CardImage");
            for (int i = 0; i < inCardCount; i++)//发牌 发3把牌
            {
                #region 发牌
                for (int j = 0; j < cardCount; j++)//本地牌 4张4张的发
                {
                    GameObject card = Instantiate<GameObject>(pbCardImage);
                    card.transform.parent = LocalParent;
                    card.transform.localScale = Vector3.one;
                    card.transform.localPosition = new Vector3((int)LocalLeftCardStartPos + index * 75, 0, 0);
                    card.name = index.ToString();
                    GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Add(card);
                    index++;
                }
                SoundManager.Instance.PlaySound(UIPaths.SOUND_FAPAI);
                yield return new WaitForSeconds(0.2f);
                //赋值
                int count = GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Count / cardCount - 1;
                for (int k = 0; k < cardCount; k++)
                {
                    int indexCount = count * cardCount + k;
                    GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[indexCount].GetComponent<UISprite>().spriteName = "card_local_max_" + GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList[indexCount];
                }
                tmpGivenCards += cardCount;
                ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
                //if (GameData.m_TableInfo.configPlayerIndex == 0 || GameData.m_TableInfo.configPlayerIndex == 1)
                //{


                yield return new WaitForSeconds(0.1f);

                if (GameDataFunc.GetPlayerInfo(LocalViewDirection.RIGHT) != null)
                {
                    for (int j = 0; j < cardCount; j++)//下家
                    {
                        GameObject card = RightOnHand.Find(indexRight.ToString()).gameObject;
                        SetActiveChildPaiCount(RightOnHand, indexRight + 1);
                        GameDataFunc.GetHoldCardObj(LocalViewDirection.RIGHT).holdObjList.Add(card);
                        indexRight++;
                    }
                    tmpGivenCards += cardCount;
                    ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
                    yield return new WaitForSeconds(0.1f);
                    //}
                }
                if (GameDataFunc.GetPlayerInfo(LocalViewDirection.UP) != null)
                {
                    for (int j = 0; j < cardCount; j++)//对家
                    {
                        GameObject card = TopOnHand.Find(indexUp.ToString()).gameObject;
                        SetActiveChildPaiCount(TopOnHand, indexUp + 1);
                        GameDataFunc.GetHoldCardObj(LocalViewDirection.UP).holdObjList.Add(card);
                        indexUp++;
                    }
                    tmpGivenCards += cardCount;
                    ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);

                    //if (GameData.m_TableInfo.configPlayerIndex == 0)
                    //{
                    yield return new WaitForSeconds(0.1f);
                }
                if (GameDataFunc.GetPlayerInfo(LocalViewDirection.LEFT) != null)
                {
                    for (int j = 0; j < cardCount; j++)//上家
                    {
                        GameObject card = LeftOnHand.Find(indexLeft.ToString()).gameObject;
                        SetActiveChildPaiCount(LeftOnHand, indexLeft + 1);
                        GameDataFunc.GetHoldCardObj(LocalViewDirection.LEFT).holdObjList.Add(card);
                        indexLeft++;
                    }
                    tmpGivenCards += cardCount;
                    ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
                    //}
                    yield return new WaitForSeconds(0.1f);
                }
                #endregion
            }


            #region  最后一张牌的处理
            GameObject card1 = Instantiate<GameObject>(pbCardImage);//自己的最后一张牌
            card1.transform.parent = LocalParent;
            card1.transform.localScale = Vector3.one;
            card1.transform.localPosition = new Vector3((int)LocalLeftCardStartPos + index * 75, 0, 0);
            tmpGivenCards += 1;
            ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
            yield return new WaitForSeconds(0.2f);
            card1.GetComponent<UISprite>().spriteName = "card_local_max_back";
            GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Add(card1);
            //if (GameData.m_TableInfo.configPlayerIndex == 0 || GameData.m_TableInfo.configPlayerIndex == 1)
            //{
            if (GameDataFunc.GetPlayerInfo(LocalViewDirection.RIGHT) != null)
            {
                GameObject card = RightOnHand.Find(indexRight.ToString()).gameObject;
                SetActiveChildPaiCount(RightOnHand, indexRight + 1);
                GameDataFunc.GetHoldCardObj(LocalViewDirection.RIGHT).holdObjList.Add(card);
                tmpGivenCards += 1;
                ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
                //}
                yield return new WaitForSeconds(0.1f);
            }
            if (GameDataFunc.GetPlayerInfo(LocalViewDirection.UP) != null)
            {
                GameObject card2 = TopOnHand.Find(indexUp.ToString()).gameObject;
                SetActiveChildPaiCount(TopOnHand, indexUp + 1);
                GameDataFunc.GetHoldCardObj(LocalViewDirection.UP).holdObjList.Add(card2);
                tmpGivenCards += 1;
                ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
                yield return new WaitForSeconds(0.1f);
            }
            //if (GameData.m_TableInfo.configPlayerIndex == 0)
            //{
            if (GameDataFunc.GetPlayerInfo(LocalViewDirection.LEFT) != null)
            {
                GameObject card3 = LeftOnHand.Find(indexLeft.ToString()).gameObject;
                SetActiveChildPaiCount(LeftOnHand, indexLeft + 1);
                GameDataFunc.GetHoldCardObj(LocalViewDirection.LEFT).holdObjList.Add(card3);
                tmpGivenCards += 1;
                ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
                yield return new WaitForSeconds(0.1f);
                //}
            }

            #endregion


            #region 把万能牌放在最左边
            List<uint> MagicCardList = new List<uint>();
            List<uint> OtherCardList = new List<uint>();
            for (int i = 0; i < GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.Count; i++)
            {
                if (GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList[i] == GameData.m_TableInfo.MagicCard)
                {
                    MagicCardList.Add(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList[i]);
                }
                else
                {
                    OtherCardList.Add(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList[i]);
                }
            }
            OtherCardList.Sort((a, b) =>
            {

                return ((int)a - (int)b);
            });


            GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList = new List<uint>();
            GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.AddRange(MagicCardList);
            GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.AddRange(OtherCardList);
            #endregion

            //GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.Sort((a, b) =>
            //{
            //    return ((int)a - (int)b);
            //});
            for (int m = 0; m < GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Count; m++)
            {
                GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[m].GetComponent<UISprite>().spriteName = "card_local_max_back";
            }
            yield return new WaitForSeconds(0.2f);
            for (int m = 0; m < GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Count; m++)
            {
                GameObject obj = GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[m];
                PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL);
                obj.name = info.localCardList[m].ToString();
                obj.GetComponent<UISprite>().spriteName = "card_local_max_" + info.localCardList[m].ToString();

                //如果是面牌  标红
                if (info.localCardList[m] == GameData.m_TableInfo.MagicCard)
                {
                    obj.GetComponent<UISprite>().color = new Color(1, 0.8f, 0.8f, 1);
                }

                GameDataFunc.GetTableCardInfoByID(info.localCardList[m]).RemainedCount -= 1;//剩余牌
            }
            IsDealCardOver = true;// GameData.m_TableInfo.makerPos 发牌结束



            CreateInCard(InCardPos, InCardNumber);//庄进张赋值
            if (InCardPos == LocalPos) ShowLocalOperateState(InCardPos, InCardNumber);
            for (int i = 0; i < GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Count; i++)
            {
                GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[i].AddComponent<BoxCollider2D>().size = new Vector3(75, 122, 0);
                GameObject obj = GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[i];
                UIEventListener.Get(obj).onPress = OnPressCard;
                UIEventListener.Get(obj).onClick = OnClickCard;
                UIEventListener.Get(obj).onDrag = OnDrag;
                UIEventListener.Get(obj).onDragEnd = OnDragEnd;
            }
            ResCardCount = GameData.m_TableInfo.resCardCount;
            DiceParent.gameObject.SetActive(false);

            AiButton.gameObject.SetActive(true);
            Debug.Log(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.Count);
            yield break;
        }

        #endregion  发牌
    }

    /// <summary>
    ///展示面牌
    /// </summary>
    private void ShowMianCard()
    {
        MianCardPanel.SetActive(true);
    }

    /// <summary>
    /// 初始化所有未摸的牌和相关数据  AllCardsOnDesk放入牌
    /// </summary>
    int FanBaoPos;//翻宝人位置
    LocalViewDirection FanBaoLocation;
    void InitCardsOnDesk()
    {
        // GameData.m_TableInfo.makerPos;
        if ((GameData.Dice1 + GameData.Dice2) % 4 == 1)
        {
            // FanBaoLocation = LocalViewDirection.LOCAL;
            FanBaoPos = GameData.m_TableInfo.makerPos;
        }
        else if ((GameData.Dice1 + GameData.Dice2) % 4 == 2)
        {
            //  FanBaoLocation = LocalViewDirection.RIGHT;
            FanBaoPos = GameData.m_TableInfo.makerPos + 1;
            if (FanBaoPos > 4)
            {
                FanBaoPos = FanBaoPos % 4;
            }
        }
        else if ((GameData.Dice1 + GameData.Dice2) % 4 == 3)
        {
            // FanBaoLocation = LocalViewDirection.UP ;
            FanBaoPos = GameData.m_TableInfo.makerPos + 2;
            if (FanBaoPos > 4)
            {
                FanBaoPos = FanBaoPos % 4;
            }
        }
        else if ((GameData.Dice1 + GameData.Dice2) % 4 == 0)
        {
            // FanBaoLocation = LocalViewDirection.LEFT;
            FanBaoPos = GameData.m_TableInfo.makerPos + 3;
            if (FanBaoPos > 4)
            {
                FanBaoPos = FanBaoPos % 4;
            }
            //  FanBaoPos = GameDataFunc.GetPlayerInfo(LocalViewDirection.LEFT).pos;
        }

        FanBaoLocation = GetLVD((byte)FanBaoPos);




        Debug.Log("dice1" + GameData.Dice1.ToString() + "dice2" + GameData.Dice2.ToString());
        int sp = GameData.Dice1 + GameData.Dice2 + GameData.Dice3 + GameData.Dice4;//开始拿牌的位置
        if (GameData.m_TableInfo.curGameCount > 0)
        {

            AllCardsOnDesk = new GameObject[TotalCardsCount];
            Array.Clear(AllCardsOnDesk, 0, AllCardsOnDesk.Length);//清空数组


            int index = 0;
            //switch (GetLVD((byte)(FanBaoPos), true))
            //{

            #region 正常情况
            if (sp < 17)
            {
                #region
                switch (FanBaoLocation)
                {



                    case LocalViewDirection.LEFT:
                        for (int i = sp; i < leftDeskStartStack; i++)//17
                        {
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < topDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < rightDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < selfDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < (sp); i++)
                        {
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        break;
                    case LocalViewDirection.RIGHT:
                        for (int i = (sp); i < rightDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < selfDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < leftDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < topDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < (sp); i++)
                        {
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        break;
                    case LocalViewDirection.UP:
                        for (int i = (sp); i < topDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;

                        }
                        for (int i = 0; i < rightDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < selfDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < leftDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < (sp); i++)
                        {
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        break;
                    case LocalViewDirection.LOCAL:
                        for (int i = (sp); i < selfDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < leftDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < topDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < rightDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < (sp); i++)
                        {
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        break;



                }

                #endregion
            }
            else
            {
                sp = sp - 17;
                #region
                switch (FanBaoLocation)
                {
                    case LocalViewDirection.LEFT:

                        for (int i = (sp); i < topDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;

                        }
                        for (int i = 0; i < rightDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < selfDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < leftDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < (sp); i++)
                        {
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        break;




                    case LocalViewDirection.RIGHT:
                        for (int i = (sp); i < selfDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < leftDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < topDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < rightDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < (sp); i++)
                        {
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        break;




                    case LocalViewDirection.UP:
                        for (int i = (sp); i < rightDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < selfDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < leftDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < topDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < (sp); i++)
                        {
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }



                        break;
                    case LocalViewDirection.LOCAL:
                        for (int i = sp; i < leftDeskStartStack; i++)//17
                        {
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < topDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = TopOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < rightDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = RightOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < selfDeskStartStack; i++)
                        {
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = SelfOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }
                        for (int i = 0; i < (sp); i++)
                        {
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2).ToString()).gameObject;
                            index++;
                            AllCardsOnDesk[index] = LeftOnDesk.transform.Find((i * 2 + 1).ToString()).gameObject;
                            index++;
                        }




                        break;



                }

                #endregion
            }



            #endregion
            //把放在上面的牌，影子去掉
            foreach (GameObject card in AllCardsOnDesk)
            {
                card.SetActive(true);
                if (int.Parse(card.name) % 2 == 0)
                {
                    card.transform.Find("Shadow").gameObject.SetActive(false);
                    card.transform.position = new Vector3(card.transform.position.x, 0.085f, card.transform.position.z);
                }
            }
            //根据当前牌局设置牌的材质
            //if (GameData.m_TableInfo.curGameCount % 2 == 0)
            //{
            //    NormalM.mainTexture = NormalTex1;
            //    ClickedM.mainTexture = ClickedTex1;
            //}
            //else
            //{
            //    NormalM.mainTexture = NormalTex2;
            //    ClickedM.mainTexture = ClickedTex2;
            //}
        }
    }
    #endregion
    /// <summary>
    /// 开始发牌
    /// </summary>
    bool CanDealCard = false;//是否可以发牌
    public void onPlayerHoldCards()
    {

        btnInvite.SetActive(false);
        btnReady.SetActive(false);
        CanDealCard = true;
        // StartCoroutine(AsynDealCard());

        PartOverResetAI();
    }


    //发牌
    IEnumerator AsynDealCard()
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_START_GAME);
        DiceParent.gameObject.SetActive(true);
        DiceParent.localEulerAngles = Vector3.zero;
        iTween.RotateFrom(DiceParent.gameObject, iTween.Hash("y", -180, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        Dice1.transform.localPosition = new Vector3(0.2f, 0, 0);
        Dice2.transform.localPosition = new Vector3(-0.2f, 0, 0);
        iTween.MoveFrom(Dice1, iTween.Hash("x", 0.875, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        iTween.MoveFrom(Dice2, iTween.Hash("x", -0.875, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        iTween.RotateBy(Dice1, iTween.Hash("x", UnityEngine.Random.Range(540, 1080), "y", UnityEngine.Random.Range(540, 1080), "z", UnityEngine.Random.Range(540, 1080), "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        iTween.RotateBy(Dice2, iTween.Hash("x", UnityEngine.Random.Range(540, 1080), "y", UnityEngine.Random.Range(540, 1080), "z", UnityEngine.Random.Range(540, 1080), "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
        OutCountDown.SetActive(false);
        yield return new WaitForSeconds(1.05f);
        SetDice(Dice1, GameData.Dice1);
        SetDice(Dice2, GameData.Dice2);
        yield return new WaitForSeconds(0.3f);

        #region  挪开挡板 
        ////挪开挡板
        //TweenPosition.Begin(TopLift, 0.5f, new Vector3(0, 0, -0.04f));
        //TweenPosition.Begin(SelfLift, 0.5f, new Vector3(0, 0, -0.04f));
        //TweenPosition.Begin(LeftLift, 0.5f, new Vector3(0, 0, -0.04f));
        //TweenPosition.Begin(RightLift, 0.5f, new Vector3(0, 0, -0.04f));
        //yield return new WaitForSeconds(0.5f);
        //SetLocations(0);//东南西北
        //InitCardsOnDesk();// 把桌上的牌放到 AllCardsOnDesk中

        ////重置牌堆，并将牌堆移上挡板
        //SelfOnDesk.gameObject.SetActive(true);
        //RightOnDesk.gameObject.SetActive(true);
        //TopOnDesk.gameObject.SetActive(true);
        //LeftOnDesk.gameObject.SetActive(true);
        //SelfOnDesk.transform.localPosition = new Vector3(0.2f, 0, 0.02f);
        //RightOnDesk.transform.localPosition = new Vector3(0.2f, 0, 0.02f);
        //TopOnDesk.transform.localPosition = new Vector3(0.2f, 0, 0.02f);
        //LeftOnDesk.transform.localPosition = new Vector3(0.2f, 0, 0.02f);

        //TweenPosition.Begin(SelfOnDesk, 0.5f, new Vector3(originalSelfOnDeskLocalPos.x, originalSelfOnDeskLocalPos.y, 0.017f));
        //TweenPosition.Begin(RightOnDesk, 0.5f, new Vector3(originalRightOnDeskLocalPos.x, originalRightOnDeskLocalPos.y, 0.017f));
        //TweenPosition.Begin(LeftOnDesk, 0.5f, new Vector3(originalLeftOnDeskLocalPos.x, originalLeftOnDeskLocalPos.y, 0.017f));
        //TweenPosition.Begin(TopOnDesk, 0.5f, new Vector3(originalTopOnDeskLocalPos.x, originalTopOnDeskLocalPos.y, 0.017f));

        //yield return new WaitForSeconds(0.5f);

        ////将挡板和麻将牌一起上移
        //ResCardCount = (uint)TotalCardsCount;
        //TweenPosition.Begin(SelfOnDesk, 0.5f, new Vector3(originalSelfOnDeskLocalPos.x, originalSelfOnDeskLocalPos.y, 0.065f));
        //TweenPosition.Begin(RightOnDesk, 0.5f, new Vector3(originalRightOnDeskLocalPos.x, originalRightOnDeskLocalPos.y, 0.065f));
        //TweenPosition.Begin(LeftOnDesk, 0.5f, new Vector3(originalLeftOnDeskLocalPos.x, originalLeftOnDeskLocalPos.y, 0.065f));
        //TweenPosition.Begin(TopOnDesk, 0.5f, new Vector3(originalTopOnDeskLocalPos.x, originalTopOnDeskLocalPos.y, 0.065f));
        //TweenPosition.Begin(TopLift, 0.5f, Vector3.zero);
        //TweenPosition.Begin(SelfLift, 0.5f, Vector3.zero);
        //TweenPosition.Begin(LeftLift, 0.5f, Vector3.zero);
        //TweenPosition.Begin(RightLift, 0.5f, Vector3.zero);
        //yield return new WaitForSeconds(0.5f);

        #endregion

        int inCardCount = 3;
        int cardCount = 4;

        int index = 0;
        int indexRight = 0;
        int indexUp = 0;
        int indexLeft = 0;
        int tmpGivenCards = 0;
        if (pbCardImage == null) pbCardImage = Resources.Load<GameObject>("Item/CardImage");
        for (int i = 0; i < inCardCount; i++)//发牌 发3把牌
        {
            #region 发牌
            for (int j = 0; j < cardCount; j++)//本地牌 4张4张的发
            {
                GameObject card = Instantiate<GameObject>(pbCardImage);
                card.transform.parent = LocalParent;
                card.transform.localScale = Vector3.one;
                card.transform.localPosition = new Vector3((int)LocalLeftCardStartPos + index * 75, 0, 0);
                card.name = index.ToString();
                GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Add(card);
                index++;
            }
            SoundManager.Instance.PlaySound(UIPaths.SOUND_FAPAI);
            yield return new WaitForSeconds(0.2f);
            //赋值
            int count = GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Count / cardCount - 1;
            for (int k = 0; k < cardCount; k++)
            {
                int indexCount = count * cardCount + k;
                GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[indexCount].GetComponent<UISprite>().spriteName = "card_local_max_" + GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList[indexCount];
            }
            tmpGivenCards += cardCount;
            ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
            //if (GameData.m_TableInfo.configPlayerIndex == 0 || GameData.m_TableInfo.configPlayerIndex == 1)
            //{


            yield return new WaitForSeconds(0.1f);

            if (GameDataFunc.GetPlayerInfo(LocalViewDirection.RIGHT) != null)
            {
                for (int j = 0; j < cardCount; j++)//下家
                {
                    GameObject card = RightOnHand.Find(indexRight.ToString()).gameObject;
                    SetActiveChildPaiCount(RightOnHand, indexRight + 1);
                    GameDataFunc.GetHoldCardObj(LocalViewDirection.RIGHT).holdObjList.Add(card);
                    indexRight++;
                }
                tmpGivenCards += cardCount;
                ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
                yield return new WaitForSeconds(0.1f);
                //}
            }
            if (GameDataFunc.GetPlayerInfo(LocalViewDirection.UP) != null)
            {
                for (int j = 0; j < cardCount; j++)//对家
                {
                    GameObject card = TopOnHand.Find(indexUp.ToString()).gameObject;
                    SetActiveChildPaiCount(TopOnHand, indexUp + 1);
                    GameDataFunc.GetHoldCardObj(LocalViewDirection.UP).holdObjList.Add(card);
                    indexUp++;
                }
                tmpGivenCards += cardCount;
                ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);

                //if (GameData.m_TableInfo.configPlayerIndex == 0)
                //{
                yield return new WaitForSeconds(0.1f);
            }
            if (GameDataFunc.GetPlayerInfo(LocalViewDirection.LEFT) != null)
            {
                for (int j = 0; j < cardCount; j++)//上家
                {
                    GameObject card = LeftOnHand.Find(indexLeft.ToString()).gameObject;
                    SetActiveChildPaiCount(LeftOnHand, indexLeft + 1);
                    GameDataFunc.GetHoldCardObj(LocalViewDirection.LEFT).holdObjList.Add(card);
                    indexLeft++;
                }
                tmpGivenCards += cardCount;
                ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
                //}
                yield return new WaitForSeconds(0.1f);
            }
            #endregion  
        }


        #region  最后一张牌的处理
        GameObject card1 = Instantiate<GameObject>(pbCardImage);//自己的最后一张牌
        card1.transform.parent = LocalParent;
        card1.transform.localScale = Vector3.one;
        card1.transform.localPosition = new Vector3((int)LocalLeftCardStartPos + index * 75, 0, 0);
        tmpGivenCards += 1;
        ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
        yield return new WaitForSeconds(0.2f);
        card1.GetComponent<UISprite>().spriteName = "card_local_max_back";
        GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Add(card1);
        //if (GameData.m_TableInfo.configPlayerIndex == 0 || GameData.m_TableInfo.configPlayerIndex == 1)
        //{
        if (GameDataFunc.GetPlayerInfo(LocalViewDirection.RIGHT) != null)
        {
            GameObject card = RightOnHand.Find(indexRight.ToString()).gameObject;
            SetActiveChildPaiCount(RightOnHand, indexRight + 1);
            GameDataFunc.GetHoldCardObj(LocalViewDirection.RIGHT).holdObjList.Add(card);
            tmpGivenCards += 1;
            ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
            //}
            yield return new WaitForSeconds(0.1f);
        }
        if (GameDataFunc.GetPlayerInfo(LocalViewDirection.UP) != null)
        {
            GameObject card2 = TopOnHand.Find(indexUp.ToString()).gameObject;
            SetActiveChildPaiCount(TopOnHand, indexUp + 1);
            GameDataFunc.GetHoldCardObj(LocalViewDirection.UP).holdObjList.Add(card2);
            tmpGivenCards += 1;
            ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
            yield return new WaitForSeconds(0.1f);
        }
        //if (GameData.m_TableInfo.configPlayerIndex == 0)
        //{
        if (GameDataFunc.GetPlayerInfo(LocalViewDirection.LEFT) != null)
        {
            GameObject card3 = LeftOnHand.Find(indexLeft.ToString()).gameObject;
            SetActiveChildPaiCount(LeftOnHand, indexLeft + 1);
            GameDataFunc.GetHoldCardObj(LocalViewDirection.LEFT).holdObjList.Add(card3);
            tmpGivenCards += 1;
            ResCardCount = (uint)(TotalCardsCount - tmpGivenCards);
            yield return new WaitForSeconds(0.1f);
            //}
        }

        #endregion



        GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.Sort((a, b) =>
        {
            return ((int)a - (int)b);
        });
        for (int m = 0; m < GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Count; m++)
        {
            GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[m].GetComponent<UISprite>().spriteName = "card_local_max_back";
        }
        yield return new WaitForSeconds(0.2f);
        for (int m = 0; m < GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Count; m++)
        {
            GameObject obj = GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[m];
            PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL);
            obj.name = info.localCardList[m].ToString();
            obj.GetComponent<UISprite>().spriteName = "card_local_max_" + info.localCardList[m].ToString();


        }
        IsDealCardOver = true;// GameData.m_TableInfo.makerPos 发牌结束

        //庄家进牌
        #region add by tj
        //CreateInCard(GameData.m_TableInfo.makerPos, InCardNumber);
        //if (GameData.m_TableInfo.makerPos == LocalPos) ShowLocalOperateState(GameData.m_TableInfo.makerPos, InCardNumber);
        //curOutWhoOperate = GameData.m_TableInfo.makerPos;
        #endregion
        CreateInCard(InCardPos, InCardNumber);//庄进张赋值
        if (InCardPos == LocalPos) ShowLocalOperateState(InCardPos, InCardNumber);
        for (int i = 0; i < GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Count; i++)
        {
            GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[i].AddComponent<BoxCollider2D>().size = new Vector3(75, 122, 0);
            GameObject obj = GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[i];
            UIEventListener.Get(obj).onPress = OnPressCard;
            UIEventListener.Get(obj).onClick = OnClickCard;
            UIEventListener.Get(obj).onDrag = OnDrag;
            UIEventListener.Get(obj).onDragEnd = OnDragEnd;
        }
        ResCardCount = GameData.m_TableInfo.resCardCount;
        DiceParent.gameObject.SetActive(false);
        yield break;
    }




    /// <summary>
    /// 玩家的操作
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="opType"></param>
    /// <param name="opCard"></param>
    /// <param name="outPos"></param>
    /// <param name="isAnGang"></param>
    public void onPlayerOperate(byte pos, CardOperateType opType, uint opCard, byte outPos, bool isAnGang)
    {

        HideOperateBtn();
        PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);
        if (pos == LocalPos)//重置听牌提示的东西（logo）
        {
            ResetTingLogo();
            ResetTingNotice();
        }
        switch (opType)
        {
            case CardOperateType.ChuPai:
                outCardNumber = opCard;
                if (opCard == GameData.m_TableInfo.MianCard)//打出的是面牌 或是万能牌
                {
                    GameData.m_TableInfo.ChiMianCount++;
                    info.MianCardList.Add(opCard);
                    CreatPlayerSendMianCard(info, opCard);
                }
                else if (opCard == GameData.m_TableInfo.MagicCard)
                {
                    info.MianCardList.Add(opCard);
                    CreatPlayerSendMianCard(info, opCard);
                }
                else
                {
                    if (info.pos != LocalPos) ShowLocalOperateState(info.pos, outCardNumber, false);//看自己能否吃碰杠
                    StartCoroutine(AsynCreatePlayerOutCard(info.guid, opCard));//显示出的牌 操作位等操作
                }

                break;
            case CardOperateType.PengPai:
                ObjOutSign.SetActive(false);
                curOutWhoOperate = info.pos;
                ShowOutCardPlayer(info.pos);//显示该谁出牌
                SoundManager.Instance.PlaySound(UIPaths.SOUND_PENG, info.sex);
                StartCoroutine(AsynPengGang(info.guid, opType, opCard, outPos, false, true));//碰牌的处理
                StartCoroutine(AsynCreateEffect(info.LVD, opType));
                if (pos == LocalPos)
                {

                    AfterOperateCheckGangHu();//检测是否还有杠 检测听
                    CheckTing();
                }

                break;
            case CardOperateType.GangPai:

                ObjOutSign.SetActive(false);
                curOutWhoOperate = info.pos;
                ShowOutCardPlayer(info.pos);
                SoundManager.Instance.PlaySound(UIPaths.SOUND_GANG, info.sex);
                StartCoroutine(AsynPengGang(info.guid, opType, opCard, outPos, isAnGang, true));
                StartCoroutine(AsynCreateEffect(info.LVD, opType));

                if (pos == LocalPos)
                {
                    AfterOperateCheckGangHu();//检测是否还有杠 检测听
                    CheckTing();
                }
                break;
            case CardOperateType.HuPai:
                //SoundManager.Instance.PlaySound(UIPaths.SOUND_HU, info.sex);
                //StartCoroutine(AsynCreateEffect(info.LVD, opType));
                //if(info.pos != outPos && info.pos == LocalPos)
                //{
                //    CreateInCard(info.pos,opCard,true);
                //}
                break;
        }
    }




    public Transform SelfMianCardParent;
    public Transform RightMianCardParent;
    public Transform UpMianCardParent;
    public Transform LeftMianCardParent;
    Transform parentMianCard;
    /// <summary>
    /// 生成玩家打出的面牌
    /// </summary>
    /// <param name="info"></param>
    private void CreatPlayerSendMianCard(PlayerInfo info, uint card = 0)
    {


        HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(info.pos);
        for (int i = 0; i < infoObj.outMianCardObjList.Count; i++)
        {
            Destroy(infoObj.outMianCardObjList[i]);
        }
        infoObj.outMianCardObjList = new List<GameObject>();

        switch (GetLVD(info.pos))
        {
            case LocalViewDirection.LOCAL:
                parentMianCard = SelfMianCardParent;
                if (card != 0)
                {
                    //  GameDataFunc.GetTableCardInfoByID(card).RemainedCount -= 1;
                    GameDataFunc.RemoverHoldCard(card, info.pos);//移除手牌
                    GameDataFunc.RemoveHoldCardObj(card, info.pos);
                    ResetLocalHoldCards();//手牌复位
                }
                if (card != 0)//生成吃面特效
                {
                    if (card == GameData.m_TableInfo.MianCard)
                    {
                        StartCoroutine(AsynCreateEffect(LocalViewDirection.LOCAL, CardOperateType.None, 1));
                    }
                    else if (card == GameData.m_TableInfo.MagicCard)
                    {
                        StartCoroutine(AsynCreateEffect(LocalViewDirection.LOCAL, CardOperateType.None, 2));
                    }
                }

                break;
            case LocalViewDirection.RIGHT:
                parentMianCard = RightMianCardParent;
                if (card != 0)
                {
                    RemoveOtherPlayerHoldCard(RightOnHand, 1);
                }
                if (card != 0)
                {
                    if (card == GameData.m_TableInfo.MianCard)
                    {
                        StartCoroutine(AsynCreateEffect(LocalViewDirection.RIGHT, CardOperateType.None, 1));
                    }
                    else if (card == GameData.m_TableInfo.MagicCard)
                    {
                        StartCoroutine(AsynCreateEffect(LocalViewDirection.RIGHT, CardOperateType.None, 2));
                    }
                }
                break;
            case LocalViewDirection.UP:
                parentMianCard = UpMianCardParent;
                if (card != 0)
                {
                    RemoveOtherPlayerHoldCard(TopOnHand, 1);
                }
                if (card != 0)
                {
                    if (card == GameData.m_TableInfo.MianCard)
                    {
                        StartCoroutine(AsynCreateEffect(LocalViewDirection.UP, CardOperateType.None, 1));
                    }
                    else if (card == GameData.m_TableInfo.MagicCard)
                    {
                        StartCoroutine(AsynCreateEffect(LocalViewDirection.UP, CardOperateType.None, 2));
                    }
                }
                break;
            case LocalViewDirection.LEFT:
                parentMianCard = LeftMianCardParent;
                if (card != 0)
                {
                    RemoveOtherPlayerHoldCard(LeftOnHand, 1);
                }
                if (card != 0)
                {
                    if (card == GameData.m_TableInfo.MianCard)
                    {
                        StartCoroutine(AsynCreateEffect(LocalViewDirection.LEFT, CardOperateType.None, 1));
                    }
                    else if (card == GameData.m_TableInfo.MagicCard)
                    {
                        StartCoroutine(AsynCreateEffect(LocalViewDirection.LEFT, CardOperateType.None, 2));
                    }
                }
                break;

        }

        for (int i = 0; i < info.MianCardList.Count; i++)
        {
            GameObject item = null;
            item = Instantiate<GameObject>(MJPaiParent.Find(info.MianCardList[i].ToString()).gameObject, parentMianCard);
            item.transform.localPosition = parentMianCard.Find(i.ToString()).localPosition;
            item.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);

            item.transform.localRotation = parentMianCard.Find(i.ToString()).localRotation;
            item.SetActive(true);
            infoObj.outMianCardObjList.Add(item);
        }

        if (card != 0)
        {
            SoundManager.Instance.PlaySound(UIPaths.SOUND_OUT_CARD + card, info.sex);
            SoundManager.Instance.PlaySound(UIPaths.SOUND_CHUPAI);
            GameDataFunc.GetTableCardInfoByID(card).RemainedCount -= 1;
        }


        //  GameDataFunc.GetTableCardInfoByID(card).RemainedCount -= 1;
        ////  PlayerInfo info = GameDataFunc.GetPlayerInfo(playerGuid);
        //  HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(info.pos);
        //  GameObject item = null;
        //  item = Instantiate<GameObject>(MJPaiParent.FindChild(card.ToString()).gameObject);
        //  Vector3 endPos = Vector3.zero;
        //  item.name = card.ToString();
        //  info.outCardList.Add(card);//打出的牌的数据
        //  infoObj.outObjList.Add(item);//打出的牌的物体
        //  SoundManager.Instance.PlaySound(UIPaths.SOUND_OUT_CARD + card, info.sex);
    }


    bool SelfHaveGone = false;//自己有没有杠 杠后有财神
                              /// <summary>
                              /// 显示自己的操作位
                              /// </summary>
                              /// <param name="pos"></param>
                              /// <param name="card"></param>
                              /// <param name="isMoPai"></param>

    void ShowLocalOperateState(byte pos, uint card, bool isMoPai = true)
    {

        bool isHu = false;
        bool isGang = false;
        bool isPeng = false;
        outCardNumber = card;
        PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalPos);
        if (pos == LocalPos && isMoPai)//自己摸上了的牌
        {

            List<List<uint>> operateList = new List<List<uint>>();
            for (int i = 0; i < info.operateCardList.Count; i++)
            {
                List<uint> opCards = new List<uint>();
                if (info.operateCardList[i].opType == CatchType.Peng)
                {
                    opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard);
                }
                else if (info.operateCardList[i].opType == CatchType.Gang || info.operateCardList[i].opType == CatchType.AnGang || info.operateCardList[i].opType == CatchType.BuGang)
                {
                    opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard);
                    // SelfHaveGone = true;
                }
                operateList.Add(opCards);
            }


            isHu = myXYHelper.Instance.ZBCheckHu(operateList, info.localCardList, GameData.m_TableInfo.MagicCard);
            if (GetGangList().Count > 0) isGang = true;
            int index = 0;
            if (isGang || isHu)
            {
                btnGuo.SetActive(true);
                btnGuo.transform.localPosition = new Vector3(-140 * index + 300, 0, 0);
                index++;
            }
            if (isGang)
            {
                btnGang.SetActive(true);
                btnGang.transform.localPosition = new Vector3(-140 * index + 300, 0, 0);
                index++;
            }
            if (isHu)
            {
                btnHu.SetActive(true);
                btnHu.transform.localPosition = new Vector3(-140 * index + 300, 0, 0);
                index++;
            }

            if (!isHu)
            {
                CheckTing();//检查是否听牌
            }



        }
        else
        {

            #region
            if (pos != LocalPos)
            {
                List<uint> tempList = new List<uint>();
                tempList.AddRange(info.localCardList.ToArray());
                tempList.Add(card);
                List<List<uint>> operateList = new List<List<uint>>();
                List<uint> allCards = new List<uint>();
                for (int i = 0; i < info.operateCardList.Count; i++)
                {
                    List<uint> opCards = new List<uint>();
                    if (info.operateCardList[i].opType == CatchType.Peng)
                    {
                        opCards.Add(info.operateCardList[i].opCard);
                        opCards.Add(info.operateCardList[i].opCard);
                        opCards.Add(info.operateCardList[i].opCard);
                    }
                    else
                    {
                        opCards.Add(info.operateCardList[i].opCard);
                        opCards.Add(info.operateCardList[i].opCard);
                        opCards.Add(info.operateCardList[i].opCard);
                        opCards.Add(info.operateCardList[i].opCard);
                    }
                    operateList.Add(opCards);
                    allCards.AddRange(opCards.ToArray());
                }
                //没有缺一不能胡
                //if (GameData.m_TableInfo.configFangChongIndex == 0)
                //{
                //    allCards.AddRange(info.localCardList.ToArray());
                //    int menCount = myXYHelper.Instance.getTypeSeCount(allCards);
                //    if (menCount <= 1) isHu = myXYHelper.Instance.checkHu(operateList, tempList, 0);
                //}
                //else

                //for (int i = 0; i < info.operateCardList.Count; i++)
                //{
                //    if (info.operateCardList[i].opType == CatchType.AnGang || info.operateCardList[i].opType == CatchType.Gang)
                //    {
                //        SelfHaveGone = true;//检查自己是否有财神
                //    }
                //}
                //    isHu = myXYHelper.Instance.newCheckHu(operateList, info.localCardList, card, SelfHaveGone);//是否杠开

                isPeng = IsOtherPeng(card);
                isGang = IsOtherGang(card);
                int index = 0;
                if (isPeng || isGang || isHu)
                {
                    btnGuo.SetActive(true);
                    btnGuo.transform.localPosition = new Vector3(-140 * index + 300, 0, 0);
                    index++;
                }
                if (isPeng)
                {
                    btnPeng.SetActive(true);
                    btnPeng.transform.localPosition = new Vector3(-140 * index + 300, 0, 0);
                    index++;
                }
                if (isGang)
                {
                    btnGang.SetActive(true);
                    btnGang.transform.localPosition = new Vector3(-140 * index + 300, 0, 0);
                    index++;
                }
                if (isHu)
                {
                    btnHu.SetActive(true);
                    btnHu.transform.localPosition = new Vector3(-140 * index + 300, 0, 0);
                    index++;
                }
            }

            #endregion
        }

    }

    #region  听牌相关

    public GameObject TingPaiPanel;//听牌提示框
    public GameObject TingCardImage;//听牌的预制体
    Dictionary<uint, List<Dictionary<uint, HuCardInfo>>> TingDic = new Dictionary<uint, List<Dictionary<uint, HuCardInfo>>>();//听牌提示返回
    public List<GameObject> TingCardList = new List<GameObject>();//听牌生成的物体
    bool IsTingPai = false;
    /// <summary>
    /// 检测听牌提示
    /// </summary>
    public void CheckTing()
    {
        TingDic = new Dictionary<uint, List<Dictionary<uint, HuCardInfo>>>();
        IsTingPai = false;
        List<uint> OutCard = new List<uint>();//打出的牌(所有玩家)
        for (int i = 0; i < GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).outCardList.Count; i++)
        {
            OutCard.Add(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).outCardList[i]);
        }
        for (int i = 0; i < GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).MianCardList.Count; i++)
        {
            OutCard.Add(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).MianCardList[i]);
        }
        for (int i = 0; i < GameDataFunc.GetPlayerInfo(LocalViewDirection.LEFT).outCardList.Count; i++)
        {
            OutCard.Add(GameDataFunc.GetPlayerInfo(LocalViewDirection.LEFT).outCardList[i]);
        }
        for (int i = 0; i < GameDataFunc.GetPlayerInfo(LocalViewDirection.RIGHT).outCardList.Count; i++)
        {
            OutCard.Add(GameDataFunc.GetPlayerInfo(LocalViewDirection.RIGHT).outCardList[i]);
        }
        for (int i = 0; i < GameDataFunc.GetPlayerInfo(LocalViewDirection.UP).outCardList.Count; i++)
        {
            OutCard.Add(GameDataFunc.GetPlayerInfo(LocalViewDirection.UP).outCardList[i]);
        }


        PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalPos);

        List<List<uint>> operateList = new List<List<uint>>();
        for (int i = 0; i < info.operateCardList.Count; i++)
        {
            List<uint> opCards = new List<uint>();
            if (info.operateCardList[i].opType == CatchType.Peng)
            {
                opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard);
            }
            else if (info.operateCardList[i].opType == CatchType.Gang || info.operateCardList[i].opType == CatchType.AnGang || info.operateCardList[i].opType == CatchType.BuGang)
            {
                opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard); opCards.Add(info.operateCardList[i].opCard);
                // SelfHaveGone = true;
            }
            operateList.Add(opCards);
        }
        List<uint> HoldCardList = new List<uint>();
        HoldCardList.AddRange(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList);//手牌

        try
        {
            TingDic = myXYHelper.Instance.ZBCheckTing(OutCard, operateList, HoldCardList, GameData.m_TableInfo.MagicCard);
            if (TingDic == null)
            {
                TingDic = new Dictionary<uint, List<Dictionary<uint, HuCardInfo>>>();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("检测听牌出错");
        }


        Debug.Log(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList);
        if (TingDic.Count != 0)
        {
            IsTingPai = true;
            ShowTingDropCard();
        }
    }

    /// <summary>
    /// 展示出打出那张可以听
    /// </summary>
    public void ShowTingDropCard()
    {
        List<uint> DrapCardList = new List<uint>();
        DrapCardList.AddRange(TingDic.Keys);
        for (int i = 0; i < GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Count; i++)
        {
            if (DrapCardList.Contains(uint.Parse(GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[i].name)))
            {
                // GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[i].transform.GetComponent<UISprite>().color = new Color(0.6f,0.6f,0.6f,1);
                GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[i].transform.Find("TingSprite").gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 重置logo
    /// </summary>
    public void ResetTingLogo()
    {
        for (int i = 0; i < GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Count; i++)
        {

            GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[i].transform.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 1);
            GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList[i].transform.Find("TingSprite").gameObject.SetActive(false);

        }
    }
    /// <summary>
    /// 显示听牌提示
    /// </summary>
    public void ShowTingNotice(object cardName)
    {
        ResetTingNotice();
        TingPaiPanel.transform.Find("BgSprite").GetComponent<UISprite>().width = 130;
        //  Dictionary<uint, List<Dictionary<uint, HuCardInfo>>> TingDic = new Dictionary<uint, List<Dictionary<uint, HuCardInfo>>>();//听牌提示返回
        uint TingDropCard = uint.Parse(cardName.ToString());
        if (TingDic[TingDropCard][0].Count > 1)
        {
            TingPaiPanel.transform.Find("BgSprite").GetComponent<UISprite>().width = TingDic[TingDropCard][0].Count * 100;
            if (TingDic[TingDropCard][0].Count % 2 == 0)//偶数张
            {
                List<uint> HuCard = new List<uint>();
                List<HuCardInfo> HuInfo = new List<HuCardInfo>();
                foreach (var item in TingDic[TingDropCard][0])
                {
                    HuCard.Add(item.Key);
                    HuInfo.Add(item.Value);
                }
                for (int i = 0; i < HuInfo.Count; i++)
                {
                    GameObject g = GameObject.Instantiate(TingCardImage, TingPaiPanel.transform.Find("BgSprite"));
                    g.transform.localScale = Vector3.one;
                    g.transform.localPosition = new Vector3(-38 - (76 * ((TingDic[TingDropCard][0].Count / 2) - 1)) + 76 * i, 27, 0);
                    TingCardList.Add(g);
                    if (HuCard[i] != 0)//任意牌都能胡
                    {
                        g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + HuCard[i].ToString();
                        if (GameDataFunc.GetTableCardInfoByID(HuCard[i]).RemainedCount < 0) GameDataFunc.GetTableCardInfoByID(HuCard[i]).RemainedCount = 0;
                        g.transform.Find("Label").GetComponent<UILabel>().text = "剩" + GameDataFunc.GetTableCardInfoByID(HuCard[i]).RemainedCount + "张";
                    }
                    else
                    {
                        g.transform.GetComponent<UISprite>().spriteName = "card_local_UnknownCard_min";
                        g.transform.Find("Label").GetComponent<UILabel>().text = "胡任意牌";
                    }

                    //if (HuInfo[0].doubleTime != 0)
                    //{
                    //    g.transform.FindChild("Label").GetComponent<UILabel>().text = HuInfo[0].doubleTime.ToString() + "分\n";
                    //}
                    //switch (HuInfo[0].huType)
                    //{
                    //    case HuType.QiDuiHu:
                    //        g.transform.FindChild("Label").GetComponent<UILabel>().text += "七对\n";
                    //        break;
                    //    case HuType.ShiSiBuDa:
                    //        g.transform.FindChild("Label").GetComponent<UILabel>().text += "烂胡\n";
                    //        break;
                    //    case HuType.YaoJiuPai:
                    //        g.transform.FindChild("Label").GetComponent<UILabel>().text += "幺九\n";
                    //        break;

                    //}
                }

                #region
                //for (int i = 0; i < TingDic[TingDropCard].Count; i++)
                //{
                //    GameObject g = GameObject.Instantiate(TingCardImage, TingPaiPanel.transform.FindChild("BgSprite"));
                //    g.transform.localScale = Vector3.one;
                //    g.transform.localPosition = new Vector3(-38*(TingDic[TingDropCard].Count/2)+38*i,27,0);
                //    TingCardList.Add(g);
                //    for (int j = 0; j < TingDic[TingDropCard].Count; j++)
                //    {
                //        if (j == 0)
                //        {
                //            List<uint> HuCard = new List<uint>();
                //            List<HuCardInfo> HuInfo = new List<HuCardInfo>();
                //            foreach (var item in TingDic[TingDropCard][j])
                //            {
                //                HuCard.Add(item.Key);
                //                HuInfo.Add(item.Value);
                //            }
                //            g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + HuCard[0].ToString();
                //            if (HuInfo[0].doubleTime != 0)
                //            {
                //                g.transform.FindChild("Label").GetComponent<UILabel>().text = HuInfo[0].doubleTime.ToString() + "分\n";
                //            }
                //            switch (HuInfo[0].huType)
                //            {
                //                case HuType.QiDuiHu:
                //                    g.transform.FindChild("Label").GetComponent<UILabel>().text += "七对\n";
                //                    break;
                //                case HuType.ShiSiBuDa:
                //                    g.transform.FindChild("Label").GetComponent<UILabel>().text += "烂胡\n";
                //                    break;
                //                case HuType.YaoJiuPai:
                //                    g.transform.FindChild("Label").GetComponent<UILabel>().text += "幺九\n";
                //                    break;

                //            }
                //        }
                //    }
                //}

                #endregion
            }
            else//基数张
            {

                List<uint> HuCard = new List<uint>();
                List<HuCardInfo> HuInfo = new List<HuCardInfo>();
                foreach (var item in TingDic[TingDropCard][0])
                {
                    HuCard.Add(item.Key);
                    HuInfo.Add(item.Value);
                }
                for (int i = 0; i < HuInfo.Count; i++)
                {
                    GameObject g = GameObject.Instantiate(TingCardImage, TingPaiPanel.transform.Find("BgSprite"));
                    g.transform.localScale = Vector3.one;
                    g.transform.localPosition = new Vector3(-76 * ((TingDic[TingDropCard][0].Count - 1) / 2) + 76 * i, 27, 0);
                    TingCardList.Add(g);

                    if (HuCard[i] != 0)//任意牌都能胡
                    {
                        g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + HuCard[i].ToString();
                        if (GameDataFunc.GetTableCardInfoByID(HuCard[i]).RemainedCount < 0) GameDataFunc.GetTableCardInfoByID(HuCard[i]).RemainedCount = 0;
                        g.transform.Find("Label").GetComponent<UILabel>().text = "剩" + GameDataFunc.GetTableCardInfoByID(HuCard[i]).RemainedCount + "张";
                    }
                    else
                    {
                        g.transform.GetComponent<UISprite>().spriteName = "card_local_UnknownCard_min";
                        g.transform.Find("Label").GetComponent<UILabel>().text = "胡任意牌";
                    }

                    //g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + HuCard[i].ToString();
                    //g.transform.FindChild("Label").GetComponent<UILabel>().text = "剩" + GameDataFunc.GetTableCardInfoByID(HuCard[i]).RemainedCount + "张";
                    //if (HuInfo[0].doubleTime != 0)
                    //{
                    //    g.transform.FindChild("Label").GetComponent<UILabel>().text = HuInfo[0].doubleTime.ToString() + "分\n";
                    //}
                    //switch (HuInfo[0].huType)
                    //{
                    //    case HuType.QiDuiHu:
                    //        g.transform.FindChild("Label").GetComponent<UILabel>().text += "七对\n";
                    //        break;
                    //    case HuType.ShiSiBuDa:
                    //        g.transform.FindChild("Label").GetComponent<UILabel>().text += "烂胡\n";
                    //        break;
                    //    case HuType.YaoJiuPai:
                    //        g.transform.FindChild("Label").GetComponent<UILabel>().text += "幺九\n";
                    //        break;

                    //}
                }
                #region
                //for (int i = 0; i < TingDic[TingDropCard].Count; i++)
                //{
                //    GameObject g = GameObject.Instantiate(TingCardImage, TingPaiPanel.transform.FindChild("BgSprite"));
                //    g.transform.localScale = Vector3.one;
                //    g.transform.localPosition = new Vector3(-76 * ((TingDic[TingDropCard].Count-1)/ 2) +76 * i, 27, 0);
                //    TingCardList.Add(g);
                //    for (int j = 0; j < TingDic[TingDropCard].Count; j++)
                //    {
                //        if (j == 0)
                //        {
                //            List<uint> HuCard = new List<uint>();
                //            List<HuCardInfo> HuInfo = new List<HuCardInfo>();
                //            foreach (var item in TingDic[TingDropCard][j])
                //            {
                //                HuCard.Add(item.Key);
                //                HuInfo.Add(item.Value);
                //            }
                //            g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + HuCard[0].ToString();
                //            if (HuInfo[0].doubleTime != 0)
                //            {
                //                g.transform.FindChild("Label").GetComponent<UILabel>().text = HuInfo[0].doubleTime.ToString() + "分\n";
                //            }
                //            switch (HuInfo[0].huType)
                //            {
                //                case HuType.QiDuiHu:
                //                    g.transform.FindChild("Label").GetComponent<UILabel>().text += "七对\n";
                //                    break;
                //                case HuType.ShiSiBuDa:
                //                    g.transform.FindChild("Label").GetComponent<UILabel>().text += "烂胡\n";
                //                    break;
                //                case HuType.YaoJiuPai:
                //                    g.transform.FindChild("Label").GetComponent<UILabel>().text += "幺九\n";
                //                    break;

                //            }
                //        }
                //    }
                //}

                #endregion

            }
        }
        else//只听一张牌
        {
            GameObject g = GameObject.Instantiate(TingCardImage, TingPaiPanel.transform.Find("BgSprite"));
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = new Vector3(0, 27, 0);
            TingCardList.Add(g);
            for (int i = 0; i < TingDic[TingDropCard].Count; i++)
            {
                if (i == 0)
                {
                    List<uint> HuCard = new List<uint>();
                    List<HuCardInfo> HuInfo = new List<HuCardInfo>();
                    foreach (var item in TingDic[TingDropCard][i])
                    {
                        HuCard.Add(item.Key);
                        HuInfo.Add(item.Value);
                    }
                    if (HuCard[i] != 0)//任意牌都能胡
                    {
                        g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + HuCard[i].ToString();
                        if (GameDataFunc.GetTableCardInfoByID(HuCard[i]).RemainedCount < 0) GameDataFunc.GetTableCardInfoByID(HuCard[i]).RemainedCount = 0;
                        g.transform.Find("Label").GetComponent<UILabel>().text = "剩" + GameDataFunc.GetTableCardInfoByID(HuCard[i]).RemainedCount + "张";
                    }
                    else
                    {
                        g.transform.GetComponent<UISprite>().spriteName = "card_local_UnknownCard_min";
                        g.transform.Find("Label").GetComponent<UILabel>().text = "胡任意牌";
                    }

                    //g.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + HuCard[i].ToString();
                    //g.transform.FindChild("Label").GetComponent<UILabel>().text = "剩"+GameDataFunc.GetTableCardInfoByID(HuCard[0]).RemainedCount + "张";
                    //if (HuInfo[0].doubleTime != 0)
                    //{
                    //    g.transform.FindChild("Label").GetComponent<UILabel>().text = HuInfo[0].doubleTime.ToString() + "分\n";
                    //}
                    //switch (HuInfo[0].huType)
                    //{
                    //    case HuType.QiDuiHu:
                    //        g.transform.FindChild("Label").GetComponent<UILabel>().text += "七对\n";
                    //        break;
                    //    case HuType.ShiSiBuDa:
                    //        g.transform.FindChild("Label").GetComponent<UILabel>().text += "烂胡\n";
                    //        break;
                    //    case HuType.YaoJiuPai:
                    //        g.transform.FindChild("Label").GetComponent<UILabel>().text += "幺九\n";
                    //        break;

                    //}
                }
            }
        }


        TingPaiPanel.SetActive(true);
    }

    /// <summary>
    /// 重置听牌panel
    /// </summary>
    public void ResetTingNotice()
    {
        TingPaiPanel.transform.Find("BgSprite").GetComponent<UISprite>().width = 130;
        for (int i = 0; i < TingCardList.Count; i++)
        {
            Destroy(TingCardList[i]);
        }
        TingCardList = new List<GameObject>();
        TingPaiPanel.SetActive(false);
    }
    #endregion
    /// <summary>
    /// 玩家摸牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="card"></param>
    /// <param name="resCards"></param>
    public void onPlayerInCard(byte pos, uint card, uint resCards)
    {
        InCardPos = pos;
        InCardNumber = card;
        curOutWhoOperate = pos;
        if (IsDealCardOver)
        {
            ResCardCount = GameData.m_TableInfo.resCardCount;
            SoundManager.Instance.PlaySound(UIPaths.SOUND_MOPAI);


            CreateInCard(InCardPos, InCardNumber);


            if (LocalPos == pos) ShowLocalOperateState(InCardPos, InCardNumber);
        }
    }



    /// <summary>
    /// 游戏结束
    /// </summary>
    public void onGameOver()
    {
        HideAIBtn();
        ViewHu();
        StartCoroutine(AsynCreateOtherCards());
        StartCoroutine(AsynRoundOver());


    }
    /// <summary>
    /// 隐藏ai按钮
    /// </summary>
    public void HideAIBtn()
    {
        AiButton.transform.Find("AingSprite").gameObject.SetActive(false);
        AiButton.gameObject.SetActive(false);
    }
    /// <summary>
    /// 玩家操作成功
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="type"></param>
    public void onPlayerOperateSuccess(byte pos, CardOperateType type)
    {
        if (pos == LocalPos) HideOperateBtn();
    }

    /// <summary>
    /// 焦点重连
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="isForce"></param>
    public void onPlayerOnForce(byte pos, bool isForce)
    {
        PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);
        info.isForce = isForce;
        PlayerObjList[(int)GetLVD(pos)].transform.Find("online").gameObject.SetActive(!isForce);
    }


    #region 聊天相关

    //聊天内容
    List<string> ChatList = new List<string>() {
         "胡的一把里个大牌",
        "你又冒胡两把牌",
        "哈哈舞的一把好牌",
        "美女在边上嘴都活的",
        "你醒醒响响么得",
        "在你下手跟坟山脚下一样",
        "慌么得个牢胡不盖钱",
        "撒为快些的打不了几把 ",
     "快些出牌啊银都要憨 "};

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
            if (strs[0] == "6")//快捷文字聊天
            {
                for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
                {
                    if (GameData.m_PlayerInfoList[i].guid == ulong.Parse(strs[1]))
                    {
                        PlayerObjList[(int)GetLVD(GameData.m_PlayerInfoList[i].pos)].transform.Find("ChatPanel").GetComponent<ChatControl>().SetValue(ChatList[int.Parse(strs[2])]);
                        SoundControl.Instance.PlayMJChatSound((int)GameData.m_PlayerInfoList[i].sex, int.Parse(strs[2]));
                    }
                }
            }
            if (strs[0] == "2")//输入的文字
            {
                for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
                {
                    if (GameData.m_PlayerInfoList[i].guid == ulong.Parse(strs[1]))
                    {
                        PlayerObjList[(int)GetLVD(GameData.m_PlayerInfoList[i].pos)].transform.Find("ChatPanel").GetComponent<ChatControl>().SetValue(strs[2]);
                        // SoundControl.Instance.PlayChatSound((int)GameData.m_PlayerInfoList[i].sex, int.Parse(strs[2]));
                    }
                }
            }

        }
    }

    /// <summary>
    /// 语音聊天
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="length"></param>
    public void onShowSoundAnimation(ulong guid, float length)
    {
        byte pos = GameDataFunc.GetPlayerInfo(guid).pos;
        Vector3 vecPos = PlayerObjList[(int)GetLVD(pos)].transform.Find("headImage").position;
        StartCoroutine(VoiceChat.Instance.AsynSoundAnimation(vecPos, length));
    }

    /// <summary>
    /// 发送表情
    /// </summary>
    /// <param name="content"></param>
    public void onPlayerSendChatFace(string content)
    {
        StartCoroutine(AsynCreateChatFace(content));
    }
    IEnumerator AsynCreateChatFace(string content)
    {
        string[] strs = content.Split('@');
        PlayerInfo info = GameDataFunc.GetPlayerInfo(ulong.Parse(strs[1]));
        GameObject preObj = Resources.Load<GameObject>("Face/Face" + strs[2]);
        GameObject face = Instantiate<GameObject>(preObj);
        face.transform.parent = transform;
        face.transform.localScale = Vector3.one;
        face.transform.localPosition = PlayerObjList[(int)info.LVD].transform.localPosition;
        SoundManager.Instance.PlaySound(UIPaths.SOUND_CHAT_FACE + strs[2], info.sex);
        yield return new WaitForSeconds(3f);
        Destroy(face);
        yield break;
    }

    #endregion

    /// <summary>
    /// 玩家离开
    /// </summary>
    /// <param name="pos"></param>
    public void onPlayerLeave(byte pos)
    {
        if (pos == LocalPos)
        {
            ManagerScene.Instance.LoadScene(SceneType.Main);
        }
        else
        {
            GameObject obj = PlayerObjList[(int)GetLVD(pos)];
            obj.SetActive(false);
            GameDataFunc.RemovePlayer(pos);
            GameDataFunc.RemovePlayerObj(pos);
        }
    }
    #endregion

    uint ResCardCount
    {
        set
        {
            LBResCardCount.gameObject.SetActive(true);
            LBResCardCount.text = value.ToString();
            SetCardsOnDesk((int)value);
        }
    }

    /// <summary>
    /// 其他玩家创建摸上来的牌
    /// </summary>
    void CreateOtherPlayerGetInCard(Transform TargetParent)
    {
        int activeCount = 0;
        for (int i = 0; i < TargetParent.childCount; i++)
        {
            if (TargetParent.Find(i.ToString()).gameObject.activeInHierarchy)
            {
                activeCount += 1;
            }
            else
            {
                break;
            }
        }
        if (TargetParent.Find((TargetParent.childCount - 1).ToString()) != null)
        {
            Transform tmp = TargetParent.Find((TargetParent.childCount - 1).ToString());
            if (tmp.gameObject.activeInHierarchy)
            {
                //  TargetParent.FindChild(activeCount.ToString()).gameObject.SetActive(true);
                tmp.localPosition = TargetParent.Find((activeCount - 1).ToString()).localPosition + new Vector3(localCardWidth, 0, 0);
            }
            else
            {
                tmp.gameObject.SetActive(true);
                tmp.localPosition = TargetParent.Find((activeCount - 1).ToString()).localPosition + new Vector3(localCardWidth, 0, 0);
            }
        }
    }
    /// <summary>
    /// 创建摸牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="card"></param>
    /// <param name="isHuCard"></param>
    void CreateInCard(byte pos, uint card, bool isHuCard = false)
    {
        ShowOutCardPlayer(pos);
        PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);
        HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(pos);
        GameObject cardObj = null;
        //cardObj = Instantiate(pbCardImage);
        info.localCardList.Add(card);



        switch (info.LVD)
        {
            case LocalViewDirection.LOCAL:
                cardObj = Instantiate(pbCardImage);
                cardObj.transform.parent = LocalParent;
                cardObj.transform.localScale = Vector3.one;
                // cardObj.transform.localPosition = new Vector3(LocalLeftCardStartPos + (info.localCardList.Count) * 75, 0, 0);
                cardObj.transform.localPosition = new Vector3(570, 0, 0);
                cardObj.GetComponent<UISprite>().spriteName = "card_local_max_" + card.ToString();
                cardObj.GetComponent<UISprite>().MakePixelPerfect();
                cardObj.name = card.ToString();
                GameDataFunc.GetTableCardInfoByID(card).RemainedCount -= 1;

                //如果是面牌  标红
                if (card == GameData.m_TableInfo.MagicCard)
                {
                    cardObj.GetComponent<UISprite>().color = new Color(1, 0.8f, 0.8f, 1);
                }

                if (isHuCard)
                {
                    cardObj.GetComponent<UISprite>().color = huPaiCardColor;
                }
                else
                {
                    cardObj.AddComponent<BoxCollider2D>().size = new Vector3(75, 122, 0);
                    UIEventListener.Get(cardObj).onPress = OnPressCard;
                    UIEventListener.Get(cardObj).onClick = OnClickCard;
                    UIEventListener.Get(cardObj).onDrag = OnDrag;
                    UIEventListener.Get(cardObj).onDragEnd = OnDragEnd;
                }

                break;
            case LocalViewDirection.RIGHT:
                CreateOtherPlayerGetInCard(RightOnHand);
                break;
            case LocalViewDirection.UP:
                CreateOtherPlayerGetInCard(TopOnHand);
                break;
            case LocalViewDirection.LEFT:
                CreateOtherPlayerGetInCard(LeftOnHand);
                break;
        }
        infoObj.holdObjList.Add(cardObj);
    }

    /// <summary>
    /// 显示出牌标志位
    /// </summary>
    /// <param name="pos"></param>
    void ShowOutCardPlayer(byte pos)
    {
        ObjCurOutImg.SetActive(true);
        ObjCurOutImg.transform.localPosition = PlayerObjList[(int)GetLVD(pos)].transform.localPosition;
        SetLocations(pos);
        OutCountDown.GetComponent<CountDownControl>().SetCountDown(OutInterval);


    }

    /// <summary>
    /// 重置玩家手牌位置
    /// </summary>
    void ResetLocalHoldCards()
    {
        #region 把万能牌放在最左边
        List<uint> MagicCardList = new List<uint>();
        List<uint> OtherCardList = new List<uint>();
        for (int i = 0; i < GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.Count; i++)
        {
            if (GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList[i] == GameData.m_TableInfo.MagicCard)
            {
                MagicCardList.Add(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList[i]);
            }
            else
            {
                OtherCardList.Add(GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList[i]);
            }
        }
        OtherCardList.Sort((a, b) =>
        {

            return ((int)a - (int)b);
        });


        GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList = new List<uint>();
        GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.AddRange(MagicCardList);
        GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL).localCardList.AddRange(OtherCardList);
        #endregion

        PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalPos);
        HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(LocalPos);
        //info.localCardList.Sort((a, b) =>
        //{
        //    return (int)a - (int)b;
        //});
        int startPos = (int)LocalLeftCardStartPos + (13 - info.localCardList.Count) * 75;
        // int startPos = (int)LocalLeftCardStartPos;
        for (int i = 0; i < info.localCardList.Count; i++)
        {
            GameObject obj = infoObj.holdObjList[i];
            obj.name = info.localCardList[i].ToString();
            obj.transform.localPosition = new Vector3(startPos + i * 75, 0, 0);
            obj.GetComponent<UISprite>().spriteName = "card_local_max_" + info.localCardList[i].ToString();

            //如果是万能牌 标红
            if (info.localCardList[i] == GameData.m_TableInfo.MagicCard)
            {
                obj.GetComponent<UISprite>().color = new Color(1, 0.8f, 0.8f, 1);
            }
            else
            {
                obj.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    /// <summary>
    /// 获得对应位置的视角
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="isIgnoreTotalCount">是否忽略玩家设置总人数，按4人计算</param>
    /// <returns></returns>
    LocalViewDirection GetLVD(byte pos, bool isIgnoreTotalCount = false)
    {
        if (!isIgnoreTotalCount)
        {
            if (GameData.m_TableInfo.configPlayerIndex == 2)
            {
                if (pos == LocalPos) return LocalViewDirection.LOCAL;
                else return LocalViewDirection.UP;
            }
            else
            {
                int rightPos = LocalPos + 1 > 4 ? LocalPos + 1 - 4 : LocalPos + 1;
                int upPos = LocalPos + 2 > 4 ? LocalPos + 2 - 4 : LocalPos + 2;
                int leftPos = LocalPos + 3 > 4 ? LocalPos + 3 - 4 : LocalPos + 3;
                if (pos == rightPos)
                    return LocalViewDirection.RIGHT;
                else if (pos == upPos)
                    return LocalViewDirection.UP;
                else if (pos == leftPos)
                    return LocalViewDirection.LEFT;
            }
        }
        else
        {
            int rightPos, upPos, leftPos = 0;
            if (GameData.m_TableInfo.configPlayerIndex == 2)
            {
                if (LocalPos == 1)
                {
                    rightPos = LocalPos + 1 > 4 ? LocalPos + 1 - 4 : LocalPos + 1;
                    upPos = LocalPos + 2 > 4 ? LocalPos + 2 - 4 : LocalPos + 2;
                    leftPos = LocalPos + 3 > 4 ? LocalPos + 3 - 4 : LocalPos + 3;
                }
                else
                {
                    rightPos = 4;
                    upPos = 1;
                    leftPos = 2;
                    if (pos == 3)
                    {
                        return LocalViewDirection.LOCAL;
                    }
                }
            }
            else
            {
                rightPos = LocalPos + 1 > 4 ? LocalPos + 1 - 4 : LocalPos + 1;
                upPos = LocalPos + 2 > 4 ? LocalPos + 2 - 4 : LocalPos + 2;
                leftPos = LocalPos + 3 > 4 ? LocalPos + 3 - 4 : LocalPos + 3;
            }
            if (pos == rightPos)
                return LocalViewDirection.RIGHT;
            else if (pos == upPos)
                return LocalViewDirection.UP;
            else if (pos == leftPos)
                return LocalViewDirection.LEFT;
        }
        return LocalViewDirection.LOCAL;
    }


    void ShowFangPaoScore()
    {
        if (GameData.m_TableInfo.configDaZiIndex == 0) return;
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            PlayerInfo info = GameData.m_PlayerInfoList[i];
            GameObject obj = PlayerObjList[(int)info.LVD];
            if (info.fangPaoScore > 0)
            {
                UISprite sp = obj.transform.Find("fpScore").GetComponent<UISprite>();
                sp.gameObject.SetActive(true);
                sp.spriteName = "UI_game_icon_bet_" + info.fangPaoScore;
            }
        }
    }

    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="card"></param>
    /// <param name="chooseGo"></param>
    void OutCard(uint card, GameObject chooseGo)
    {
        Log.Debug("出牌");
        curOutWhoOperate = 0;
        chooseGo.transform.localPosition = new Vector3(0, -130, 0);
        //  ClientToServerMsg.Send(Opcodes.Client_PlayerOperate, GameData.m_TableInfo.id, (byte)CardOperateType.ChuPai, card, card, card);
        List<uint> listcard = new List<uint>();
        listcard.Add(card);
        ClientToServerMsg.SendPlayCard(listcard);
    }

    /// <summary>
    /// 同牌显示
    /// </summary>
    /// <param name="card"></param>
    void ShowOutCards(uint card)
    {
        HideOutCards();
        //检查碰
        bool isPeng = false;
        for (int i = 0; i < GameData.m_HoldCardsList.Count; i++)
        {
            HoldCardsObj info = GameData.m_HoldCardsList[i];
            for (int k = 0; k < info.pengGangList.Count; k++)
            {
                PengOrGangObj chpInfo = info.pengGangList[k];
                if (chpInfo.objBase.name == card.ToString() && chpInfo.opType == CardOperateType.PengPai)
                {
                    for (int j = 0; j < chpInfo.objBase.transform.parent.parent.parent.childCount; j++)
                    {
                        Transform tempItem = chpInfo.objBase.transform.parent.parent.parent.Find(j.ToString());
                        if (tempItem.gameObject.activeInHierarchy)
                        {
                            outCards.Add(tempItem.Find("Content").Find(chpInfo.objBase.name).gameObject);
                        }
                    }
                    isPeng = true;
                    break;
                }
            }
            if (isPeng) break;
        }
        if (!isPeng)
        {
            //检查海里的牌
            for (int i = 0; i < GameData.m_HoldCardsList.Count; i++)
            {
                HoldCardsObj info = GameData.m_HoldCardsList[i];
                for (int k = 0; k < info.outObjList.Count; k++)
                {
                    if (info.outObjList[k] != null)
                    {
                        if (info.outObjList[k].name == card.ToString())
                        {
                            outCards.Add(info.outObjList[k]);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < outCards.Count; i++)
        {
            //这里要写一个处理别人刚出的，给你吃碰杠的牌一个特殊标志
            outCards[i].GetComponent<MeshRenderer>().material = ClickedM;
        }
    }
    void HideOutCards()
    {
        for (int i = 0; i < outCards.Count; i++)
        {
            if (outCards[i] != null)
            {
                //outCards[i].GetComponent<UISprite>().color = Color.white;
                //这里要隐藏出的牌
                outCards[i].GetComponent<MeshRenderer>().material = NormalM;

            }
        }
        outCards.Clear();
    }
    PengGangFromDirection GetPengGangFromDirection(LocalViewDirection dir, byte outPos)
    {
        PengGangFromDirection result = PengGangFromDirection.LOCAL;
        LocalViewDirection outDir = GameDataFunc.GetPlayerInfo(outPos).LVD;
        switch (dir)
        {
            case LocalViewDirection.LOCAL:
                switch (outDir)
                {
                    case LocalViewDirection.LOCAL:
                        result = PengGangFromDirection.LOCAL;
                        break;
                    case LocalViewDirection.RIGHT:
                        result = PengGangFromDirection.XIAJIA;
                        break;
                    case LocalViewDirection.UP:
                        result = PengGangFromDirection.DUIJIA;
                        break;
                    case LocalViewDirection.LEFT:
                        result = PengGangFromDirection.SHANGJIA;
                        break;
                }
                break;
            case LocalViewDirection.RIGHT:
                switch (outDir)
                {
                    case LocalViewDirection.LOCAL:
                        result = PengGangFromDirection.SHANGJIA;
                        break;
                    case LocalViewDirection.RIGHT:
                        result = PengGangFromDirection.LOCAL;
                        break;
                    case LocalViewDirection.UP:
                        result = PengGangFromDirection.XIAJIA;
                        break;
                    case LocalViewDirection.LEFT:
                        result = PengGangFromDirection.DUIJIA;
                        break;
                }
                break;
            case LocalViewDirection.UP:
                switch (outDir)
                {
                    case LocalViewDirection.LOCAL:
                        result = PengGangFromDirection.DUIJIA;
                        break;
                    case LocalViewDirection.RIGHT:
                        result = PengGangFromDirection.SHANGJIA;
                        break;
                    case LocalViewDirection.UP:
                        result = PengGangFromDirection.LOCAL;
                        break;
                    case LocalViewDirection.LEFT:
                        result = PengGangFromDirection.XIAJIA;
                        break;
                }
                break;
            case LocalViewDirection.LEFT:
                switch (outDir)
                {
                    case LocalViewDirection.LOCAL:
                        result = PengGangFromDirection.XIAJIA;
                        break;
                    case LocalViewDirection.RIGHT:
                        result = PengGangFromDirection.DUIJIA;
                        break;
                    case LocalViewDirection.UP:
                        result = PengGangFromDirection.SHANGJIA;
                        break;
                    case LocalViewDirection.LEFT:
                        result = PengGangFromDirection.LOCAL;
                        break;
                }
                break;
        }
        return result;
    }



    void HideOperateBtn()
    {
        btnGuo.SetActive(false);
        btnHu.SetActive(false);
        btnGang.SetActive(false);
        btnPeng.SetActive(false);
    }
    /// <summary>
    /// 是否有碰
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    bool IsOtherPeng(uint card)
    {
        PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalPos);
        List<uint> limitList = info.limitPengCardList.FindAll((a) => { return a == card; });
        if (limitList.Count > 0) return false;
        List<uint> tempList = info.localCardList.FindAll((a) => { return a == card; });
        if (tempList.Count >= 2)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 是否有杠
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    bool IsOtherGang(uint card)
    {
        PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalPos);
        List<uint> tempList = info.localCardList.FindAll((a) => { return a == card; });
        if (tempList.Count == 3)
            return true;
        else
            return false;
    }
    List<uint> GetGangList()
    {
        List<uint> cards = new List<uint>();
        PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalPos);
        for (int i = 0; i < info.operateCardList.Count; i++)
        {
            if (info.operateCardList[i].opType == CatchType.Peng)
            {
                for (int j = 0; j < info.localCardList.Count; j++)
                {
                    if (info.operateCardList[i].opCard == info.localCardList[j])
                    {
                        cards.Add(info.localCardList[j]);
                        break;
                    }
                }
            }
        }
        for (int i = 0; i < info.localCardList.Count; i++)
        {
            List<uint> count = info.localCardList.FindAll((card) => { return card == info.localCardList[i]; });
            if (count.Count == 4)
            {
                bool isCz = false;
                for (int k = 0; k < cards.Count; k++)
                {
                    if (info.localCardList[i] == cards[k])
                    {
                        isCz = true;
                        break;
                    }
                }
                if (!isCz)
                    cards.Add(info.localCardList[i]);
            }
        }
        return cards;
    }

    #region  出牌类型转换
    CatchType GetPengGangType(CardOperateType type)
    {
        switch (type)
        {
            case CardOperateType.PengPai:
                return CatchType.Peng;
            case CardOperateType.GangPai:
                return CatchType.Gang;
        }
        return CatchType.Peng;
    }
    CardOperateType GetPengGangType(CatchType type)
    {
        switch (type)
        {
            case CatchType.Peng:
                return CardOperateType.PengPai;
            case CatchType.Gang:
            case CatchType.AnGang:
            case CatchType.BuGang:
                return CardOperateType.GangPai;
        }
        return CardOperateType.PengPai;
    }

    #endregion


    /// <summary>
    /// 显示胡牌特效
    /// </summary>
    public void ViewHu()
    {
        if (GameData.m_RoundOverInfo.isHuPai)
        {
            PlayerInfo info = GameDataFunc.GetPlayerInfo(GameData.m_RoundOverInfo.huPos);
            SoundManager.Instance.PlaySound(UIPaths.SOUND_HU, info.sex);
            StartCoroutine(AsynCreateEffect(info.LVD, CardOperateType.HuPai));
            if (GameData.m_RoundOverInfo.isDianPaoHu)
            {
                if (info.pos == LocalPos) CreateInCard(info.pos, GameData.m_RoundOverInfo.dianPaoCard, true);
            }
            else if (GameData.m_RoundOverInfo.isQiangGuangHu)
            {
                if (info.pos == LocalPos) CreateInCard(info.pos, GameData.m_RoundOverInfo.qiangGangCard, true);
            }
            else
            {
                if (LocalPos == GameData.m_RoundOverInfo.huPos)
                {
                    HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(GameData.m_RoundOverInfo.huPos);
                    if (infoObj != null)
                    {
                        for (int i = 0; i < infoObj.holdObjList.Count; i++)
                        {
                            if (uint.Parse(infoObj.holdObjList[i].name) == GameData.m_RoundOverInfo.huCard)
                            {
                                infoObj.holdObjList[i].GetComponent<UISprite>().color = huPaiCardColor;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 设置当前剩余牌数
    /// </summary>
    /// <param name="RemainedCards"></param>
    void SetCardsOnDesk(int RemainedCards)
    {
        //SelfOnDesk
        SelfOnDesk.SetActive(true);
        RightOnDesk.SetActive(true);
        LeftOnDesk.SetActive(true);
        TopOnDesk.SetActive(true);
        int startDisableCount = TotalCardsCount - RemainedCards - GameData.m_TableInfo.GangCount - GameData.m_TableInfo.ChiMianCount;
        for (int i = 0; i < startDisableCount; i++)
        {
            AllCardsOnDesk[i].SetActive(false);
        }
        for (int i = 0; i < GameData.m_TableInfo.GangCount + GameData.m_TableInfo.ChiMianCount; i++)
        {
            int number = int.Parse(AllCardsOnDesk[TotalCardsCount - i - 1].name);
            AllCardsOnDesk[TotalCardsCount - i - 1].SetActive(false);//先直接隐藏掉这张牌
            if (number % 2 == 1)//判断是在上还是在下，如果被隐藏的牌在下面，要把上面的那张牌移动到下面去
            {

                AllCardsOnDesk[TotalCardsCount - i - 2].transform.position = AllCardsOnDesk[TotalCardsCount - i - 1].transform.position;
                AllCardsOnDesk[TotalCardsCount - i - 2].transform.Find("Shadow").gameObject.SetActive(true);
            }

        }

        CreatMianCard();//每次都要创建面牌
    }

    /// <summary>
    /// 重置桌上的牌136张
    /// </summary>
    public void SetCardOnDesk()
    {
        SelfOnDesk.SetActive(true);
        RightOnDesk.SetActive(true);
        LeftOnDesk.SetActive(true);
        TopOnDesk.SetActive(true);
    }
    /// <summary>
    /// 清理所有桌面上已经打出来的牌
    /// </summary>
    void DestroyAllOutCards()
    {
        for (int i = 0; i < SelfChu.childCount; i++)
        {
            for (int j = 0; j < SelfChu.Find(i.ToString()).Find("Content").childCount; j++)
            {
                DestroyImmediate(SelfChu.Find(i.ToString()).Find("Content").GetChild(j).gameObject);
            }
        }
        for (int i = 0; i < TopChu.childCount; i++)
        {
            for (int j = 0; j < TopChu.Find(i.ToString()).Find("Content").childCount; j++)
            {
                DestroyImmediate(TopChu.Find(i.ToString()).Find("Content").GetChild(j).gameObject);
            }
        }
        for (int i = 0; i < LeftChu.childCount; i++)
        {
            for (int j = 0; j < LeftChu.Find(i.ToString()).Find("Content").childCount; j++)
            {
                DestroyImmediate(LeftChu.Find(i.ToString()).Find("Content").GetChild(j).gameObject);
            }
        }
        for (int i = 0; i < RightChu.childCount; i++)
        {
            for (int j = 0; j < RightChu.Find(i.ToString()).Find("Content").childCount; j++)
            {
                DestroyImmediate(RightChu.Find(i.ToString()).Find("Content").GetChild(j).gameObject);
            }
        }
    }

    void SetPengGangPositions(Transform ObjParent, PengGangFromDirection Direction, CatchType OP)
    {
        switch (OP)
        {
            case CatchType.AnGang:
                for (int i = 0; i < ObjParent.childCount; i++)
                {
                    GameObject currentObj = ObjParent.Find(i.ToString()).gameObject;
                    if (i < 3)
                    {

                    }
                }
                break;
            case CatchType.BuGang:
                break;
            case CatchType.Peng:
                break;
            case CatchType.Gang:
                break;
        }
    }

    /// <summary>
    /// 设置子级显示的牌数量
    /// </summary>
    /// <param name="TargetTrans">对应的父级</param>
    /// <param name="Count">数量</param>
    void SetActiveChildPaiCount(Transform TargetTrans, int Count)
    {

        //for (int i = 0; i < Count; i++)
        //{
        //    TargetTrans.FindChild(i.ToString()).gameObject.SetActive(true);
        //}

        for (int i = 0; i < TargetTrans.childCount; i++)
        {
            Transform item = TargetTrans.GetChild(i);
            int re = int.MinValue;
            if (int.TryParse(item.name, out re))
            {
                if (re <= Count - 1)
                {
                    item.gameObject.SetActive(true);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }



    /// <summary>
    /// 设置骰子
    /// </summary>
    /// <param name="TargetDice"></param>
    /// <param name="Number"></param>
    void SetDice(GameObject TargetDice, int Number)
    {
        TargetDice.transform.localEulerAngles = DicePointRList[Number - 1];
    }

    /// <summary>
    /// 移除其他玩家手牌
    /// </summary>
    /// <param name="TargetParent"></param>
    /// <param name="RemoveCount"></param>
    void RemoveOtherPlayerHoldCard(Transform TargetParent, int RemoveCount)
    {
        Debug.Log("remove " + TargetParent.parent.name + " " + RemoveCount.ToString());
        int activeCount = 0;
        for (int i = 0; i < TargetParent.childCount; i++)
        {
            if (TargetParent.Find(i.ToString()).gameObject.activeInHierarchy)
            {
                activeCount += 1;
            }
        }
        for (int i = 0; i < RemoveCount; i++)
        {
            if (i == 0)
            {
                if (TargetParent.Find((TargetParent.childCount - 1).ToString()).gameObject.activeInHierarchy)
                {
                    TargetParent.Find((TargetParent.childCount - 1).ToString()).gameObject.SetActive(false);
                    activeCount -= 1;
                    continue;
                }
            }
            Transform tmp = TargetParent.Find((activeCount - 1).ToString());
            tmp.gameObject.SetActive(false);
            activeCount -= 1;
        }
    }

    /// <summary>
    /// 移除刚刚打出来的牌
    /// </summary>
    /// <param name="TargetParent"></param>
    /// <param name="pos"></param>
    public static void RemoveOutCardObj(Transform TargetParent, byte pos)
    {
        HoldCardsObj info = GameDataFunc.GetHoldCardObj(pos);
        TargetParent.Find((info.outObjList.Count - 1).ToString()).gameObject.SetActive(false);
        info.outObjList.RemoveAt(info.outObjList.Count - 1);
    }


    /// <summary>
    /// 清理桌上的东西
    /// </summary>
    void ClearTable()
    {
        if (MianPaiCard != null)
        {
            Destroy(MianPaiCard);
        }
        IsDealCardOver = false;
        isDragOutCard = true;//是否可以拖动出牌
        isCurDragOutCard = false;//是否在拖动出牌
        if (effectObjList.Count > 0)
        {
            foreach (GameObject ef in effectObjList)
            {
                Destroy(ef);
            }
        }
        effectObjList.Clear();

        UIManager.Instance.HideUIPanel(UIPaths.UIPanel_RoundOver);
        ObjOutSign.SetActive(false);

        foreach (TableCardInfo tmpInfo in GameData.AllCardsInfo)
        {
            tmpInfo.ShownCount = 0;
            tmpInfo.RemainedCount = tmpInfo.TotalCount;
        }
        LBResCardCount.gameObject.SetActive(false);


        GameDataFunc.ClearData();
        GameDataFunc.ClearDataObj();
        HideOperateBtn();
        curOutWhoOperate = 0;
        InCardPos = 0;
        totalGangCount = 0;
        GameData.m_TableInfo.GangCount = 0;
        GameData.m_TableInfo.ChiMianCount = 0;
        outCards.Clear();

        foreach (Transform item in LocalParent)
            Destroy(item.gameObject);
        foreach (Transform item in RightParent)
            Destroy(item.gameObject);
        foreach (Transform item in UpParent)
            Destroy(item.gameObject);
        foreach (Transform item in LeftParent)
            Destroy(item.gameObject);

        foreach (Transform item in transform.Find("OutCardParent"))
            Destroy(item.gameObject);

        SetActiveChildPaiCount(LeftOnHand, 0);
        SetActiveChildPaiCount(RightOnHand, 0);
        SetActiveChildPaiCount(TopOnHand, 0);
        SetActiveChildPaiCount(SelfChu, 0);
        SetActiveChildPaiCount(RightChu, 0);
        SetActiveChildPaiCount(LeftChu, 0);
        SetActiveChildPaiCount(TopChu, 0);
        SetActiveChildPaiCount(SelfCPG, 0);
        SetActiveChildPaiCount(LeftCPG, 0);
        SetActiveChildPaiCount(RightCPG, 0);
        SetActiveChildPaiCount(TopCPG, 0);
        SetActiveChildPaiCount(SelfOver, 0);
        SetActiveChildPaiCount(LeftOver, 0);
        SetActiveChildPaiCount(RightOver, 0);
        SetActiveChildPaiCount(TopOver, 0);
        SetActiveChildPaiCount(SelfHu, 0);
        SetActiveChildPaiCount(RightHu, 0);
        SetActiveChildPaiCount(LeftHu, 0);
        SetActiveChildPaiCount(TopHu, 0);


        SelfOnDesk.SetActive(false);
        RightOnDesk.SetActive(false);
        LeftOnDesk.SetActive(false);
        TopOnDesk.SetActive(false);

        //更新积分
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            PlayerInfo info = GameData.m_PlayerInfoList[i];
            GameObject obj = PlayerObjList[(int)info.LVD];
            obj.transform.Find("name").GetComponent<UILabel>().text = info.name;
            if (info.score < 0)
                obj.transform.Find("score").GetComponent<UILabel>().color = Color.red;
            else
                obj.transform.Find("score").GetComponent<UILabel>().color = Color.white;
          
            obj.transform.Find("score").GetComponent<UILabel>().text = info.score.ToString();
            obj.transform.Find("score").GetComponent<UILabel>().text = info.Gold.ToString();

            obj.transform.Find("fpScore").gameObject.SetActive(false);

        }


        ResetMianCard();

    }

    /// <summary>
    /// 清除打出的面牌
    /// </summary>
    public void ResetMianCard()
    {
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(GameData.m_PlayerInfoList[i].pos);
            for (int j = 0; j < infoObj.outMianCardObjList.Count; j++)
            {
                Destroy(infoObj.outMianCardObjList[j]);
            }
            infoObj.outMianCardObjList = new List<GameObject>();

            GameData.m_PlayerInfoList[i].MianCardList = new List<uint>();
        }
        GameData.m_TableInfo.ChiMianCount = 0;
    }


    //显示庄的图标
    void ShowMakerImg()
    {
        ObjMakerImg.SetActive(true);
        ObjMakerImg.transform.localPosition = PlayerObjList[(int)GetLVD(GameData.m_TableInfo.makerPos)].transform.localPosition + new Vector3(-35, -10);
    }
    /// <summary>
    /// 显示房主图标
    /// </summary>
    void ShowFangZhuImg()
    {

        if (!GameData.m_TableInfo.IsDaiLiCreat && !GameData.m_TableInfo.IsPiPei)
        {
            if (!GameData.m_TableInfo.IsDaiLiCreat)
            {
                PlayerInfo info = GameDataFunc.GetPlayerInfo(GameData.m_TableInfo.fangZhuGuid);
                ObjFangZhuImg.transform.parent = PlayerObjList[(int)info.LVD].transform;
                ObjFangZhuImg.transform.localPosition = new Vector3(-14, 31);
            }

        }
        else
        {
            ObjFangZhuImg.gameObject.SetActive(false);
        }
        //PlayerInfo info = GameDataFunc.GetPlayerInfo(GameData.m_TableInfo.fangZhuGuid);
        //ObjFangZhuImg.transform.parent = PlayerObjList[(int)info.LVD].transform;
        //ObjFangZhuImg.transform.localPosition = new Vector3(-14, 31);
    }


    /// <summary>
    /// 玩家出牌特效 和出的牌
    /// </summary>
    /// <param name="playerGuid"></param>
    /// <param name="card"></param>
    /// <returns></returns>
    IEnumerator AsynCreatePlayerOutCard(ulong playerGuid, uint card)
    {

        PlayerInfo info = GameDataFunc.GetPlayerInfo(playerGuid);
        HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(info.pos);
        GameObject item = null;
        item = Instantiate<GameObject>(MJPaiParent.Find(card.ToString()).gameObject);
        Vector3 endPos = Vector3.zero;
        item.name = card.ToString();
        info.outCardList.Add(card);//打出的牌的数据
        infoObj.outObjList.Add(item);//打出的牌的物体
        SoundManager.Instance.PlaySound(UIPaths.SOUND_OUT_CARD + card, info.sex);
        switch (info.LVD)
        {
            case LocalViewDirection.LOCAL:
                GameDataFunc.RemoverHoldCard(card, info.pos);
                GameDataFunc.RemoveHoldCardObj(card, info.pos);
                ResetLocalHoldCards();//手牌复位
                HideOutCards();
                ObjPlayerChooseCard = null;
                SetActiveChildPaiCount(SelfChu, info.outCardList.Count);//显示自己打出的牌
                StartCoroutine(AsyncSetMJPai(SelfChu.Find((info.outCardList.Count - 1).ToString()).Find("Content"), item));//显示自己打出的牌
                break;
            case LocalViewDirection.RIGHT:
                RemoveOtherPlayerHoldCard(RightOnHand, 1);
                SetActiveChildPaiCount(RightChu, info.outCardList.Count);
                StartCoroutine(AsyncSetMJPai(RightChu.Find((info.outCardList.Count - 1).ToString()).Find("Content"), item));
                GameDataFunc.GetTableCardInfoByID(card).RemainedCount -= 1;
                break;
            case LocalViewDirection.UP:
                RemoveOtherPlayerHoldCard(TopOnHand, 1);
                SetActiveChildPaiCount(TopChu, info.outCardList.Count);
                StartCoroutine(AsyncSetMJPai(TopChu.Find((info.outCardList.Count - 1).ToString()).Find("Content"), item));
                GameDataFunc.GetTableCardInfoByID(card).RemainedCount -= 1;
                break;
            case LocalViewDirection.LEFT:
                RemoveOtherPlayerHoldCard(LeftOnHand, 1);
                SetActiveChildPaiCount(LeftChu, info.outCardList.Count);
                StartCoroutine(AsyncSetMJPai(LeftChu.Find((info.outCardList.Count - 1).ToString()).Find("Content"), item));
                GameDataFunc.GetTableCardInfoByID(card).RemainedCount -= 1;
                break;
        }
        SoundManager.Instance.PlaySound(UIPaths.SOUND_CHUPAI);
        yield return new WaitForEndOfFrame();
        item.name = card.ToString();
        endPos = item.transform.position;
        //item.GetComponent<UISprite>().MakePixelPerfect();
        //TweenScale.Begin(item, 0.2f, Vector3.one);
        ObjOutSign.gameObject.SetActive(true);
        Vector3 startPos = ObjOutSign.transform.position;
        ObjOutSign.transform.position = endPos + new Vector3(0, outMarkHeight, 0);
        iTween.MoveFrom(ObjOutSign.gameObject, startPos, 0.2f);
        yield break;
    }


    /// <summary>
    /// 碰杠的处理
    /// </summary>
    /// <param name="playerGuid"></param>
    /// <param name="opType"></param>
    /// <param name="card"></param>
    /// <param name="outPos"></param>
    /// <param name="isAnGang"></param>
    /// <param name="isNormal"></param>
    /// <returns></returns>
    IEnumerator AsynPengGang(ulong playerGuid, CardOperateType opType, uint card, byte outPos, bool isAnGang, bool isNormal = true)
    {
        PlayerInfo info = GameDataFunc.GetPlayerInfo(playerGuid);
        HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(info.pos);
        List<uint> cards = new List<uint>();

        bool isBuGang = false;
        int startIndex = 1;
        switch (opType)
        {
            case CardOperateType.PengPai:
                cards.Add(card); cards.Add(card); cards.Add(card);
                break;
            case CardOperateType.GangPai:
                totalGangCount += 1;
                GameData.m_TableInfo.GangCount += 1;
                if (info.pos == outPos) startIndex = 0;
                cards.Add(card); cards.Add(card); cards.Add(card); cards.Add(card);
                for (int i = 0; i < info.operateCardList.Count; i++)
                {
                    if (info.operateCardList[i].opType == CatchType.Peng)
                    {
                        if (info.operateCardList[i].opCard == card)
                        {
                            info.operateCardList[i].opType = CatchType.BuGang;
                            isBuGang = true;
                            itemObj = infoObj.pengGangList[i].objBase;
                            break;
                        }
                    }
                }
                break;
        }


        if (isBuGang)
        {
            GameDataFunc.GetTableCardInfoByID(card).RemainedCount = 0;
            Log.Debug("补杠");
            SetActiveChildPaiCount(itemObj.transform.parent.parent.parent, 4);
            GameObject newPai = Instantiate(itemObj);
            newPai.name = itemObj.name;
            StartCoroutine(AsyncSetMJPai(itemObj.transform.parent.parent.parent.Find("3").Find("Content"), newPai));
            if (info.LVD == LocalViewDirection.LOCAL)
            {
                if (isNormal)
                {
                    GameDataFunc.RemoverHoldCard(card, info.pos);
                    GameDataFunc.RemoveHoldCardObj(card, info.pos);
                    ResetLocalHoldCards();
                }
            }
            switch (info.LVD)
            {
                case LocalViewDirection.RIGHT:
                    RemoveOtherPlayerHoldCard(RightOnHand, 1);
                    break;
                case LocalViewDirection.LEFT:
                    RemoveOtherPlayerHoldCard(LeftOnHand, 1);
                    break;
                case LocalViewDirection.UP:
                    RemoveOtherPlayerHoldCard(TopOnHand, 1);
                    break;
            }

            #region
            //if (isNormal && LocalPos != info.pos)
            //{
            //    List<uint> tempList = new List<uint>();
            //    PlayerInfo localInfo = GameDataFunc.GetPlayerInfo(LocalPos);
            //    tempList.AddRange(localInfo.localCardList);
            //    tempList.Add(card);
            //    List<List<uint>> operateList = new List<List<uint>>();
            //    List<uint> allCards = new List<uint>();
            //    for (int i = 0; i < localInfo.operateCardList.Count; i++)
            //    {
            //        List<uint> opCards = new List<uint>();
            //        if (localInfo.operateCardList[i].opType == CatchType.Peng)
            //        {
            //            opCards.Add(localInfo.operateCardList[i].opCard);
            //            opCards.Add(localInfo.operateCardList[i].opCard);
            //            opCards.Add(localInfo.operateCardList[i].opCard);
            //        }
            //        else
            //        {
            //            opCards.Add(localInfo.operateCardList[i].opCard);
            //            opCards.Add(localInfo.operateCardList[i].opCard);
            //            opCards.Add(localInfo.operateCardList[i].opCard);
            //            opCards.Add(localInfo.operateCardList[i].opCard);
            //        }
            //        operateList.Add(opCards);
            //        allCards.AddRange(opCards.ToArray());
            //    }
            //    bool isHu = false;
            //    //if (GameData.m_TableInfo.configFangChongIndex == 0)
            //    //{
            //    //    allCards.AddRange(localInfo.localCardList.ToArray());
            //    //    int menCount = myXYHelper.Instance.getTypeSeCount(allCards);
            //    //    if (menCount <= 1) isHu = myXYHelper.Instance.checkHu(operateList, tempList, 0);
            //    //}
            //  //  else
            //        isHu = myXYHelper.Instance.ZBCheckHu(operateList, tempList, GameData.m_TableInfo.MagicCard);
            //    int index = 0;
            //    if (isHu)
            //    {
            //        btnGuo.SetActive(true);
            //        btnGuo.transform.localPosition = new Vector3(-140 * index + 300, 0, 0);
            //        index++;

            //        btnHu.SetActive(true);
            //        btnHu.transform.localPosition = new Vector3(-140 * index + 300, 0, 0);
            //        index++;
            //    }
            //}

            #endregion
        }
        else
        {
            switch (opType)
            {
                case CardOperateType.PengPai:
                    GameDataFunc.GetTableCardInfoByID(card).RemainedCount -= 3;
                    ObjOutSign.gameObject.SetActive(false);
                    if (isNormal)
                    {
                        OpreateCardInfo cpg = new OpreateCardInfo();
                        cpg.pos = outPos;
                        cpg.opType = GetPengGangType(opType);
                        cpg.opCard = card;
                        info.operateCardList.Add(cpg);
                        switch (info.LVD)
                        {
                            case LocalViewDirection.RIGHT:
                                RemoveOtherPlayerHoldCard(RightOnHand, 2);//移除玩家手牌
                                break;
                            case LocalViewDirection.LEFT:
                                RemoveOtherPlayerHoldCard(LeftOnHand, 2);
                                break;
                            case LocalViewDirection.UP:
                                RemoveOtherPlayerHoldCard(TopOnHand, 2);
                                break;
                        }
                    }
                    break;
                case CardOperateType.GangPai:
                    GameDataFunc.GetTableCardInfoByID(card).RemainedCount = 0;

                    ObjOutSign.gameObject.SetActive(false);
                    if (isNormal)
                    {
                        OpreateCardInfo cpg = new OpreateCardInfo();
                        cpg.pos = outPos;
                        cpg.opType = GetPengGangType(opType);
                        cpg.opCard = card;
                        info.operateCardList.Add(cpg);
                        switch (info.LVD)
                        {
                            case LocalViewDirection.RIGHT:
                                if (isAnGang)
                                {
                                    RemoveOtherPlayerHoldCard(RightOnHand, 4);
                                }
                                else
                                {
                                    RemoveOtherPlayerHoldCard(RightOnHand, 3);
                                }
                                break;
                            case LocalViewDirection.LEFT:
                                if (isAnGang)
                                {
                                    RemoveOtherPlayerHoldCard(LeftOnHand, 4);
                                }
                                else
                                {
                                    RemoveOtherPlayerHoldCard(LeftOnHand, 3);
                                }
                                break;
                            case LocalViewDirection.UP:
                                if (isAnGang)
                                {
                                    RemoveOtherPlayerHoldCard(TopOnHand, 4);
                                }
                                else
                                {
                                    RemoveOtherPlayerHoldCard(TopOnHand, 3);
                                }
                                break;
                        }
                    }
                    break;
            }
            if (info.pos != outPos && isNormal)
            {
                GameDataFunc.RemoveOutCardInfo(outPos);
                HoldCardsObj tmpInfo = GameDataFunc.GetHoldCardObj(outPos);
                switch (tmpInfo.LVD)
                {
                    case LocalViewDirection.LEFT:
                        RemoveOutCardObj(LeftChu, outPos);
                        break;
                    case LocalViewDirection.RIGHT:
                        RemoveOutCardObj(RightChu, outPos);
                        break;
                    case LocalViewDirection.UP:
                        RemoveOutCardObj(TopChu, outPos);
                        break;
                    case LocalViewDirection.LOCAL:
                        RemoveOutCardObj(SelfChu, outPos);
                        break;
                }
            }
            int count = infoObj.pengGangList.Count;
            switch (info.LVD)
            {
                case LocalViewDirection.LOCAL:
                    if (isNormal)
                    {
                        for (int i = startIndex; i < cards.Count; i++)
                        {
                            GameDataFunc.RemoverHoldCard(cards[i], info.pos);
                            GameDataFunc.RemoveHoldCardObj(cards[i], info.pos);
                        }
                        ResetLocalHoldCards();
                    }
                    SetActiveChildPaiCount(SelfCPG, count + 1);
                    GameObject cpgObj = null;
                    switch (GetPengGangFromDirection(info.LVD, outPos))
                    {
                        case PengGangFromDirection.DUIJIA:
                            cpgObj = Instantiate(CPGOnDuiJia);
                            break;
                        case PengGangFromDirection.LOCAL:
                            cpgObj = Instantiate(CPGOnLocal);
                            break;
                        case PengGangFromDirection.SHANGJIA:
                            cpgObj = Instantiate(CPGOnShangJia);
                            break;
                        case PengGangFromDirection.XIAJIA:
                            cpgObj = Instantiate(CPGOnXiaJia);
                            break;
                    }
                    //生成碰杠的牌
                    StartCoroutine(AsyncSetCPGSinglePairt(SelfCPG.Find(count.ToString()), cpgObj, count.ToString(), cards, isAnGang));//必须等这个线程结束才能跑下一句
                    break;
                case LocalViewDirection.RIGHT:
                    SetActiveChildPaiCount(RightCPG, count + 1);
                    GameObject cpgObj1 = null;
                    switch (GetPengGangFromDirection(info.LVD, outPos))
                    {
                        case PengGangFromDirection.DUIJIA:
                            cpgObj1 = Instantiate(CPGOnDuiJia);
                            break;
                        case PengGangFromDirection.LOCAL:
                            cpgObj1 = Instantiate(CPGOnLocal);
                            break;
                        case PengGangFromDirection.SHANGJIA:
                            cpgObj1 = Instantiate(CPGOnShangJia);
                            break;
                        case PengGangFromDirection.XIAJIA:
                            cpgObj1 = Instantiate(CPGOnXiaJia);
                            break;
                    }
                    StartCoroutine(AsyncSetCPGSinglePairt(RightCPG.Find(count.ToString()), cpgObj1, count.ToString(), cards, isAnGang));//必须等这个线程结束才能跑下一句
                    break;
                case LocalViewDirection.UP:
                    SetActiveChildPaiCount(TopCPG, count + 1);
                    GameObject cpgObj2 = null;
                    switch (GetPengGangFromDirection(info.LVD, outPos))
                    {
                        case PengGangFromDirection.DUIJIA:
                            cpgObj2 = Instantiate(CPGOnDuiJia);
                            break;
                        case PengGangFromDirection.LOCAL:
                            cpgObj2 = Instantiate(CPGOnLocal);
                            break;
                        case PengGangFromDirection.SHANGJIA:
                            cpgObj2 = Instantiate(CPGOnShangJia);
                            break;
                        case PengGangFromDirection.XIAJIA:
                            cpgObj2 = Instantiate(CPGOnXiaJia);
                            break;
                    }
                    StartCoroutine(AsyncSetCPGSinglePairt(TopCPG.Find(count.ToString()), cpgObj2, count.ToString(), cards, isAnGang));//必须等这个线程结束才能跑下一句
                    break;
                case LocalViewDirection.LEFT:
                    SetActiveChildPaiCount(LeftCPG, count + 1);
                    GameObject cpgObj3 = null;
                    switch (GetPengGangFromDirection(info.LVD, outPos))
                    {
                        case PengGangFromDirection.DUIJIA:
                            cpgObj3 = Instantiate(CPGOnDuiJia);
                            break;
                        case PengGangFromDirection.LOCAL:
                            cpgObj3 = Instantiate(CPGOnLocal);
                            break;
                        case PengGangFromDirection.SHANGJIA:
                            cpgObj3 = Instantiate(CPGOnShangJia);
                            break;
                        case PengGangFromDirection.XIAJIA:
                            cpgObj3 = Instantiate(CPGOnXiaJia);
                            break;
                    }
                    StartCoroutine(AsyncSetCPGSinglePairt(LeftCPG.Find(count.ToString()), cpgObj3, count.ToString(), cards, isAnGang));//必须等这个线程结束才能跑下一句
                    break;
            }
            PengOrGangObj objInfo = new PengOrGangObj();
            objInfo.pos = outPos;
            objInfo.opType = opType;
            objInfo.objBase = itemObj;
            infoObj.pengGangList.Add(objInfo);
            yield break;
        }
        //yield return new WaitForEndOfFrame();

    }

    /// <summary>
    /// 操作对象 特效
    /// </summary>
    /// <param name="lvd"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    IEnumerator AsynCreateEffect(LocalViewDirection lvd, CardOperateType type, int AddType = 0)
    {
        if (pbEffectItem == null) pbEffectItem = Resources.Load<GameObject>("Item/EffectImage");
        GameObject effect = Instantiate(pbEffectItem);
        effectObjList.Add(effect);
        effect.transform.parent = transform;
        effect.transform.localScale = Vector3.one * 1.1f;
        switch (lvd)
        {
            case LocalViewDirection.LOCAL:
                effect.transform.localPosition = new Vector3(0, -150, 0);
                break;
            case LocalViewDirection.RIGHT:
                effect.transform.localPosition = new Vector3(300, 0);
                break;
            case LocalViewDirection.UP:
                effect.transform.localPosition = new Vector3(0, 150, 0);
                break;
            case LocalViewDirection.LEFT:
                effect.transform.localPosition = new Vector3(-300, 0);
                break;
        }
        UISprite sp = effect.GetComponent<UISprite>();
        switch (type)
        {
            case CardOperateType.PengPai:
                sp.spriteName = "UI_game_comic_peng";
                break;
            case CardOperateType.GangPai:
                sp.spriteName = "UI_game_comic_gang";
                break;
            case CardOperateType.HuPai:
                sp.spriteName = "UI_game_comic_hu";
                break;
            default:
                switch (AddType)
                {
                    case 1://吃面
                        sp.spriteName = "UI_game_icon_ChiMian";
                        break;
                    case 2://载宝
                        sp.spriteName = "UI_game_icon_ZaiBao";
                        break;
                }
                break;

        }

        sp.MakePixelPerfect();
        TweenScale.Begin(effect, 0.1f, Vector3.one * 1.2f);
        yield return new WaitForSeconds(0.1f);
        TweenScale.Begin(effect, 0.1f, Vector3.one);
        yield return new WaitForSeconds(0.9f);
        if (type != CardOperateType.HuPai)
            Destroy(effect);
        yield break;
    }
    IEnumerator AsynRoundOver()
    {
        yield return new WaitForSeconds(2);
        // GameData.m_TableInfo.nextReadyTime = 20;
        UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_RoundOver);

    }

    #region
    /// <summary>
    /// 显示结束的操作
    /// </summary>
    /// <returns></returns>
    //IEnumerator AsynCreateOtherCards()
    //{
    //    bool isHuPai = false;
    //    int index = 0;
    //    HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(LocalViewDirection.UP);
    //    PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalViewDirection.UP);
    //    if (info != null)
    //    {
    //        info.localCardList.Sort((a, b) =>
    //        {
    //            return ((int)a - (int)b);
    //        });

    //        if (info.pos == GameData.m_RoundOverInfo.huPos) isHuPai = true;
    //        else isHuPai = false;
    //        index = 0;
    //        SetActiveChildPaiCount(TopOver, info.localCardList.Count);//显示结束的牌
    //        SetActiveChildPaiCount(TopOnHand, 0);//隐藏手牌
    //        for (int i = 0; i < info.localCardList.Count; i++)
    //        {

    //            GameObject tmpCard = Instantiate(MJPaiParent.FindChild(info.localCardList[i].ToString()).gameObject);
    //            StartCoroutine(AsyncSetMJPai(TopOver.FindChild(i.ToString()).FindChild("Content"), tmpCard));
    //            if (isHuPai && index == 0 && info.localCardList[i] == GameData.m_RoundOverInfo.huCard)
    //            {
    //                index++;
    //                SetActiveChildPaiCount(TopHu, 1);
    //                StartCoroutine(AsyncSetMJPai(TopHu.FindChild("0").FindChild("Content"), tmpCard));
    //            }
    //        }
    //    }



    //    infoObj = GameDataFunc.GetHoldCardObj(LocalViewDirection.RIGHT);
    //    info = GameDataFunc.GetPlayerInfo(LocalViewDirection.RIGHT);
    //    if (info != null)
    //    {
    //        info.localCardList.Sort((a, b) =>
    //        {
    //            return ((int)a - (int)b);
    //        });
    //        if (info.pos == GameData.m_RoundOverInfo.huPos) isHuPai = true;
    //        else isHuPai = false;
    //        index = 0;
    //        SetActiveChildPaiCount(RightOver, info.localCardList.Count);
    //        SetActiveChildPaiCount(RightOnHand, 0);
    //        for (int i = 0; i < info.localCardList.Count; i++)
    //        {

    //            GameObject tmpCard = Instantiate(MJPaiParent.FindChild(info.localCardList[i].ToString()).gameObject);
    //            StartCoroutine(AsyncSetMJPai(RightOver.FindChild(i.ToString()).FindChild("Content"), tmpCard));
    //            if (isHuPai && index == 0 && info.localCardList[i] == GameData.m_RoundOverInfo.huCard)
    //            {
    //                index++;
    //                SetActiveChildPaiCount(RightHu, 1);
    //                StartCoroutine(AsyncSetMJPai(RightHu.FindChild("0").FindChild("Content"), tmpCard));
    //            }
    //        }
    //    }
    //    //if (GameData.m_TableInfo.configPlayerIndex == 0 || GameData.m_TableInfo.configPlayerIndex == 1)
    //    //{

    //    //if (GameData.m_TableInfo.configPlayerIndex == 0)
    //    //{
    //    infoObj = GameDataFunc.GetHoldCardObj(LocalViewDirection.LEFT);
    //    info = GameDataFunc.GetPlayerInfo(LocalViewDirection.LEFT);

    //    List<uint> Cardlist1 = new List<uint>();
    //    Cardlist1.AddRange(info.localCardList);
    //    if (info != null)
    //    {
    //        info.localCardList.Sort((a, b) =>
    //        {
    //            return ((int)a - (int)b);
    //        });
    //        if (info.pos == GameData.m_RoundOverInfo.huPos) isHuPai = true;
    //        else isHuPai = false;
    //        index = 0;
    //        SetActiveChildPaiCount(LeftOver, info.localCardList.Count);
    //        SetActiveChildPaiCount(LeftOnHand, 0);
    //        for (int i = 0; i < info.localCardList.Count; i++)
    //        {

    //            GameObject tmpCard = Instantiate(MJPaiParent.FindChild(info.localCardList[i].ToString()).gameObject);
    //            StartCoroutine(AsyncSetMJPai(LeftOver.FindChild(i.ToString()).FindChild("Content"), tmpCard));
    //            if (isHuPai && index == 0 && info.localCardList[i] == GameData.m_RoundOverInfo.huCard)
    //            {
    //                index++;
    //                SetActiveChildPaiCount(LeftHu, 1);
    //                StartCoroutine(AsyncSetMJPai(LeftHu.FindChild("0").FindChild("Content"), tmpCard));
    //            }
    //        }
    //    }
    //    //}
    //    //}
    //    yield break;
    //}


    public Transform sanDParent;//3d物体的父亲

    IEnumerator AsynCreateOtherCards()
    {
        bool isHuPai = false;
        int index = 0;
        HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(LocalViewDirection.UP);
        PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalViewDirection.UP);
        if (info != null)
        {
            info.localCardList.Sort((a, b) =>
            {
                return ((int)a - (int)b);
            });

            if (info.pos == GameData.m_RoundOverInfo.huPos) isHuPai = true;
            else isHuPai = false;
            index = 0;
            SetActiveChildPaiCount(TopOver, info.localCardList.Count);
            SetActiveChildPaiCount(TopOnHand, 0);
            for (int i = 0; i < info.localCardList.Count; i++)
            {

                GameObject tmpCard = Instantiate(MJPaiParent.Find(info.localCardList[i].ToString()).gameObject);

                if (isHuPai && info.localCardList[i] == GameData.m_RoundOverInfo.huCard)
                {
                    StartCoroutine(AsyncSetMJPai(TopOver.Find(i.ToString()).Find("Content"), tmpCard, true));
                }
                else
                {
                    StartCoroutine(AsyncSetMJPai(TopOver.Find(i.ToString()).Find("Content"), tmpCard));
                }

                //if (isHuPai && index == 0 && info.localCardList[i] == GameData.m_RoundOverInfo.huCard)
                //{
                //    index++;
                //    SetActiveChildPaiCount(TopHu, 1);
                //    StartCoroutine(AsyncSetMJPai(TopHu.FindChild("0").FindChild("Content"), tmpCard));
                //}
            }
        }
        //if (GameData.m_TableInfo.configPlayerIndex == 0 || GameData.m_TableInfo.configPlayerIndex == 1)
        //{
        infoObj = GameDataFunc.GetHoldCardObj(LocalViewDirection.RIGHT);
        info = GameDataFunc.GetPlayerInfo(LocalViewDirection.RIGHT);
        if (info != null)
        {
            info.localCardList.Sort((a, b) =>
            {
                return ((int)a - (int)b);
            });
            if (info.pos == GameData.m_RoundOverInfo.huPos) isHuPai = true;
            else isHuPai = false;
            index = 0;
            SetActiveChildPaiCount(RightOver, info.localCardList.Count);
            SetActiveChildPaiCount(RightOnHand, 0);
            for (int i = 0; i < info.localCardList.Count; i++)
            {

                GameObject tmpCard = Instantiate(MJPaiParent.Find(info.localCardList[i].ToString()).gameObject);
                if (isHuPai && info.localCardList[i] == GameData.m_RoundOverInfo.huCard)
                {
                    StartCoroutine(AsyncSetMJPai(RightOver.Find(i.ToString()).Find("Content"), tmpCard, true));
                }
                else
                {
                    StartCoroutine(AsyncSetMJPai(RightOver.Find(i.ToString()).Find("Content"), tmpCard));
                }

                //StartCoroutine(AsyncSetMJPai(RightOver.FindChild(i.ToString()).FindChild("Content"), tmpCard));
                //if (isHuPai && index == 0 && info.localCardList[i] == GameData.m_RoundOverInfo.huCard)
                //{
                //    index++;
                //    ObjOutSign.transform.SetParent(tmpCard.transform.parent);
                //    ObjOutSign.transform.localPosition = Vector3.zero;
                //    ObjOutSign.transform.SetParent(sanDParent);
                //}
                //if (isHuPai && index == 0 && info.localCardList[i] == GameData.m_RoundOverInfo.huCard)
                //{
                //    index++;
                //    SetActiveChildPaiCount(RightHu, 1);
                //    StartCoroutine(AsyncSetMJPai(RightHu.FindChild("0").FindChild("Content"), tmpCard));
                //}
            }
        }
        //if (GameData.m_TableInfo.configPlayerIndex == 0)
        //{
        infoObj = GameDataFunc.GetHoldCardObj(LocalViewDirection.LEFT);
        info = GameDataFunc.GetPlayerInfo(LocalViewDirection.LEFT);
        if (info != null)
        {
            info.localCardList.Sort((a, b) =>
            {
                return ((int)a - (int)b);
            });
            if (info.pos == GameData.m_RoundOverInfo.huPos) isHuPai = true;
            else isHuPai = false;
            index = 0;
            SetActiveChildPaiCount(LeftOver, info.localCardList.Count);
            SetActiveChildPaiCount(LeftOnHand, 0);
            for (int i = 0; i < info.localCardList.Count; i++)
            {

                GameObject tmpCard = Instantiate(MJPaiParent.Find(info.localCardList[i].ToString()).gameObject);

                if (isHuPai && info.localCardList[i] == GameData.m_RoundOverInfo.huCard)
                {
                    StartCoroutine(AsyncSetMJPai(LeftOver.Find(i.ToString()).Find("Content"), tmpCard, true));
                }
                else
                {
                    StartCoroutine(AsyncSetMJPai(LeftOver.Find(i.ToString()).Find("Content"), tmpCard));
                }

                //StartCoroutine(AsyncSetMJPai(LeftOver.FindChild(i.ToString()).FindChild("Content"), tmpCard));
                //if (isHuPai && index == 0 && info.localCardList[i] == GameData.m_RoundOverInfo.huCard)
                //{
                //    index++;
                //    ObjOutSign.transform.SetParent(tmpCard.transform.parent);
                //    ObjOutSign.transform.localPosition = Vector3.zero;
                //    ObjOutSign.transform.SetParent(sanDParent);
                //}
                //if (isHuPai && index == 0 && info.localCardList[i] == GameData.m_RoundOverInfo.huCard)
                //{
                //    index++;
                //    SetActiveChildPaiCount(LeftHu, 1);
                //    StartCoroutine(AsyncSetMJPai(LeftHu.FindChild("0").FindChild("Content"), tmpCard));
                //}
            }
        }
        //}
        //}
        yield break;
    }
    #endregion
    /// <summary>
    /// 设置吃碰杠每一组的牌
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncSetCPGSinglePairt(Transform TargetParent, GameObject NewCPGObj, string NewName, List<uint> CardList, bool isAnGang)
    {
        Transform realParent = TargetParent;
        NewCPGObj.transform.parent = realParent;
        NewCPGObj.transform.localScale = Vector3.one;
        NewCPGObj.transform.localPosition = Vector3.zero;
        NewCPGObj.transform.localEulerAngles = Vector3.zero;
        NewCPGObj.name = NewName;
        NewCPGObj.SetActive(true);
        NewCPGObj.transform.parent = realParent.parent;
        Destroy(TargetParent.gameObject);

        SetActiveChildPaiCount(NewCPGObj.transform, CardList.Count);
        for (int i = 0; i < CardList.Count; i++)
        {
            itemObj = Instantiate<GameObject>(MJPaiParent.Find(CardList[i].ToString()).gameObject);
            itemObj.name = CardList[i].ToString();
            StartCoroutine(AsyncSetMJPai(NewCPGObj.transform.Find(i.ToString()).Find("Content"), itemObj));
            if (isAnGang && i < 3)
            {
                NewCPGObj.transform.Find(i.ToString()).Find("Content").localEulerAngles = new Vector3(0, 0, 180);
            }
            else
            {
                NewCPGObj.transform.Find(i.ToString()).Find("Content").localEulerAngles = Vector3.zero;
            }
        }
        yield return new WaitForEndOfFrame();
    }

    //设置牌面
    //IEnumerator AsyncSetMJPai(Transform MJPaiParent, GameObject NewMJPaiObj)
    //{
    //    for (int i = 0; i < MJPaiParent.childCount; i++)
    //    {
    //        Destroy(MJPaiParent.GetChild(i).gameObject);
    //    }
    //    yield return new WaitForEndOfFrame();
    //    NewMJPaiObj.transform.parent = MJPaiParent;
    //    NewMJPaiObj.transform.localScale = Vector3.one;
    //    NewMJPaiObj.transform.localPosition = Vector3.zero;
    //    NewMJPaiObj.transform.localEulerAngles = Vector3.zero;
    //    NewMJPaiObj.SetActive(true);
    //}

    IEnumerator AsyncSetMJPai(Transform MJPaiParent, GameObject NewMJPaiObj, bool IsHuThisCard = false)
    {
        for (int i = 0; i < MJPaiParent.childCount; i++)
        {
            Destroy(MJPaiParent.GetChild(i).gameObject);
        }
        yield return new WaitForEndOfFrame();
        NewMJPaiObj.transform.parent = MJPaiParent;
        NewMJPaiObj.transform.localScale = Vector3.one;
        NewMJPaiObj.transform.localPosition = Vector3.zero;
        NewMJPaiObj.transform.localEulerAngles = Vector3.zero;
        NewMJPaiObj.SetActive(true);

        if (IsHuThisCard)
        {
            ObjOutSign.transform.SetParent(NewMJPaiObj.transform);
            ObjOutSign.transform.localPosition = new Vector3(0, 0.5f, 0);
            ObjOutSign.transform.SetParent(sanDParent);
        }
    }
    /// <summary>
    /// 设置出牌标记箭头
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncSetOutSignMark()
    {
        yield return new WaitForEndOfFrame();
        HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(GameData.m_TableInfo.lastOutCardPos);
        ObjOutSign.gameObject.SetActive(true);
        ObjOutSign.transform.position = infoObj.outObjList[infoObj.outObjList.Count - 1].transform.position + new Vector3(0, outMarkHeight, 0);
    }

}
