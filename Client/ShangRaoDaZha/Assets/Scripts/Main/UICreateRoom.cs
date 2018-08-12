using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum TestEnum
{
    [Description("aaa")] aaa,
    [Description("bbb")] bbb,
    [Description("ccc")] ccc,
    [Description("ddd")] ddd,
}
public class UICreateRoom : UIBase<UICreateRoom>
{
    byte roundIndex = 0;
    byte fangChongIndex = 0;
    byte shengPaiIndex = 0;
    byte daZiIndex = 0;
    byte playerIndex = 0;
    byte payIndex = 0;

    Color32 normalColor = new Color32(111,87,23,255);
    Color32 chooseColor = new Color32(152,36,10,255);
    Transform TranBtnBase;

    void Start ()
    {
        TranBtnBase = transform.Find("Base").Find("BtnBase");
        foreach (Transform item in TranBtnBase)
            UIEventListener.Get(item.gameObject).onClick = OnClick;
	}
	
    void OnClick(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        byte index = 0;
        switch (go.name)
        {
            case "btnCreate":
                ClientToServerMsg.Send(FrameworkForCSharp.NetWorks.Opcodes.Client_PlayerCreateXYQPRoom, roundIndex,fangChongIndex,shengPaiIndex,daZiIndex,playerIndex,payIndex, 
                        Input.location.lastData.latitude, Input.location.lastData.longitude);
                break;
            case "btnClose":
                UIManager.Instance.HideUIPanel(UIPaths.UIPanel_CreateRoom);
                break;
            case "btnRound0":
            case "btnRound1":
            case "btnRound2":
                index = byte.Parse(go.name.Substring(8));
                if (index == roundIndex) return;
                TranBtnBase.Find("btnRound"+index).Find("txt").GetComponent<UILabel>().color = chooseColor;
                TranBtnBase.Find("btnRound" + roundIndex).Find("txt").GetComponent<UILabel>().color = normalColor;
                TranBtnBase.Find("btnRound" + index).GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
                TranBtnBase.Find("btnRound" + roundIndex).GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
                roundIndex = index;
                break;
            case "btnFangChong0":
            case "btnFangChong1":
                index = byte.Parse(go.name.Substring(12));
                if (index == fangChongIndex) return;
                TranBtnBase.Find("btnFangChong" + index).Find("txt").GetComponent<UILabel>().color = chooseColor;
                TranBtnBase.Find("btnFangChong" + fangChongIndex).Find("txt").GetComponent<UILabel>().color = normalColor;
                TranBtnBase.Find("btnFangChong" + index).GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
                TranBtnBase.Find("btnFangChong" + fangChongIndex ).GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
                fangChongIndex = index;
                break;
            case "btnCards0":
            case "btnCards1":
                index = byte.Parse(go.name.Substring(8));
                if (index == shengPaiIndex) return;
                TranBtnBase.Find("btnCards" + index).Find("txt").GetComponent<UILabel>().color = chooseColor;
                TranBtnBase.Find("btnCards" + shengPaiIndex).Find("txt").GetComponent<UILabel>().color = normalColor;
                TranBtnBase.Find("btnCards" + index).GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
                TranBtnBase.Find("btnCards" + shengPaiIndex).GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
                shengPaiIndex = index;
                break;
            case "btnDaZi0":
            case "btnDaZi1":
            case "btnDaZi2":
            case "btnDaZi3":
                index = byte.Parse(go.name.Substring(7));
                if (index == daZiIndex) return;
                TranBtnBase.Find("btnDaZi" + index).Find("txt").GetComponent<UILabel>().color = chooseColor;
                TranBtnBase.Find("btnDaZi" + daZiIndex).Find("txt").GetComponent<UILabel>().color = normalColor;
                TranBtnBase.Find("btnDaZi" + index).GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
                TranBtnBase.Find("btnDaZi" + daZiIndex).GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
                daZiIndex = index;
                break;
            case "btnPlayer0":
            case "btnPlayer1":
            case "btnPlayer2":
                index = byte.Parse(go.name.Substring(9));
                if (index == playerIndex) return;
                TranBtnBase.Find("btnPlayer" + index).Find("txt").GetComponent<UILabel>().color = chooseColor;
                TranBtnBase.Find("btnPlayer" + playerIndex).Find("txt").GetComponent<UILabel>().color = normalColor;
                TranBtnBase.Find("btnPlayer" + index).GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
                TranBtnBase.Find("btnPlayer" + playerIndex).GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
                playerIndex = index;
                break;
            case "btnPay0":
            case "btnPay1":
                index = byte.Parse(go.name.Substring(6));
                if (index == payIndex) return;
                TranBtnBase.Find("btnPay" + index).Find("txt").GetComponent<UILabel>().color = chooseColor;
                TranBtnBase.Find("btnPay" + payIndex).Find("txt").GetComponent<UILabel>().color = normalColor;
                TranBtnBase.Find("btnPay" + index).GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
                TranBtnBase.Find("btnPay" + payIndex).GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
                payIndex = index;
                break;
        }
    }

}
