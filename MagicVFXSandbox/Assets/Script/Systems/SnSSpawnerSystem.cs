using Unity.Entities;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;

namespace SnSECS
{
    //[BurstCompile]
    public partial struct SnSSpawnerSystem : ISystem
    {
        /// <summary>
        /// Runs every update frame
        /// </summary>
        /// <param name="state"></param>
        //[BurstCompile]
        public void OnUpdate(ref SystemState state) 
        {
            //Enforces a spawner singleton
            //exits function if the object that called it is not the singleton version
            if (!SystemAPI.TryGetSingletonEntity<SnSSpawnComponent>(out Entity spawnerEntity))
            {
                return;
            }

            //grabs a (read and writeable (RW)) reference to the spawn component held in the spawner singleton entity
            RefRW<SnSSpawnComponent> spawnerComponent = SystemAPI.GetComponentRW<SnSSpawnComponent>(spawnerEntity);

            /*EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            Entity entity = ecb.Instantiate(spawnerComponent.ValueRO._particlePrefab);

            ecb.Playback(state.EntityManager);*/

            //spawns an entity with the passed in prefab as it's game object
            Entity entity = state.EntityManager.Instantiate(spawnerComponent.ValueRO._particlePrefab);
            
            
            if (state.EntityManager.AddComponent<SNSEffectComponent>(entity))
            {
                RefRW<SNSEffectComponent> effectComponent = SystemAPI.GetComponentRW<SNSEffectComponent>(entity);
                //TODO: UPDATE ENTITY VALUES HERE
            }

            /*state.EntityManager.Instantiate()
            spawnerComponent.ValueRO._particlePrefab;
            entity.*/

                //create an entity and assign it a mesh (render compent?) to render a cube (in tutorial)
        }

    }
}