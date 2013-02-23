using System;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    /// <summary>
    /// Author：张前园
    /// Time：2012-08-08
    /// Reason：重写Add方法与Remove方法
    /// </summary>
    public class PersonsSet<T> : HashedSet<T>, IObservableSet<T>
    {
        /// <summary>
        /// 重写add方法
        /// 避免业务代码关注内部细节
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(T o)
        {
            if(base.Add(o))
            {
                var e = new ItemChangedEventArgs<T>(ItemChangedType.Added, o);
                this.OnItemChanged(e);
                return true;
                //if (person is Person)
                //{
                //    var o = person as Person;
                //    o.Name = "816";
                //}                           
                return true;
            }
                
            return false;        
        }

        /// <summary>
        /// 重写remove方法
        /// 避免业务代码关注内部细节
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new bool Remove(T o)
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
