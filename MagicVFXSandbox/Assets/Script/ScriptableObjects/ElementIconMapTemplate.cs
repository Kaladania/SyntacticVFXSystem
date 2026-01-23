using SnSECS;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementIconMap", menuName = "Scriptable Objects/ElementIconMap")]
public class ElementIconMapTemplate : ScriptableObject
{
    private Dictionary<Elements, Sprite> _uiIcons = new Dictionary<Elements, Sprite>();

    [Tooltip("Index of Element (place in same order as ui icons)")]
    [SerializeField]
    public List<Elements> _keys = new List<Elements>();

    [Tooltip("Index of UI Icon (place in same order as enum elements")]
    [SerializeField]
    public List<Sprite> _values = new List<Sprite>();

    //immediately populates the dictionary for use
    private void OnEnable()
    {
        if (_keys.Count != _values.Count)
            throw new System.Exception(string.Format("INVALID MAP LAYOUT. Only {0} keys and {1} values given via inspector. " +
                "Ensure both keys and values have the same number of elements"));

        for (int i = 0; i < _keys.Count; i++)
            _uiIcons.Add(_keys[i], _values[i]);
    }

    /// <summary>
    /// Gets the ui icon mapped to the given element
    /// </summary>
    /// <param name="element">The mapped UI icon to load</param>
    /// <returns>The mappped UI Icon</returns>
    public Sprite GetUIIcon(Elements element) { return _uiIcons[element]; }

}
