using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentPanel : UIBase<ContentPanel>
{

    public UIButton CloseBtn;
    public UILabel ContentLable;
	// Use this for initialization
	void Start () {
        CloseBtn.onClick.Add(new EventDelegate(this.Close));
        ContentLable.text = Player.Instance.content;

    }

    private void Close()
    {
        UIManager.Instance.HideUIPanel(UIPaths.ContentPanel);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
