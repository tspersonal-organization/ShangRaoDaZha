using FrameworkForCSharp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : UIBase<SoundControl>
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 判定出牌音效
    /// </summary>
    /// <param name="cardlist"></param>
    public void CheckOperateSoundType(List<uint> cardlist, int sex)
    {
        cardlist = CardTools.CardValueSort(cardlist);//按值排序
        switch (cardlist.Count)
        {
            case 0:
                int a = UnityEngine.Random.Range(1, 3);
                if (sex == 1)
                {
                    SoundManager.Instance.PlaySound(UIPaths.GUOPAI_MAN + a.ToString());
                }
                else
                {
                    SoundManager.Instance.PlaySound(UIPaths.GUOPAI_WOMAN + a.ToString());
                }

                break;
            case 1:
                int value = (int)cardlist[cardlist.Count - 1] % 100;
                if (sex == 1)
                {
                    SoundManager.Instance.PlaySound(UIPaths.SINGLE_MAN + value.ToString());
                }
                else
                {
                    SoundManager.Instance.PlaySound(UIPaths.SINGLE_WOMAN + value.ToString());
                }
                // SoundManager.Instance.PlaySound(UIPaths.SINGLE + value.ToString());
                break;
            case 2:
                int value1 = (int)cardlist[cardlist.Count - 1] % 100;

                if (sex == 1)
                {
                    SoundManager.Instance.PlaySound(UIPaths.DOUBLE_MAN + value1.ToString());
                }
                else
                {
                    SoundManager.Instance.PlaySound(UIPaths.DOUBLE_WOMAN + value1.ToString());
                }
                //if (value1 > 2 && value1 < 16)
                //{
                //    SoundManager.Instance.PlaySound(UIPaths.DOUBLE + value1.ToString());
                //}
                //else
                //{
                //    int  value2 = (int)cardlist[1] % 100;
                //    if (value2 > 2 && value2 < 16)
                //    {
                //        SoundManager.Instance.PlaySound(UIPaths.DOUBLE + value2.ToString());
                //    }
                //    else
                //    {
                //       //对王
                //    }
                //}

                break;
            case 3:
                int value2 = (int)cardlist[cardlist.Count - 1] % 100;
                // SoundManager.Instance.PlaySound(UIPaths.THREECARD);
                if (sex == 1)
                {
                    SoundManager.Instance.PlaySound(UIPaths.TRIPLE_MAN + value2.ToString());
                }
                else
                {
                    SoundManager.Instance.PlaySound(UIPaths.TRIPLE_WOMAN + value2.ToString());
                }
                break;

            default:
                LandlordPokerType type = LandlordPokerType.None;
                try
                {
                  //  type = myQiPaiHelper.Instance.checkPokerType(cardlist);
                }
                catch (Exception e)
                {

                }


                switch (type)
                {
                    case LandlordPokerType.Bomb:
                        if (sex == 1)
                        {
                            SoundManager.Instance.PlaySound(UIPaths.BOOM_MAN);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySound(UIPaths.BOOM_WOMAN);
                        }
                        SoundManager.Instance.PlaySound(UIPaths.BOOM);
                        CardTypeAnim.Instance.PlayBoomAnim();
                        break;
                    case LandlordPokerType.ContinueDoube:
                        if (sex == 1)
                        {
                            SoundManager.Instance.PlaySound(UIPaths.LIANDUI_MAN);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySound(UIPaths.LIANDUI_WOMAN);
                        }
                        // SoundManager.Instance.PlaySound(UIPaths.LIANDUI);
                        CardTypeAnim.Instance.PlayLianDuiAnim();
                        break;
                    case LandlordPokerType.ShuZi:
                        if (sex == 1)
                        {
                            SoundManager.Instance.PlaySound(UIPaths.SHUNZHI_MAN);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySound(UIPaths.SHUNZHI_WOMAN);
                        }
                        // SoundManager.Instance.PlaySound(UIPaths.SHUNZHI);
                        CardTypeAnim.Instance.PlayShunZhiAnim();
                        break;
                    case LandlordPokerType.WangBomb:
                        if (sex == 1)
                        {
                            SoundManager.Instance.PlaySound(UIPaths.ROCK_MAN);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySound(UIPaths.ROCK_WOMAN);
                        }
                        SoundManager.Instance.PlaySound(UIPaths.ROCK);
                        CardTypeAnim.Instance.PlayRockAnim();
                        break;

                }
                break;
        }
    }


    /// <summary>
    /// 播放
    /// </summary>
    /// <param name="sex"></param>
    /// <param name="index"></param>
    public void PlayChatSound(int sex, int index)
    {
        if (sex == 1)
        {
            SoundManager.Instance.PlaySound(UIPaths.CHAT_MAN + (index + 1).ToString());
        }
        else
        {
            SoundManager.Instance.PlaySound(UIPaths.CHAT_WOMAN + (index + 1).ToString());
        }

    }

    /// <summary>
    /// 麻将播放
    /// </summary>
    /// <param name="sex"></param>
    /// <param name="index"></param>
    public void PlayMJChatSound(int sex, int index)
    {
        if (sex == 1)
        {
            SoundManager.Instance.PlaySound(UIPaths.MJCHAT_MAN + (index + 1).ToString());
        }
        else
        {
            SoundManager.Instance.PlaySound(UIPaths.MJCHAT_WOMAN + (index + 1).ToString());
        }

    }


    /// <summary>
    /// 牛牛播放牌型音效
    /// </summary>
    /// <param name="set"></param>
    /// <param name="PeiPaiType"></param>
    public void PlayNiuNiuCardType(int sex, NNType PeiPaiType)
    {
        if (sex == 1)//男
        {
            if ((int)PeiPaiType < 11)
            {
                SoundManager.Instance.PlaySound(UIPaths.NN_MAN + ((int)PeiPaiType).ToString() + "k");
            }
            else
            {
                SoundManager.Instance.PlaySound(UIPaths.NN_Special);
            }
          
        }
        else
        {
            if ((int)PeiPaiType < 11)
            {
                SoundManager.Instance.PlaySound(UIPaths.NN_WOMAN + ((int)PeiPaiType).ToString() + "k");
            }
            else
            {
                SoundManager.Instance.PlaySound(UIPaths.NN_Special);
            }

        }
    }
}
