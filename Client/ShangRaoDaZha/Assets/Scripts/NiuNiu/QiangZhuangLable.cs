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

public class QiangZhuangLable : MonoBehaviour {


    void   OnEnable()
    {
        StartCoroutine(PlayAnim());
    }


    IEnumerator PlayAnim()
    {
        TweenScale.Begin(this.gameObject, 1f, new Vector3(1.2f, 1.2f, 0));
        yield return new WaitForSeconds(1f);
        transform.localScale = Vector3.one;
        this.gameObject.SetActive(false);
    }


    public void SetValue(bool Qiang)
    {
        if (Qiang)
        {
            transform.GetComponent<UISprite>().spriteName = "UI_game_icon_QiangZhuang";
        }
        else
        {
            transform.GetComponent<UISprite>().spriteName = "UI_game_icon_BuQiang";
        }
        transform.GetComponent<UISprite>().MakePixelPerfect();
        this.gameObject.SetActive(true);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
