using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : IComparer
{
    public uint data;
    public int Index;//唯一标识
    public int Compare(object x, object y)
    {
        CardData card1 = (CardData)x;
        CardData card2 = (CardData)y;
        int result = 0;
        if (card1.data % 100 > card2.data % 100)
        {
            result = 1;
            return result;
        }
        else
        {
            return result;
        }
      
    }

    public void SetData(uint data)
    {
        this.data = data;
    }
}
