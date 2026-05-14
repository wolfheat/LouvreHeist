using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ObjectHandeler
{
    private static ISavable[] savables;
    private static ISavable[] conflictsItems;

    [MenuItem("Tools/Analyze GUIDs", priority = 1)]
    public static void AnalyzeGUIDs()
    {
        // Goes through all items and chcek for conflicts
        savables = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID).OfType<ISavable>().ToArray();

        Debug.Log("OBJECTS FOUND: "+savables.Length);

        Dictionary<string,string> used = new Dictionary<string, string>();

        List<ISavable> conflictList = new List<ISavable>();

        int conflicts = 0;

        foreach (ISavable savable in savables) {
            string GUID = savable.GUID;
            string itemName = (savable as MonoBehaviour).name;
            if (used.ContainsKey(GUID)) {
                conflictList.Add(savable);
                conflicts++;
                Debug.Log("Conflict: " + itemName + " with " + used[GUID]);
                continue;
            }
            used.Add(GUID,itemName);
        }
        Debug.Log("CONFLICTS: "+conflicts);
        conflictsItems = conflictList.ToArray();
    }


    [MenuItem("Tools/Resolve all GUID conflicts", priority = 1)]
    //[ContextMenu("Resolve all GUID conflicts")]
    public static void ResolveGUIDConflicts()
    {        
        if (conflictsItems == null || conflictsItems.Length == 0) {
            Debug.Log("No conflicts stored.");
            return;
        }
        
        // Goes through all items and chcek for conflicts

        Debug.Log("RESOLVING CONFLICTS: ");
        
        foreach (ISavable conflictItem in conflictsItems) {
            MonoBehaviour conflictObject = conflictItem as MonoBehaviour;   
            conflictItem.SetGUID(System.Guid.NewGuid().ToString());
            UnityEditor.EditorUtility.SetDirty(conflictObject);
        }

        Debug.Log("CONFLICTS RESOLVED: "+conflictsItems.Length);
        conflictsItems = null;
    }

    
    //[ContextMenu("Print All GUID's")]
    [MenuItem("Tools/Print All GUID's", priority = 2)]
    public static void PrintAllGuids()
    {
        if (savables == null || savables.Length == 0) {
            Debug.Log("No stored GUID Objects.");
            return;
        }

        Debug.Log("ALL GUIDs: ");

        foreach (ISavable savable in savables) {
            string GUID = savable.GUID;
            Debug.Log( "Item: " + savable.GUID + " " + (savable as MonoBehaviour).name);
        }
        Debug.Log("TOTAL ITEMS: "+savables.Length);
    }
}
