using UnityEngine;
using DataRecorder;
using System.Collections.Generic;
using SnSECS;
using Unity.VisualScripting;
using QuizManager;
using UnityEngine.SceneManagement;

public class TestManager_Sandbox : MonoBehaviour
    {
        public static TestManager_Sandbox _instance;
        
        public static int _testerID = 0;

        [SerializeField]
        private DataRecorder.Recorder _recorder = null;

        [SerializeField]
        private string _folderLocation = string.Empty;

        bool _testing = false;

        public delegate void EndTestEvent();
        public static event EndTestEvent shutDownWorkers;


        private void Awake()
        {
            if (_instance != this)
            {
                _instance = this;
            }

            OrbController.combinationLoaded += RecordCombination;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (_folderLocation == string.Empty)
            {
                _folderLocation = "Assets/Testing Results/Sandbox Sandbox Combinations/";
            }

            if (_recorder == null)
            {
                Debug.LogError("ERROR! Data recorder reference is null. Creating a new runtime data recorder");

                _recorder = new DataRecorder.Recorder();
            }

            //Sets up the Data Recorder
            _recorder.SetupRecorder(_testerID, _folderLocation);
            _recorder.WriteToFile("Combos loaded: ");

        }

        // Update is called once per frame
        void Update()
        {
          
        }

        private void RecordCombination(List<Elements> elements)
        {
        _recorder.WriteToFile(_recorder.ComboToString(elements));
        }

        public void LoadTest()
        {
            TestManager._testerID = _testerID;
        SceneManager.LoadScene("Mode_Quiz_Identification");
        }
        
    }
