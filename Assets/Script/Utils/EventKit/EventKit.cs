using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace EventToolKit
{
    public enum Sticky
    {
        /// <summary>
        /// 非粘性通知
        /// </summary>
        None,
        /// <summary>
        /// 粘性通知
        /// </summary>
        Sticky,
        /// <summary>
        /// 粘性通知队列
        /// </summary>
        StickyArray
    }

    public class EventKit
    {
        /// <summary>
        /// 节点导航字典，存储该节点的所有事件
        /// </summary>
        private static Dictionary<string, HashSet<(string key, int index)>> _nodeNav = new();

        /// <summary>
        /// 事件字典
        /// </summary>
        private static Dictionary<string, Dictionary<int, EventData>> _events = new();

        /// <summary>
        /// 粘性通知字典
        /// </summary>
        private static Dictionary<string, List<Action>> _stickyNotify = new();

        /// <summary>
        /// 事件索引
        /// </summary>
        private static int _eventIndex = 0;

        /// <summary>
        /// 添加无参事件监听
        /// </summary>
        /// <param name="id">节点id</param>
        /// <param name="key">事件名</param>
        /// <param name="evt">事件函数</param>
        /// <returns></returns>
        public static EventData AddEvent(string id, string key, Action evt)
        {
            int index = _eventIndex++;

            if (!_events.ContainsKey(key))
            {
                _events.Add(key, new Dictionary<int, EventData>());
            }

            EventData eventData = new EventData()
            {
                id = id,
                index = index,
                key = key,
                action = evt
            };

            _events[key][index] = eventData;

            if (!_nodeNav.ContainsKey(id))
            {
                _nodeNav.Add(id, new HashSet<(string e, int index)>());
            }

            _nodeNav[id].Add((key, index));

            _stickyNotify.TryGetValue(key, out var stickies);

            if (stickies != null)
            {
                foreach (var sticky in stickies)
                {
                    sticky.Invoke();
                }

                _stickyNotify.Remove(key);
            }

            return eventData;
        }

        /// <summary>
        /// 添加有参事件监听
        /// </summary>
        /// <param name="id">节点id</param>
        /// <param name="key">事件名</param>
        /// <param name="evt">事件函数</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static EventData AddEvent<T>(string id, string key, Action<T> evt)
        {
            int index = _eventIndex++;

            if (!_events.ContainsKey(key))
            {
                _events.Add(key, new Dictionary<int, EventData>());
            }

            EventData eventData = new EventData()
            {
                id = id,
                index = index,
                key = key,
                action = evt
            };

            _events[key][index] = eventData;

            if (!_nodeNav.ContainsKey(id))
            {
                _nodeNav.Add(id, new HashSet<(string e, int index)>());
            }

            _nodeNav[id].Add((key, index));

            _stickyNotify.TryGetValue(key, out var stickies);

            if (stickies != null)
            {
                foreach (var sticky in stickies)
                {
                    sticky.Invoke();
                }

                _stickyNotify.Remove(key);
            }

            return eventData;
        }

        /// <summary>
        /// 发送无参事件
        /// </summary>
        /// <param name="key">事件名</param>
        /// <param name="sticky">粘性通知类型</param>
        public static void SendEvent(string key, Sticky sticky = Sticky.None)
        {
            _events.TryGetValue(key, out var arr);

            if (arr != null)
            {
                foreach (var action in arr)
                {
                    if (action.Value.action is Action valueAction)
                    {
                        valueAction.Invoke();
                    }
                    else
                    {
#if DEBUG
                        Debug.LogError("事件为有参函数");
#endif
                    }
                }
            }
            else
            {
                if (sticky != Sticky.None)
                {
                    if (!_stickyNotify.ContainsKey(key))
                    {
                        _stickyNotify.Add(key, new());
                    }

                    Action notify = () => { SendEvent(key); };

                    if (sticky == Sticky.StickyArray)
                    {
                        _stickyNotify[key].Add(notify);
                    }
                    else
                    {
                        if (_stickyNotify[key].Count > 0)
                        {
                            _stickyNotify[key][0] = notify;
                        }
                        else
                        {
                            _stickyNotify[key].Add(notify);
                        }
                    }
                }
                else
                {
#if DEBUG
                    Debug.LogWarning("事件" + key + "未注册");
#endif
                }
            }
        }

        /// <summary>
        /// 发送有参事件
        /// </summary>
        /// <param name="key">事件名</param>
        /// <param name="data">参数</param>
        /// <param name="sticky">粘性通知类型</param>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="T"></typeparam>
        public static void SendEvent<T>(string key, T data, Sticky sticky = Sticky.None)
        {
            _events.TryGetValue(key, out var arr);

            if (arr != null)
            {
                foreach (var action in arr)
                {
                    if (action.Value.action is Action<T> valueAction)
                    {
                        valueAction.Invoke(data);
                    }
                    else
                    {
#if DEBUG
                        Debug.LogError("事件为无参函数或参数错误");
#endif
                    }
                }
            }
            else
            {
                if (sticky != Sticky.None)
                {
                    if (!_stickyNotify.ContainsKey(key))
                    {
                        _stickyNotify.Add(key, new());
                    }

                    Action notify = () => { SendEvent(key, data); };

                    if (sticky == Sticky.StickyArray)
                    {
                        _stickyNotify[key].Add(notify);
                    }
                    else
                    {
                        if (_stickyNotify[key].Count > 0)
                        {
                            _stickyNotify[key][0] = notify;
                        }
                        else
                        {
                            _stickyNotify[key].Add(notify);
                        }
                    }
                }
                else
                {
#if DEBUG
                    Debug.LogWarning("事件" + key + "未注册");
#endif
                }
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="data"></param>
        public static void RemoveEvent(EventData data)
        {
            _nodeNav.TryGetValue(data.id, out var datas);
            datas?.Remove((data.key, data.index));

            _events.TryGetValue(data.key, out var arr);
            arr?.Remove(data.index);
        }

        /// <summary>
        /// 移除节点上的所有事件
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveAllEvents(string id)
        {
            _nodeNav.TryGetValue(id, out var data);

            if (data != null)
            {
                foreach (var res in data)
                {
                    _events.TryGetValue(res.key, out var arr);

                    if (arr != null)
                    {
                        arr.Remove(res.index);
                    }
                }

                _nodeNav.Remove(id);
            }
        }
    }
}