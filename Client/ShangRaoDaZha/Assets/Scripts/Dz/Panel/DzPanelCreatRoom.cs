using CreateRoomEntity;
using UnityEngine;

public class DzPanelCreatRoom : MonoBehaviour
{
    private CreateRoom roominfo;//创建房间的协议
    public UIButton CreatBtn;
    public UIButton CloseBtn;

    public UIButton TwoPlayer;
    public UIButton FourPlayer;

    public UIButton FiveRound;
    public UIButton TenRound;

    public UIButton JinDian;
    public UIButton BaoPai;
    public UIButton BaWangBtn;

    public UIButton KaiJiang7;
    public UIButton KaiJiang11;
    public UIButton KaiJiang13;
    public UIButton KaiJiang14;

    public UIButton FaWangTanPai;
    public UIButton WuZhaTanPai;

    // Use this for initialization
    void Start () {
        CreatBtn.onClick.Add(new EventDelegate(this.CreatRoom));
        TwoPlayer.onClick.Add(new EventDelegate(this.TwoPlayerClick));
        FourPlayer.onClick.Add(new EventDelegate(this.FourPlayerClick));
        FiveRound.onClick.Add(new EventDelegate(this.FiveRoundClick));
        TenRound.onClick.Add(new EventDelegate(this.TenRoundClick));

        JinDian.onClick.Add(new EventDelegate(this.JinDianClick));
        BaoPai.onClick.Add(new EventDelegate(this.BaoPaiClick));

        KaiJiang7.onClick.Add(new EventDelegate(this.KaiJiang7Click));
        KaiJiang11.onClick.Add(new EventDelegate(this.KaiJiang11Click));
        KaiJiang13.onClick.Add(new EventDelegate(this.KaiJiang13Click));
        KaiJiang14.onClick.Add(new EventDelegate(this.KaiJiang14Click));
        FaWangTanPai.onClick.Add(new EventDelegate(this.FaWangTanPaiClick));
        WuZhaTanPai.onClick.Add(new EventDelegate(this.WuZhaTanPaiClick));

        BaWangBtn.onClick.Add(new EventDelegate(this.BaWangClick));
        CloseBtn.onClick.Add(new EventDelegate(()=>
        {
            gameObject.SetActive(false);
        }));
    }
    bool reset = false;
    private void OnEnable()
    {
        roominfo = new CreateRoom();
        roominfo.IsBaWang = false;
        roominfo.ClubId =0;
        reset = true;
        FiveRoundClick();
        FourPlayerClick();
        JinDianClick();
        KaiJiang7Click();
        KaiJiang11Click();
       KaiJiang13Click();
       KaiJiang14Click();
       // BaWangClick();
       // FaWangTanPaiClick();
        WuZhaTanPaiClick();
        
        reset = false;
    }


