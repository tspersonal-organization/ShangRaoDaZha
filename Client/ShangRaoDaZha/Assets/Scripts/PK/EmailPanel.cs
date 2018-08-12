using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailPanel : UIBase<EmailPanel> {

    public GameObject EmailItem;
    public Transform EmailItemParent;

    public UIButton CloseBtn;
    private void OnEnable()
    {
        int count = EmailItemList.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(EmailItemList[i]);
        }
        ClientToServerMsg.GetClubMemListInfo((uint)GameData.CurrentClubInfo.Id);
      //  Reset();
    }

    private List<GameObject> EmailItemList = new List<GameObject>();
    public void CreatData()
    {
        int count = EmailItemList.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(EmailItemList[i]);
        }
        EmailItemList = new List<GameObject>();
        for (int i = 0; i < GameData.CurrentClubInfo.ApplyMemList.Count; i++)
        {
            GameObject item = Instantiate(EmailItem, EmailItemParent);
            item.SetActive(true);
            item.transform.GetComponent<EmailItemControl>().SetValue(GameData.CurrentClubInfo.ApplyMemList[i],false);
            //todo  设置信息
            item.transform.localPosition = new Vector3(0,166- EmailItemList.Count*100,0);
            EmailItemList.Add(item);
        }

        for (int i = 0; i < GameData.CurrentClubInfo.InviteList.Count; i++)
        {
            GameObject item = Instantiate(EmailItem, EmailItemParent);
            item.SetActive(true);
            item.transform.GetComponent<EmailItemControl>().SetValue(GameData.CurrentClubInfo.InviteList[i], true);
            //todo  设置信息
            item.transform.localPosition = new Vector3(0, 166 - EmailItemList.Count * 100, 0);
            EmailItemList.Add(item);
        }
    }

    /// <summary>
    /// 重置
    /// </summary>
    private void Reset()
    {
        
    }
    // Use this for initialization
    void Start () {
        CloseBtn.onClick.Add(new EventDelegate(this.CloseBtnClick));

    }

    private void CloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
