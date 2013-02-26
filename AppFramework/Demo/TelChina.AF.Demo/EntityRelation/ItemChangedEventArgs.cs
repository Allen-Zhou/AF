using System;

namespace TelChina.AF.Demo
{
    public class ItemChangedEventArgs<T> : EventArgs
    {
        public ItemChangedEventArgs(ItemChangedType type, T item)
        {
            this.Type = type;
            this.Item = item;
        }

        public ItemChangedType Type { get; private set; }

        public T Item { get; private set; }
    }
}
