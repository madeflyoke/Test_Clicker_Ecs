using Core.Common.Interfaces;

namespace Core.Common.Components.Events
{
    public struct CommonListenerComponent<T> where T: struct
    {
        public ICommonNotifyListener NotifyListener;
    }
}
