using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatRoomPanel : UIBase<CreatRoomPanel>
{
    public GameObject WuDangHu;
    public GameObject ZaiBao;
    public GameObject TaoShang;
    public GameObject NiuNiuObj;//牛牛按钮

    public GameObject WuDangHuPanel;
    public GameObject ZaiBaoPanel;
    public GameObject TaoShangPanel;
    public GameObject NiuNiuPanel;//牛牛按钮






    public UIButton BackBtn;
    public GameObject MaskObj;
 
    // Use this for initialization
    void Start () {

       // TaoShangClidk(null);//默认为讨赏界面

       
      //  BackBtn.onClick.Add(new EventDelegate(this.Close));
       // UIEventListener.Get(MaskObj).onClick = this.CloseClick;
    


        //if (GameData.IsAppStore && Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    WuDangHu.SetActive(false);
        //    ZaiBao.SetActive(false);
        //}
        //else
        //{
        //    WuDangHu.SetActive(true);
        //    ZaiBao.SetActive(true);
        //}

        //UIEventListener.Get(TaoShang).onClick = this.TaoShangClidk;
        //UIEventListener.Get(ZaiBao).onClick = this.ZaiBaoClidk;
        //UIEventListener.Get(WuDangHu).onClick = this.WuDangHuClidk;
        //UIEventListener.Get(NiuNiuObj).onClick = this.NiuNiuObjClidk;
    }



    #region  click
    //切换到牛牛
    private void NiuNiuObjClidk(GameObject go)
    {
        WuDangHuPanel.SetActive(false);
        ZaiBaoPanel.SetActive(false);
        TaoShangPanel.SetActive(false);
        NiuNiuPanel.SetActive(true);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    //切换到无挡胡
    private void WuDangHuClidk(GameObject go)
    {
        NiuNiuPanel.SetActive(false);
        WuDangHuPanel.SetActive(true);
        ZaiBaoPanel.SetActive(false);
        TaoShangPanel.SetActive(false);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

   //切换到载保
    private void ZaiBaoClidk(GameObject go)
    {
        NiuNiuPanel.SetActive(false);
        WuDangHuPanel.SetActive(false);
        ZaiBaoPanel.SetActive(true);
        TaoShangPanel.SetActive(false);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    //切换到讨赏
    private void TaoShangClidk(GameObject go)
    {
        NiuNiuPanel.SetActive(false);
        WuDangHuPanel.SetActive(false);
        ZaiBaoPanel.SetActive(false);
        TaoShangPanel.SetActive(true);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    #endregion
    /// <summary>
    /// 代理替人开房
    /// </summary>
    //private void InsteadCreatDDZRoom()
    //{
    //    ClientToServerMsg.Send(Opcodes.Client_AgentCreateXYQPRoom, (byte)RoundNum, (byte)0, Input.location.lastData.latitude, Input.location.lastData.longitude);
    //}

    private void CloseClick(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        GameData.IsClubAutoCreatRoom = false;
        Close();
    }

    private void Close()
    {
        UIManager.Instance.HideUiPanel(UIPaths.PanelCreatRoom);
    }

    /// <summary>
    /// 创建ddz房间
    /// </summary>
    //private void CreatDDZRoom()
    //{

    //    if (type == RoomType.XYGQP)
    //    {
    //        ClientToServerMsg.Send(Opcodes.Client_PlayerCreateXYQPRoom, (byte)RoundNum, (byte)PayMethod, Input.location.lastData.latitude, Input.location.lastData.longitude);
    //    }
    //    else if (type == RoomType.WDH)
    //    {
    //        ClientToServerMsg.Send(Opcodes.Client_PlayerCreateXYQPRoom,(byte)type, (byte)RoundNum, (byte)PayMethod, Input.location.lastData.latitude, Input.location.lastData.longitude);
    //    }
      

    //}

    // Update is called once per frame
    void Update () {
		
	}

    #region  ddz Btnclick
    /// <summary>
    /// 设置斗地主btn
    /// </summary>
    //public void SetDDZBtnClick()
    //{
    //    FourRoundBtn.onClick.Add(new EventDelegate(this.FourRoundBtnClick));
    //    EightRoundBtn.onClick.Add(new EventDelegate(this.EightRoundBtnClick));
    //    SixteenRoundBtn.onClick.Add(new EventDelegate(this.SixteenRoundBtnClick));
    //    OwnerPayBtn.onClick.Add(new EventDelegate(this.OwnerPayBtnClick));
    //    AAPayBtn.onClick.Add(new EventDelegate(this.AAPayBtnClick));

    //    FourRoundBtn1.onClick.Add(new EventDelegate(this.FourRoundBtnClick));
    //    EightRoundBtn1.onClick.Add(new EventDelegate(this.EightRoundBtnClick));
    //    SixteenRoundBtn1.onClick.Add(new EventDelegate(this.SixteenRoundBtnClick));
    //    OwnerPayBtn1.onClick.Add(new EventDelegate(this.OwnerPayBtnClick));
    //    AAPayBtn1.onClick.Add(new EventDelegate(this.AAPayBtnClick));
    //    WithJiangMaBtn.onClick.Add(new EventDelegate(WithJiangMa));
    //    WithOutJiangMaBtn.onClick.Add(new EventDelegate(WithOutJiangMa));
    //}

    #endregion
    #region  btclick

    ////不带奖码
    //private void WithOutJiangMa()
    //{
    //    JiangShangIndex = false;
    //}

    ////带奖码
    //private void WithJiangMa()
    //{
    //    JiangShangIndex =true;
    //}

    //private void AAPayBtnClick()
    //{
    //    PayMethod = 1;
    //    switch (type)
    //    {
    //        case RoomType.XYGQP:
    //            SetLableShow((int)PayMethod, (int)RoundNum);
    //            break;
    //        case RoomType.WDH:
    //            break;
    //    }

    //}

    //private void OwnerPayBtnClick()
    //{
    //    PayMethod = 0;
    //    switch (type)
    //    {
    //        case RoomType.XYGQP:
    //            SetLableShow((int)PayMethod, (int)RoundNum);
    //            break;
    //        case RoomType.WDH:
    //            break;
    //    }
    //}

    //private void SixteenRoundBtnClick()
    //{
    //    RoundNum = 2;
    //    switch (type)
    //    {
    //        case RoomType.XYGQP:
    //            SetLableShow((int)PayMethod, (int)RoundNum);
    //            break;
    //        case RoomType.WDH:
    //            break;
    //    }
    //}

    //private void EightRoundBtnClick()
    //{
    //    RoundNum = 1;

    //    switch (type)
    //    {
    //        case RoomType.XYGQP:
    //            SetLableShow((int)PayMethod, (int)RoundNum);
    //            break;
    //        case RoomType.WDH:
    //            break;
    //    }
    //}

    //private void FourRoundBtnClick()
    //{
    //    RoundNum = 0;
    //    switch (type)
    //    {
    //        case RoomType.XYGQP:
    //            SetLableShow((int)PayMethod, (int)RoundNum);
    //            break;
    //        case RoomType.WDH:
    //            break;
    //    }
    //}


    //#endregion  


    //int perFour =1;//每人4局需要的钻石
    ///// <summary>
    ///// 设置讨赏的显示
    ///// </summary>
    ///// <param name="payindex"></param>
    ///// <param name="round"></param>
    //public void SetLableShow(int payindex,int round)
    //{
    //    switch (payindex)
    //    {
    //        case 0://房主
    //           TaoShangRoundOne.text=string.Format("四局(房卡X{0})",4*perFour);
    //            TaoShangRoundTwo.text=string.Format("八局(房卡X{0})", 2*4 * perFour); 
    //         TaoShangRoundThree.text = string.Format("十六局(房卡X{0})", 4 * 4 * perFour);
    //            switch (round)
    //            {
    //                case 0:
    //                    TaoShangPayOne.text = string.Format("房主支付(房卡X{0})",  4 * perFour);
    //                    TaoShangPayTwo.text = string.Format("平摊付(房卡X{0})",  perFour);
    //                    break;
    //                case 1:
    //                    TaoShangPayOne.text = string.Format("房主支付(房卡X{0})", 2 * 4 * perFour);
    //                    TaoShangPayTwo.text = string.Format("平摊付(房卡X{0})", 2 * perFour);
    //                    break;
    //                case 2:
    //                    TaoShangPayOne.text = string.Format("房主支付(房卡X{0})", 4 * 4 * perFour);
    //                    TaoShangPayTwo.text = string.Format("平摊付(房卡X{0})", 4 * perFour);
    //                    break;
    //            }

    //            break;
    //        case 1://平摊
    //            TaoShangRoundOne.text = string.Format("四局(房卡X{0})",  perFour);
    //            TaoShangRoundTwo.text = string.Format("八局(房卡X{0})", 2 *  perFour);
    //            TaoShangRoundThree.text = string.Format("十六局(房卡X{0})", 4 *  perFour);

    //            switch (round)
    //            {
    //                case 0:
    //                    TaoShangPayOne.text = string.Format("房主支付(房卡X{0})", 4 * perFour);
    //                    TaoShangPayTwo.text = string.Format("平摊付(房卡X{0})", perFour);
    //                    break;
    //                case 1:
    //                    TaoShangPayOne.text = string.Format("房主支付(房卡X{0})", 2 * 4 * perFour);
    //                    TaoShangPayTwo.text = string.Format("平摊付(房卡X{0})", 2 * perFour);
    //                    break;
    //                case 2:
    //                    TaoShangPayOne.text = string.Format("房主支付(房卡X{0})", 4 * 4 * perFour);
    //                    TaoShangPayTwo.text = string.Format("平摊付(房卡X{0})", 4 * perFour);
    //                    break;
    //            }


    //            break;
    //    }
    //}

    ///// <summary>
    ///// 设置无挡胡的显示
    ///// </summary>
    ///// <param name="payindex"></param>
    ///// <param name="round"></param>
    ///// <param name="isJiangShang"></param>
    //public void SetWuDangHuLableShow(int payindex, int round,bool isJiangShang)
    //{
    //    switch (payindex)
    //    {
    //        case 0://房主
    //            TaoShangRoundOne1.text = string.Format("四局(房卡X{0})", 4 * perFour);
    //            TaoShangRoundTwo1.text = string.Format("八局(房卡X{0})", 2 * 4 * perFour);
    //            TaoShangRoundThree1.text = string.Format("十六局(房卡X{0})", 4 * 4 * perFour);
    //            switch (round)
    //            {
    //                case 0:
    //                    TaoShangPayOne1.text = string.Format("房主支付(房卡X{0})", 4 * perFour);
    //                    TaoShangPayTwo1.text = string.Format("平摊付(房卡X{0})", perFour);
    //                    break;
    //                case 1:
    //                    TaoShangPayOne1.text = string.Format("房主支付(房卡X{0})", 2 * 4 * perFour);
    //                    TaoShangPayTwo1.text = string.Format("平摊付(房卡X{0})", 2 * perFour);
    //                    break;
    //                case 2:
    //                    TaoShangPayOne1.text = string.Format("房主支付(房卡X{0})", 4 * 4 * perFour);
    //                    TaoShangPayTwo1.text = string.Format("平摊付(房卡X{0})", 4 * perFour);
    //                    break;
    //            }

    //            break;
    //        case 1://平摊
    //            TaoShangRoundOne1.text = string.Format("四局(房卡X{0})", perFour);
    //            TaoShangRoundTwo1.text = string.Format("八局(房卡X{0})", 2 * perFour);
    //            TaoShangRoundThree1.text = string.Format("十六局(房卡X{0})", 4 * perFour);

    //            switch (round)
    //            {
    //                case 0:
    //                    TaoShangPayOne1.text = string.Format("房主支付(房卡X{0})", 4 * perFour);
    //                    TaoShangPayTwo1.text = string.Format("平摊付(房卡X{0})", perFour);
    //                    break;
    //                case 1:
    //                    TaoShangPayOne1.text = string.Format("房主支付(房卡X{0})", 2 * 4 * perFour);
    //                    TaoShangPayTwo1.text = string.Format("平摊付(房卡X{0})", 2 * perFour);
    //                    break;
    //                case 2:
    //                    TaoShangPayOne1.text = string.Format("房主支付(房卡X{0})", 4 * 4 * perFour);
    //                    TaoShangPayTwo1.text = string.Format("平摊付(房卡X{0})", 4 * perFour);
    //                    break;
    //            }


    //            break;
    //    }

    //    if (isJiangShang)
    //    {

    //    }
    //    else
    //    {

    //    }


    //}
    #endregion
}
