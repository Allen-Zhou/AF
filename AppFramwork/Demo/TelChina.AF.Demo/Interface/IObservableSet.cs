using System;
using Iesi.Collections.Generic;

namespace TelChina.AF.Demo
{
    public interface IObservableSet<T> : ISet<T>
    {
        event EventHandler<ItemChangedEventArgs<T>> ItemChanged;
    }
}
