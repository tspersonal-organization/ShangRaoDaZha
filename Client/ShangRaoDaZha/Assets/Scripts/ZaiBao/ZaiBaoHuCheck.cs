using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZaiBaoHuCheck  {

    /// <summary>
    /// 检查是否为七对
    /// </summary>
    /// <param name="HoldCard"></param>
    /// <param name="MagicCard"></param>
    /// <returns></returns>
    public static bool IsDoubleSeven(List<uint>HoldCard,uint MagicCard)
    {
        List<uint> MagicCardList = new List<uint>();
        List<uint> OtherCardList = new List<uint>();
        for (int i = 0; i < HoldCard.Count; i++)
        {
            if (HoldCard[i] ==MagicCard)
            {
                MagicCardList.Add(HoldCard[i]);
            }
            else
            {
                OtherCardList.Add(HoldCard[i]);
            }
        }
        OtherCardList.Sort((a, b) =>
        {
            return ((int)a - (int)b);
        });

        if (MagicCardList.Count > 1) return false;
        return true;
    }

    public static bool IsYaoJiu(List<uint>HoldCard,uint MagicCard)
    {
        List<uint> StandardYaoJiuList = new List<uint>()
        { 101,109,201,209,301,309};
        return true;
    }
}
