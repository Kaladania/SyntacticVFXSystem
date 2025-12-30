using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.VFX;

namespace SnSECS
{


    /// <summary>
    /// Holds the visual effect paramters for a given element
    /// </summary>
    public struct SNSElementComponent : ISharedComponentData, IEquatable<SNSElementComponent>
    {
        public ElementType _type; //type of element
        public VisualEffectAsset _head; //element projectile head
        public VisualEffectAsset _trail; //element projectile trail
        public VisualEffectAsset _ambience; //element projectile ambience

        public override bool Equals(object obj)
        {
            return obj is SNSElementComponent component && Equals(component);
        }

        public bool Equals(SNSElementComponent other)
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

        public static bool operator ==(SNSElementComponent left, SNSElementComponent right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SNSElementComponent left, SNSElementComponent right)
        {
            return !(left == right);
        }
    }

}