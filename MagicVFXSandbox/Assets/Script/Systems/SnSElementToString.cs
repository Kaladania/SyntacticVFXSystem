using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.VFX;

namespace SnSECS
{

    //PRE-CONDITIONS:
    //- Entity has the required components derrived from the type combo
    //- Components where their element was featured multiple times have altered stats (so there is a limit of 1 component per element)

    /// <summary>
    /// Generates a layered SNS system
    /// </summary>
    /// /// <param name="entity"> The entity attached element componets to use in the generation</param>
    public struct SnSElementToStringSystem
    {

        public static string Get(Entity entity)
        {
            //initalise variables
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            List<VisualEffectAsset> generatedVFXs = new List<VisualEffectAsset>();

            SNSFireComponent tempFireComponent;
            SNSWaterComponet tempWaterComponent;
            SNSLightningComponet tempLightningComponent;
            SNSEarthComponet tempEarthComponent;

            //grabs an array full of the type of components attached to the entity
            NativeArray<ComponentType> elementArray = entityManager.GetComponentTypes(entity, Allocator.Temp);

            //Adds the specified asset to the list of assets to spawn
            //Starts at 1 because Unity automatically stores a 'simulate' flag at index 0
            for (int i = 1; i < elementArray.Length; i++)
            {

                //loads the correct VFX and alter's it's state depending on the component type at the current index
                if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSFireComponent>())
                {
                    tempFireComponent = entityManager.GetSharedComponentManaged<SNSFireComponent>(entity);

                    foreach (ElementType element in tempFireComponent._types)
                    {
                        switch (element)
                        {
                            case ElementType.BASE:

                                generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSFireComponent>(entity)._head);
                                break;

                            case ElementType.EXTRA:

                                generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSFireComponent>(entity)._trail);
                                break;
                            case ElementType.AMBIENCE:

                                generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSFireComponent>(entity)._ambience);
                                break;
                            default:
                                break;
                        }
                    }

                }
                else if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSWaterComponet>())
                {
                    tempWaterComponent = entityManager.GetSharedComponentManaged<SNSWaterComponet>(entity);

                    foreach (ElementType element in tempWaterComponent._types)
                    {
                        switch (element)
                        {
                            case ElementType.BASE:


                                generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSWaterComponet>(entity)._head);
                                break;

                            case ElementType.EXTRA:

                                generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSWaterComponet>(entity)._trail);
                                break;
                            case ElementType.AMBIENCE:

                                generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSWaterComponet>(entity)._ambience);
                                break;
                            default:
                                break;
                        }
                    }

                }
                else if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSEarthComponet>())  // elementArray[0].GetHashCode() == basicWaterComponent.GetHashCode())
                {
                    tempEarthComponent = entityManager.GetSharedComponentManaged<SNSEarthComponet>(entity);

                    foreach (ElementType element in tempEarthComponent._types)
                    {
                        switch (element)
                        {
                            case ElementType.BASE:


                                generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSEarthComponet>(entity)._head);
                                break;

                            case ElementType.EXTRA:

                                generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSEarthComponet>(entity)._trail);
                                break;
                            case ElementType.AMBIENCE:

                                generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSEarthComponet>(entity)._ambience);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSLightningComponet>())  // elementArray[0].GetHashCode() == basicWaterComponent.GetHashCode())
                {
                    tempLightningComponent = entityManager.GetSharedComponentManaged<SNSLightningComponet>(entity);

                    foreach (ElementType element in tempLightningComponent._types)
                    {
                        switch (element)
                        {
                            case ElementType.BASE:


                                generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSLightningComponet>(entity)._head);
                                break;

                            case ElementType.EXTRA:

                                generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSLightningComponet>(entity)._trail);
                                break;
                            case ElementType.AMBIENCE:

                                generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSLightningComponet>(entity)._ambience);
                                break;
                            default:
                                break;
                        }
                    }
                }

                /// IF COMPONENT TYPE IS FOUND, SPAWN THE SYSTEM ATTACHED [CURRENT PLAN IS TO USE A VISUAL EFFECT COMPONENT FOR EACH ELEMENT]
                /// TRY AND SEE IF THERES A WAY TO ADD A NODE CHAIN TO A VFX ASSET INSTEAD OF HAVING TO STORE THE ENTIRE ASSET
                /// (LIKE HOW YOU CAN REFERENCE A SPECIFIC CHAIN IN NIAGARA)
            }

            #region (Commented Out) Code if want to load the assets here instead of via their components

            /*VisualEffectAsset snsEffect = new VisualEffectAsset();
            VisualEffectAsset loadedElementAsset = new VisualEffectAsset();
            //adds the all specifed componets to the entity
            for (int i = 0; i < elements.Count; i++)
            {




                loadedElementAsset = LoadVFXAssets(Elements.WATER, ElementType.BASE);

                snsEffect.GetExposedProperties


                if (i == 0) //the first element is always designated as the 'base' element
                {

                    switch (elements[i])
                    {
                        case Elements.FIRE:
                            entityManager.AddComponentObject(entity, new SNSFireComponent(ElementType.BASE));
                            break;
                        case Elements.EARTH:
                            break;
                        case Elements.WATER:
                            entityManager.AddComponentObject(entity, new SNSWaterComponet(ElementType.BASE));
                            break;
                        case Elements.LIGHTNING:
                            break;
                        case Elements.NONE:
                            break;
                        default:
                            break;
                    }
                }


                switch (elements[i])
                {
                    case Elements.FIRE:
                        entityManager.AddComponentObject(entity, new SNSFireComponent(ElementType.EXTRA));
                        break;
                    case Elements.EARTH:
                        break;
                    case Elements.WATER:
                        entityManager.AddComponentObject(entity, new SNSFireComponent(ElementType.EXTRA));
                        break;
                    case Elements.LIGHTNING:
                        break;
                    case Elements.NONE:
                        break;
                    default:
                        break;
                }
            }*/
            #endregion
            return string.Empty;
        }
    }
}
