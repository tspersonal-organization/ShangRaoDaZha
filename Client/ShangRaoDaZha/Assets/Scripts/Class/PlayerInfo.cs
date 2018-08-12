using System.Collections.Generic;
using FrameworkForCSharp.Utils;

public enum LocalViewDirection
{
    LOCAL = 0,
    RIGHT = 1,
    UP = 2,
    LEFT = 3,
}

public enum PengGangFromDirection//碰杠的来源方向
{
    LOCAL,//自己
    DUIJIA,//对家
    SHANGJIA,//上家
    XIAJIA,//下家
}

public class PlayerInfo
{
    public uint FanBeiCount;//
    public NNType PeiPaiType;//配牌信息
    public  List<List<uint>> PeiPaiInfo = new List<List<uint>>();//牛牛配牌信息
    public Dictionary<byte, uint> PosAndChipDic = new Dictionary<byte, uint>();//牛牛给其他玩家下的注
    public bool IsGangKai = false;//是否为杠开
    public   bool IsFirstChuPai = false;//是否为庄家切第一次出牌
    public bool IsAi;//是否托管
    public int LeftCardNum;//剩余数量
    public CardOperateType OperateType;

    public byte pos;
    public ulong guid;
    public byte sex;
    public bool isStartReady;
    public bool isNextReady;
    public bool isForce;
    public string ip;
    public string mask;
    public string name;
    public string headID;
    public int changeScore;
    public uint TSTaoShangScore;
    public int score;
    public int HuiHeScore;
    public int TaoShangScore;
    public long Gold;//金币数

    public PrizeType prizeType;//翻倍类型
    public int GangKaiCount;//杠次数
  

    public uint fangPaoScore;
    public bool isOperateFangPao;
    public uint totalHuCount;
    public uint totalMakerCount;
    public LocalViewDirection LVD;
    public HuType huType;
    public byte menCount;
    public float N;
    public float E;

    public List<OpreateCardInfo> operateCardList = new List<OpreateCardInfo>();//碰杠的牌
    public List<uint> outCardList = new List<uint>();//出的牌
    public List<uint> localCardList = new List<uint>();//发的手牌
    public List<uint> limitPengCardList = new List<uint>();

    public List<uint> MianCardList = new List<uint>();//打出的面牌
}

public class OpreateCardInfo
{
    public byte pos;
    public CatchType opType;
    public uint opCard;
}