using System.Collections.Generic;
using UnityEngine;

public class DzPanelMessage : UIBase<DzPanelMessage>
{
    public GameObject EmailItem;
    public Transform EmailItemParent;
    public UIButton CloseBtn;

    private List<GameObject> EmailItemList = new List<GameObject>();

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

    void Start()
    {
        CloseBtn.onClick.Add(new EventDelegate(this.CloseBtnClick));

    }

    public void CreatData()
    {
        int count = EmailItemList.Count;
        if (count == 0)
        {
            Player.Instance.HaveEmail = false;
            DzViewMain.Instance.InitInviteClubMessage();
        }
        for (int i = 0; i < count; i++)
        {
            Destroy(EmailItemList[i]);
        }
        EmailItemList = new List<GameObject>();
        for (int i = 0; i < GameData.CurrentClubInfo.ApplyMemList.Count; i++)
        {
            GameObject item = Instantiate(EmailItem, EmailItemParent);
            item.SetActive(true);
            item.transform.GetComponent<DzItemMessage>().SetValue(GameData.CurrentClubInfo.ApplyMemList[i], false);
            //todo  设置信息
            item.transform.localPosition = new Vector3(0, 166 - EmailItemList.Count * 100, 0);
            EmailItemList.Add(item);
        }

        for (int i = 0; i < GameData.CurrentClubInfo.InviteList.Count; i++)
        {
            GameObject item = Instantiate(EmailItem, EmailItemParent);
            item.SetActive(true);
            item.transform.GetComponent<DzItemMessage>().SetValue(GameData.CurrentClubInfo.InviteList[i], true);
            //todo  设置信息
            item.transform.localPosition = new Vector3(0, 166 - EmailItemList.Count * 100, 0);
            EmailItemList.Add(item);
        }
    }

    public void CloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }
}
