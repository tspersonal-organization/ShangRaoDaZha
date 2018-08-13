using FrameworkForCSharp.NetWorks;
using System.Collections.Generic;
using UnityEngine;

public class DzPanelGameOverSmall : UIBase<DzPanelGameOverSmall>
{
    public List<GameObject> ItemList;

    public UIButton ShareBtn;
    public UIButton ContinueBtn;


    public UIButton LeaveRoomBtn;
    public UIButton ChangeTable;//换桌按钮

    public GameObject CardObj;//牌预制体
    Dictionary<int, List<GameObject>> PosAndCardList = new Dictionary<int, List<GameObject>>();

    public Dictionary<int, List<List<GameObject>>> PosTaoShangObjList = new Dictionary<int, List<List<GameObject>>>();//讨赏牌列表
    // Use this for initialization
    void Start () {
        ShareBtn.onClick.Add(new EventDelegate(this.ShareResult));
        ContinueBtn.onClick.Add(new EventDelegate(this.GoOnGame));
        ChangeTable.onClick.Add(new EventDelegate(this.ChangTable));
        LeaveRoomBtn.onClick.Add(new EventDelegate(this.LeaveRoom));
        if (GameData.m_TableInfo.IsPiPei)
        {
            ShareBtn.gameObject.SetActive(false);
            ChangeTable.gameObject.SetActive(true);
            LeaveRoomBtn.gameObject.SetActive(true);
        }
        else
        {
            ShareBtn.gameObject.SetActive(true);
            ChangeTable.gameObject.SetActive(false);
            LeaveRoomBtn.gameObject.SetActive(false);
        }

        
    }

