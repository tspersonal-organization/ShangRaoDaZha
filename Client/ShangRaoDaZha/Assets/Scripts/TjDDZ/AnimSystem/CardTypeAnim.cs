using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTypeAnim : UIBase<CardTypeAnim>
{

    public UISprite BoomSprite;
    public UISprite RocketSprite;
    public UISprite ShunZhiSprite;
    public UISprite LianDuiSprite;

    public UISprite BoomAnimSprite;
    public UISprite RocketAnimSprite;

    public GameObject CenterPoint;
    public GameObject EndPoint;
    // Use this for initialization
    void Start () {
		
	}

    float count = 0;
    float timer = 0.1f;
    int index = 0;
    float count1 = 0;
   
    int index1 = 0;
    // Update is called once per frame
    void Update () {
        if (RocketAnimSprite.gameObject.activeSelf)
        {
            count += Time.deltaTime;
            if (count > timer)
            {
                count = 0;
                index++;
                if (index > 7)
                {
                    index = 0;
                    RocketAnimSprite.gameObject.SetActive(false);
                }
                else
                {
                    RocketAnimSprite.spriteName = "comic_game_rocket_" + index.ToString();
                    RocketAnimSprite.MakePixelPerfect();
                }
              
            }
        }

        if (BoomAnimSprite.gameObject.activeSelf)
        {
            count1 += Time.deltaTime;
            if (count1 > timer)
            {
                count1 = 0;
                index1++;
                if (index1 > 8)
                {
                    index1 = 0;
                    BoomAnimSprite.gameObject.SetActive(false);
                }
                else
                {
                    BoomAnimSprite.spriteName = "comic_game_Boom_" + index1.ToString();
                    BoomAnimSprite.MakePixelPerfect();
                }

            }
        }
    }

   

    /// <summary>
    /// 播放火箭动画
    /// </summary>
    public void PlayRockAnim()
    {

        // RocketSprite.transform.localPosition = Vector3.zero;
        RocketSprite.transform.localScale = new Vector3(4,4,0);
        RocketSprite.gameObject.SetActive(true);
      // RocketSprite.transform.GetComponent<TweenPosition>().PlayForward();

      Hashtable args = new Hashtable();

        // Vector3 wordpos = new Vector3(To.x/320f,To.y/320f,0);

       // Vector3 wordpos = ToWorldPos(CenterPoint.transform.position);
        //移动的速度，  
        args.Add("speed", 5f);
       // args.Add("easeType", iTween.EaseType.easeOutBack);
        //移动的整体时间。如果与speed共存那么优先speed  
        args.Add("time", 1f);
      //  args.Add("position", wordpos);

        args.Add("scale", new Vector3(1, 1, 1));
        iTween.ScaleTo(RocketSprite.gameObject,args);
       // iTween.MoveTo(RocketSprite.gameObject, args);
        StartCoroutine(WaitPlayRock());
        //// AnimSystem.Instance.MoveTo(RocketSprite.gameObject,new Vector3(738f/320, RocketSprite.transform.position.y, RocketSprite.transform.position.z));
    }


    IEnumerator WaitPlayRock()
    {
        yield return new  WaitForSeconds(1f);
        PlayRockAnimEnd();
    }
    public void PlayRockAnimEnd()
    {
        RocketSprite.gameObject.SetActive(false);
       
        RocketAnimSprite.gameObject.SetActive(true);
    }
    /// <summary>
    /// 播放炸弹动画
    /// </summary>
    public void PlayBoomAnim()
    {
        BoomSprite.transform.localScale = new Vector3(4, 4, 0);
        BoomSprite.gameObject.SetActive(true);

        Hashtable args = new Hashtable();

     
        args.Add("speed", 5f);
        // args.Add("easeType", iTween.EaseType.easeOutBack);
        //移动的整体时间。如果与speed共存那么优先speed  
        args.Add("time", 1f);
        //  args.Add("position", wordpos);

        args.Add("scale", new Vector3(1, 1, 1));
        iTween.ScaleTo(BoomSprite.gameObject, args);

     
        StartCoroutine(WaitPlayBoom());
    }

    IEnumerator WaitPlayBoom()
    {
        yield return new WaitForSeconds(1f);
        PlayBoomAnimEnd();
    }
    public void PlayBoomAnimEnd()
    {
        BoomSprite.gameObject.SetActive(false);
        BoomAnimSprite.gameObject.SetActive(true);
    }
    /// <summary>
    /// 播放顺子动画
    /// </summary>
    public void PlayShunZhiAnim()
    {
        ShunZhiSprite.transform.localPosition = Vector3.zero;
        // RocketSprite.transform.GetComponent<TweenPosition>().PlayForward();

        Hashtable args = new Hashtable();

        // Vector3 wordpos = new Vector3(To.x/320f,To.y/320f,0);

        Vector3 wordpos = ToWorldPos(CenterPoint.transform.position);
        //移动的速度，  
        args.Add("speed", 5f);
        args.Add("easeType", iTween.EaseType.easeOutBack);
        //移动的整体时间。如果与speed共存那么优先speed  
        args.Add("time", 1f);
        args.Add("position", wordpos);

        iTween.MoveTo(ShunZhiSprite.gameObject, args);
        StartCoroutine(WaitPlayShunZhi());
    }

    IEnumerator WaitPlayShunZhi()
    {
        yield return new WaitForSeconds(1f);
        PlayShunZhiAnimEnd();
    }
    public void PlayShunZhiAnimEnd()
    {
        Hashtable args = new Hashtable();

        // Vector3 wordpos = new Vector3(To.x/320f,To.y/320f,0);

        Vector3 wordpos = ToWorldPos(EndPoint.transform.position);
        //移动的速度，  
        args.Add("speed", 5f);
        args.Add("easeType", iTween.EaseType.easeInBack);
        //移动的整体时间。如果与speed共存那么优先speed  
        args.Add("time", 1f);
        args.Add("position", wordpos);

        iTween.MoveTo(ShunZhiSprite.gameObject, args);
      //  BoomAnimSprite.gameObject.SetActive(true);
    }
    /// <summary>
    /// 播放炸弹动画
    /// </summary>
    public void PlayLianDuiAnim()
    {
        LianDuiSprite.transform.localPosition = Vector3.zero;
        // RocketSprite.transform.GetComponent<TweenPosition>().PlayForward();

        Hashtable args = new Hashtable();

        // Vector3 wordpos = new Vector3(To.x/320f,To.y/320f,0);

        Vector3 wordpos = ToWorldPos(CenterPoint.transform.position);
        //移动的速度，  
        args.Add("speed", 5f);
        args.Add("easeType", iTween.EaseType.easeOutBack);
        //移动的整体时间。如果与speed共存那么优先speed  
        args.Add("time", 1f);
        args.Add("position", wordpos);

        iTween.MoveTo(LianDuiSprite.gameObject, args);
        StartCoroutine(WaitPlayLianDui());
    }

    IEnumerator WaitPlayLianDui()
    {
        yield return new WaitForSeconds(1f);
        PlayLianDuiAnimEnd();
    }
    public void PlayLianDuiAnimEnd()
    {
        Hashtable args = new Hashtable();

        // Vector3 wordpos = new Vector3(To.x/320f,To.y/320f,0);

        Vector3 wordpos = ToWorldPos(EndPoint.transform.position);
        //移动的速度，  
        args.Add("speed", 5f);
        args.Add("easeType", iTween.EaseType.easeInBack);
        //移动的整体时间。如果与speed共存那么优先speed  
        args.Add("time", 1f);
        args.Add("position", wordpos);

        iTween.MoveTo(LianDuiSprite.gameObject, args);
       
    }
    public Vector3 ToWorldPos(Vector3 vec)
    {

        //  Debug.Log(" ngui:"+vec);
        Camera guiCamera = NGUITools.FindCameraForLayer(this.gameObject.layer);   //通过脚本所在物体的层获得相应层上的相机
        Vector3 pos = guiCamera.WorldToScreenPoint(vec);         //获取UI界面的屏幕坐标
                                                                 //  Debug.Log(" 屏幕:" + pos );                                                    //  pos.z = 1f;//设置为零时转换后的pos全为0,屏幕空间的原因，被坑过的我提醒大家，切记要改！
        pos = guiCamera.ScreenToWorldPoint(pos);
        //将屏幕坐标转换为世界坐标
        //  Debug.Log(" 世界:" + pos);
        return pos;
    }
}
