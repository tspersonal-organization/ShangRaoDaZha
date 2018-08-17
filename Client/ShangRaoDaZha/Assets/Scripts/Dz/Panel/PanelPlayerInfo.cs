using UnityEngine;

public class PanelPlayerInfo : UIBase<PanelPlayerInfo>
{

    public UITexture Headtexture;
    public UISprite Sex;
    public UILabel name;
    public UILabel ID;
    public UILabel IP;
    public UILabel Address;

    public UIButton CancleMasterBtn;
    public UIButton SetMasterBtn;
    public UIButton TickOutBtn;
    public UIButton CloseBtn;


    private void OnEnable()
    {
        if (ManagerScene.Instance.currentSceneType != SceneType.Game)
        {
            bool _bChoseAdmin = false;//选中的玩家是否是管理员
            bool _bAdmin = false;//自己是否是管理员
            for (int i = 0; i < GameData.CurrentClubInfo.MemMasterList.Count; i++)
            {
                if (GameData.CurrentClubInfo.MemMasterList[i].Guid == GameData.ChoseMem.Guid)
                {
                    _bChoseAdmin = true;
                }
                if (GameData.CurrentClubInfo.MemMasterList[i].Guid == Player.Instance.guid)
                {
                    _bAdmin = true;
                }
            }

            if (GameData.ChoseMem.Guid == GameData.CurrentClubInfo.CreatorGuid)
            {
                //如果选中的是会长
                if (GameData.CurrentClubInfo.CreatorGuid == Player.Instance.guid)
                {
                    //如果自己是会长
                    TickOutBtn.gameObject.SetActive(true);
                    CancleMasterBtn.gameObject.SetActive(false);
                    SetMasterBtn.gameObject.SetActive(false);
                    TickOutBtn.transform.localPosition = new Vector3(0, -161, 0);
                }
                else
                {
                    //如果自己是管理员
                    TickOutBtn.gameObject.SetActive(false);
                    CancleMasterBtn.gameObject.SetActive(false);
                    SetMasterBtn.gameObject.SetActive(false);
                }

            }
            else if (_bChoseAdmin)
            {
                //如果选中的是管理员
                if (GameData.CurrentClubInfo.CreatorGuid == Player.Instance.guid)
                {
                    //如果自己是会长
                    TickOutBtn.gameObject.SetActive(true);
                    CancleMasterBtn.gameObject.SetActive(true);
                    SetMasterBtn.gameObject.SetActive(false);
                    TickOutBtn.transform.localPosition = new Vector3(-150, -161, 0);
                    CancleMasterBtn.transform.localPosition = new Vector3(150, -161, 0);
                }
                else if (_bAdmin)
                {
                    //如果自己是管理员
                    if (GameData.ChoseMem.Guid == Player.Instance.guid)
                    {
                        //如果点击的是自己
                        TickOutBtn.gameObject.SetActive(true);
                        CancleMasterBtn.gameObject.SetActive(false);
                        SetMasterBtn.gameObject.SetActive(false);
                        TickOutBtn.transform.localPosition = new Vector3(0, -161, 0);
                    }
                    else
                    {
                        //如果点击的是另一个管理员
                        TickOutBtn.gameObject.SetActive(false);
                        CancleMasterBtn.gameObject.SetActive(false);
                        SetMasterBtn.gameObject.SetActive(false);
                    }
                }
                else
                {
                    //如果自己是普通会员
                    TickOutBtn.gameObject.SetActive(false);
                    CancleMasterBtn.gameObject.SetActive(false);
                    SetMasterBtn.gameObject.SetActive(false);
                }
            }
            else
            {
                //选中的是普通成员
                if (GameData.CurrentClubInfo.CreatorGuid == Player.Instance.guid)
                {
                    //如果自己是会长
                    TickOutBtn.gameObject.SetActive(true);
                    CancleMasterBtn.gameObject.SetActive(false);
                    SetMasterBtn.gameObject.SetActive(true);
                    TickOutBtn.transform.localPosition = new Vector3(-150, -161, 0);
                    SetMasterBtn.transform.localPosition = new Vector3(150, -161, 0);
                }
                else if (_bAdmin)
                {
                    //如果自己是管理员
                    TickOutBtn.gameObject.SetActive(true);
                    CancleMasterBtn.gameObject.SetActive(false);
                    SetMasterBtn.gameObject.SetActive(false);
                    TickOutBtn.transform.localPosition = new Vector3(0, -161, 0);
                }
                else
                {
                    //如果自己是普通会员
                    if (GameData.ChoseMem.Guid == Player.Instance.guid)
                    {
                        //如果点击的是自己
                        TickOutBtn.gameObject.SetActive(true);
                        CancleMasterBtn.gameObject.SetActive(false);
                        SetMasterBtn.gameObject.SetActive(false);
                        TickOutBtn.transform.localPosition = new Vector3(0, -161, 0);
                    }
                    else
                    {
                        //如果点击的是其他会员
                        TickOutBtn.gameObject.SetActive(false);
                        CancleMasterBtn.gameObject.SetActive(false);
                        SetMasterBtn.gameObject.SetActive(false);
                    }
                }
            }

            DownloadImage.Instance.Download(Headtexture, GameData.ChoseMem.HeadId);
            name.text = "昵称:" + GameData.ChoseMem.Name;
            if (GameData.ChoseMem.Sex == 1)
            {
                Sex.spriteName = "man";
            }
            else
            {
                Sex.spriteName = "girl";
            }
            Sex.MakePixelPerfect();
            ID.text = "ID:" + GameData.ChoseMem.Guid.ToString();
            IP.text = "IP:" + GameData.ChoseMem.Ip.ToString();
            Address.text = "地址:" + GameData.ChoseMem.Adress;
        }
        else
        {
            CancleMasterBtn.gameObject.SetActive(false);
            SetMasterBtn.gameObject.SetActive(false);
            TickOutBtn.gameObject.SetActive(false);

            DownloadImage.Instance.Download(Headtexture, GameData.ChosePlayer.headID);
            name.text = "昵称:" + GameData.ChosePlayer.name;
            if (GameData.ChosePlayer.sex == 1)
            {
                Sex.spriteName = "man";
            }
            else
            {
                Sex.spriteName = "girl";
            }
            Sex.MakePixelPerfect();
            ID.text = "ID:" + GameData.ChosePlayer.guid.ToString();
            IP.text = "IP:" + GameData.ChosePlayer.ip.ToString();
            Address.text = "地址:" + GameData.ChosePlayer.address;
        }

    }
    // Use this for initialization
    void Start()
    {
        CancleMasterBtn.onClick.Add(new EventDelegate(CancleMasterBtnClick));
        SetMasterBtn.onClick.Add(new EventDelegate(SetMasterBtnClick));
        TickOutBtn.onClick.Add(new EventDelegate(TickOutBtnClick));
        CloseBtn.onClick.Add(new EventDelegate(() =>
        {
            this.gameObject.SetActive(false);
        }));
    }

    private void CancleMasterBtnClick()
    {
        this.gameObject.SetActive(false);
        ClientToServerMsg.SetMasterOper((uint)GameData.CurrentClubInfo.Id, GameData.ChoseMem.Guid, false);
    }

    private void SetMasterBtnClick()
    {
        this.gameObject.SetActive(false);
        ClientToServerMsg.SetMasterOper((uint)GameData.CurrentClubInfo.Id, GameData.ChoseMem.Guid, true);
    }

    private void TickOutBtnClick()
    {
        this.gameObject.SetActive(false);
        ClientToServerMsg.RemovePlayerFromClub((uint)GameData.CurrentClubInfo.Id, GameData.ChoseMem.Guid);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
