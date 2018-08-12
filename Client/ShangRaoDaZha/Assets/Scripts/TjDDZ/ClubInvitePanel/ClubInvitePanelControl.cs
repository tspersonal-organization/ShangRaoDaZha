/*
* 开发人：滕俊
* 项目：
* 功能描述：
*
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubInvitePanelControl : UIBase<ClubInvitePanelControl>
{

    public GameObject ClubInviteItem;
    public Transform ItemParent;
    public UIButton CloseBtn;
	// Use this for initialization
	void Start () {
        CloseBtn.onClick.Add(new EventDelegate(()=>
        {
            this.gameObject.SetActive(false);
        }));

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        InitData();
    }

    private List<GameObject> CreatObj = new List<GameObject>();
    public  void InitData()
    {
        int count = CreatObj.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(CreatObj[i]);
        }
        CreatObj = new List<GameObject>();
        for (int i = 0; i < GameData.InviteClubIdAndName.Count; i++)
        {
            GameObject g = Instantiate(ClubInviteItem, ItemParent);
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = new Vector3(0,147-(i*100),0);
            g.transform.GetComponent<ClubInviteItemControl>().SetValue(GameData.InviteClubIdAndName[i]);
            g.SetActive(true);
            CreatObj.Add(g);
        }
    }
}
