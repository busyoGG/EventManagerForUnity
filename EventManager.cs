using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EventManager
    {
        /// <summary>
        /// 事件字典
        /// </summary>
        private static Dictionary<string, Dictionary<string, EventData>> _eventDictionary = new Dictionary<string, Dictionary<string, EventData>>();

        /// <summary>
        /// 节点字典
        /// </summary>
        private static Dictionary<string, Dictionary<string, bool>> _eventNode = new Dictionary<string, Dictionary<string, bool>>();

        /// <summary>
        /// 粘性通知字典
        /// </summary>
        private static Dictionary<string, ArrayList> _stickyDic = new Dictionary<string, ArrayList>();

        /// <summary>
        /// 存储当前事件的粘性通知是否为队列通知
        /// </summary>
        private static Dictionary<string, bool> _stickyArrayFlag = new Dictionary<string, bool>();

        /// <summary>
        /// 在需要监听某个事件的脚本中，调用这个方法来监听这个事件
        /// </summary>
        /// <param name = "id">当前节点id</param>
        /// <param name="eventName">事件名</param>
        /// <param name"Action">注册监听的函数</param>
        public static void addListening(string id, string eventName, Action<ArrayList> action)
        {

            EventData eventData = null;
            if (!_eventDictionary.ContainsKey(eventName))
            {
                _eventDictionary.Add(eventName, new Dictionary<string, EventData>());
            }

            Dictionary<string, EventData> eventDic = _eventDictionary[eventName];

            eventDic.TryGetValue(id, out eventData);
            if (eventData != null)
            {
                eventData.SetEventCallBack(action);
            }
            else
            {
                eventData = new EventData(eventName, action);
                eventDic.Add(id, eventData);
            }

            if (_eventNode.ContainsKey(id))
            {
                // _eventNode[id][eventName] = true;
            }
            else
            {
                Dictionary<string, bool> dicNode = new Dictionary<string, bool>
                {
                    { id, true }
                };
                _eventNode.Add(id, dicNode);
            }

            //触发粘性通知
            ArrayList stickyArray;
            _stickyDic.TryGetValue(eventName, out stickyArray);
            if (stickyArray != null)
            {
                //有粘性通知的情况下一定能获取到是否为通知队列
                bool isArray = _stickyArrayFlag[eventName];
                if (isArray)
                {
                    for (int i = 0, len = stickyArray.Count; i < len; i++)
                    {
                        TriggerEvent(eventName, stickyArray[i] as ArrayList);
                    }
                }
                else
                {
                    TriggerEvent(eventName, stickyArray);
                }
                //完成通知，移除数据
                _stickyArrayFlag.Remove(eventName);
                _stickyDic.Remove(eventName);
            }
        }

        /// <summary>
        /// 在不需要监听的时候停止监听
        /// </summary>
        /// <param name = "id">当前节点id</param>
        /// <param name="eventName">事件名</param>
        /// <param name"Action">注册监听的函数</param>
        public static void removeListening(string id, string eventName, Action<ArrayList> action)
        {
            EventData eventData = null;

            Dictionary<string, EventData> eventDic = null;

            _eventDictionary.TryGetValue(eventName, out eventDic);

            if (eventDic != null)
            {
                eventDic.TryGetValue(id, out eventData);

                if (eventData != null)
                {
                    eventData.RemoveEvent();
                    //移除事件字典中的事件对应的作用域
                    eventDic.Remove(id);
                    //如果事件字典中事件的作用域为0 则移除该事件
                    if (eventDic.Count <= 0)
                    {
                        _eventDictionary.Remove(eventName);
                    }
                    //如果有事件存在，则一定在对应的node字典中存在
                    _eventNode[id].Remove(eventName);

                    Debug.Log("移除事件" + id + eventName);
                }
            }
        }

        /// <summary>
        /// 移除节点上的所有事件
        /// </summary>
        /// <param name="id">节点</param>
        public static void RemoveAll(string id)
        {
            if (_eventNode.ContainsKey(id))
            {
                foreach (var data in _eventNode[id])
                {
                    Dictionary<string, EventData> eventDic = null;

                    _eventDictionary.TryGetValue(data.Key, out eventDic);

                    if (eventDic != null)
                    {
                        EventData eventData = null;
                        eventDic.TryGetValue(id, out eventData);
                        if (eventData != null)
                        {
                            //移除事件
                            eventData.RemoveEvent();
                            //移除事件字典中的事件对应的作用域
                            eventDic.Remove(id);
                            //如果事件字典中事件的作用域为0 则移除该事件
                            if (eventDic.Count <= 0)
                            {
                                _eventDictionary.Remove(data.Key);
                            }
                        }
                    }
                }
                //移除节点字典中的对应节点数据
                _eventNode.Remove(id);
            }
        }

        /// <summary>
        /// 触发某个事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="obj">参数列表，可以为空，但是记得在回调函数里面对该参数进行判空处理</param>
        public static void TriggerEvent(string eventName, ArrayList obj)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {

                Dictionary<string, EventData> eventDic = null;

                _eventDictionary.TryGetValue(eventName, out eventDic);

                if (eventDic != null)
                {
                    EventData triggerEvent = null;
                    foreach (var data in eventDic)
                    {
                        triggerEvent = data.Value;
                        triggerEvent.Triggered(obj);
                    }
                }
            }
        }

        /// <summary>
        /// 粘性通知
        /// </summary>
        /// <param name="eventName">通知名</param>
        /// <param name="obj">数据</param>
        /// <param name="isArray">是否队列</param>
        public static void TriggerEventSticky(string eventName, ArrayList obj, bool isArray = false)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {

                Dictionary<string, EventData> eventDic = null;

                _eventDictionary.TryGetValue(eventName, out eventDic);

                if (eventDic != null)
                {
                    EventData triggerEvent = null;
                    foreach (var data in eventDic)
                    {
                        triggerEvent = data.Value;
                        triggerEvent.Triggered(obj);
                    }
                }
            }
            else
            {
                //保存通知内容
                bool arrayFlag;
                _stickyArrayFlag.TryGetValue(eventName, out arrayFlag);

                if (isArray)
                {
                    arrayFlag = true;
                    ArrayList res;
                    _stickyDic.TryGetValue(eventName, out res);
                    if (res == null)
                    {
                        res = new ArrayList();
                        _stickyDic.Add(eventName, res);
                        _stickyArrayFlag.Add(eventName, arrayFlag);
                    }
                    else
                    {
                        _stickyDic[eventName] = res;
                        _stickyArrayFlag[eventName] = arrayFlag;
                    }
                    res.Add(obj);

                }
                else
                {
                    arrayFlag = false;
                    _stickyDic.Add(eventName, obj);
                    if (arrayFlag)
                    {
                        _stickyArrayFlag[eventName] = arrayFlag;
                    }
                    else
                    {
                        _stickyArrayFlag.Add(eventName, arrayFlag);
                    }
                }
            }
        }
    }
}