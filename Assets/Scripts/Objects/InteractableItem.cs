using System.Linq;
using UnityEngine;

public class InteractableItem : Interactable, ISavable
{
    public ItemData Data;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    #region Savable_Required_Region
    public string GUID => guid;
    [SerializeField] private string guid;

    public void SetGUID(string newGUID) => guid = newGUID;

#if UNITY_EDITOR
    private void OnValidate()
    {

        ISavable[] allItems = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>().ToArray();

        if (string.IsNullOrEmpty(guid)) {
            guid = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
    [ContextMenu("FORCE RENEW GUID")]
    private void ForceRenewGUID()
    {   
        guid = System.Guid.NewGuid().ToString();
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    public void RestoreState(object stateObject)
    {
        if (stateObject is not ItemPickUpState) {
            Debug.Log("Unable to Read Object as ItemPickUpState");
            return;
        }
        ItemPickUpState state = (ItemPickUpState)stateObject;
                
        Debug.Log("InteractableItem Restoring State for: " + name + " state active is: " + state.IsAvailable + " GUID: " + GUID);
        // Only evenr need to read a disabled when saving it, when restoring they are all active by default from scene layout
        if (!state.IsAvailable) {
            gameObject.SetActive(false);
        }
    }

    #endregion



    public object GetState()
    {
        Debug.Log("InteractableItem Getting State for: "+name+" state active is: "+gameObject.activeSelf+" GUID: "+GUID);
        return new ItemPickUpState()
        {
            IsAvailable = gameObject.activeSelf
        };
    }
}
