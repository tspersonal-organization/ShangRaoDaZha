using UnityEngine;

public class ClubMemInfoControl : MonoBehaviour {

    private UITexture _headTexture;
    private UILabel _nameLable;
    private UIButton _clickItem;

    private MemInfo _menInfo;
    private PlayerInfo _playerInfo;

    public ClubMemInfoControl(UIButton clickItem)
    {
        _clickItem = clickItem;
    }
    
    void Start()
    {
        if (ManagerScene.Instance.currentSceneType != SceneType.Game)
        {
            _headTexture = transform.Find("Head").GetComponent<UITexture>();
            _nameLable = transform.Find("Name").GetComponent<UILabel>();
            _clickItem = gameObject.GetComponent<UIButton>();
            _clickItem.onClick.Add(new EventDelegate(this.ClickItemClick));
        }
        else
        {
            _clickItem = gameObject.GetComponent<UIButton>();
            _clickItem.onClick.Add(new EventDelegate(this.ClickItemClickGame));
        }
    }
    
    public void SetData(MemInfo info)
    {
        _menInfo = info;
        if (ManagerScene.Instance.currentSceneType != SceneType.Game)
        {
            _headTexture = transform.Find("Head").GetComponent<UITexture>();
            _nameLable = transform.Find("Name").GetComponent<UILabel>();
            DownloadImage.Instance.Download(_headTexture, info.HeadId);
            _nameLable.text = info.Name;
        }
    }
    public void SetDataGame(PlayerInfo info)
    {
        _playerInfo = info;
    }

    /// <summary>
    /// 成员点击
    /// </summary>
    private void ClickItemClick()
    {
        GameData.ChoseMem = _menInfo;
        UIManager.Instance.ShowUiPanel(UIPaths.PanelPlayerInfo, OpenPanelType.MinToMax);
    }
    /// <summary>
    /// 成员点击
    /// </summary>
    private void ClickItemClickGame()
    {
        GameData.ChosePlayer = _playerInfo;
        UIManager.Instance.ShowUiPanel(UIPaths.PanelPlayerInfo, OpenPanelType.MinToMax);
    }
}
