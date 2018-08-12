using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public GameObject card;
    public GameObject parent;
    public List<GameObject> LocalCardsObjList = new List<GameObject>();

    public GameObject JiangMaParent;
    public GameObject ChiledOne;
    public GameObject ChiledTwo;
    public GameObject ChiledThree;
    public GameObject ChiledFour;

    void OnEnable()
    {
        StartCoroutine("ScaleChange");
    }

    IEnumerator ScaleChange()
    {
        Hashtable args = new Hashtable();

        //放大的倍数  
        args.Add("scale", new Vector3(1, 1, 1));
        //args.Add("scale", msgNotContinue.transform);  
        // x y z 标示放大的倍数  
        args.Add("x", 1);
        args.Add("y", 1);
        args.Add("z", 1);
        args.Add("time", 0.5f);
        args.Add("easeType", iTween.EaseType.easeOutBack);
        iTween.ScaleTo(ChiledOne, args);
        yield return new WaitForSeconds(0.25f);
        iTween.ScaleTo(ChiledTwo, args);
        yield return new WaitForSeconds(0.25f);
        iTween.ScaleTo(ChiledThree, args);
        yield return new WaitForSeconds(0.25f);
        iTween.ScaleTo(ChiledFour, args);
    }
    // Use this for initialization
    void Start () {

        #region
        ////Vector3 v = AnimSystem.Instance.ToWorldPos(transform.position);
        //for (int i = 0; i < 30; i++)
        //{
        //    GameObject item = GameObject.Instantiate(card, parent.transform);
        //    item.transform.localPosition = new Vector3(0, 0, 0);
        //    item.transform.localScale = Vector3.one;
        //    item.SetActive(false);
        //    uint a = (uint)UnityEngine.Random.Range(203, 216);
        //    item.transform.GetComponent<Card>().SetValue(a);
        //    LocalCardsObjList.Add(item);//增加手牌
        //                                // UIEventListener.Get(item).onPress = OnPressCard;
        //    UIEventListener.Get(item).onDragOver = OnDragOver;
        //    UIEventListener.Get(item).onDragEnd = onDragEnd;

        //}

        //StartCoroutine(SendCard());
        //   CardSort();


        //List<uint> valuelist = new List<uint>() { 103,113,105,104,213,313,413,107,105,109,205,209,213,312,212,208,108,412,405,406,109,110,110,111,112,115,114,201,202,202,102,102,305};

        //valuelist = CardTools.CardValueSort(valuelist);
        //for (int i = 0; i < valuelist.Count; i++)
        //{
        //    Debug.Log(valuelist[i]);
        //}

        //List<uint> cardlist = new List<uint>() { 106, 206, 306, 107, 207, 307,208,516, 617,516  };
        //cardlist = CardTools.JokerInsteadForNum(cardlist);
        //for (int i = 0; i < cardlist.Count; i++)
        //{
        //    Debug.Log(cardlist[i]);
        //}

        #endregion

    }




    IEnumerator SendCard()
    {
        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {
            LocalCardsObjList[i].transform.localPosition = new Vector3(-450 + i * 30,0,0);
            LocalCardsObjList[i].SetActive(true);
            yield return new WaitForSeconds(0.05f);
        }

       // StartCoroutine(Reset());
        Reset();
        SetValue();
        yield return new WaitForSeconds(0.5f);
        Reset1();
      
    }


    void SetValue()
    {
        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {
            uint a = (uint)UnityEngine.Random.Range(203, 216);
            LocalCardsObjList[i].transform.GetComponent<Card>().SetValue(a);
        }
    }
    void  Reset()
    {
        Vector3 v = transform.position;
        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {
            AnimSystem.Instance.MoveTo(LocalCardsObjList[i],v);
          //  LocalCardsObjList[i].transform.localPosition = new Vector3(-450 + i * 30, 0, 0);
         
        }
      //  yield return new WaitForSeconds(3f);
      //  StartCoroutine(Reset1());
    }

   void Reset1()
    {
        Vector3 v = transform.position;
        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {
            AnimSystem.Instance.MoveTo(LocalCardsObjList[i], new Vector3((-450 +i * 30)/320f, v.y, 0));
          
        }
       // yield return null;
    }


    private void onDragEnd(GameObject go)
    {
        Debug.LogError("dragend  end");
    }

    void OnGUI()
    {
        if (GUILayout.Button("移动"))
        {
            this.Move();
        }
        if (GUILayout.Button("移动2"))
        {
            this.Move1();
        }
    }

    private void Move1()
    {
        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {
            Vector3 v = LocalCardsObjList[i].transform.position;
            // iTween.MoveTo(LocalCardsObjList[i], new Vector3((-450+i*30)/320f, (-150 +i * 10) / 320f, v.z), 1f);
            if (i % 2 == 0)
            {

                 AnimSystem.Instance.MoveTo(LocalCardsObjList[i], new Vector3(-450 + i * 30, v.y-200, v.z));
               // iTween.MoveTo(LocalCardsObjList[i], (new Vector3(v.x, (v.y - 200f) / 320f, v.z)), 1f);
                //LocalCardsObjList[i].transform.GetComponent<TweenPosition>().from = v;
                //LocalCardsObjList[i].transform.GetComponent<TweenPosition>().to = new Vector3(v.x, -200, v.z);
                //LocalCardsObjList[i].transform.GetComponent<TweenPosition>().PlayForward();
            }
            else
            {
                AnimSystem.Instance.MoveTo(LocalCardsObjList[i], new Vector3(-450 + i * 30, v.y +200, v.z));
              //  iTween.MoveTo(LocalCardsObjList[i], (new Vector3(v.x, (v.y + 200f) / 320f, v.z)), 1f);
                //LocalCardsObjList[i].transform.GetComponent<TweenPosition>().from = v;
                //LocalCardsObjList[i].transform.GetComponent<TweenPosition>().to = new Vector3(v.x, 200, v.z);
                //LocalCardsObjList[i].transform.GetComponent<TweenPosition>().PlayForward();
            }

        }
    }

    private void Move()
    {
        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {
            Vector3 v = LocalCardsObjList[i].transform.position;
            // iTween.MoveTo(LocalCardsObjList[i], new Vector3((450 -i * 30) / 320f, (150-i*10)/320f, v.z), 1f);

            //Vector3 newv = ToWorldPos(new Vector3(v.x, v.y + 200, v.z));
            if (i % 2 == 0)
            {

                 AnimSystem.Instance.MoveTo(LocalCardsObjList[i], new Vector3(450 - i * 30, v.y+200, v.z));
                // iTween.MoveTo(LocalCardsObjList[i], ToWorldPos(new Vector3(v.x, v.y+200, v.z)), 1f);
               // iTween.MoveTo(LocalCardsObjList[i], (new Vector3(v.x, (v.y + 200) / 320, v.z)), 1f);
                //LocalCardsObjList[i].transform.GetComponent<TweenPosition>().from = v ;
                //LocalCardsObjList[i].transform.GetComponent<TweenPosition>().to = new Vector3(v.x, 200, v.z);
                //LocalCardsObjList[i].transform.GetComponent<TweenPosition>().PlayForward();
            }
            else
            {
                 AnimSystem.Instance.MoveTo(LocalCardsObjList[i], new Vector3(450 - i * 30, v.y-200, v.z));
                // iTween.MoveTo(LocalCardsObjList[i], ToWorldPos(new Vector3(v.x,v.y-200,v.z)), 1f);
               // iTween.MoveTo(LocalCardsObjList[i], (new Vector3(v.x, (v.y - 200) / 320, v.z)), 1f);
                //LocalCardsObjList[i].transform.GetComponent<TweenPosition>().from = v;
                //LocalCardsObjList[i].transform.GetComponent<TweenPosition>().to = new Vector3(v.x, -200, v.z);
                //LocalCardsObjList[i].transform.GetComponent<TweenPosition>().PlayForward();
            }

        }
    }

    public Vector3 ToWorldPos(Vector3 vec)
    {
        Camera guiCamera = NGUITools.FindCameraForLayer(this.gameObject.layer);   //通过脚本所在物体的层获得相应层上的相机
        Vector3 pos = guiCamera.WorldToScreenPoint(vec);         //获取UI界面的屏幕坐标
                                                                 //  pos.z = 1f;//设置为零时转换后的pos全为0,屏幕空间的原因，被坑过的我提醒大家，切记要改！
                                                                 //  pos = guiCamera.ScreenToWorldPoint(pos);
        pos = Camera.main.ScreenToWorldPoint(pos);
        //将屏幕坐标转换为世界坐标
        return pos;
    }
    /// <summary>
    /// 玩家手牌排序
    /// </summary>
    public void CardSort()
    {
        for (int i = 0; i < LocalCardsObjList.Count; i++)
        {
            LocalCardsObjList[i].transform.localPosition = new Vector3(28 * i, 0, 0);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}

    public List<GameObject> CurChooseCardsObjList = new List<GameObject>();
    //牌的点击和拖动
    void OnPressCard(GameObject go, bool isPress)
    {

        Debug.LogError("press");
        if (isPress)
        {
            CurChooseCardsObjList.Clear();
        }
        else
        {
            if (CurChooseCardsObjList.Count == 0)
            {
                if (go.transform.localPosition.y == 20) go.transform.localPosition = new Vector3(go.transform.localPosition.x, 0);
                else go.transform.localPosition = new Vector3(go.transform.localPosition.x, 20);
            }
            else
            {
                for (int i = 0; i < CurChooseCardsObjList.Count; i++)
                {
                    if (CurChooseCardsObjList[i].transform.localPosition.y == 20) CurChooseCardsObjList[i].transform.localPosition = new Vector3(CurChooseCardsObjList[i].transform.localPosition.x, 0);
                    else CurChooseCardsObjList[i].transform.localPosition = new Vector3(CurChooseCardsObjList[i].transform.localPosition.x, 20);
                    CurChooseCardsObjList[i].GetComponent<UISprite>().color = Color.white;
                }
            }
        }
    }
    void OnDragOver(GameObject go)
    {
        Debug.LogError("dragover");
        go.GetComponent<UISprite>().color = Color.gray;
        CurChooseCardsObjList.Add(go);
    }
}
