﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelUserInfo : UIBase<PanelUserInfo>
{

    public UITexture Headtexture;
    public UILabel Name;
    public UILabel Id;
    public UILabel Ip;
    public UILabel Address;

    public UIButton CancleMasterBtn;
    public UIButton SetMasterBtn;
    public UIButton TickOutBtn;
    public UIButton CloseBtn;

    private void OnEnable()
    {
        CancleMasterBtn.gameObject.SetActive(false);
        SetMasterBtn.gameObject.SetActive(false);
        TickOutBtn.gameObject.SetActive(false);

        DownloadImage.Instance.Download(Headtexture, Player.Instance.headID);
        Name.text = Player.Instance.otherName;
        Id.text = Player.Instance.openID;
        Ip.text = "无";
        Address.text = "无";
    }
    // Use this for initialization
    void Start()
    {
        CancleMasterBtn.onClick.Add(new EventDelegate(CancleMasterBtnClick));
        SetMasterBtn.onClick.Add(new EventDelegate(SetMasterBtnClick));
        TickOutBtn.onClick.Add(new EventDelegate(TickOutBtnClick));
        CloseBtn.onClick.Add(new EventDelegate(() =>
        {
            this.gameObject.SetActive(false);
        }));
    }

    private void CancleMasterBtnClick()
    {
        this.gameObject.SetActive(false);
        ClientToServerMsg.SetMasterOper((uint)GameData.CurrentClubInfo.Id, GameData.ChoseMem.guid, false);
    }

    private void SetMasterBtnClick()
    {
        this.gameObject.SetActive(false);
        ClientToServerMsg.SetMasterOper((uint)GameData.CurrentClubInfo.Id, GameData.ChoseMem.guid, true);
    }

    private void TickOutBtnClick()
    {
        this.gameObject.SetActive(false);
        ClientToServerMsg.RemovePlayerFromClub((uint)GameData.CurrentClubInfo.Id, GameData.ChoseMem.guid);
    }
}
