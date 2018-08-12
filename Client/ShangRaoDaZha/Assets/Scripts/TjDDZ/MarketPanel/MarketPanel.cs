using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketPanel : UIBase<MarketPanel>
{
    public GameObject MaskObj;
    public UIButton CloseBtn;

    public UIButton SixRoomCardBtn;
    public UIButton ThirtyRoomCardBtn;


    public UILabel[] diamondsLabel;//钻石描述
    public UILabel[] rmbLabel;//人民币描述
    public List<GameObject> ChongZhiList;//充值按钮


    public UIButton FirstDimToGold;
    public UIButton SecDimToGold;
    public UIButton ThreDimToGold;
    public UIButton FourDimToGold;
    // Use this for initialization
    void Start () {

        FirstDimToGold.onClick.Add(new EventDelegate(this.FirstDimToGoldClick));
        SecDimToGold.onClick.Add(new EventDelegate(this.SecDimToGoldClick));
        ThreDimToGold.onClick.Add(new EventDelegate(this.ThreDimToGoldClick));
        FourDimToGold.onClick.Add(new EventDelegate(this.FourDimToGoldClick));

        SixRoomCardBtn.onClick.Add(new EventDelegate(this.ChangeSixRoomCard));
        ThirtyRoomCardBtn.onClick.Add(new EventDelegate(this.ChangeThirtyRoomCard));
        UIEventListener.Get(MaskObj).onClick = this.ClickObj;
        CloseBtn.onClick.Add(new EventDelegate(this.Close));

        for (int i = 0; i < ChongZhiList.Count; i++)
        {
            UIEventListener.Get(ChongZhiList[i]).onClick = this.OnBuyDiamondClick;
        }


        if(GameData.IsAppStore && Application.platform == RuntimePlatform.IPhonePlayer)
        {
            foreach(UILabel item in diamondsLabel)
            {
                item.gameObject.SetActive(true);
            }
            foreach(UILabel item in rmbLabel)
            {
                item.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (UILabel item in diamondsLabel)
            {
                item.gameObject.SetActive(false);
            }
            foreach (UILabel item in rmbLabel)
            {
                item.text = "充值";
            }
        }
    }

    private void FourDimToGoldClick()
    {
        DiamIndex = 500;
        ChangeGold();
    }

    private void ThreDimToGoldClick()
    {
        DiamIndex =200;
        ChangeGold();
    }

    private void SecDimToGoldClick()
    {
        DiamIndex = 100;
        ChangeGold();
    }

    private void FirstDimToGoldClick()
    {
        DiamIndex = 50;
        ChangeGold();
    }

    //兑换30房卡
    private void ChangeThirtyRoomCard()
    {
        if (Player.Instance.money < 30)
        {
            GameData.ResultCodeStr = "钻石不足";
            UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
        }
        else
        {
            MoneyType type = MoneyType.Card;
            uint num =60;
            ClientToServerMsg.Send(Opcodes.Client_ExchangDiamondToCard, (byte)type,num);
        }
    }
    //兑换6房卡
    private void ChangeSixRoomCard()
    {
        if (Player.Instance.money < 6)
        {
            GameData.ResultCodeStr = "钻石不足";
            UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
        }
        else
        {
            MoneyType type = MoneyType.Card;
            uint num =12;
            ClientToServerMsg.Send(Opcodes.Client_ExchangDiamondToCard, (byte)type, num);
        }
    }

    private int DiamIndex = 0;
    //兑换30房卡
    private void ChangeGold()
    {
        if (Player.Instance.money < DiamIndex)
        {
            GameData.ResultCodeStr = "钻石不足";
            UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
        }
        else
        {
            MoneyType type = MoneyType.Gold;
            uint num = (uint)(DiamIndex * 10);
            ClientToServerMsg.Send(Opcodes.Client_ExchangDiamondToCard, (byte)type, num);
        }
    }
  


    private void Close()
    {
        UIManager.Instance.HideUiPanel(UIPaths.MarketPanel);
    }

    private void ClickObj(GameObject go)
    {
        if (go == MaskObj)
        {
            UIManager.Instance.HideUiPanel(UIPaths.MarketPanel);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnBuyDiamondClick(GameObject ClickedButton)
    {
        if (GameData.IsAppStore && Application.platform == RuntimePlatform.IPhonePlayer)
        {
            int index = 0;
         
            switch (ClickedButton.name)
            {
                case "RecordBtnSprite1":
                    index = 0;
                    break;
                case "RecordBtnSprite2":
                    index = 1;
                    break;
                case "RecordBtnSprite3":
                    index = 2;
                    break;
                case "RecordBtnSprite4":
                    index =3;
                    break;
            }

            InAppPurchasing.Instance.BuyProduct(index);
        }
        else
        {
            Application.OpenURL(GameData.RechargeURL + Player.Instance.guid.ToString());
        }
    }
}
