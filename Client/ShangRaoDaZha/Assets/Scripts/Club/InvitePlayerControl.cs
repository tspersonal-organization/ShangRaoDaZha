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

public class InvitePlayerControl : UIBase<InvitePlayerControl>
{

    public UIInput playerId;
    public UIButton SureBtn;
    public UIButton CloseBtn;//关闭MASK
	void Start () {
        CloseBtn.onClick.Add(new EventDelegate(this.CloseBtnClick));
        SureBtn.onClick.Add(new EventDelegate(this.InviteFriend));
    }

    /// <summary>
    /// 邀请玩家
    /// </summary>
    private void InviteFriend()
    {
        if (playerId.value != "")
        {
            ClientToServerMsg.InvitePlayerJoinClub(ulong.Parse(playerId.value),(uint)GameData.CurrentClubInfo.Id, GameData.CurrentClubInfo.ClubName);
        }
        else
        {
            GameData.ResultCodeStr = "请输入正确的玩家id";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
