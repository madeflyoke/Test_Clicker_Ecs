namespace Core.Common.Interfaces
{
    public interface IValueChangedListener<T> where T : struct
    {
        public void OnValueChanged(T value);
    }
}
