using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace SnSECS
{

    public enum ElementType
    {
        BASE,
        EXTRA,
        AMBIENCE
    }

    public enum Elements
    {
        FIRE,
        EARTH,
        WATER,
        LIGHTNING,
        NONE
    }
    public struct SnSLoadElementsSystem
    {
        /// <summary>
        /// Runs every update frame
        /// </summary>
        /// <param name="state"></param>
        public static Entity LoadElement(List<Elements> elements)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //spawns an entity with the passed in prefab as it's game object
            Entity entity = entityManager.CreateEntity();

            //adds the all specifed componets to the entity
            for (int i = 0; i < elements.Count; i++)
            {
                if (i == 0) //the first element is always designated as the 'base' element
                {

                    switch (elements[i])
                    {
                        case Elements.FIRE:
                            SNSFireComponent newFireComponent = new SNSFireComponent(ElementType.BASE);
                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.BASE));
                            break;
                        case Elements.EARTH:
                            break;
                        case Elements.WATER:
                            entityManager.AddSharedComponentManaged(entity, new SNSWaterComponet(ElementType.BASE));
                            break;
                        case Elements.LIGHTNING:
                            break;
                        case Elements.NONE:
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    switch (elements[i])
                    {
                        case Elements.FIRE:
                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.EXTRA));
                            break;
                        case Elements.EARTH:
                            break;
                        case Elements.WATER:
                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.EXTRA));
                            break;
                        case Elements.LIGHTNING:
                            break;
                        case Elements.NONE:
                            break;
                        default:
                            break;
                    }
                }

            }

            return entity;
        }

    }
}