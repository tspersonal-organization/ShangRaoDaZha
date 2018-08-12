using FrameworkForCSharp.NetWorks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvitePanel : UIBase<InvitePanel>
{

    public UIInput InputFiled;//输入框
    public UIButton SureBtn;//确认按钮
    public UIButton CloseBtn;
	// Use this for initialization
	void Start () {
        CloseBtn.onClick.Add(new EventDelegate(this.ClosePanel));
        SureBtn.onClick.Add(new EventDelegate(this.BindFriend));

    }

    private void ClosePanel()
    {
        UIManager.Instance.HideUiPanel(UIPaths.InvitePanel);
    }

    /// <summary>
    /// 绑定邀请人
    /// </summary>
    private void BindFriend()
    {
        if (InputFiled.value != "")
        {
            ulong inviteGuid = ulong.Parse(InputFiled.value);
            ClientToServerMsg.Send(Opcodes.Client_SetInviteGuid, inviteGuid);
        }
      

    }

    // Update is called once per frame
    void Update () {
		
	}
}
