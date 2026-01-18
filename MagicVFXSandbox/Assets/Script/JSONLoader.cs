using SnSECS;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Newtonsoft.Json;

[System.Serializable]
public struct QuestionData
{
    

    //[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public List<List<Elements>> combos; //holds the possible combinations to choose from
    public int answerIndex;//holds the index of the correct answer

}

[System.Serializable]
public struct SectionData
{
    public int ID; //holds the ID of the current section
    public List<QuestionData> questions; //holds a list of all questions in the section

}

[System.Serializable]
public struct Sections
{
    public List<SectionData> sections;
}

public class JSONLoader : MonoBehaviour
{
    //public string _JSONLabel = "SNSElements.json"; // Label assigned to addressable JSON
    void Start()
    {
    }

    public static List<SectionData> CreateFromJSON(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        //Sections sections = JsonUtility.FromJson<Sections>(jsonString);
        List<SectionData> sections = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SectionData>>(jsonString);
        /*foreach (SectionData section in sections.sections)
        {
            Debug.Log($"Got Section: {section.ID} which has {section.questions.Length} questions");
        }
*/
        return sections;
    }

}