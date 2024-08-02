using System.Collections;
using System.Collections.Generic;
using EventToolKit;
using UnityEngine;

public enum EEvent
{
    A,
    B,
    C,
    D
}

public class Launcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string id = "launcher";

        //不同类型通知
        var eventA = EventKit.AddEvent(id, EEvent.A, EventA);

        EventKit.AddEvent<string>(id, EEvent.B, EventB);

        EventKit.AddEvent<(string, int)>(id, EEvent.C, EventC);
        
        //移除所有监听
        //EventKit.RemoveAllEvents(id);
        //移除事件A监听
        // EventKit.RemoveEvent(eventA);

        EventKit.SendEvent(EEvent.A);

        EventKit.SendEvent(EEvent.B, "事件B数据");

        EventKit.SendEvent(EEvent.C, ("事件C数据", 3));

        //粘性通知例
        EventKit.SendEvent(EEvent.D,Sticky.Sticky);

        EventKit.AddEvent(id, EEvent.D, EventD);
    }

    private void EventA()
    {
        Debug.Log("触发事件A");
    }

    private void EventB(string data)
    {
        Debug.Log("触发事件B 传入的数据为 " + data);
    }

    public void EventC((string str, int num) data)
    {
        Debug.Log("触发事件C 传入的数据为 " + data.str + " 和 " + data.num);
    }

    public void EventD()
    {
        Debug.Log("触发粘性通知");
    }
}