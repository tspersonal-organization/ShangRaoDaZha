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

public class RoundAnimControl : MonoBehaviour {

    public GameObject[] m_MoveChips;

    int index = 0;

    public static RoundAnimControl instance = null;
    // Use this for initialization
    void Start()
    {
        instance = this;
        //SetPath(new Vector3(0,-200,0),new Vector3(0,400,0));
    }

    public void SetPath(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(Mover(startPos, endPos));
    }

    IEnumerator Mover(Vector3 startPos, Vector3 endPos)
    {
        //if (index > 7) index = 0;
        //GameObject obj = m_MoveChips[index];
        //obj.SetActive(true);
        ////  obj.transform.localPosition = startPos;
        //index++;
        //for (int i = 0; i < 10; i++)
        //{
        //    Transform t = obj.transform.FindChild("Sprite" + i.ToString());
        //    t.gameObject.SetActive(false);
        //}
        //    for (int i = 0; i < 10; i++)
        //  {
        //    Transform t = obj.transform.FindChild("Sprite" + i.ToString());
        //    t.gameObject.SetActive(true);
        //    TweenPosition tp = t.GetComponent<TweenPosition>();
        //    if (tp == null)
        //    {
        //        tp = t.gameObject.AddComponent<TweenPosition>();
        //    }
        //    tp.ResetToBeginning();
        //    t.transform.localPosition = startPos;
        //    tp.from = startPos;
        //    tp.to = endPos;
        //     tp.duration = 0.15f + 0.05f * i;
        //    // tp.duration = 0.5f + 0.1f * i;

        //  //  tp.duration = 1.5f;
        //    tp.onFinished.Clear();
        //    tp.onFinished.Add(new EventDelegate(()=>
        //    {
        //        tp.gameObject.SetActive(false);
        //    }));
        //   // yield return new WaitForSeconds(0.25f);
        //    yield return new WaitForSeconds(0.05f);
        //    tp.PlayForward();
        //}

        //// yield return new WaitForSeconds(0.45f);

        //yield return new WaitForSeconds(2f);
        //obj.SetActive(false);


        if (index > 7) index = 0;
        GameObject obj = m_MoveChips[index];
        obj.SetActive(true);
        for (int i = 1; i < 7; i++)
        {
            Transform t = obj.transform.Find("Sprite" + i.ToString());
            t.gameObject.SetActive(false);
            TweenPosition tp = t.GetComponent<TweenPosition>();
            if (tp == null)
            {
                tp = t.gameObject.AddComponent<TweenPosition>();
            }
            tp.ResetToBeginning();
            t.transform.localPosition = startPos;
            t.gameObject.SetActive(true);
            tp.from = startPos;
            tp.to = endPos;
            tp.duration = 0.15f + 0.05f * i;
            tp.PlayForward();
        }
        index++;
        yield return new WaitForSeconds(0.45f);
        obj.SetActive(false);

    }


    public void SetLocalPath(Vector3 startPos, Vector3 endPos,Transform parent)
    {
        StartCoroutine(LocalMover(startPos, endPos,parent));
    }

    IEnumerator LocalMover(Vector3 startPos, Vector3 endPos,Transform parent)
    {
        if (index > 7) index = 0;
        GameObject obj = m_MoveChips[index];
        obj.SetActive(true);
        //  obj.transform.localPosition = startPos;
        index++;
        for (int i = 0; i < 10; i++)
        {
            Transform t = obj.transform.Find("Sprite" + i.ToString());
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < 10; i++)
        {
            Transform t = obj.transform.Find("Sprite" + i.ToString());
            Transform parentFor = t.parent;
            t.SetParent(parent);
            t.gameObject.SetActive(true);
            TweenPosition tp = t.GetComponent<TweenPosition>();
            if (tp == null)
            {
                tp = t.gameObject.AddComponent<TweenPosition>();
            }
            tp.ResetToBeginning();
            t.transform.localPosition = startPos;
            tp.from = startPos;
            tp.to = endPos;
            tp.duration = 0.15f + 0.05f * i;
            // tp.duration = 0.5f + 0.1f * i;

            //  tp.duration = 1.5f;
            tp.onFinished.Clear();
            tp.onFinished.Add(new EventDelegate(() =>
            {
                t.SetParent(parentFor);
                tp.gameObject.SetActive(false);
            }));
            // yield return new WaitForSeconds(0.25f);
            yield return new WaitForSeconds(0.05f);
            tp.PlayForward();
        }

        // yield return new WaitForSeconds(0.45f);

        yield return new WaitForSeconds(2f);
        obj.SetActive(false);

    }
}
