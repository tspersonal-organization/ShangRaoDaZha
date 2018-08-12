﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKClubItemControl : MonoBehaviour {

   
    public UIButton ClubItem;
    public UIButton ChoseBtn;
    public UILabel ClubNameLable;
    public UILabel ClubIdLable;
    public UILabel ClubRoomCountLable;

    private PkClubInfo InfoData;


	// Use this for initialization
	void Start () {
        ChoseBtn.onClick.Add(new EventDelegate(this.ChoseBtnClick));
        ClubItem.onClick.Add(new EventDelegate(this.ClubItemClick));
    }

    /// <summary>
    /// 俱乐部的点击
    /// </summary>
    private void ClubItemClick()
    {
        GameData.CurrentClickClubInfo = InfoData;
        ClientToServerMsg.ApplyClubRoomList(InfoData.ClubId);
      //  MainPanel.Instance.CreatClubRoomList();

    }

    /// <summary>
    /// 是否选中此俱乐部
    /// </summary>
    bool Chosed = false;
    private void ChoseBtnClick()
    {
        MainPanel.Instance.ClearClubChose();//清空选中的其他俱乐部
        Chosed = !Chosed;
        if (Chosed)
        {
            MainPanel.Instance.ChosedClubId = InfoData.ClubId;
            ChoseBtn.transform.parent.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
        }
        else
        {
            MainPanel.Instance.ChosedClubId = 0;
          
            ChoseBtn.transform.parent.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetValue(PkClubInfo info)
    {
        InfoData = info;
        ClubNameLable.text = info.ClubName;
        ClubIdLable.text = "ID:"+info.ClubId.ToString();
        ClubRoomCountLable.text = "已开" + info.RoomCount + "桌";
    }


}
