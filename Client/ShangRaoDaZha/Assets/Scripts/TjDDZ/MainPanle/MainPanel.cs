using System.Collections.Generic;
using UnityEngine;

public class MainPanel : UIBase<MainPanel>
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

    public UIButton CreatRoomBtn;//创建
    public UIButton JoinRoomBtn;//加入

    public UIButton ZhanJiBtn;//战绩
    public UIButton MessageBtn;//消息

    public UIButton EmailBtn;//邮件按钮

    public UIButton SettingBtn;//设置
    public UIButton ShareBtn;//分享

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

        //发送定位信息
        //ClientToServerMsg.SetAdrress(location);
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
            obj.transform.GetComponent<PKClubRoomItemControl>().SetValue(GameData.PKClubRoomList[i]);
            obj.transform.localPosition = new Vector3(-5, 60 - 80 * PKClubRoomItemObjList.Count, 0);
            PKClubRoomItemObjList.Add(obj);
        }
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
            obj.transform.GetComponent<PKClubItemControl>().SetValue(GameData.PKClubInfoList[i]);
            obj.transform.localPosition = new Vector3(-5, 150 - 80 * PKClubItemObjList.Count, 0);
            PKClubItemObjList.Add(obj);
        }
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

    protected override void OnDestroy()
    {
        GameEventDispatcher.Instance.removeEventListener(EventIndex.PlayerLivingDataChange, this.SetPlayerInfo);
        base.OnDestroy();
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
        ClubBackBtn.onClick.Add(new EventDelegate(ClubInfoBack));

        EmailBtn.onClick.Add(new EventDelegate(OpenEmailPanel));
        CreatRoomBtn.onClick.Add(new EventDelegate(CreatRoomClick));
        JoinRoomBtn.onClick.Add(new EventDelegate(JoinRoomClick));

        ZhanJiBtn.onClick.Add(new EventDelegate(ZhanjiClick));
        MessageBtn.onClick.Add(new EventDelegate(MessageClick));
        SettingBtn.onClick.Add(new EventDelegate(SettingBtnClick));
        
        ShareBtn.onClick.Add(new EventDelegate(this.SharePicture));
        
        JoinClubBtn.onClick.Add(new EventDelegate(() =>
        {
            UIManager.Instance.ShowUIPanel(UIPaths.ApplyJoinClubPanel, OpenPanelType.MinToMax);
        }));
        CreatClubBtn.onClick.Add(new EventDelegate(() =>
        {
            GameData.ResultCodeStr = "请联系客服";
            UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_Dialog, OpenPanelType.MinToMax);
            //UIManager.Instance.ShowUIPanel(UIPaths.ApplyJoinClubPanel, OpenPanelType.MinToMax);
        }));
    }

    /// <summary>
    /// 打开邮件面板
    /// </summary>
    private void OpenEmailPanel()
    {
        UIManager.Instance.ShowUIPanel(UIPaths.ContentPanel, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    
    /// <summary>
    /// 打开邀请列表
    /// </summary>
    private void OpenInviteMessagePanel()
    {
        Debug.LogError("打开邀请列表");
        UIManager.Instance.ShowUIPanel(UIPaths.ClubInvitePlayerPanel);
    }

    //俱乐部点击
    private void ClubBtnClick()
    {
        ClientToServerMsg.GetClubList();//获取俱乐部了列表
    }


    /// <summary>
    ///增加邀请人
    /// </summary>
    private void OpenInvitePanel()
    {
        UIManager.Instance.ShowUIPanel(UIPaths.InvitePanel, OpenPanelType.MinToMax);
    }

    /// <summary>
    /// 分享图片
    /// </summary>
    private void SharePicture()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            AuthorizeOrShare.Instance.ShareImageURL("欢迎来玩上吉打炸！", GameData.ShareImageURL);

        }
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void OpenMarketPanel()
    {
        UIManager.Instance.ShowUIPanel(UIPaths.MarketPanel, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void SettingBtnClick()
    {
        UIManager.Instance.ShowUIPanel(UIPaths.SettingPanel, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void RuleBtnClick()
    {
        UIManager.Instance.ShowUIPanel(UIPaths.RulePanel, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void MessageClick()
    {
        UIManager.Instance.ShowUIPanel(UIPaths.EmailPanel, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void ZhanjiClick()
    {
        UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_ZhanJiList, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void JoinRoomClick()
    {
        UIManager.Instance.ShowUIPanel(UIPaths.JoinRoomPanel, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void CreatRoomClick()
    {
        UIManager.Instance.ShowUIPanel(UIPaths.CreatRoomPanel, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    /// <summary>
    /// 显示我的房间  代理专用
    /// </summary>
    public void ShowAngentRoomListPanel()
    {
        UIManager.Instance.ShowUIPanel(UIPaths.MyRoomPanel, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    #endregion


    #region  初始化玩家信息

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
    #endregion

    float speed = 30f;
    int index = 0;
    // Update is called once per frame
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
}
