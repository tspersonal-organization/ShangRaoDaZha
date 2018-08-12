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

public class AnimTest : MonoBehaviour {

    public List<GameObject> Points;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnGUI()
    {
        if (GUILayout.Button("移动1"))
        {
            RoundAnimControl.instance.SetPath(Points[0].transform.localPosition,Points[1].transform.localPosition);
        }
        if (GUILayout.Button("移动2"))
        {
            RoundAnimControl.instance.SetPath(Points[1].transform.localPosition, Points[2].transform.localPosition);
        }
        if (GUILayout.Button("移动3"))
        {
            RoundAnimControl.instance.SetPath(Points[2].transform.localPosition, Points[0].transform.localPosition);
        }
    }
}
