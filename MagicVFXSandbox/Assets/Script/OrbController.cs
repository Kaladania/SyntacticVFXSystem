using SnSECS;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.VFX;
using static UnityEditor.Rendering.FilterWindow;
using static UnityEngine.Rendering.DebugUI;


public class OrbController : MonoBehaviour
{
    public const int MAX_COMBO_LIMIT = 5; //states the maximum number of elements that can be added to a combination
    public const int NUM_ELEMENTS = 5; //states the maximum number of elements that can be added to a combination
    private int _nextComboIndex = 0; //holds the position of the next element to be added
    private List<Elements> _currentCombo = new List<Elements>(); //holds the current combination of elements

    [SerializeField]
    private Image[] _uiIconPositions = new Image[MAX_COMBO_LIMIT]; //holds the spawn positions of the icons

    private Dictionary<Elements, Sprite> _uiIcons = new Dictionary<Elements, Sprite>();

    [SerializeField]
    private List<Elements> _keys = new List<Elements>();

    [SerializeField]
    private List<Sprite> _values = new List<Sprite>();

    [SerializeField]
    private VisualEffectAsset _vfx = new VisualEffectAsset(); //TURN INTO A LIST/ECS SYSTEM. Holds the templated VFX systems

    [SerializeField]
    private Transform _spawnPoint; //holds the spawn point of the VFX projectiles

    [SerializeField]
    private GameObject _projectile; //holds a prefab for a basic projectile

    [SerializeField]
    private GameObject _childProjectile; //holds a prefab for a basic projectile

    [SerializeField]
    private GameObject _turret;

    [SerializeField]
    private GameObject _turretSpawnPoint;

    //projectile modifiers
    [SerializeField]
    private float _buddyProjectileDistance = 1f; //states how far apart duplicate projectiles should be spawned (for water modifier)

    //projectile modifiers
    [SerializeField]
    private float _projectileSpeedIncrease = 0.25f; //states how far apart duplicate projectiles should be spawned (for water modifier)

    [SerializeField]
    private int _AOEBaseDensity = 1; //states how many projectiles should be spawned in the circular AOE attack

    [SerializeField]
    private int _AOEDensityIncrease = 1; //states how many additional projectiles should be spawned in the circular AOE attack

    [SerializeField]
    private float _AOESpawnRadius = 1f; //spawn radius of AOE attack
    /*private float _AOELevel = 0; //altered by Earth
    private float _projectileTargets = 1; //altered by Fire*/

    private int[] _duplicateElementCounts; //keeps a count of the number of duplicate elements in a combo


    /*#if VERSION_SNS
        private EntityArchetype _comboArchedtype1 = EntityManager.CreateArchetype(typeof(SNSElementComponent));
    #endif*/

    void Start()
    {
        //Create Icon Dictionary
        _uiIcons.Clear();

        if (_keys.Count != _values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values on application start. Make sure that both key and value types are serializable " +
                "and have the same number of elements."));

        for (int i = 0; i < _keys.Count; i++)
            _uiIcons.Add(_keys[i], _values[i]);

        _duplicateElementCounts = new int[NUM_ELEMENTS];

        _duplicateElementCounts[(int)Elements.FIRE] = 0; //number of extra targets a projectile can hit
        _duplicateElementCounts[(int)Elements.WATER] = 0; //number of extra projectiles to spawn
        _duplicateElementCounts[(int)Elements.EARTH] = 0; //density level of AOE
        _duplicateElementCounts[(int)Elements.LIGHTNING] = 0; //level of speed increase



