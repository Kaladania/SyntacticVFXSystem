using SnSECS;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.VFX;

public enum ControlType
{
    AUTOMATIC,
    MANUAL
}

public class OrbController : OrbAutoController
{
    
    private int _nextComboIndex = 0; //holds the position of the next element to be added
    private List<Elements> _currentCombo = new List<Elements>(); //holds the current combination of elements

    [SerializeField]
    private Image[] _uiIconPositions = new Image[SNSData.MAX_COMBO_LIMIT]; //holds the spawn positions of the icons

    
    [SerializeField]
    private ElementIconMapTemplate _iconData = null; //holds the scriptable object data container for ui icon maps.

    private int[] _uniqueElementCounts; //keeps a count of the number of duplicate elements in a combo

    [SerializeField]
    private DataRecorder.Recorder _dataRecorder = null;

    [SerializeField]
    private int _id = 0;


    public delegate void SNSCombination(List<Elements> elements);
    public static event SNSCombination combinationLoaded;


    /*#if VERSION_SNS
        private EntityArchetype _comboArchedtype1 = EntityManager.CreateArchetype(typeof(SNSElementComponent));
    #endif*/

    void Start()
    {
       
        _uniqueElementCounts = new int[SNSData.NUM_ELEMENTS - 1];

        _uniqueElementCounts[(int)Elements.FIRE] = 0; //number of extra targets a projectile can hit
        _uniqueElementCounts[(int)Elements.WATER] = 0; //number of extra projectiles to spawn
        _uniqueElementCounts[(int)Elements.EARTH] = 0; //density level of AOE
        _uniqueElementCounts[(int)Elements.LIGHTNING] = 0; //level of speed increase

        if (_dataRecorder == null)
        {
            Debug.LogError("ERROR! Data recorder reference is null. Creating a new runtime data recorder");

            _dataRecorder = new DataRecorder.Recorder();
        }

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

        
        if (_nextComboIndex >= SNSData.MAX_COMBO_LIMIT)
        {
            _nextComboIndex = SNSData.MAX_COMBO_LIMIT;
            //LoadCombination(); //Automatically generates a system once all the combination slots have been filled
        }
        else
        {
            _currentCombo.Add(element); //adds the paramter to the combination list

            //Updates the UI to show the icon for the current added element
            _uiIconPositions[_nextComboIndex].sprite = _iconData.GetUIIcon(element);
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
            _uiIconPositions[lastIndex].sprite = _iconData.GetUIIcon(Elements.NONE);
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
        CreateProjectile(GenerateVFX());
#elif VERSION_SNS_PROC

        VisualEffectAsset vfx = GenerateVFX();
        SpawnVFX(vfx);

#else
        SpawnVFX(_vfx);
#endif
        //writes the combo to the data collection file
        //_dataRecorder.WriteComboToFile(_currentCombo);

        //empties combination and resets counters

        combinationLoaded?.Invoke(_currentCombo);
        _nextComboIndex = 0;
        _currentCombo.Clear();
        ClearDuplicateArray();

        //resets icon images and visibility
        foreach (Image icon in _uiIconPositions)
        {
            icon.sprite = _iconData.GetUIIcon(Elements.NONE);
            icon.gameObject.SetActive(false);
        }
    }

    private void ClearDuplicateArray()
    {
        for (int i = 0; i < _uniqueElementCounts.Length; i++)
        {
            _uniqueElementCounts[i] = 0;
        }
    }

#if VERSION_SNS
    /// <summary>
    /// Re
    /// </summary>
    /// <returns></returns>
    private List<VisualEffectAsset> GenerateVFX()
    {
        
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
            _uniqueElementCounts[(int)element.]++;
        }

    }*/

    private void CreateSpell(List<VisualEffectAsset> generatedVFXs)
    {
        
        //loads the SNS VFX and uses it to spawn a projectile used for the spell
        CreateProjectile(generatedVFXs);

        #region [COMMENTED OUT] Code to add spell modifiers (such as double projectiles)
        /*
        
        //records a count of the number of duplicate elements in a combo

        //if there are more than 2 elements in the combination, water modifier gets an extra point
        //improves logic perception because it feels weird to only have 1 projectile when only 1 water element is added
        if (_currentCombo.Count >= 2)
        {
            _uniqueElementCounts[(int)Elements.WATER]++;
        }

        foreach (Elements element in _currentCombo)
        {
            _uniqueElementCounts[(int)element]++;
        }


        
        if (projectile != null)
        {
            ProjectileMovement controller = projectile.GetComponent<ProjectileMovement>();
            int targetCount = 0;
            int projectileSpeed = 0;


            if (controller != null)
            {
                //number of fire elements in combo states how many enemies the projectile can hit before being destroyed
                targetCount = _uniqueElementCounts[(int)Elements.FIRE];
                controller.Targets += targetCount;

                //number of lightning elements in combo states how fast the projectile moves
                projectileSpeed = _uniqueElementCounts[(int)Elements.LIGHTNING];
                controller.Speed += (_projectileSpeedIncrease * projectileSpeed);
            }


            //Determines if a mass amount of projectiles should be spawned for an AOE attack
            float spawnAngle, spawnRadians;
            Vector3 newSpawnPosition = projectile.transform.position;
            int numProjectiles = _uniqueElementCounts[(int)Elements.EARTH];



            //Determines if duplicate extra projectiles need to be spawned
            //number of duplicate water elements in combo states the number of additional projectiles to spawn
            for (int i = 0; i < _uniqueElementCounts[(int)Elements.WATER]; i++)
            {
                newSpawnPosition.x += (_buddyProjectileDistance * (1 * i));

                *//*Vector3 newPosition = new Vector3((projectile.transform.position.x + (_buddyProjectileDistance * (1 * i))), projectile.transform.position.y,
                     projectile.transform.position.z);*//*
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
                for (int j = 0; j < _uniqueElementCounts[(int)Elements.WATER]; j++)
                {
                    newSpawnPosition.x += (_buddyProjectileDistance * (1 * i));

                    *//*Vector3 newPosition = new Vector3((projectile.transform.position.x + (_buddyProjectileDistance * (1 * i))), projectile.transform.position.y,
                         projectile.transform.position.z);*//*
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
        }*/
        #endregion
    }

    /// <summary>
    /// Combines the list of VFX systems to create a PCG VFX
    /// </summary>
    /// <param name="vfxToSpawn">The generated particle system to spawn</param>
    public GameObject CreateProjectile(List<VisualEffectAsset> generatedVFXs)
    {
        GameObject projectile = null;


        if (_spawnPoint != null && generatedVFXs != null)
        {
            Vector3 childSpawnPointPosition = _childProjectile.transform.position;
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
                controller.Direction = transform.right;
            }

            //loops through the rest of the array and adds the child VFX (trial + ambience)
            for (int i = 1; i < generatedVFXs.Count; i++)
            {
                GameObject childObject = Instantiate(_childProjectile, _spawnPoint.position + _childProjectile.transform.position, UnityEngine.Quaternion.identity);
                
                childObject.transform.parent = projectile.transform;
                //childObject.transform.localPosition = new Vector3(childPosition.x, childObject.transform.position.y, childObject.transform.position.z);

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
