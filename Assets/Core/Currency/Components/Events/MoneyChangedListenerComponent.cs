using System;
using Core.Currency.Interfaces;

namespace Core.Currency.Components.Events
{
    [Serializable]
    public struct MoneyChangedListenerComponent
    {
        public IMoneyChangedListener Value;
    }
}
