using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKInvitePlayerJoinClub : MonoBehaviour {

    public UIButton SeruBtn;
    public UIButton MaskBtn;
    public UIInput input;
    // Use this for initialization
    void Start () {
        MaskBtn.onClick.Add(new EventDelegate(()=>
        {
            this.gameObject.SetActive(false);
        }));
        SeruBtn.onClick.Add(new EventDelegate(this.InvitePlayerJoinClub));
    }

    /// <summary>
    /// 邀请玩家加入俱乐部
    /// </summary>
    private void InvitePlayerJoinClub()
    {
        this.gameObject.SetActive(false);
        ClientToServerMsg.InvitePlayerJoinClub(ulong.Parse(input.value),(uint)GameData.CurrentClubInfo.Id);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
