using UnityEngine;
using Unity.Entities;

namespace SnSECS
{
    //Sets data to an Entity
    //Done here as Entites can only be created at runtime
    public class SnSSpawnerAuthoring : MonoBehaviour
    {
        public GameObject _projectilePrefab; //holds the prefab blueprint used to spawn projectiles

    }

    //Baker will create an entity and assign components to it

    //Entities cannot be accessed or created in the Unity Editor
    //Requires a bake to generate and populate said Entities instead
    class SnSSpawnerBaker : Baker<SnSSpawnerAuthoring>
    {
        /// <summary>
        /// Creates a new entity
        /// </summary>
        /// <param name="authoring">A copy of the unity editor assigned values to pass into the Entity components</param>
        public override void Bake(SnSSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            //Adds a spawn component to the entity
            AddComponent(entity, new SnSSpawnComponent
            {
                //Sets the component's properties
                _particlePrefab = GetEntity(authoring._projectilePrefab, TransformUsageFlags.Dynamic),
                _spawnPosition = authoring.transform.position
            });
            //throw new System.NotImplementedException();
        }
        
    }
}
 