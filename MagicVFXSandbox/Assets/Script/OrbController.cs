using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
//using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
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

    [SerializeField]
    private Dictionary<Elements, Sprite> _uiIcons = new Dictionary<Elements, Sprite>();
    //private Sprite[] _uiIcons = new Sprite[NUM_ELEMENTS];

    [SerializeField]
    private List<Elements> _keys = new List<Elements>();

    [SerializeField]
    private List<Sprite> _values = new List<Sprite>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
/*    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<Elements, Sprite> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
*/
    void Start()
    {
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
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            AddElementToCombination(Elements.FIRE);
        }

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            AddElementToCombination(Elements.WATER);
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            AddElementToCombination(Elements.EARTH);
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            AddElementToCombination(Elements.LIGHTNING);
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            LoadCombination();
        }

    }


    void AddElementToCombination(Elements element)
    {
        _currentCombo.Add(element);
        _uiIconPositions[_nextComboIndex].sprite = _uiIcons[element];
        _uiIconPositions[_nextComboIndex].gameObject.SetActive(true);

        _nextComboIndex++;

        if (_nextComboIndex >= MAX_COMBO_LIMIT)
        {
            LoadCombination();
        }
    }

    void LoadCombination()
    {
        Debug.Log("Combination Loaded");

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

}
