using System.Collections.Generic;
using UnityEngine;

public class Player
{
    static Player _instance = null;

    public static Player Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Player();
            return _instance;
        }
    }

    public bool HaveEmail = false;
    public ulong guid;
    public string account;
    public string Ip;//ip地址
    public string Address;//定位地址
    public uint lastEnterRoomID;
    public string gongGao;
    public uint shareRoomID = 0;
    public string shareUrl = string.Empty;
    public bool isDaiLi;////是否是代理玩家
    public List<uint> handCardList = new List<uint>();//玩家手牌
    public bool isLogin = false;
    public string content;
    public uint everydayShareCount;

    public ulong InviteGuid;//邀请人guid

    public long RoomCard;//房卡
    public long money;//钻石
    public long Gold;//金币

    public string otherName
    {
        get
        {
            return PlayerPrefs.GetString("otherName");
        }
        set
        {
            PlayerPrefs.SetString("otherName", value);
        }
    }
    public string headID
    {
        get
        {
            return PlayerPrefs.GetString("headID");
        }
        set
        {
            PlayerPrefs.SetString("headID",value);
        }
    }
    public string openID
    {
        get
        {
            return PlayerPrefs.GetString("openID", "n");
        }
        set
        {
            PlayerPrefs.SetString("openID",value);
        }
    }
    public int sex
    {
        get { return PlayerPrefs.GetInt("sex", 1); }
        set { PlayerPrefs.SetInt("sex",value); }
    }

    public bool GameBGSoundOff { get { return bool.Parse(PlayerPrefs.GetString("GameBGSoundOff", "False")); } set { PlayerPrefs.SetString("GameBGSoundOff", value.ToString()); } }
    public bool GameEffectSoundOff { get { return bool.Parse(PlayerPrefs.GetString("GameEffectSoundOff", "True")); } set { PlayerPrefs.SetString("GameEffectSoundOff", value.ToString()); } }
    public float GameBGSoundValue { get { return PlayerPrefs.GetFloat("GameBGSoundValue", 1.0f); } set { PlayerPrefs.SetFloat("GameBGSoundValue", value); } }
    public float GameEffectSoundValue { get { return PlayerPrefs.GetFloat("GameEffectSoundValue", 1.0f); } set { PlayerPrefs.SetFloat("GameEffectSoundValue", value); } }
    public string GameTestAccount { get { return PlayerPrefs.GetString("GameTestAccount", "0"); } set { PlayerPrefs.SetString("GameTestAccount", value); } }
    public int FYLGameBGIndex { get { return PlayerPrefs.GetInt("FYLBGINDEX", 1); } set { PlayerPrefs.SetInt("FYLBGINDEX", value); } }
    public bool GameLanguage { get { return bool.Parse(PlayerPrefs.GetString("GameLanguage", "True")); } set { PlayerPrefs.SetString("GameLanguage", value.ToString()); } }

    // 游戏登出
    public void Logout()
    {
        openID = "n";
        headID = "";
        otherName = "";
        isLogin = false;
        ConnServer.Instance.DisconnectServer();
        ConnServer.ConnectionServer(ToolsFunc.GetServerIP(ServerInfo.Data.ip), ServerInfo.Data.port);
        //ManagerScene.Instance.LoadScene(SceneType.DzViewLogin);
    }
}
