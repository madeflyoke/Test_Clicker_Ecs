using System;
using Core.Currency.Components.Events;
using Core.Utils.Enums;
using Packages.LeoEcs.VoodyConversion.MonoHelpers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddCurrDebugBtn : MonoBehaviour
{
    [SerializeField] private Button _btn;
    [SerializeField] private TMP_InputField _value;
    
    private void Awake()
    {
        _btn.onClick.AddListener(AddCurr);
    }

    private void AddCurr()
    {
        int.TryParse(_value.text, out int value);
        MoneyCurrencyChangedRequestComponent component = new MoneyCurrencyChangedRequestComponent()
        {
            Operation = OperationType.Add,
            Value = Mathf.Clamp(value,0, int.MaxValue)
        };
        WorldHandler.GetMainWorld().GetPool<MoneyCurrencyChangedRequestComponent>()
            .Add(WorldHandler.GetMainWorld().NewEntity()) = component;
    }
}
