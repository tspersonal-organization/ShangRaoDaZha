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
        StartCoroutine(DownloadHead(tex, imgurl));
    }

    public IEnumerator DownloadHead(UITexture texHead, string sUrl)
    {
        if (!string.IsNullOrEmpty(sUrl) && sUrl != "0" && sUrl != "headid")
        {
            if (dictHeadImg.ContainsKey(sUrl))
            {
                if (texHead == null)
                {

                }
                else
                {
                    texHead.mainTexture = dictHeadImg[sUrl];
                }
            }
            else
            {
                WWW ww = new WWW(sUrl);
                yield return ww;
                try
                {
                    if (ww.error == null)
                    {
                        ww.texture.Compress(true);
                        if (texHead == null)
                        {

                        }
                        else
                        {
                            texHead.mainTexture = ww.texture;
                        }
                        if (!dictHeadImg.ContainsKey(sUrl))
                            dictHeadImg.Add(sUrl, ww.texture);
                    }
                    else
                    {
                        texHead.mainTexture = Resources.Load<Texture2D>("Ui/Texture/DefaultHead");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("DownloadHead_Exception_" + e.Message);
                    texHead.mainTexture = Resources.Load<Texture2D>("Ui/Texture/DefaultHead");
                }
            }
        }
        else
        {
            texHead.mainTexture = Resources.Load<Texture2D>("Ui/Texture/DefaultHead");
        }
    }
}
