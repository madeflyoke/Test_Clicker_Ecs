using Core.Common.Interfaces;

namespace Core.Common.Components.Events
{
    public struct ValueChangedListenerComponent<TComponent, TData> where TComponent : struct where TData : struct
    {
        public IValueChangedListener<TData> Listener;
    }
}
