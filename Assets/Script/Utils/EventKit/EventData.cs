using System;

namespace EventToolKit
{
    public struct EventData
    {
        public string id;
        public string key;
        public int index;
        public Delegate action;
    }
}