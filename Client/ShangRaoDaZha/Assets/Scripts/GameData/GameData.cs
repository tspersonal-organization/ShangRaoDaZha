using FrameworkForCSharp.Utils;
using S2CEntity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationType//东南西北
{
    E = 1,
    W = 2,
    S = 3,
    N = 4,
}

public class PkClubInfo
{
    public uint ClubId;
    public string ClubName;
    public int RoomCount;//开的房间数
}

//俱乐部开的房间信息
public class PKClubRoomInfo
{
   public  uint codeId;//房间id
    public byte playType;//玩法类型，普通、包牌
    public uint playerCount;//玩家人数
    public uint gameCount;//局数

    public bool wuZhaTp;//无炸摊牌
    public bool faWangTp;//罚王摊牌
    public bool BaWang;//是否八王

    public List<uint> JiangMa;//开的奖码

    public string create_playername;//开房人名称
    public ulong create_playerguid;//开房人guid

    public List<PKClubPlayerInfo> PKClubPlayerInfoList = new List<PKClubPlayerInfo>(); 
}

//房间里的玩家信息
public class PKClubPlayerInfo
{
    public byte pos;
    public ulong Guid;
    public string OtherName;
    public string HeadId;
}

public class GameData
{
    #region 牛牛开房记录
    public static int DiFen
    {
        get { return PlayerPrefs.GetInt("DiFen", -1); }
        set { PlayerPrefs.SetInt("DiFen", value); }
    }
    public static int Round
    {
        get { return PlayerPrefs.GetInt("Round", -1); }
        set { PlayerPrefs.SetInt("Round", value); }
    }

    public static int payMethod
    {
        get { return PlayerPrefs.GetInt("payMethod", -1); }
        set { PlayerPrefs.SetInt("payMethod", value); }
    }
 
    public static bool ShunZhiNiu
    {
        get { return bool.Parse(PlayerPrefs.GetString("ShunZhiNiu", "False")); }
        set { PlayerPrefs.SetString("ShunZhiNiu", value.ToString()); }
    }
    public static bool ZaDanNiu
    {
        get { return bool.Parse(PlayerPrefs.GetString("ZaDanNiu", "False")); }
        set { PlayerPrefs.SetString("ZaDanNiu", value.ToString()); }
    }
    public static bool WuXiaoNiu
    {
        get { return bool.Parse(PlayerPrefs.GetString("WuXiaoNiu", "False")); }
        set { PlayerPrefs.SetString("WuXiaoNiu", value.ToString()); }
    }
    public static bool WuHuaNiu
    {
        get { return bool.Parse(PlayerPrefs.GetString("WuHuaNiu", "False")); }
        set { PlayerPrefs.SetString("WuHuaNiu", value.ToString()); }
    }
    public static bool XianJiaMaiMa
    {
        get { return bool.Parse(PlayerPrefs.GetString("XianJiaMaiMa", "False")); }
        set { PlayerPrefs.SetString("XianJiaMaiMa", value.ToString()); }
    }
    #endregion


    #region 牛牛常量
    public const int TIME_QIANGZHUANG = 9;
    public const int TIME_XIAZHU = 8;
    public const int TIME_LIANGPAI = 14;


    #endregion



    public static PlayerInfo ChosePlayer;//游戏中选择的玩家

    public static MemInfo ChoseMem ;//俱乐部成员列表点击的成员
    public static List<PKClubRoomInfo> PKClubRoomList = new List<PKClubRoomInfo>();//莫格俱乐部开的房间数
    public static List<PkClubInfo> PKClubInfoList = new List<PkClubInfo>();//玩家加入的俱乐部数量
    public static PkClubInfo CurrentClickClubInfo;//点击进入的那个俱乐部 private PkClubInfo InfoData;

    public static SendRoomInfo sendroominfo = null;//房间信息

    public static List<ClubInfo> InviteClubIdAndName = new List<ClubInfo>();//邀请玩家加入俱乐部的消息列表
   // public static ClubInfo CurrentClickClubInfo;//点击进入的那个俱乐部 private PkClubInfo InfoData;

