using System;
using Iesi.Collections.Generic;

namespace TelChina.AF.Demo
{
    public class ObservableSet<T> : HashedSet<T>, IObservableSet<T>
    {
        public override bool Add(T o)
        {
            if (base.Add(o))
            {
                var e = new ItemChangedEventArgs<T>(ItemChangedType.Added, o);
                this.OnItemChanged(e);                
                return true;
            }

            return false;
        }

        public override bool Remove(T o)
        {
            if (base.Remove(o))
            {
                var e = new ItemChangedEventArgs<T>(ItemChangedType.Removed, o);
                this.OnItemChanged(e);
                return true;
            }

            return false;
        }

        public event EventHandler<ItemChangedEventArgs<T>> ItemChanged;

        protected void OnItemChanged(ItemChangedEventArgs<T> e)
        {
            var itemChanged = this.ItemChanged;            
            if (itemChanged != null) 
                itemChanged(this, e);
        }
    }
}
