using QuizManager;
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

    [SerializeField]
    Stopwatch _stopwatch = null;

    [SerializeField]
    GameObject _creditsPanel = null;

    [SerializeField]
    Button _beginButton = null;

    [SerializeField]
    GameObject _instructionPanel = null;


    [SerializeField]
    private ReactionTimeContainerTemplate _reactionTimeData = null;

    public delegate void InputRecieved();
    public static event InputRecieved mouseButtonPressed;


    //const float _targetChance = 0.6f;
    const float _distactorChance = 0.6f;

    int _currentChosenIndex = -1;

    Coroutine _currentCoroutine = null;
    bool _characterSpawned = false;

    private void Awake()
    {
        Stopwatch.countdownFinished += AttemptSpawn;
        TestManager_Base.shutDownWorkers += CleanUp;
        //Stopwatch.stopwatchPaused += AttemptSpawn;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_buttonTexts == null)
        {
            Debug.LogError("ERROR: Reference to button texts are null");
        }

        if (_stopwatch == null)
        {
            Debug.LogError("ERROR: Reference to stopwatch is null");
        }

        //ensures both characters are captialised
        char.ToUpper(_targetChar);
        char.ToUpper(_distractorChar);

       
        //AttemptSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if ( _characterSpawned)
        {

            if (Input.GetMouseButtonDown(0)) //left mouse button
            {
                DespawnCharacter(0);
            }
            else if (Input.GetMouseButtonDown(2)) //middle mouse button
            {
                DespawnCharacter(1);
            }
            else if (Input.GetMouseButtonDown(1)) //right mouse button
            {
                DespawnCharacter(2);
            }
        }
    }

    public void StartTest()
    {
        _beginButton.gameObject.SetActive(false);
        _instructionPanel.SetActive(false);
        _stopwatch.StartCountdown();
    }

    void AttemptSpawn()
    {
        if (_currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(SpawnCharacter());
        }
    }

    private IEnumerator SpawnCharacter()
    {
        yield return new WaitForSeconds(_spawnDelaySeconds); //delays the spawn of the next character

        _currentChosenIndex = UnityEngine.Random.Range(0, 3); //chooses a random side to spawn
        float diceRoll = UnityEngine.Random.Range(1, 100) / 100.0f; // /100 turns the roll into a percentage chance
        DistractionTypes type = DistractionTypes.MAX_ENUM;

        if (diceRoll <= _distactorChance)
        {
            type = (DistractionTypes)UnityEngine.Random.Range(0, (int)DistractionTypes.MAX_ENUM); //randomly chooses a distraction type (1 in 3 chance)
            
        }

        Tuple<char, Color> loadedCharacter = GenerateCharacter(type);

        //changes a random button to 'spawn' a character
        _buttonTexts[_currentChosenIndex].text = loadedCharacter.Item1.ToString();
        _buttonTexts[_currentChosenIndex].color = loadedCharacter.Item2;

        _currentCoroutine = null;
        _characterSpawned = true;
        _stopwatch.StartStopWatch();
    }

    public void DespawnCharacter(int index)
    {
        _stopwatch.PauseStopWatch();
        if (index == _currentChosenIndex)
        {
            _buttonTexts[_currentChosenIndex].text = "";
            _buttonTexts[_currentChosenIndex].color = Color.black;
            
            _reactionTimeData._hits++;
            Debug.Log($"Hits {_reactionTimeData._hits}");
  
            AttemptSpawn();
        }
        else
        {
            _reactionTimeData._misses++;
            Debug.Log($"Misses {_reactionTimeData._misses}");
        }

        


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

        return Tuple.Create(chosenCharacter, chosenColour);
    }


    void CleanUp()
    {
        _creditsPanel.SetActive(true);
        
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
            _stopwatch.PauseStopWatch();
        }
    }
}
