using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;

namespace SnSECS
{
    /// <summary>
    /// Holds the visual effect paramters for a given element
    /// </summary>
    public struct SNSFireComponent : ISharedComponentData
    {

        public ElementType _type; //type of element
        public VisualEffectAsset _head; //element projectile head
        public VisualEffectAsset _trail; //element projectile trail
        public VisualEffectAsset _ambience; //element projectile ambience

        /// <summary>
        /// Constructs the component with it's default values
        /// </summary>
        /// <param name="elementType"></param>
        public SNSFireComponent(ElementType elementType)
        {
            switch (elementType)
            {
                case ElementType.BASE:

                    _type = elementType;
                    _head = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Fire").Result;
                    _trail = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Fire").Result;
                    break;
                case ElementType.EXTRA:

                    _type = elementType;
                    _head = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Fire").Result;
                    _trail = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Fire").Result;
                    break;
                default:
                    //defaults to spawning the 'EXTRA' VFXs
                    _type = elementType;
                    _head = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Fire").Result;
                    _trail = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Fire").Result;

                    break;
            }

            _ambience = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Fire").Result;
        }

    }
}

