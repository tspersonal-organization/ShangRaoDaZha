using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : UIBase<UIMain>
{
    public UILabel LBName;
    public UILabel LBGuid;
    public UILabel LBMoney;
    public UITexture ImgHead;

    void Start()
    {
        foreach (Transform item in transform.Find("BtnBase"))
            UIEventListener.Get(item.gameObject).onClick = OnClick;

        DownloadImage.Instance.Download(ImgHead,Player.Instance.headID);

        LBName.text = Player.Instance.otherName;
        LBGuid.text = "ID:" + Player.Instance.guid;
        Money = Player.Instance.money;

    }

    public long Money { set { LBMoney.text = value.ToString(); } }

    void OnClick(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        switch (go.name)
        {
            case "btnCreate":
                UIManager.Instance.ShowUiPanel(UIPaths.UIPanel_CreateRoom, OpenPanelType.MinToMax);
                break;
            case "btnAddRoom":
                UIManager.Instance.ShowUiPanel(UIPaths.UIPanel_AddRoom, OpenPanelType.MinToMax);
                break;
            case "btnSetting":
                UIManager.Instance.ShowUiPanel(UIPaths.UIPanel_Setting, OpenPanelType.MinToMax);
                break;
            case "btnWanFa":
                UIManager.Instance.ShowUiPanel(UIPaths.UIPanel_Help, OpenPanelType.MinToMax);
                break;
            case "btnGongGao":
                UIManager.Instance.ShowUiPanel(UIPaths.UIPanel_GongGao, OpenPanelType.MinToMax);
                break;
            case "btnZhanJi":
                UIManager.Instance.ShowUiPanel(UIPaths.PanelHistory, OpenPanelType.MinToMax);
                break;
            case "btnStore":
            case "btnAddMoney":
                UIManager.Instance.ShowUiPanel(UIPaths.UIPanel_Shop, OpenPanelType.MinToMax);
                break;
        }
    }

}
