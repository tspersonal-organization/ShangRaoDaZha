using FrameworkForCSharp.Utils;
using System.Collections;
using System.Collections.Generic;
using S2CEntity;
using UnityEngine;

public class PartGameOverControl
{

    public static PartGameOverControl _partGameOverControl;

    public static PartGameOverControl instance
    {
        get
        {
            if (_partGameOverControl == null)
            {
                _partGameOverControl = new PartGameOverControl();
                return _partGameOverControl;
            }
            return _partGameOverControl;
        }
    }

    public List<SettleDownInfo> SettleInfoList = new List<SettleDownInfo>();
    public List<PlayerInfo> TotalGameOverInfoList = new List<PlayerInfo>();
    public List<List<SettleDownInfo>> ListGameOverSmall = new List<List<SettleDownInfo>>();

    public int ZhuangPos;
    public int HelperPos;


    //无挡胡结算信息
    public List<WUHSettleDownInfo> WuDangHuSettleInfoList = new List<WUHSettleDownInfo>();
    public bool ISWDHHuPai = false;
    public uint HuPos;
    public HuType WDHHuType;
}

//讨赏
public class SettleDownInfo
{

    public bool IsWin;
    public int pos;
    public int Score;
    public int HuiHeFen;
    public int TaoShangFen;
    public int ChangeScore;

    public int ZhaDanScore;
    public int FaWangScore;

    public long Gold;//玩家的总金币数
    public List<uint> LeftCardList = new List<uint>();//剩余手牌数
    public int Index;//第几个完成

    public List<List<uint>> TaoShangCardList = new List<List<uint>>();//打出的讨赏牌


}

#region//无挡胡
public class WUHSettleDownInfo
{
    public byte pos;
    public int Score;
    public int ChangeScore;
    public int HuiHeScore;
    public int TaoShangScore;
    public HuType CardHutype;

    public byte GetCardCount;//碰的牌
    public List<GetCardInfo> GetCardInfoList = new List<GetCardInfo>();
    public List<uint> CardList = new List<uint>();//手牌


}

//吃碰信息
public class GetCardInfo
{
    public byte Pos;//位置  谁打来
    public CatchType type;//接的类型
    public uint Card;//牌
}

#endregion
