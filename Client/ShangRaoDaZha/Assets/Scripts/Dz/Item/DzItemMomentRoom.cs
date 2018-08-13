using FrameworkForCSharp.NetWorks;
using System.Collections.Generic;
using UnityEngine;

public class DzItemMomentRoom : MonoBehaviour
{
    public UIButton RoomItemBtn;
    public List<UITexture> PlayreHeadList;
    public UILabel RoomidLable;
    public UILabel RoundCountLable;
    public UIButton OperateBtn;

    private bool IsPlaying = false;
    PKClubRoomInfo InfoData;

    void Start()
    {
        RoomItemBtn.onClick.Add(new EventDelegate(this.ItemClick));
        OperateBtn.onClick.Add(new EventDelegate(this.Invite));
    }


    private void ItemClick()
    {
        if (!IsPlaying)
        {
            ClientToServerMsg.Send(Opcodes.Client_PlayerEnterRoom, InfoData.codeId, Input.location.lastData.latitude, Input.location.lastData.longitude);
        }

    }
    private void Invite()
    {
        GameData.Tips = "该功能暂未开放！";
        UIManager.Instance.ShowUiPanel(UIPaths.PanelTips, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    public void SetValue(PKClubRoomInfo info)
    {
        InfoData = info;
        RoomidLable.text = info.codeId.ToString();
        RoundCountLable.text = info.playerCount + "人/" + info.gameCount + "局/" + info.playType;
        for (int i = 0; i < info.PKClubPlayerInfoList.Count; i++)
        {
            PlayreHeadList[i].gameObject.SetActive(true);
            DownloadImage.Instance.Download(PlayreHeadList[i], info.PKClubPlayerInfoList[i].HeadId);
        }
        if (info.playerCount == info.PKClubPlayerInfoList.Count)
        {
            IsPlaying = true;
            //游戏正在进行中
        }

    }
}
