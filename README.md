# EventManagerForUnity

事件工具

利用元组来实现指定参数的监听事件，方便传参，避免事件通知时传参产生的拆装箱。

## 使用方法

### 监听

监听方法会返回一个 `EventData` 对象，该对象可用于取消监听。

```c#
EventKit.AddEvent(id, EEvent.A, EventA);

EventKit.AddEvent<string>(id, EEvent.B, EventB);

EventKit.AddEvent<(string, int)>(id, EEvent.C, EventC);

//----- 事件函数 -----

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
```
### 通知
```c#
EventKit.SendEvent(EEvent.A);

EventKit.SendEvent(EEvent.B, "事件B数据");

EventKit.SendEvent(EEvent.C, ("事件C数据", 3));
```

### 粘性通知
```c#
EventKit.SendEvent(EEvent.D,Sticky.Sticky);

EventKit.AddEvent(id, EEvent.D, EventD);

//----- 事件函数 -----

public void EventD()
{
    Debug.Log("触发粘性通知");
}
```

### 清除
```c#
//移除一个监听
EventKit.RemoveEvent(eventA);
//移除所有监听
EventKit.RemoveAllEvents(id);
```

详细说明见 [Unity自定义事件系统](https://busyo.buzz/article/2474251272a9/)