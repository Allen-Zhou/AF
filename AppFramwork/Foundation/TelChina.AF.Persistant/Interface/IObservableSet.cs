using System;
using Iesi.Collections.Generic;

namespace TelChina.AF.Persistant
{
    public interface IObservableSet<T> : ISet<T>
    {
        event EventHandler<ItemChangedEventArgs<T>> ItemChanged;
    }
}
