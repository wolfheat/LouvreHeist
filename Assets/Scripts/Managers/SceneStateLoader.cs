using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SceneStateLoader : MonoBehaviour
{

    Dictionary<string, Dictionary<string, object>> scenesData = new();


    public static SceneStateLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    public void ClearAllData() => scenesData = new();
    public bool TryLoad(string sceneName)
    {
        if (!scenesData.ContainsKey(sceneName)) {
            Debug.Log("Scene is not stored in the Dictionary so cant load any, use Defaults");
            return false;
        }

        // Use the file to load the data
        LoadSceneData(scenesData[sceneName]);

        return true;
    }
    public void Save(string sceneName)
    {
        // Use the file to load the data
        SaveSceneData(sceneName);
    }

    private void SaveSceneData(string sceneName)
    {
        var data = new Dictionary<string, object>();

        // Do I need to specify what scene to store items from? Try to find all Items for now
        ISavable[] allItems = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISavable>().ToArray();
        foreach (var item in allItems) {
            data[item.GUID] = item.GetState();
        }

        scenesData[sceneName] = data;        
    }

    private void LoadSceneData(Dictionary<string, object> value)
    {

        ISavable[] allItems = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>().ToArray();

        // Has a valid data file here, use it to load all items to its states
        foreach (var itemData in value) {
            // Find the Object
            foreach (var itemIngame in allItems) {
                if(itemIngame.GUID == itemData.Key) {
                    // Found the item
                    itemIngame.RestoreState(itemData.Value);
                    continue;
                }
            }
        }
    }
}
