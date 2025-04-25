using Core.UI.Gameplay.Business;
using UnityEngine;

namespace Core.Services.GameData
{
    [CreateAssetMenu(fileName = "ViewPrefabsProvider", menuName = "Game/ViewPrefabsProvider")]
    public class ViewPrefabsProvider : ScriptableObject
    {
        [field: SerializeField] public BusinessElementView BusinessElementViewPrefab { get;private set; }
    }
}
