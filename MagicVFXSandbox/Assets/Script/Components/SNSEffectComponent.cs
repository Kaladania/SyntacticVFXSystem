using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.VFX;

namespace SnSECS
{
    /// <summary>
    /// Holds the generated visual effect data
    /// </summary>
    public struct SNSEffectComponent : ISharedComponentData, IEquatable<SNSEffectComponent>
    {
        public int _seedID; //PCG Seed
        public VisualEffectAsset _head; //projectile head
        public VisualEffectAsset _trail; //projectile trail
        public VisualEffectAsset _ambience; //projectile ambience

        public override bool Equals(object obj)
        {
            return obj is SNSEffectComponent component && Equals(component);
        }

        public bool Equals(SNSEffectComponent other)
        {
            return _seedID == other._seedID &&
                   EqualityComparer<VisualEffectAsset>.Default.Equals(_head, other._head) &&
                   EqualityComparer<VisualEffectAsset>.Default.Equals(_trail, other._trail) &&
                   EqualityComparer<VisualEffectAsset>.Default.Equals(_ambience, other._ambience);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_seedID, _head, _trail, _ambience);
        }

        public static bool operator ==(SNSEffectComponent left, SNSEffectComponent right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SNSEffectComponent left, SNSEffectComponent right)
        {
            return !(left == right);
        }

        /*public override int GetHashCode()
           => _head.GetHashCode() ^ _trail.GetHashCode() ^ _ambience.GetHashCode();

        public override bool Equals(object other)
            => other is SNSEffectComponent otherS && Equals(otherS);

        public bool Equals(SNSEffectComponent other)
            => (_head == other._head) && (_trail == other._trail) && (_ambience == other._ambience);*/
    }

    /*public struct SNSEffectComponent : IEquatable<SNSEffectComponent>
    {
        private readonly int _value;
        public S(int f)
        {
            _value = f;
        }

        public override int GetHashCode()
            => _value.GetHashCode();

        public override bool Equals(object other)
            => other is S otherS && Equals(otherS);

        public bool Equals(S other)
            => _value == other._value;
    }*/

}