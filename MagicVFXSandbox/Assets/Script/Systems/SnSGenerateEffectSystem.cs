using System.Collections.Generic;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;
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
         
        public static GameObject GenerateSnS(Entity entity)
        {
            //initalise variables
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //List<VisualEffect> generatedVFXs = new List<VisualEffect>();

            SNSFireComponent tempFireComponent;
            SNSWaterComponent tempWaterComponent;
            SNSLightningComponent tempLightningComponent;
            SNSEarthComponent tempEarthComponent;

            VisualEffect tempVFXComponent;

            GameObject VFXObject = new GameObject();
            VFXObject.name = "SNS VFX Object";

            GameObject tempObject = new GameObject();

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
                        
                        tempObject = CreateVFXObject(tempFireComponent, type); //loads a gameobject with a vfx component holding a customised VFX asset

                        if (i == 1)
                        {
                            VFXObject = tempObject; //sets the object as the main parent if it's the base element
                        }
                        else
                        {
                            tempObject.transform.parent = VFXObject.transform; //attached the gameobject to the base vfx object parent as a child
                        }
                           

                    }
                    
                }
                else if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSWaterComponent>())
                {
                    tempWaterComponent = entityManager.GetSharedComponentManaged<SNSWaterComponent>(entity);

                    foreach (ElementType type in tempWaterComponent._types)
                    {
                        tempObject = CreateVFXObject(tempWaterComponent, type); //loads a gameobject with a vfx component holding a customised VFX asset

                        if (i == 1)
                        {
                            VFXObject = tempObject; //sets the object as the main parent if it's the base element
                        }
                        else
                        {
                            tempObject.transform.parent = VFXObject.transform; //attached the gameobject to the base vfx object parent as a child
                        }
                    }

                }
                else if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSEarthComponent>())
                {
                    tempEarthComponent = entityManager.GetSharedComponentManaged<SNSEarthComponent>(entity);

                    foreach (ElementType type in tempEarthComponent._types)
                    {
                        tempObject = CreateVFXObject(tempEarthComponent, type); //loads a gameobject with a vfx component holding a customised VFX asset

                        if (i == 1)
                        {
                            VFXObject = tempObject; //sets the object as the main parent if it's the base element
                        }
                        else
                        {
                            tempObject.transform.parent = VFXObject.transform; //attached the gameobject to the base vfx object parent as a child
                        }

                    }

                }
                else if (elementArray[i].TypeIndex == TypeManager.GetTypeIndex<SNSLightningComponent>())
                {
                    tempLightningComponent = entityManager.GetSharedComponentManaged<SNSLightningComponent>(entity);

                    foreach (ElementType type in tempLightningComponent._types)
                    {
                        tempObject = CreateVFXObject(tempLightningComponent, type); //loads a gameobject with a vfx component holding a customised VFX asset

                        if (i == 1)
                        {
                            VFXObject = tempObject; //sets the object as the main parent if it's the base element
                        }
                        else
                        {
                            tempObject.transform.parent = VFXObject.transform; //attached the gameobject to the base vfx object parent as a child
                        }
                    }

                }

                //if this is the first component to be loaded, replace the empty parent with the child
                //hierachy is now: PARENT(Base) -> CHILDREN (tails and ambience) instead of PARENT(EMPTY) -> CHILDREN (Base, tails and ambience)
                /*if (i == 1)
                {
                    GameObject childObject = VFXObject.transform.GetChild(0).gameObject;
                    childObject.transform.SetParent(null);
                    VFXObject = childObject;
                }*/

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
            return VFXObject;
        }

        /// <summary>
        /// Creates a new VFX component and updates it with the correct VFX asset and asset stats
        /// </summary>
        /// <param name="component">Entity component to load VFX data from</param>
        /// <param name="type">Type of VFX data to load</param>
        /// <returns>A game object initalised VFX component</returns>
        private static GameObject CreateVFXObject(SNSFireComponent component, ElementType type)
        {
            GameObject gameObject = new GameObject();
            //sets up the VFX component and loads the correct asset
            VisualEffect vfxComponent = gameObject.AddComponent<VisualEffect>();

            if (vfxComponent != null)
            {
                switch (type)
                {
                    case ElementType.BASE:
                        vfxComponent.visualEffectAsset = component._head;
                        break;
                    case ElementType.EXTRA:
                        vfxComponent.visualEffectAsset = component._trail;
                        break;
                    case ElementType.AMBIENCE:
                        vfxComponent.visualEffectAsset = component._ambience;
                        break;
                    default:
                        Debug.LogWarning("WARNING: Element type was \"NONE\" when trying to select what type of asset to add to the VFX component");
                        break;
                }

                //alters the states on the loaded VFX Asset
                List<VFXExposedProperty> exposedProperties = new List<VFXExposedProperty>();
                vfxComponent.visualEffectAsset.GetExposedProperties(exposedProperties);

                try
                {
                    vfxComponent.SetFloat("Scale", component._scale);
                    vfxComponent.SetFloat("Speed", component._speed);
                    vfxComponent.SetFloat("Density", component._density); //some weird error with setting the value as 'Density' means it needed to be changed
                }
                catch (System.Exception e)
                {
                    throw e;
                }
            }
            else
            {
                gameObject = null; //nullifies the gameobject to indicate that something has gone wrong
                Debug.LogWarning("WARNING: Unable to generate an SNS VFX component. CreateVFXObject() was unable to add a VFX component to the new game object");
            }

            return gameObject;
        }

        /// <summary>
        /// Creates a new VFX component and updates it with the correct VFX asset and asset stats
        /// </summary>
        /// <param name="component">Entity component to load VFX data from</param>
        /// <param name="type">Type of VFX data to load</param>
        /// <returns>A game object initalised VFX component</returns>
        private static GameObject CreateVFXObject(SNSWaterComponent component, ElementType type)
        {
            GameObject gameObject = new GameObject();
            //sets up the VFX component and loads the correct asset
            VisualEffect vfxComponent = gameObject.AddComponent<VisualEffect>();

            if (vfxComponent != null)
            {
                switch (type)
                {
                    case ElementType.BASE:
                        vfxComponent.visualEffectAsset = component._head;
                        break;
                    case ElementType.EXTRA:
                        vfxComponent.visualEffectAsset = component._trail;
                        break;
                    case ElementType.AMBIENCE:
                        vfxComponent.visualEffectAsset = component._ambience;
                        break;
                    default:
                        Debug.LogWarning("WARNING: Element type was \"NONE\" when trying to select what type of asset to add to the VFX component");
                        break;
                }

                //alters the states on the loaded VFX Asset
                vfxComponent.SetFloat("Scale", component._scale);
                vfxComponent.SetFloat("Speed", component._speed);
                vfxComponent.SetFloat("Density", component._density); //some weird error with setting the value as 'Density' means it needed to be changed

            }
            else
            {
                gameObject = null; //nullifies the gameobject to indicate that something has gone wrong
                Debug.LogWarning("WARNING: Unable to generate an SNS VFX component. CreateVFXObject() was unable to add a VFX component to the new game object");
            }

            return gameObject;
        }

        /// <summary>
        /// Creates a new VFX component and updates it with the correct VFX asset and asset stats
        /// </summary>
        /// <param name="component">Entity component to load VFX data from</param>
        /// <param name="type">Type of VFX data to load</param>
        /// <returns>A game object initalised VFX component</returns>
        private static GameObject CreateVFXObject(SNSEarthComponent component, ElementType type)
        {
            GameObject gameObject = new GameObject();
            //sets up the VFX component and loads the correct asset
            VisualEffect vfxComponent = gameObject.AddComponent<VisualEffect>();

            if (vfxComponent != null)
            {
                switch (type)
                {
                    case ElementType.BASE:
                        vfxComponent.visualEffectAsset = component._head;
                        break;
                    case ElementType.EXTRA:
                        vfxComponent.visualEffectAsset = component._trail;
                        break;
                    case ElementType.AMBIENCE:
                        vfxComponent.visualEffectAsset = component._ambience;
                        break;
                    default:
                        Debug.LogWarning("WARNING: Element type was \"NONE\" when trying to select what type of asset to add to the VFX component");
                        break;
                }

                //alters the states on the loaded VFX Asset
                vfxComponent.SetFloat("Scale", component._scale);
                vfxComponent.SetFloat("Speed", component._speed);
                vfxComponent.SetFloat("Density", component._density); //some weird error with setting the value as 'Density' means it needed to be changed

            }
            else
            {
                gameObject = null; //nullifies the gameobject to indicate that something has gone wrong
                Debug.LogWarning("WARNING: Unable to generate an SNS VFX component. CreateVFXObject() was unable to add a VFX component to the new game object");
            }

            return gameObject;
        }

        /// <summary>
        /// Creates a new VFX component and updates it with the correct VFX asset and asset stats
        /// </summary>
        /// <param name="component">Entity component to load VFX data from</param>
        /// <param name="type">Type of VFX data to load</param>
        /// <returns>A game object initalised VFX component</returns>
        private static GameObject CreateVFXObject(SNSLightningComponent component, ElementType type)
        {
            GameObject gameObject = new GameObject();
            //sets up the VFX component and loads the correct asset
            VisualEffect vfxComponent = gameObject.AddComponent<VisualEffect>();

            if (vfxComponent != null)
            {
                switch (type)
                {
                    case ElementType.BASE:
                        vfxComponent.visualEffectAsset = component._head;
                        break;
                    case ElementType.EXTRA:
                        vfxComponent.visualEffectAsset = component._trail;
                        break;
                    case ElementType.AMBIENCE:
                        vfxComponent.visualEffectAsset = component._ambience;
                        break;
                    default:
                        Debug.LogWarning("WARNING: Element type was \"NONE\" when trying to select what type of asset to add to the VFX component");
                        break;
                }

                //alters the states on the loaded VFX Asset
                vfxComponent.SetFloat("Scale", component._scale);
                vfxComponent.SetFloat("Speed", component._speed);
                vfxComponent.SetFloat("Amount", component._density); //some weird error with setting the value as 'Density' means it needed to be changed

            }
            else
            {
                gameObject = null; //nullifies the gameobject to indicate that something has gone wrong
                Debug.LogWarning("WARNING: Unable to generate an SNS VFX component. CreateVFXObject() was unable to add a VFX component to the new game object");
            }

                return gameObject;
        }

        //private static GameObject CreateGameObject(List<VisualEffect>)



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