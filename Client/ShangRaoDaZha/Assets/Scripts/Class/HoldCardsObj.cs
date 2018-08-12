using FrameworkForCSharp.Utils;
using System.Collections.Generic;
using UnityEngine;

public class HoldCardsObj
{
    public byte pos;
    public LocalViewDirection LVD;
    /// <summary>
    /// 手上牌
    /// </summary>
    public List<GameObject> holdObjList = new List<GameObject>();
    /// <summary>
    /// 打出去的牌
    /// </summary>
    public List<GameObject> outObjList = new List<GameObject>();
    /// <summary>
    /// 打出去的面牌牌
    /// </summary>
    public List<GameObject> outMianCardObjList = new List<GameObject>();
    /// <summary>
    /// 吃碰杠对象
    /// </summary>
    public List<PengOrGangObj> pengGangList = new List<PengOrGangObj>();
}

public class PengOrGangObj
{
    public byte pos;
    public CardOperateType opType;
    public GameObject objBase;
}
