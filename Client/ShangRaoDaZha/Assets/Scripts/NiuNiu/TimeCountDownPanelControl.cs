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

public class TimeCountDownPanelControl : MonoBehaviour
{
    public static TimeCountDownPanelControl Instance;
    public UILabel TimeLable;
    public UILabel DescLable;

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
		
	}

    float timer = 0;
    int index = 0;
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0;
            index--;
            if (index >= 0)
            {
                TimeLable.text = index.ToString();

            }
            else
            {
                HideTimer();
                //TimeLable.transform.parent.gameObject.SetActive(false);
            }
        }
	}

    /// <summary>
    /// 设置倒计时
    /// </summary>
    /// <param name="countDown"></param>
    /// <param name="desc"></param>
    public void Init(int countDown,string desc)
    {
        this.gameObject.SetActive(true);
        TimeLable.gameObject.SetActive(true);
        DescLable.gameObject.SetActive(true);
        //this.gameObject.SetActive(true);
        index = countDown;
        DescLable.text = desc;

    }

    public void HideTimer()
    {
        TimeLable.gameObject.SetActive(false);
        DescLable.gameObject.SetActive(false);
    }
}
