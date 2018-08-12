using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataFunc : GameData
{
    public static PlayerInfo GetPlayerInfo(byte pos)
    {
        for (int i = 0; i < m_PlayerInfoList.Count; i++)
        {
            if (m_PlayerInfoList[i].pos == pos) return m_PlayerInfoList[i];
        }
        return null;
    }

    /// <summary>
    /// 移除玩家
    /// </summary>
    /// <param name="pos"></param>
    public static void  RemovePlayerinfo(byte pos)
    {
        for (int i = 0; i < m_PlayerInfoList.Count; i++)
        {
            if (m_PlayerInfoList[i].pos == pos)
            {
                m_PlayerInfoList.Remove(m_PlayerInfoList[i]);
                return;
            }
        }
    }
    /// <summary>
    /// 获取特定牌的张数
    /// </summary>
    /// <param name="CardID"></param>
    /// <returns></returns>
    public static TableCardInfo GetTableCardInfoByID(uint CardID)
    {
        TableCardInfo resultInfo = new TableCardInfo();
        for (int i = 0; i < GameData.AllCardsInfo.Count; i++)
        {
            if (GameData.AllCardsInfo[i].CardNumber == CardID)
            {
                resultInfo = AllCardsInfo[i];
                break;
            }
        }
        return resultInfo;
    }

    public static PlayerInfo GetPlayerInfo(ulong guid)
    {
        for (int i = 0; i < m_PlayerInfoList.Count; i++)
        {
            if (m_PlayerInfoList[i].guid == guid) return m_PlayerInfoList[i];
        }
        return null;
    }
    public static PlayerInfo GetPlayerInfo(LocalViewDirection lvd)
    {
        for (int i = 0; i < m_PlayerInfoList.Count; i++)
        {
            if (lvd == m_PlayerInfoList[i].LVD)
                return m_PlayerInfoList[i];
        }
        return null;
    }
    public static void RemovePlayer(byte pos)
    {
        for (int i = 0; i < m_PlayerInfoList.Count; i++)
        {
            if (pos == m_PlayerInfoList[i].pos)
            {
                m_PlayerInfoList.RemoveAt(i);
                break;
            }
        }
    }
    public static void RemoverHoldCard(uint card, byte pos)
    {
        PlayerInfo info = GetPlayerInfo(pos);
        for (int i = 0; i < info.localCardList.Count; i++)
        {
            if (card == info.localCardList[i])
            {
                info.localCardList.RemoveAt(i);
                break;
            }
        }
    }
    public static void ClearData()
    {
        for (int i = 0; i < m_PlayerInfoList.Count; i++)
        {
            m_PlayerInfoList[i].localCardList.Clear();
            m_PlayerInfoList[i].outCardList.Clear();
            m_PlayerInfoList[i].operateCardList.Clear();
            m_PlayerInfoList[i].limitPengCardList.Clear();
        }
    }
    /// <summary>
    /// 移除一个打出的牌
    /// </summary>
    /// <param name="pos"></param>
    public static void RemoveOutCardInfo(byte pos)
    {
        PlayerInfo info = GetPlayerInfo(pos);
        info.outCardList.RemoveAt(info.outCardList.Count - 1);
    }

    public static HoldCardsObj GetHoldCardObj(byte pos)
    {
        for (int i = 0; i < m_HoldCardsList.Count; i++)
        {
            if (pos == m_HoldCardsList[i].pos)
            {
                return m_HoldCardsList[i];
            }
        }
        return null;
    }
    public static HoldCardsObj GetHoldCardObj(LocalViewDirection LVD)
    {
        for (int i = 0; i < m_HoldCardsList.Count; i++)
        {
            if (LVD == m_HoldCardsList[i].LVD)
            {
                return m_HoldCardsList[i];
            }
        }
        return null;
    }
    public static void RemoveOutCardObj(byte pos)
    {
        HoldCardsObj info = GetHoldCardObj(pos);
        GameObject.Destroy(info.outObjList[info.outObjList.Count - 1]);
        info.outObjList.RemoveAt(info.outObjList.Count - 1);
    }

    public static void RemoveOutCardObj(LocalViewDirection lvd)
    {
        HoldCardsObj info = GetHoldCardObj(lvd);
        GameObject.Destroy(info.outObjList[info.outObjList.Count - 1]);
        info.outObjList.RemoveAt(info.outObjList.Count - 1);
    }
    public static void ClearDataObj()
    {
        for (int i = 0; i < m_HoldCardsList.Count; i++)
        {
            m_HoldCardsList[i].holdObjList.Clear();
            m_HoldCardsList[i].outObjList.Clear();
            m_HoldCardsList[i].pengGangList.Clear();
        }
    }
    public static void RemovePlayerObj(byte pos)
    {
        for (int i = 0; i < m_HoldCardsList.Count; i++)
        {
            if (pos == m_HoldCardsList[i].pos)
            {
                m_HoldCardsList.RemoveAt(i);
                break;
            }
        }
    }
    public static void RemoveHoldCardObj(uint card, byte pos)
    {
        HoldCardsObj info = GetHoldCardObj(pos);
        for (int i = 0; i < info.holdObjList.Count; i++)
        {
            if (card == uint.Parse(info.holdObjList[i].name))
            {
                if (info.holdObjList[i] != null)
                    GameObject.Destroy(info.holdObjList[i]);
                info.holdObjList.RemoveAt(i);
                break;
            }
        }
    }

    public static void RemoveHoldCardObj(uint card, LocalViewDirection lvd)
    {
        HoldCardsObj info = GetHoldCardObj(lvd);
        for (int i = 0; i < info.holdObjList.Count; i++)
        {
            if (card == uint.Parse(info.holdObjList[i].name))
            {
                if (info.holdObjList[i] != null)
                    GameObject.Destroy(info.holdObjList[i]);
                info.holdObjList.RemoveAt(i);
                break;
            }
        }
    }

}
