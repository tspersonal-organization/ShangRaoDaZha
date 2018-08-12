using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUpdate : UIBase<GameUpdate>
{
    public UILabel LBClientV;
    public UILabel LBServerV;
    public UILabel LBContent;

	// Use this for initialization
	void Start ()
    {
        LBClientV.text = "客户端版本:"+Application.version;
        LBServerV.text = "服务器版本" + ServerInfo.Data.version;
        if (ServerInfo.Data.update_message.Contains("@"))
        {
            string[] strs = ServerInfo.Data.update_message.Split('@');
            LBContent.text = strs[0];
        }
        else
        {
            LBContent.text = ServerInfo.Data.update_message;
        }
       
	}
	
    public void DownloadApp()
    {
        if (Application.platform == RuntimePlatform.Android)
            Application.OpenURL(ServerInfo.Data.update_android_url);
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
            Application.OpenURL(ServerInfo.Data.update_ios_url);
    }
}
