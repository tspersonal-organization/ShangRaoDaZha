using FrameworkForCSharp.NetWorks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myRoomItem : MonoBehaviour {


    public AngentRoomInfo info;//代理开房的信息
    //onPlayerDisposeRoom
    public uint RoomId;
    public uint RoomRound;
    public UIButton DisMisBtn;

    public UIButton ShareBtn;
	// Use this for initialization
	void Start () {
        DisMisBtn = transform.Find("DisMisBtnSprite").GetComponent<UIButton>();
        ShareBtn = transform.Find("ShareBtnSprite ").GetComponent<UIButton>();
        DisMisBtn.onClick.Add(new EventDelegate(this.DisPoseRoom));
        ShareBtn.onClick.Add(new EventDelegate(this.ShareRoom));
    }

    /// <summary>
    /// 分享房间
    /// </summary>
    private void ShareRoom()
    {
        switch (info.Roomtype)
        {
            case FrameworkForCSharp.Utils.RoomType.WDH:
                AuthorizeOrShare.Instance.ShareRoomID(RoomId, "一起来玩闲娱狗吧！" + RoomRound.ToString() + "局！"+info.PlayerCount+"缺"+(4- info.PlayerCount).ToString() + "!房主支付!", "闲娱狗无挡胡");
                break;
            case FrameworkForCSharp.Utils.RoomType.ZB:
                AuthorizeOrShare.Instance.ShareRoomID(RoomId, "一起来玩闲娱狗吧！" + RoomRound.ToString() + "局！" + info.PlayerCount + "缺" + (4 - info.PlayerCount).ToString() + "!房主支付!", "闲娱狗栽宝");
                break;
            case FrameworkForCSharp.Utils.RoomType.PK:
                AuthorizeOrShare.Instance.ShareRoomID(RoomId, "一起来玩闲娱狗吧！" + RoomRound.ToString() + "局！" + info.PlayerCount + "缺" + (4 - info.PlayerCount).ToString() + "!房主支付!", "闲娱狗讨赏");
                break;

        }
     //   AuthorizeOrShare.Instance.ShareRoomID(RoomId, "一起来玩闲娱狗吧！" + RoomRound.ToString() + "局" + "  房主支付!", "闲娱狗");
        //// jishu = (int)GameData.m_TableInfo.configPayIndex;
        //switch ((int)GameData.m_TableInfo.configPayIndex)
        //{
        //    // jishu = (int)GameData.m_TableInfo.configPayIndex;
        //    case 0:
        //        // jishu =1111;
              
        //        // RuleLable.text = "局数：" + GameData.m_TableInfo.curGameCount.ToString() + "/" + GameData.m_TableInfo.configRoundIndex.ToString() + "  房主支付";
        //        //  jishu = 11112222;
        //        break;
        //    case 1:
        //        //  jishu = 2222;
        //        AuthorizeOrShare.Instance.ShareRoomID(RoomId, "一起来玩闲娱狗吧！" + ((int)GameData.m_TableInfo.configRoundIndex).ToString() + "局" + "  平摊支付!", "闲娱狗");
        //        // RuleLable.text = "局数：" + GameData.m_TableInfo.curGameCount.ToString() + "/" + GameData.m_TableInfo.configRoundIndex.ToString() + "  平均支付";
        //        // jishu = 22223333;
        //        break;
        //}

    }

    private void DisPoseRoom()
    {
        ClientToServerMsg.Send(Opcodes.Client_PlayerDisposeRoom, RoomId);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
