using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBtnDetectFace : MonoBehaviour {

    private void Awake()
    {
        GetComponent<UIButton>().onClick.Add(new EventDelegate(delegate {
            Debug.Log("颜值按钮 被点击了！！");
            UIManager.Instance.ShowUiPanel(ConstDetectFace.FacePanel,OpenPanelType.MinToMax);
        }));
    }

    // Use this for initialization
    void Start () {

	}

	// Update is called once per frame
	void Update () {
		
	}
}
