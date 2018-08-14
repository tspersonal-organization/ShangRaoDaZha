using System.Collections.Generic;
using FrameworkForCSharp.NetWorks;
using UnityEngine;

public class DzPanelGameOverSmall : UIBase<DzPanelGameOverSmall>
{
    public UIButton ChangeTable; //换桌按钮
    public UIButton ContinueBtn;
    public List<GameObject> ItemList;
    public UIButton LeaveRoomBtn;

    public UIButton ShareBtn;

    private void OnEnable()
    {
        for (var i = 0; i < ItemList.Count; i++)
            ItemList[i].SetActive(false);
        Reset();
        SetInfo();
    }

    private void Start()
    {
        ShareBtn.onClick.Add(new EventDelegate(ShareResult));
        ContinueBtn.onClick.Add(new EventDelegate(GoOnGame));
        ChangeTable.onClick.Add(new EventDelegate(ChangTable));
        LeaveRoomBtn.onClick.Add(new EventDelegate(LeaveRoom));

        ShareBtn.gameObject.SetActive(true);
        ChangeTable.gameObject.SetActive(false);
        LeaveRoomBtn.gameObject.SetActive(false);
    }

    /// <summary>
    ///     离开房间申请
    /// </summary>
    private void LeaveRoom()
    {
        ClientToServerMsg.Send(Opcodes.Client_PlayerLeaveRoom, GameData.m_TableInfo.id, true);
    }

    /// <summary>
    ///     换桌
    /// </summary>
    private void ChangTable()
    {
        ClientToServerMsg.Send(Opcodes.Client_PiPei_ChangeDesk, (byte) GameData.GlobleRoomType, GameData.m_TableInfo.id,
            Input.location.lastData.latitude, Input.location.lastData.longitude);
    }

    /// <summary>
    ///     继续游戏
    /// </summary>
    private void GoOnGame()
    {
        if (!GameData.m_IsNormalOver)
            ClientToServerMsg.Send(Opcodes.Client_PlayerReady, GameData.m_TableInfo.id); //发送准备协议

        UIManager.Instance.HideUiPanel(UIPaths.PanelGameOverSmall);
    }

    /// <summary>
    ///     分享结果
    /// </summary>
    private void ShareResult()
    {
        //AuthorizeOrShare.Instance.ShareCapture();
    }

    public void Reset()
    {
        for (var i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].transform.Find("HeadSprite").Find("LandSprite").gameObject.SetActive(false);
            ItemList[i].transform.Find("HeadSprite").Find("HelperSprite").gameObject.SetActive(false);
        }
    }

    /// <summary>
    ///     设置值
    /// </summary>
    public void SetInfo()
    {
        for (var i = 0; i < PartGameOverControl.instance.SettleInfoList.Count; i++)
        {
            ItemList[i].SetActive(true);

            if (PartGameOverControl.instance.SettleInfoList[i].ChangeScore > 0)
                ItemList[i].transform.Find("LabScore").GetComponent<UILabel>().text =
                    "+" + PartGameOverControl.instance.SettleInfoList[i].ChangeScore;
            else
                ItemList[i].transform.Find("LabScore").GetComponent<UILabel>().text = PartGameOverControl
                    .instance.SettleInfoList[i].ChangeScore.ToString();

            ItemList[i].transform.Find("Name").GetComponent<UILabel>().text = GameDataFunc
                .GetPlayerInfo((byte) PartGameOverControl.instance.SettleInfoList[i].Pos).name;

            DownloadImage.Instance.Download(ItemList[i].transform.Find("HeadSprite").GetComponent<UITexture>(),
                GameDataFunc.GetPlayerInfo((byte) PartGameOverControl.instance.SettleInfoList[i].Pos).headID);
        }
    }
}