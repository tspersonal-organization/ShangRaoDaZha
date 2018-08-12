using System;
using System.Collections;
using System.Collections.Generic;

public class CommentController
{
    private Dictionary<int,string> commentTexts = new Dictionary<int,string>();
    private Dictionary<int, string> commentSound = new Dictionary<int, string>();

    public CommentController()
    {
        commentTexts.Add(101, "再难过也不能改变事实");
        commentTexts.Add(102, "娘啊赏个好七十分不到");
        commentTexts.Add(103, "怪不得你只能怪你厂家");
        commentTexts.Add(104, "除了颜值有很多优点吧");

        commentTexts.Add(701, "七分像人三分像鬼");
        commentTexts.Add(702, "下次不要再说好帅了");
        commentTexts.Add(703, "就是这个分莫心里打鼓");
        commentTexts.Add(704, "赶紧弄点口水打扮下头发");

        commentTexts.Add(751, "做好心里准备接受了吗");
        commentTexts.Add(752, "整天不打扮都能这么高分");
        commentTexts.Add(753, "系统都不相信你这个分");
        commentTexts.Add(754, "高兴太早测了吓一跳");

        commentTexts.Add(801, "再化个妆你要成仙");
        commentTexts.Add(802, "怎么长的，长的这么春楼");
        commentTexts.Add(803, "走路带风回头率百分百");
        commentTexts.Add(804, "随便一亮惊艳一片");

        commentTexts.Add(851, "你很不安全请个保镖");
        commentTexts.Add(852, "你的颜值爆表无法形容");
        commentTexts.Add(853, "这个颜值找对象免费");
        commentTexts.Add(854, "这么漂亮请你签个名");
    }

    public string GetCommentText( int aPoints ){
        string tmpStr = "今天气色还不错啊";

        try{
            tmpStr = commentTexts[getExactlyNum(aPoints)];
        }
        catch(Exception e){
            Console.WriteLine(e.ToString());
        }
        return tmpStr;
    }

    public void PlayCommentSound( int aPoints ){
        int arragePoints = (aPoints / 10) * 10 + aPoints % 5;

        SoundManager.Instance.PlaySound("SoundDetectFaceCommnet/"+getExactlyNum(aPoints));
    }

    int getExactlyNum( int aPoints ){
        
        if( aPoints < 70 ){
            aPoints = 10;
        }else if( aPoints >= 70 && aPoints < 85){
            
        }else{
            aPoints = 85;
        }

        int arragePoints = (aPoints / 5) * 5;

        return arragePoints*10+new Random().Next(1,4) ;
    }
}
