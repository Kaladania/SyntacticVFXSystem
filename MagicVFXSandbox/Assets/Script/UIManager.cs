using SnSECS;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CurrentQuestionData _currentQuestionData = null; //holds the scriptable object data container for question data.

    [SerializeField]
    private ElementIconMapTemplate _iconData = null; //holds the scriptable object data container for ui icon maps.

    [SerializeField]
    private List<GameObject> _panels = null;

    private void Awake()
    {
        CurrentQuestionData.scriptableObjectUpdated += SetupQuestionUI; //sets up event to load new combo when the question data scriptable object is updated
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*Button buttonComponent;
        for (int i = 0; i < _panels.Count; i++)
        {
            buttonComponent = _panels[i].GetComponent<Button>();
            buttonComponent.onClick.AddListener(delegate { StopTimer(i); });
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    }

    /// <summary>
    /// Deselects the current button by set the current selected object as "null" (nothing)
    /// </summary>
    public void ResetSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
