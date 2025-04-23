using System;
using Core.Services;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private ServicesProvider _servicesProvider;
        [SerializeField] private EcsBootstrapper _ecsBootstrapper;

        public void Start()
        {
            _servicesProvider.Initialize();
            _ecsBootstrapper.Initialize(_servicesProvider.PlayerDataService, _servicesProvider.GameDataProviderService);
        }
    }
}