        //Create SNS Element Entity Archetype

#if VERSION_SNS
        //VisualEffectAsset vfx = GenerateVFX();
        //SpawnVFX(GenerateVFX());
#endif
    }

    // Update is called once per frame
    void Update()
    {

        //Adds elements to the combination list depending on the pressed key

        if (Keyboard.current.qKey.wasPressedThisFrame) //fire
        {
            AddElementToCombination(Elements.FIRE);
        }

        if (Keyboard.current.wKey.wasPressedThisFrame) //water
        {
            AddElementToCombination(Elements.WATER);
        }

        if (Keyboard.current.eKey.wasPressedThisFrame) //earth
        {
            AddElementToCombination(Elements.EARTH);
        }

        if (Keyboard.current.rKey.wasPressedThisFrame) //lightning
        {
            AddElementToCombination(Elements.LIGHTNING);
        }

        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            RemoveLastElementFromCombination();
        }

        //Manually generates a new particle system if 'Enter' is pressed or the Mouse button is clicked (and there is a valid combo)
        if ((Keyboard.current.enterKey.wasPressedThisFrame || Input.GetMouseButtonDown(0)) && _currentCombo.Count > 0)
        {
            LoadCombination();
        }



    }

    /// <summary>
    /// Adds an 'element' (system parameter) to the combination list
    /// </summary>
    /// <param name="element">An enum detailing the type of 'element' paramter to add</param>
    void AddElementToCombination(Elements element)
    {

        
        if (_nextComboIndex >= MAX_COMBO_LIMIT)
        {
            _nextComboIndex = MAX_COMBO_LIMIT;
            //LoadCombination(); //Automatically generates a system once all the combination slots have been filled
        }
        else
        {
            _currentCombo.Add(element); //adds the paramter to the combination list

            //Updates the UI to show the icon for the current added element
            _uiIconPositions[_nextComboIndex].sprite = _uiIcons[element];
            _uiIconPositions[_nextComboIndex].gameObject.SetActive(true);

            _nextComboIndex++;
        }
            
/*
        //Automatically generates a system once all the combination slots have been filled
        if (_nextComboIndex >= MAX_COMBO_LIMIT)
        {
            LoadCombination();
        }*/
    }

    void RemoveLastElementFromCombination()
    {
        if (_nextComboIndex <= 0)
        {
            _nextComboIndex = 0;
            //LoadCombination();
        }
        else
        {
            int lastIndex = _currentCombo.Count - 1;
            _currentCombo.RemoveAt(lastIndex); //removes the last element in the list

            //Updates the UI to hide the icon for the last added element
            _uiIconPositions[lastIndex].sprite = _uiIcons[Elements.NONE];
            _uiIconPositions[lastIndex].gameObject.SetActive(false);

            _nextComboIndex--;
        }
    }

    /// <summary>
    /// Generates a Combination (POSSIBLY REPLACE WITH ECS SYSTEM)
    /// </summary>
    void LoadCombination()
    {
        Debug.Log("Combination Loaded");

#if VERSION_SNS
        CreateSpell(GenerateVFX());
#elif VERSION_SNS_PROC

        VisualEffectAsset vfx = GenerateVFX();
        SpawnVFX(vfx);

#else
        SpawnVFX(_vfx);
#endif

        //empties combination and resets counters
        _nextComboIndex = 0;
        _currentCombo.Clear();

        //resets icon images and visibility
        foreach (Image icon in _uiIconPositions)
        {
            icon.sprite = _uiIcons[Elements.NONE];
            icon.gameObject.SetActive(false);
        }
    }

