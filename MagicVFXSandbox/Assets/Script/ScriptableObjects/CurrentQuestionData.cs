using System.Collections.Generic;
using SnSECS;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "CurrentQuestionData", menuName = "Scriptable Objects/CurrentQuestionData")]
public class CurrentQuestionData : ScriptableObject
{
    [Tooltip("Combination Options to Load")]
    public List<List<Elements>> _combos=new List<List<Elements>>();

    [Tooltip("Index of the correct combo answer")]
    public int _answerIndex = 0;

    public delegate void ScriptableObjectUpdateEvent();
    public static event ScriptableObjectUpdateEvent scriptableObjectUpdated;

    /// <summary>
    /// Returns the combo designated as the correct answer
    /// </summary>
    /// <returns>The correct combo</returns>
    public List<Elements> GetAnswerCombo() { return _combos[_answerIndex]; }
    public void SetQuestionData(List<List<Elements>> combos, int index)
    {
        _combos = combos;
        _answerIndex = index;

        if (scriptableObjectUpdated != null)
        {
            scriptableObjectUpdated?.Invoke(); //raises an event to state that new data has been given to the scriptable object
        }
        else
        {
            Debug.LogWarning("Update event was null");
        }
    }

}
