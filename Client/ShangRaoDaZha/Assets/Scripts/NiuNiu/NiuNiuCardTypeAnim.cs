/*
* 开发人：滕俊
* 项目：
* 功能描述：
*
*
*/

using FrameworkForCSharp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiuNiuCardTypeAnim : MonoBehaviour {

    public UISprite SelfSprite;

    UISprite typeSprite;
    UISprite NumSprite;
    void OnEnable()
    {
        typeSprite = transform.Find("type").GetComponent<UISprite>();
        NumSprite = transform.Find("Num").GetComponent<UISprite>();
        SelfSprite.transform.GetComponent<TweenScale>().PlayForward();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="PeiPaiType"></param>
    /// <param name="FanBeiCount"></param>
    public void SetCardValue(NNType PeiPaiType, UInt32 FanBeiCount)
    {
        SelfSprite.gameObject.SetActive(true);
        SelfSprite.spriteName = "UI_game_icon_CardType_"+(int)PeiPaiType;
        SelfSprite.MakePixelPerfect();
        if (FanBeiCount > 1)
        {
            typeSprite.gameObject.SetActive(true);
            NumSprite.gameObject.SetActive(true);
            NumSprite.spriteName = FanBeiCount.ToString();
            NumSprite.MakePixelPerfect();
        }
       
    }


    /// <summary>
    /// 重置
    /// </summary>
    public void ResetToBegin()
    {
        SelfSprite.gameObject.SetActive(false);
        transform.Find("type").GetComponent<UISprite>().gameObject.SetActive(false);
        transform.Find("Num").GetComponent<UISprite>().gameObject.SetActive(false);
        SelfSprite.GetComponent<TweenScale>().ResetToBeginning();
    }
}
