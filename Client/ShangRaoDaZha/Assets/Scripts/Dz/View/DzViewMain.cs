using System.Collections.Generic;
using UnityEngine;

public class DzViewMain : UIBase<DzViewMain>
{
    public GameObject HaveEmailNotice;//有邮件的红点提示
    public UILabel ChoseClubName;//
    public UIButton ClubBackBtn;//房间列表的返回
    public TweenPosition ClubListTw;//动画插件
    public TweenPosition RoomListTw;
    public GameObject PKClubItem;//俱乐部item
    public Transform PKClubItemParent;

    public GameObject PKClubRoomItem;//房间列表item
    public Transform PKClubRoomItemParent;

    public UIButton JoinClubBtn;//加入俱乐部按钮
    public UIButton CreatClubBtn;//加入俱乐部按钮
    public UIButton ClubInfoBtn;//俱乐部信息

    public UIButton CreatRoomBtn;//创建
    public UIButton JoinRoomBtn;//加入

    public UIButton ZhanJiBtn;//战绩
    public UIButton MessageBtn;//消息
    public UIButton EmailBtn;//邮件按钮
    public UIButton SettingBtn;//设置
    public UIButton ShareBtn;//分享

    public GameObject UserInfo;//用户信息
    public UILabel PlayerName;
    public UILabel PlayerId;
    public GameObject PlayerHeadSprite;
    public UILabel GongGaoLable;


    void Start()
    {

        SetButClickEvent();
        SetPlayerInfo();

        InitInviteClubMessage();
        GameEventDispatcher.Instance.addEventListener(EventIndex.PlayerLivingDataChange, this.SetPlayerInfo);

        CreatPkClubList();
    }

    float speed = 30f;
    int index = 0;

    void Update()
    {

        if (GameData.m_PaoMaDengList.Count > 0)
        {
            GongGaoLable.text = GameData.m_PaoMaDengList[index];
            GongGaoLable.transform.localPosition = new Vector3(GongGaoLable.transform.localPosition.x - speed * Time.deltaTime, GongGaoLable.transform.localPosition.y, GongGaoLable.transform.localPosition.z);

            if (GongGaoLable.transform.localPosition.x < -550)
            {
                index++;
                if (index == GameData.m_PaoMaDengList.Count)
                {
                    index = 0;
                }
                GongGaoLable.transform.localPosition = new Vector3(410f, GongGaoLable.transform.localPosition.y, GongGaoLable.transform.localPosition.z);
            }
        }
        // GongGaoLable.transform.Translate();

    }

    protected override void OnDestroy()
    {
        GameEventDispatcher.Instance.removeEventListener(EventIndex.PlayerLivingDataChange, this.SetPlayerInfo);
        base.OnDestroy();
    }

    /// <summary>
    /// 是否有信息
    /// </summary>
    /// <param name="have"></param>
    public void HasEmail(bool have)
    {
        if (have)
        {
            HaveEmailNotice.gameObject.SetActive(true);
        }
        else
        {
            HaveEmailNotice.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 返回
    /// </summary>
    private void ClubInfoBack()
    {
        // ClubListTw.SetOnFinished(new EventDelegate(this.ClubListTwFinish));
        ClubListTw.PlayReverse();
        // ClubListTw.ResetToBeginning();

        // RoomListTw.SetOnFinished(new EventDelegate(this.RoomListTwFinish));
        RoomListTw.PlayReverse();
        //  RoomListTw.ResetToBeginning();
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);

        //ClubListTw.PlayReverse();
        //RoomListTw.PlayReverse();
        //ClubListTw.onFinished.Add(new EventDelegate(ClubListTwFinish));
        //RoomListTw.onFinished.Add(new EventDelegate(RoomListTwFinish));


        ChosedClubId = 0;
    }

    /// <summary>
    /// 俱乐部创建房间列表返回
    /// </summary>
    public void CreatClubRoomList()
    {
        CreatPKClubRoomList();
        ChoseClubName.text = GameData.CurrentClickClubInfo.ClubName;
        ClubListTw.PlayForward();
        RoomListTw.PlayForward();

    }

    /// <summary>
    /// 生成莫格俱乐部的房间列表
    /// </summary>
    List<GameObject> PKClubRoomItemObjList = new List<GameObject>();
    public void CreatPKClubRoomList()
    {
        int count = PKClubRoomItemObjList.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(PKClubRoomItemObjList[i]);
        }
        PKClubRoomItemObjList = new List<GameObject>();
        for (int i = 0; i < GameData.PKClubRoomList.Count; i++)
        {
            GameObject obj = Instantiate(PKClubRoomItem, PKClubRoomItemParent);
            obj.SetActive(true);
            obj.transform.GetComponent<DzItemMomentRoom>().SetValue(GameData.PKClubRoomList[i]);
            PKClubRoomItemObjList.Add(obj);
        }
        PKClubRoomItemParent.GetComponent<UITable>().repositionNow = true;
    }

