using FrameworkForCSharp.NetWorks;
using UnityEngine;

public class DzViewLogin : UIBase<DzViewLogin>
{
    public GameObject btnLogin;

    public UIButton UserInfoContentBtn;//用户信息
    public GameObject UserInfoContent;//用户信息
    public UIButton CloseBtn;//关闭按钮
    public UIButton ChoseBtn;//关闭按钮


    public GameObject QuickLogin;



    void Start()
    {
        transform.Find("Version").GetComponent<UILabel>().text = "版本号:" + Application.version;
        UIEventListener.Get(btnLogin).onClick = OnClick;
        UIEventListener.Get(QuickLogin).onClick = OnClick;
        if (ServerInfo.Data.login_with_device && Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //btnLogin.GetComponent<UISprite>().spriteName = "UI_login_btn_FastLogin";
            QuickLogin.SetActive(true);
            btnLogin.SetActive(false);
        }

        else
        {
            QuickLogin.SetActive(false);
            btnLogin.SetActive(true);


            if (Player.Instance.openID != "n")
            {
                btnLogin.gameObject.SetActive(false);
                ClientToServerMsg.Send(Opcodes.Client_Character_Create, Player.Instance.openID, Player.Instance.otherName, Player.Instance.headID, (byte)Player.Instance.sex);
            }

        }

        UserInfoContentBtn.onClick.Add(new EventDelegate(this.ShowUserInfoPanel));
        CloseBtn.onClick.Add(new EventDelegate(this.CloseUserInfo));
        ChoseBtn.onClick.Add(new EventDelegate(this.ChoseUserInfo));
    }

    //选择是否接收
    private void ChoseUserInfo()
    {
        ChoseBtn.transform.Find("Sprite").gameObject.SetActive(!ChoseBtn.transform.Find("Sprite").gameObject.activeSelf);
    }

    //展现用户协议
    private void ShowUserInfoPanel()
    {
        UserInfoContent.SetActive(true);
    }

    //关闭用户协议
    private void CloseUserInfo()
    {
        UserInfoContent.SetActive(false);
    }

    public void OnClick(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        if (!ChoseBtn.transform.Find("Sprite").gameObject.activeSelf)
        {
            return;
        }

        //Player.Instance.openID = "ouf9U0_JKF32uu8EgZrvGtkL_yyI";// SystemInfo.deviceUniqueIdentifier;
        //Player.Instance.otherName = Player.Instance.openID.Substring(0, 6);
        //Player.Instance.headID = "headid";
        //Player.Instance.sex = 1;
        //ClientToServerMsg.Send(Opcodes.Client_Character_Create, Player.Instance.openID, Player.Instance.otherName, Player.Instance.headID, (byte)Player.Instance.sex);
        //return;

        if (go == btnLogin)
        {

            AuthorizeOrShare.Instance.Authorize();
            //if (ServerInfo.Data.login_with_device)
            //{
            //    Player.Instance.openID = SystemInfo.deviceUniqueIdentifier;
            //    Player.Instance.otherName = SystemInfo.deviceUniqueIdentifier.Substring(0, 6);
            //    Player.Instance.headID = "headid";
            //    Player.Instance.sex = 1;
            //    ClientToServerMsg.Send(Opcodes.Client_Character_Create, Player.Instance.openID, Player.Instance.otherName, Player.Instance.headID, (byte)Player.Instance.sex);
            //}
            //else
            //{
            //    AuthorizeOrShare.Instance.Authorize();
            //    Player.Instance.openID = SystemInfo.deviceUniqueIdentifier;
            //    Player.Instance.otherName = SystemInfo.deviceUniqueIdentifier.Substring(0, 6);
            //    Player.Instance.headID = "headid";
            //    Player.Instance.sex = 1;
            //    ClientToServerMsg.Send(Opcodes.Client_Character_Create, Player.Instance.openID, Player.Instance.otherName, Player.Instance.headID, (byte)Player.Instance.sex);
            //}
        }
        if (go == QuickLogin)
        {
            AuthorizeOrShare.Instance.QuickLog();
        }
    }

}
