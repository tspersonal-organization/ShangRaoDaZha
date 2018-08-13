using System.Collections.Generic;
using UnityEngine;

public class DzPanelGameOverBig : UIBase<DzPanelGameOverBig>
{
    public List<GameObject> ItemList;
    public GameObject ItemOnceHistory;//一局的结算数据
    public Transform ItemOnceHistoryBase;//一局的结算数据父对象
    public List<GameObject> ListItemOnceHistory;

    public UIButton ShareBtn;
    public UIButton ContinueBtn;

    void Start()
    {
        ShareBtn.onClick.Add(new EventDelegate(this.ShareResult));
        ContinueBtn.onClick.Add(new EventDelegate(this.GoOnGame));
 
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    private void GoOnGame()
    {
        ManagerScene.Instance.LoadScene(SceneType.Main);

        UIManager.Instance.HideUiPanel(UIPaths.PanelGameOverSmall);
    }

    /// <summary>
    /// 分享结果
    /// </summary>
    private void ShareResult()
    {
        AuthorizeOrShare.Instance.ShareCapture();
    }


    private void OnEnable()
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].SetActive(false);
        }
        ListItemOnceHistory = new List<GameObject>();
        for (var i = 0; i < ItemOnceHistoryBase.childCount; i++)
        {
            ListItemOnceHistory.Add(ItemOnceHistoryBase.GetChild(i).gameObject);
        }
        SetInfo();
    }
    // Update is called once per frame
    void Update()
    {

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
        for (int i = 0; i < PartGameOverControl.instance.TotalGameOverInfoList.Count; i++)
        {
            ItemList[i].gameObject.SetActive(true);
            ItemList[i].transform.Find("PlayerNameLabel").GetComponent<UILabel>().text = GameDataFunc
                .GetPlayerInfo((byte) PartGameOverControl.instance.TotalGameOverInfoList[i].pos).name.ToString();
            ItemList[i].transform.Find("ChangeScoreLabel").GetComponent<UILabel>().text =
                (PartGameOverControl.instance.TotalGameOverInfoList[i].score).ToString();


            DownloadImage.Instance.Download(ItemList[i].transform.Find("HeadSprite").GetComponent<UITexture>(),
                GameDataFunc.GetPlayerInfo((byte) PartGameOverControl.instance.TotalGameOverInfoList[i].pos).headID);

        }
        //设置单局数据
        for (var i = 0; i < PartGameOverControl.instance.ListGameOverSmall.Count; i++)
        {
            GameObject go;
            if (i < ListItemOnceHistory.Count)
            {
                go = ListItemOnceHistory[i];
            }
            else
            {
                go = Instantiate(ItemOnceHistory, ItemOnceHistoryBase, false);
                ListItemOnceHistory.Add(go);
            }
            go.SetActive(true);
            go.transform.Find("LabJuShu").GetComponent<UILabel>().text = "第" + (i + 1) + "局";
            Transform tPlayer = go.transform.Find("Player");
            for (var j = 0; j < tPlayer.childCount; j++)
            {
                if (j < PartGameOverControl.instance.ListGameOverSmall[i].Count)
                {
                    tPlayer.GetChild(j).gameObject.SetActive(true);
                    tPlayer.GetChild(j).Find("LabName").GetComponent<UILabel>().text = GameDataFunc
                        .GetPlayerInfo((byte) PartGameOverControl.instance.ListGameOverSmall[i][j].pos).name;
                    int score = PartGameOverControl.instance.ListGameOverSmall[i][j].HuiHeFen;
                    UILabel labScore = tPlayer.GetChild(j).Find("LabScore").GetComponent<UILabel>();
                    if (score > 0)
                    {
                        labScore.text = "+" + score;
                    }
                    else
                    {
                        labScore.text = score.ToString();
                    }
                }
                else
                {
                    tPlayer.GetChild(j).gameObject.SetActive(false);
                }
            }
            var data = PartGameOverControl.instance.ListGameOverSmall[i];
            go.transform.Find("BtnLook").GetComponent<UIButton>().onClick.Add(new EventDelegate(delegate
            {
                PartGameOverControl.instance.SettleInfoList = data;
                UIManager.Instance.ShowUiPanel(UIPaths.PanelGameOverSmall);
            }));
        }

    }
}
