using UnityEngine;
using DataRecorder;
using System.Collections.Generic;
using SnSECS;
using Unity.VisualScripting;

namespace QuizManager
{

    public enum TestingState
    {
        SETUP,
        PRE_TEST,
        TESTING,
        POST_TEST
    }

    public class TestManager : MonoBehaviour
    {
        public static TestManager _instance;
        private TestingState _state;
        private int _currentComboIndex = 0; //stores the index of the current combo being tested
        private List<List<Elements>> _sectionCombos; //stores all the combos due to be tested in the current section

        private List<SectionData> _sections;
        private int _currentSection = 0;
        private int _currentQuestion = 0;

        //private event CurrentQuestionData.ScriptableObjectUpdateEvent _updateEvent; //the event triggered by new question data being given to the questionData scriptable object

        [SerializeField]
        private int _testerID = 0;

        [SerializeField]
        private DataRecorder.Recorder _recorder = null;

        [SerializeField]
        private CurrentQuestionData _currentQuestionData = null; //holds the scriptable object data container for question data.

        [SerializeField]
        private string _filePath = string.Empty;


        private void Awake()
        {
            if (_instance != this)
            {
                _instance = this;
            }

            UIManager.timeRecorded += RecordTime;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            if (_filePath == string.Empty)
            {
                _filePath = "Assets/SNSTestingQuestions.json";
            }

            _state = TestingState.SETUP;

            if (_recorder == null)
            {
                Debug.LogError("ERROR! Data recorder reference is null. Creating a new runtime data recorder");

                _recorder = new DataRecorder.Recorder();
            }

            //Sets up the Data Recorder
            _recorder.SetupRecorder(_testerID);

            //_updateEvent = SetupSpawner; 
            //sets up event to load new combo when the question data scriptable object is updated
            //call "Load Combos for Section" from JSON Loader
            //e.g. _sectionCombos = LoadCombosFromSection(_currentComboIndex);

            //OR 
            //Since the JSON NEEDS a struct, return the struct instead. Then populate it in a function

            //- - - - - 
            //Call function to get JSON loader to parse JSON and return a list of Section Data (populated with question data)




            _sections = JSONLoader.CreateFromJSON(_filePath);

            if (_currentQuestionData != null)
            {
                _recorder.WriteToFile($"--- SECTION: {_currentSection + 1} ----");
                ChangeQuestion();
            }
            else
            {
                Debug.LogError("ERROR: Reference to Question Data Scriptable Object is null");
            }
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

        void ChangeQuestion()
        {
            if (_currentSection < _sections.Count)
            {
                //if there are still more questions to be asked in the current section, load the next question
                if (_currentQuestion < _sections[_currentSection].questions.Count)
                {
                    
                    //Stores the current question data in the scriptable object
                    //Triggers the "updateSO" event to get the combo spawner and UI Manager to analyse the new data
                    QuestionData question = _sections[_currentSection].questions[_currentQuestion];
                    _currentQuestionData.SetQuestionData(question.combos, question.answerIndex);
                    _currentQuestion++;

                    string recorderText =
                        $"Question: {_currentQuestion}\n Tested Combination: " + _recorder.ComboToString(question.combos[question.answerIndex]);
                    _recorder.WriteToFile(recorderText);
                }
                else
                {
                    _currentSection++;
                    _currentQuestion = 0;

                    _recorder.WriteToFile($"--- SECTION: {_currentSection + 1} ----");
                }

                    

            }
            else
            {
                //signals that the test has finised.
                UpdateTestingState(TestingState.POST_TEST);
            }
        }

        public void StopTimer(int answerIndex)
        {
            string questionResult = $"Chosen answer: {answerIndex}";
            questionResult += answerIndex == _currentQuestionData._answerIndex ? " [CORRECT]" : " [INCORRECT]";

            _recorder.WriteToFile(questionResult);
            ChangeQuestion();

        }

        /// <summary>
        /// Event delegate the writes the total reaction time to the data record file
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void RecordTime(double elapsedTime)
        {
            _recorder.WriteToFile($"Reaction Time: {elapsedTime}");
        }

    }
}