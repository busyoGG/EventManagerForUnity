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

详细说明见 [Unity自定义事件系统](https://busyo.buzz/article/Unity/%E5%B7%A5%E5%85%B7/Unity%E8%87%AA%E5%AE%9A%E4%B9%89%E4%BA%8B%E4%BB%B6%E7%B3%BB%E7%BB%9F/)