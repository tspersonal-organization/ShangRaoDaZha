using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKTanPaiPanel : MonoBehaviour {

    public List<GameObject> PlayerList;
    public GameObject CardObj;

    public UIButton CloseBtn;//关闭按钮
	// Use this for initialization
	void Start () {
        CloseBtn.onClick.Add(new EventDelegate(this.CloseClick));

    }

    void CloseClick()
    {
        if (gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
            UIManager.Instance.ShowUiPanel(UIPaths.PanelGameOverSmall);
        }
       
    }
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        SetData();
        StartCoroutine(TimeDely());
    }

    Dictionary<int, List<GameObject>> PosAndCardList = new Dictionary<int, List<GameObject>>();
    public void SetData()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerList[i].SetActive(false);
        }

        for (int i = 0; i < PartGameOverControl.instance.SettleInfoList.Count; i++)
        {
            PartGameOverControl.instance.SettleInfoList[i].LeftCardList = CardTools.CardValueSort(PartGameOverControl.instance.SettleInfoList[i].LeftCardList);
            PlayerList[i].SetActive(true);
            PlayerList[i].transform.GetComponent<UILabel>().text= GameDataFunc.GetPlayerInfo((byte)PartGameOverControl.instance.SettleInfoList[i].pos).name.ToString();
            PosAndCardList[PartGameOverControl.instance.SettleInfoList[i].pos] = new List<GameObject>();
            int count = PosAndCardList[PartGameOverControl.instance.SettleInfoList[i].pos].Count;
            for (int j = 0; j < count; j++)
            {
                Destroy(PosAndCardList[PartGameOverControl.instance.SettleInfoList[i].pos][j]);
            }
            PosAndCardList[PartGameOverControl.instance.SettleInfoList[i].pos] = new List<GameObject>();
            for (int j = 0; j < PartGameOverControl.instance.SettleInfoList[i].LeftCardList.Count; j++)
            {
                GameObject g = GameObject.Instantiate(CardObj, PlayerList[i].transform.Find("CardPoint"));
                g.transform.localScale = new Vector3(0.6f, 0.6f, 0.4f);
                g.transform.localPosition = new Vector3(j * 25, 0, 0);
                g.SetActive(true);
                g.transform.GetComponent<Card>().SetValue(PartGameOverControl.instance.SettleInfoList[i].LeftCardList[j]);
                PosAndCardList[PartGameOverControl.instance.SettleInfoList[i].pos].Add(g);
            }
        }

    }


    IEnumerator TimeDely()
    {
        yield return new WaitForSeconds(5f);
        CloseClick();
    }
}
