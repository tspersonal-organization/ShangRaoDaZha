using cn.sharesdk.unity3d;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using FrameworkForCSharp.NetWorks;

public class AuthorizeOrShare : UIBase<AuthorizeOrShare>
{

    //分享功能的名字   用于分享成功后识别哪个功能调用的分享
    public static string shareFunctionName= "";

    //颜值识别分享功能 name
    public const string SFN_DETECTFACE = "SFN_DETECTFACE";

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            InitSDK();
        }
    }

    #region 微信授权登录
    [HideInInspector]
    public ShareSDK ssdk;

    void InitSDK()
    {
        ssdk = gameObject.GetComponent<ShareSDK>();
        ssdk.authHandler = OnAuthResultHandler;
        ssdk.showUserHandler = OnGetUserInfoResultHandler;
        ssdk.shareHandler = OnShareResultHandler;
    }

    /// <summary>
    /// 分享截屏
    /// </summary>
    public void ShareCapture( PlatformType[] aPlatforms = null  )
    {
        ShareContent content = new ShareContent();
        ScreenCapture.CaptureScreenshot("Shot4Share.png");
        //设置图片路径
        content.SetImagePath(Application.persistentDataPath + "/Shot4Share.png");
        content.SetShareType(ContentType.Image);

        //ssdk.ShowShareContentEditor(PlatformType.WeChat, content);
        if( aPlatforms == null ){
            aPlatforms = new PlatformType[] { PlatformType.WeChat, PlatformType.WeChatMoments };
        }
        GameData.ResultCodeStr = "平台数量"+aPlatforms.Length.ToString();
        ssdk.ShowPlatformList(aPlatforms, content, 100, 100);
    }

    /// <summary>
    /// 分享房间号
    /// </summary>
    /// <param name="roomID"></param>
    ///  <param name="titlePrefix">标题前缀</param>
    public void ShareRoomID(uint roomID, string text, string titlePrefix)
    {
        ShareContent content = new ShareContent();
      
        content.SetTitle(titlePrefix + "房间号:" + roomID.ToString());
      
        content.SetText(text);
        ToolsFunc.SetShareIcon();
      
        content.SetImagePath(Application.persistentDataPath + "/icon.png");
      
        content.SetUrl(Player.Instance.shareUrl + roomID.ToString());
        content.SetShareType(ContentType.Webpage);
        ssdk.ShowPlatformList(new PlatformType[] { PlatformType.WeChat, PlatformType.WeChatMoments }, content, 100, 100);
      
    }
    /// <summary>
    /// 分享链接
    /// </summary>
    /// <param name="roomID"></param>
    /// <param name="text"></param>
    /// <param name="titlePrefix"></param>
    public void ShareURL(string text, string titlePrefix,string url)
    {
        ShareContent content = new ShareContent();
        content.SetTitle(titlePrefix);
        content.SetText(text);
        ToolsFunc.SetShareIcon();
        content.SetImagePath(Application.persistentDataPath + "/icon.png");
        content.SetUrl(url);
        content.SetShareType(ContentType.Webpage);
        ssdk.ShowPlatformList(new PlatformType[] { PlatformType.WeChat, PlatformType.WeChatMoments }, content, 100, 100);
    }
    /// <summary>
    /// 分享图片URL
    /// </summary>
    /// <param name="text"></param>
    /// <param name="imageURL"></param>
    public void ShareImageURL(string text, string imageURL)
    {
        ShareContent content = new ShareContent();
        content.SetTitle(text);
        content.SetImageUrl(imageURL);
        content.SetShareType(ContentType.Image);
        ssdk.ShowPlatformList(new PlatformType[] { PlatformType.WeChat, PlatformType.WeChatMoments }, content, 100, 100);
    }
    /// <summary>
    /// 登录授权
    /// </summary>
    public void Authorize()
    {

        Player.Instance.openID = SystemInfo.deviceUniqueIdentifier;//"4a14a16a941f3eeade18ba90e009f960";//SystemInfo.deviceUniqueIdentifier;//
        Player.Instance.otherName = Player.Instance.openID.Substring(0, 6);// SystemInfo.deviceUniqueIdentifier.Substring(0, 6);// Player.Instance.openID.Substring(0,6);//SystemInfo.deviceUniqueIdentifier.Substring(0, 6);//
        Player.Instance.headID = "headid";
        Player.Instance.sex = 1;
        ClientToServerMsg.Send(Opcodes.Client_Character_Create, Player.Instance.openID, Player.Instance.otherName, Player.Instance.headID, (byte)Player.Instance.sex);
        return;

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            ssdk.Authorize(PlatformType.WeChat);
            UIManager.Instance.ShowUIPanel(UIPaths.LoadingObj);
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            // Player.Instance.openID = DateTime.Now.Ticks.ToString();
            // Player.Instance.otherName = DateTime.Now.Ticks.ToString().Substring(0, 6);
            Player.Instance.openID =  SystemInfo.deviceUniqueIdentifier;//"4a14a16a941f3eeade18ba90e009f960";//SystemInfo.deviceUniqueIdentifier;//
            Player.Instance.otherName = Player.Instance.openID.Substring(0, 6);// SystemInfo.deviceUniqueIdentifier.Substring(0, 6);// Player.Instance.openID.Substring(0,6);//SystemInfo.deviceUniqueIdentifier.Substring(0, 6);//
            Player.Instance.headID = "headid";
            Player.Instance.sex = 1;
            ClientToServerMsg.Send(Opcodes.Client_Character_Create, Player.Instance.openID, Player.Instance.otherName, Player.Instance.headID, (byte)Player.Instance.sex);
        }
    }

    /// <summary>
    /// 快速登录
    /// </summary>
    public void QuickLog()
    {
        Player.Instance.openID = SystemInfo.deviceUniqueIdentifier;//  "92820196e9ab51daaf5d94e01a18ed77";// SystemInfo.deviceUniqueIdentifier;//  "1000293";//  "ouf9U06WBhaIqxjrHfJaWmm0-szQ";//
        Player.Instance.otherName = Player.Instance.openID.Substring(0, 6);// SystemInfo.deviceUniqueIdentifier.Substring(0, 6);//"095f39";//    
        Player.Instance.headID = "headid";
        Player.Instance.sex = 1;
        ClientToServerMsg.Send(Opcodes.Client_Character_Create, Player.Instance.openID, Player.Instance.otherName, Player.Instance.headID, (byte)Player.Instance.sex);
        return;
    }

    /// <summary>
    /// 获取用户信息回调
    /// </summary>
    /// <param name="reqID"></param>
    /// <param name="state"></param>
    /// <param name="type"></param>
    /// <param name="data"></param>
    void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable data)
    {
        if (state == ResponseState.Success)
        {
            string strs = MiniJSON.jsonEncode(data);
            JsonData jd = JsonMapper.ToObject(strs);
            Player.Instance.openID = jd["openid"].ToString();
            Player.Instance.otherName = jd["nickname"].ToString();
            Player.Instance.headID = jd["headimgurl"].ToString();
            Player.Instance.sex = byte.Parse(jd["sex"].ToString());
            UIManager.Instance.HideUIPanel(UIPaths.LoadingObj);
            ClientToServerMsg.Send(Opcodes.Client_Character_Create, Player.Instance.openID, Player.Instance.otherName, Player.Instance.headID, (byte)Player.Instance.sex);
        }
        else
        {
            Debug.Log("获取信息失败");
        }
    }

    /// <summary>
    /// 授权登录回调
    /// </summary>
    /// <param name="reqID"></param>
    /// <param name="state"></param>
    /// <param name="type"></param>
    /// <param name="data"></param>
    void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable data)
    {
        if (state == ResponseState.Success)
        {
            ssdk.GetUserInfo(PlatformType.WeChat);
        }
        else if (state == ResponseState.Fail)
        {
            Debug.Log("授权失败");
            print("fail! throwable stack = " + data["stack"] + "; error msg = " + data["msg"]);
            //GameData.ResultCodeStr = "fail! throwable stack = " + data["stack"] + "; error msg = " + data["msg"];
            //UIManager.Instance.ShowUIPanel(UIPaths.ShowStringDialog, OpenPanelType.MinToMax);
        }

    }
    /// <summary>
    /// 分享回调
    /// </summary>
    /// <param name="reqID"></param>
    /// <param name="state"></param>
    /// <param name="type"></param>
    /// <param name="data"></param>
    void OnShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("share successfully - share result :");
            print(MiniJSON.jsonEncode(result));

            switch(shareFunctionName){
                case SFN_DETECTFACE:
                    {
                        EventCenter.instance.Raise(new DetectFaceSharedEvent());
                        shareFunctionName = "";
                        break;
                    }
            }
           //ClientToServerMsg.Send(Opcodes.Client_ShareSuccess);
        }
        else if (state == ResponseState.Fail)
        {
            #if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
            //GameData.ResultCodeStr = "fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"];
            #elif UNITY_IPHONE
           // GameData.ResultCodeStr = "fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"];
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
            #endif
            //UIManager.Instance.ShowUIPanel(UIPaths.ShowStringDialog, OpenPanelType.MinToMax);

            shareFunctionName = "";
        }
        else if (state == ResponseState.Cancel)
        {
            shareFunctionName = "";
            print("cancel !");
        }else{
            shareFunctionName = "";
        }

        if( state != ResponseState.Success ){
            EventCenter.instance.Raise(new ShareFailureEvent());
        }

        if( !shareFunctionName.Equals("") ){
            Debug.LogError("分享功能回调时 AuthorizeOrShare.shareFunctionName 未清空");
        }
    }

    #endregion
    protected override void OnDestroy()
    {

    }
}
