using SnSECS;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
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

        //for every possible answer in the current question
        for (int comboIndex = 0; comboIndex < _panels.Count; comboIndex++)
        {
            
            
            //procceeds to enable a panel and populate it with the correct icons
            if (comboIndex < _currentQuestionData._combos.Count)
            {
                currentComboToLoad = _currentQuestionData._combos[comboIndex];

                _panels[comboIndex].SetActive(true);
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
                _panels[comboIndex].SetActive(false);
            }

        }

       /* _uiIconPositions[_nextComboIndex].sprite = _uiIcons[element];
        _uiIconPositions[_nextComboIndex].gameObject.SetActive(true);
*/
    }
}
