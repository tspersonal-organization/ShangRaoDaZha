using UnityEngine;

public class PanelDialog : UIBase<PanelDialog>
{

	// Use this for initialization
	void Start ()
    {
        UIEventListener.Get(transform.Find("BgMask").gameObject).onClick = OnClick;
        transform.Find("Base").Find("desc").GetComponent<UILabel>().text = GameData.ResultCodeStr;
	}
    void OnClick(GameObject go)
    {
        UIManager.Instance.HideUiPanel(UIPaths.PanelDialog);
    }
	
}
