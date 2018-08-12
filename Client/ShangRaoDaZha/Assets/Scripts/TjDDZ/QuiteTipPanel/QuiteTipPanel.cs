using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuiteTipPanel : UIBase<QuiteTipPanel>
{

    public UIButton SureBtn;
    public UIButton CancleBtn;
	// Use this for initialization
	void Start () {
        SureBtn.onClick.Add(new EventDelegate(this.QuiteGame));
        CancleBtn.onClick.Add(new EventDelegate(this.ClosePanel));
         
       

    }

    /// <summary>
    /// 关闭弹窗
    /// </summary>
    private void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    private void QuiteGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
