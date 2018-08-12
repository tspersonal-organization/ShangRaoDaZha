using FrameworkForCSharp.NetWorks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FrameworkForCSharp.Utils;

public class ChatFace : UIBase<ChatFace>
{
    public GameObject btnClose;
    public UIButton SendContentBtn;
    public UIInput InputChat;
    // public UIButton btnTxt;
    public GameObject FacePanel;
    public GameObject TxtPanel;
    public GameObject MJTxtPanel;
    public Transform btnBaseFace;
    public Transform btnBaseTxt;

    public Transform btnMJBaseTxt;



    public UIButton FaceBtn;
    public UIButton TextBtn;
    // Use this for initialization
    void Start()
    {
        TxtPanel.gameObject.SetActive(false);
        MJTxtPanel.gameObject.SetActive(false);
        InputChat.onSubmit.Add(new EventDelegate(SendInputChat));
        UIEventListener.Get(btnClose).onClick = OnClick;
        UIEventListener.Get(SendContentBtn.gameObject).onClick = OnClick;
        // UIEventListener.Get(btnTxt.gameObject).onClick = OnClick;
        foreach (Transform item in btnBaseFace)
            UIEventListener.Get(item.gameObject).onClick = OnClick;
        foreach (Transform item in btnBaseTxt)
            UIEventListener.Get(item.gameObject).onClick = OnClick;
        foreach (Transform item in btnMJBaseTxt)
            UIEventListener.Get(item.gameObject).onClick = OnClick;

        //if (GameData.GlobleRoomType == RoomType.XYGQP)
        //{
        //    TxtPanel.gameObject.SetActive(true);
        //}
        //else
        //{
        //    MJTxtPanel.gameObject.SetActive(true);
        //}


        FaceBtn.onClick.Add(new EventDelegate(this.FaceBtnClick));
        TextBtn.onClick.Add(new EventDelegate(this.TextBtnClick));
        //btnTxt.isEnabled = false;
        //  FacePanel.SetActive(true);
    }

    private void OnEnable()
    {
        FaceBtnClick();
    }
    /// <summary>
    /// 快捷语点击
    /// </summary>
    private void TextBtnClick()
    {
        if (TextBtn.transform.Find("Sprite").gameObject.activeSelf) return;
        TextBtn.transform.Find("Sprite").gameObject.SetActive(true);
        FaceBtn.transform.Find("Sprite").gameObject.SetActive(false);
        if (GameData.GlobleRoomType == RoomType.PK|| GameData.GlobleRoomType == RoomType.NN)
        {
            TxtPanel.gameObject.SetActive(true);
        }
        else
        {
            MJTxtPanel.gameObject.SetActive(true);
        }
        FacePanel.SetActive(false);

    }

    private void FaceBtnClick()
    {
        if (FaceBtn.transform.Find("Sprite").gameObject.activeSelf) return;
        TextBtn.transform.Find("Sprite").gameObject.SetActive(false);
        FaceBtn.transform.Find("Sprite").gameObject.SetActive(true);
        TxtPanel.gameObject.SetActive(false);
        MJTxtPanel.gameObject.SetActive(false);
        FacePanel.SetActive(true);
    }

    private void SendInputChat()
    {
        string fileName = "";
        if (InputChat.value == "" || InputChat.value == "请输入聊天内容。。。")
        { }
        else
        {
            fileName = "2@" + Player.Instance.guid + "@" + InputChat.value;
            ClientToServerMsg.Send(Opcodes.Client_PlayerSpeak, GameData.m_TableInfo.id, fileName);
            UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
        }
    }

    void OnClick(GameObject go)
    {
        string fileName = "";
        switch (go.name)
        {
            case "btnClose":
                UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
                break;
            case "SendSprite":
                if (InputChat.value == "" || InputChat.value == "请输入聊天内容。。。")
                { }
                else
                {
                    fileName = "2@" + Player.Instance.guid + "@" + InputChat.value;
                    ClientToServerMsg.Send(Opcodes.Client_PlayerSpeak, GameData.m_TableInfo.id, fileName);
                    UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
                }

                break;
            case "btnTxt":
                //btnFace.isEnabled = true;
                //btnTxt.isEnabled = false;
                //FacePanel.SetActive(false);
                //TxtPanel.SetActive(true);
                break;
            case "face1001":
            case "face1002":
            case "face1003":
            case "face1004":
            case "face1005":
            case "face1006":
            case "face1007":
                string faceID = go.name.Substring(4);
                fileName = "3@" + Player.Instance.guid + "@" + faceID;
                ClientToServerMsg.Send(Opcodes.Client_PlayerSpeak, GameData.m_TableInfo.id, fileName);
                UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
                break;
            //case "txt0":
            //case "txt1":
            //case "txt2":
            //case "txt3":
            //case "txt4":
            //case "txt5":
            //case "txt6":
            //case "txt7":
            //    string txtIndex = go.name.Substring(3);
            //    fileName = "5@" + Player.Instance.guid + "@" + txtIndex;
            //    ClientToServerMsg.Send(Opcodes.Client_PlayerSpeak, GameData.m_TableInfo.id, fileName);
            //    UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
            //    break;
            case "ItemSprite0":
            case "ItemSprite1":
            case "ItemSprite2":
            case "ItemSprite3":
            case "ItemSprite4":
            case "ItemSprite5":
            case "ItemSprite6":
            case "ItemSprite7":
                string txtIndex = go.name.Substring(10);
                fileName = "5@" + Player.Instance.guid + "@" + txtIndex;
                ClientToServerMsg.Send(Opcodes.Client_PlayerSpeak, GameData.m_TableInfo.id, fileName);
                UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
                break;
            case "MItemSprite0":
            case "MItemSprite1":
            case "MItemSprite2":
            case "MItemSprite3":
            case "MItemSprite4":
            case "MItemSprite5":
            case "MItemSprite6":
            case "MItemSprite7":
            case "MItemSprite8":
                string txtIndex1 = go.name.Substring(11);
                fileName = "6@" + Player.Instance.guid + "@" + txtIndex1;
                ClientToServerMsg.Send(Opcodes.Client_PlayerSpeak, GameData.m_TableInfo.id, fileName);
                UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
                break;
        }
    }

}
