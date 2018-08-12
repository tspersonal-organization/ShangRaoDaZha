using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkForCSharp.Utils;

public class TableInfo
{

    #region NN
    public List<int> QiangZhuangPosList = new List<int>();//已经抢庄列表
    public Dictionary<int, int> PosAndChipDic = new Dictionary<int, int>();//玩家下的注  断线回来  中途进入需要用

    public bool IsQiangZhuangState = false;//是否为抢庄状态
    public bool IsXiaZhuState = false;//是否为配牌状态
    public NNYongPai NNYongPaiType;
    public List<uint> CanChipList;//可下注的值

    public bool EnShunZhiNiu;
    public bool EnBoomNiu;
    public bool EnWuXiaoNiu;
    public bool EnWuHuaNiu;
    public bool EnXianJiaMaiMA;
    #endregion

    public bool isBawang;//是否八王
    public bool IsPiPei;//是否为匹配场
    public bool IsDaiLiCreat;//是否代理开房

    public uint id;
    public ulong fangZhuGuid;
    public byte configPlayerIndex;
    public byte configRoundIndex;
    public byte configFangChongIndex;
    public byte configShengPaiIndex;
    public byte configDaZiIndex;
    public byte configPayIndex;

    public byte ZhuangPos;
    public bool IsJIangMa;//是否奖码
                          //  public int LianZhuangCount;//连庄次数

    #region  栽宝需要
    public ulong RoomGuid;
    public uint FirstDices;//骰子
    public uint SecendDices;
    public uint ThirdDices;//骰子
    public uint FouthDices;

    public uint MianCard;//面牌
    public uint MagicCard;//奶子
    public int ChiMianCount;//吃面次数
    public int GangCount=0;//杠的次数
    public int ForbiddenIndex;//不允许拿的那一盾牌
    #endregion 

    public byte makerPos;
    public uint lianZhuangCount;
    public RoomStatusType roomState;
    public uint curGameCount;
    public bool isQueryLeaveRoom;
    public uint queryLeaveRoomWaitTime;
    public bool isWaitFangPao;
    public byte lastOutCardPos;
    public byte waitOutCardPos;
    public uint resCardCount;
    public bool isOutCardInfo;
    public byte outCardPos;
    public uint outCardNumber;
    public bool isInCardInfo;
    public byte inCardPos;
    public uint inCardNumber;
    public bool isQiangGangHu;
    public byte qiangGangPos;
    public uint qiangGangCard;

    public List<byte> operateLeaveRoomList = new List<byte>();
    public List<byte> operateQiangGangList = new List<byte>();


    public uint FriendCard=0;
    public int FriendPos=0;
}
