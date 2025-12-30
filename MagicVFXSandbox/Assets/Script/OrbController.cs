using SnSECS;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
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
        if ((Keyboard.current.enterKey.wasPressedThisFrame || Input.GetMouseButton(0)) && _currentCombo.Count > 0)
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
        SpawnVFX(GenerateVFX());
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

        //Create an Entity with a correct amount (and type) of element components
        Entity entity = SnSLoadElementsSystem.LoadElement(_currentCombo);
        //GameObject gameObject = Instantiate(_projectile, _spawnPoint.position, UnityEngine.Quaternion.identity);
        

        // VisualEffectAsset generatedVFX = Generate effect (generate effect and returns the final result)
        //above function was written to return an entity. So function call should be SNSGenerateEffect.GenerateEffect(SnSLoadElementsSystem.LoadElement(_currentCombo))
        //the entity returned from 'load element' gets passed into 'generate effect'
        return SnSGenerateEffectSystem.GenerateSnS(entity); ; //returns the generated effect
    }


    /// <summary>
    /// Spawns the particle system in-game
    /// </summary>
    /// <param name="vfxToSpawn">The generated particle system to spawn</param>
    void SpawnVFX(List<VisualEffectAsset> generatedVFXs)
    {
        GameObject gameObject;

        if (_spawnPoint != null && generatedVFXs != null)
        {
            gameObject = Instantiate(_projectile, _spawnPoint.position, UnityEngine.Quaternion.identity);

            VisualEffect baseVfx = gameObject.GetComponent<VisualEffect>();

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

           //loops through the rest of the array and adds the child VFX (trial + ambience)
            for (int i = 1; i < generatedVFXs.Count; i++)
            {
                GameObject childObject = Instantiate(_childProjectile, _spawnPoint.position, UnityEngine.Quaternion.identity);
                childObject.transform.parent = gameObject.transform;

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

           /* foreach (VisualEffectAsset asset in generatedVFXs)
            {
                //creates a vfx component for each asset so they can all be spawned
                vfxComponent = gameObject.AddComponent<VisualEffect>();
                vfxComponent.visualEffectAsset = asset;
            }*/

            /*VisualEffect baseVfx = gameObject.GetComponent<VisualEffect>();

            if (baseVfx == null)
            {
                Debug.LogError("WARNING! Failed to find Visual Effect Component");
            }
            else
            {
                //Adds the particle system to the loaded projectile prefab
                baseVfx.visualEffectAsset = vfxToSpawn;
            }*/
   


        }
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
