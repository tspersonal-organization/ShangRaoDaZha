using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiangMaPanel : MonoBehaviour
{
    public UIButton MaskBtn;
    public GameObject ChiledOne;
    public GameObject ChiledTwo;
    public GameObject ChiledThree;
    public GameObject ChiledFour;

    void OnEnable()
    {
        try
        {

            if (!GameData.m_TableInfo.IsPiPei)
            {
                ChiledOne.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + WuDangHuGame.Instance.JiangMaList[0].ToString();
                ChiledTwo.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + WuDangHuGame.Instance.JiangMaList[1].ToString();
                ChiledThree.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + WuDangHuGame.Instance.JiangMaList[2].ToString();
                ChiledFour.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + WuDangHuGame.Instance.JiangMaList[3].ToString();
                ChiledOne.transform.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                ChiledTwo.transform.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                ChiledThree.transform.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                ChiledFour.transform.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                StartCoroutine("ScaleChange");
            }
            else
            {
                ChiledOne.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + WuDangHuGameJinBi.Instance.JiangMaList[0].ToString();
                ChiledTwo.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + WuDangHuGameJinBi.Instance.JiangMaList[1].ToString();
                ChiledThree.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + WuDangHuGameJinBi.Instance.JiangMaList[2].ToString();
                ChiledFour.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + WuDangHuGameJinBi.Instance.JiangMaList[3].ToString();
                ChiledOne.transform.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                ChiledTwo.transform.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                ChiledThree.transform.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                ChiledFour.transform.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                StartCoroutine("ScaleChange");
            }
          

            //EnsureJiangMa();//确定有效码
            //SetJiangMaCardColor();

        }
        catch (Exception e)
        {
            
        }
       
    }

    /// <summary>
    /// 确定有效的奖码数
    /// </summary>
    int HuPos;//胡牌的位置
    List<int> EffectNum = new List<int>();
    public void EnsureJiangMa()
    {
        EffectNum = new List<int>();
        for (int i = 1; i < 5; i++)
        {
            PlayerInfo info = GameDataFunc.GetPlayerInfo((byte)i);
            if (info.huType != FrameworkForCSharp.Utils.HuType.None)
            {
                HuPos = i;
            }
        }
        switch ((int)GameData.m_TableInfo.ZhuangPos)
        {
            case 1:
                switch (HuPos)
                {
                    case 1:
                        EffectNum.Add(1);
                        EffectNum.Add(5);
                        EffectNum.Add(9);
                        break;
                    case 2:
                        EffectNum.Add(2);
                        EffectNum.Add(6);
                        break;
                    case 3:
                        EffectNum.Add(3);
                        EffectNum.Add(7);
                        break;
                    case 4:
                        EffectNum.Add(4);
                        EffectNum.Add(8);
                        break;
                }
                break;
            case 2:
                switch (HuPos)
                {
                    case 1:
                        EffectNum.Add(4);
                        EffectNum.Add(8);
                      
                        break;
                    case 2:
                        EffectNum.Add(1);
                        EffectNum.Add(5);
                        EffectNum.Add(9);
                        break;
                    case 3:
                        EffectNum.Add(2);
                        EffectNum.Add(6);
                        break;
                    case 4:
                        EffectNum.Add(3);
                        EffectNum.Add(7);
                        break;
                }
                break;
            case 3:
                switch (HuPos)
                {
                    case 1:
                        EffectNum.Add(3);
                        EffectNum.Add(7);

                        break;
                    case 2:
                        EffectNum.Add(4);
                        EffectNum.Add(8);
                       
                        break;
                    case 3:
                        EffectNum.Add(1);
                        EffectNum.Add(5);
                        EffectNum.Add(9);
                        break;
                    case 4:
                        EffectNum.Add(2);
                        EffectNum.Add(6);
                        break;
                }
                break;
            case 4:
                switch (HuPos)
                {
                    case 1:
                        EffectNum.Add(2);
                        EffectNum.Add(6);

                        break;
                    case 2:
                        EffectNum.Add(3);
                        EffectNum.Add(7);
                      
                        break;
                    case 3:
                        EffectNum.Add(4);
                        EffectNum.Add(8);
                        break;
                    case 4:
                        EffectNum.Add(1);
                        EffectNum.Add(5);
                        EffectNum.Add(9);
                        break;
                }
                break;
        }
    }


    /// <summary>
    /// 设置中奖码的牌的颜色
    /// </summary>
    public void SetJiangMaCardColor()
    {
        if (GameData.m_TableInfo.IsPiPei)//匹配场
        {
            if (EffectNum.Count != 0)
            {

                for (int i = 0; i < WuDangHuGameJinBi.Instance.JiangMaList.Count; i++)
                {
                    if ((int)WuDangHuGameJinBi.Instance.JiangMaList[i] < 400)//万条同
                    {
                        int a = (int)WuDangHuGameJinBi.Instance.JiangMaList[i];

                        #region
                        for (int j = 0; j < EffectNum.Count; j++)
                        {
                            if (WuDangHuGameJinBi.Instance.JiangMaList[i] % 100 == EffectNum[j])
                            {
                                if (i == 0)
                                {
                                    ChiledOne.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 1)
                                {
                                    ChiledTwo.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 2)
                                {
                                    ChiledThree.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 3)
                                {
                                    ChiledFour.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }

                            }
                        }

                        #endregion
                    }
                    else if ((int)WuDangHuGameJinBi.Instance.JiangMaList[i] >= 400 && (int)WuDangHuGameJinBi.Instance.JiangMaList[i] < 800)
                    {
                        #region
                        for (int j = 0; j < EffectNum.Count; j++)
                        {
                            if ((WuDangHuGameJinBi.Instance.JiangMaList[i] / 100 - 3) == EffectNum[j])
                            {
                                if (i == 0)
                                {
                                    ChiledOne.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 1)
                                {
                                    ChiledTwo.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 2)
                                {
                                    ChiledThree.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 3)
                                {
                                    ChiledFour.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }

                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        for (int j = 0; j < EffectNum.Count; j++)
                        {
                            if ((WuDangHuGameJinBi.Instance.JiangMaList[i] / 100 - 6) == EffectNum[j])
                            {
                                if (i == 0)
                                {
                                    ChiledOne.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 1)
                                {
                                    ChiledTwo.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 2)
                                {
                                    ChiledThree.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 3)
                                {
                                    ChiledFour.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                            }
                        }

                        #endregion
                    }
                }

            }
        }
        else
        {

            if (EffectNum.Count != 0)
            {

                for (int i = 0; i < WuDangHuGame.Instance.JiangMaList.Count; i++)
                {
                    if ((int)WuDangHuGame.Instance.JiangMaList[i] < 400)//万条同
                    {
                        int a = (int)WuDangHuGame.Instance.JiangMaList[i];

                        #region
                        for (int j = 0; j < EffectNum.Count; j++)
                        {
                            if (WuDangHuGame.Instance.JiangMaList[i] % 100 == EffectNum[j])
                            {
                                if (i == 0)
                                {
                                    ChiledOne.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 1)
                                {
                                    ChiledTwo.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 2)
                                {
                                    ChiledThree.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 3)
                                {
                                    ChiledFour.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }

                            }
                        }

                        #endregion
                    }
                    else if ((int)WuDangHuGame.Instance.JiangMaList[i] >= 400 && (int)WuDangHuGame.Instance.JiangMaList[i] < 800)
                    {
                        #region
                        for (int j = 0; j < EffectNum.Count; j++)
                        {
                            if ((WuDangHuGame.Instance.JiangMaList[i] / 100 - 3) == EffectNum[j])
                            {
                                if (i == 0)
                                {
                                    ChiledOne.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 1)
                                {
                                    ChiledTwo.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 2)
                                {
                                    ChiledThree.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 3)
                                {
                                    ChiledFour.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }

                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        for (int j = 0; j < EffectNum.Count; j++)
                        {
                            if ((WuDangHuGame.Instance.JiangMaList[i] / 100 - 6) == EffectNum[j])
                            {
                                if (i == 0)
                                {
                                    ChiledOne.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 1)
                                {
                                    ChiledTwo.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 2)
                                {
                                    ChiledThree.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                                else if (i == 3)
                                {
                                    ChiledFour.transform.GetComponent<UISprite>().color = new Color(1, 1, 0.5f, 1);
                                }
                            }
                        }

                        #endregion
                    }
                }

            }
        }
      
    }
    // Use this for initialization
    void Start () {
        MaskBtn.onClick.Add(new EventDelegate(this.Reset));

    }
    private bool MoveEnd = false;
    private void Reset()
    {
        if (MoveEnd)
        {
            MoveEnd = false;
            ChiledOne.transform.localScale = Vector3.zero;
            ChiledTwo.transform.localScale = Vector3.zero;
            ChiledThree.transform.localScale = Vector3.zero;
            ChiledFour.transform.localScale = Vector3.zero;
            if (GameData.m_TableInfo.IsPiPei)
            {
                if (WuDangHuGameJinBi.Instance != null) WuDangHuGameJinBi.Instance.onGameOver();
            }
            else
            {
                if (WuDangHuGame.Instance != null) WuDangHuGame.Instance.onGameOver();
            }
           
            this.gameObject.SetActive(false);
        }
       

    }

    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator ScaleChange()
    {
        Hashtable args = new Hashtable();

        //放大的倍数  
        args.Add("scale", new Vector3(1, 1, 1));
        //args.Add("scale", msgNotContinue.transform);  
        // x y z 标示放大的倍数  
        args.Add("x", 1);
        args.Add("y", 1);
        args.Add("z", 1);
        args.Add("time", 0.5f);
        args.Add("easeType", iTween.EaseType.easeOutBack);
        iTween.ScaleTo(ChiledOne, args);
        yield return new WaitForSeconds(0.25f);
        iTween.ScaleTo(ChiledTwo, args);
        yield return new WaitForSeconds(0.25f);
        iTween.ScaleTo(ChiledThree, args);
        yield return new WaitForSeconds(0.25f);
        iTween.ScaleTo(ChiledFour, args);

        yield return new WaitForSeconds(0.5f);
        EnsureJiangMa();//确定有效码
        SetJiangMaCardColor();

        MoveEnd = true;

        if (GameData.m_TableInfo.IsPiPei)
        {
            Reset();
        }
    }
}
