public class DzPanelNotice : UIBase<DzPanelNotice>
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
        UIManager.Instance.HideUiPanel(UIPaths.PanelNotice);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
