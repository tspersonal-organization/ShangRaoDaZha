using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInfo 
{
    public string statusCode;
    public bool has_server;
    public bool login_with_device;
    public string version;
    public string update_message;
    public string update_android_url;
    public string update_ios_url;
    public string ip;
    public ushort port;

    public static ServerInfo Data = new ServerInfo();

}
