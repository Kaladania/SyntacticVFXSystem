//using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

using UnityEngine.VFX;

/// <summary>
/// Provides data to spawn a syntactic system in game
/// </summary>
public struct SnSSpawnComponent : IComponentData
{

    public Entity _particlePrefab; //holds the spawned prefab containing the generated particle system
    public float3 _spawnPosition; //holds the position to spawn the prefab at
    //public VisualEffectAsset _generatedEffectAsset; //holds the generated syntactic particle system


}
