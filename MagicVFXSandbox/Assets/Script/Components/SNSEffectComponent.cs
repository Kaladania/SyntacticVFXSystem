using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.VFX;

namespace SnSECS
{
    /// <summary>
    /// Holds the generated visual effect data
    /// </summary>
    public struct SNSEffectComponent : ISharedComponentData
    {
        public int _seedID; //PCG Seed
        public VisualEffectAsset _head; //projectile head
        public VisualEffectAsset _trail; //projectile trail
        public VisualEffectAsset _ambience; //projectile ambience
    }

}