using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoundOver : UIBase<UIRoundOver>
{
    public GameObject[] ItemArray;//每个item
    public GameObject btnShare;
    public GameObject btnRight;
    public Transform TranMyFrame;
    public UISprite ImgType;

    public UIButton ChangeTable;
    public UIButton QuiteBtn;

   // int[] baseScoreArray = new int[] { 1, 1, 3, 5, 10 };
 //   string[] baseString = new string[] {"缺门", "缺一", "缺三", "缺五", "缺十", };

	// Use this for initialization
	void Start ()
    {
        if (GameData.m_TableInfo.IsPiPei)
        {
            QuiteBtn.gameObject.SetActive(true);
            ChangeTable.gameObject.SetActive(true);
            btnShare.SetActive(false);
        }
        else
        {
            QuiteBtn.gameObject.SetActive(false);
            ChangeTable.gameObject.SetActive(false);
            btnShare.SetActive(true);
        }
        UIEventListener.Get(QuiteBtn.gameObject).onClick = OnClick;
        UIEventListener.Get(btnShare).onClick = OnClick;
        UIEventListener.Get(btnRight).onClick = OnClick;
        UIEventListener.Get(ChangeTable.gameObject).onClick = OnClick;
        // GameDataFunc.GetPlayerInfo
        //ImgType.spriteName = GameDataFunc.GetPlayerInfo(Player.Instance.guid).changeScore >= 0? "UI_GameResult_icon_win" : "UI_GameResult_icon_lose";
        if (GameData.m_RoundOverInfo.isHuPai)//有人胡牌
        {
            ImgType.spriteName = GameDataFunc.GetPlayerInfo(Player.Instance.guid).huType != HuType.None ? "UI_GameResult_icon_win" : "UI_GameResult_icon_lose";
        }
        else//流局
        {
            ImgType.spriteName = "UI_GameResult_icon_draw";
        }
           
        if (GameDataFunc.GetPlayerInfo(Player.Instance.guid).huType != HuType.None)//胡牌了
        {
            SoundManager.Instance.PlaySound(UIPaths.GAMEWIN);
        }
        else
        {
            SoundManager.Instance.PlaySound(UIPaths.GAMELOSE);
        }
        if (GameData.m_IsNormalOver)
        {
            btnRight.GetComponent<UISprite>().spriteName = "UI_GameResult_btn_RoomResult";
        }

        for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        {
            PlayerInfo info = GameData.m_PlayerInfoList[i];
            GameObject obj = ItemArray[info.pos - 1];
            obj.SetActive(true);
            obj.transform.Find("name").GetComponent<UILabel>().text = info.name;
            if (!GameData.m_TableInfo.IsPiPei)
            {
                obj.transform.Find("score").GetComponent<UILabel>().text = info.changeScore.ToString();
            }
            else
            {
                obj.transform.Find("score").GetComponent<UILabel>().text = info.changeScore.ToString();
               // obj.transform.Find("score").GetComponent<UILabel>().text = (info.changeScore * JinBiDataControl.Instance.TaoSHangRate).ToString();
            }
           

            DownloadImage.Instance.Download(obj.transform.Find("headImage").GetComponent<UITexture>(), info.headID);


            obj.transform.Find("ZhuangSprite").gameObject.SetActive(false);
            if (info.pos == GameData.m_TableInfo.ZhuangPos)
            {
                obj.transform.Find("ZhuangSprite").gameObject.SetActive(true);
            }

            if (GameData.GlobleRoomType == RoomType.WDH)
            {
                #region  wdh
                if (info.huType != HuType.None)
                {
                    obj.transform.Find("rightTxt").GetComponent<UILabel>().text = "";
                    obj.transform.Find("leftTxt").GetComponent<UILabel>().text = "";
                    //        None = 0,
                    //QiDui = 1,
                    //Bao_GangKai = 2,
                    //QiXing = 3,
                    //Bao_QiXing = 4,
                    //PengPengHu = 5,
                    //Bao_PengPengHu = 6

                    if (info.prizeType == PrizeType.TianHu)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X2\n";// "七对\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "天胡\n";
                    }
                    if (info.prizeType == PrizeType.Bao_TianHu)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X1\n";// "七对\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "天胡当宝\n";
                    }
                    if (info.prizeType == PrizeType.QiDui)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X4\n";// "七对\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "七对\n";
                    }
                    else if (info.prizeType == PrizeType.PengPengHu)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X2\n";// "碰碰胡不当宝\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "碰碰胡不当宝\n";
                    }
                    else if (info.prizeType == PrizeType.Bao_PengPengHu)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X1\n";// "碰碰胡不当宝\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "碰碰胡当宝\n";
                    }
                    else if (info.prizeType == PrizeType.QiXing)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X2\n";// "七星\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "七星不当宝\n";
                    }
                    else if (info.prizeType == PrizeType.Bao_QiXing)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X1\n";// "七星\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "七星当宝\n";
                    }
                    if (info.prizeType == PrizeType.DanDiao)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X4\n";// "七星\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "单吊不当宝\n";
                    }
                    if (info.prizeType == PrizeType.Bao_DanDiao)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X2\n";// "七星\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "单吊当宝\n";
                    }

                    if (info.prizeType == PrizeType.Bao_GangKai)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X1\n";// "七星\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "杠开当宝\n";
                    }
                    if (info.GangKaiCount > 0 && info.prizeType != PrizeType.Bao_GangKai)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X2\n";// "杠开不当宝\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "杠开不当宝\n";
                    }
                    if (info.GangKaiCount > 1)
                    {
                        switch (info.GangKaiCount)
                        {
                            case 1:
                                obj.transform.Find("rightTxt").Find("LiangxuGang").GetComponent<UILabel>().text = "X1";
                                break;
                            case 2:
                                obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X2\n";// "连续杠开\n";
                                obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "连续杠开\n";
                                break;
                            case 3:
                                obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X4\n";// "连续杠开\n";
                                obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "连续杠开\n";
                                break;
                            case 4:
                                obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "X4\n";// "连续杠开\n";
                                obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "连续杠开\n";
                                break;
                        }
                    }

                    
                    if (GameData.m_TableInfo.IsJIangMa)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += info.TaoShangScore.ToString() + "\n";// "奖码\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "奖码\n";
                    }


                }
                else if (info.huType == HuType.None)
                {
                    obj.transform.Find("rightTxt").GetComponent<UILabel>().text = "";
                    obj.transform.Find("leftTxt").GetComponent<UILabel>().text = "";


                }

                #endregion
            }
            else if (GameData.GlobleRoomType == RoomType.ZB)
            {

                #region zb
                obj.transform.Find("rightTxt").GetComponent<UILabel>().text = "";
                obj.transform.Find("leftTxt").GetComponent<UILabel>().text = "";
                if (info.huType != HuType.None)
                {
                    switch (info.huType)
                    {
                       
                        case HuType.QiDuiHu:
                            obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "3分\n";// "七对\n";
                            obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "七对\n";
                            break;
                        case HuType.ShiSiBuDa:
                            obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "3分\n";// "七对\n";
                            obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "十四不搭\n";
                            break;
                        case HuType.YaoJiuPai:
                            obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "3分\n";// "七对\n";
                            obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "幺九牌\n";
                            break;
                        case HuType.Normal:
                            obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "2分\n";// "七对\n";
                            obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "普通胡\n";
                            break;
                    }

                    if (!GameData.m_RoundOverInfo.IsUseMagicCard)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "1分\n";// 
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "无挡\n";
                    }
                    if (GameData.m_RoundOverInfo.MianCount > 0|| GameData.m_RoundOverInfo.GangCount>0)
                    {
                        obj.transform.Find("rightTxt").GetComponent<UILabel>().text += (GameData.m_RoundOverInfo.GangCount+ GameData.m_RoundOverInfo.MianCount).ToString()+ "分\n";// "七对\n";
                        obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "吃面X"+ (GameData.m_RoundOverInfo.GangCount + GameData.m_RoundOverInfo.MianCount).ToString()+"\n";
                    }
                    switch ((int)GameData.m_RoundOverInfo.ZaiBaoCount)
                    {
                        case 1:
                            obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "10分\n";// "七对\n";
                            obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "栽宝X" + GameData.m_RoundOverInfo.ZaiBaoCount.ToString() + "\n";
                            break;
                        case 2:
                            obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "30分\n";// "七对\n";
                            obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "栽宝X" + GameData.m_RoundOverInfo.ZaiBaoCount.ToString() + "\n";
                            break;
                        case 3:
                            obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "50分\n";// "七对\n";
                            obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "栽宝X" + GameData.m_RoundOverInfo.ZaiBaoCount.ToString() + "\n";
                            break;
                        case 4:
                            obj.transform.Find("rightTxt").GetComponent<UILabel>().text += "70分\n";// "七对\n";
                            obj.transform.Find("leftTxt").GetComponent<UILabel>().text += "栽宝X" + GameData.m_RoundOverInfo.ZaiBaoCount.ToString() + "\n";
                            break;


                    }
                }
               

                #endregion
            }



            #region
            //string leftTxt = "跑分";
            //string rightTxt = info.fangPaoScore.ToString();
            //if(info.menCount < 3)
            //{
            //    leftTxt += "\n"+baseString[(int)info.huType];
            //    rightTxt += "\n*" + baseScoreArray[(int)info.huType];

            //    if (info.pos == GameData.m_RoundOverInfo.huPos && GameData.m_RoundOverInfo.isHuPai)
            //    {
            //        if (GameData.m_RoundOverInfo.isDianPaoHu || GameData.m_RoundOverInfo.isQiangGuangHu)
            //        {
            //            leftTxt += "\n点炮";
            //            rightTxt += "\n*1";
            //        }
            //        else
            //        {
            //            leftTxt += "\n自摸";
            //            rightTxt += "\n*4";
            //        }
            //    }
            //    int mingGangCount = 0;
            //    int anGangCount = 0;
            //    for (int k = 0; k < info.operateCardList.Count; k++)
            //    {
            //        if (info.operateCardList[k].opType == CatchType.AnGang)
            //        {
            //            anGangCount++;
            //        }
            //        else if (info.operateCardList[k].opType == CatchType.Gang || info.operateCardList[k].opType == CatchType.BuGang)
            //        {
            //            mingGangCount++;
            //        }
            //    }
            //    if (anGangCount > 0)
            //    {
            //        leftTxt += "\n暗杠*" + anGangCount;
            //        int baseScore = baseScoreArray[(int)info.huType];
            //        int totalScore = 0;
            //        for (int k = 0; k < GameData.m_PlayerInfoList.Count; k++)
            //        {
            //            if (GameData.m_PlayerInfoList[k].pos == info.pos) continue;
            //            int tempScore = (int)(info.fangPaoScore + GameData.m_PlayerInfoList[k].fangPaoScore);
            //            totalScore += (tempScore +1) * 4*anGangCount*baseScore;
            //        }
            //        rightTxt += "\n+" + totalScore;

            //    }
            //    if (mingGangCount > 0)
            //    {
            //        leftTxt += "\n明杠*" + mingGangCount;
            //        int baseScore = baseScoreArray[(int)info.huType];
            //        int totalScore = 0;
            //        for (int k = 0; k < GameData.m_PlayerInfoList.Count; k++)
            //        {
            //            if (GameData.m_PlayerInfoList[k].pos == info.pos) continue;
            //            int tempScore = (int)(info.fangPaoScore + GameData.m_PlayerInfoList[k].fangPaoScore);
            //            totalScore += (tempScore+1) * mingGangCount*baseScore;
            //        }
            //        rightTxt += "\n+" + totalScore;
            //    }
            //}

            //obj.transform.Find("leftTxt").GetComponent<UILabel>().text = leftTxt;
            //obj.transform.Find("rightTxt").GetComponent<UILabel>().text = rightTxt;

            #endregion
        }
        ItemArray[0].transform.parent.GetComponent<UIGrid>().enabled = true;

        TranMyFrame.parent = ItemArray[GameDataFunc.GetPlayerInfo(Player.Instance.guid).pos - 1].transform;
        TranMyFrame.localPosition = Vector3.zero;

        GameDataReset();
    }

    /// <summary>
    /// 一局结束重置
    /// </summary>
    public void GameDataReset()
    {
        GameData.m_TableInfo.ChiMianCount = 0;
        GameData.m_TableInfo.GangCount = 0;
    }

    private void OnClick(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        if (go == btnShare)
        {
            AuthorizeOrShare.Instance.ShareCapture();
        }
        else if(go == btnRight)
        {
            if(GameData.m_IsNormalOver)
            {
                UIManager.Instance.ShowUiPanel(UIPaths.UIPanel_TotalScore, OpenPanelType.MinToMax);
                UIManager.Instance.HideUiPanel(UIPaths.UIPanel_RoundOver);
            }
            else
            {
                ClientToServerMsg.Send(Opcodes.Client_PlayerReady, GameData.m_TableInfo.id);
            }

            UIManager.Instance.HideUiPanel(UIPaths.UIPanel_RoundOver);
        }
        else if (go == ChangeTable.gameObject)
        {
            ClientToServerMsg.Send(Opcodes.Client_PiPei_ChangeDesk, (byte)GameData.GlobleRoomType, GameData.m_TableInfo.id, Input.location.lastData.latitude, Input.location.lastData.longitude);
        }

        else if (go == QuiteBtn.gameObject)
        {
            ClientToServerMsg.Send(Opcodes.Client_PlayerLeaveRoom, GameData.m_TableInfo.id, true);
        }

    }
}
