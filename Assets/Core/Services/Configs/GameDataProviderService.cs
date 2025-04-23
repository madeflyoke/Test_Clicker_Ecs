using Core.Business.Data;
using Core.Terms;
using Core.Utils;
using UnityEngine;

namespace Core.Services.Configs
{
    public class GameDataProviderService
    {
        public BusinessConfig BusinessConfig { get; private set; }
        public TermsConfig TermsConfig { get; private set; }

        public GameDataProviderService()
        {
            BusinessConfig = Resources.Load<BusinessConfig>(Constants.ResourcesPaths.BusinessConfig);
            TermsConfig = Resources.Load<TermsConfig>(Constants.ResourcesPaths.TermsConfig);
        }
    }
}
