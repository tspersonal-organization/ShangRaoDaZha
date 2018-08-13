using FrameworkForCSharp.NetWorks;
using UnityEngine;

public class DzPanelSetting2 : UIBase<DzPanelSetting2>
{

    public GameObject btnClose;
    public GameObject btnOperate;
    public UIButton CloseBtn;

    public UISlider MusicSlider;
    public UISlider SoundEffectSlider;

    public UIButton DisposeRoomBtn;//解散房间按钮

    // public UITexture ImgHead;
    // public UILabel LBName;
    // public UILabel LBGuid;

    // Use this for initialization
    void Start()
    {

        DisposeRoomBtn.onClick.Add(new EventDelegate(() =>
        {
            // ClientToServerMsg.SendDisPosRoom(GameData.m_TableInfo.id);

            ClientToServerMsg.Send(Opcodes.Client_PlayerLeaveRoom, GameData.m_TableInfo.id, true);
            gameObject.SetActive(false);
        }));
        UIEventListener.Get(btnClose).onClick = OnClick;
        UIEventListener.Get(btnOperate).onClick = OnClick;
        CloseBtn.onClick.Add(new EventDelegate(this.Close));

        if (Player.Instance.GameBGSoundOff)
        {
            MusicSlider.value = Player.Instance.GameBGSoundValue;
        }
        else
        {
            MusicSlider.value = 0;
        }

        if (Player.Instance.GameEffectSoundOff)
        {
            SoundEffectSlider.value = Player.Instance.GameEffectSoundValue;
        }
        else
        {
            SoundEffectSlider.value = 0;
        }

        MusicSlider.onChange.Add(new EventDelegate(this.MusicValueChange));

        SoundEffectSlider.onChange.Add(new EventDelegate(this.SoundValueChange));

        //UIEventListener.Get(btnClose).onClick = OnClick;
        //UIEventListener.Get(btnOperate).onClick = OnClick;
        //CloseBtn.onClick.Add(new EventDelegate(this.Close));
        //MusicSlider.onChange.Add(new EventDelegate(this.MusicValueChange));
        //MusicSlider.value = Player.Instance.GameBGSoundValue;
        //SoundEffectSlider.onChange.Add(new EventDelegate(this.SoundValueChange));
        //SoundEffectSlider.value = Player.Instance.GameEffectSoundValue;


    }


    /// <summary>
    /// 音效的改变
    /// </summary>
    private void SoundValueChange()
    {

        if (SoundEffectSlider.value != 0)
        {
            Player.Instance.GameEffectSoundOff = true;
            Player.Instance.GameEffectSoundValue = SoundEffectSlider.value;
        }
        else
        {
            Player.Instance.GameEffectSoundOff = false;
        }
    }


    /// <summary>
    /// 音乐的改变
    /// </summary>
    private void MusicValueChange()
    {

        if (MusicSlider.value != 0)
        {
            Player.Instance.GameBGSoundOff = true;
            Player.Instance.GameBGSoundValue = MusicSlider.value;
        }
        else
        {
            Player.Instance.GameBGSoundOff = false;
        }
        SoundManager.Instance.SetBGVolume();
    }

    private void Close()
    {
        UIManager.Instance.HideUiPanel(UIPaths.PanelSetting2);
    }



    void OnClick(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.BUTTONCLICK);
        //if (go == btnBGSoundOff)
        //{
        //    Player.Instance.GameBGSoundOff = !Player.Instance.GameBGSoundOff;
        //    ChangeBGOff();
        //}
        //else if(go == btnEffectSoundOff)
        //{
        //    Player.Instance.GameEffectSoundOff = !Player.Instance.GameEffectSoundOff;
        // ChangeEffectOff();
        //}
        if (go == btnClose)
        {
            UIManager.Instance.HideUiPanel(UIPaths.PanelSetting2);
        }
        else if (go == btnOperate)
        {
            if (DzViewMain.Instance != null)
            {
                Player.Instance.Logout();
            }
            else
            {
                ClientToServerMsg.Send(FrameworkForCSharp.NetWorks.Opcodes.Client_DisposeRoom, GameData.m_TableInfo.id);
            }
        }
    }

}
