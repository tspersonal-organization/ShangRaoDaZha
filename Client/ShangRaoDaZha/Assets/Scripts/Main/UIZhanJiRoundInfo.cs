using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIZhanJiRoundInfo : UIBase<UIZhanJiRoundInfo>
{
    public GameObject[] ItemArray;
    public GameObject btnClose;
    public UIWrapContent WC;
   

    void Start()
    {
        UIEventListener.Get(btnClose).onClick = OnClick;
        for (int i = 0; i < ItemArray.Length; i++)
            UIEventListener.Get(ItemArray[i].transform.Find("btnOperate").gameObject).onClick = OnClick;

        ShowItems();
    }

    public void ShowItems()
    {

        if (GameData.m_RecordRoundList.Count <= 6)
        {
            for (int i = 0; i < GameData.m_RecordRoundList.Count; i++)
            {
                ItemArray[i].SetActive(true);
                LoadItem(ItemArray[i], GameData.m_RecordRoundList[i],i);
                UIEventListener.Get(ItemArray[i].transform.Find("btnOperate").gameObject).onClick = OnClick;
            }
        }
        else
        {
            WC.enabled = true;
            WC.maxIndex = 0;
            WC.minIndex = -(GameData.m_RecordRoundList.Count - 1);
            WC.onInitializeItem = OnInitializeItem;

            for (int i = 0; i < 6; i++)
            {
                ItemArray[i].SetActive(true);
                LoadItem(ItemArray[i], GameData.m_RecordRoundList[i],i);
                UIEventListener.Get(ItemArray[i].transform.Find("btnOperate").gameObject).onClick = OnClick;
            }
        }
    }

    void OnInitializeItem(GameObject go, int wrapIndex, int realIndex)
    {
        LoadItem(go, GameData.m_RecordRoundList[Math.Abs(realIndex)], Math.Abs(realIndex));
    }

    void OnClick(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        if (btnClose == go)
        {
            UIManager.Instance.HideUiPanel(UIPaths.UIPanel_ZhanJiRoundInfo);
        }
        else
        {
             uint index = uint.Parse(go.transform.parent.name);

           // uint index = uint.Parse(go.name);
            Log.Debug(index);

            // GameData.m_RecordRoundList[(int)index].ZhuangGuid;
            for (int i = 0; i < GameData.m_HuiFangList[(int)index].playerList.Count; i++)//确定那一条录像  设置庄的位置
            {
                if (GameData.m_HuiFangList[(int)index].playerList[i].guid == GameData.m_RecordRoundList[(int)index].ZhuangGuid)
                {
                    GameData.m_TableInfo.makerPos = GameData.m_HuiFangList[(int)index].playerList[i].pos;
                }
            }
            GameData.m_TableInfo.curGameCount = index;//房间的第几局
            GameData.MianCard = GameData.m_HuiFangList[(int)index].MianCard;//载宝回放需要
            GameData.MagicCard= GameData.m_HuiFangList[(int)index].MagicCard;


            UIManager.Instance.ShowUiPanel(UIPaths.UIPanel_HuiFang);
        }
    }

    void LoadItem(GameObject go, RecordListInfo info,int num)
    {
        string timeText = info.startTime.ToString("yyyy / MM / dd \nHH: mm-") + info.endTime.ToString("HH:mm");
        go.transform.Find("time").GetComponent<UILabel>().text = timeText;
        go.name = num.ToString();
        go.transform.Find("roomID").GetComponent<UILabel>().text = "房间号:" + GameData.m_TableInfo.id;
        //go.transform.FindChild("fangZhuName").GetComponent<UILabel>().text = info.fangZhuName;
        string names = "";
        string scores = "";
        int index = 0;
        for (int i = 0; i < info.playerInfo.Count; i++)
        {
            string[] strs = info.playerInfo[i].Split('@');
            if (names.Length == 0) names += strs[1];
            else names += "\n" + strs[1];

            if (scores.Length == 0) scores += strs[2];
            else scores += "\n" + strs[2];
            if (strs[0] == info.fangZhuGuid.ToString()) index = i;
        }
        go.transform.Find("nameTxt").GetComponent<UILabel>().text = names;
        go.transform.Find("scoreTxt").GetComponent<UILabel>().text = scores;
        go.transform.Find("fangZhu").localPosition = new Vector3(-235, 48 - index * 32, 0);
    }
}
