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
                            
                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.BASE));
                            break;
                        case Elements.EARTH:
                            entityManager.AddSharedComponentManaged(entity, new SNSEarthComponet(ElementType.BASE));
                            break;
                        case Elements.WATER:
                            entityManager.AddSharedComponentManaged(entity, new SNSWaterComponet(ElementType.BASE));
                            break;
                        case Elements.LIGHTNING:

                            entityManager.AddSharedComponentManaged(entity, new SNSLightningComponet(ElementType.BASE));
                            break;

                        case Elements.NONE:

                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.BASE));
                            Debug.LogError("Base Element Enum = \"NONE\" when trying to load entity components from combo. \nDefaulted to Fire Elemnent");

                            break;
                        default:
                            break;
                    }

                }
                else if (1 < i && i < 3)
                {
                    switch (elements[i])
                    {
                        case Elements.FIRE:
                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.EXTRA));
                            break;
                        case Elements.EARTH:
                            entityManager.AddSharedComponentManaged(entity, new SNSEarthComponet(ElementType.EXTRA));
                            break;
                        case Elements.WATER:
                            entityManager.AddSharedComponentManaged(entity, new SNSWaterComponet(ElementType.EXTRA));
                            break;
                        case Elements.LIGHTNING:
                            entityManager.AddSharedComponentManaged(entity, new SNSLightningComponet(ElementType.EXTRA));
                            break;
                        case Elements.NONE:

                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.EXTRA));
                            Debug.LogError("Extra Element Enum = \"NONE\" when trying to load entity components from combo. \nDefaulted to Fire Elemnent");

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
                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.AMBIENCE));
                            break;
                        case Elements.EARTH:
                            entityManager.AddSharedComponentManaged(entity, new SNSEarthComponet(ElementType.AMBIENCE));
                            break;
                        case Elements.WATER:
                            entityManager.AddSharedComponentManaged(entity, new SNSWaterComponet(ElementType.AMBIENCE));
                            break;
                        case Elements.LIGHTNING:
                            entityManager.AddSharedComponentManaged(entity, new SNSLightningComponet(ElementType.AMBIENCE));
                            break;
                        case Elements.NONE:

                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.AMBIENCE));
                            Debug.LogError("Extra Element Enum = \"NONE\" when trying to load entity components from combo. \nDefaulted to Fire Elemnent");

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