using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZaiBaoPanel : UIBase<ZaiBaoPanel>
{
    private uint RoundNum = 0;//局数
    private uint PayMethod = 0;//支付方式
  

    //   private bool JiangShangIndex;//是否奖码
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
   

    public UIButton CreatBtn;
    public UIButton InsteadBtn;//替人开房
    // Use this for initialization
    void OnEnable()
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
        SetLableShow((int)PayMethod, (int)RoundNum);

        CreatBtn.onClick.Add(new EventDelegate(this.CreatWDHRoom));
        InsteadBtn.onClick.Add(new EventDelegate(this.InsteadCreatWDHRoom));

        if (GameData.IsClubAutoCreatRoom)
        {
            InsteadBtn.gameObject.SetActive(false);
        }
    }

    private void InsteadCreatWDHRoom()
    {
        ClientToServerMsg.Send(Opcodes.Client_AgentCreateXYQPRoom, (byte)RoomType.ZB, (byte)RoundNum, (byte)0, Input.location.lastData.latitude, Input.location.lastData.longitude);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void CreatWDHRoom()
    {
        if (!GameData.IsClubAutoCreatRoom)
        {
            ClientToServerMsg.Send(Opcodes.Client_PlayerCreateXYQPRoom, (byte)RoomType.ZB, (byte)RoundNum, (byte)PayMethod, Input.location.lastData.latitude, Input.location.lastData.longitude);
            SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        }
        else
        {
            ClientToServerMsg.Send(Opcodes.Client_Club_Config_AutoRoom,GameData.CurrentClubInfo.Id, (byte)RoomType.ZB, (byte)RoundNum);
            SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        }
           
    }

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

     
    }
    // Update is called once per frame
    void Update()
    {

    }

    #region   
   
    private void AAPayBtnClick()
    {
        PayMethod = 1;
        SetLableShow((int)PayMethod, (int)RoundNum);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void OwnerPayBtnClick()
    {
        PayMethod = 0;
        SetLableShow((int)PayMethod, (int)RoundNum);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void SixteenRoundBtnClick()
    {
        RoundNum = 2;
        SetLableShow((int)PayMethod, (int)RoundNum);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void EightRoundBtnClick()
    {
        RoundNum = 1;
        SetLableShow((int)PayMethod, (int)RoundNum);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void FourRoundBtnClick()
    {
        RoundNum = 0;
        SetLableShow((int)PayMethod, (int)RoundNum);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    #endregion
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

                OwnerPayBtn.transform.Find("Sprite").gameObject.SetActive(true);
                AAPayBtn.transform.Find("Sprite").gameObject.SetActive(false);

                TaoShangRoundOne.text = string.Format("八局(房卡X{0})", 2*4 * perFour);
                TaoShangRoundTwo.text = string.Format("十二局(房卡X{0})", 3 * 4 * perFour);
                TaoShangRoundThree.text = string.Format("十六局(房卡X{0})", 4 * 4 * perFour);
                switch (round)
                {
                    case 0:
                        TaoShangPayOne.text = string.Format("房主支付(房卡X{0})",2* 4 * perFour);
                        TaoShangPayTwo.text = string.Format("平摊付(房卡X{0})", 2*perFour);

                        FourRoundBtn.transform.Find("Sprite").gameObject.SetActive(true);
                        EightRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
                        SixteenRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
                        break;
                    case 1:
                        TaoShangPayOne.text = string.Format("房主支付(房卡X{0})", 3 * 4 * perFour);
                        TaoShangPayTwo.text = string.Format("平摊付(房卡X{0})", 3 * perFour);

                        FourRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
                        EightRoundBtn.transform.Find("Sprite").gameObject.SetActive(true);
                        SixteenRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
                        break;
                    case 2:
                        TaoShangPayOne.text = string.Format("房主支付(房卡X{0})", 4 * 4 * perFour);
                        TaoShangPayTwo.text = string.Format("平摊付(房卡X{0})", 4 * perFour);

                        FourRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
                        EightRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
                        SixteenRoundBtn.transform.Find("Sprite").gameObject.SetActive(true);
                        break;
                }

                break;
            case 1://平摊
                OwnerPayBtn.transform.Find("Sprite").gameObject.SetActive(false);
                AAPayBtn.transform.Find("Sprite").gameObject.SetActive(true);

                TaoShangRoundOne.text = string.Format("八局(房卡X{0})",2* perFour);
                TaoShangRoundTwo.text = string.Format("十二局(房卡X{0})", 3 * perFour);
                TaoShangRoundThree.text = string.Format("十六局(房卡X{0})", 4 * perFour);

                switch (round)
                {
                    case 0:
                        TaoShangPayOne.text = string.Format("房主支付(房卡X{0})", 2*4 * perFour);
                        TaoShangPayTwo.text = string.Format("平摊付(房卡X{0})", 2*perFour);

                        FourRoundBtn.transform.Find("Sprite").gameObject.SetActive(true);
                        EightRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
                        SixteenRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
                        break;
                    case 1:
                        TaoShangPayOne.text = string.Format("房主支付(房卡X{0})", 3 * 4 * perFour);
                        TaoShangPayTwo.text = string.Format("平摊付(房卡X{0})", 3 * perFour);

                        FourRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
                        EightRoundBtn.transform.Find("Sprite").gameObject.SetActive(true);
                        SixteenRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
                        break;
                    case 2:
                        TaoShangPayOne.text = string.Format("房主支付(房卡X{0})", 4 * 4 * perFour);
                        TaoShangPayTwo.text = string.Format("平摊付(房卡X{0})", 4 * perFour);

                        FourRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
                        EightRoundBtn.transform.Find("Sprite").gameObject.SetActive(false);
                        SixteenRoundBtn.transform.Find("Sprite").gameObject.SetActive(true);
                        break;
                }


                break;
        }

      
    }
}
