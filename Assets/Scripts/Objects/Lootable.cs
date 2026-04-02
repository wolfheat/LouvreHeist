using System.Linq;
using UnityEngine;

public class Lootable : MonoBehaviour, ISavable
{
    
    #region Savable_Required_Region
    public string GUID => guid;
    [SerializeField] private string guid;

    public void SetGUID(string newGUID) => guid = newGUID;

#if UNITY_EDITOR
    private void OnValidate()
    {

        ISavable[] allItems = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>().ToArray();

        if (string.IsNullOrEmpty(guid) || allItems.Count(x => x.GUID == guid) > 1) {
            guid = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif

    public object GetState()
    {
        return new ItemPickUpState()
        {
            IsAvailable = gameObject.activeSelf,
        };
    }

    public void RestoreState(object stateObject)
    {
        if (stateObject is not ItemPickUpState) {
            Debug.Log("Unable to Read Object as ItemPickUpState");
            return;
        }
        ItemPickUpState state = (ItemPickUpState)stateObject;

        // Only evenr need to read a disabled when saving it, when restoring they are all active by default from scene layout
        if (!state.IsAvailable) {
            gameObject.SetActive(false);
        }
    }

    #endregion



}
