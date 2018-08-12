
using UnityEngine;
using System.Collections;

/// <summary>
/// 不带数据参数的事件监听
/// </summary>
public delegate void EventHandlerFunction();
/// <summary>
/// 带数据参数的事件监听
/// </summary>
public delegate void DataEventHandlerFunction(object date);

/// <summary>
/// 事件监听器提供的接口,包括，添加，删除，派发
/// </summary>
public interface IDispatcher
{
 
    void addEventListener(int  eventID, EventHandlerFunction handler);
    void addEventListener(int  eventID, DataEventHandlerFunction handler);
    void removeEventListener(int  eventID, EventHandlerFunction handler);
    void removeEventListener(int  eventID, DataEventHandlerFunction handler);
    void dispatchEvent(int  eventID);
    void dispatchEvent(int  eventID, object data);
    void clearEvents();
    void clearEvent(int  eventID);
}
