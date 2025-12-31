using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEditor;
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

        public float _scale; //the size of the effect
        public float _speed; //speed of the effect
        public Color _colour; //effect colour

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
                    _head = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Water.vfx", typeof(VisualEffectAsset));
                    _trail = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Water.vfx", typeof(VisualEffectAsset));
                    break;
                case ElementType.EXTRA:

                    _type = elementType;
                    _head = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Water.vfx", typeof(VisualEffectAsset));
                    _trail = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Water.vfx", typeof(VisualEffectAsset));
                    break;
                default:
                    //defaults to spawning the 'EXTRA' VFXs
                    _type = elementType;
                    _head = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Water.vfx", typeof(VisualEffectAsset));
                    _trail = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Water.vfx", typeof(VisualEffectAsset));

                    break;
            }

            _ambience = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Water.vfx", typeof(VisualEffectAsset));

            _scale = 1;
            _speed = 1;
            _colour = Color.white;
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