    private void OnEnable()
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].SetActive(false);
        }
        Reset();
        SetInfo();
    }
    /// <summary>
    /// 离开房间申请
    /// </summary>
    private void LeaveRoom()
    {
        ClientToServerMsg.Send(Opcodes.Client_PlayerLeaveRoom, GameData.m_TableInfo.id, true);
    }

    /// <summary>
    /// 换桌
    /// </summary>
    private void ChangTable()
    {
       // ManagerScene.Instance.LoadScene(SceneType.Main);
        ClientToServerMsg.Send(Opcodes.Client_PiPei_ChangeDesk, (byte)GameData.GlobleRoomType, GameData.m_TableInfo.id, Input.location.lastData.latitude, Input.location.lastData.longitude);
    }

    protected override void OnDestroy()
    {
        if (GameData.m_TableInfo.configPlayerIndex == 4)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {

                ItemList[i].transform.Find("outCount").Find("ShangSprite1").gameObject.SetActive(false);
                ItemList[i].transform.Find("outCount").Find("ShangSprite2").gameObject.SetActive(false);
                ItemList[i].transform.Find("outCount").Find("ShangSprite3").gameObject.SetActive(false);
                ItemList[i].transform.Find("outCount").Find("ShangSprite4").gameObject.SetActive(false);
                for (int j = 0; j < PosAndCardList[i + 1].Count; j++)
                {
                    Destroy(PosAndCardList[i + 1][j]);
                }
                PosAndCardList[i + 1] = new List<GameObject>();
            }
        }
        else if (GameData.m_TableInfo.configPlayerIndex == 2)
        {
            for (int i = 0; i < 2; i++)
            {

                ItemList[i].transform.Find("outCount").Find("ShangSprite1").gameObject.SetActive(false);
                ItemList[i].transform.Find("outCount").Find("ShangSprite2").gameObject.SetActive(false);
                ItemList[i].transform.Find("outCount").Find("ShangSprite3").gameObject.SetActive(false);
                ItemList[i].transform.Find("outCount").Find("ShangSprite4").gameObject.SetActive(false);
                for (int j = 0; j < PosAndCardList[i + 1].Count; j++)
                {
                    Destroy(PosAndCardList[i + 1][j]);
                }
                PosAndCardList[i + 1] = new List<GameObject>();
            }
        }
       

        //销毁生成的讨赏牌
        for (int i = 1; i < PosTaoShangObjList.Count+1; i++)
        {
            for (int j = 0; j < PosTaoShangObjList[i].Count; j++)
            {
                for (int k = 0; k < PosTaoShangObjList[i][j].Count; k++)
                {
                    Destroy(PosTaoShangObjList[i][j][k]);
                }
            }
        }
        base.OnDestroy();


    }
    /// <summary>
    /// 继续游戏
    /// </summary>
    private void GoOnGame()
    {
        if (!GameData.m_IsNormalOver)
        {
            ClientToServerMsg.Send(Opcodes.Client_PlayerReady, GameData.m_TableInfo.id);//发送准备协议
        }
        else
        {
          //  UIManager.Instance.ShowUiPanel(UIPaths.UIPanel_TotalScore, OpenPanelType.MinToMax);
          //  ManagerScene.Instance.LoadScene(SceneType.Main);
        }
      
        UIManager.Instance.HideUiPanel(UIPaths.PanelGameOverSmall);
    }

    /// <summary>
    /// 分享结果
    /// </summary>
    private void ShareResult()
    {
        AuthorizeOrShare.Instance.ShareCapture();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Reset()
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].transform.Find("HeadSprite").Find("LandSprite").gameObject.SetActive(false);
            ItemList[i].transform.Find("HeadSprite").Find("HelperSprite").gameObject.SetActive(false);
          
        }
    }


    /// <summary>
    /// 设置值
    /// </summary>
    public void SetInfo()
    {
        for (int i = 0; i < PartGameOverControl.instance.SettleInfoList.Count; i++)
        {
            ItemList[i].SetActive(true);
            if (PartGameOverControl.instance.ZhuangPos == PartGameOverControl.instance.SettleInfoList[i].pos)
            {
              //  ItemList[i].transform.FindChild("HeadSprite").FindChild("LandSprite").gameObject.SetActive(true);
               // ItemList[i].transform.FindChild("HeadSprite").FindChild("").gameObject.SetActive(true);
            }
            if (PartGameOverControl.instance.HelperPos == PartGameOverControl.instance.SettleInfoList[i].pos)
            {
               // ItemList[i].transform.FindChild("HeadSprite").FindChild("HelperSprite").gameObject.SetActive(true);
                // ItemList[i].transform.FindChild("HeadSprite").FindChild("").gameObject.SetActive(true);
            }
            ItemList[i].transform.Find("PlayerNameLabel").GetComponent<UILabel>().text = GameDataFunc.GetPlayerInfo((byte)PartGameOverControl.instance.SettleInfoList[i].pos).name.ToString();
            if (!GameData.m_TableInfo.IsPiPei)//是否为匹配场
            {
                if (PartGameOverControl.instance.SettleInfoList[i].HuiHeFen > 0)
                {
                    ItemList[i].transform.Find("PartScoreLabel").GetComponent<UILabel>().text = "+" + PartGameOverControl.instance.SettleInfoList[i].HuiHeFen.ToString();
                }
                else
                {
                    ItemList[i].transform.Find("PartScoreLabel").GetComponent<UILabel>().text = PartGameOverControl.instance.SettleInfoList[i].HuiHeFen.ToString();
                }
                ItemList[i].transform.Find("TotalScoreLabel").GetComponent<UILabel>().text = "炸弹分：" + PartGameOverControl.instance.SettleInfoList[i].ZhaDanScore.ToString();
                ItemList[i].transform.Find("FWScoreLabel").GetComponent<UILabel>().text = "罚分：" + PartGameOverControl.instance.SettleInfoList[i].FaWangScore.ToString();

                ItemList[i].transform.Find("TotalPartScore").Find("Label").GetComponent<UILabel>().text = (PartGameOverControl.instance.SettleInfoList[i].ChangeScore).ToString();
                ItemList[i].transform.Find("CurrentScore").Find("Label").GetComponent<UILabel>().text = (PartGameOverControl.instance.SettleInfoList[i].Score).ToString();
              
            }
            else
            {
                if (PartGameOverControl.instance.SettleInfoList[i].HuiHeFen > 0)
                {
                    ItemList[i].transform.Find("PartScoreLabel").GetComponent<UILabel>().text = "+" + (PartGameOverControl.instance.SettleInfoList[i].HuiHeFen * JinBiDataControl.Instance.TaoSHangRate);
                }
                else
                {
                    ItemList[i].transform.Find("PartScoreLabel").GetComponent<UILabel>().text =
                    (PartGameOverControl.instance.SettleInfoList[i].HuiHeFen *
                     JinBiDataControl.Instance.TaoSHangRate).ToString();
                }
                ItemList[i].transform.Find("TotalScoreLabel").GetComponent<UILabel>().text = "炸弹分：" + (PartGameOverControl.instance.SettleInfoList[i].ZhaDanScore * JinBiDataControl.Instance.TaoSHangRate).ToString();
                ItemList[i].transform.Find("FWScoreLabel").GetComponent<UILabel>().text = "罚分：" + PartGameOverControl.instance.SettleInfoList[i].FaWangScore.ToString();

                ItemList[i].transform.Find("TotalPartScore").Find("Label").GetComponent<UILabel>().text = (PartGameOverControl.instance.SettleInfoList[i].ChangeScore).ToString();
               // ItemList[i].transform.FindChild("TotalPartScore").FindChild("Label").GetComponent<UILabel>().text = (PartGameOverControl.instance.SettleInfoList[i].ChangeScore * JinBiDataControl.Instance.TaoSHangRate).ToString();
                ItemList[i].transform.Find("CurrentScore").Find("Label").GetComponent<UILabel>().text = (PartGameOverControl.instance.SettleInfoList[i].Gold).ToString();
            }

            //生成剩余手牌
            PosAndCardList[PartGameOverControl.instance.SettleInfoList[i].pos] = new List<GameObject>();
            PartGameOverControl.instance.SettleInfoList[i].LeftCardList = CardTools.CardValueSort(PartGameOverControl.instance.SettleInfoList[i].LeftCardList);

            //生成打出的讨赏牌
            PosTaoShangObjList[PartGameOverControl.instance.SettleInfoList[i].pos] = new List<List<GameObject>>(); 
        }

        for (int i = 0; i < PartGameOverControl.instance.SettleInfoList.Count; i++)
        {
            DownloadImage.Instance.Download(ItemList[i].transform.Find("HeadSprite").GetComponent<UITexture>(), GameDataFunc.GetPlayerInfo((byte)PartGameOverControl.instance.SettleInfoList[i].pos).headID);
        }
        }
}
