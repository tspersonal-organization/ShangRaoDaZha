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

public class ClubMastorControl : UIBase<ClubMastorControl>
{


    public UIButton AutoCreatRoomBtn;
    public UIButton ReNameBtn;
    public UIButton CreatRoomTongJi;
    public UIButton MemControl;
    public GameObject Mask;
   
	// Use this for initialization
	void Start () {
        // UIEventListener.Get(Mask).onClick = this.CloseClick;
        Mask.transform.GetComponent<UIButton>().onClick.Add(new EventDelegate(()=>
        {
            this.gameObject.SetActive(false);
        }));
        AutoCreatRoomBtn.onClick.Add(new EventDelegate(()=>
        {
            GameData.IsClubAutoCreatRoom = true;
            UIManager.Instance.ShowUIPanel(UIPaths.CreatRoomPanel, OpenPanelType.MinToMax);
        }));
        ReNameBtn.onClick.Add(new EventDelegate(()=>
        {
            UIManager.Instance.ShowUIPanel(UIPaths.ClubRenamePanel, OpenPanelType.MinToMax);
        }));
        CreatRoomTongJi.onClick.Add(new EventDelegate(() =>
        {
            UIManager.Instance.ShowUIPanel(UIPaths.CreatRoomTongJiPanel, OpenPanelType.MinToMax);
        }));
        MemControl.onClick.Add(new EventDelegate(() =>
        {
           // ClientToServerMsg.GetClubMemListInfo((uint)GameData.CurrentClubInfo.Id);//
           // UIManager.Instance.ShowUIPanel(UIPaths.ClubMemControlPanel, OpenPanelType.MinToMax);
        }));
    }

    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="go"></param>
    private void CloseClick(GameObject go)
    {
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
