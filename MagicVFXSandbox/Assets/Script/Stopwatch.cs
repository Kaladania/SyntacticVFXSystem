using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Stopwatch : MonoBehaviour
{

    [SerializeField]
    private int _countDownLength = 0;

    [SerializeField]
    private TextMeshProUGUI _timerUI = null;

    [SerializeField]
    private double _reactionTimeMin = 0;

    [SerializeField]
    private double _reactionTimeMax = 1;


    private bool _timerActive = false; //states if the countdown timer should current be de-incrimenting
    private bool _stopwatchActive = false;
    private float _countdownTimeRemaining = 0.0f; //time in seconds
    private double _startTime = 0.0f; //records the start time of the question

    public delegate void StopWatchReset(double elapsedTime);
    public static event StopWatchReset stopwatchPaused;

    public delegate void StopWatchEvent();
    public static event StopWatchEvent countdownFinished;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {

        //defaults the countdown length to 3 if the value is invalid
        //countdown length is not inclusive (hence +1)
        if (_countDownLength <= 0)
        {
            _countDownLength = 3;
        }
        _countDownLength += 1;

        if (_timerUI == null)
        {
            _timerUI = new TextMeshProUGUI();
            UnityEngine.Debug.LogWarning("WARNING: Reference to Timer UI is null. Creating a new text mesh pro object");
        }

        if (_reactionTimeMin > _reactionTimeMax)
        {
            double temp = _reactionTimeMax;
            _reactionTimeMax = _reactionTimeMin;
            _reactionTimeMin = temp;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_timerActive)
        {
            //incriments the timer while active
            if (_countdownTimeRemaining > 1)
            {
                _countdownTimeRemaining -= Time.deltaTime;
                UpdateDisplayTime();

            }
            else
            {
                countdownFinished?.Invoke();
                ResetCountdownTimer();
            }
        }
        else if (_stopwatchActive)
        {
            if ((Time.timeAsDouble - _startTime) * 1000 >=_reactionTimeMax )
            {
                PauseStopWatch();
            }
        }
    }

    public void StartCountdown()
    {
        SetTimers();
    }

    public void StartStopWatch()
    {
        _stopwatchActive = true;
        _startTime = Time.timeAsDouble;
    }

    /// <summary>
    /// Updates the timer display number
    /// </summary>
    void UpdateDisplayTime()
    {
        float seconds = Mathf.FloorToInt(_countdownTimeRemaining % 60);
        _timerUI.text = seconds.ToString();
    }

    /// <summary>
    /// Resets countdown timer data and flags
    /// </summary>
    void ResetCountdownTimer()
    {
        _timerUI.gameObject.SetActive(false);
        _countdownTimeRemaining = 0.0f;
        _timerActive = false;
        _timerUI.text = _countDownLength.ToString();

    }

    /// <summary>
    /// Sets all timers to their default values
    /// </summary>
    public void SetTimers()
    {
        _countdownTimeRemaining = _countDownLength;
        _timerUI.gameObject.SetActive(true);
        _timerActive = true;
    }

    public void PauseStopWatch()
    {
        //records the endtime and calculates the total elapsed time
        double endTime = Time.timeAsDouble;
        _stopwatchActive = false;
        double elapsedTime = (endTime - _startTime) * 1000; //calculates the elapsed time in ms

        //disregards times under the minimum
        if (_reactionTimeMin < elapsedTime && elapsedTime < _reactionTimeMax)
        {
            elapsedTime = System.Math.Round(elapsedTime, 2); //rounds the millseconds to 2 decimal palces

            //Debug.Log($"Elapsed time: {elapsedTime}ms");

            
        }
        else
        {
            elapsedTime = -1; //marks the time as '-1' representing an "ommission"
        }

        stopwatchPaused?.Invoke(elapsedTime); //triggers event to Test Manager that time has been recorded

    }

}
