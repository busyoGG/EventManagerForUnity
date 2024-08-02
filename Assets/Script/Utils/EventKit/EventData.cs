using System;

namespace EventToolKit
{
    public struct EventData
    {
        public string id;
        public Enum key;
        public int index;
        public Delegate action;
    }
}