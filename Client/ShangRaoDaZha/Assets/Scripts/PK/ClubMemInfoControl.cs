using UnityEngine;

public class ClubMemInfoControl : MonoBehaviour {

    public UITexture HeadTexture;
    public UILabel NameLable;
    public UIButton ClickItem;

    private MemInfo DataInfo;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    public void SetData(MemInfo info)
    {
        DataInfo = info;
        DownloadImage.Instance.Download(HeadTexture,info.headid);
        NameLable.text = info.name;


    }
	// Use this for initialization
	void Start () {
       
        ClickItem.onClick.Add(new EventDelegate(this.ClickItemClick));

    }

    /// <summary>
    /// 成员点击
    /// </summary>
    private void ClickItemClick()
    {
        GameData.ChoseMem = DataInfo;
        UIManager.Instance.ShowUiPanel(UIPaths.PanelPlayerInfo, OpenPanelType.MinToMax);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
