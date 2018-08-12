/*
* 开发人：滕俊
* 项目：
* 功能描述：
*
*
*/

using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NiuNiuPanel : MonoBehaviour {

    private uint RoundNum = 0;//局数
    private uint PayMethod = 0;//支付方式
    private uint DiFenIndex = 0;//底分选择
    private bool EnBoomNiu = false;
    private bool EnShunZhiNiu = false;
    private bool EnWuXiaoNiu = false;
    private bool EnWuHuaNiu = false;

    private bool EnXianJiaMaiMa = false;//允许闲家买码

    public UIButton DiFenOneBtn;//底分1
    public UIButton DiFenTwoBtn;
    public UIButton DiFenThreeBtn;


    public UIButton FourRoundBtn;
    public UIButton EightRoundBtn;
    public UIButton SixteenRoundBtn;

    public UILabel TaoShangRoundOne;
    public UILabel TaoShangRoundTwo;
    public UILabel TaoShangRoundThree;
    public UILabel TaoShangPayOne;
    public UILabel TaoShangPayTwo;

    public UIButton OwnerPayBtn;
    public UIButton AAPayBtn;

    public UIButton BoomNiuBtn;
    public UIButton ShunZhiNiuBtn;
    public UIButton WuXiaoNiuBtn;
    public UIButton WuHuaNiuBtn;
    public UIButton XianJiaMaiMaBtn;


    public UIButton CreatBtn;
    public UIButton InsteadBtn;//替人开房
    // Use this for initialization
    void Start()
    {
        if (Player.Instance.isDaiLi)
        {
            InsteadBtn.gameObject.SetActive(true);
        }
        else
        {
            InsteadBtn.gameObject.SetActive(false);
        }
        SetDDZBtnClick();
        CreatBtn.onClick.Add(new EventDelegate(this.CreatDDZRoom));
        InsteadBtn.onClick.Add(new EventDelegate(this.InsteadCreatDDZRoom));
       

        if (GameData.IsClubAutoCreatRoom)
        {
            InsteadBtn.gameObject.SetActive(false);
        }
     


    }

    private void OnEnable()
    {
        SetLableShow(0, 0);
        if (GameData.payMethod == -1)
        {
            OwnerPayBtnClick();
        }
      
        if (GameData.DiFen != -1)
        {
            switch (GameData.DiFen)
            {
                case 0:
                    DiFenOneClick();
                    break;
                case 1:
                    DiFenTwoBtnClick();
                    break;
                case 2:
                    DiFenThreeClick();
                    break;
                default:
                    break;
            }
        }
        if (GameData.Round != -1)
        {
            switch (GameData.Round)
            {
                case 0:
                    FourRoundBtnClick();
                    break;
                case 1:
                    EightRoundBtnClick();
                    break;
                case 2:
                    SixteenRoundBtnClick();
                    break;
                default:
                    break;
            }
        }
        if (GameData.payMethod != -1)
        {
            switch (GameData.payMethod)
            {
                case 0:
                    OwnerPayBtnClick();
                    break;
                case 1:
                    AAPayBtnClick();
                    break;

            }
        }
        if (GameData.ShunZhiNiu)
        {
            ShunZhiNiuBtnClick();
        }
        if (GameData.ZaDanNiu)
        {
            BoomNiuClick();
        }
        if (GameData.WuXiaoNiu)
        {
            WuXiaoNiuBtnClick();
        }
        if (GameData.WuHuaNiu)
        {
            WuHuaNiuBtnClick();
        }
        if (GameData.XianJiaMaiMa)
        {
            XianJiaMaiMaBtnClick();
        }

    }

    //private void OnEnable()
    //{
    //    OwnerPayBtnClick();
    //    //OwnerPayBtn.transform.FindChild("Sprite").gameObject.SetActive(true);
    //    //AAPayBtn.transform.FindChild("Sprite").gameObject.SetActive(false);
    //}
    /// <summary>
    /// 设置斗地主btn
    /// </summary>
    public void SetDDZBtnClick()
    {
        FourRoundBtn.onClick.Add(new EventDelegate(this.FourRoundBtnClick));
        EightRoundBtn.onClick.Add(new EventDelegate(this.EightRoundBtnClick));
        SixteenRoundBtn.onClick.Add(new EventDelegate(this.SixteenRoundBtnClick));
        OwnerPayBtn.onClick.Add(new EventDelegate(this.OwnerPayBtnClick));
        AAPayBtn.onClick.Add(new EventDelegate(this.AAPayBtnClick));


        DiFenOneBtn.onClick.Add(new EventDelegate(this.DiFenOneClick));
        DiFenTwoBtn.onClick.Add(new EventDelegate(this.DiFenTwoBtnClick));
        DiFenThreeBtn.onClick.Add(new EventDelegate(this.DiFenThreeClick));


     BoomNiuBtn.onClick.Add(new EventDelegate(this.BoomNiuClick));
     ShunZhiNiuBtn.onClick.Add(new EventDelegate(this.ShunZhiNiuBtnClick)); ;
     WuXiaoNiuBtn.onClick.Add(new EventDelegate(this.WuXiaoNiuBtnClick)); ;
     WuHuaNiuBtn.onClick.Add(new EventDelegate(this.WuHuaNiuBtnClick)); ;
     XianJiaMaiMaBtn.onClick.Add(new EventDelegate(this.XianJiaMaiMaBtnClick)); ;
    //FourRoundBtn1.onClick.Add(new EventDelegate(this.FourRoundBtnClick));
    //EightRoundBtn1.onClick.Add(new EventDelegate(this.EightRoundBtnClick));
    //SixteenRoundBtn1.onClick.Add(new EventDelegate(this.SixteenRoundBtnClick));
    //OwnerPayBtn1.onClick.Add(new EventDelegate(this.OwnerPayBtnClick));
    //AAPayBtn1.onClick.Add(new EventDelegate(this.AAPayBtnClick));
    //WithJiangMaBtn.onClick.Add(new EventDelegate(WithJiangMa));
    //WithOutJiangMaBtn.onClick.Add(new EventDelegate(WithOutJiangMa));
}

    private void XianJiaMaiMaBtnClick()
    {
        EnXianJiaMaiMa = !EnXianJiaMaiMa;
        GameData.XianJiaMaiMa = EnXianJiaMaiMa;
        if (EnXianJiaMaiMa)
        {
            XianJiaMaiMaBtn.transform.Find("Sprite").gameObject.SetActive(true);
        }
        else
        {
            XianJiaMaiMaBtn.transform.Find("Sprite").gameObject.SetActive(false);
        }
    }

    private void WuHuaNiuBtnClick()
    {
        EnWuHuaNiu = !EnWuHuaNiu;
        GameData.WuHuaNiu = EnWuHuaNiu;
        if (EnWuHuaNiu)
        {
            WuHuaNiuBtn.transform.Find("Sprite").gameObject.SetActive(true);
        }
        else
        {
            WuHuaNiuBtn.transform.Find("Sprite").gameObject.SetActive(false);
        }
    }

    private void WuXiaoNiuBtnClick()
    {
        EnWuXiaoNiu = !EnWuXiaoNiu;
        GameData.WuXiaoNiu = EnWuXiaoNiu;
        if (EnWuXiaoNiu)
        {
            WuXiaoNiuBtn .transform.Find("Sprite").gameObject.SetActive(true);
        }
        else
        {
            WuXiaoNiuBtn.transform.Find("Sprite").gameObject.SetActive(false);
        }
    }

    private void ShunZhiNiuBtnClick()
    {
        EnShunZhiNiu = !EnShunZhiNiu;
        GameData.ShunZhiNiu = EnShunZhiNiu;
        if (EnShunZhiNiu)
        {
            ShunZhiNiuBtn.transform.Find("Sprite").gameObject.SetActive(true);
        }
        else
        {
            ShunZhiNiuBtn.transform.Find("Sprite").gameObject.SetActive(false);
        }
    }

    private void BoomNiuClick()
    {
        EnBoomNiu = !EnBoomNiu;
        GameData.ZaDanNiu = EnBoomNiu;
        if (EnBoomNiu)
        {
            BoomNiuBtn.transform.Find("Sprite").gameObject.SetActive(true);
        }
        else
        {
            BoomNiuBtn.transform.Find("Sprite").gameObject.SetActive(false);
        }
    }

    private void DiFenThreeClick()
    {
        DiFenIndex = 2;
        DiFenOneBtn.transform.Find("Sprite").gameObject.SetActive(false);
        DiFenTwoBtn.transform.Find("Sprite").gameObject.SetActive(false);
        DiFenThreeBtn.transform.Find("Sprite").gameObject.SetActive(true);
        GameData.DiFen = (int)DiFenIndex;
    }

    private void DiFenTwoBtnClick()
    {
        DiFenIndex = 1;
        DiFenOneBtn.transform.Find("Sprite").gameObject.SetActive(false);
        DiFenTwoBtn.transform.Find("Sprite").gameObject.SetActive(true);
        DiFenThreeBtn.transform.Find("Sprite").gameObject.SetActive(false);
        GameData.DiFen = (int)DiFenIndex;
    }

    private void DiFenOneClick()
    {
        DiFenIndex = 0;
        DiFenOneBtn.transform.Find("Sprite").gameObject.SetActive(true);
        DiFenTwoBtn.transform.Find("Sprite").gameObject.SetActive(false);
        DiFenThreeBtn.transform.Find("Sprite").gameObject.SetActive(false);
        GameData.DiFen = (int)DiFenIndex;
    }

    private void AAPayBtnClick()
    {
        PayMethod = 1;
        GameData.payMethod = (int)PayMethod;
        AAPayBtn.transform.Find("Sprite").gameObject.SetActive(true);
        OwnerPayBtn.transform.Find("Sprite").gameObject.SetActive(false);
        //OwnerPayBtn.transform.FindChild("Sprite").gameObject.SetActive(false);
        //AAPayBtn.transform.FindChild("Sprite").gameObject.SetActive(true);
        SetLableShow((int)PayMethod, (int)RoundNum);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void OwnerPayBtnClick()
    {
        PayMethod = 0;
        GameData.payMethod = (int)PayMethod;
        AAPayBtn.transform.Find("Sprite").gameObject.SetActive(false);
        OwnerPayBtn.transform.Find("Sprite").gameObject.SetActive(true);
        //OwnerPayBtn.transform.FindChild("Sprite").gameObject.SetActive(true);
        //AAPayBtn.transform.FindChild("Sprite").gameObject.SetActive(false);
        SetLableShow((int)PayMethod, (int)RoundNum);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void SixteenRoundBtnClick()
    {
        RoundNum = 2;
        GameData.Round = (int)RoundNum;
        FourRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
        EightRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
        SixteenRoundBtn.transform.Find("Sprite").gameObject.SetActive(true);
        SetLableShow((int)PayMethod, (int)RoundNum);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void EightRoundBtnClick()
    {
        RoundNum = 1;
        GameData.Round = (int)RoundNum;
        SetLableShow((int)PayMethod, (int)RoundNum);
        FourRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
        EightRoundBtn.transform.Find("Sprite").gameObject.SetActive(true);
        SixteenRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void FourRoundBtnClick()
    {
        RoundNum = 0;
        GameData.Round = (int)RoundNum;
        SetLableShow((int)PayMethod, (int)RoundNum);
        FourRoundBtn.transform.Find("Sprite").gameObject.SetActive(true);
        EightRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
        SixteenRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);

    }
    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 代理替人开房
    /// </summary>
    private void InsteadCreatDDZRoom()
    {
        ClientToServerMsg.Send(Opcodes.Client_AgentCreateXYQPRoom, (byte)RoomType.NN, (byte)RoundNum, (byte)0, Input.location.lastData.latitude, Input.location.lastData.longitude, (byte)1, (byte)DiFenIndex, EnShunZhiNiu, EnBoomNiu, EnWuXiaoNiu, EnWuHuaNiu, EnXianJiaMaiMa);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    /// <summary>
    /// 创建ddz房间
    /// </summary>
    private void CreatDDZRoom()
    {

        // CreateRoomPayType俱乐部自动开房
        if (!GameData.IsClubAutoCreatRoom)
        {
            ClientToServerMsg.Send(Opcodes.Client_PlayerCreateXYQPRoom, (byte)RoomType.NN, (byte)RoundNum, (byte)PayMethod, Input.location.lastData.latitude, Input.location.lastData.longitude, (byte)1, (byte)DiFenIndex, EnShunZhiNiu, EnBoomNiu, EnWuXiaoNiu, EnWuHuaNiu, EnXianJiaMaiMa);
            SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        }
        else
        {
            ClientToServerMsg.Send(Opcodes.Client_Club_Config_AutoRoom,GameData.CurrentClubInfo.Id, (byte)RoomType.NN, (byte)RoundNum,  (byte)1, (byte)DiFenIndex, EnShunZhiNiu, EnBoomNiu, EnWuXiaoNiu, EnWuHuaNiu, EnXianJiaMaiMa);
            SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        }
      
    }

    int perFour = 1;//每人4局需要的钻石
    /// <summary>
    /// 设置讨赏的显示
    /// </summary>
    /// <param name="payindex"></param>
    /// <param name="round"></param>
    public void SetLableShow(int payindex, int round)
    {
        switch (payindex)
        {
            case 0://房主
                TaoShangRoundOne.text = string.Format("       10局(房卡X{0})", 5);
                TaoShangRoundTwo.text = string.Format("       20局(房卡X{0})",10);
                TaoShangRoundThree.text = string.Format("       30局(房卡X{0})", 15);

                TaoShangPayOne.text = string.Format("      房主支付");
                TaoShangPayTwo.text = string.Format("      平摊付");
                

                break;
            case 1://平摊
                TaoShangRoundOne.text = string.Format("       10局(房卡X{0})", 1);
                TaoShangRoundTwo.text = string.Format("       20局(房卡X{0})", 2);
                TaoShangRoundThree.text = string.Format("       30局(房卡X{0})", 3);

                TaoShangPayOne.text = string.Format("      房主支付");
                TaoShangPayTwo.text = string.Format("      平摊付");
               

                break;
        }
    }
}
