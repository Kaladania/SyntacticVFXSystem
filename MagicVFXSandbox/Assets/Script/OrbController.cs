using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.VFX;
using static UnityEngine.Rendering.DebugUI;



public class OrbController : MonoBehaviour
{
    enum Elements
    {
        FIRE,
        EARTH,
        WATER,
        LIGHTNING,
        NONE
    }
    public const int MAX_COMBO_LIMIT = 5; //states the maximum number of elements that can be added to a combination
    public const int NUM_ELEMENTS = 5; //states the maximum number of elements that can be added to a combination
    private int _nextComboIndex = 0; //holds the position of the next element to be added
    private List<Elements> _currentCombo = new List<Elements>(); //holds the current combination of elements

    [SerializeField]
    private Image[] _uiIconPositions = new Image[MAX_COMBO_LIMIT]; //holds the spawn positions of the icons

    private Dictionary<Elements, Sprite> _uiIcons = new Dictionary<Elements, Sprite>();
    //private Sprite[] _uiIcons = new Sprite[NUM_ELEMENTS];

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
    void Start()
    {
        //GameObject object = new GameObject();
        //_projectile = new GameObject();
        _uiIcons.Clear();

        if (_keys.Count != _values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values on application start. Make sure that both key and value types are serializable " +
                "and have the same number of elements."));

        for (int i = 0; i < _keys.Count; i++)
            _uiIcons.Add(_keys[i], _values[i]);
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

        //Manually generates a new particle system if 'Enter' is pressed
        if (Keyboard.current.enterKey.wasPressedThisFrame && _currentCombo.Count > 0)
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
        _currentCombo.Add(element); //adds the paramter to the combination list

        //Updates the UI to show the icon for the current added element
        _uiIconPositions[_nextComboIndex].sprite = _uiIcons[element];
        _uiIconPositions[_nextComboIndex].gameObject.SetActive(true);

        _nextComboIndex++;

        //Automatically generates a system once all the combination slots have been filled
        if (_nextComboIndex >= MAX_COMBO_LIMIT)
        {
            LoadCombination();
        }
    }

    /// <summary>
    /// Generates a Combination (POSSIBLY REPLACE WITH ECS SYSTEM)
    /// </summary>
    void LoadCombination()
    {
        Debug.Log("Combination Loaded");

        SpawnVFX(_vfx);

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

    /// <summary>
    /// Spawns the particle system in-game
    /// </summary>
    /// <param name="vfxToSpawn">The generated particle system to spawn</param>
    void SpawnVFX(VisualEffectAsset vfxToSpawn)
    {
        GameObject gameObject;

        if (_spawnPoint != null && _vfx != null)
        {
            gameObject = Instantiate(_projectile,  _spawnPoint.position, UnityEngine.Quaternion.identity);

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

}
