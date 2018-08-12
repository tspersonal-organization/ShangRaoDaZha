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

public class ClubListPanelControl : UIBase<ClubListPanelControl>
{

    public GameObject Clubitem;//item预制体
    public Transform ClubItemParent;//父物体
    List<GameObject> CreatedItemList = new List<GameObject>();//

    public UIButton CloseBtn;
    public UIButton JoinClubBtn;//加入俱乐部按钮
    void OnEnable()
    {
        InitData();
    }
	// Use this for initialization
	void Start () {
        CloseBtn.onClick.Add(new EventDelegate(()=>
        {
            this.gameObject.SetActive(false);
        }));

        JoinClubBtn.onClick.Add(new EventDelegate(this.JoinClub));

    }

    /// <summary>
    /// 加入俱乐部
    /// </summary>
    private void JoinClub()
    {
        UIManager.Instance.ShowUIPanel(UIPaths.ApplyJoinClubPanel, OpenPanelType.MinToMax);
    }

    // Update is called once per frame
    void Update () {
		
	}

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        int count = CreatedItemList.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(CreatedItemList[i]);
        }
        CreatedItemList = new List<GameObject>();

        for (int i = 0; i < GameData.ClubInfoList.Count; i++)
        {
            GameObject g = Instantiate(Clubitem, ClubItemParent);
            g.transform.localScale = Vector3.one;
            if (i % 2 == 1)
            {
                g.transform.localPosition = new Vector3(230f, 114 - ((i-1) / 2) * 186, 0);
            }
            else
            {
                g.transform.localPosition = new Vector3(-230f,114-(i/2)*186,0);
            }
            g.transform.GetComponent<ClubItemControl>().SetValue(GameData.ClubInfoList[i]);
            g.SetActive(true);
            CreatedItemList.Add(g);
        }
    }
}
