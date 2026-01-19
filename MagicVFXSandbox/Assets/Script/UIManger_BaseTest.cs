using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DistractionTypes
{
    COLOUR,
    CHARACTER,
    BOTH,
    MAX_ENUM
}

public class UIManger_BaseTest : MonoBehaviour
{
    [SerializeField]
    List<TextMeshProUGUI> _buttonTexts = null;

    [SerializeField]
    Color _targetColour = Color.white;

    [SerializeField]
    Color _distractorColour = Color.red;

    [SerializeField]
    char _targetChar = 'A';

    [SerializeField]
    char _distractorChar = 'B';

    [SerializeField]
    float _spawnDelaySeconds = 1.0f;

    //const float _targetChance = 0.6f;
    const float _distactorChance = 0.6f;

    int _currentChosenIndex = -1;

    private void Awake()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_buttonTexts == null)
        {
            Debug.LogError("ERROR: Reference to button texts are null");
        }

        //ensures both characters are captialised
        char.ToUpper(_targetChar);
        char.ToUpper(_distractorChar);

        StartCoroutine(SpawnCharacter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnCharacter()
    {
        yield return new WaitForSeconds(_spawnDelaySeconds); //delays the spawn of the next character

        _currentChosenIndex = UnityEngine.Random.Range(0, 2); //chooses a random side to spawn
        int diceRoll = UnityEngine.Random.Range(1, 100) / 100; // /100 turns the roll into a percentage chance
        DistractionTypes type = DistractionTypes.MAX_ENUM;

        if (diceRoll >= _distactorChance)
        {
            type = (DistractionTypes)UnityEngine.Random.Range(0, (int)DistractionTypes.MAX_ENUM); //randomly chooses a distraction type (1 in 3 chance)
            
        }

        Tuple<char, Color> loadedCharacter = GenerateCharacter(type);

        //changes a random button to 'spawn' a character
        _buttonTexts[_currentChosenIndex].text = loadedCharacter.Item1.ToString();
        _buttonTexts[_currentChosenIndex].color = loadedCharacter.Item2;

    }

    public void DespawnCharacter()
    {
        _buttonTexts[_currentChosenIndex].text = "";
        _buttonTexts[_currentChosenIndex ].color = Color.black;

        StartCoroutine(SpawnCharacter());
    }

    /// <summary>
    /// Returns a tuple of info to load for the round's character
    /// </summary>
    /// <param name="type">Type of distraction to add to the loaded character</param>
    /// <returns>The customised character</returns>
    private Tuple<char, Color> GenerateCharacter(DistractionTypes type)
    {
        //Tuple<char, Color> generatedCharacter = Tuple.Create(_targetChar, _targetColour);

        Color chosenColour = _targetColour;
        char chosenCharacter = _targetChar;

        switch (type)
        {
            case DistractionTypes.COLOUR:
                chosenColour = _distractorColour;
                break;
            case DistractionTypes.CHARACTER:
                chosenCharacter = _distractorChar;
                break;
            case DistractionTypes.BOTH:
                chosenColour = _distractorColour;
                chosenCharacter = _distractorChar;
                break;
            default:
                //leaves it with it's defaul values
                break;
        }

        return Tuple.Create(_targetChar, _targetColour);
    }
}