    public static bool IsClubAutoCreatRoom = false;//是否为俱乐部自动开房
    public static ClubInfo CurrentClubInfo = new ClubInfo();//当前生成的俱乐部信息
    public static List<ClubInfo> ClubInfoList = new List<ClubInfo>();//俱乐部列表

    public static string GlobleTipString;//全局提示文字
    public static int GoldReduce = 0;//金币场扣的金币

    public static RoomType GlobleRoomType = RoomType.Other;//全局房间信息
    public static uint MianCard=0;//面牌  回放需要用
    public static uint MagicCard=0;

    public static List<FinishPlayer> FinishPlayerPos = new List<FinishPlayer>();

    public  uint RoundNum = 0;//局数
    public  uint PayMethod = 0;//支付方式
    public static List<string> m_PaoMaDengList = new List<string>();//跑马灯信息
    public static List<AngentRoomInfo> AngentRoomList = new List<AngentRoomInfo>();//代理开的房间
    public static bool IsAppStore = false;
    public static string ShareImageURL = "";
    public static string RechargeURL = "";
    public static bool m_IsNormalOver;
    public static string ResultCodeStr;
    public static string Tips;

    public static TableInfo m_TableInfo = new TableInfo();//桌子信息
    public static RoundOverInfo m_RoundOverInfo = new RoundOverInfo();
    public static RecordListInfo m_ChooseRecordListInfo = new RecordListInfo();

    public static List<PlayerInfo> m_PlayerInfoList = new List<PlayerInfo>();//玩家列表
    public static List<HoldCardsObj> m_HoldCardsList = new List<HoldCardsObj>();//无挡胡玩家对象  手里牌 出的牌

    public static Queue<string> m_VoiceQueue = new Queue<string>();//语音
    public static List<RecordListInfo> m_RecordList = new List<RecordListInfo>();
    public static List<RecordListInfo> m_RecordRoundList = new List<RecordListInfo>();//一房间的多局记录
    public static List<HuiFangInfo> m_HuiFangList = new List<HuiFangInfo>();

    /// <summary>
    /// 所有牌的相关信息
    /// </summary>
    public static List<TableCardInfo> AllCardsInfo = new List<TableCardInfo>();
    /// <summary>
    /// 两个骰子的数字
    /// </summary>
    public static int Dice1 = 1;
    public static int Dice2 = 1;
    public static int Dice3 = 1;
    public static int Dice4 = 1;
    /// <summary>
    /// 初始化所有的信息
    /// </summary>
    public static void InitAllCardsInfo()
    {
        uint[] cardNumbers = { 101, 102, 103, 104, 105, 106, 107, 108, 109, 201, 202, 203, 204, 205, 206, 207, 208, 209, 301, 302, 303, 304, 305, 306, 307, 308, 309, 401, 501, 601, 701, 801, 901, 1001 };
        AllCardsInfo.Clear();
        for (int i = 0; i < cardNumbers.Length; i++)
        {
            TableCardInfo cinfo = new TableCardInfo();
            cinfo.CardNumber = cardNumbers[i];
            cinfo.RemainedCount = 4;
            cinfo.TotalCount = 4;
            AllCardsInfo.Add(cinfo);
        }
    }

    public static int GenerateDice(int DiceIndex)
    {
        uint result = 1;
        uint p1 = m_TableInfo.id;
        uint p3 = (uint)m_TableInfo.curGameCount;
        uint p2 = 1;

        foreach (PlayerInfo p in m_PlayerInfoList)
        {
            p2 += (uint)p.guid;
        }
        Debug.Log("p1:" + p1 + " p2:" + p2 + " p3:" + p3);
        switch (DiceIndex)
        {
            case 1:
                result = (p1 * p2 * p3) % 7;
                if (result == 0)
                {
                    result = 1;
                }
                break;
            case 2:
                result = (p1 * p2 * p3 * p3) % 7;
                if (result == 0)
                {
                    result = 1;
                }
                break;
        }
        Debug.Log("Generate dice :" + result.ToString());
        return (int)result;

    }
}

//代理房间信息
public class AngentRoomInfo
{
    public RoomType Roomtype;
    public uint RoomId;
    public uint Time;
    public uint RoomRound;
    public int PlayerCount;
    public List<string> HeadNames;
}

public class FinishPlayer
{
    public int pos;
    public int index;
}
