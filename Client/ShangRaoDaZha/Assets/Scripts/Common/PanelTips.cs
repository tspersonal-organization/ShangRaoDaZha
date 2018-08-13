using UnityEngine;

public class PanelTips : UIBase<PanelTips>
{
    // Use this for initialization
    void Start()
    {
        UIEventListener.Get(transform.Find("BgMask").gameObject).onClick = OnClick;
        transform.Find("Base").Find("desc").GetComponent<UILabel>().text = GameData.Tips;
    }
    void OnClick(GameObject go)
    {
        UIManager.Instance.HideUiPanel(UIPaths.PanelTips);
    }
}
