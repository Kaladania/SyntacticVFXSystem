using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;

namespace SnSECS
{
    /// <summary>
    /// Holds the visual effect paramters for a given element
    /// </summary>
    public struct SNSFireComponent : ISharedComponentData, IEquatable<SNSFireComponent>, ICloneable, IDisposable
    {

        //public List<ElementType> _types; //type of element
        //public bool[] _types;
        public HashSet<ElementType> _types;
        public VisualEffectAsset _head; //element projectile head
        public VisualEffectAsset _trail; //element projectile trail
        public VisualEffectAsset _ambience; //element projectile ambience
        public float _density; //density of the effect
        public float _scale; //the size of the effect
        public float _speed; //speed of the effect
        public Color _colour; //effect colour

        /// <summary>
        /// Constructs the component with it's default values
        /// </summary>
        /// <param name="elementType"></param>
        public SNSFireComponent(ElementType elementType)
        {
            /*switch (elementType)
            {
                case ElementType.BASE:

                    _type = elementType;
                    _head = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Fire.vfx", typeof(VisualEffectAsset));
                    _trail = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Fire.vfx", typeof(VisualEffectAsset));
                    break;
                case ElementType.EXTRA:

                    _type = elementType;
                    _head = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Fire.vfx", typeof(VisualEffectAsset));
                    _trail = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Fire.vfx", typeof(VisualEffectAsset));
                    break;
                default:
                    //defaults to spawning the 'EXTRA' VFXs
                    _type = elementType;
                    _head = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Fire.vfx", typeof(VisualEffectAsset));
                    _trail = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Fire.vfx", typeof(VisualEffectAsset));
                    break;
            }*/

            //_types = new List<ElementType>();
            //_types.Add(elementType);

            /*_types = new bool[3];

            //updates the type list if the element type is valid
            //type "None" is used if want to initalise a basic, uncustomised component
            if (elementType != ElementType.NONE)
            {
                _types[((int)elementType)] = true;
            }*/

            _types = new HashSet<ElementType>();

            if (elementType != ElementType.NONE)
            {
                _types.Add(elementType);
            }

            _head = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Head_Fire.vfx", typeof(VisualEffectAsset));
            _trail = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Tail_Fire.vfx", typeof(VisualEffectAsset));
            _ambience = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath("Assets/Systems/Base_Ambience_Fire.vfx", typeof(VisualEffectAsset));

            _scale = 1;
            _speed = 1;
            _density = 1;
            _colour = Color.white;
        }

        public override bool Equals(object obj)
        {
            return obj is SNSFireComponent component && Equals(component);
        }

        public bool Equals(SNSFireComponent other)
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

        public static bool operator ==(SNSFireComponent left, SNSFireComponent right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SNSFireComponent left, SNSFireComponent right)
        {
            return !(left == right);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(_head);
            UnityEngine.Object.Destroy(_trail);
            UnityEngine.Object.Destroy(_ambience);
        }

        public object Clone()
        {
            return new SNSFireComponent
            {
                _head = UnityEngine.Object.Instantiate(_head),
                _trail = UnityEngine.Object.Instantiate(_trail),
                _ambience = UnityEngine.Object.Instantiate(_ambience)
            };
        }

        /*public int Compare(Elements a, Elements b)
        {
            int3 cellId = GetCellID(GridBounds, CellsPerAxis, a.xyz);
            int3 otherCellId = GetCellID(GridBounds, CellsPerAxis, b.xyz);
            int xDiff = cellId.x.CompareTo(otherCellId.x);
            if (xDiff == 0)
            {
                int yDiff = cellId.y.CompareTo(otherCellId.y);
                if (yDiff == 0)
                {
                    return cellId.z.CompareTo(otherCellId.z);
                }
                return yDiff;
            }
            return xDiff;
        }*/
    }
}

