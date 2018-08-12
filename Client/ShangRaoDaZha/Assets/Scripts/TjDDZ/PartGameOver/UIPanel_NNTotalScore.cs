/*
* 开发人：滕俊
* 项目：
* 功能描述：
*
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel_NNTotalScore : UIBase<UIPanel_NNTotalScore>
{
    public List<GameObject> ItemList;

    public UIButton ShareBtn;
    public UIButton ContinueBtn;
    // Use this for initialization
    void Start()
    {
        ShareBtn.onClick.Add(new EventDelegate(this.ShareResult));
        ContinueBtn.onClick.Add(new EventDelegate(this.GoOnGame));

        // Reset();
       // SetInfo();
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    private void GoOnGame()
    {
        ManagerScene.Instance.LoadScene(SceneType.Main);

        UIManager.Instance.HideUIPanel(UIPaths.GameOverPanel);
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
            ItemList[i].transform.Find("PlayerNameLabel").GetComponent<UILabel>().text = GameDataFunc.GetPlayerInfo((byte)PartGameOverControl.instance.TotalGameOverInfoList[i].pos).name.ToString();
            ItemList[i].transform.Find("ChangeScoreLabel").GetComponent<UILabel>().text = (PartGameOverControl.instance.TotalGameOverInfoList[i].score).ToString();


            DownloadImage.Instance.Download(ItemList[i].transform.Find("HeadSprite").GetComponent<UITexture>(), GameDataFunc.GetPlayerInfo((byte)PartGameOverControl.instance.TotalGameOverInfoList[i].pos).headID);

        }
    }
}
