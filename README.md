# EventManagerForUnity
事件管理器

## 使用方法

### 监听
```c#
EventManager.AddListening("caller_1","event_name",delegate (ArrayList data){
    Debug.Log("触发事件" + data[0]);
});
```
### 通知
```c#
ArrayList data = new ArrayList();
data.Add("一条通知");
EventManager.TriggerEvent("event_name",data);
```

### 粘性通知
```c#
ArrayList data = new ArrayList();
data.Add("一条通知");
EventManager.TriggerEventSticky("event_name_sticky",data);

ArrayList data1 = new ArrayList();
data.Add("一条通知");
EventManager.TriggerEventSticky("event_name_sticky",data1,true);
ArrayList data2 = new ArrayList();
data.Add("两条通知");
EventManager.TriggerEventSticky("event_name_sticky",data2,true);
```

### 清除
```c#
//移除一个监听
EventManager.RemoveListening("caller_1","event_name");
//移除所有监听
EventManager.RemoveAll("caller_1");
```

详细说明见 [Unity自定义事件系统](https://busyo.buzz/article/2474251272a9/)