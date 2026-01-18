using SnSECS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct AnswerData
{
    public float totalTime;
}
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CurrentQuestionData _currentQuestionData = null; //holds the scriptable object data container for question data.

    [SerializeField]
    private ElementIconMapTemplate _iconData = null; //holds the scriptable object data container for ui icon maps.

    [SerializeField]
    private TextMeshProUGUI _timerUI = null;

    [SerializeField]
    private int _countDownLength = 0;

    [SerializeField]
    private List<GameObject> _panels = null;

    [SerializeField]
    private List<Button> _buttons = null;
    

    private bool _timerActive = true; //states if the countdown timer should current be de-incrimenting
    private float _countdownTimeRemaining = 0.0f; //time in seconds
    private double _startTime = 0.0f; //records the start time of the question

    public delegate void RecordedTimeEvent(double elapsedTime);
    public static event RecordedTimeEvent timeRecorded;

    [SerializeField]
    private Color _uninteractableColour = Color.gray;


    private void Awake()
    {
        CurrentQuestionData.scriptableObjectUpdated += SetupQuestionUI; //sets up event to load new combo when the question data scriptable object is updated
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        //defaults the countdown length to 3 if the value is invalid
        if (_countDownLength <= 0)
        {
            _countDownLength = 3;
        }

        if (_timerUI == null)
        {
            _timerUI = new TextMeshProUGUI();
            UnityEngine.Debug.LogWarning("WARNING: Reference to Timer UI is null. Creating a new text mesh pro object");
        }

        /*Button button = null;
        foreach (GameObject panel in _panels)
        {
            button = panel.transform.parent.gameObject.GetComponent<Button>();

            if (button != null)
            {
                _buttons.Add(button);
            }
            
        }*/
      
        //SetTimers();
    }

    // Update is called once per frame
    void Update()
    {
        if ( _timerActive)
        {
            //incriments the timer while active
            if (_countdownTimeRemaining > 1)
            {
                _countdownTimeRemaining -= Time.deltaTime;
                UpdateDisplayTime();

            }
            else
            {
                ResetCountdownTimer();
                EnableButtons();
                _startTime = Time.timeAsDouble;
            }
        }
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
    void SetTimers()
    {
        _countdownTimeRemaining = _countDownLength;
        _timerUI.gameObject.SetActive(true );
        _timerActive = true;
    }

    /// <summary>
    /// Toggles visibility and design of panels/ui icons to reflect the state of the current question
    /// </summary>
    void SetupQuestionUI()
    {
        //create panel
        //add the correct sprites

        //Updates the UI to show the icon for the current added element

        List<Elements> currentComboToLoad = null;
        GameObject childObject = null;
        GameObject panelParent = null;

        //for every possible answer in the current question
        for (int comboIndex = 0; comboIndex < _panels.Count; comboIndex++)
        {
            panelParent = _panels[comboIndex].transform.parent.gameObject;

            //procceeds to enable a panel and populate it with the correct icons
            if (comboIndex < _currentQuestionData._combos.Count)
            {
                currentComboToLoad = _currentQuestionData._combos[comboIndex];

                panelParent.SetActive(true);
                childObject = _panels[comboIndex].transform.GetChild(comboIndex).gameObject;

                //for every element in the current combination being loaded for the possible question answers
                for (int i = 0; i < SNSData.MAX_COMBO_LIMIT; i++)
                {
                    

                    //loads the ui icons for every element in the combination
                    if (i < currentComboToLoad.Count)
                    {
                        //loads the correct UI element and sets the icon 
                        _panels[comboIndex].transform.GetChild(i).gameObject.GetComponent<Image>().sprite = _iconData.GetUIIcon(currentComboToLoad[i]);
                        _panels[comboIndex].transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else //disables any unused icons
                    {
                        _panels[comboIndex].transform.GetChild(i).gameObject.SetActive(false);
                    }


                }

            }
            else
            {
                //disables the parent of the panel (the button) - therefore disabling the option completely
                panelParent.SetActive(false);
            }

        }

        //EventSystem.current.SetSelectedGameObject(null);
        SetTimers();
        DisableButtons();
    }

    void DisableButtons()
    {
        ColorBlock colorBlock;
        foreach (Button button in _buttons)
        {
            button.interactable = false;

            //darkens the inactive buttons
            colorBlock = button.colors;
            colorBlock.normalColor = _uninteractableColour;
            button.colors = colorBlock;
        }
    }
    void EnableButtons()
    {
        ColorBlock colorBlock;
        foreach (Button button in _buttons)
        {
            button.interactable = true;

            //sets the colour the inactive buttons to normal
            colorBlock = button.colors;
            colorBlock.normalColor = Color.white;
            button.colors = colorBlock;
        }
    }


    /// <summary>
    /// Deselects the current button by set the current selected object as "null" (nothing)
    /// </summary>
    public void ResetSelection()
    {
        //records the endtime and calculates the total elapsed time
        double endTime = Time.timeAsDouble;
        double elapsedTime = endTime - _startTime;
        elapsedTime = System.Math.Round(elapsedTime, 2); //rounds the millseconds to 2 decimal palces

        timeRecorded?.Invoke(elapsedTime); //triggers event to Test Manager that time has been recorded

        //UnityEngine.Debug.Log($"Gamelapsed time was: {elapsedTime} ");

        EventSystem.current.SetSelectedGameObject(null);

    }
}
