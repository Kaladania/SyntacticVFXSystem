using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class JSONLoader : MonoBehaviour
{
    public string _JSONLabel = "SNSElements.json"; // Label assigned to addressable JSON

    void Start()
    {
        //adds a callback to run a deserializing function the moment the JSON file has been fully loaded
        Addressables.LoadAssetAsync<TextAsset>(_JSONLabel).Completed += OnJsonLoaded;
        //LoadJson();
    }

    /*void LoadJson()
    {
        Addressables.LoadAssetAsync<TextAsset>(_JSONLabel).Completed += OnJsonLoaded;
    }*/

    void OnJsonLoaded(AsyncOperationHandle<TextAsset> JSONreference)
    {
        //Deserialises JSON file if successfully loaded
        if (JSONreference.Status == AsyncOperationStatus.Succeeded)
        {
            string jsonContent = JSONreference.Result.text; //writes all content within the JSON file to a string

            PlayerData myData = JsonUtility.FromJson<PlayerData>(jsonContent);

            // Print all properties
            Debug.Log($"Player Name: {myData.playerName}");
            Debug.Log($"Level: {myData.level}");
            Debug.Log($"Experience: {myData.experience}");

            // Print Inventory Items
            Debug.Log("Inventory Items:");
            foreach (var item in myData.inventory)
            {
                Debug.Log($"- {item.itemName} (ID: {item.itemID}, Quantity: {item.quantity})");
            }

            // Print Settings
            Debug.Log("Settings:");
            Debug.Log($"- Sound Volume: {myData.settings.soundVolume}");
            Debug.Log($"- Music Volume: {myData.settings.musicVolume}");
            Debug.Log($"- Full Screen: {myData.settings.isFullScreen}");
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + JSONreference.Status);
        }

        // Release memory
        Addressables.Release(JSONreference);
    }
}

// JSON Data Model
[System.Serializable]
public class InventoryItem
{
    public int itemID;
    public string itemName;
    public int quantity;
}

[System.Serializable]
public struct Settings // Rename to BaseElement
{
    public float soundVolume;
    public float musicVolume;
    public bool isFullScreen;
}

public struct ExtraElement
{

}

//ENTIRE THING
[System.Serializable]
public struct PlayerData //Rename to SNSElementComponent
{
    public string playerName;
    public int level;
    public int experience;
    public InventoryItem[] inventory;
    public Settings settings;
}

//PlayerData and Settings used to be classes. See about why they are failing with structs