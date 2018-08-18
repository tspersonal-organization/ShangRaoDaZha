using System.Collections.Generic;
using UnityEngine;

public class DzPanelMomentInfo : UIBase<DzPanelMomentInfo>
{
    public GameObject MasterObj;
    public GameObject Administrator;

    public UILabel ClubId;
    public UILabel GameCount;

    public UIButton InviteBtn;
    public UIButton QuiteBtn;
    public UIButton CloseBtn;

    public UIButton AdminPower;
    public UIButton MemberPower;

    public GameObject ClubMemItem;
    public GameObject SearchAll;
    public Transform ClubMemItemParent;
    List<GameObject> CreatRoomItem = new List<GameObject>();//生成的房间配置

    void OnEnable()
    {
        for (var i = 0; i < ClubMemItemParent.childCount; i++)
        {
            GameObject go = ClubMemItemParent.GetChild(i).gameObject;
            go.SetActive(false);
            CreatRoomItem.Add(go);
        }
        InitData();
    }

    void Start()
    {
        CloseBtn.onClick.Add(new EventDelegate(this.CloseBtnClick));
        InviteBtn.onClick.Add(new EventDelegate(this.ClubMastorBtnClick));
        QuiteBtn.onClick.Add(new EventDelegate(this.QuiteBtnClick));
        UIEventListener.Get(SearchAll).onClick = delegate
        {
            LoadMemer(13, GameData.CurrentClubInfo.NormalMemList.Count);
            SearchAll.SetActive(false);
        };
    }

    public void InitData()
    {
        SearchAll.SetActive(false);
        InviteBtn.gameObject.SetActive(false);
        QuiteBtn.gameObject.SetActive(true);
        QuiteBtn.transform.localPosition = new Vector3(0, -210, 0);

        //设置自己的权限
        if (GameData.CurrentClubInfo.CreatorGuid == Player.Instance.guid)
        {
            InviteBtn.gameObject.SetActive(true);
            QuiteBtn.transform.localPosition = new Vector3(-130, -210, 0);
            InviteBtn.transform.localPosition = new Vector3(130, -210, 0);
        }
        for (int i = 0; i < GameData.CurrentClubInfo.MemMasterList.Count; i++)
        {
            if (GameData.CurrentClubInfo.MemMasterList[i].Guid == Player.Instance.guid)
            {
                InviteBtn.gameObject.SetActive(true);
                QuiteBtn.transform.localPosition = new Vector3(-130, -210, 0);
                InviteBtn.transform.localPosition = new Vector3(130, -210, 0);
            }
        }
        ClubId.text = "好友圈ID:" + GameData.CurrentClubInfo.Id.ToString();
        GameCount.text = "牌局总数:" + GameData.CurrentClubInfo.ActiveRoomInfoList.Count.ToString();
        int nPower = (int) GameData.CurrentClubInfo.creatPower;
        AdminPower.enabled = false;
        MemberPower.enabled = false;
        if (nPower == 0)
        {
            //管理员开房
            AdminPower.transform.Find("Check").GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            MemberPower.transform.Find("Check").GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
        }
        else if (nPower == 1)
        {
            //会员开房    
            AdminPower.transform.Find("Check").GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            MemberPower.transform.Find("Check").GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
        }
        else
        {
            //无
            AdminPower.transform.Find("Check").GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            MemberPower.transform.Find("Check").GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
        }

        //设置会长信息
        MasterObj.GetComponent<ClubMemInfoControl>().SetData(GameData.CurrentClubInfo.CreatorMemInfo);

        //设置管理员信息  最多2个
        for (int i = 0; i < 2; i++){
            Administrator.transform.Find("Administrator" + (i + 1)).gameObject.SetActive(false);
        }

        for (int i = 0; i < GameData.CurrentClubInfo.MemMasterList.Count; i++)
        {
            if (i < 2)
            {
                Transform admin = Administrator.transform.Find("Administrator" + (i + 1));
                admin.gameObject.SetActive(true);
                admin.GetComponent<ClubMemInfoControl>().SetData(GameData.CurrentClubInfo.MemMasterList[i]);
            }
        }
        
        //成员信息
        int nMemerCount = GameData.CurrentClubInfo.NormalMemList.Count;
        if (nMemerCount > 13)
        {
            SearchAll.SetActive(true);
            LoadMemer(0, 13);
        }
        else if (nMemerCount == 13)
        {
            SearchAll.SetActive(false);
            LoadMemer(0, 13);
        }
        else
        {
            SearchAll.SetActive(false);
            LoadMemer(0, nMemerCount);
        }
    }

    /// <summary>
    /// 加载成员
    /// </summary>
    /// <param name="nStart">初始下标</param>
    /// <param name="nCount">加载数量</param>
    private void LoadMemer(int nStart = 0,int nCount = 13)
    {
        for (int i = nStart; i < nCount; i++)
        {
            GameObject item = null;
            if (i < CreatRoomItem.Count)
            {
                item = CreatRoomItem[i];
                item.SetActive(true);
                item.transform.GetComponent<ClubMemInfoControl>().SetData(GameData.CurrentClubInfo.NormalMemList[i]);
            }
            else
            {
                item = Instantiate(ClubMemItem, ClubMemItemParent);
                item.SetActive(true);
                item.transform.GetComponent<ClubMemInfoControl>().SetData(GameData.CurrentClubInfo.NormalMemList[i]);
                CreatRoomItem.Add(item);
            }
        }
    }

    //我要退出点击
    private void QuiteBtnClick()
    {
        this.gameObject.SetActive(false);
        ClientToServerMsg.QuiteClub((uint)GameData.CurrentClubInfo.Id, Player.Instance.guid);
    }
    //邀请玩家店家
    private void ClubMastorBtnClick()
    {
        UIManager.Instance.ShowUiPanel(UIPaths.PanelJoinMomentInvite, OpenPanelType.MinToMax);
    }

    //关闭点击
    private void CloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }
}
