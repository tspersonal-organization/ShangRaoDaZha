using FrameworkForCSharp.NetWorks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskDismissRoom : UIBase<AskDismissRoom>
{
    public GameObject[] ItemArray;

    public GameObject[] ItemArrayTwo;
    public GameObject btnAgree;
    public GameObject btnJuJue;
    public UILabel LBCountDown;
	// Use this for initialization
	void Start ()
    {
        UIEventListener.Get(btnAgree).onClick = OnClick;
        UIEventListener.Get(btnJuJue).onClick = OnClick;
        Init();
        if (IsInvoking("CountDonw")) CancelInvoke("CountDonw");
        InvokeRepeating("CountDonw",0,1);
         ItemArray[0].transform.parent.GetComponent<UIGrid>().enabled = true;
       // ItemArray[0].transform.parent.GetComponent<UIGrid>().

    }

    public void Init()
    {
        if (GameData.GlobleRoomType!=FrameworkForCSharp.Utils.RoomType.NN)//麻将 四人局及以下
        {
            ItemArray[0].transform.parent.gameObject.SetActive(true);
            ItemArrayTwo[0].transform.parent.gameObject.SetActive(false);
            for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
            {
                PlayerInfo info = GameData.m_PlayerInfoList[i];
                GameObject obj = ItemArray[info.pos - 1];
                obj.SetActive(true);
                obj.transform.Find("name").GetComponent<UILabel>().text = info.name;
                obj.transform.Find("state").gameObject.SetActive(IsAgreeList(info.pos));
                DownloadImage.Instance.Download(obj.transform.Find("headImage").GetComponent<UITexture>(), info.headID);
            }
            if (IsAgreeList(GameDataFunc.GetPlayerInfo(Player.Instance.guid).pos)) HideBtn();
        }
        else 
        {
            for (int i = 0; i < ItemArrayTwo.Length; i++) 
            {
                ItemArrayTwo[i].gameObject.SetActive(false);
            }
            ItemArray[0].transform.parent.gameObject.SetActive(false);
            ItemArrayTwo[0].transform.parent.gameObject.SetActive(true);
            for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
            {
                PlayerInfo info = GameData.m_PlayerInfoList[i];
                GameObject obj = ItemArrayTwo[info.pos - 1];
                obj.SetActive(true);
                obj.transform.Find("name").GetComponent<UILabel>().text = info.name;
                obj.transform.Find("state").gameObject.SetActive(IsAgreeList(info.pos));
                DownloadImage.Instance.Download(obj.transform.Find("headImage").GetComponent<UITexture>(), info.headID);
            }
            if (IsAgreeList(GameDataFunc.GetPlayerInfo(Player.Instance.guid).pos)) HideBtn();
        }
      
    }

    bool IsAgreeList(byte pos)
    {
        for (int i = 0; i < GameData.m_TableInfo.operateLeaveRoomList.Count; i++)
        {
            if (pos == GameData.m_TableInfo.operateLeaveRoomList[i]) return true;
        }
        return false;
    }

    void CountDonw()
    {
        if(GameData.m_TableInfo.queryLeaveRoomWaitTime > 0)
        {
            GameData.m_TableInfo.queryLeaveRoomWaitTime--;
            LBCountDown.text = GameData.m_TableInfo.queryLeaveRoomWaitTime + "秒后解散";
            if(GameData.m_TableInfo.queryLeaveRoomWaitTime == 0)
            {
                CancelInvoke("CountDonw");
            }
        }
        else
        {
            CancelInvoke("CountDonw");
        }
    }
	
    void OnClick(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        if (go == btnAgree)
        {
            ClientToServerMsg.Send(Opcodes.Client_PlayerDealQueryLeaveResult, GameData.m_TableInfo.id,true);
        }
        else if(go == btnJuJue)
        {
            ClientToServerMsg.Send(Opcodes.Client_PlayerDealQueryLeaveResult, GameData.m_TableInfo.id, false);
        }
    }

    public void HideBtn()
    {
        btnAgree.SetActive(false);
        btnJuJue.SetActive(false);
    }
   
}
