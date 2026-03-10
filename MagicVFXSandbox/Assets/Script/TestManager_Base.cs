using DataRecorder;
using SnSECS;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuizManager
{

    public class TestManager_Base : MonoBehaviour
    {
        public static TestManager_Base _instance;
        private TestingState _state;

        [SerializeField]
        private int _testerID = 0;

        [SerializeField]
        private DataRecorder.Recorder _recorder = null;

        [SerializeField]
        private string _folderLocation = string.Empty;

        [SerializeField]
        private ReactionTimeContainerTemplate _reactionTimeData = null;

        private double _SRT = 0;
        private int srtCount = 20;

        bool trackingSRT = true;
        bool srtWriten = false;

        [SerializeField]
        private int _testDurationSeconds = 0;

        public delegate void EndTestEvent();
        public static event EndTestEvent shutDownWorkers;


        private void Awake()
        {
            if (_instance != this)
            {
                _instance = this;
            }

            Stopwatch.stopwatchPaused += RecordTime;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //updates the ID for the identification test manager and sandbox manager
            TestManager._testerID = _testerID;
            TestManager_Sandbox._testerID = _testerID;



            if (_folderLocation == string.Empty)
            {
                _folderLocation = "Assets/Testing Results/Reaction Test/";
            }

            _state = TestingState.SETUP;

            if (_recorder == null)
            {
                Debug.LogError("ERROR! Data recorder reference is null. Creating a new runtime data recorder");

                _recorder = new DataRecorder.Recorder();
            }

            if (_reactionTimeData == null)
            {
                Debug.LogError("ERROR! Reaction time data is null. Creating a new runtime data recorder");

            }

            if (_testDurationSeconds == 0)
            {
                _testDurationSeconds = 0;
            }

            //Sets up the Data Recorder
            _recorder.SetupRecorder(_testerID, _folderLocation);

           

        }

        // Update is called once per frame
        void Update()
        {

        }

        void UpdateTestingState(TestingState newState)
        {
            //only changes state if the new incoming state if different to the old state
            if (newState != _state)
            {
                switch (newState)
                {
                    case TestingState.SETUP:
                        break;
                    case TestingState.PRE_TEST:
                        break;
                    case TestingState.TESTING:
                        break;
                    case TestingState.POST_TEST:
                        break;
                    default:
                        break;
                }
            }
        }

        public void StartTest()
        {
            StartCoroutine(EndTest());
        }

        private IEnumerator EndTest()
        {
            yield return new WaitForSeconds(_testDurationSeconds);

            Stopwatch.stopwatchPaused -= RecordTime;
            shutDownWorkers?.Invoke();

            double averageTimeMS = (_reactionTimeData._cumulativeReactionTimeMS / (_reactionTimeData._hits + _reactionTimeData._misses));
            int totalClicks = _reactionTimeData._hits + _reactionTimeData._misses;

            string stringToWrite = $"Total Clicks: {totalClicks}" + $"\nHits: {_reactionTimeData._hits}" +
                $"\nMisses: {_reactionTimeData._misses}" + $"\nAverage Reaction Time: {averageTimeMS}ms";

            _recorder.WriteToFile(stringToWrite);

            if (!srtWriten)
            {
                _recorder.WriteToFile($"SRT: {_SRT / 20} \n");
            }
        }

        /// <summary>
        /// Event delegate the writes the total reaction time to the data record file
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void RecordTime(double elapsedTime)
        {
            //ignores any omitted results
            if (elapsedTime != -1)
            {
                _reactionTimeData._cumulativeReactionTimeMS += elapsedTime;
                srtCount--;

                if (trackingSRT)
                {
                    if (srtCount > 0)
                    {
                        _SRT += elapsedTime;

                    }
                    else
                    {
                        double SRT = _SRT / 20;
                        string ToWrite = $"SRT: {SRT} \n";
                        _recorder.WriteToFile(ToWrite);
                        trackingSRT = false;
                        srtWriten = true;
                    }
                    //_recorder.WriteToFile($"Reaction Time: {elapsedTime} seconds");
                }
            }
        }

        /// <summary>
        /// Loads the next test
        /// </summary>
        public void SwitchToNextTest()
        {
            SceneManager.LoadScene("Mode_Sandbox");
        }
    }
}