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
            List<ElementType> entityElements = new List<ElementType>();

            const float scaleModifier = 0.2f;
            const float speedModifier = 0.2f;
            const float densityModifier = 2f;


            //TODO: USE FIRE AS AN EXAMPLE TO REWRITE THE LOAD ELEMENT CODE
            //Propogate a template component. ONLY ADD THE TEMPLATE COMPONENTS TO THE ENTITY AFTER ALL ELEMENTS HAVE BEEN ANALYSED
            //Allows template components to be udpated without having to continously get and set copies
            //focus on the dictionary but leave the list for now. If the dictionary doesn't work due to the class degrading due to the cast
            //      add the elements to the list instead. Use a find function to check if the element is already in the list


            //adds the all specifed componets to the entity
            for (int i = 0; i < elements.Count; i++)
            {
                switch (elements[i])
                {
                    case Elements.FIRE:

                        if (componentMap.ContainsKey(Elements.FIRE))
                        {
                            tempFireComponent = (SNSFireComponent)componentMap[Elements.FIRE];
                            tempFireComponent._scale += scaleModifier;
                            tempFireComponent._density += densityModifier;
                            tempFireComponent._speed += speedModifier;
                        }
                        
                        
                        //componentMap.Add(Elements.FIRE, entityManager.AddSharedComponentManaged<SNSFireComponent>(entity));
                        //tempFireComponent = (SNSFireComponent)componentMap[Elements.FIRE];

                        if (i == 0)
                        {
                            tempFireComponent._types.Add(ElementType.BASE);
                            //entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.BASE));
                                
                        }
                        else if (1 <= i && i <= 3)
                        {
                            tempFireComponent._types.Add(ElementType.EXTRA);
                            //entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.EXTRA));
                        }
                        else
                        {
                            tempFireComponent._types.Add(ElementType.AMBIENCE);
                            //entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.AMBIENCE));
                        }


                        componentMap[Elements.FIRE] = tempFireComponent;

                        break;

                    case Elements.EARTH:
                        break;
                    case Elements.WATER:
                        break;
                    case Elements.LIGHTNING:
                        break;
                    case Elements.NONE:
                        break;
                    default:
                        break;
                }

                



                if (i == 0) //1st element is assigned 'BASE' for it's Head VFX
                {

                    switch (elements[i])
                    {
                        case Elements.FIRE:
                            
                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.BASE));
                            tempFireComponent = entityManager.GetSharedComponentManaged<SNSFireComponent>(entity);
                            break;
                        case Elements.EARTH:
                            entityManager.AddSharedComponentManaged(entity, new SNSEarthComponet(ElementType.BASE));
                            tempEarthComponent = entityManager.GetSharedComponentManaged<SNSEarthComponet>(entity);
                            break;
                        case Elements.WATER:
                            entityManager.AddSharedComponentManaged(entity, new SNSWaterComponet(ElementType.BASE));
                            tempFireComponent = entityManager.GetSharedComponentManaged<SNSFireComponent>(entity);
                            break;
                        case Elements.LIGHTNING:
                            entityManager.AddSharedComponentManaged(entity, new SNSLightningComponet(ElementType.BASE));
                            tempLightningComponent = entityManager.GetSharedComponentManaged<SNSLightningComponet>(entity);
                            break;

                        case Elements.NONE:

                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.BASE));
                            tempFireComponent = entityManager.GetSharedComponentManaged<SNSFireComponent>(entity);
                            Debug.LogError("Base Element Enum = \"NONE\" when trying to load entity components from combo. \nDefaulted to Fire Elemnent");

                            break;
                        default:
                            break;
                    }

                }
                else if (1 < i && i < 3) //2nd and 3rc elements are assigned 'EXTRA' for their tail VFX
                {
                    switch (elements[i])
                    {
                        case Elements.FIRE:
                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.EXTRA));
                            tempFireComponent = entityManager.GetSharedComponentManaged<SNSFireComponent>(entity);
                            break;
                        case Elements.EARTH:
                            entityManager.AddSharedComponentManaged(entity, new SNSEarthComponet(ElementType.EXTRA));
                            tempEarthComponent = entityManager.GetSharedComponentManaged<SNSEarthComponet>(entity);
                            break;
                        case Elements.WATER:
                            entityManager.AddSharedComponentManaged(entity, new SNSWaterComponet(ElementType.EXTRA));
                            tempWaterComponent = entityManager.GetSharedComponentManaged<SNSWaterComponet>(entity);
                            break;
                        case Elements.LIGHTNING:
                            entityManager.AddSharedComponentManaged(entity, new SNSLightningComponet(ElementType.EXTRA));
                            tempLightningComponent = entityManager.GetSharedComponentManaged<SNSLightningComponet>(entity);
                            break;
                        case Elements.NONE:

                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.EXTRA));
                            tempFireComponent = entityManager.GetSharedComponentManaged<SNSFireComponent>(entity);
                            Debug.LogError("Extra Element Enum = \"NONE\" when trying to load entity components from combo. \nDefaulted to Fire Elemnent");

                            break;
                        default:
                            break;
                    }
                }
                else //4th and 5th elements are assigned 'AMBIENCE' for their Ambience
                {
                    switch (elements[i])
                    {
                        case Elements.FIRE:
                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.AMBIENCE));
                                componentMap.Add(Elements.FIRE, entityManager.GetSharedComponentManaged<SNSFireComponent>(entity));
                            tempFireComponent = entityManager.GetSharedComponentManaged<SNSFireComponent>(entity);
                            break;
                        case Elements.EARTH:
                            entityManager.AddSharedComponentManaged(entity, new SNSEarthComponet(ElementType.AMBIENCE));
                            tempEarthComponent = entityManager.GetSharedComponentManaged<SNSEarthComponet>(entity);
                            break;
                        case Elements.WATER:
                            entityManager.AddSharedComponentManaged(entity, new SNSWaterComponet(ElementType.AMBIENCE));
                            tempWaterComponent = entityManager.GetSharedComponentManaged<SNSWaterComponet>(entity);
                            break;
                        case Elements.LIGHTNING:
                            entityManager.AddSharedComponentManaged(entity, new SNSLightningComponet(ElementType.AMBIENCE));
                            tempLightningComponent = entityManager.GetSharedComponentManaged<SNSLightningComponet>(entity);
                            break;
                        case Elements.NONE:

                            entityManager.AddSharedComponentManaged(entity, new SNSFireComponent(ElementType.AMBIENCE));
                            tempFireComponent = entityManager.GetSharedComponentManaged<SNSFireComponent>(entity);
                            Debug.LogError("Extra Element Enum = \"NONE\" when trying to load entity components from combo. \nDefaulted to Fire Elemnent");

                            break;
                        default:
                            break;
                    }
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