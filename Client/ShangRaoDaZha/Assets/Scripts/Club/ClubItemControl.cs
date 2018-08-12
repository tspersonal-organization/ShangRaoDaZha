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

public class ClubItemControl : MonoBehaviour {

    public UILabel ClubNameLable;
    public UILabel CreatorLable;
    public UILabel PlayerCountLable;
    public UILabel IdLable;
    public UIButton LookBtn;


    private ClubInfo DataInfo;//数据
    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetValue(ClubInfo info)
    {
        this.DataInfo = info;
        ClubNameLable.text = info.ClubName;
        CreatorLable.text = "管理员:"+info.CreatorName;
        PlayerCountLable.text = "人数:" + info.PlayerCount;
        IdLable.text = "ID:" + info.Id;


    }

    void Awake()
    {

    }
	// Use this for initialization
	void Start () {
        LookBtn.onClick.Add(new EventDelegate(this.GetClubInfo));

    }

   
    /// <summary>
    /// 获取俱乐部信息
    /// </summary>
    private void GetClubInfo()
    {
      
        ClientToServerMsg.GetClubInfo((uint)DataInfo.Id);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
