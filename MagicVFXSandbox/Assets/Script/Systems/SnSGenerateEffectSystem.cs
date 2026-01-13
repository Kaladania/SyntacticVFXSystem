using System.Collections.Generic;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;
using static UnityEngine.ParticleSystem;

namespace SnSECS
{

    //PRE-CONDITIONS:
    //- Entity has the required components derrived from the type combo
    //- Components where their element was featured multiple times have altered stats (so there is a limit of 1 component per element)

    /// <summary>
    /// Generates a layered SNS system
    /// </summary>
    /// /// <param name="entity"> The entity attached element componets to use in the generation</param>
    public struct SnSGenerateEffectSystem
    {
         
        public static List<VisualEffectAsset> GenerateSnS(Entity entity)
        {
            //initalise variables
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            List<VisualEffectAsset> generatedVFXs = new List<VisualEffectAsset>();

            //grabs an array full of the type of components attached to the entity
            NativeArray<ComponentType> elementArray = entityManager.GetComponentTypes(entity, Allocator.Temp);

            //Starts at 1 because Unity automatically stores a 'simulate' flag at index 0
            for (int i = 1; i < elementArray.Length; i++)
            {
               
                //loads the correct VFX and alter's it's state depending on the component type at the current index
                if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSFireComponent>())
                {
                    if (i == 1)
                    {
                        //Adds the specified asset to the list of assets to spawn
                        generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSFireComponent>(entity)._head);
                    }
                    else
                    {
                        generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSFireComponent>(entity)._trail);
                        //TODO: TYPE-EXTRA stuff
                    }

                    generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSFireComponent>(entity)._ambience);

                }
                else if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSWaterComponet>())
                {
                    if (i == 1)
                    {
                        
                        //Adds the specified asset to the list of assets to spawn
                        generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSWaterComponet>(entity)._head);
                    }
                    else
                    {
                        generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSWaterComponet>(entity)._trail);
                        //TODO: TYPE-EXTRA stuff
                    }

                    generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSWaterComponet>(entity)._ambience);
                }
                else if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSEarthComponet>())  // elementArray[0].GetHashCode() == basicWaterComponent.GetHashCode())
                {
                    if (i == 1)
                    {

                        //Adds the specified asset to the list of assets to spawn
                        generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSEarthComponet>(entity)._head);
                    }
                    else
                    {
                        generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSEarthComponet>(entity)._trail);
                        //TODO: TYPE-EXTRA stuff
                    }

                    generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSEarthComponet>(entity)._ambience);
                }
                else if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSLightningComponet>())  // elementArray[0].GetHashCode() == basicWaterComponent.GetHashCode())
                {
                    if (i == 1)
                    {

                        //Adds the specified asset to the list of assets to spawn
                        generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSLightningComponet>(entity)._head);
                    }
                    else
                    {
                        generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSLightningComponet>(entity)._trail);
                        //TODO: TYPE-EXTRA stuff
                    }

                    generatedVFXs.Add(entityManager.GetSharedComponentManaged<SNSLightningComponet>(entity)._ambience);
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
            return generatedVFXs;
        }

        /*private static VisualEffectAsset LoadVFXAsset(Elements element, ElementType type)
        {

        }
*/

        /// <summary>
        /// Loads the corresponding VFX system for the given element and type
        /// </summary>
        /// <param name="element">element vfx to return</param>
        /// <param name="type">type of element vfx</param>
        /// <returns>The corresponding VFX asset</returns>
        private static VisualEffectAsset LoadVFXAssets(Elements element, ElementType type)
        {
            VisualEffectAsset assetToReturn = null;

            switch (element)
            {
                case Elements.FIRE:

                    switch (type)
                    {
                        case ElementType.BASE:

                            assetToReturn = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Fire").Result;
                            break;
                        case ElementType.EXTRA:

                            assetToReturn = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Fire").Result;
                            break;

                        case ElementType.AMBIENCE:

                            assetToReturn = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Fire").Result;
                            break;
                        default:
                            //defaults to spawning the 'EXTRA' VFXs
                            assetToReturn = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Fire").Result;
                            break;
                    }

                    break;
                case Elements.EARTH:
                    break;
                case Elements.WATER:

                    switch (type)
                    {
                        case ElementType.BASE:

                            assetToReturn = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Water").Result;
                            break;
                        case ElementType.EXTRA:

                            assetToReturn = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Water").Result;
                            break;

                        case ElementType.AMBIENCE:

                            assetToReturn = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Water").Result;
                            break;
                        default:
                            //defaults to spawning the 'EXTRA' VFXs
                            assetToReturn = Addressables.LoadAssetAsync<VisualEffectAsset>("Base_Head_Water").Result;
                            break;
                    }

                    break;
                case Elements.LIGHTNING:
                    break;
                case Elements.NONE:
                    break;
                default:
                    break;
            }

            return assetToReturn;
            
        }
    }

}