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

public class ClubMemControlPanelControl : UIBase<ClubMemControlPanelControl>
{


    public GameObject item;
    public Transform ItemParent;
    List<GameObject> CreatItem;

    public UIButton ApplyListBtn;
    public UIButton AllMemBtn;
    public UIButton InviteBtn;
    public UIButton CloseBtn;
	// Use this for initialization
	void Start () {
        ApplyListBtn.onClick.Add(new EventDelegate(this.ApplyListBtnClick));
        AllMemBtn.onClick.Add(new EventDelegate(this.AllMemBtnClick));
        InviteBtn.onClick.Add(new EventDelegate(this.InviteFriendClick));
        CloseBtn.onClick.Add(new EventDelegate(()=>
        {
            this.gameObject.SetActive(false);
        }));
    }

    private void OnEnable()
    {
        ApplyListBtnClick();
    }
    /// <summary>
    /// 邀请玩家点击
    /// </summary>
    private void InviteFriendClick()
    {
        UIManager.Instance.ShowUIPanel(UIPaths.InvitePlayerPanel);
    }

    List<GameObject> CreatedObj = new List<GameObject>();
    /// <summary>
    /// 销毁生成的物体
    /// </summary>
    private void DestroyObj()
    {
        int count = CreatedObj.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(CreatedObj[i]);
        }

        CreatedObj = new List<GameObject>();
    }

    bool showApply = false;
    //共外部调用
    public void InitData()
    {
        if (showApply)
        {
            ApplyListBtnClick();
        }
        else
        {
            AllMemBtnClick();
        }
    }
    /// <summary>
    /// 所有成员点击
    /// </summary>
    private void AllMemBtnClick()
    {
        showApply = false;
        PanelReset();
        DestroyObj();
        ApplyListBtn.transform.Find("Sprite").gameObject.SetActive(false);
        AllMemBtn.transform.Find("Sprite").gameObject.SetActive(true);
        CreatAllItem();
    }

    /// <summary>
    /// 申请点击
    /// </summary>
    private void ApplyListBtnClick()
    {
        showApply = true;
        PanelReset();
        DestroyObj();
        ApplyListBtn.transform.Find("Sprite").gameObject.SetActive(true);
        AllMemBtn.transform.Find("Sprite").gameObject.SetActive(false);
        CreatApplyItem();


    }


    void PanelReset()
    {
        ItemParent.localPosition = Vector3.zero;
        ItemParent.GetComponent<UIPanel>().clipOffset = new Vector2(0,0);
    }

    void CreatApplyItem()
    {
        for (int i = 0; i < GameData.CurrentClubInfo.ApplyMemList.Count; i++)
        {
            GameObject g = GameObject.Instantiate(item, ItemParent);
            g.transform.localScale = Vector3.one;
            g.transform.GetComponent<MemItemControl>().SetValue(true, GameData.CurrentClubInfo.ApplyMemList[i]);
            g.transform.localPosition = new Vector3(0, 90 - 100 * CreatedObj.Count, 0);
            g.SetActive(true);
            CreatedObj.Add(g);
          //  yield return new WaitForEndOfFrame();
        }
    }

    void CreatAllItem()
    {
        for (int i = 0; i < GameData.CurrentClubInfo.MemList.Count; i++)
        {
            GameObject g = GameObject.Instantiate(item, ItemParent);
            g.transform.localScale = Vector3.one;
            g.transform.GetComponent<MemItemControl>().SetValue(false, GameData.CurrentClubInfo.MemList[i]);
            g.transform.localPosition = new Vector3(0, 90 - 100 * CreatedObj.Count, 0);
            g.SetActive(true);
            CreatedObj.Add(g);
           // yield return new WaitForEndOfFrame();
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
