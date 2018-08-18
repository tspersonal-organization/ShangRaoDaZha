using UnityEngine;

public class DzItemMessage : MonoBehaviour {


    public UITexture HeadTexture;
    public UILabel DescLable;
    public UIButton AgreeBtn;
    public UIButton RefuseBtn;

    private MemInfo infoData;
    private bool Invite = false;//是否为邀请信息

    void Start ()
    {
        AgreeBtn.onClick.Add(new EventDelegate(this.AgreeBtnClick));
        RefuseBtn.onClick.Add(new EventDelegate(this.RefuseBtnClick));
    }

    
    private void RefuseBtnClick()
    {
        if (Invite)//是邀请信息
        {
            ClientToServerMsg.OperateInviteMessage(infoData.ClubId,false);
            GameData.CurrentClubInfo.InviteList.Remove(infoData);
        }
        else
        {
            ClientToServerMsg.OperatePlayerApply(infoData.ClubId, infoData.Guid,false);
            GameData.CurrentClubInfo.ApplyMemList.Remove(infoData);
        }
    }

    private void AgreeBtnClick()
    {
        if (Invite)//是邀请信息
        {
            ClientToServerMsg.OperateInviteMessage(infoData.ClubId, true);
            GameData.CurrentClubInfo.InviteList.Remove(infoData);
        }
        else
        {
            ClientToServerMsg.OperatePlayerApply(infoData.ClubId, infoData.Guid, true);
            GameData.CurrentClubInfo.ApplyMemList.Remove(infoData);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="IsInvite">是否为邀请信息</param>
    public void SetValue(MemInfo info,bool IsInvite)
    {
        Invite = IsInvite;
        this.infoData = info;
        DownloadImage.Instance.Download(HeadTexture, info.HeadId);
        if (Invite)//是邀请信息
        {
            DescLable.text = info.Name + " 邀请你加入 " + info.ClubName;
        }
        else
        {
            DescLable.text = info.Name + " 申请加入俱乐部 " ;
        }

    }
}
