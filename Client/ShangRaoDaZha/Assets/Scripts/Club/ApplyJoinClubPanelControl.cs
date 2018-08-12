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

public class ApplyJoinClubPanelControl : MonoBehaviour {

    public static ApplyJoinClubPanelControl Instance;
    public UIButton JoinClubBtn;
    public UIInput ClubIdInput;//

    public UIButton MaskBtn;

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
        JoinClubBtn.onClick.Add(new EventDelegate(this.ApplyJoinClub));
        MaskBtn.onClick.Add(new EventDelegate(()=>
        {
            this.gameObject.SetActive(false);
        }));
    }

    /// <summary>
    /// 申请加入俱乐部
    /// </summary>
    private void ApplyJoinClub()
    {
        if (string.IsNullOrEmpty(ClubIdInput.value))
            return;
        uint clubid;
        if (uint.TryParse(ClubIdInput.value,out clubid))
        {
            ClientToServerMsg.ApplyJoinClub(clubid);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