#if VERSION_SNS
    /// <summary>
    /// Re
    /// </summary>
    /// <returns></returns>
    private List<VisualEffectAsset> GenerateVFX()
    {
        //records a count of the number of duplicate elements in a combo
        foreach (Elements element in _currentCombo)
        {
            _duplicateElementCounts[(int)element]++;
        }

        //Create an Entity with a correct amount (and type) of element components
        Entity entity = SnSLoadElementsSystem.LoadElement(_currentCombo);
        //GameObject gameObject = Instantiate(_projectile, _spawnPoint.position, UnityEngine.Quaternion.identity);
        

        // VisualEffectAsset generatedVFX = Generate effect (generate effect and returns the final result)
        //above function was written to return an entity. So function call should be SNSGenerateEffect.GenerateEffect(SnSLoadElementsSystem.LoadElement(_currentCombo))
        //the entity returned from 'load element' gets passed into 'generate effect'
        return SnSGenerateEffectSystem.GenerateSnS(entity); ; //returns the generated effect
    }

   /* private void RecordDuplicates()
    {
        foreach (Elements element in _currentCombo)
        {
            _duplicateElementCounts[(int)element.]++;
        }

    }*/

    private void CreateSpell(List<VisualEffectAsset> generatedVFXs)
    {
        //loads the SNS VFX and uses it to spawn a projectile used for the spell
        GameObject projectile = CreateProjectile(generatedVFXs);

        if (projectile != null)
        {
            ProjectileMovement controller = projectile.GetComponent<ProjectileMovement>();
            int targetCount = 1;
            int projectileSpeed = 1;


            if (controller != null)
            {
                //number of fire elements in combo states how many enemies the projectile can hit before being destroyed
                targetCount = _duplicateElementCounts[(int)Elements.FIRE];
                controller.Targets += targetCount;

                //number of lightning elements in combo states how fast the projectile moves
                projectileSpeed = _duplicateElementCounts[(int)Elements.LIGHTNING];
                controller.Speed += (_projectileSpeedIncrease * projectileSpeed);
            }


            //Determines if a mass amount of projectiles should be spawned for an AOE attack
            float spawnAngle, spawnRadians;
            Vector3 newSpawnPosition = projectile.transform.position;
            int numProjectiles = _duplicateElementCounts[(int)Elements.EARTH];



            //Determines if duplicate extra projectiles need to be spawned
            //number of duplicate water elements in combo states the number of additional projectiles to spawn
            for (int i = 0; i < _duplicateElementCounts[(int)Elements.WATER]; i++)
            {
                newSpawnPosition.x += (_buddyProjectileDistance * (1 * i));

                /*Vector3 newPosition = new Vector3((projectile.transform.position.x + (_buddyProjectileDistance * (1 * i))), projectile.transform.position.y,
                     projectile.transform.position.z);*/
                GameObject childProjectile = Instantiate(projectile, newSpawnPosition, Quaternion.identity);

                controller = childProjectile.GetComponent<ProjectileMovement>();

                //updates modifers for the child projectiles
                if (controller != null)
                {
                    controller.Targets += targetCount;
                    controller.Speed += (_projectileSpeedIncrease * projectileSpeed);
                }



            }



            //Spawn extra projectiles if the spell is becoming an AOE attack

            for (int i = 0; i < numProjectiles; i++)
            {
                //calculates the angle of the spawn and converts it to radians
                spawnAngle = i * (360 / numProjectiles);
                spawnRadians = spawnAngle * Mathf.Deg2Rad;

                //calculates
                newSpawnPosition.x = transform.position.x + (_AOESpawnRadius * Mathf.Cos(spawnRadians));
                newSpawnPosition.z = transform.position.z + (_AOESpawnRadius * Mathf.Sin(spawnRadians));

                //Determines if duplicate extra projectiles need to be spawned
                //number of duplicate water elements in combo states the number of additional projectiles to spawn
                for (int j = 0; j < _duplicateElementCounts[(int)Elements.WATER]; j++)
                {
                    newSpawnPosition.x += (_buddyProjectileDistance * (1 * i));

                    /*Vector3 newPosition = new Vector3((projectile.transform.position.x + (_buddyProjectileDistance * (1 * i))), projectile.transform.position.y,
                         projectile.transform.position.z);*/
                    GameObject childProjectile = Instantiate(projectile, newSpawnPosition, Quaternion.identity);

                    controller = childProjectile.GetComponent<ProjectileMovement>();

                    //updates modifers for the child projectiles
                    if (controller != null)
                    {
                        controller.Targets += targetCount;
                        controller.Speed += (_projectileSpeedIncrease * projectileSpeed);
                    }

                    

                }
            }
        }
    }

    /// <summary>
    /// Spawns the particle system in-game
    /// </summary>
    /// <param name="vfxToSpawn">The generated particle system to spawn</param>
    private GameObject CreateProjectile(List<VisualEffectAsset> generatedVFXs)
    {
        GameObject projectile = null;

        if (_spawnPoint != null && generatedVFXs != null)
        {
            projectile = Instantiate(_projectile, _spawnPoint.position, UnityEngine.Quaternion.identity);

            VisualEffect baseVfx = projectile.GetComponent<VisualEffect>();

            //Adds the base VFX for the head of the particle
           if (baseVfx == null)
           {
               Debug.LogError("WARNING! Failed to find Visual Effect Component");
           }
           else
           {
               //Adds the particle system to the loaded projectile prefab
               baseVfx.visualEffectAsset = generatedVFXs[0];
           }

            //rotate the projectile to face the turret facing direction
            projectile.transform.rotation = Quaternion.LookRotation(transform.forward);
            
            ProjectileMovement controller = projectile.GetComponent<ProjectileMovement>();

            if (controller != null)
            {
                controller.Direction = transform.forward;
            }

            //loops through the rest of the array and adds the child VFX (trial + ambience)
            for (int i = 1; i < generatedVFXs.Count; i++)
            {
                GameObject childObject = Instantiate(_childProjectile, _spawnPoint.position, UnityEngine.Quaternion.identity);
                childObject.transform.parent = projectile.transform;

                VisualEffect childVFX = childObject.GetComponent<VisualEffect>();

                if (childVFX == null)
                {
                    Debug.LogError("WARNING! Failed to find Visual Effect Component");
                }
                else
                {
                    //Adds the particle system to the loaded projectile prefab
                    childVFX.visualEffectAsset = generatedVFXs[i];
                }
            }

        }

        return projectile;
    }

#endif

#if VERSION_SNS_PROC
    void SpawnVFX(Entity vfxToSpawn)
    {
        GameObject gameObject;

        if (_spawnPoint != null && _vfx != null)
        {
            gameObject = Instantiate(_projectile, _spawnPoint.position, UnityEngine.Quaternion.identity);

            VisualEffect vfx = gameObject.GetComponent<VisualEffect>();

            if (vfx == null)
            {
                Debug.LogError("WARNING! Failed to find Visual Effect Component");
            }
            else
            {
                //Adds the particle system to the loaded projectile prefab
                vfx.visualEffectAsset = vfxToSpawn;
            }

        }
    }
#endif

}