    private void WuZhaTanPaiClick()
    {
        if (roominfo.PlayTypeIndex == 1)
        {
           
            return;
        } 
        if (WuZhaTanPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1")//勾选上的
        {
            if (reset) return;
            WuZhaTanPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.WuZhaTp = false;
        }
        else
        {
            FaWangTanPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.FaWangTp = false;

            WuZhaTanPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            roominfo.WuZhaTp = true;
        }
    }

    private void FaWangTanPaiClick()
    {
        if (roominfo.PlayTypeIndex == 1)
        {

            return;
        }
        if (FaWangTanPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1")//勾选上的
        {
            FaWangTanPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.FaWangTp = false;
        }
        else
        {
            WuZhaTanPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.WuZhaTp = false;

            FaWangTanPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            roominfo.FaWangTp = true;
        }
    }

    private void KaiJiang14Click()
    {
        if (KaiJiang14.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1")//勾选上的
        {
            if (reset) return;
            KaiJiang14.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.JiangMa.Remove(3);
        }
        else
        {
            KaiJiang14.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            roominfo.JiangMa.Add(3);
        }
    }

    private void KaiJiang13Click()
    {
        if (KaiJiang13.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1")//勾选上的
        {
            if (reset) return;
            KaiJiang13.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.JiangMa.Remove(2);
        }
        else
        {
            KaiJiang13.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            roominfo.JiangMa.Add(2);
        }
    }

    private void KaiJiang11Click()
    {
        if (KaiJiang11.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1")//勾选上的
        {
            if (reset) return;
            KaiJiang11.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.JiangMa.Remove(1);
        }
        else
        {
            KaiJiang11.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            roominfo.JiangMa.Add(1);
        }
    }

    private void KaiJiang7Click()
    {
        if (KaiJiang7.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1")//勾选上的
        {
            if (reset) return;
            KaiJiang7.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName ="UI_create_btn_check_2";
            roominfo.JiangMa.Remove(0);
        }
        else
        {
            KaiJiang7.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            roominfo.JiangMa.Add(0);
        }
    }

    private void BaoPaiClick()
    {
        if (roominfo.PlayerCountIndex == 0) return;//两人
        if (BaoPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1")//勾选上的
        {
            BaoPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";

        }
        else
        {
            BaWangBtn.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.IsBaWang = false;
            BaoPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            JinDian.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.PlayTypeIndex = 1;

            WuZhaTanPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.WuZhaTp = false;
            FaWangTanPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.FaWangTp = false;

        }
    }

    private void JinDianClick()
    {
        if (JinDian.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1")//勾选上的
        {
            //FiveRound.transform.FindChild("Sprite").transform.GetComponent<UISprite>().spriteName ="UI_create_btn_check_2";

        }
        else
        {
            BaWangBtn.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.IsBaWang = false;
            JinDian.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            BaoPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.PlayTypeIndex = 0;
        }
    }

    private void TwoPlayerClick()
    {
        if (TwoPlayer.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1")//勾选上的
        {
            //FiveRound.transform.FindChild("Sprite").transform.GetComponent<UISprite>().spriteName ="UI_create_btn_check_2";

        }
        else
        {
            FourPlayer.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            TwoPlayer.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            roominfo.PlayerCountIndex = 0;

            JinDian.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            BaoPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            BaWangBtn.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.IsBaWang = false;
            roominfo.PlayTypeIndex = 0;//两人不让包牌
        }
    }

    private void FourPlayerClick()
    {
        if (FourPlayer.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1")//勾选上的
        {
            //FiveRound.transform.FindChild("Sprite").transform.GetComponent<UISprite>().spriteName ="UI_create_btn_check_2";

        }
        else
        {
            FourPlayer.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            TwoPlayer.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.PlayerCountIndex = 1;
        }
    }

    private void FiveRoundClick()
    {
        if (FiveRound.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1")//勾选上的
        {
            //FiveRound.transform.FindChild("Sprite").transform.GetComponent<UISprite>().spriteName ="UI_create_btn_check_2";
           
        }
        else
        {
            TenRound.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            FiveRound.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            roominfo.PlayCountIndex = 0;
        }
    }

    private void TenRoundClick()
    {
        if (TenRound.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1")//勾选上的
        {
            //FiveRound.transform.FindChild("Sprite").transform.GetComponent<UISprite>().spriteName ="UI_create_btn_check_2";

        }
        else
        {
            FiveRound.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            TenRound.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";
            roominfo.PlayCountIndex = 1;
        }
    }

    private void BaWangClick()
    {
        if (BaWangBtn.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName == "UI_create_btn_check_1") return;
        roominfo.IsBaWang = !roominfo.IsBaWang;
        if (roominfo.IsBaWang)
        {
            BaWangBtn.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_1";//勾选上
            JinDian.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            BaoPai.transform.Find("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
            roominfo.PlayTypeIndex = 0;
        }
        else
        {
           // BaWangBtn.transform.FindChild("Sprite").transform.GetComponent<UISprite>().spriteName = "UI_create_btn_check_2";
        }
    }


    /// <summary>
    /// 创建房间
    /// </summary>
    private void CreatRoom()
    {
        //ClientToServerMsg.SendCreatRoom(roominfo);
        //return;

        if (GameData.PKClubInfoList.Count == 0)
        {
            GameData.ResultCodeStr = "请先申请加入俱乐部";

            UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
        }
        else if (DzViewMain.Instance.ChosedClubId == 0)
        {
            GameData.ResultCodeStr = "请先选择俱乐部";

            UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
        }
        else
        {
            roominfo.ClubId = DzViewMain.Instance.ChosedClubId;
            ClientToServerMsg.SendCreatRoom(roominfo);
        }
       
    }

    // Update is called once per frame
    void Update () {
		
	}
}
