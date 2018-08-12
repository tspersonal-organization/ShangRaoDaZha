using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatControl : UIBase<ChatControl>
{

    public UISprite ChatKuang;
    public UILabel ChatContent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 设置聊天内容
    /// </summary>
    /// <param name="content"></param>
    public void SetValue(string content)
    {
        this.gameObject.SetActive(true);
        ChatContent.text = content;
        ChatKuang.width = ChatContent.width + 30;
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }
}
