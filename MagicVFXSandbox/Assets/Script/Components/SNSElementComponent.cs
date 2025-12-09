using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.VFX;

namespace SnSECS
{


    /// <summary>
    /// Holds the visual effect paramters for a given element
    /// </summary>
    public struct SNSElementComponent : ISharedComponentData
    {
        public ElementType _type; //type of element
        public VisualEffectAsset _head; //element projectile head
        public VisualEffectAsset _trail; //element projectile trail
        public VisualEffectAsset _ambience; //element projectile ambience
    }

}