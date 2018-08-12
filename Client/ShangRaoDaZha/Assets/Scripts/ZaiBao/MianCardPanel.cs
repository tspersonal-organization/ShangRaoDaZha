using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MianCardPanel : MonoBehaviour {

    public GameObject ChiledOne;

    void OnEnable()
    {
        try
        {
            ChiledOne.transform.GetComponent<UISprite>().spriteName = "card_local_max_" + GameData.m_TableInfo.MianCard.ToString();
         
            StartCoroutine("ScaleChange");
        }
        catch (Exception e)
        {

        }

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
        args.Add("time",1f);
        args.Add("easeType", iTween.EaseType.easeOutBack);

        //移动结束时调用，参数和上面类似
        args.Add("oncomplete", "AnimationEnd");
      
        args.Add("oncompletetarget", gameObject);
        iTween.ScaleTo(ChiledOne, args);
        yield return new WaitForSeconds(1f);
     

     
    }

    void AnimationEnd()
    {
        ChiledOne.transform.localScale = Vector3.zero;
        this.gameObject.SetActive(false);
    }
}
