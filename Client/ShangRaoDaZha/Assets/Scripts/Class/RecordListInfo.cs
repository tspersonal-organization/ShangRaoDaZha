using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FrameworkForCSharp.Utils;

public class RecordListInfo
{
    public RoomType roomType;

    public ulong guid;
    public uint id;
    public ulong fangZhuGuid;
    public string fangZhuName;

    public ulong ZhuangGuid;

    public DateTime startTime;
    public DateTime endTime;
    public List<string> playerInfo = new List<string>();//guid|name|socre|headid
}