    /// <summary>
    /// 生成加入的俱乐部个数
    /// </summary>
    public uint ChosedClubId = 0;//选中的俱乐部id
    List<GameObject> PKClubItemObjList = new List<GameObject>();
    public void CreatPkClubList()
    {
        int count = PKClubItemObjList.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(PKClubItemObjList[i]);
        }
        PKClubItemObjList = new List<GameObject>();
        for (int i = 0; i < GameData.PKClubInfoList.Count; i++)
        {
            GameObject obj = Instantiate(PKClubItem, PKClubItemParent);
            obj.SetActive(true);
            obj.transform.GetComponent<DzItemMoment>().SetValue(GameData.PKClubInfoList[i]);
            PKClubItemObjList.Add(obj);
        }
        PKClubItemParent.GetComponent<UITable>().repositionNow = true;
    }

    /// <summary>
    /// 清空选中的俱乐部
    /// </summary>
    public void ClearClubChose()
    {
        for (int i = 0; i < PKClubItemObjList.Count; i++)
        {
            PKClubItemObjList[i].transform.Find("IsChosed").GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
        }
    }


    /// <summary>
    /// 初始化邀请俱乐部信息
    /// </summary>
    public void InitInviteClubMessage()
    {
        MessageBtn.transform.Find("Tips").gameObject.SetActive(false);
        if (Player.Instance.HaveEmail)
        {
            MessageBtn.transform.Find("Tips").gameObject.SetActive(true);
        }
        else
        {
            MessageBtn.transform.Find("Tips").gameObject.SetActive(false);
        }
    }
    #region  btn 点击事件

    /// <summary>
    /// 设置btn点击事件
    /// </summary>
    public void SetButClickEvent()
    {

        EmailBtn.onClick.Add(new EventDelegate(OpenEmailPanel));
        CreatRoomBtn.onClick.Add(new EventDelegate(CreatRoomClick));
        JoinRoomBtn.onClick.Add(new EventDelegate(JoinRoomClick));
        ZhanJiBtn.onClick.Add(new EventDelegate(HistoryClick));
        MessageBtn.onClick.Add(new EventDelegate(MessageClick));
        SettingBtn.onClick.Add(new EventDelegate(SettingBtnClick));
        ShareBtn.onClick.Add(new EventDelegate(ShareClick));

        ClubInfoBtn.onClick.Add(new EventDelegate(OpenClubInfoPanel));
        ClubBackBtn.onClick.Add(new EventDelegate(ClubInfoBack));
        JoinClubBtn.onClick.Add(new EventDelegate(() =>
        {
            UIManager.Instance.ShowUiPanel(UIPaths.PanelJoinMoment, OpenPanelType.MinToMax);
        }));
        CreatClubBtn.onClick.Add(new EventDelegate(() =>
        {
            GameData.ResultCodeStr = "请联系客服";
            UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
            //UIManager.Instance.ShowUiPanel(UIPaths.PanelJoinMoment, OpenPanelType.MinToMax);
        }));

        UIEventListener.Get(UserInfo).onClick = delegate
        {
            UIManager.Instance.ShowUiPanel(UIPaths.PanelUserInfo, OpenPanelType.MinToMax);
            SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        };
    }

    /// <summary>
    /// 分享图片
    /// </summary>
    private void ShareClick()
    {
        //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    AuthorizeOrShare.Instance.ShareImageURL("欢迎来玩上吉打炸！", GameData.ShareImageURL);

        //}
        UIManager.Instance.ShowUiPanel(UIPaths.PanelShare, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    /// <summary>
    /// 打开公告面板
    /// </summary>
    private void OpenEmailPanel()
    {
        UIManager.Instance.ShowUiPanel(UIPaths.PanelNotice, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    /// <summary>
    /// 设置界面
    /// </summary>
    private void SettingBtnClick()
    {
        UIManager.Instance.ShowUiPanel(UIPaths.PanelSetting, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    /// <summary>
    /// 消息界面
    /// </summary>
    private void MessageClick()
    {
        UIManager.Instance.ShowUiPanel(UIPaths.PanelMessage, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    /// <summary>
    /// 战绩
    /// </summary>
    private void HistoryClick()
    {
        UIManager.Instance.ShowUiPanel(UIPaths.PanelHistory, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    /// <summary>
    /// 加入房间
    /// </summary>
    private void JoinRoomClick()
    {
        UIManager.Instance.ShowUiPanel(UIPaths.PanelJoinRoom, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    private void CreatRoomClick()
    {
        UIManager.Instance.ShowUiPanel(UIPaths.PanelCreatRoom, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    /// <summary>
    /// 显示我的房间  代理专用
    /// </summary>
    public void ShowAngentRoomListPanel()
    {
        UIManager.Instance.ShowUiPanel(UIPaths.MyRoomPanel, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }


    /// <summary>
    /// 打开俱乐部信息面板
    /// </summary>
    private void OpenClubInfoPanel()
    {
        //GameData.Tips = "该功能暂未开放！";
        //UIManager.Instance.ShowUiPanel(UIPaths.PanelTips, OpenPanelType.MinToMax);
        ClientToServerMsg.GetClubInfo(GameData.CurrentClickClubInfo.ClubId);
    }

    #endregion

    /// <summary>
    /// 设置玩家信息
    /// </summary>
    public void SetPlayerInfo()
    {
        PlayerName.text = Player.Instance.otherName;
        PlayerId.text = "ID:" + Player.Instance.guid;

        DownloadImage.Instance.Download(PlayerHeadSprite.transform.GetComponent<UITexture>(), Player.Instance.headID);
        GongGaoLable.text = Player.Instance.content;
    }

}
