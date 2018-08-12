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

public class ChuoPaiControl : MonoBehaviour {

    public GameObject MaskObj;
    public List<UISprite> CardList = new List<UISprite>();

    void Awake()
    {
         UIEventListener.Get(MaskObj).onClick = this.Close;
        //MaskObj.transform.GetComponent<UIButton>().onClick.Add(new EventDelegate(()=>
        //{
        //    this.gameObject.SetActive(false);
        //}));
    }
	// Use this for initialization
	void OnEnable () {
      
      
        PlayerInfo info = GameDataFunc.GetPlayerInfo(Player.Instance.guid);
        for (int i = 0; i < CardList.Count; i++)
        {
              CardList[i].transform.localPosition = new Vector3(0, -820, 0);
            CardList[i].transform.localRotation = Quaternion.EulerAngles(0,0,0);
        }
        for (int i = 0; i < info.localCardList.Count; i++)
        {
            CardList[i].spriteName = info.localCardList[i].ToString();
        }
        MoveUpAnim();
        StartCoroutine(RotateAnim());
      //  RotateAnim();
    }

    /// <summary>
    /// 牌向上移动
    /// </summary>
    private void MoveUpAnim()
    {
       
       
        for (int i = 0; i < CardList.Count; i++)
        {
            TweenPosition.Begin(CardList[i].gameObject, 0.5f, new Vector3(0,-226f,0));
          //  TweenRotation
        }
    }

   IEnumerator RotateAnim()
    {
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < CardList.Count; i++)
        {
            iTween.RotateTo(CardList[i].gameObject,new Vector3(0,0, (4 - i) * (-3f)),0.5f);
          
        }
    }
    public  void Close(GameObject go)
    {
        ResetPosition();
        this.gameObject.SetActive(false);
        NiuNiuGame.Instance.FanPaiAnim();
        ClientToServerMsg.SendShowCard();
    }


    /// <summary>
    /// 重置牌的位置
    /// </summary>
    private void ResetPosition()
    {

        CardList[0].transform.localPosition = new Vector3(54,-7,0);
        CardList[1].transform.localPosition = new Vector3(36, -3, 0);
        CardList[2].transform.localPosition = new Vector3(24, -1, 0);
        CardList[3].transform.localPosition = new Vector3(7, -1, 0);
        CardList[4].transform.localPosition = new Vector3(0, 0, 0);
    }


    // Update is called once per frame
    void Update () {
		
	}
}
