using FrameworkForCSharp.NetWorks;
using System;
using UnityEngine;

public class UIZhanJiList : UIBase<UIZhanJiList>
{
    public GameObject[] ItemArray;
    public GameObject btnClose;
    public UIWrapContent WC;
	void Start ()
    {
        ClientToServerMsg.Send(Opcodes.Client_RoomRecordBase, 0, 10000);
        UIEventListener.Get(btnClose).onClick = OnClick;
        for (int i = 0; i < ItemArray.Length; i++)
            UIEventListener.Get(ItemArray[i]).onClick = OnClick;

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
           // UIManager.Instance.ShowUiPanel(UIPaths.UIPanel_ZhanJiRoundInfo, OpenPanelType.MinToMax);
            ulong guid = ulong.Parse(go.name);
            for (int i = 0; i < GameData.m_RecordList.Count; i++)
            {
                if(guid == GameData.m_RecordList[i].guid)//所有大战绩列表
                {
                    GameData.m_ChooseRecordListInfo = GameData.m_RecordList[i];
                    GameData.m_TableInfo.fangZhuGuid = GameData.m_RecordList[i].fangZhuGuid;

                    //if (GameData.m_RecordList[i].roomType == FrameworkForCSharp.Utils.RoomType.XYGQP)
                    //{
                    //    GameData.ResultCodeStr = "此条战绩没有录像！！！";
                    //    UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
                    //}
                    //else if (GameData.m_RecordList[i].roomType == FrameworkForCSharp.Utils.RoomType.NN)
                    //{
                    //    GameData.ResultCodeStr = "此条战绩没有录像！！！";
                    //    UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
                    //}
                    //else
                    //{
                    //    ClientToServerMsg.Send(Opcodes.Client_RoomRecordDetail, guid);
                    //    GameData.GlobleRoomType = GameData.m_RecordList[i].roomType;//点的是什么房间类型
                    //}
                    break;
                }
            }
          
        }
    }

    public void ShowItems()
    {

        if (GameData.m_RecordList.Count <= 6)
        {
            for (int i = 0; i < GameData.m_RecordList.Count; i++)
            {
                ItemArray[i].SetActive(true);
                LoadItem(ItemArray[i], GameData.m_RecordList[i]);
            }
        }
        else
        {
            WC.enabled = true;
            WC.maxIndex = 0;
            WC.minIndex = -(GameData.m_RecordList.Count - 1);
            WC.onInitializeItem = OnInitializeItem;

            for (int i = 0; i < 6; i++)
            {
                ItemArray[i].SetActive(true);
                LoadItem(ItemArray[i], GameData.m_RecordList[i]);
            }
        }
    }

    void OnInitializeItem(GameObject go, int wrapIndex, int realIndex)
    {
        LoadItem(go, GameData.m_RecordList[Math.Abs(realIndex)]);
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
        go.transform.Find("roomID").GetComponent<UILabel>().text = "房间号:"+info.id.ToString();
        //go.transform.FindChild("fangZhuName").GetComponent<UILabel>().text = info.fangZhuName;
        string names = "";
        string scores = "";

        string names1 = "";
        string scores1 = "";
        int index = 0;
        for (int i = 0; i < info.playerInfo.Count; i++)
        {
            string[] strs = info.playerInfo[i].Split('@');
            if (i < 4)
            {
                if (names.Length == 0) names += strs[1];
                else names += "\n" + strs[1];

                if (scores.Length == 0) scores += strs[2];
                else scores += "\n" + strs[2];
            }
            else
            {
                if (names1.Length == 0) names1 += strs[1];
                else names1 += "\n" + strs[1];

                if (scores1.Length == 0) scores1 += strs[2];
                else scores1 += "\n" + strs[2];
            }
            //string[] strs = info.playerInfo[i].Split('@');
            //if (names.Length == 0) names += strs[1];
            //else names += "\n" + strs[1];

            //if (scores.Length == 0) scores += strs[2];
            //else scores += "\n" + strs[2];

            if (strs[0] == info.fangZhuGuid.ToString()) index = i;

        }
        go.transform.Find("nameTxt").GetComponent<UILabel>().text = names;
        go.transform.Find("scoreTxt").GetComponent<UILabel>().text = scores;

        go.transform.Find("nameTxt (1)").GetComponent<UILabel>().text = names1;
        go.transform.Find("scoreTxt (1)").GetComponent<UILabel>().text = scores1;
        // go.transform.FindChild("fangZhu").localPosition = new Vector3(-235,48-index*32,0);
        if (index < 4)
        {
            go.transform.Find("fangZhu").localPosition = new Vector3(40, 48 - index * 32, 0);
        }
        else
        {
            go.transform.Find("fangZhu").localPosition = new Vector3(327, 48 - (index-4) * 32, 0);
        }
       
        switch (info.roomType)
        {
            case FrameworkForCSharp.Utils.RoomType.PK:
                go.transform.Find("RoomTypeLabel").GetComponent<UISprite>().spriteName = "UI_history_txt_game1";
                break;
            case FrameworkForCSharp.Utils.RoomType.WDH:
                go.transform.Find("RoomTypeLabel").GetComponent<UISprite>().spriteName = "UI_history_txt_game2";
                break;
            case FrameworkForCSharp.Utils.RoomType.NN:
                go.transform.Find("RoomTypeLabel").GetComponent<UISprite>().spriteName = "NiuNiu";
                break;
            case FrameworkForCSharp.Utils.RoomType.ZB:
                go.transform.Find("RoomTypeLabel").GetComponent<UISprite>().spriteName = "UI_history_txt_game3";
                break;
            case FrameworkForCSharp.Utils.RoomType.Other:
                go.transform.Find("RoomTypeLabel").GetComponent<UISprite>().spriteName = "";
                break;



        }
        go.transform.Find("RoomTypeLabel").GetComponent<UISprite>().MakePixelPerfect();
    }
}
