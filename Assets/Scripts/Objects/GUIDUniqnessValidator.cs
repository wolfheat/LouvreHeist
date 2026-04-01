using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;

public class GUIDUniqnessValidator : MonoBehaviour
{

#if UNITY_EDITOR    
    private void OnValidate()
    {
        // Find all items with GUID
        // var GUIDHolders


        HashSet<string> usedStrings = new HashSet<string>();

        ISavable[] allItems = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>().ToArray();
        foreach (var item in allItems) {
            string currentGUID = item.GUID;
            while (usedStrings.Contains(currentGUID)) {
                // Generate Unique new
                currentGUID = System.Guid.NewGuid().ToString();
            }

            //SetGUID
            if (currentGUID != item.GUID)
                item.SetGUID(currentGUID);

            usedStrings.Add(currentGUID);
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

}
