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

public class ChangeSprite : MonoBehaviour {

    public UISprite SelfSprite;

    void OnEnable()
    {
        SelfSprite = transform.GetComponent<UISprite>();
    }
	// Use this for initialization
	void Start () {
		
	}

    float timer = 0;
    float Timer1 = 0.05f;
    float index = 0;
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > Timer1)
        {
            index++;
            timer = 0;
            if (index > 9)
            {
                index = 1;
            }
            SelfSprite.spriteName = "comic_game_gold_" + index.ToString();
        }


    }
}
