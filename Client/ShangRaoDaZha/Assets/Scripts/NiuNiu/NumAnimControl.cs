/*
* 开发人：滕俊
* 项目：
* 功能描述：
*
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumAnimControl : MonoBehaviour {

    UISprite typeSprite;
    UISprite NumSprite;
    UISprite NumSprite1;

   

   
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        typeSprite = transform.Find("type").GetComponent<UISprite>();
        NumSprite = transform.Find("Num").GetComponent<UISprite>();
        NumSprite1 = transform.Find("Num (1)").GetComponent<UISprite>();
    }
    /// <summary>
    /// 设置分数
    /// </summary>
    /// <param name="mark"></param>
    public void SetValue(int mark)
    {
        this.gameObject.SetActive(true);
        if (mark >= 0)
        {
            typeSprite.spriteName = "+";
            if (mark < 10)
            {
                NumSprite.spriteName = mark.ToString();
            }
            else
            {
                int mark1 = mark / 10;
                int mark2 = mark % 10;
                NumSprite1.gameObject.SetActive(true);
                NumSprite.spriteName = mark1.ToString();
                NumSprite1.spriteName = mark2.ToString();
            }
        }
        else
        {
            typeSprite.spriteName ="-";
            if (mark >-10)
            {
                NumSprite.spriteName = mark.ToString();
            }
            else
            {
                int mark1 = mark / 10;
                int mark2 = mark % 10;
                NumSprite1.gameObject.SetActive(true);
                NumSprite.spriteName = mark1.ToString();
                NumSprite1.spriteName = mark2.ToString();
            }
        }
        typeSprite.MakePixelPerfect();
        NumSprite.MakePixelPerfect();
        NumSprite1.MakePixelPerfect();
     
        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1.5f);
        NumSprite1.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
