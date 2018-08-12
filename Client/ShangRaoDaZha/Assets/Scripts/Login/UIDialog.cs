using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialog : UIBase<UIDialog>
{

	// Use this for initialization
	void Start ()
    {
        UIEventListener.Get(transform.Find("MB").gameObject).onClick = OnClick;
        transform.Find("Base").Find("desc").GetComponent<UILabel>().text = GameData.ResultCodeStr;
	}
    void OnClick(GameObject go)
    {
        UIManager.Instance.HideUIPanel(UIPaths.UIPanel_Dialog);
    }
	
}
