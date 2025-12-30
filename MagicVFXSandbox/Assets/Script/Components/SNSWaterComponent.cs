using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;

namespace SnSECS
{
    /// <summary>
    /// Holds the visual effect paramters for a given element
    /// </summary>
    public struct SNSWaterComponet : ISharedComponentData, IEquatable<SNSWaterComponet>
    {

        public ElementType _type; //type of element
        public VisualEffectAsset _head; //element projectile head
        public VisualEffectAsset _trail; //element projectile trail
        public VisualEffectAsset _ambience; //element projectile ambience

        /// <summary>
        /// Constructs the component with it's default values
        /// </summary>
        /// <param name="elementType"></param>
        public SNSWaterComponet(ElementType elementType)
        {
            switch (elementType)
            {
                case ElementType.BASE:

                    _type = elementType;
                    _head = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Water").Result;
                    _trail = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Water").Result;
                    break;
                case ElementType.EXTRA:

                    _type = elementType;
                    _head = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Water").Result;
                    _trail = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Water").Result;
                    break;
                default:
                    //defaults to spawning the 'EXTRA' VFXs
                    _type = elementType;
                    _head = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Water").Result;
                    _trail = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Water").Result;

                    break;
            }

            _ambience = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Water").Result;
        }

        public override bool Equals(object obj)
        {
            return obj is SNSWaterComponet componet && Equals(componet);
        }

        public bool Equals(SNSWaterComponet other)
        {
            return _type == other._type &&
                   EqualityComparer<VisualEffectAsset>.Default.Equals(_head, other._head) &&
                   EqualityComparer<VisualEffectAsset>.Default.Equals(_trail, other._trail) &&
                   EqualityComparer<VisualEffectAsset>.Default.Equals(_ambience, other._ambience);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_type, _head, _trail, _ambience);
        }

        public static bool operator ==(SNSWaterComponet left, SNSWaterComponet right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SNSWaterComponet left, SNSWaterComponet right)
        {
            return !(left == right);
        }
    }
}
