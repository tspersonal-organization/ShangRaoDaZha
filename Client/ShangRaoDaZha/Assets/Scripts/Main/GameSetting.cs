using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : UIBase<GameSetting>
{
  
    public GameObject btnClose;
    public GameObject btnOperate;
    public UIButton CloseBtn;

    public UISlider MusicSlider;
    public UISlider SoundEffectSlider;

  

    // public UITexture ImgHead;
    // public UILabel LBName;
    // public UILabel LBGuid;

    // Use this for initialization
    void Start ()
    {
       
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
      
        //LBName.text = Player.Instance.otherName;
        //LBGuid.text = "ID:"+Player.Instance.guid;
        //DownloadImage.Instance.Download(ImgHead, Player.Instance.headID);
        //if (Game.Instance != null)
        //{
        //    btnOperate.GetComponent<UISprite>().spriteName =  GameData.m_TableInfo.roomState == FrameworkForCSharp.Utils.RoomStatusType.None ? "UI_setting_btn_Leave" : "UI_setting_btn_dismiss";
        //}

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
        UIManager.Instance.HideUiPanel(UIPaths.PanelSetting);
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
         if(go == btnClose)
        {
            UIManager.Instance.HideUiPanel(UIPaths.PanelSetting);
        }
        else if(go == btnOperate)
        {
            Player.Instance.Logout();
           
        }
    }
	
}
