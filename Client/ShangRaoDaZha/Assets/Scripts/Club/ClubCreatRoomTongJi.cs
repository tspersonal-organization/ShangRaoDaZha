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

public class ClubCreatRoomTongJi : MonoBehaviour {

    public UIButton CloseBtn;

    public UIButton ToDayBtn;
    public UIButton YesTBtn;

    public GameObject TongJiLable;
    public Transform TongJiParent;
    private bool ChoseToday = false;
	// Use this for initialization
	void Start () {
        CloseBtn.onClick.Add(new EventDelegate(()=>
        {
            this.gameObject.SetActive(false);
        }));
        ToDayBtn.onClick.Add(new EventDelegate(this.ToDayBtnClick));
        YesTBtn.onClick.Add(new EventDelegate(this.YesTBtnClick));
        ToDayBtnClick();
    }


    void PanelReset()
    {
        TongJiParent.localPosition = Vector3.zero;
        TongJiParent.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
    }

    //昨日点击
    private void YesTBtnClick()
    {
        if (ChoseToday)
        {
            int count = CreatList.Count;
            for (int i = 0; i < count; i++)
            {
                Destroy(CreatList[i]);
            }
            CreatList = new List<GameObject>();
            PanelReset();
            for (int i = 0; i < GameData.CurrentClubInfo.HistoryDataList.Count; i++)
            {
                if (GameData.CurrentClubInfo.HistoryDataList[i].Time != DateTime.Now.ToShortDateString())
                {
                    foreach (var item in GameData.CurrentClubInfo.HistoryDataList[i].RoomTypeAndDownCount)
                    {
                        GameObject g = Instantiate(TongJiLable, TongJiParent);
                        g.transform.localScale = Vector3.one;
                        g.SetActive(true);
                        CreatList.Add(g);
                        g.transform.localPosition = new Vector3(0, 15 - 50 * (CreatList.Count - 1), 0);
                        switch (item.Key)
                        {
                            case FrameworkForCSharp.Utils.ClubRoomType.WDH_Eight:
                                g.transform.GetComponent<UILabel>().text = string.Format("无挡胡  8局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.WDH_Twelve:
                                g.transform.GetComponent<UILabel>().text = string.Format("无挡胡  12局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.WDH_Sixteen:
                                g.transform.GetComponent<UILabel>().text = string.Format("无挡胡  16局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.ZB_Eight:
                                g.transform.GetComponent<UILabel>().text = string.Format("栽宝  8局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.ZB_Twelve:
                                g.transform.GetComponent<UILabel>().text = string.Format("栽宝  12局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.ZB_Sixteen:
                                g.transform.GetComponent<UILabel>().text = string.Format("栽宝  16局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.TS_Eight:
                                g.transform.GetComponent<UILabel>().text = string.Format("讨赏  8局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.TS_Twelve:
                                g.transform.GetComponent<UILabel>().text = string.Format("讨赏  12局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.TS_Sixteen:
                                g.transform.GetComponent<UILabel>().text = string.Format("讨赏  16局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.NN_Ten:
                                g.transform.GetComponent<UILabel>().text = string.Format("牛牛  10局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.NN_Twenty:
                                g.transform.GetComponent<UILabel>().text = string.Format("牛牛  20局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.NN_Thirty:
                                g.transform.GetComponent<UILabel>().text = string.Format("牛牛  30局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.None:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        ChoseToday = false;
        ToDayBtn.transform.Find("Sprite").gameObject.SetActive(false);
        YesTBtn.transform.Find("Sprite").gameObject.SetActive(true);
    }

    List<GameObject> CreatList = new List<GameObject>();
    //今日点击
    private void ToDayBtnClick()
    {
        ToDayBtn.transform.Find("Sprite").gameObject.SetActive(true);
        YesTBtn.transform.Find("Sprite").gameObject.SetActive(false);
        if (!ChoseToday)
        {
            int count = CreatList.Count;
            for (int i = 0; i < count; i++)
            {
                Destroy(CreatList[i]);
            }
            CreatList = new List<GameObject>();
            PanelReset();
            for (int i = 0; i < GameData.CurrentClubInfo.HistoryDataList.Count; i++)
            {
                if (GameData.CurrentClubInfo.HistoryDataList[i].Time == DateTime.Now.ToShortDateString())
                {
                    foreach (var item in GameData.CurrentClubInfo.HistoryDataList[i].RoomTypeAndDownCount)
                    {
                        GameObject g = Instantiate(TongJiLable, TongJiParent);
                        g.transform.localScale = Vector3.one;
                        g.SetActive(true);
                        CreatList.Add(g);
                        g.transform.localPosition = new Vector3(0,15-50*(CreatList.Count-1),0);
                        switch (item.Key)
                        {
                            case FrameworkForCSharp.Utils.ClubRoomType.WDH_Eight:
                                g.transform.GetComponent<UILabel>().text = string.Format("无挡胡  8局     完成    {0}局",item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.WDH_Twelve:
                                g.transform.GetComponent<UILabel>().text = string.Format("无挡胡  12局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.WDH_Sixteen:
                                g.transform.GetComponent<UILabel>().text = string.Format("无挡胡  16局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.ZB_Eight:
                                g.transform.GetComponent<UILabel>().text = string.Format("栽宝  8局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.ZB_Twelve:
                                g.transform.GetComponent<UILabel>().text = string.Format("栽宝  12局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.ZB_Sixteen:
                                g.transform.GetComponent<UILabel>().text = string.Format("栽宝  16局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.TS_Eight:
                                g.transform.GetComponent<UILabel>().text = string.Format("讨赏  8局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.TS_Twelve:
                                g.transform.GetComponent<UILabel>().text = string.Format("讨赏  12局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.TS_Sixteen:
                                g.transform.GetComponent<UILabel>().text = string.Format("讨赏  16局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.NN_Ten:
                                g.transform.GetComponent<UILabel>().text = string.Format("牛牛  10局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.NN_Twenty:
                                g.transform.GetComponent<UILabel>().text = string.Format("牛牛  20局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.NN_Thirty:
                                g.transform.GetComponent<UILabel>().text = string.Format("牛牛  30局     完成    {0}局", item.Value);
                                break;
                            case FrameworkForCSharp.Utils.ClubRoomType.None:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        ChoseToday = true;
       
    }

    // Update is called once per frame
    void Update () {
		
	}
}
