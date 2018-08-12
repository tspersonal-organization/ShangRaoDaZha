/*
* 开发人：滕俊
* 项目：
* 功能描述：
*
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubReNamePanelControl : UIBase<ClubReNamePanelControl>
{


    public UIButton SureBtn;
    public UIInput NameInput;
    public UIButton MaskBtn;

	// Use this for initialization
	void Start () {
        SureBtn.onClick.Add(new EventDelegate(this.ReNameClick));
        MaskBtn.onClick.Add(new EventDelegate(()=>
        {
          //  this.gameObject.SetActive(false);
        }));
    }

    private void ReNameClick()
    {
        if(NameInput.value!="")
        ClientToServerMsg.ReNameClub((uint)GameData.CurrentClubInfo.Id, NameInput.value);
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
