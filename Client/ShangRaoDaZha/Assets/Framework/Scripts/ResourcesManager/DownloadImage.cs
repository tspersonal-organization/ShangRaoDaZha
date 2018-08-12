using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadImage : MonoBehaviour
{

    Dictionary<string, Texture2D> dictHeadImg = new Dictionary<string, Texture2D>();

    public static DownloadImage Instance = null;

    void Start()
    {
        Instance = this;
    }

    public void Download(UITexture tex,string imgurl,bool isKongWei = false)
    {
        //if(isKongWei)
        //{
        //    tex.mainTexture = Resources.Load<Texture2D>("SitDownHead");
        //    return;
        //}
        if (imgurl == "headid" || imgurl == null)
        {
            tex.mainTexture = Resources.Load<Texture2D>("Ui/Texture/DefaultHead");
            return;
        }
      
        else if (imgurl != null)
        {
            StartCoroutine(SaveDownloadImage(tex, imgurl));
        }
        Log.Debug(imgurl);

    }

    IEnumerator SaveDownloadImage(UITexture tex,string imgurl)
    {
        if (dictHeadImg.ContainsKey(imgurl))
        {
            tex.mainTexture = dictHeadImg[imgurl];
        }
        else
        {
            WWW www = new WWW(imgurl);
            while (!www.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
            if (www.error == null)
            {
                if (dictHeadImg.ContainsKey(imgurl))
                    dictHeadImg[imgurl] = www.texture;
                else
                    dictHeadImg.Add(imgurl, www.texture);
                tex.mainTexture = www.texture;
            }
        }
        yield break;
    }

    IEnumerator DownloadImg(UITexture tex, string imgurl)
    {
        WWW www = new WWW(imgurl);
        while (!www.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        if (www.error == null)
        {
            tex.mainTexture = www.texture;
        }   
        yield break;
    }
}
