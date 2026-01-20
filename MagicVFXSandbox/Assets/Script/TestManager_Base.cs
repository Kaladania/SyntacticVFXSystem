using DataRecorder;
using SnSECS;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

            StartCoroutine(EndTest());

        }

        // Update is called once per frame
        void Update()
        {
            switch (_state)
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
        }

        /// <summary>
        /// Event delegate the writes the total reaction time to the data record file
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void RecordTime(double elapsedTime)
        {
            _reactionTimeData._cumulativeReactionTimeMS += elapsedTime;
            //_recorder.WriteToFile($"Reaction Time: {elapsedTime} seconds");
        }

    }
}