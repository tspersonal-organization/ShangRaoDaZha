using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShop : UIBase<UIShop>
{
    public GameObject[] btnBase;
    public GameObject btnClose;

	// Use this for initialization
	void Start ()
    {
        UIEventListener.Get(btnClose).onClick = OnClick;
        for (int i = 0; i < btnBase.Length; i++)
            UIEventListener.Get(btnBase[i]).onClick = OnClick;
	}
	
    void OnClick(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        if (go == btnClose)
        {
            UIManager.Instance.HideUIPanel(UIPaths.UIPanel_Shop);
        }
        else
        {
            int index = int.Parse(go.name.Substring(6));
            InAppPurchasing.Instance.BuyProduct(index);
        }
    }
}
