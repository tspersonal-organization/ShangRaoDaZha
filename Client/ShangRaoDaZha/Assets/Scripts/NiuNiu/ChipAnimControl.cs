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

public class ChipAnimControl : MonoBehaviour {


    public GameObject StartPoint;
    public GameObject EndObj;

    public void PlayeXianJiaMaiMa(GameObject start,GameObject end)
    {
        StartCoroutine(OtherMover(start, end));
    }

    IEnumerator OtherMover(GameObject start, GameObject end)
    {

        //  obj.transform.localPosition = startPos;

        for (int i = 0; i < 10; i++)
        {
            Transform t = gameObject.transform.Find("Sprite" + i.ToString());
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < 10; i++)
        {
            Transform t = gameObject.transform.Find("Sprite" + i.ToString());
            t.gameObject.SetActive(true);
            TweenPosition tp = t.GetComponent<TweenPosition>();
            if (tp == null)
            {
                tp = t.gameObject.AddComponent<TweenPosition>();
            }
            tp.ResetToBeginning();
            t.transform.localPosition = start.transform.localPosition;
            tp.from = start.transform.localPosition;
            tp.to = end.transform.position;
            tp.duration = 0.15f + 0.05f * i;
            // tp.duration = 0.5f + 0.1f * i;

            //  tp.duration = 1.5f;
            tp.onFinished.Clear();
            tp.onFinished.Add(new EventDelegate(() =>
            {
                tp.gameObject.SetActive(false);
            }));
            // yield return new WaitForSeconds(0.25f);
            yield return new WaitForSeconds(0.05f);
            tp.PlayForward();
        }

        // yield return new WaitForSeconds(0.45f);
        end.SetActive(true);
        yield return new WaitForSeconds(1f);


    }

    public void PlayAnim()
    {
       
        gameObject.SetActive(true);
        StartCoroutine(Mover(StartPoint.transform.localPosition, EndObj.transform.localPosition));
    }



    IEnumerator Mover(Vector3 startPos, Vector3 endPos)
    {
       
        //  obj.transform.localPosition = startPos;
     
        for (int i = 0; i < 10; i++)
        {
            Transform t = gameObject.transform.Find("Sprite" + i.ToString());
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < 10; i++)
        {
            Transform t = gameObject.transform.Find("Sprite" + i.ToString());
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
                tp.gameObject.SetActive(false);
            }));
            // yield return new WaitForSeconds(0.25f);
            yield return new WaitForSeconds(0.05f);
            tp.PlayForward();
        }

        // yield return new WaitForSeconds(0.45f);

        yield return new WaitForSeconds(1f);
      

    }
}
