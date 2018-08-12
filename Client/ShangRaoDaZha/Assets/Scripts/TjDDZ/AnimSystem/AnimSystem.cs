using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimSystem : UIBase<AnimSystem>
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	  	
	}

    public void MoveTo(GameObject target,Vector3 To, iTween.EaseType type = iTween.EaseType.linear, Action CallBack=null)
    {
        Hashtable args = new Hashtable();

        // Vector3 wordpos = new Vector3(To.x/320f,To.y/320f,0);

        Vector3 wordpos = ToWorldPos(To);
        //移动的速度，  
        args.Add("speed", 10f);
        args.Add("easeType", type);
        //移动的整体时间。如果与speed共存那么优先speed  
        args.Add("time", 1f);
        args.Add("position", wordpos);

        iTween.MoveTo(target,args);
    }

    public Vector3 ToWorldPos(Vector3 vec)
    {

      //  Debug.Log(" ngui:"+vec);
        Camera guiCamera = NGUITools.FindCameraForLayer(this.gameObject.layer);   //通过脚本所在物体的层获得相应层上的相机
        Vector3 pos = guiCamera.WorldToScreenPoint(vec);         //获取UI界面的屏幕坐标
      //  Debug.Log(" 屏幕:" + pos );                                                    //  pos.z = 1f;//设置为零时转换后的pos全为0,屏幕空间的原因，被坑过的我提醒大家，切记要改！
        pos = guiCamera.ScreenToWorldPoint(pos);
        //将屏幕坐标转换为世界坐标
      //  Debug.Log(" 世界:" + pos);
        return pos;
    }
}
