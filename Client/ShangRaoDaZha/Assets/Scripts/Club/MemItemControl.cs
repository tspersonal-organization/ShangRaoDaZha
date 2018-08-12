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

public class MemItemControl : MonoBehaviour {


    private MemInfo MeminfoData;//需要的数据
    public UITexture HeadTex;
    public UILabel NameLable;
    public UILabel IdLable;

    public UIButton AgreeBtn;
    public UIButton RefuseBtn;
    public UIButton RemoveBtn;
	// Use this for initialization
	void Start () {
        AgreeBtn.onClick.Add(new EventDelegate(this.AgreeBtnClick));
        RefuseBtn.onClick.Add(new EventDelegate(this.RefuseBtnClick));
        RemoveBtn.onClick.Add(new EventDelegate(this.RemoveBtnClick));

    }

    private void RemoveBtnClick()
    {
        ClientToServerMsg.RemovePlayerFromClub((uint)GameData.CurrentClubInfo.Id, MeminfoData.guid);
    }

    private void RefuseBtnClick()
    {
        ClientToServerMsg.OperatePlayerApply((uint)GameData.CurrentClubInfo.Id, MeminfoData.guid,false);
    }

    private void AgreeBtnClick()
    {
        ClientToServerMsg.OperatePlayerApply((uint)GameData.CurrentClubInfo.Id, MeminfoData.guid, true);
    }

    // Update is called once per frame
    void Update () {
		
	}

    /// <summary>
    /// 设置信息
    /// </summary>
    /// <param name="Apply">是否为申请列表</param>
    public void SetValue(bool Apply, MemInfo data)
    {
        if (Apply)
        {
            AgreeBtn.gameObject.SetActive(true);
            RefuseBtn.gameObject.SetActive(true);
            RemoveBtn.gameObject.SetActive(false);
        }
        else
        {
            AgreeBtn.gameObject.SetActive(false);
            RefuseBtn.gameObject.SetActive(false);
            RemoveBtn.gameObject.SetActive(true);
        }
        this.MeminfoData = data;
        InitData();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitData()
    {

       
        // HeadTex;
        NameLable.text="名称:"+ MeminfoData.name.ToString();
        IdLable.text="ID:"+ MeminfoData.guid.ToString();
        try
        {
            DownloadImage.Instance.Download(HeadTex, MeminfoData.headid);
        }
        catch
        {
            Debug.LogError("下载头像出错");
        }
      
    }
}
