using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCountDown : MonoBehaviour {


    public UILabel timerShowLable;//

    void Awake()
    {
        timerShowLable = transform.Find("Label").GetComponent<UILabel>();
    }
	// Use this for initialization
	void Start () {
		
	}

    bool CanCount = false;//是否开始倒计时
    float timer = 0;
    float timer1 = 1;
    float totalTime = 0;
	// Update is called once per frame
	void Update () {
        if (CanCount)
        {
            timer += Time.deltaTime;
            if (timer > timer1)
            {
                totalTime--;
                timer = 0;
                timerShowLable.text = totalTime.ToString();
              
                if (totalTime == 0)
                {
                    CanCount = false;
                }
            }
        }
	}

    /// <summary>
    /// 开始倒计时
    /// </summary>
    /// <param name="time"></param>
    public void SetTimeCountDown(int time)
    {
        totalTime = time;
        CanCount = true;
    }
}
