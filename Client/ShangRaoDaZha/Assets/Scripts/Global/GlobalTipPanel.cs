using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTipPanel : UIBase<GlobalTipPanel>
{

    public UILabel TipLable;
 

    void OnEnable()
    {
        TipLable.text = GameData.GlobleTipString;
        StartCoroutine(PlayAnim());
    }
	// Use this for initialization
	IEnumerator PlayAnim () {
       //
        TipLable.transform.GetComponent<TweenPosition>().PlayForward();
        TipLable.transform.GetComponent<TweenColor>().PlayForward();
        yield return new WaitForSeconds(1f);
        TipLable.transform.GetComponent<TweenPosition>().ResetToBeginning();
        TipLable.transform.GetComponent<TweenColor>().ResetToBeginning();
        UIManager.Instance.HideUIPanel(UIPaths.GlobleTipPanel);
       // this.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
