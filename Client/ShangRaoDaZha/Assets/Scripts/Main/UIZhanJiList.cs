using FrameworkForCSharp.NetWorks;
using System.Collections.Generic;
using UnityEngine;

public class UIZhanJiList : UIBase<UIZhanJiList>
{
    public List<GameObject> listItem;
    public Transform itemHistoryBase;
    public GameObject itemHistory;
    public GameObject btnClose;
    public UIWrapContent WC;
    void Start()
    {
        ClientToServerMsg.Send(Opcodes.Client_RoomRecordBase, 0, 10000);

        listItem = new List<GameObject>();
        for (var i = 0; i < itemHistoryBase.childCount; i++)
        {
            itemHistoryBase.GetChild(i).gameObject.SetActive(false);
            listItem.Add(itemHistoryBase.GetChild(i).gameObject);
        }

        UIEventListener.Get(btnClose).onClick = OnClick;
        for (int i = 0; i < listItem.Count; i++)
            UIEventListener.Get(listItem[i]).onClick = OnClick;

    }

    void OnClick(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        if (btnClose == go)
        {
            UIManager.Instance.HideUiPanel(UIPaths.PanelHistory);
        }
        else
        {

            ulong guid = ulong.Parse(go.name);
            for (int i = 0; i < GameData.m_RecordList.Count; i++)
            {
                if (guid == GameData.m_RecordList[i].guid)//所有大战绩列表
                {
                    GameData.m_ChooseRecordListInfo = GameData.m_RecordList[i];
                    GameData.m_TableInfo.fangZhuGuid = GameData.m_RecordList[i].fangZhuGuid;
                    
                    break;
                }
            }

        }
    }

    public void ShowItems()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject go = Instantiate(itemHistory, itemHistoryBase, false);
            listItem.Add(go);
            go.SetActive(true);
            LoadItem(go, GameData.m_RecordList[i]);
        }
        itemHistoryBase.GetComponent<UIGrid>().repositionNow = true;
    }

    /// <summary>
    /// 设置item的信息
    /// </summary>
    /// <param name="go"></param>
    /// <param name="info"></param>
    void LoadItem(GameObject go, RecordListInfo info)
    {
        string timeText = info.startTime.ToString("yyyy / MM / dd \nHH: mm-") + info.endTime.ToString("HH:mm");
        go.transform.Find("time").GetComponent<UILabel>().text = timeText;
        go.name = info.guid.ToString();
        go.transform.Find("roomID").GetComponent<UILabel>().text = "房间号:" + info.id.ToString();
        Transform player = go.transform.Find("Player");
        for (int i = 0; i < player.childCount; i++)
        {
            if (i < info.playerInfo.Count)
            {
                string[] strs = info.playerInfo[i].Split('@');
                player.GetChild(i).gameObject.SetActive(true);
                player.GetChild(i).Find("LabName").GetComponent<UILabel>().text = strs[1];
                player.GetChild(i).Find("LabScore").GetComponent<UILabel>().text = strs[2];
            }
            else
            {
                player.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
