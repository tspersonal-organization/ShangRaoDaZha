using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour , IComparable
{

    public UISprite image;
    public int index;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 设置value
    /// </summary>
    public void SetValue(uint value,int Index=-1)
    {
        image.spriteName = value.ToString();
        if (value / 100 == 7)
        {
            transform.Find("Sprite").gameObject.SetActive(true);
        }
        this.index = Index;
    }

    /// <summary>
    /// 排序用的
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int CompareTo(object obj)
    {
        Card card1 = ((GameObject)obj).transform.GetComponent<Card>();
        int result =0;
        if (((int.Parse(card1.image.spriteName)) % 100) > (int.Parse(image.spriteName) % 100))
        {
            result = 1;
            return result;
        }
        else if (((int.Parse(card1.image.spriteName)) % 100) < (int.Parse(image.spriteName) % 100))
        {
            result = 0;
            return result;
        }
        else
        {
            return result;
        }
    }

    /// <summary>
    /// 放大缩小
    /// </summary>
    public void ScaleMove()
    {
        Hashtable args = new Hashtable();



        args.Add("scale",Vector3.one);
        //移动的速度，  
      
        args.Add("easeType", iTween.EaseType.linear);
        //移动的整体时间。如果与speed共存那么优先speed  
        args.Add("time", 0.3f);
      

        iTween.ScaleTo(this.gameObject, args);
    }
}
