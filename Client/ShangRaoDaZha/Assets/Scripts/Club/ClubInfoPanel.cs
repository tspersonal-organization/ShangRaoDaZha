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

public class ClubInfoPanel : UIBase<ClubInfoPanel>
{
    public GameObject MasterObj;
    public GameObject GuanLiYuanObj;

    public UILabel ClubId;
    public UILabel GameCount;
   

    public UIButton InviteBtn;
    public UIButton QuiteBtn;
    public UIButton CloseBtn;

 
    void OnEnable()
    {
        InitData();
    }


    public void InitData()
    {
        InviteBtn.gameObject.SetActive(false);
        QuiteBtn.gameObject.SetActive(true);

        if (GameData.CurrentClubInfo.CreatorGUID == Player.Instance.guid)
        {
             InviteBtn.gameObject.SetActive(true);
            QuiteBtn.gameObject.SetActive(false);
        }


        for (int i = 0; i < GameData.CurrentClubInfo.MemMasterList.Count; i++)
        {
            if (GameData.CurrentClubInfo.MemMasterList[i].guid == Player.Instance.guid)
            {
                InviteBtn.gameObject.SetActive(true);
                QuiteBtn.gameObject.SetActive(false);
            }
        }
         ClubId.text=GameData.CurrentClubInfo.Id.ToString();
    GameCount.text=0.ToString();
    DownloadImage.Instance.Download(MasterObj.transform.Find("MasterTexture").GetComponent<UITexture>(),GameData.CurrentClubInfo.CreatorName);
        MasterObj.transform.Find("MasterNane").GetComponent<UILabel>().text = GameData.CurrentClubInfo.CreatorName;
        for (int i = 0; i < 2; i++)
        {
            GuanLiYuanObj.transform.Find("MasterTexture" + i).gameObject.SetActive(false);
        }

        for (int i = 0; i < GameData.CurrentClubInfo.MemMasterList.Count; i++)
        {
            GuanLiYuanObj.transform.Find("MasterTexture" + i).gameObject.SetActive(true);
            DownloadImage.Instance.Download(GuanLiYuanObj.transform.Find("MasterTexture" + i).GetComponent<UITexture>(), GameData.CurrentClubInfo.MemMasterList[i].headid);
            GuanLiYuanObj.transform.Find("MasterTexture" + i+ "/MasterNane").GetComponent<UILabel>().text = GameData.CurrentClubInfo.MemMasterList[i].name;
        }
        CreatMem();
    }

    public GameObject ClubMemItem;
    public Transform ClubMemItemParent;

    List<GameObject> CreatRoomItem = new List<GameObject>();//生成的房间配置
    private void CreatMem()
    {
        int count = CreatRoomItem.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(CreatRoomItem[i]);
        }
        CreatRoomItem = new List<GameObject>();
        for (int i = 0; i < GameData.CurrentClubInfo.MemList.Count; i++)
        {
            GameObject item = Instantiate(ClubMemItem, ClubMemItemParent);
            item.SetActive(true);
            item.transform.GetComponent<ClubMemInfoControl>().SetData(GameData.CurrentClubInfo.MemList[i]);
            item.transform.localPosition = new Vector3(-310+100*(i%7),-85-90*(i/7),0);
            CreatRoomItem.Add(item);
        }
    }
	// Use this for initialization
	void Start () {
        CloseBtn.onClick.Add(new EventDelegate(this.CloseBtnClick));
        InviteBtn.onClick.Add(new EventDelegate(this.ClubMastorBtnClick));
        QuiteBtn.onClick.Add(new EventDelegate(this.QuiteBtnClick));
	}

    //我要退出点击
    private void QuiteBtnClick()
    {
        this.gameObject.SetActive(false);
        ClientToServerMsg.QuiteClub((uint)GameData.CurrentClubInfo.Id,Player.Instance.guid);
    }
   //邀请玩家店家
    private void ClubMastorBtnClick()
    {
        UIManager.Instance.ShowUiPanel(UIPaths.InviteJoinClubPanel, OpenPanelType.MinToMax);
    }

    //关闭点击
    private void CloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
