using FrameworkForCSharp.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTools  {

    /// <summary>
    /// 数量排序
    /// </summary>
    /// <param name="Cardlist"></param>
    /// <returns></returns>
    public static List<uint> newlist =new List<uint>();
    public static Dictionary<uint, List<uint>> ValueAndCount = new Dictionary<uint, List<uint>>();

    public static List<uint> CardNumSort(List<uint> Cardlist)
    {
        newlist = new List<uint>();
        ValueAndCount = new Dictionary<uint, List<uint>>();
        List<uint> WangList = new List<uint>();
        List<uint> EightList = new List<uint>();
        List<uint> SevenList = new List<uint>();
        List<uint> SixList = new List<uint>();
        List<uint> FiveList = new List<uint>();
        List<uint> FourList = new List<uint>();
        List<uint> ThreeList = new List<uint>();
        List<uint> TwoList = new List<uint>();
        List<uint> OneList = new List<uint>();
        for (int i = 0; i < Cardlist.Count; i++)
        {
            if (Cardlist[i] == 516 || Cardlist[i] == 617)
            {
                WangList.Add(Cardlist[i]);
            }
            else
            {
                if (ValueAndCount.ContainsKey(Cardlist[i] % 100))
                {
                    ValueAndCount[Cardlist[i] % 100].Add(Cardlist[i]);
                }
                else
                {
                    ValueAndCount[Cardlist[i] % 100] = new List<uint>();
                    ValueAndCount[Cardlist[i] % 100].Add(Cardlist[i]);
                }
            }
           
        }
        foreach (var item in ValueAndCount)
        {
            switch (item.Value.Count)
            {
                case 1:
                    OneList.AddRange(item.Value);
                    break;
                case 2:
                    TwoList.AddRange(item.Value);
                    break;
                case 3:
                    ThreeList.AddRange(item.Value);
                    break;
                case 4:
                    FourList.AddRange(item.Value);
                    break;
                case 5:
                    FiveList.AddRange(item.Value);
                    break;
                case 6:
                    SixList.AddRange(item.Value);
                    break;
                case 7:
                    SevenList.AddRange(item.Value);
                    break;
                case 8:
                    EightList.AddRange(item.Value);
                    break;
            }
        }
        WangList = CardValueSort(WangList);
        EightList = CardValueSort(EightList);
        SevenList = CardValueSort(SevenList);
        SixList = CardValueSort(SixList);
        FiveList = CardValueSort(FiveList);
        FourList = CardValueSort(FourList);
        ThreeList = CardValueSort(ThreeList);
        TwoList = CardValueSort(TwoList);
        OneList = CardValueSort(OneList);

        newlist.AddRange(WangList);
        newlist.AddRange(EightList);
        newlist.AddRange(SevenList);
        newlist.AddRange(SixList);
        newlist.AddRange(FiveList);
        newlist.AddRange(FourList);
        newlist.AddRange(ThreeList);
        newlist.AddRange(TwoList);
        newlist.AddRange(OneList);
      

        return newlist;
    }

    /// <summary>
    /// 值排序
    /// </summary>
    /// <param name="Cardlist"></param>
    /// <returns></returns>
    public static List<uint> CardValueSort(List<uint> Cardlist)
    {
        for (int i = 0; i < Cardlist.Count; i++)
        {
            uint min = Cardlist[i] % 100;
            int minindex = i;

            for (int j = i; j < Cardlist.Count; j++)
            {
                if (Cardlist[j] % 100 < min)
                {
                    min = Cardlist[j] % 100;
                    minindex = j;
                }
            }

            if (minindex != i)
            {
                uint change = Cardlist[i];


                Cardlist[i] = Cardlist[minindex];
                Cardlist[minindex] = change;

            }
        }

        List<uint> newlist = new List<uint>();
        for (int i = Cardlist.Count-1; i >=0; i--)
        {
            newlist.Add(Cardlist[i]);
        }
        return newlist;
    }

    /// <summary>
    /// 是否踩炸弹
    /// </summary>
    /// <param name="ChoseList"></param>
    /// <param name="HandCard"></param>
    /// <returns></returns>
    public static bool IsSeperateBoom(List<uint>ChoseList,List<uint>HandCard)
    {

        if (ChoseList.Count == 0) return false;
        LandlordPokerType type;// = myQiPaiHelper.Instance.checkPokerType(ChoseList);
       // if (type == LandlordPokerType.Bomb) return false;
        for (int i = 0; i < ChoseList.Count; i++)
        {
            int index = 0;
            for (int j = 0; j < HandCard.Count; j++)
            {
                if (ChoseList[i] % 100 == HandCard[j] % 100)
                {
                    index++;
                }
                if (index >= 4)
                {
                    return true;
                }
            }
        }
        return false;
    }

    


    /// <summary>
    /// 王能当什么牌
    /// </summary>
    /// <param name="CardList"></param>
    /// <returns></returns>
    public static List<uint> JokerInsteadForNum(List<uint>CardList)
    {
        List<int> indexList = new List<int>();
        CardList = CardValueSort(CardList);
        LandlordPokerType type= LandlordPokerType.Bomb;// = myQiPaiHelper.Instance.checkPokerType(CardList);

        List<uint> newlist = new List<uint>();
        List<uint> JokerList = new List<uint>();
        List<uint> NoJokerList = new List<uint>();
        if (!CardList.Contains(516) && !CardList.Contains(617))//看是否包含王
        {
            return CardList;
        }
        else
        {

            for (int i = 0; i < CardList.Count; i++)
            {
                if (CardList[i] == 516 || CardList[i] == 617)
                {
                    JokerList.Add(CardList[i]);
                }
                else
                {
                    NoJokerList.Add(CardList[i]);
                }
            }
            NoJokerList = CardValueSort(NoJokerList);
        }
    


        //下面都是包含王的情况
        switch (type)
           {
            case LandlordPokerType.Signal:
                newlist =  CardList;
                break;
            case LandlordPokerType.Double:
                switch (NoJokerList.Count)
                {
                    case 0:
                        newlist = CardList;
                        break;
                    case 1:
                        newlist.Add(NoJokerList[0]);
                        newlist.Add(NoJokerList[0]%100+700);//补一张牌
                      
                        break;
                }
              
                break;
            case LandlordPokerType.Three:
                switch (NoJokerList.Count)
                {
                    case 0:
                        newlist = CardList;
                        break;
                    case 1:
                        newlist.Add(NoJokerList[0]);
                        newlist.Add(NoJokerList[0] % 100 + 700);//补一张牌
                        newlist.Add(NoJokerList[0] % 100 + 700);//补一张牌
                      
                        break;
                    case 2:
                        newlist.Add(NoJokerList[0]);
                        newlist.Add(NoJokerList[1]);
                        newlist.Add(NoJokerList[0] % 100 + 700);//补一张牌
                       
                        break;
                }
                break;
            case LandlordPokerType.ShuZi:
                newlist = GetMostStraigth(NoJokerList, CardList.Count);
                break;
            case LandlordPokerType.ContinueDoube:
                newlist = GetMostDoubleStraigth(NoJokerList, CardList.Count);
                break;
            case LandlordPokerType.ContinueThreeNone:
                newlist = GetMostThreeStraigth(NoJokerList, CardList.Count);
                break;
            case LandlordPokerType.Bomb:
                for (int i = 0; i < NoJokerList.Count; i++)
                {
                    newlist.Add(NoJokerList[i]);
                }
                for (int i = 0; i < JokerList.Count; i++)
                {
                    newlist.Add(NoJokerList[0]%100+700);
                }
                break;
          

        }
        return newlist;
    }
    /// <summary>
    /// 获取最大的顺子
    /// </summary>
    /// <param name="nojokerList"></param>
    /// <param name="count">多少个的顺子</param>
    /// <returns></returns>
    public static List<uint> GetMostStraigth(List<uint>nojokerList,int  count)
    {
        List<uint> returnList = new List<uint>();

        nojokerList = CardValueSort(nojokerList);
        if (nojokerList[nojokerList.Count - 1] % 100 + count - 1 > 14)
        {
            List<uint> NewList = new List<uint>();
            for (int i = 0; i < count; i++)
            {
                NewList.Add(700+14-(uint)i);
            }
            for (int i = 0; i < NewList.Count; i++)
            {
                for (int j =0; j < nojokerList.Count; j++)
                {
                    if (NewList[i] % 100 == nojokerList[j] % 100)
                    {
                        NewList[i] = nojokerList[j];
                    }
                }
            }

            NewList = CardValueSort(NewList);
            returnList = NewList;
        }
        else
        {
            List<uint> NewList = new List<uint>();
            for (int i = 0; i < count; i++)
            {
                if (i == 0)
                {
                    NewList.Add(nojokerList[nojokerList.Count-1]);
                }
                else
                {
                    NewList.Add(nojokerList[nojokerList.Count - 1] %100+(uint)i+700);
                }
            }
            for (int i = 0; i < NewList.Count; i++)
            {
                for (int j = 0; j < nojokerList.Count; j++)
                {
                    if (NewList[i] % 100 == nojokerList[j] % 100)
                    {
                        NewList[i] = nojokerList[j];
                    }
                }
            }

            returnList = NewList;
        }
        return returnList;
    }

    /// <summary>
    /// 获取最大的顺子
    /// </summary>
    /// <param name="nojokerList"></param>
    /// <param name="count">多少个的顺子</param>
    /// <returns></returns>
    public static List<uint> GetMostDoubleStraigth(List<uint> nojokerList, int count)
    {
        List<uint> returnList = new List<uint>();

        nojokerList = CardValueSort(nojokerList);
        if (nojokerList[nojokerList.Count - 1] % 100 + (count/2) - 1 > 14)
        {
            List<uint> NewList = new List<uint>();
            for (int i = 0; i < count/2; i++)
            {
                NewList.Add(700 + 14 - (uint)i);
                NewList.Add(700 + 14 - (uint)i);
            }
            List<int> ChangeIndex = new List<int>();//那些位置交换过
            for (int j = 0; j < nojokerList.Count; j++)
            {
                for (int i = 0; i < NewList.Count; i++)
                {
                    if (NewList[i] % 100 == nojokerList[j] % 100)
                    {
                        if (!ChangeIndex.Contains(i))
                        {
                            NewList[i] = nojokerList[j];
                            ChangeIndex.Add(i);
                            break;
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }

            NewList = CardValueSort(NewList);
            returnList = NewList;
        }
        else
        {
            List<uint> NewList = new List<uint>();
            for (int i = 0; i < count/2; i++)
            {
                if (i == 0)
                {
                    NewList.Add(nojokerList[nojokerList.Count - 1]);
                    NewList.Add(nojokerList[nojokerList.Count - 1] %100+700);
                }
                else
                {
                    NewList.Add(nojokerList[nojokerList.Count - 1] % 100 + (uint)i + 700);
                    NewList.Add(nojokerList[nojokerList.Count - 1] % 100 + (uint)i + 700);
                }
            }

            List<int> ChangeIndex = new List<int>();//那些位置交换过
          for (int j = 0; j < nojokerList.Count; j++)
           {
                for (int i = 0; i < NewList.Count; i++)
                {
                    if (NewList[i] % 100 == nojokerList[j] % 100)
                    {
                        if (!ChangeIndex.Contains(i))
                        {
                            NewList[i] = nojokerList[j];
                            ChangeIndex.Add(i);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                      
                    }
                }
            }

            NewList = CardValueSort(NewList);
            returnList = NewList;
        }
        return returnList;
    }


    /// <summary>
    /// 获取最大的三顺子
    /// </summary>
    /// <param name="nojokerList"></param>
    /// <param name="count">多少个的顺子</param>
    /// <returns></returns>
    public static List<uint> GetMostThreeStraigth(List<uint> nojokerList, int count)
    {
        List<uint> returnList = new List<uint>();

        nojokerList = CardValueSort(nojokerList);
        if (nojokerList[nojokerList.Count - 1] % 100 + (count /3) - 1 > 14)
        {
            List<uint> NewList = new List<uint>();
            for (int i = 0; i < count / 3; i++)
            {
                NewList.Add(700 + 14 - (uint)i);
                NewList.Add(700 + 14 - (uint)i);
                NewList.Add(700 + 14 - (uint)i);
            }
            List<int> ChangeIndex = new List<int>();//那些位置交换过
            for (int j = 0; j < nojokerList.Count; j++)
            {
                for (int i = 0; i < NewList.Count; i++)
                {
                    if (NewList[i] % 100 == nojokerList[j] % 100)
                    {
                        if (!ChangeIndex.Contains(i))
                        {
                            NewList[i] = nojokerList[j];
                            ChangeIndex.Add(i);
                            break;
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }

            NewList = CardValueSort(NewList);
            returnList = NewList;
        }
        else
        {
            List<uint> NewList = new List<uint>();
            for (int i = 0; i < count /3; i++)
            {
                if (i == 0)
                {
                    NewList.Add(nojokerList[nojokerList.Count - 1]);
                    NewList.Add(nojokerList[nojokerList.Count - 1] % 100 + 700);
                    NewList.Add(nojokerList[nojokerList.Count - 1] % 100 + 700);
                }
                else
                {
                    NewList.Add(nojokerList[nojokerList.Count - 1] % 100 + (uint)i + 700);
                    NewList.Add(nojokerList[nojokerList.Count - 1] % 100 + (uint)i + 700);
                    NewList.Add(nojokerList[nojokerList.Count - 1] % 100 + (uint)i + 700);
                }
            }

            List<int> ChangeIndex = new List<int>();//那些位置交换过
            for (int j = 1; j < nojokerList.Count; j++)
            {
                for (int i = 1; i < NewList.Count; i++)
                {
                    if (NewList[i] % 100 == nojokerList[j] % 100)
                    {
                        if (!ChangeIndex.Contains(i))
                        {
                            NewList[i] = nojokerList[j];
                            ChangeIndex.Add(i);
                            break;
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }

            NewList = CardValueSort(NewList);
            returnList = NewList;
        }
        return returnList;
    }



    public static LandlordPokerType CheckCardType(List<uint>Cardlist)
    {
        LandlordPokerType type = LandlordPokerType.None;

        return type;
    }


    public static bool IsSingle(List<uint>cardlist)
    {
        if (cardlist.Count != 1)
        {
            return false;
        }
        return true;
    }

    public static bool IsDouble(List<uint> cardlist)
    {
        if (cardlist.Count != 1)
        {
            return false;
        }
        return true;
    }
}
