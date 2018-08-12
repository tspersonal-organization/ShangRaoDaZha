using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceListener
{
    //加载完成时调用
    void OnLoaded(string assetName,object asset);
}
