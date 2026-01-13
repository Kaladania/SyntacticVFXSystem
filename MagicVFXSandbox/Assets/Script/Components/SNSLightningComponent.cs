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
    public struct SNSLightningComponet : ISharedComponentData, IEquatable<SNSLightningComponet>
    {

        public List<ElementType> _types; //type of element
        public VisualEffectAsset _head; //element projectile head
        public VisualEffectAsset _trail; //element projectile trail
        public VisualEffectAsset _ambience; //element projectile ambience

        public float _scale; //the size of the effect
        public float _speed; //speed of the effect
        public float _density; //density of the effect
        public Color _colour; //effect colour

        /// <summary>
        /// Constructs the component with it's default values
        /// </summary>
        /// <param name="elementType"></param>
        public SNSLightningComponet(ElementType elementType)
        {
            /*switch (elementType)
            {
                case ElementType.BASE:

                    _type = elementType;
                    _head = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_.vfx", typeof(VisualEffectAsset));
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
            }*/

            _types = new List<ElementType>();
            _types.Add(elementType);

            _head = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Lightning.vfx", typeof(VisualEffectAsset));
            _trail = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Tail_Lightning.vfx", typeof(VisualEffectAsset));
            _ambience = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Ambience_Lightning.vfx", typeof(VisualEffectAsset));

            _scale = 1;
            _speed = 1;
            _density = 1;
            _colour = Color.white;
        }

        public override bool Equals(object obj)
        {
            return obj is SNSLightningComponet componet && Equals(componet);
        }

        public bool Equals(SNSLightningComponet other)
        {
            return _types == other._types &&
                   EqualityComparer<VisualEffectAsset>.Default.Equals(_head, other._head) &&
                   EqualityComparer<VisualEffectAsset>.Default.Equals(_trail, other._trail) &&
                   EqualityComparer<VisualEffectAsset>.Default.Equals(_ambience, other._ambience);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_types, _head, _trail, _ambience);
        }

        public static bool operator ==(SNSLightningComponet left, SNSLightningComponet right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SNSLightningComponet left, SNSLightningComponet right)
        {
            return !(left == right);
        }
    }
}
