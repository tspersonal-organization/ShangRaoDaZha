
using UnityEngine;
using System;
using System.Collections.Generic;

public class GameEventDispatcher : IDispatcher
{
    

 
    /// <summary>
    /// 不带参数，无返回值的回调函数队列
    /// </summary>
    private Dictionary<int, List<EventHandlerFunction>> listners = new Dictionary<int, List<EventHandlerFunction>>();
    /// <summary>
    /// 带有参数，无返回值的回调函数队列
    /// </summary>
    private Dictionary<int, List<DataEventHandlerFunction>> dataListners = new Dictionary<int, List<DataEventHandlerFunction>>();

    // 临时队列,避免重复new操作
    private static List<EventHandlerFunction> t_EventHandlerList;//主要是colone时用
    private static List<DataEventHandlerFunction> t_DateEventHandlerList;

    /// <summary>
    /// 单例
    /// </summary>
    private static GameEventDispatcher _inst;
    public static GameEventDispatcher Instance
    {
        get
        {
            if (_inst == null)
            {
                _inst = new GameEventDispatcher();
              
                t_EventHandlerList = new List<EventHandlerFunction>();
                t_DateEventHandlerList = new List<DataEventHandlerFunction>();
            }
            return _inst;
        }
    }

   
   /// <summary>
   /// 不带参的添加
   /// </summary>
   /// <param name="eventID"></param>
   /// <param name="handler"></param>
    public void addEventListener(int  eventID, EventHandlerFunction handler)
    {
        int e = (int)eventID;
        if (listners.ContainsKey(e))
        {
            listners[e].Add(handler);
        }
        else
        {
            List<EventHandlerFunction> handlers = new List<EventHandlerFunction>();
            handlers.Add(handler);
            listners.Add(e, handlers);
        }
    }

  /// <summary>
  ///带参的添加
  /// </summary>
  /// <param name="eventID"></param>
  /// <param name="handler"></param>
    public void addEventListener(int  eventID, DataEventHandlerFunction handler)
    {
        int e = (int)eventID;
        if (dataListners.ContainsKey(e))
        {
            dataListners[e].Add(handler);
        }
        else
        {
            List<DataEventHandlerFunction> handlers = new List<DataEventHandlerFunction>();
            handlers.Add(handler);
            dataListners.Add(e, handlers);
        }
    }

  /// <summary>
  /// 删除事件监听
  /// </summary>
  /// <param name="eventID"></param>
  /// <param name="handler"></param>
    public void removeEventListener(int  eventID, EventHandlerFunction handler)
    {
        int e = (int)eventID;
        if (listners.ContainsKey(e))
        {
            List<EventHandlerFunction> handlers = listners[e];
            handlers.Remove(handler);
            handler = null;

            if (handlers.Count == 0)
            {
                listners.Remove(e);
            }
        }
    }
  
    public void removeEventListener(int  eventID, DataEventHandlerFunction handler)
    {
        int e = (int)eventID;
        if (dataListners.ContainsKey(e))
        {
            List<DataEventHandlerFunction> handlers = dataListners[e];
            handlers.Remove(handler);

            if (handlers.Count == 0)
            {
                dataListners.Remove(e);
            }
        }
    }

    /// <summary>
    /// 删除所有添加的事件监听
    /// </summary>
    public void clearEvents()
    {
        listners.Clear();
        dataListners.Clear();
    }

  /// <summary>
  /// 删除一个
  /// </summary>
  /// <param name="eventID"></param>
    public void clearEvent(int  eventID)
    {
        int e = (int)eventID;
        if (listners.ContainsKey(e))
        {
            List<EventHandlerFunction> handlers = listners[e];
            handlers.Clear();
            listners.Remove(e);
        }
        if (dataListners.ContainsKey(e))
        {
            List<DataEventHandlerFunction> handlers = dataListners[e];
            handlers.Clear();
            dataListners.Remove(e);
        }
    }

  
   /// <summary>
   /// 事件分发  不带参
   /// </summary>
   /// <param name="eventID"></param>
    public void dispatchEvent(int  eventID)
    {
        dispatch((int)eventID, null);
    }
   
   /// <summary>
   /// 事件分发带参
   /// </summary>
   /// <param name="eventID"></param>
   /// <param name="data"></param>
    public void dispatchEvent(int  eventID, object data)
    {
        dispatch((int)eventID, data);
    }

    private void dispatch(int eventID, object data)
    {
      
        lock (dataListners)
        {
           
            if (dataListners.ContainsKey(eventID))
            {
                // 这里之所以需要cloen操作，是为了避免事件调用时，事件队列长度发生变化！
                List<DataEventHandlerFunction> handlers = cloenArray(dataListners[eventID]);
                int len = handlers.Count;
                for (int i = 0; i < len; i++)
                {
                        handlers[i](data);
                  
                }
            }
        }

        lock (listners)
        {
            if (listners.ContainsKey(eventID))
            {
                // 这里之所以需要cloen操作，是为了避免事件调用时，事件队列长度发生变化！
                List<EventHandlerFunction> handlers = cloenArray(listners[eventID]);
                int len = handlers.Count;
                for (int i = 0; i < len; i++)
                {
                        handlers[i]();
                   
                }
            }
        }
    }

    /// <summary>
    /// 这里之所以需要cloen操作，是为了避免事件调用时，事件队列长度发生变化！
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private List<EventHandlerFunction> cloenArray(List<EventHandlerFunction> list)
    {
        t_EventHandlerList.Clear();
        int len = list.Count;
        for (int i = 0; i < len; i++)
        {
            t_EventHandlerList.Add(list[i]);
        }
        return t_EventHandlerList;
    }
  
    private List<DataEventHandlerFunction> cloenArray(List<DataEventHandlerFunction> list)
    {
        t_DateEventHandlerList.Clear();
        int len = list.Count;
        for (int i = 0; i < len; i++)
        {
            t_DateEventHandlerList.Add(list[i]);
        }
        return t_DateEventHandlerList;
    }
}

