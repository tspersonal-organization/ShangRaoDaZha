using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPaths
{
    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
    public static readonly string PathURL =
#if UNITY_ANDROID && !UNITY_EDITOR  //安卓
      "file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE && !UNITY_EDITOR  //iPhone
     "file://" + Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_EDITOR//windows平台和web平台
    "file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;
#endif

    #region 界面路径
    public const string ClubInvitePlayerPanel = "Panel/ClubInvitePanel";
    public const string InvitePlayerPanel = "Panel/InvitePlayerPanel";
    public const string CreatRoomTongJiPanel = "Panel/CreatRoomTongJiPanel";
    public const string ClubMemControlPanel = "Panel/ClubMemControlPanel";
    public const string ClubRenamePanel = "Panel/ClubRenamePanel";//俱乐部管理panel
    public const string ClubMastorPanel = "Panel/ClubMastorPanel";//俱乐部管理panel
    public const string ClubListPanel = "Panel/ClubListPanel";//俱乐部列表panelClubInfoPanel


    public const string GlobleTipPanel = "Panel/GlobalTipPanel";

    public const string GameUpdate = "Panel/GameUpdate";
    public const string LoadingObj = "Panel/LoadingObj";
    public const string LoadingInApp = "Panel/LoadingInApp";
    public const string DisconnectServer = "Panel/DisconnectServer";

    public const string UIPanel_CreateRoom = "Panel/UIPanel_CreateRoom";
    public const string UIPanel_AddRoom = "Panel/UIPanel_AddRoom";
    public const string UIPanel_Setting = "Panel/UIPanel_Setting";
    public const string UIPanel_RoundOver = "Panel/UIPanel_RoundOver";
  //  public const string UIPanel_TotalScore = "Panel/UIPanel_TotalScore";
    public const string UIPanel_Help = "Panel/UIPanel_Help";
    public const string UIPanel_GongGao = "Panel/UIPanel_GongGao";
    public const string UIPanel_ZhanJiRoundInfo = "Panel/UIPanel_ZhanJiRoundInfo";
    public const string UIPanel_Shop = "Panel/UIPanel_Shop";
    public const string UIPanel_HuiFang = "Panel/UIPanel_HuiFang";
    public const string UIPanel_Distance = "Panel/UIPanel_Distance";



    //add by tj
    public const string ReconectTipPanel = "DDZPanel/ReconectTipPanel";
    public const string InvitePanel = "DDZPanel/InvitePanel";
    public const string MyRoomPanel = "DDZPanel/MyRoomPanel";
    public const string UIPanel_TotalScore = "DDZPanel/TotalGameOverPanel";
    // public const string PanelCreatRoom = "DDZPanel/PanelCreatRoom";
    public const string MarketPanel = "DDZPanel/MarketPanel";
    public const string TipModlePanel = "DDZPanel/TipModlePanel";
    public const string DDZGamePanel = "DDZPanel/DDZGamePanel";

    public const string RulePanel = "DDZPanel/RulePanel";



    //------------------------------------------------------------
    public const string PanelDialog = "Prefabs/Common/Panel/PanelDialog";//服务器返回的提示
    public const string PanelTips = "Prefabs/Common/Panel/PanelTips";//主动操作的提示

    public const string PanelJoinMoment = "Prefabs/Dz/Panel/PanelJoinMoment";//申请加入俱乐部
    public const string PanelJoinMomentInvite = "Prefabs/Dz/Panel/PanelJoinMomentInvite";//邀请加入俱乐部 
    public const string PanelMomentInfo = "Prefabs/Dz/Panel/PanelMomentInfo";//俱乐部信息
    public const string PanelPlayerInfo = "Prefabs/Dz/Panel/PanelPlayerInfo";// 俱乐部成员信息

    public const string PanelUserInfo = "Prefabs/Dz/Panel/PanelUserInfo";//用户信息
    public const string PanelCreatRoom = "Prefabs/Dz/Panel/PanelCreatRoom";//创建房间
    public const string PanelJoinRoom = "Prefabs/Dz/Panel/PanelJoinRoom";//加入房间
    public const string PanelNotice = "Prefabs/Dz/Panel/PanelNotice";//公告
    public const string PanelHistory = "Prefabs/Dz/Panel/PanelHistory";//战绩
    public const string PanelMessage = "Prefabs/Dz/Panel/PanelMessage";//消息
    public const string PanelShare = "Prefabs/Dz/Panel/PanelShare";//分享
    public const string PanelSetting = "Prefabs/Dz/Panel/PanelSetting";//设置
    public const string PanelSetting2 = "Prefabs/Dz/Panel/PanelSetting2";//游戏设置

    public const string PanelChat = "Prefabs/Dz/Panel/PanelChat";//聊天
    public const string PanelDestoryRoom = "Prefabs/Dz/Panel/PanelDestoryRoom";//解散房间
    public const string PanelShowCard = "Prefabs/Dz/Panel/PanelShowCard";//摊牌
    public const string PanelGameOverBig = "Prefabs/Dz/Panel/PanelGameOverBig";//总结算
    public const string PanelGameOverSmall = "Prefabs/Dz/Panel/PanelGameOverSmall";//单局结算


    //------------------------------------------------------------


    #endregion

    #region 声音路径
    public const string SOUND_BGM_GAME = "Sound/sound_game";
    public const string SOUND_BUTTON = "Sound/sound_game_btn";

    public const string SOUND_OUT_CARD = "Sound/WuDangHu/{0}/sound_";
    public const string SOUND_PENG = "Sound/WuDangHu/{0}/sound_game_peng";
    public const string SOUND_GANG = "Sound/WuDangHu/{0}/sound_game_gang";
    public const string SOUND_HU = "Sound/WuDangHu/{0}/sound_game_hu";

    public const string SOUND_SITDOWN = "Sound/sound_game_sitdown";
    public const string SOUND_FAPAI = "Sound/sound_game_fapai";
    public const string SOUND_CHOOSE = "Sound/sound_game_choose";
    public const string SOUND_START_GAME = "Sound/sound_game_start";
    public const string SOUND_MOPAI = "Sound/sound_game_mopai";
    public const string SOUND_CHUPAI = "Sound/sound_game_chupai";
    public const string SOUND_READY = "Sound/sound_game_ready";


    //ADD by tj
    public const string SINGLE_MAN = "Sound/boy/single/card_single_";
    public const string SINGLE_WOMAN = "Sound/girl/single/card_single_";
   

    public const string DOUBLE_MAN = "Sound/boy/double/card_double_";
    public const string DOUBLE_WOMAN = "Sound/girl/double/card_double_";
    public const string TRIPLE_MAN = "Sound/boy/triple/card_triple_";
    public const string TRIPLE_WOMAN = "Sound/girl/triple/card_triple_";
  

  //  public const string DANI = "Sound/Operate/card_dani_";
    public const string GUOPAI_MAN = "Sound/boy/cardtype/card_pass_";
    public const string GUOPAI_WOMAN = "Sound/girl/cardtype/card_pass_";
    //public const string DANI1 = "Sound/Operate/card_dani_1";
    //public const string DANI2 = "Sound/Operate/card_dani_2";
    //public const string DANI3 = "Sound/Operate/card_dani_3";
    //public const string GUOPAI1= "Sound/Operate/card_last_1";
    //public const string GUOPAI2 = "Sound/Operate/card_last_2";
    //public const string GUOPAI3 = "Sound/Operate/card_last_3";

   // public const string THREECARD = "Sound/SpecialType/card_three_M";
    public const string BOOM_MAN = "Sound/boy/cardtype/card_boom";
    public const string BOOM_WOMAN = "Sound/girl/cardtype/card_boom";
    public const string LIANDUI_MAN = "Sound/boy/cardtype/card_LianDui";
    public const string LIANDUI_WOMAN = "Sound/girl/cardtype/card_LianDui";
    public const string ROCK_MAN = "Sound/boy/cardtype/card_rocket";
    public const string ROCK_WOMAN = "Sound/girl/cardtype/card_rocket";
    public const string SHUNZHI_MAN = "Sound/boy/cardtype/card_shunzi";  
    public const string SHUNZHI_WOMAN = "Sound/girl/cardtype/card_shunzi";

    public const string CHAT_MAN = "Sound/boy/chat/sound_chat_100";
    public const string CHAT_WOMAN = "Sound/girl/chat/sound_chat_100";
    public const string MJCHAT_MAN = "Sound/WuDangHu/Chat/boy/sound_chat_100";
    public const string MJCHAT_WOMAN = "Sound/WuDangHu/Chat/girl/sound_chat_100";
    public const string NN_MAN = "Sound/NiuNiu/man/man";
    public const string NN_WOMAN = "Sound/NiuNiu/woman/woman";
    public const string NN_Special = "Sound/NiuNiu/special";

    public const string BOOM = "Sound/SpecialType/card_boom_sound";
    public const string ROCK = "Sound/SpecialType/card_rocket_sound";


    //  public const string TIMECHECK = "Sound/OtherEffect/game_TimeOut";
    public const string DDZBG = "Sound/OtherEffect/sound_game_BGM";
    public const string BUTTONCLICK = "Sound/OtherEffect/sound_game_btn";
    public const string GAMECHOSE = "Sound/OtherEffect/sound_game_choose";
    public const string FAPAI = "Sound/OtherEffect/sound_game_fapai";
    public const string GAMELOSE = "Sound/OtherEffect/sound_game_lose";
    public const string GAMEREADY = "Sound/OtherEffect/sound_game_ready";
    public const string GAMESITDOWN = "Sound/OtherEffect/sound_game_sitdown";
    public const string GAMESTART = "Sound/OtherEffect/sound_game_start";
    public const string GAMEWIN = "Sound/OtherEffect/sound_game_win";
    public const string WASHCARD = "Sound/OtherEffect/washCard";

   

    #endregion

    #region 语音路劲
    /// <summary>
    /// 上传声音路径
    /// </summary>
    public static string CHAT_UPLOAD_SOUND_PATH = "http://game.youthgamer.com:86/gameResource.aspx?method=upLoadSound";
    /// <summary>
    /// 语音下载路径
    /// </summary>
    public static string CHAT_DOWN_SOUND_PATH = "http://game.youthgamer.com:86/TableSound/";

    public const string RESOURCES_PATH = "http://game.youthgamer.com:120/";

    public const string SOUND_CHAT_FACE = "Sound/WuDangHu/Chat/{0}/";
    #endregion

}
