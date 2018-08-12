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

public class ClubRoomItemControl : MonoBehaviour {

    public bool IsConfigRoom = false;//是否为配置房间

    private RoomConfig configData;//配置信息
    private ActiveRoomInfo RoomInfo;//激活的房间信息

    public UILabel ConfigTypeLable;//配置种类
    public UILabel RoomIdLanle;//房间号
    public UILabel RoomDecLanle;//房间配置描述

    public GameObject headTexture;
    public Transform HeadParent;
    /// <summary>
    /// 设置配置信息
    /// </summary>
    public void SetConfigValu(RoomConfig data )
    {
        this.configData = data;
        IsConfigRoom = true;
        RoomIdLanle.gameObject.SetActive(false);
        transform.GetComponent<UISprite>().spriteName = "BG_MyRoom_waiting";
        switch (data.roomType)
        {
            case FrameworkForCSharp.Utils.RoomType.PK:
                ConfigTypeLable.text = "讨赏";
                break;
            case FrameworkForCSharp.Utils.RoomType.WDH:
                ConfigTypeLable.text = "无挡胡";
                break;
            case FrameworkForCSharp.Utils.RoomType.ZB:
                ConfigTypeLable.text = "栽宝";
                break;
            case FrameworkForCSharp.Utils.RoomType.NN:
                ConfigTypeLable.text = "牛牛";
                break;
            case FrameworkForCSharp.Utils.RoomType.Other:
                break;
            default:
                break;
        }

        RoomDecLanle.text = "总局数:" + data.GameCount;

        this.gameObject.SetActive(true);


    }

   /// <summary>
   /// 设置激活房间信息
   /// </summary>
    public void SetRoomValue(ActiveRoomInfo info)
    {
        this.RoomInfo = info;
        IsConfigRoom = false;
        transform.GetComponent<UISprite>().spriteName = "BG_MyRoom_playing";
        //   RoomIdLanle.gameObject.SetActive(false);
        RoomIdLanle.text ="房间号:"+ info.RoomId;
        switch (info.roomtype)
        {
            case FrameworkForCSharp.Utils.RoomType.PK:
                ConfigTypeLable.text = "讨赏";
                break;
            case FrameworkForCSharp.Utils.RoomType.WDH:
                ConfigTypeLable.text = "无挡胡";
                break;
            case FrameworkForCSharp.Utils.RoomType.ZB:
                ConfigTypeLable.text = "载宝";
                break;
            case FrameworkForCSharp.Utils.RoomType.NN:
                ConfigTypeLable.text = "牛牛";
                break;
            case FrameworkForCSharp.Utils.RoomType.Other:
                break;
            default:
                break;
        }
      
        RoomDecLanle.text = "总局数:" + info.RoomCount;
        CreatPlayerList();//生成玩家头像
        this.gameObject.SetActive(true);
    }


    List<GameObject> PlayerList = new List<GameObject>();
    /// <summary>
    /// 创建玩家
    /// </summary>
    private void CreatPlayerList()
    {
        int count = PlayerList.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(PlayerList[i]);
        }
        PlayerList = new List<GameObject>();
        for (int i = 0; i < RoomInfo.PlayerCountHeadList.Count; i++)
        {
            GameObject g = Instantiate(headTexture, HeadParent);
            g.transform.localScale = new Vector3(0.6f,0.6f,0);
            g.transform.localPosition = new Vector3(-113+64* PlayerList.Count,-47,0);
            PlayerList.Add(g);
            DownloadImage.Instance.Download(g.transform.GetComponent<UITexture>(), RoomInfo.PlayerCountHeadList[i]);
            g.SetActive(true);
        }
       
    }

	// Use this for initialization
	void Start () {
        UIEventListener.Get(this.gameObject).onClick = this.EnterRoom;
	}

    /// <summary>
    ///进入房间
    /// </summary>
    /// <param name="go"></param>
    private void EnterRoom(GameObject go)
    {
        if (IsConfigRoom)
        {
            uint chipIndex = 0;
            if (configData.roomType == FrameworkForCSharp.Utils.RoomType.NN)
            {
                switch (configData.NiuNiuChips[0])
                {
                    case 1:
                        chipIndex = 0;
                        break;
                    case 2:
                        chipIndex = 1;
                        break;
                    case 5:
                        chipIndex = 2;
                        break;
                }
            }
           
          
            ClientToServerMsg.ClubEnterRoom((uint)GameData.CurrentClubInfo.Id,false,0,0,0, configData.clubRoomType,chipIndex,configData.YongPaiType, configData.IsJiangMa, configData.ShunZhiNiu, configData.ZhaDanNiu, configData.wuXiaoNiu, configData.WuHuaNiu, configData.XianJiaMaiMa);
        }
        else
        {
            uint chipIndex = 0;
            if (RoomInfo.roomtype == FrameworkForCSharp.Utils.RoomType.NN)
            {
                switch (RoomInfo.NiuNiuChips[0])
                {
                    case 1:
                        chipIndex = 0;
                        break;
                    case 2:
                        chipIndex = 1;
                        break;
                    case 5:
                        chipIndex = 2;
                        break;
                }
            }
           
            ClientToServerMsg.ClubEnterRoom((uint)GameData.CurrentClubInfo.Id, true, 0, 0, RoomInfo.RoomId, RoomInfo.clubRoomType, chipIndex, RoomInfo.YongPaiType, RoomInfo.IsJiangMa, RoomInfo.ShunZhiNiu, RoomInfo.ZhaDanNiu, RoomInfo.wuXiaoNiu, RoomInfo.WuHuaNiu, RoomInfo.XianJiaMaiMa);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
