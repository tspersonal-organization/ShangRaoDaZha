public class DzPanelJoinMomentInvite : UIBase<DzPanelJoinMomentInvite>
{

    public UIButton SeruBtn;
    public UIButton MaskBtn;
    public UIInput input;

    void Start () {
        MaskBtn.onClick.Add(new EventDelegate(()=>
        {
            this.gameObject.SetActive(false);
        }));
        SeruBtn.onClick.Add(new EventDelegate(this.InvitePlayerJoinClub));
    }

    /// <summary>
    /// 邀请玩家加入俱乐部
    /// </summary>
    private void InvitePlayerJoinClub()
    {
        this.gameObject.SetActive(false);
        ClientToServerMsg.InvitePlayerJoinClub(ulong.Parse(input.value),(uint)GameData.CurrentClubInfo.Id);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
