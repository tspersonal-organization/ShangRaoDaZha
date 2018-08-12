using CreateRoomEntity;
using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using Google.Protobuf;
using System.Collections.Generic;
using System.IO;

public class ClientToServerMsg
{
    public static void Send(Opcodes op, params object[] pms)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)op, 100);
        for (int i = 0; i < pms.Length; i++)
        {
            if (pms[i].GetType() == typeof(string)) message.writeString((string)pms[i]);
            else if (pms[i].GetType() == typeof(bool)) message.writeBool((bool)pms[i]);
            else if (pms[i].GetType() == typeof(int)) message.writeInt32((int)pms[i]);
            else if (pms[i].GetType() == typeof(long)) message.writeInt64((long)pms[i]);
            else if (pms[i].GetType() == typeof(byte)) message.writeUInt8((byte)pms[i]);
            else if (pms[i].GetType() == typeof(uint)) message.writeUInt32((uint)pms[i]);
            else if (pms[i].GetType() == typeof(ulong)) message.writeUInt64((ulong)pms[i]);
            else if (pms[i].GetType() == typeof(float)) message.writeFloat((float)pms[i]);

        }
        SendMsg(message);
    }
    /// <summary>
    /// 发送地址给
    /// </summary>
    /// <param name="roomid"></param>
    /// <param name="agree"></param>
    public static void SetAdrress(string address)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Set_Client_Address, 100);
        message.writeString(address);//房间号
       

        SendMsg(message);
    }

    /// <summary>
    /// 设置取消管理员
    /// </summary>
    /// <param name="roomid"></param>
    /// <param name="agree"></param>
    public static void SetMasterOper(uint clubid,ulong playerid,bool oper)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Club_Set_Admin, 100);
        message.writeUInt32(clubid);//房间号
        message.writeUInt64(playerid);//玩家id
        message.writeBool(oper);//是否包牌

        SendMsg(message);
    }

    /// <summary>
    /// 传解散房间
    /// </summary>
    /// <param name="roomid"></param>
    /// <param name="agree"></param>
    public static void SendDisPosRoom(uint roomid)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_DisposeRoom, 100);
        message.writeUInt32(roomid);//房间号
      //  message.writeBool(agree);//是否包牌

        SendMsg(message);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="roomid"></param>
    /// <param name="agree"></param>
    public static void SendBaoPaiInfo(uint roomid,bool agree)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Ask_Bao_Pai_Result, 100);
      message.writeUInt32(roomid);//房间号
        message.writeBool(agree);//是否包牌

        SendMsg(message);
    }
    /// <summary>
    /// 创建房间
    /// </summary>
    public static void SendCreatRoom(CreateRoom roominfo)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_PlayerCreateXYQPRoom, 100);
        //CreateRoom roominfo = new CreateRoom();
        //uint32 club_id = 1;//俱乐部id，只能俱乐部开房，个人不能开房
        //uint32 player_count_index = 2;//房间人数下标（new int[] { 2, 4 };//人數）
        //uint32 play_count_index = 3;//局数下标（new int[] { 5, 10 };//圈数)
        //uint32 play_type_index = 4;//玩法下标（枚举类:PlayType)
        //repeated uint32 jiang_ma = 5;//开奖（牌数，{7,11,13,14}）
        //bool fa_wang_tp = 6;//罚王摊牌
        //bool wu_zha_tp = 7;//无炸摊牌
        //double pox = 8;//经纬度
        //double poy = 9;//经纬度

        //roominfo.ClubId = 100000;
        //roominfo.FaWangTp = false;
        ////roominfo.JiangMa = new Google.Protobuf.Collections.RepeatedField<uint>();
        //roominfo.PlayCountIndex = 0;
        //roominfo.PlayerCountIndex = 0;
        //roominfo.PlayTypeIndex = 0;
        //roominfo.Pox = 0;
        //roominfo.Poy = 0;

        //roominfo.Address = "";
        //roominfo.WuZhaTp = false;
       


        MemoryStream memoryStream = new MemoryStream();
        roominfo.WriteTo(memoryStream);
        message.writeInt32(memoryStream.ToArray().Length);
        message.writeBytes(memoryStream.ToArray());

        SendMsg(message);

    }

    /// <summary>
    /// 处理俱乐部邀请信息
    /// </summary>
    public static void OperateInviteMessage(uint clubid,bool agree)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Club_Invite_Response, 100);
       
        message.writeUInt64(clubid);
        message.writeBool(agree);
        SendMsg(message);
    }
    /// <summary>
    ///  邀请玩家加入俱乐部
    /// </summary>
    /// <param name="playerGuid"></param>
    /// <param name="clubid"></param>
    /// <param name="clubName"></param>
    public static void InvitePlayerJoinClub(ulong playerGuid,uint clubid,string clubName="")
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Club_Invite_Player, 100);
        message.writeUInt64(playerGuid);
        message.writeUInt32(clubid);
       // message.writeString(clubName);
        SendMsg(message);
    }
    /// <summary>
    /// 移除玩家
    /// </summary>
    /// <param name="clubid"></param>
    /// <param name="playerid"></param>
    public static void RemovePlayerFromClub(uint clubid,ulong playerid)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Leave_Club, 100);
        message.writeUInt32(clubid);
        message.writeUInt64(playerid);
      
        SendMsg(message);
    }
    /// <summary>
    /// 处理玩家的申请
    /// </summary>
    /// <param name="clubid"></param>
    /// <param name="playerid"></param>
    /// <param name="Agree"></param>
    public static void OperatePlayerApply(uint clubid,ulong playerid,bool Agree)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Agree_Enter_Club, 100);
        message.writeUInt32(clubid);
        message.writeUInt64(playerid);
        message.writeBool(Agree);
        SendMsg(message);
    }
    /// <summary>
    /// 获取俱乐部成员列表   和申请列表
    /// </summary>
    /// 
    /// <param name="clubid"></param>
    public static void GetClubMemListInfo(uint clubid)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Query_Club_Player_Info, 100);
        message.writeInt32(GameData.PKClubInfoList.Count);
        if (GameData.PKClubInfoList.Count == 0)
        {
            message.writeUInt32(0);
        }
        else
        {
           
            for (int i = 0; i < GameData.PKClubInfoList.Count; i++)
            {
                message.writeUInt32(GameData.PKClubInfoList[i].ClubId);
            }
        }
       

        SendMsg(message);
    }
    /// <summary>
    /// 申请加入俱乐部
    /// </summary>
    /// <param name="clubid"></param>
    public static void ApplyJoinClub(uint clubid)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Apply_For_Club, 100);
        message.writeUInt32(clubid);
      
        SendMsg(message);
    }
    /// <summary>
    /// 申请俱乐部房间列表
    /// </summary>
    /// <param name="clubid"></param>
    public static void ApplyClubRoomList(uint clubid)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Query_Club_RoomList, 100);
        message.writeUInt32(clubid);

        SendMsg(message);
    }
    /// <summary>
    /// 俱乐部改名
    /// </summary>
    /// <param name="clubid"></param>
    /// <param name="name"></param>
    public static void ReNameClub(uint clubid,string name)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Change_Club_Name, 100);
        message.writeUInt32(clubid);
        message.writeString(name);
        SendMsg(message);
    }
    /// <summary>
    /// 通过俱乐部进入房间
    /// </summary>
    /// <param name="clubid"></param>
    /// <param name="haveroomid"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    public static void ClubEnterRoom(uint clubid,bool haveroomid,float x,float y,uint roomId,ClubRoomType type,uint ChipIndex,NNYongPai yongpai,JiangMaType jiangma,bool shunzhiniu,bool zhadanniu,bool wuxiaoniu,bool wuhuaniu,bool xianjiamaima)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Club_Player_Enter_Room, 100);
        message.writeUInt32(clubid);
        message.writeBool(haveroomid);
        message.writeFloat(x);
        message.writeFloat(y);
        message.writeUInt32(roomId);
        message.writeUInt8((byte)type);
        message.writeUInt8((byte)ChipIndex);

        message.writeUInt8((byte)yongpai);
        message.writeUInt8((byte)jiangma);

        message.writeBool(shunzhiniu);
        message.writeBool(zhadanniu);
        message.writeBool(wuxiaoniu);
        message.writeBool(wuhuaniu);
        message.writeBool(xianjiamaima);

        SendMsg(message);
    }
    /// <summary>
    /// 退出或提出俱乐部
    /// </summary>
    /// <param name="ClubId"></param>
    /// <param name="PlayerGuid"></param>
    public static void QuiteClub(uint ClubId,ulong PlayerGuid)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Leave_Club, 100);
        message.writeUInt32(ClubId);
        message.writeUInt64(PlayerGuid);
        SendMsg(message);
    }
    /// <summary>
    /// 获取俱乐部信息
    /// </summary>
    public static void GetClubInfo(uint id)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Query_Club_Info, 100);
        message.writeUInt32(id);
        SendMsg(message);
    }
    /// <summary>
    /// 获取俱乐部列表
    /// </summary>
    public static void GetClubList()
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_Query_Club_List, 100);
      
        SendMsg(message);
    }
    /// <summary>
    /// 发送亮牌协议
    /// </summary>
    public static void SendShowCard()
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_PeiPai, 100);
        message.writeUInt32(GameData.m_TableInfo.id);
        SendMsg(message);
    }

    /// <summary>
    /// 发送下注协议
    /// </summary>
    public static void SendDropChip(uint chip, Dictionary<byte, uint> OtherChipDic)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_ChipIn, 100);
        message.writeUInt32(GameData.m_TableInfo.id);
       
        message.writeUInt32(chip);

        message.writeInt32(OtherChipDic.Count);
        foreach (var item in OtherChipDic)
        {
            message.writeUInt8(item.Key);
            message.writeUInt32(item.Value);
        }
        SendMsg(message);
    }

  /// <summary>
  /// 牛牛抢庄
  /// </summary>
  /// <param name="RoomId"></param>
  /// <param name="QiangZhuang"></param>
  /// <param name="Count">加注倍数</param>
    public static void SendQiangZhuang(bool QiangZhuang,uint Count)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_QiangZhuang, 100);
        message.writeUInt32(GameData.m_TableInfo.id);
        message.writeBool(QiangZhuang);
        message.writeUInt32(Count);
      
        SendMsg(message);
    }

    // 把数据发给服务器
    static void SendMsg(NetworkMessage message)
    {
        
        Log.Debug(((Opcodes)message.cmd).ToString());
        ConnServer.m_WaitServerMsgCount++;
        Connection conn = ConnServer.global.Tcp_gateway[0];
        if (conn != null)
            conn.send(message);
    }

    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="cards"></param>
  public   static void SendPlayCard(List<uint> cards)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_PlayerOperate, 100);
        message.writeUInt32(GameData.m_TableInfo.id);
        message.writeUInt8((byte)CardOperateType.ChuPai);
        message.writeInt32(cards.Count);
        for (int i = 0; i < cards.Count; i++)
        {
            message.writeUInt32(cards[i]);
        }

        SendMsg(message);
    }

    /// <summary>
    /// 讨赏过牌
    /// </summary>
    /// <param name="cards"></param>
    public static void SendTaoShangGuo(List<uint> cards)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_PlayerOperate, 100);
        message.writeUInt32(GameData.m_TableInfo.id);
        message.writeUInt8((byte)CardOperateType.GuoPai);
        message.writeInt32(cards.Count);
        for (int i = 0; i < cards.Count; i++)
        {
            message.writeUInt32(cards[i]);
        }

        SendMsg(message);
    }

    /// <summary>
    /// 碰牌
    /// </summary>
    /// <param name="cards"></param>
    public static void SendWuDangHuPeng(List<uint> cards)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_PlayerOperate, 100);
        message.writeUInt32(GameData.m_TableInfo.id);
        message.writeUInt8((byte)CardOperateType.PengPai);
        message.writeInt32(cards.Count);
        for (int i = 0; i < cards.Count; i++)
        {
            message.writeUInt32(cards[i]);
        }

        SendMsg(message);
    }

    /// <summary>
    /// 杠牌牌
    /// </summary>
    /// <param name="cards"></param>
    public static void SendWuDangHuGang(List<uint> cards)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_PlayerOperate, 100);
        message.writeUInt32(GameData.m_TableInfo.id);
        message.writeUInt8((byte)CardOperateType.GangPai);
        message.writeInt32(cards.Count);
        for (int i = 0; i < cards.Count; i++)
        {
            message.writeUInt32(cards[i]);
        }

        SendMsg(message);
    }

    /// <summary>
    /// 过牌
    /// </summary>
    /// <param name="cards"></param>
    public static void SendWuDangHuGuo(List<uint> cards)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_PlayerOperate, 100);
     
        message.writeUInt32(GameData.m_TableInfo.id);
        message.writeUInt8((byte)CardOperateType.GuoPai);
        message.writeInt32(cards.Count);
        for (int i = 0; i < cards.Count; i++)
        {
            message.writeUInt32(cards[i]);
        }

        SendMsg(message);
    }

    /// <summary>
    /// 胡牌
    /// </summary>
    /// <param name="cards"></param>
    public static void SendWuDangHuHuPai(List<uint> cards)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_PlayerOperate, 100);
        message.writeUInt32(GameData.m_TableInfo.id);
        message.writeUInt8((byte)CardOperateType.HuPai);
        message.writeInt32(cards.Count);
        for (int i = 0; i < cards.Count; i++)
        {
            message.writeUInt32(cards[i]);
        }

        SendMsg(message);
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    /// <param name="round"></param>
    /// <param name="payindex"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static void  CreatRoom(uint round,uint payindex,float x,float y)
    {
        NetworkMessage message = NetworkMessage.Create((ushort)Opcodes.Client_PlayerCreateXYQPRoom, 100);
        message.writeUInt8((byte)round);
        message.writeUInt8((byte)payindex);

        SendMsg(message);
    }
}
