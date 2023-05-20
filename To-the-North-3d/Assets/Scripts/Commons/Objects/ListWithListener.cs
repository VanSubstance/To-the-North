using System;
using System.Collections.Generic;

namespace Assets.Scripts.Commons.Objects
{
    public class ListWithListener<T> : List<T>
    {
        public Action<T> onValueAddedListener, onValueRemovedListener;
        public new void Add(T item)
        {
            base.Add(item);
            onValueAddedListener?.Invoke(item);
        }
        public new void Remove(T item)
        {
            base.Remove(item);
            onValueRemovedListener?.Invoke(item);
        }
    }
}
