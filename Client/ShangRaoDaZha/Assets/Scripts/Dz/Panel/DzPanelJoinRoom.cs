using FrameworkForCSharp.NetWorks;
using System.Collections.Generic;
using UnityEngine;

public class DzPanelJoinRoom : UIBase<UICreateRoom>
{
    public GameObject MaskObj;
    public UIButton CloseBtn;
    public UIButton ClearBtn;
    public UIButton BackNumBtn;


    public List<UILabel> RoomLableList;

    
    // Use this for initialization
    void Start () {
        SetNumShow();
        ClearBtn.onClick.Add(new EventDelegate(ClearNum));
        BackNumBtn.onClick.Add(new EventDelegate(BackNum));
        UIEventListener.Get(MaskObj).onClick = this.Close;
        CloseBtn.onClick.Add(new EventDelegate(this.CloseClick));

    }

    /// <summary>
    /// 返回撤销
    /// </summary>
    private void BackNum()
    {
        string newstr = "";
        for (int i = 0; i < _num.Length-1; i++)
        {
            newstr += _num[i];
        }
        num = newstr;
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    /// <summary>
    /// 清除
    /// </summary>
    private void ClearNum()
    {
        num = "";
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    private void CloseClick()
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        UIManager.Instance.HideUiPanel(UIPaths.PanelJoinRoom);
    }

    private void Close(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        CloseClick();
    }

    // Update is called once per frame
    void Update () {
		
	}

    string _num = "";
    string num
    {
        get
        {
            return _num;
                
                }
        set {
            _num= value ;
            SetNumShow();
                }
    }
    /// <summary>
    /// 数字点击
    /// </summary>
    /// <param name="str"></param>
    public void Btn0Click()
    {
        num += 0.ToString();
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    public void Btn1Click()
    {
        num += 1.ToString();
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    public void Btn2Click()
    {
        num += 2.ToString();
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    public void Btn3Click()
    {
        num +=3.ToString();
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    public void Btn4Click()
    {
        num += 4.ToString();
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    public void Btn5Click()
    {
        num +=5.ToString();
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    public void Btn6Click()
    {
        num += 6.ToString();
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    public void Btn7Click()
    {
        num +=7.ToString();
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    public void Btn8Click()
    {
        num +=8.ToString();
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }
    public void Btn9Click()
    {
        num +=9.ToString();
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
    }

    /// <summary>
    /// 显示房间号
    /// </summary>
    public void SetNumShow()
    {
        for (int i = 0; i < RoomLableList.Count; i++)
        {
            if (i < num.Length)
            {
                RoomLableList[i].text = num[i].ToString();
            }
            else
            {
                RoomLableList[i].text = "";
            }
          
        }

        if (num.Length == 6)
        {
            ClientToServerMsg.Send(Opcodes.Client_PlayerEnterRoom, uint.Parse(_num), Input.location.lastData.latitude, Input.location.lastData.longitude);
            UIManager.Instance.HideUiPanel(UIPaths.PanelJoinRoom);
        }
    }
}
