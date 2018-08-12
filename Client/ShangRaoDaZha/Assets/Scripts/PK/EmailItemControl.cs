using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailItemControl : MonoBehaviour {


    public UITexture HeadTexture;
    public UILabel DescLable;
    public UIButton AgreeBtn;
    public UIButton RefuseBtn;

    private MemInfo infoData;
    private bool Invite = false;//是否为邀请信息
    // Use this for initialization
    void Start () {
        AgreeBtn.onClick.Add(new EventDelegate(this.AgreeBtnClick));
        RefuseBtn.onClick.Add(new EventDelegate(this.RefuseBtnClick));

    }

    
    private void RefuseBtnClick()
    {
        if (Invite)//是邀请信息
        {
            ClientToServerMsg.OperateInviteMessage(infoData.ClubId,false);
        }
        else
        {
            ClientToServerMsg.OperatePlayerApply(infoData.ClubId, infoData.guid,false);
        }
    }

    private void AgreeBtnClick()
    {
        if (Invite)//是邀请信息
        {
            ClientToServerMsg.OperateInviteMessage(infoData.ClubId, true);
        }
        else
        {
            ClientToServerMsg.OperatePlayerApply(infoData.ClubId, infoData.guid, true);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="IsInvite">是否为邀请信息</param>
    public void SetValue(MemInfo info,bool IsInvite)
    {
        Invite = IsInvite;
        this.infoData = info;
        DownloadImage.Instance.Download(HeadTexture, info.headid);
        if (Invite)//是邀请信息
        {
            DescLable.text = info.name + " 邀请你加入 " + info.ClubName;
        }
        else
        {
            DescLable.text = info.name + " 申请加入俱乐部 " ;
        }

    }
}
