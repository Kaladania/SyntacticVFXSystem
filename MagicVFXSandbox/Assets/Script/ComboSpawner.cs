using QuizManager;
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

    [SerializeField]
    private CurrentQuestionData _questionDataSO; //the scriptable object holding the current question data

    private Coroutine _currentCoroutine = null;

    private bool _spawnProjectile = false;

    //ScriptableObjectUpdateEvent _updateEvent; //the event triggered by new question data being given to the questionData scriptable object

    private void Awake()
    {
        CurrentQuestionData.scriptableObjectUpdated += SetupSpawner; //sets up event to load new combo when the question data scriptable object is updated
        TestManager.shutDownWorkers += Cleanup; //sets up event to disable spawning
    }

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
        /*LoadCombination(new List<Elements>(){ Elements.FIRE, Elements.WATER, Elements.LIGHTNING });
        StartCoroutine(SpawnProjectile(_spawnFrequency, _currentSnSVFX));*/

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

    /// <summary>
    /// Loads a new combination and begins endlessly spawning it
    /// Event delegate that is called once new question data has been supplied by the Test Manager
    /// </summary>
    void SetupSpawner()
    {
        //stops the current co-routine if there is any
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _spawnProjectile = false;
        }

        //_currentCoroutine = SpawnProjectile(_spawnFrequency, _currentSnSVFX);
        LoadCombination(_questionDataSO.GetAnswerCombo());

        //set to 0 so it starts instantly
        _currentCoroutine = StartCoroutine(SpawnProjectile(0, _currentSnSVFX));
        //StartCoroutine(_currentCoroutine);

    }

    void StartSpawner()
    {
        StartCoroutine(SpawnProjectile(_spawnFrequency, _currentSnSVFX));
    }

    private IEnumerator SpawnProjectile(float coundownDuration, List<VisualEffectAsset> vfx)
    {
        //_spawnProjectile = true;
        yield return new WaitForSeconds(coundownDuration); //returns a reference to the spawned enemy after a specified amount of time

        _orbController.CreateProjectile(vfx);
        //StartCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(SpawnProjectile(_spawnFrequency, vfx));
    }

    void Cleanup()
    {
        //stops the current co-routine if there is any
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _spawnProjectile = false;
        }
    }
}
