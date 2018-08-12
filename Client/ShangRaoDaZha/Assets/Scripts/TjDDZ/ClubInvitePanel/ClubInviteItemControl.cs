/*
* 开发人：滕俊
* 项目：
* 功能描述：
*
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubInviteItemControl : MonoBehaviour {

    public UIButton RefuseBtn;
    public UIButton AgreeBtn;
    public UILabel DescLable;
    private ClubInfo DataInfo;
	// Use this for initialization
	void Start () {
        RefuseBtn.onClick.Add(new EventDelegate(this.RefuseInvite));
        AgreeBtn.onClick.Add(new EventDelegate(this.AgreeBtnInvite));
    }

    private void AgreeBtnInvite()
    {
        ClientToServerMsg.OperateInviteMessage((uint)DataInfo.Id,true);
    }

    private void RefuseInvite()
    {
        ClientToServerMsg.OperateInviteMessage((uint)DataInfo.Id, false);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetValue(ClubInfo info)
    {
        this.DataInfo = info;
        DescLable.text = "俱乐部:" + DataInfo.ClubName + " 邀请你加入！";
    }
}
