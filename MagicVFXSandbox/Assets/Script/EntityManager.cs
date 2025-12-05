using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.tvOS;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Rendering.DebugUI;

public class EntityManager: MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Creates a new syntactic particle entity
    void CreateEntity()
    {

    }

    /// <summary>
    /// Creates a copy of an existing entity (including its components)
    /// </summary>
    void Instantiate()
    {

    }

    /// <summary>
    /// Destroys a given entity
    /// </summary>
    void DestroyEntity()
    {

    }

    /// <summary>
    /// [TEMPLATE] Adds a component to a given entity
    /// </summary>
    /// <typeparam name="T">Component Type to add</typeparam>
    void AddComponent<T>()
    {

    }

    /// <summary>
    /// [TEMPLATE] Removes a component from a given entity
    /// </summary>
    /// <typeparam name="T">Component type to remove</typeparam>
    void RemoveComponent<T>()
    {

    }

    /// <summary>
    /// Returns a bool stating if a given entity has at least 1 component of a specified type
    /// </summary>
    /// <typeparam name="T">Component type to search for</typeparam>
    /// <returns>Bool stating if a component of the requested type is attached to an entity</returns>
    bool HasComponent<T>()
    {
        return true; //if at least 1 component has been found to match the specified type
    }

    /// <summary>
    /// Returns the value of the first component that matched the specifed type
    /// </summary>
    /// <typeparam name="T">Component type to retrive</typeparam>
    /// <returns>Value of the 1st component found that matches the specifed type</returns>
    void GetComponent<T>()
    {
        //MAKE SURE IT RETURNS A VALUE REFERENCE TO OF A COMPONENT TYPE
    }

    /// <summary>
    /// Overwrites the value of the 1st existing component that matches the specified type
    /// </summary>
    /// <typeparam name="T">Component type to overwrite</typeparam>
    void SetComponent<T>()
    {

    }
}
