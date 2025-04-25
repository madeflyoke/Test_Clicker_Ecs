using System.Reflection;
using Core.Currency.Components.Events;
using Core.Utils.Enums;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class AddCurrencyDebugBtn : MonoBehaviour
    {
        [SerializeField] private Button _btn;
        [SerializeField] private TMP_InputField _value;
    
        public void Start()
        {
            _btn.onClick.AddListener(AddCurr);
            void AddCurr()
            {
                int.TryParse(_value.text, out int value);

                EcsWorld world = (EcsWorld) typeof(EcsBootstrapper).GetField("_defaultWorld", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(FindObjectOfType<EcsBootstrapper>());
            
                MoneyCurrencyChangedRequestComponent component = new MoneyCurrencyChangedRequestComponent()
                {
                    Operation = OperationType.Add,
                    Value = Mathf.Clamp(value,0, int.MaxValue)
                };
                world.GetPool<MoneyCurrencyChangedRequestComponent>()
                    .Add(world.NewEntity()) = component;
            }
        }
    }
}
