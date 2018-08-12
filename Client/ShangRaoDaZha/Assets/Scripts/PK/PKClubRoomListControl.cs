using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKClubRoomListControl : MonoBehaviour {


    public static PKClubRoomListControl Instance;

    public UIButton ClubInfoBtn;

    private void Awake()
    {
        Instance = this;
        Init();
    }

    private void Init()
    {
        ClubInfoBtn.onClick.Add(new EventDelegate(this.OpenClubInfoPanel));
    }

    /// <summary>
    /// 打开俱乐部信息面板
    /// </summary>
    private void OpenClubInfoPanel()
    {
        GameData.Tips = "该功能暂未开放！";
        UIManager.Instance.ShowUiPanel(UIPaths.PanelTips, OpenPanelType.MinToMax);
        //ClientToServerMsg.GetClubInfo(GameData.CurrentClickClubInfo.ClubId);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
