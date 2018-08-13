using UnityEngine;

public class DzPanelShare : UIBase<DzPanelShare>
{
    public GameObject BtnBgMask;
    public GameObject BtnClose;
    public GameObject BtnWeChat;
    public GameObject BtnWeChatMoment;

    private void Start()
    {
        UIEventListener.Get(BtnBgMask).onClick = OnClickBtnClose;
        UIEventListener.Get(BtnClose).onClick = OnClickBtnClose;
        UIEventListener.Get(BtnWeChat).onClick = OnClickBtnWeChat;
        UIEventListener.Get(BtnWeChatMoment).onClick = OnClickBtnWeChatMoment;
    }

    private void OnClickBtnClose(GameObject go)
    {
        UIManager.Instance.HideUiPanel(UIPaths.PanelShare);
    }

    private void OnClickBtnWeChat(GameObject go)
    {
        GameData.Tips = "该功能暂未开放！";
        UIManager.Instance.ShowUiPanel(UIPaths.PanelTips, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void OnClickBtnWeChatMoment(GameObject go)
    {
        GameData.Tips = "该功能暂未开放！";
        UIManager.Instance.ShowUiPanel(UIPaths.PanelTips, OpenPanelType.MinToMax);
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
}
