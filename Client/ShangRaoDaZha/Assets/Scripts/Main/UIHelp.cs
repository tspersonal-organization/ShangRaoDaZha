using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelp : UIBase<UIHelp>
{
    public void Close()
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        UIManager.Instance.HideUIPanel(gameObject);
    }
}
