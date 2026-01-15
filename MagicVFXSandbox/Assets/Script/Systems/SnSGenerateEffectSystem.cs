using System.Collections.Generic;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;
using static UnityEngine.EventSystems.EventTrigger;
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
         
        public static List<VisualEffect> GenerateSnS(Entity entity)
        {
            //initalise variables
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            List<VisualEffect> generatedVFXs = new List<VisualEffect>();

            SNSFireComponent tempFireComponent;
            SNSWaterComponent tempWaterComponent;
            SNSLightningComponent tempLightningComponent;
            SNSEarthComponent tempEarthComponent;

            VisualEffect tempVFXComponent;

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

                    foreach (ElementType type in tempFireComponent._types)
                    {
                        generatedVFXs.Add(CreateVFXComponent(tempFireComponent, type));

                    }
                    

                }
                else if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSWaterComponent>())
                {
                    tempWaterComponent = entityManager.GetSharedComponentManaged<SNSWaterComponent>(entity);

                    foreach (ElementType type in tempWaterComponent._types)
                    {
                        generatedVFXs.Add(CreateVFXComponent(tempWaterComponent, type));

                    }

                }
                else if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSEarthComponent>())
                {
                    tempEarthComponent = entityManager.GetSharedComponentManaged<SNSEarthComponent>(entity);

                    foreach (ElementType type in tempEarthComponent._types)
                    {
                        generatedVFXs.Add(CreateVFXComponent(tempEarthComponent, type));

                    }

                }
                else if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSLightningComponent>())
                {
                    tempLightningComponent = entityManager.GetSharedComponentManaged<SNSLightningComponent>(entity);

                    foreach (ElementType type in tempLightningComponent._types)
                    {
                        generatedVFXs.Add(CreateVFXComponent(tempLightningComponent, type));

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
                            entityManager.AddComponentObject(entity, new SNSWaterComponent(ElementType.BASE));
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

        /// <summary>
        /// Creates a new VFX component and updates it with the correct VFX asset and asset stats
        /// </summary>
        /// <param name="component">Entity component to load VFX data from</param>
        /// <param name="type">Type of VFX data to load</param>
        /// <returns>The initalised VFX component</returns>
        private static VisualEffect CreateVFXComponent(SNSFireComponent component, ElementType type)
        {
            //sets up the VFX component and loads the correct asset
            VisualEffect tempVFXComponent = new VisualEffect();

            switch (type)
            {
                case ElementType.BASE:
                    tempVFXComponent.visualEffectAsset = component._head;
                    break;
                case ElementType.EXTRA:
                    tempVFXComponent.visualEffectAsset = component._trail;
                    break;
                case ElementType.AMBIENCE:
                    tempVFXComponent.visualEffectAsset = component._ambience;
                    break;
                default:
                    Debug.LogWarning("WARNING: Element type was \"NONE\" when trying to select what type of asset to add to the VFX component");
                    break;
            }

            //alters the states on the loaded VFX Asset
            tempVFXComponent.SetFloat("Scale", component._scale);
            tempVFXComponent.SetFloat("Speed", component._speed);
            tempVFXComponent.SetFloat("Density", component._density);

            //adds finalised component to list of components to add to the projectile
            return tempVFXComponent;
        }

        /// <summary>
        /// Creates a new VFX component and updates it with the correct VFX asset and asset stats
        /// </summary>
        /// <param name="component">Entity component to load VFX data from</param>
        /// <param name="type">Type of VFX data to load</param>
        /// <returns>The initalised VFX component</returns>
        private static VisualEffect CreateVFXComponent(SNSWaterComponent component, ElementType type)
        {
            //sets up the VFX component and loads the correct asset
            VisualEffect tempVFXComponent = new VisualEffect();

            switch (type)
            {
                case ElementType.BASE:
                    tempVFXComponent.visualEffectAsset = component._head;
                    break;
                case ElementType.EXTRA:
                    tempVFXComponent.visualEffectAsset = component._trail;
                    break;
                case ElementType.AMBIENCE:
                    tempVFXComponent.visualEffectAsset = component._ambience;
                    break;
                default:
                    Debug.LogWarning("WARNING: Element type was \"NONE\" when trying to select what type of asset to add to the VFX component");
                    break;
            }

            //alters the states on the loaded VFX Asset
            tempVFXComponent.SetFloat("Scale", component._scale);
            tempVFXComponent.SetFloat("Speed", component._speed);
            tempVFXComponent.SetFloat("Density", component._density);

            //adds finalised component to list of components to add to the projectile
            return tempVFXComponent;
        }

        /// <summary>
        /// Creates a new VFX component and updates it with the correct VFX asset and asset stats
        /// </summary>
        /// <param name="component">Entity component to load VFX data from</param>
        /// <param name="type">Type of VFX data to load</param>
        /// <returns>The initalised VFX component</returns>
        private static VisualEffect CreateVFXComponent(SNSEarthComponent component, ElementType type)
        {
            //sets up the VFX component and loads the correct asset
            VisualEffect tempVFXComponent = new VisualEffect();

            switch (type)
            {
                case ElementType.BASE:
                    tempVFXComponent.visualEffectAsset = component._head;
                    break;
                case ElementType.EXTRA:
                    tempVFXComponent.visualEffectAsset = component._trail;
                    break;
                case ElementType.AMBIENCE:
                    tempVFXComponent.visualEffectAsset = component._ambience;
                    break;
                default:
                    Debug.LogWarning("WARNING: Element type was \"NONE\" when trying to select what type of asset to add to the VFX component");
                    break;
            }

            //alters the states on the loaded VFX Asset
            tempVFXComponent.SetFloat("Scale", component._scale);
            tempVFXComponent.SetFloat("Speed", component._speed);
            tempVFXComponent.SetFloat("Density", component._density);

            //adds finalised component to list of components to add to the projectile
            return tempVFXComponent;
        }

        /// <summary>
        /// Creates a new VFX component and updates it with the correct VFX asset and asset stats
        /// </summary>
        /// <param name="component">Entity component to load VFX data from</param>
        /// <param name="type">Type of VFX data to load</param>
        /// <returns>The initalised VFX component</returns>
        private static VisualEffect CreateVFXComponent(SNSLightningComponent component, ElementType type)
        {
            //sets up the VFX component and loads the correct asset
            VisualEffect tempVFXComponent = new VisualEffect();

            switch (type)
            {
                case ElementType.BASE:
                    tempVFXComponent.visualEffectAsset = component._head;
                    break;
                case ElementType.EXTRA:
                    tempVFXComponent.visualEffectAsset = component._trail;
                    break;
                case ElementType.AMBIENCE:
                    tempVFXComponent.visualEffectAsset = component._ambience;
                    break;
                default:
                    Debug.LogWarning("WARNING: Element type was \"NONE\" when trying to select what type of asset to add to the VFX component");
                    break;
            }

            //alters the states on the loaded VFX Asset
            tempVFXComponent.SetFloat("Scale", component._scale);
            tempVFXComponent.SetFloat("Speed", component._speed);
            tempVFXComponent.SetFloat("Amount", component._density); //some weird error with setting the value as 'Density' means it needed to be changed

            //adds finalised component to list of components to add to the projectile
            return tempVFXComponent;
        }



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