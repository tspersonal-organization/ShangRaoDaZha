using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWorkConnnectControl : UIBase<NetWorkConnnectControl>
{
    public UIButton MaskBtn;
	// Use this for initialization
	void Start () {
        MaskBtn.onClick.Add(new EventDelegate(this.Close));

    }

    private void Close()
    {
        //  if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        //{
        //    UIManager.Instance.HideUIPanel(UIPaths.ReconectTipPanel);
           
        //}
        //else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        //{

        //    UIManager.Instance.HideUIPanel(UIPaths.ReconectTipPanel);
           
        //}

      
    }

    // Update is called once per frame
    void Update () {
		
	}
}
