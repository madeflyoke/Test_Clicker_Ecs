namespace Core.Common.Components.Events
{
    public struct NotifyValueChangedComponent<T> where T :struct
    {
        public T ValueSource;
    }
}
