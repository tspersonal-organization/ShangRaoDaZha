using FrameworkForCSharp.NetWorks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKClubRoomItemControl : MonoBehaviour {

    public UIButton RoomItemBtn;
    public List<UITexture> PlayreHeadList;
    public UILabel RoomidLable;
    public UILabel RoundCountLable;
    public UISprite OperateBtn;

    PKClubRoomInfo InfoData;
    // Use this for initialization
    void Start () {
        RoomItemBtn.onClick.Add(new EventDelegate(this.ItemClick));

    }

    private bool IsPlaying = false;
    private void ItemClick()
    {
        if (!IsPlaying)
        {
            ClientToServerMsg.Send(Opcodes.Client_PlayerEnterRoom, InfoData.codeId, Input.location.lastData.latitude, Input.location.lastData.longitude);
        }
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetValue(PKClubRoomInfo info)
    {
        InfoData = info;
        RoomidLable.text = info.codeId.ToString();
        RoundCountLable.text = info.gameCount.ToString() + "局";
        for (int i = 0; i < info.PKClubPlayerInfoList.Count; i++)
        {
            PlayreHeadList[i].gameObject.SetActive(true);
            DownloadImage.Instance.Download(PlayreHeadList[i], info.PKClubPlayerInfoList[i].HeadId);
        }
        if (info.playerCount == info.PKClubPlayerInfoList.Count)
        {
            IsPlaying = true;
            //游戏正在进行中
        }
       
    }
}
