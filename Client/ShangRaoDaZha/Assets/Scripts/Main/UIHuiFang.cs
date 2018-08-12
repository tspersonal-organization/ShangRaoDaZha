using FrameworkForCSharp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHuiFang : UIBase<UIHuiFang>
{

    public UISprite LogoSprite;//桌面logo水影
    public UILabel LBRoomID;
    public UILabel GameName;
    public UILabel LBGameCount;
    public UILabel LBRoomDesc;
    public UILabel LBResCardCount;
    public UILabel LBBeiShu;

    public Transform LocalParent;
    public Transform RightParent;
    public Transform UpParent;
    public Transform LeftParent;
    public Transform OutParent;

    public GameObject ObjMakerImg;
    public GameObject ObjCurOutImg;
    public GameObject ObjLastCardImg;
    public GameObject ObjFangZhuImg;

    GameObject pbCardImage;
    GameObject pbLocalItem;
    GameObject pbUpItem;
    GameObject pbEffectItem;
    GameObject ObjEffect;

    Vector3[] PlayerStartPos4 = new Vector3[] { new Vector3(-569, -156), new Vector3(562, 171), new Vector3(-389, 265), new Vector3(-569, 22) };
    Vector3[] PlayerStartPos2 = new Vector3[] { new Vector3(-569, -156), new Vector3(-389, 265) };
    string[] playerString = new string[] { "4人", "3人", "2人" };
    string[] daZiString = new string[] { "不打炮子", "最多二颗", "最多三颗", "最多四颗", };
    int[] gameRoundCount = new int[] { 4, 8, 16 };
    int leftAndRightHideOffset = 28;
    int outCardOffsetX = 44;
    int outCardOffsetY = 54;
    Color32 huPaiCardColor = new Color32(253, 191, 56, 255);

    string roomDesc;
    byte LocalPos;
    List<GameObject> PlayerObjList = new List<GameObject>();//玩家头像
    List<string> OperateList = new List<string>();
    bool isPlay = false;
    uint lastCard;
    uint lastPos;

    float offsetTime = 2f;
    float countDownTime = 2f;
    float speed = 1;
    int operateIndex = 0;



    void OnEnable()
    {
        OperateList = new List<string>();
        foreach (Transform item in transform.Find("PlayerBase"))
            PlayerObjList.Add(item.gameObject);
        foreach (Transform item in transform.Find("BtnBase"))
            UIEventListener.Get(item.gameObject).onClick = OnClickBtn;
        GameData.m_PlayerInfoList.Clear();
        GameDataFunc.ClearData();
        GameDataFunc.ClearDataObj();
        for (int i = 0; i < GameData.m_HuiFangList[(int)(GameData.m_TableInfo.curGameCount)].playerList.Count; i++)
        {
            PlayerInfo info = GameData.m_HuiFangList[(int)(GameData.m_TableInfo.curGameCount)].playerList[i];
            PlayerInfo newInfo = new PlayerInfo();
            newInfo.pos = info.pos;
            newInfo.guid = info.guid;
            newInfo.headID = info.headID;
            newInfo.name = info.name;
            newInfo.sex = info.sex;
            newInfo.score = info.score;
            newInfo.fangPaoScore = info.fangPaoScore;
            for (int k = 0; k < info.localCardList.Count; k++)
            {
                newInfo.localCardList.Add(info.localCardList[k]);
            }
            GameData.m_PlayerInfoList.Add(newInfo);
        }

        OperateList.AddRange(GameData.m_HuiFangList[(int)(GameData.m_TableInfo.curGameCount)].operateList.ToArray());
        InitRoomData();
        InitPlayerData();
        StartCoroutine(AsynCreateCards());
    }

    #region  初始化

    public GameObject MianCard;//面牌
    /// <summary>
    /// 初始化房间信息
    /// </summary>
    void InitRoomData()
    {
        switch (GameData.GlobleRoomType)
        {
            case RoomType.WDH:
                GameName.text = "无挡胡";
                MianCard.SetActive(false);
                LogoSprite.spriteName = "UI_game_icon_DeskLogo_1";
                LogoSprite.MakePixelPerfect();
                break;
            case RoomType.ZB:
                GameName.text = "载宝";
                MianCard.SetActive(true);
                MianCard.transform.GetComponent<UISprite>().spriteName = "card_local_min_" + GameData.MagicCard.ToString();
                MianCard.transform.GetComponent<UISprite>().color = new Color(1,0.7f,0.7f,1);
                LogoSprite.spriteName = "UI_game_icon_DeskLogo_2";
                LogoSprite.MakePixelPerfect();
                break;
        }
     
        LBRoomID.text = "房间号:" + GameData.m_TableInfo.id;
        // LBGameCount.text = "局数:" + (GameData.m_TableInfo.curGameCount + 1) + "/" + gameRoundCount[GameData.m_TableInfo.configRoundIndex];

        LBGameCount.text = "局数:" + ((int)GameData.m_TableInfo.curGameCount + 1) + "/" + GameData.m_TableInfo.configRoundIndex;// gameRoundCount[GameData.m_TableInfo.configRoundIndex];
        //roomDesc = GameData.m_TableInfo.configFangChongIndex == 0 ? "缺一不可放冲\\n" : "缺一可放冲\\n";
        //roomDesc += (GameData.m_TableInfo.configShengPaiIndex == 0 ? "不剩牌\\n" : "剩7顿\\n");
        //roomDesc += daZiString[GameData.m_TableInfo.configDaZiIndex] + "\\n";
        //roomDesc += playerString[GameData.m_TableInfo.configPlayerIndex] + "\\n";
        roomDesc += (GameData.m_TableInfo.configPayIndex == 0 ? "房主支付" : "房费均分");
        // LBRoomDesc.text = roomDesc.Replace("\\n", "\n");
        LBRoomDesc.text = roomDesc;
        ShowFangZhuImg();
        ShowMakerImg();
    }

    /// <summary>
    /// 初始化玩家信息
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

        #region  代理开的房  玩家是不在里面的
        if (LocalPos == 0)
        {
            LocalPos = GameData.m_PlayerInfoList[0].pos;
        }

        #endregion

        LocalViewSitDown();
        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            PlayerInfo info = GameData.m_PlayerInfoList[i];
            GameObject obj = PlayerObjList[info.pos - 1];
            obj.SetActive(true);
            obj.transform.Find("name").GetComponent<UILabel>().text = info.name;
            obj.transform.Find("score").GetComponent<UILabel>().text = info.score.ToString();
            DownloadImage.Instance.Download(obj.transform.Find("headImage").GetComponent<UITexture>(), info.headID);
            if (info.fangPaoScore > 0)
            {
                UISprite sp = obj.transform.Find("fpScore").GetComponent<UISprite>();
                sp.gameObject.SetActive(true);
                sp.spriteName = "UI_game_icon_bet_" + info.fangPaoScore;
            }
            info.LVD = GetLVD(info.pos);

            Debug.Log("");
        }

        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            HoldCardsObj obj = new HoldCardsObj();
            obj.pos = GameData.m_PlayerInfoList[i].pos;
            obj.LVD = GameData.m_PlayerInfoList[i].LVD;
            GameData.m_HoldCardsList.Add(obj);
        }
    }


    /// <summary>
    /// 发牌
    /// </summary>
    /// <returns></returns>
    IEnumerator AsynCreateCards()
    {
        PlayerInfo info = GameDataFunc.GetPlayerInfo(LocalViewDirection.LOCAL);
        GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList = new List<GameObject>();
        if (pbCardImage == null) pbCardImage = Resources.Load<GameObject>("Item/CardImage");
        int startPos = -451 + (13 - info.localCardList.Count) * 75;
        info.localCardList.Sort((a, b) =>
        {
            return (int)a - (int)b;
        });
        for (int j = 0; j < info.localCardList.Count; j++)//本地牌
        {
            GameObject card = Instantiate<GameObject>(pbCardImage);
            card.transform.parent = LocalParent;
            card.transform.localScale = Vector3.one;
            card.transform.localPosition = new Vector3(startPos + j * 75, 0, 0);
            card.name = info.localCardList[j].ToString();
            card.AddComponent<BoxCollider2D>().size = new Vector3(75, 122, 0);
            card.GetComponent<UISprite>().spriteName = "card_local_max_" + info.localCardList[j].ToString();
            GameDataFunc.GetHoldCardObj(LocalViewDirection.LOCAL).holdObjList.Add(card);
        }
      
            info = GameDataFunc.GetPlayerInfo(LocalViewDirection.RIGHT);
            GameDataFunc.GetHoldCardObj(LocalViewDirection.RIGHT).holdObjList = new List<GameObject>();
            info.localCardList.Sort((a, b) =>
            {
                return (int)a - (int)b;
            });
            for (int j = 0; j < info.localCardList.Count; j++)//下家
            {
                GameObject card = Instantiate<GameObject>(pbCardImage);
                card.transform.parent = RightParent;
                card.transform.localScale = Vector3.one;
                card.transform.localPosition = new Vector3(0, 239 - j * leftAndRightHideOffset, 0);
                card.GetComponent<UISprite>().spriteName = "card_right_min_" + info.localCardList[j].ToString();
                card.GetComponent<UISprite>().MakePixelPerfect();
                card.GetComponent<UISprite>().depth = j + 3;
                card.name = info.localCardList[j].ToString();
                GameDataFunc.GetHoldCardObj(LocalViewDirection.RIGHT).holdObjList.Add(card);
            }
  



        info = GameDataFunc.GetPlayerInfo(LocalViewDirection.UP);
        GameDataFunc.GetHoldCardObj(LocalViewDirection.UP).holdObjList = new List<GameObject>();
        info.localCardList.Sort((a, b) =>
        {
            return (int)a - (int)b;
        });
        for (int j = 0; j < info.localCardList.Count; j++)//对家
        {
            GameObject card = Instantiate<GameObject>(pbCardImage);
            card.transform.parent = UpParent;
            card.transform.localScale = Vector3.one;
            card.transform.localPosition = new Vector3(-253 + j * 43, 0, 0);
            card.GetComponent<UISprite>().spriteName = "card_local_mid_" + info.localCardList[j].ToString();
            card.GetComponent<UISprite>().MakePixelPerfect();
            card.name = info.localCardList[j].ToString();
            GameDataFunc.GetHoldCardObj(LocalViewDirection.UP).holdObjList.Add(card);
        }
       


            info = GameDataFunc.GetPlayerInfo(LocalViewDirection.LEFT);
            GameDataFunc.GetHoldCardObj(LocalViewDirection.LEFT).holdObjList = new List<GameObject>();
            info.localCardList.Sort((a, b) =>
            {
                return (int)a - (int)b;
            });
            for (int j = 0; j < info.localCardList.Count; j++)//上家
            {
                GameObject card = Instantiate<GameObject>(pbCardImage);
                card.transform.parent = LeftParent;
                card.transform.localScale = Vector3.one;
                card.transform.localPosition = new Vector3(0, -144 + j * leftAndRightHideOffset, 0);
                card.GetComponent<UISprite>().spriteName = "card_left_min_" + info.localCardList[j].ToString();
                card.GetComponent<UISprite>().MakePixelPerfect();
                card.GetComponent<UISprite>().depth = 20 - j;
                card.name = info.localCardList[j].ToString();
                GameDataFunc.GetHoldCardObj(LocalViewDirection.LEFT).holdObjList.Add(card);
            }
        
        yield break;
    }


    #endregion

    void Update()
    {
        if (isPlay && operateIndex < OperateList.Count)
        {
            if (countDownTime <= 0)
            {
                countDownTime = offsetTime;
                PlayOperate();
            }
            else
            {
                countDownTime -= Time.deltaTime * speed;
            }
        }
    }

  
    void OnClickBtn(GameObject go)
    {
        switch (go.name)
        {
            case "btnQuit":
                UIManager.Instance.HideUIPanel(UIPaths.UIPanel_HuiFang);
                break;
            case "btnPlayOrPause":
                isPlay = !isPlay;
                transform.Find("BtnBase").Find("btnPlayOrPause").GetComponent<UISprite>().spriteName = isPlay ? "UI_record_btn_pause" : "UI_record_btn_play";
                break;
            case "btnNext":
                if (speed < 4) speed *= 2;
                LBBeiShu.text = "x" + speed;
                break;
            case "btnBack":
                if (speed > 0.25f) speed /= 2;
                LBBeiShu.text = "x" + speed;
                break;
        }
    }

    /// <summary>
    /// 得到对应位置
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    LocalViewDirection GetLVD(byte pos)
    {
        if (GameData.m_TableInfo.configPlayerIndex == 2)
        {
            if (pos == LocalPos) return LocalViewDirection.LOCAL;
            else return LocalViewDirection.UP;
        }
        else if (GameData.m_TableInfo.configPlayerIndex == 1)
        {
            int rightPos = LocalPos + 1 > 3 ? LocalPos + 1 - 3 : LocalPos + 1;
            int upPos = LocalPos + 2 > 3 ? LocalPos + 2 - 3 : LocalPos + 2;
            if (pos == rightPos)
                return LocalViewDirection.RIGHT;
            else if (pos == upPos)
                return LocalViewDirection.UP;
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
        return LocalViewDirection.LOCAL;
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    void LocalViewSitDown()
    {
        if (GameData.m_TableInfo.configPlayerIndex == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                int index = LocalPos + i;
                if (index > 2) index -= 2;
                PlayerObjList[index - 1].transform.localPosition = PlayerStartPos2[i];
            }
        }
        else if (GameData.m_TableInfo.configPlayerIndex == 1)
        {
            for (int i = 0; i < PlayerObjList.Count; i++)
            {
                int index = LocalPos + i;
                if (index > 3) index -= 3;
                PlayerObjList[index - 1].transform.localPosition = PlayerStartPos4[i];
            }
        }
        else
        {
            for (int i = 0; i < PlayerObjList.Count; i++)
            {
                int index = LocalPos + i;
                if (index > 4) index -= 4;
                PlayerObjList[index - 1].transform.localPosition = PlayerStartPos4[i];
            }
        }
    }

    /// <summary>
    /// 显示房主图标
    /// </summary>
    void ShowFangZhuImg()
    {
        try
        {
            PlayerInfo info = GameDataFunc.GetPlayerInfo(GameData.m_TableInfo.fangZhuGuid);
            ObjFangZhuImg.transform.parent = PlayerObjList[info.pos - 1].transform;
            ObjFangZhuImg.transform.localPosition = new Vector3(-14, 31);
        }
        catch
        {
            ObjFangZhuImg.gameObject.SetActive(false);
        }
      
    }

   /// <summary>
   /// 显示庄家位置
   /// </summary>
    void ShowMakerImg()
    {
        ObjMakerImg.SetActive(true);
        ObjMakerImg.transform.SetParent(PlayerObjList[GameData.m_TableInfo.makerPos - 1].transform);
        ObjMakerImg.transform.localPosition = new Vector3(60,-40,0);
      //  ObjMakerImg.transform.localPosition = PlayerObjList[GameData.m_TableInfo.makerPos - 1].transform.localPosition + new Vector3(-35, -10);
    }

    /// <summary>
    /// 回放操作
    /// </summary>
    void PlayOperate()
    {
        string[] opStrs = OperateList[operateIndex].Split(':');
        RoomMessageType type = (RoomMessageType)Enum.Parse(typeof(RoomMessageType), opStrs[0]);
        if (type == RoomMessageType.PlayerInCard)
        {
            byte pos = byte.Parse(opStrs[1]);
            uint card = uint.Parse(opStrs[2]);
            uint resCard = uint.Parse(opStrs[3]);
            CreateInCard(pos, card);
            LBResCardCount.text = resCard.ToString();
        }
        else if (type == RoomMessageType.playerOperate)
        {
            byte pos = byte.Parse(opStrs[1]);
            CardOperateType opType = (CardOperateType)Enum.Parse(typeof(CardOperateType), opStrs[2]);
            uint card = uint.Parse(opStrs[3]);
            byte outPos = byte.Parse(opStrs[4]);
            PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);
            switch (opType)
            {
                case CardOperateType.ChuPai:
                    lastPos = pos;
                    lastCard = card;
                    StartCoroutine(AsynCreatePlayerOutCard(pos, card));//生成出的牌
                    break;
                case CardOperateType.PengPai:
                    ShowOutCardPlayer(pos);
                    SoundManager.Instance.PlaySound(UIPaths.SOUND_PENG, info.sex);
                    StartCoroutine(AsynPengGang(info.guid, opType, card, outPos));
                    StartCoroutine(AsynCreateEffect(info.LVD, opType));
                    break;
                case CardOperateType.GangPai:
                    ShowOutCardPlayer(info.pos);
                    SoundManager.Instance.PlaySound(UIPaths.SOUND_GANG, info.sex);
                    StartCoroutine(AsynPengGang(info.guid, opType, card, outPos));
                    StartCoroutine(AsynCreateEffect(info.LVD, opType));
                    break;
                case CardOperateType.HuPai:
                    ShowOutCardPlayer(info.pos);
                    SoundManager.Instance.PlaySound(UIPaths.SOUND_HU, info.sex);
                    StartCoroutine(AsynCreateEffect(info.LVD, opType));
                    ObjLastCardImg.SetActive(false);
                    //GameDataFunc.RemoveOutCardInfo(outPos);
                    //GameDataFunc.RemoveOutCardObj(outPos);
                    //CreateInCard(info.pos, lastCard, true);
                    break;
            }
        }
        operateIndex++;
    }

    /// <summary>
    /// 玩家摸牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="card"></param>
    /// <param name="isHuCard"></param>
    void CreateInCard(byte pos, uint card, bool isHuCard = false)
    {
        ShowOutCardPlayer(pos);
        PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);
        HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(info.LVD);
        GameObject cardObj = null;
        cardObj = Instantiate(pbCardImage);
        info.localCardList.Add(card);
        infoObj.holdObjList.Add(cardObj);
        cardObj.name = card.ToString();
        if (isHuCard)
            cardObj.GetComponent<UISprite>().color = huPaiCardColor;
        switch (info.LVD)
        {
            case LocalViewDirection.LOCAL:
                cardObj.transform.parent = LocalParent;
                cardObj.transform.localScale = Vector3.one;
                cardObj.transform.localPosition = new Vector3(571, 0, 0);
                cardObj.GetComponent<UISprite>().spriteName = "card_local_max_" + card.ToString();
                cardObj.GetComponent<UISprite>().MakePixelPerfect();

                break;
            case LocalViewDirection.RIGHT:
                cardObj.transform.parent = RightParent;
                cardObj.transform.localScale = Vector3.one;
                cardObj.transform.localPosition = new Vector3(0, 295, 0);
                cardObj.GetComponent<UISprite>().spriteName = "card_right_min_" + card.ToString();
                cardObj.GetComponent<UISprite>().MakePixelPerfect();
                cardObj.GetComponent<UISprite>().depth = 1;
                break;
            case LocalViewDirection.UP:
                cardObj.transform.parent = UpParent;
                cardObj.transform.localScale = Vector3.one;
                cardObj.transform.localPosition = new Vector3(-310, 0, 0);
                cardObj.GetComponent<UISprite>().spriteName = "card_local_mid_" + card.ToString();
                cardObj.GetComponent<UISprite>().MakePixelPerfect();
                break;
            case LocalViewDirection.LEFT:
                cardObj.transform.parent = LeftParent;
                cardObj.transform.localScale = Vector3.one;
                cardObj.transform.localPosition = new Vector3(0, -201, 0);
                cardObj.GetComponent<UISprite>().spriteName = "card_left_min_" + card.ToString();
                cardObj.GetComponent<UISprite>().MakePixelPerfect();
                cardObj.GetComponent<UISprite>().depth = 20;
                break;
        }
    }
    void ShowOutCardPlayer(byte pos)
    {
        ObjCurOutImg.SetActive(true);
        ObjCurOutImg.transform.localPosition = PlayerObjList[pos - 1].transform.localPosition;
    }
    void ResetLocalHoldCards(byte pos)
    {
        PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);
        HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(info.LVD);
        info.localCardList.Sort((a, b) =>
        {
            return (int)a - (int)b;
        });
        int startPos = 0;
        switch (info.LVD)
        {
            case LocalViewDirection.LOCAL:
                startPos = -451 + (13 - info.localCardList.Count) * 75;
                for (int i = 0; i < info.localCardList.Count; i++)
                {
                    GameObject obj = infoObj.holdObjList[i];
                    obj.name = info.localCardList[i].ToString();
                    obj.transform.localPosition = new Vector3(startPos + i * 75, 0, 0);
                    obj.GetComponent<UISprite>().spriteName = "card_local_max_" + info.localCardList[i].ToString();
                }
                break;
            case LocalViewDirection.RIGHT:
                startPos = 239;
                for (int i = 0; i < info.localCardList.Count; i++)
                {
                    GameObject obj = infoObj.holdObjList[i];
                    obj.name = info.localCardList[i].ToString();
                    obj.transform.localPosition = new Vector3(0, startPos - i * leftAndRightHideOffset, 0);
                    obj.GetComponent<UISprite>().spriteName = "card_right_min_" + info.localCardList[i].ToString();
                    obj.GetComponent<UISprite>().depth = i + 5;
                }
                break;
            case LocalViewDirection.UP:
                startPos = -253;
                for (int i = 0; i < info.localCardList.Count; i++)
                {
                    GameObject obj = infoObj.holdObjList[i];
                    obj.name = info.localCardList[i].ToString();
                    obj.transform.localPosition = new Vector3(startPos + i * 43, 0, 0);
                    obj.GetComponent<UISprite>().spriteName = "card_local_mid_" + info.localCardList[i].ToString();
                }
                break;
            case LocalViewDirection.LEFT:
                startPos = -144;
                for (int i = 0; i < info.localCardList.Count; i++)
                {
                    GameObject obj = infoObj.holdObjList[i];
                    obj.name = info.localCardList[i].ToString();
                    obj.transform.localPosition = new Vector3(0, startPos + i * leftAndRightHideOffset, 0);
                    obj.GetComponent<UISprite>().spriteName = "card_left_min_" + info.localCardList[i].ToString();
                    obj.GetComponent<UISprite>().depth = 20 - i;
                }
                break;
        }

    }
    string GetDirectFileName(LocalViewDirection dir, byte outPos)
    {
        string str = "";
        LocalViewDirection outDir = GameDataFunc.GetPlayerInfo(outPos).LVD;
        switch (dir)
        {
            case LocalViewDirection.LOCAL:
                switch (outDir)
                {
                    case LocalViewDirection.LOCAL:
                        str = "";
                        break;
                    case LocalViewDirection.RIGHT:
                        str = "UI_game_right";
                        break;
                    case LocalViewDirection.UP:
                        str = "UI_game_up";
                        break;
                    case LocalViewDirection.LEFT:
                        str = "UI_game_left";
                        break;
                }
                break;
            case LocalViewDirection.RIGHT:
                switch (outDir)
                {
                    case LocalViewDirection.LOCAL:
                        str = "UI_game_down";
                        break;
                    case LocalViewDirection.RIGHT:
                        str = "";
                        break;
                    case LocalViewDirection.UP:
                        str = "UI_game_up";
                        break;
                    case LocalViewDirection.LEFT:
                        str = "UI_game_left";
                        break;
                }
                break;
            case LocalViewDirection.UP:
                switch (outDir)
                {
                    case LocalViewDirection.LOCAL:
                        str = "UI_game_down";
                        break;
                    case LocalViewDirection.RIGHT:
                        str = "UI_game_right";
                        break;
                    case LocalViewDirection.UP:
                        str = "";
                        break;
                    case LocalViewDirection.LEFT:
                        str = "UI_game_left";
                        break;
                }
                break;
            case LocalViewDirection.LEFT:
                switch (outDir)
                {
                    case LocalViewDirection.LOCAL:
                        str = "UI_game_down";
                        break;
                    case LocalViewDirection.RIGHT:
                        str = "UI_game_right";
                        break;
                    case LocalViewDirection.UP:
                        str = "UI_game_up";
                        break;
                    case LocalViewDirection.LEFT:
                        str = "";
                        break;
                }
                break;
        }
        return str;
    }

    #region
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
    /// 玩家出牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="card"></param>
    /// <returns></returns>
    IEnumerator AsynCreatePlayerOutCard(byte pos, uint card)
    {
        if (GameData.GlobleRoomType == RoomType.ZB)
        {
            if (card == GameData.MagicCard || card == GameData.MianCard)//吃面或栽宝
            {
                CreatMianCardAndMagicCard(pos, card);//生成面牌
                PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);

                SoundManager.Instance.PlaySound(UIPaths.SOUND_OUT_CARD + card, info.sex);
                GameDataFunc.RemoverHoldCard(card, info.pos);
                GameDataFunc.RemoveHoldCardObj(card, info.LVD);
                ResetLocalHoldCards(info.pos);
                yield return null;
            }
            else
            {
                #region 
                PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);
                HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(info.LVD);
                if (pbCardImage == null) pbCardImage = Resources.Load<GameObject>("Item/CardImage");
                GameObject item = null;
                item = Instantiate<GameObject>(pbCardImage);
                item.transform.parent = transform.Find("OutCardParent");
                item.transform.localScale = Vector3.one * 1.2f;
                Vector3 endPos = Vector3.zero;
                int number = 0;
                if (GameData.m_TableInfo.configPlayerIndex == 2)
                {
                    number = 16;
                }
                else
                {
                    if (info.LVD == LocalViewDirection.LOCAL || info.LVD == LocalViewDirection.UP) number = 8;
                    else number = 6;//6排或8排
                }
                int lie = infoObj.outObjList.Count % number;
                int hang = infoObj.outObjList.Count / number;
                item.name = card.ToString();
                info.outCardList.Add(card);
                infoObj.outObjList.Add(item);
                SoundManager.Instance.PlaySound(UIPaths.SOUND_OUT_CARD + card, info.sex);
                GameDataFunc.RemoverHoldCard(card, info.pos);
                GameDataFunc.RemoveHoldCardObj(card, info.LVD);
                ResetLocalHoldCards(info.pos);

                //放打出的牌
                switch (info.LVD)
                {
                    case LocalViewDirection.LOCAL:
                        item.transform.localPosition = new Vector3(0, -130, 0);
                        item.GetComponent<UISprite>().spriteName = "card_local_mid_" + card.ToString();
                        if (GameData.m_TableInfo.configPlayerIndex == 2)
                            endPos = new Vector3(-320, -68, 0) + new Vector3(lie * outCardOffsetX, hang * -outCardOffsetY, 0);
                        else
                            endPos = new Vector3(-138, -68, 0) + new Vector3(lie * outCardOffsetX, hang * -outCardOffsetY, 0);
                        item.GetComponent<UISprite>().depth = 5 + hang;
                        yield return new WaitForSeconds(0.3f);
                        TweenPosition.Begin(item, 0.1f, endPos);
                        break;
                    case LocalViewDirection.RIGHT:
                        item.transform.localPosition = new Vector3(380, 0, 0);
                        item.GetComponent<UISprite>().spriteName = "card_local_mid_" + card.ToString();
                        endPos = new Vector3(256, -128, 0) + new Vector3(hang * outCardOffsetX, lie * outCardOffsetY, 0);
                        item.GetComponent<UISprite>().depth = 22 - lie;
                        yield return new WaitForSeconds(0.3f);
                        TweenPosition.Begin(item, 0.1f, endPos);
                        break;
                    case LocalViewDirection.UP:
                        item.transform.localPosition = new Vector3(0, 220, 0);
                        item.GetComponent<UISprite>().spriteName = "card_local_mid_" + card.ToString();
                        if (GameData.m_TableInfo.configPlayerIndex == 2)
                            endPos = new Vector3(360, 104, 0) + new Vector3(lie * -outCardOffsetX, hang * outCardOffsetY, 0);
                        else
                            endPos = new Vector3(173, 104, 0) + new Vector3(lie * -outCardOffsetX, hang * outCardOffsetY, 0);
                        item.GetComponent<UISprite>().depth = 10 - hang;
                        yield return new WaitForSeconds(0.3f);
                        TweenPosition.Begin(item, 0.1f, endPos);
                        break;
                    case LocalViewDirection.LEFT:
                        item.transform.localPosition = new Vector3(-380, 0, 0);
                        item.GetComponent<UISprite>().spriteName = "card_local_mid_" + card.ToString();
                        endPos = new Vector3(-220, 155, 0) + new Vector3(hang * -outCardOffsetX, lie * -outCardOffsetY, 0);
                        item.GetComponent<UISprite>().depth = 5 + lie;
                        yield return new WaitForSeconds(0.3f);
                        TweenPosition.Begin(item, 0.1f, endPos);
                        break;
                }
                SoundManager.Instance.PlaySound(UIPaths.SOUND_CHUPAI);
                item.GetComponent<UISprite>().MakePixelPerfect();
                TweenScale.Begin(item, 0.2f, Vector3.one);
                ObjLastCardImg.SetActive(true);
                TweenPosition.Begin(ObjLastCardImg, 0.2f, endPos + new Vector3(0, 45, 0));
                yield break;

                #endregion
            }
        }

        else
        {
            #region 
            PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);
            HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(info.LVD);
            if (pbCardImage == null) pbCardImage = Resources.Load<GameObject>("Item/CardImage");
            GameObject item = null;
            item = Instantiate<GameObject>(pbCardImage);
            item.transform.parent = transform.Find("OutCardParent");
            item.transform.localScale = Vector3.one * 1.2f;
            Vector3 endPos = Vector3.zero;
            int number = 0;
            if (GameData.m_TableInfo.configPlayerIndex == 2)
            {
                number = 16;
            }
            else
            {
                if (info.LVD == LocalViewDirection.LOCAL || info.LVD == LocalViewDirection.UP) number = 8;
                else number = 6;//6排或8排
            }
            int lie = infoObj.outObjList.Count % number;
            int hang = infoObj.outObjList.Count / number;
            item.name = card.ToString();
            info.outCardList.Add(card);
            infoObj.outObjList.Add(item);
            SoundManager.Instance.PlaySound(UIPaths.SOUND_OUT_CARD + card, info.sex);
            GameDataFunc.RemoverHoldCard(card, info.pos);
            GameDataFunc.RemoveHoldCardObj(card, info.LVD);
            ResetLocalHoldCards(info.pos);
            switch (info.LVD)
            {
                case LocalViewDirection.LOCAL:
                    item.transform.localPosition = new Vector3(0, -130, 0);
                    item.GetComponent<UISprite>().spriteName = "card_local_mid_" + card.ToString();
                    if (GameData.m_TableInfo.configPlayerIndex == 2)
                        endPos = new Vector3(-320, -68, 0) + new Vector3(lie * outCardOffsetX, hang * -outCardOffsetY, 0);
                    else
                        endPos = new Vector3(-138, -68, 0) + new Vector3(lie * outCardOffsetX, hang * -outCardOffsetY, 0);
                    item.GetComponent<UISprite>().depth = 5 + hang;
                    yield return new WaitForSeconds(0.3f);
                    TweenPosition.Begin(item, 0.1f, endPos);
                    break;
                case LocalViewDirection.RIGHT:
                    item.transform.localPosition = new Vector3(380, 0, 0);
                    item.GetComponent<UISprite>().spriteName = "card_local_mid_" + card.ToString();
                    endPos = new Vector3(256, -128, 0) + new Vector3(hang * outCardOffsetX, lie * outCardOffsetY, 0);
                    item.GetComponent<UISprite>().depth = 22 - lie;
                    yield return new WaitForSeconds(0.3f);
                    TweenPosition.Begin(item, 0.1f, endPos);
                    break;
                case LocalViewDirection.UP:
                    item.transform.localPosition = new Vector3(0, 220, 0);
                    item.GetComponent<UISprite>().spriteName = "card_local_mid_" + card.ToString();
                    if (GameData.m_TableInfo.configPlayerIndex == 2)
                        endPos = new Vector3(360, 104, 0) + new Vector3(lie * -outCardOffsetX, hang * outCardOffsetY, 0);
                    else
                        endPos = new Vector3(173, 104, 0) + new Vector3(lie * -outCardOffsetX, hang * outCardOffsetY, 0);
                    item.GetComponent<UISprite>().depth = 10 - hang;
                    yield return new WaitForSeconds(0.3f);
                    TweenPosition.Begin(item, 0.1f, endPos);
                    break;
                case LocalViewDirection.LEFT:
                    item.transform.localPosition = new Vector3(-380, 0, 0);
                    item.GetComponent<UISprite>().spriteName = "card_local_mid_" + card.ToString();
                    endPos = new Vector3(-220, 155, 0) + new Vector3(hang * -outCardOffsetX, lie * -outCardOffsetY, 0);
                    item.GetComponent<UISprite>().depth = 5 + lie;
                    yield return new WaitForSeconds(0.3f);
                    TweenPosition.Begin(item, 0.1f, endPos);
                    break;
            }
            SoundManager.Instance.PlaySound(UIPaths.SOUND_CHUPAI);
            item.GetComponent<UISprite>().MakePixelPerfect();
            TweenScale.Begin(item, 0.2f, Vector3.one);
            ObjLastCardImg.SetActive(true);
            TweenPosition.Begin(ObjLastCardImg, 0.2f, endPos + new Vector3(0, 45, 0));
            yield break;

            #endregion
        }



    }

    public GameObject SelfMianCardParent;
    public GameObject LeftMianCardParent;
    public GameObject UpMianCardParent;
    public GameObject RightMianCardParent;
    /// <summary>
    /// 吃面和载宝
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="card"></param>
    public void CreatMianCardAndMagicCard(byte pos, uint card)
    {
        PlayerInfo info = GameDataFunc.GetPlayerInfo(pos);
        info.MianCardList.Add(card);

       

        switch (info.LVD)
        {
            case LocalViewDirection.LOCAL:
                for (int i = 0; i < info.MianCardList.Count; i++)
                {
                    SelfMianCardParent.transform.Find((i + 1).ToString()).gameObject.SetActive(true);
                    SelfMianCardParent.transform.Find((i + 1).ToString()).GetComponent<UISprite>().spriteName = "card_local_min_" + info.MianCardList[i].ToString();
                }
                if (card == GameData.MianCard)
                {
                    StartCoroutine(AsynCreateEffect(LocalViewDirection.LOCAL, CardOperateType.None, 1));
                }
                else
                {
                    StartCoroutine(AsynCreateEffect(LocalViewDirection.LOCAL, CardOperateType.None, 2));
                }

                break;
            case LocalViewDirection.RIGHT:
                for (int i = 0; i < info.MianCardList.Count; i++)
                {
                    RightMianCardParent.transform.Find((i + 1).ToString()).gameObject.SetActive(true);
                    RightMianCardParent.transform.Find((i + 1).ToString()).GetComponent<UISprite>().spriteName = "card_local_min_" + info.MianCardList[i].ToString();
                }
                if (card == GameData.MianCard)
                {
                    StartCoroutine(AsynCreateEffect(LocalViewDirection.RIGHT, CardOperateType.None, 1));
                }
                else
                {
                    StartCoroutine(AsynCreateEffect(LocalViewDirection.RIGHT, CardOperateType.None, 2));
                }
                break;
            case LocalViewDirection.UP:
                for (int i = 0; i < info.MianCardList.Count; i++)
                {
                    UpMianCardParent.transform.Find((i + 1).ToString()).gameObject.SetActive(true);
                    UpMianCardParent.transform.Find((i + 1).ToString()).GetComponent<UISprite>().spriteName = "card_local_min_" + info.MianCardList[i].ToString();
                }
                if (card == GameData.MianCard)
                {
                    StartCoroutine(AsynCreateEffect(LocalViewDirection.UP, CardOperateType.None, 1));
                }
                else
                {
                    StartCoroutine(AsynCreateEffect(LocalViewDirection.UP , CardOperateType.None, 2));
                }
                break;
            case LocalViewDirection.LEFT:
                for (int i = 0; i < info.MianCardList.Count; i++)
                {
                    LeftMianCardParent.transform.Find((i + 1).ToString()).gameObject.SetActive(true);
                    LeftMianCardParent.transform.Find((i + 1).ToString()).GetComponent<UISprite>().spriteName = "card_local_min_" + info.MianCardList[i].ToString();
                }
                if (card == GameData.MianCard)
                {
                    StartCoroutine(AsynCreateEffect(LocalViewDirection.LEFT, CardOperateType.None, 1));
                }
                else
                {
                    StartCoroutine(AsynCreateEffect(LocalViewDirection.LEFT, CardOperateType.None, 2));
                }
                break;

              
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        //面牌的重置
        for (int i = 0; i < SelfMianCardParent.transform.childCount; i++)
        {
            SelfMianCardParent.transform.Find((i+1).ToString()).gameObject.SetActive(false);
        }
        for (int i = 0; i < LeftMianCardParent.transform.childCount; i++)
        {
            LeftMianCardParent.transform.Find((i + 1).ToString()).gameObject.SetActive(false);
        }
        for (int i = 0; i < UpMianCardParent.transform.childCount; i++)
        {
            UpMianCardParent.transform.Find((i + 1).ToString()).gameObject.SetActive(false);
        }
        for (int i = 0; i < RightMianCardParent.transform.childCount; i++)
        {
            RightMianCardParent.transform.Find((i + 1).ToString()).gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 玩家碰牌杠牌
    /// </summary>
    /// <param name="playerGuid"></param>
    /// <param name="opType"></param>
    /// <param name="card"></param>
    /// <param name="outPos"></param>
    /// <param name="isNormal"></param>
    /// <returns></returns>
    IEnumerator AsynPengGang(ulong playerGuid, CardOperateType opType, uint card, byte outPos, bool isNormal = true)
    {
        PlayerInfo info = GameDataFunc.GetPlayerInfo(playerGuid);
        HoldCardsObj infoObj = GameDataFunc.GetHoldCardObj(info.LVD);
        List<uint> cards = new List<uint>();
        GameObject itemObj = null;
        string prdfix = "";
        bool isBuGang = false;
        int startIndex = 1;
        switch (opType)
        {
            case CardOperateType.PengPai:
                cards.Add(card); cards.Add(card); cards.Add(card);
                break;
            case CardOperateType.GangPai:
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
            Log.Debug("补杠");
            UISprite sp = itemObj.transform.Find("image4").GetComponent<UISprite>();
            sp.gameObject.SetActive(true);
            sp.spriteName = itemObj.transform.Find(card.ToString()).GetComponent<UISprite>().spriteName;
            sp.MakePixelPerfect();
            GameDataFunc.RemoverHoldCard(card, info.pos);
            GameDataFunc.RemoveHoldCardObj(card, info.LVD);
            ResetLocalHoldCards(info.pos);
        }
        else
        {
            switch (opType)
            {
                case CardOperateType.PengPai:
                case CardOperateType.GangPai:
                    //TranLastOutCardImg.gameObject.SetActive(false);
                    if (isNormal)
                    {
                        OpreateCardInfo cpg = new OpreateCardInfo();
                        cpg.pos = outPos;
                        cpg.opType = GetPengGangType(opType);
                        cpg.opCard = card;
                        info.operateCardList.Add(cpg);
                    }
                    break;
            }

            if (info.pos != outPos && isNormal)
            {
                GameDataFunc.RemoveOutCardInfo(outPos);
                PlayerInfo info1 = GameDataFunc.GetPlayerInfo(outPos);
                GameDataFunc.RemoveOutCardObj(info1.LVD);
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
                            GameDataFunc.RemoveHoldCardObj(cards[i], info.LVD);
                        }
                        ResetLocalHoldCards(info.pos);
                    }
                    if (pbLocalItem == null) pbLocalItem = Resources.Load<GameObject>("Item/LocalItem");
                    itemObj = Instantiate(pbLocalItem);
                    itemObj.transform.parent = LocalParent;
                    itemObj.transform.localScale = Vector3.one;
                    itemObj.transform.localPosition = new Vector3(-550 + 164 * count, -18, 0);
                    prdfix = "card_local_mid_";
                    break;
                case LocalViewDirection.RIGHT:
                    if (isNormal)
                    {
                        for (int i = startIndex; i < cards.Count; i++)
                        {
                            GameDataFunc.RemoverHoldCard(cards[i], info.pos);
                            GameDataFunc.RemoveHoldCardObj(cards[i], info.LVD);
                        }
                        //for (int i = 0; i < infoObj.holdObjList.Count; i++)
                        //{
                        //    infoObj.holdObjList[i].transform.localPosition = new Vector3(0, 239 - i * leftAndRightHideOffset);
                        //    infoObj.holdObjList[i].GetComponent<UISprite>().depth = i + 5;
                        //}
                        ResetLocalHoldCards(info.pos);
                    }
                    prdfix = "card_local_mid_";
                    if (pbUpItem == null) pbUpItem = Resources.Load<GameObject>("Item/UpItem");
                    itemObj = Instantiate<GameObject>(pbUpItem);
                    itemObj.transform.parent = RightParent;
                    itemObj.transform.localScale = Vector3.one;
                    itemObj.transform.localPosition = new Vector3(109, -147 + count * outCardOffsetY, 0);
                    itemObj.GetComponent<UIPanel>().depth = 25 - count;
                    break;
                case LocalViewDirection.UP:
                    if (isNormal)
                    {
                        for (int i = startIndex; i < cards.Count; i++)
                        {
                            GameDataFunc.RemoverHoldCard(cards[i], info.pos);
                            GameDataFunc.RemoveHoldCardObj(cards[i], info.LVD);
                        }
                        //for (int i = 0; i < infoObj.holdObjList.Count; i++)
                        //{
                        //    infoObj.holdObjList[i].transform.localPosition = new Vector3(-253 + 34 * i, 0);
                        //}
                        ResetLocalHoldCards(info.pos);
                    }
                    prdfix = "card_local_mid_";
                    if (pbUpItem == null) pbUpItem = Resources.Load<GameObject>("Item/UpItem");
                    itemObj = Instantiate<GameObject>(pbUpItem);
                    itemObj.transform.parent = UpParent;
                    itemObj.transform.localScale = Vector3.one;
                    itemObj.transform.localPosition = new Vector3(364 - count * 164, 0, 0);
                    itemObj.GetComponent<UIPanel>().depth = 25;
                    break;
                case LocalViewDirection.LEFT:
                    if (isNormal)
                    {
                        for (int i = startIndex; i < cards.Count; i++)
                        {
                            GameDataFunc.RemoverHoldCard(cards[i], info.pos);
                            GameDataFunc.RemoveHoldCardObj(cards[i], info.LVD);
                        }
                        //for (int i = 0; i < infoObj.holdObjList.Count; i++)
                        //{
                        //    infoObj.holdObjList[i].transform.localPosition = new Vector3(0, -144 + i * 25);
                        //    infoObj.holdObjList[i].GetComponent<UISprite>().depth = 20 - i;
                        //}
                        ResetLocalHoldCards(info.pos);
                    }
                    prdfix = "card_local_mid_";
                    if (pbUpItem == null) pbUpItem = Resources.Load<GameObject>("Item/UpItem");
                    itemObj = Instantiate(pbUpItem);
                    itemObj.transform.parent = LeftParent;
                    itemObj.transform.localScale = Vector3.one;
                    itemObj.transform.localPosition = new Vector3(-86, 289 - count * outCardOffsetY, 0);
                    itemObj.GetComponent<UIPanel>().depth = count + 25;
                    break;
            }
            PengOrGangObj objInfo = new PengOrGangObj();
            objInfo.pos = outPos;
            objInfo.opType = opType;
            objInfo.objBase = itemObj;
            itemObj.name = card.ToString();
            cards.Sort((a, b) => { return (int)a - (int)b; });
            int opCardPos = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                itemObj.transform.Find("image" + (i + 1).ToString()).gameObject.SetActive(true);
                UISprite sp = itemObj.transform.Find("image" + (i + 1).ToString()).GetComponent<UISprite>();
                sp.spriteName = prdfix + cards[i].ToString();
                if (card == cards[i]) opCardPos = i;
                sp.MakePixelPerfect();
                sp.gameObject.name = cards[i].ToString();
            }
            if (info.pos != outPos)
            {
                itemObj.transform.Find("direct").GetComponent<UISprite>().spriteName = GetDirectFileName(info.LVD, outPos);
                itemObj.transform.Find("direct").GetComponent<UISprite>().MakePixelPerfect();
            }
            else
            {
                itemObj.transform.Find("direct").gameObject.SetActive(false);
            }
            infoObj.pengGangList.Add(objInfo);
        }
        yield break;
    }

    //生成特定的特效
    IEnumerator AsynCreateEffect(LocalViewDirection lvd, CardOperateType type,int AddType=0)
    {
        if (pbEffectItem == null) pbEffectItem = Resources.Load<GameObject>("Item/EffectImage");
        GameObject effect = Instantiate(pbEffectItem);
        ObjEffect = effect;
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
}
