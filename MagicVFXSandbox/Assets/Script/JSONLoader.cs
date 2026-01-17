using SnSECS;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class QuestionData
{
    public List<List<Elements>> _combos; //holds the possible combinations to choose from
    public int _answerIndex; //holds the index of the correct answer
    public int _questionID; //holds the ID of the current question

    /*public QuestionData(List<List<Elements>> combos, int answerIndex, int questionID)
    {
        _combos = comobs;
        _answerIndex = answerIndex;
        _questionID = questionID;
    }*/
}

[System.Serializable]
public class SectionData
{
    public int _sectionID; //holds the ID of the current section
    public List<QuestionData> _questions; //holds a list of all questions in the section
    

    /*public SectionData()
    {
        _questions = new List<QuestionData>();
        _sectionID = -1;
    }*/
}

[System.Serializable]
public struct QuestionDatas
{
    public int answerIndex;//holds the index of the correct answer
    public Elements[][] combos; //holds the possible combinations to choose from

    public int questionID; //holds the ID of the current question

    /*public QuestionData(List<List<Elements>> combos, int answerIndex, int questionID)
    {
        _combos = comobs;
        _answerIndex = answerIndex;
        _questionID = questionID;
    }*/
}

[System.Serializable]
public struct SectionDatas
{
    public int ID; //holds the ID of the current section
    public QuestionDatas[] questions; //holds a list of all questions in the section


    /*public SectionData()
    {
        _questions = new List<QuestionData>();
        _sectionID = -1;
    }*/
}

[System.Serializable]
public class Sections
{
    public SectionDatas[] sections;
}

public class JSONLoader : MonoBehaviour
{
    //public string _JSONLabel = "SNSElements.json"; // Label assigned to addressable JSON

   
    void Start()
    {
    }

    public static Sections CreateFromJSON(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        Debug.Log(jsonString);
        Sections sections = JsonUtility.FromJson<Sections>(jsonString);

        //Sections sections = new Sections();
        //JsonUtility.FromJsonOverwrite(jsonString, sections);

        foreach (SectionDatas section in sections.sections)
        {
            Debug.Log($"Got Section: {section.ID} which has {section.questions.Length} questions");
        }

        return sections;
    }

}