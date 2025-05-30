using System.Collections.Generic;
using Core.Business.Enums;
using Core.Services.GameData;
using Core.Services.PlayerData;
using Core.Services.PlayerData.Business;
using Core.Utils.PlayerData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Services
{
    public class ServicesProvider : MonoBehaviour
    {
        public GameDataProviderService GameDataProviderService { get; private set; }
        public PlayerDataService PlayerDataService { get; private set; }
        
        public EntityFactoriesService EntityFactoriesService { get; private set; }

        public void Initialize()
        {
            GameDataProviderService = new GameDataProviderService();
            PlayerDataService = new PlayerDataService(this);
            EntityFactoriesService = new EntityFactoriesService(this);
        }
    }
}