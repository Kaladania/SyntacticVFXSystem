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
        AMBIENCE,
        NONE
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

            SNSFireComponent tempFireComponent = new SNSFireComponent(ElementType.NONE);
            SNSWaterComponet tempWaterComponent = new SNSWaterComponet(ElementType.NONE);
            SNSLightningComponet tempLightningComponent = new SNSLightningComponet(ElementType.NONE);
            SNSEarthComponet tempEarthComponent = new SNSEarthComponet(ElementType.NONE);

            Dictionary<Elements, ISharedComponentData> componentMap = new Dictionary<Elements, ISharedComponentData>();

            const float scaleModifier = 0.2f;
            const float speedModifier = 0.2f;
            const float densityModifier = 2f;

            //adds the all specifed componets to the entity
            for (int i = 0; i < elements.Count; i++)
            {
                switch (elements[i])
                {
                    case Elements.FIRE:

                        //increases the modifers if the element is a duplicate (increases the intensity/prominence of the VFX)
                        if (componentMap.ContainsKey(Elements.FIRE))
                        {
                            //tempFireComponent = (SNSFireComponent)componentMap[Elements.FIRE];

                            tempFireComponent._scale += scaleModifier;
                            tempFireComponent._density += densityModifier;
                            tempFireComponent._speed += speedModifier;

                            
                            componentMap[Elements.FIRE] = tempWaterComponent;
                        }

                        //states the type of VFX needing to be loaded for this specific element
                        //One element can have multiple VFX types depending it's position
                        if (i == 0)
                        {
                            tempFireComponent._types.Add(ElementType.BASE);
                        }
                        else if (1 <= i && i <= 3)
                        {
                            tempFireComponent._types.Add(ElementType.EXTRA);
                        }
                        else
                        {
                            tempFireComponent._types.Add(ElementType.AMBIENCE);
                        }

                        componentMap[Elements.FIRE] = tempFireComponent;

                        break;


                    //increases the modifers if the element is a duplicate (increases the intensity/prominence of the VFX)
                    case Elements.WATER:

                        if (componentMap.ContainsKey(Elements.WATER))
                        {
                            tempWaterComponent = (SNSWaterComponet)componentMap[Elements.WATER];
                            tempWaterComponent._scale += scaleModifier;
                            tempWaterComponent._density += densityModifier;
                            tempWaterComponent._speed += speedModifier;
                        }

                        if (i == 0)
                        {
                            tempWaterComponent._types.Add(ElementType.BASE);
                        }
                        else if (1 <= i && i <= 3)
                        {
                            tempWaterComponent._types.Add(ElementType.EXTRA);
                        }
                        else
                        {
                            tempWaterComponent._types.Add(ElementType.AMBIENCE);
                        }

                        componentMap[Elements.WATER] = tempWaterComponent;

                        break;
                }
            }


                
            //Adds the finished list of components to the entity
            foreach (KeyValuePair<Elements, ISharedComponentData> component in componentMap)
            {
                switch (component.Key)
                {
                    case Elements.FIRE:
                        entityManager.AddSharedComponentManaged(entity, (SNSFireComponent)componentMap[Elements.FIRE]);
                        break;
                    case Elements.EARTH:
                        entityManager.AddSharedComponentManaged(entity, (SNSEarthComponet)componentMap[Elements.EARTH]);
                        break;
                    case Elements.WATER:
                        entityManager.AddSharedComponentManaged(entity, (SNSWaterComponet)componentMap[Elements.WATER]);
                        break;
                    case Elements.LIGHTNING:
                        entityManager.AddSharedComponentManaged(entity, (SNSLightningComponet)componentMap[Elements.LIGHTNING]);
                        break;
                    case Elements.NONE:
                        entityManager.AddSharedComponentManaged(entity, (SNSFireComponent)componentMap[Elements.FIRE]);
                        Debug.LogWarning("Element Key in Component Map Dictionary was set to \"NONE\". Defaulting to Fire Element.");
                        break;
                    default:
                        break;
                }
            }

            return entity;
        }

    }
}