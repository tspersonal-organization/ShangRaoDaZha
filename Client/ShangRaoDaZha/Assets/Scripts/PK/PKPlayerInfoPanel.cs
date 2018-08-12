using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKPlayerInfoPanel : UIBase<PKPlayerInfoPanel>
{

    public UITexture Headtexture;
    public UILabel name;
    public UILabel ID;
    public UILabel IP;
    public UILabel Address;

    public UIButton CancleMasterBtn;
    public UIButton SetMasterBtn;
    public UIButton TickOutBtn;
    public UIButton CloseBtn;


    private void OnEnable()
    {
        CancleMasterBtn.gameObject.SetActive(false);
        SetMasterBtn.gameObject.SetActive(false);
        TickOutBtn.gameObject.SetActive(false);

        if (GameData.CurrentClubInfo.CreatorGUID == Player.Instance.guid)
        {
            // InviteBtn.gameObject.SetActive(true);
            CancleMasterBtn.gameObject.SetActive(true);
            SetMasterBtn.gameObject.SetActive(true);
            TickOutBtn.gameObject.SetActive(true);
        }


        for (int i = 0; i < GameData.CurrentClubInfo.MemMasterList.Count; i++)
        {
            if (GameData.CurrentClubInfo.MemMasterList[i].guid == Player.Instance.guid)
            {
                TickOutBtn.gameObject.SetActive(true);
            }
        }

        DownloadImage.Instance.Download(Headtexture,GameData.ChoseMem.headid);
        name.text = GameData.ChoseMem.name;
        ID.text = GameData.ChoseMem.guid.ToString();
        IP.text = GameData.ChoseMem.IP.ToString();
        Address.text = GameData.ChoseMem.Adress;
    }
    // Use this for initialization
    void Start () {
        CancleMasterBtn.onClick.Add(new EventDelegate(CancleMasterBtnClick));
        SetMasterBtn.onClick.Add(new EventDelegate(SetMasterBtnClick));
        TickOutBtn.onClick.Add(new EventDelegate(TickOutBtnClick));
        CloseBtn.onClick.Add(new EventDelegate(()=>
        {
            this.gameObject.SetActive(false);
        }));
    }

    private void CancleMasterBtnClick()
    {
        this.gameObject.SetActive(false);
       ClientToServerMsg.SetMasterOper((uint)GameData.CurrentClubInfo.Id, GameData.ChoseMem.guid,false);
    }

    private void SetMasterBtnClick()
    {
        this.gameObject.SetActive(false);
        ClientToServerMsg.SetMasterOper((uint)GameData.CurrentClubInfo.Id, GameData.ChoseMem.guid, true);
    }

    private void TickOutBtnClick()
    {
        this.gameObject.SetActive(false);
        ClientToServerMsg.RemovePlayerFromClub((uint)GameData.CurrentClubInfo.Id, GameData.ChoseMem.guid);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
