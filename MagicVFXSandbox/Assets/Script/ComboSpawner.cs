using SnSECS;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.VFX;

public class ComboSpawner : MonoBehaviour
{
    [SerializeField]
    private float _spawnFrequency = 1f; //spawn countdown duration

    [SerializeField]
    private Transform _spawnPoint; //transform of spawn point

    [SerializeField]
    private OrbAutoController _orbController; //reference to the current orb controller component used ingame

    List<VisualEffectAsset> _currentSnSVFX; //holds the current generated VFX

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_orbController == null)
        {
            Debug.LogError("ERROR! Orb Controller reference is null. Creating a new runtime orb controller");

            _orbController = new OrbController();
        }

        if (_spawnPoint == null)
        {
            _spawnPoint = transform;
        }

        //TEMP TEST FUNCTIONS
        //Automatically spawns a loaded projectile on start.
        //Actually needs to be an event instagator instead (See Plan Outline)
        LoadCombination(new List<Elements>(){ Elements.FIRE, Elements.WATER, Elements.LIGHTNING });
        StartCoroutine(SpawnProjectile(_spawnFrequency, _currentSnSVFX));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Generates a Combination (POSSIBLY REPLACE WITH ECS SYSTEM)
    /// </summary>
    void LoadCombination(List<Elements> combo)
    {
        //Loads, Generates and stores the VFX blueprint for the currently tested projectile
        _currentSnSVFX = SnSGenerateEffectSystem.GenerateSnS(SnSLoadElementsSystem.LoadElement(combo));
        //_orbController.CreateProjectile(_currentSnSVFX);
    }

    void StartSpawner()
    {
        StartCoroutine(SpawnProjectile(_spawnFrequency, _currentSnSVFX));
    }

    private IEnumerator SpawnProjectile(float coundownDuration, List<VisualEffectAsset> vfx)
    {
        yield return new WaitForSeconds(coundownDuration); //returns a reference to the spawned enemy after a specified amount of time

        _orbController.CreateProjectile(_currentSnSVFX);
        StartCoroutine(SpawnProjectile(coundownDuration, vfx));
    }
}
