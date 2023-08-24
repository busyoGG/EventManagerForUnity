using System;
using System.Collections;

namespace Game
{
    public class EventData
    {
        private Action<ArrayList> _event = null;

        private string _eventName = string.Empty;

        public EventData(string eventName, Action<ArrayList> eventCallback) {
            _eventName = eventName;
            _event = eventCallback;
        }

        public void SetEventCallBack(Action<ArrayList> eventCallback)
        {
            _event = eventCallback;
        }

        public void RemoveEvent()
        {
            _event = null;
        }

        public void Triggered(ArrayList obj)
        {
            if (_event != null)
            {
                _event.Invoke(obj);
            }
        }
    }
}