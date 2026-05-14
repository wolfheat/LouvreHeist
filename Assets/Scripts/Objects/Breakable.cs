using UnityEngine;
using Wolfheat.StartMenu;

public class Breakable : MonoBehaviour, ISavable
{
    // Breakable box, when broken generates its content at its place? From Data file SO?

    // Have specifik shatter material when breaking?

    [SerializeField] private Animator animator;
    
    [SerializeField] private Collider collider;

    protected bool isOpen = false;

    public bool IsOpen => isOpen;




    #region Savable_Required_Region
    public string GUID => guid;
    [SerializeField] private string guid;
    public void SetGUID(string newGUID) => guid = newGUID;

#if UNITY_EDITOR
    private void OnValidate()
    {
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

    public object GetState()
    {
        return new BreakableState()
        {
            IsOpen = isOpen
        };
    }

    public void RestoreState(object stateObject)
    {
        if (stateObject is not BreakableState) {
            Debug.Log("Unable to Read Object as LockpickableState");
            return;
        }
        BreakableState state = (BreakableState)stateObject;

        isOpen = state.IsOpen;

        if (isOpen) {
            Break();
            //animator.SetBool("brake", true);
            //animator.CrossFade("Open")
        }

    }

    #endregion




    public void Break()
    {
        // Breaks the Item

        // Creates shatter parts from Data definition

        // Removes the Breakable? Maybe replace it with a broken one?

        Debug.Log("Box was successfully broken");

        Debug.Log("Creating Shatter parts as its broken");

        Debug.Log("Replacing box with its broken version (which is not breakable any longer)");

        Debug.Log("Creating content at its place, this is pickable");

        // Run Glass destroy animator? Show Shattered glass particles?
        animator?.SetBool("break", true);

        isOpen = true;

        collider.enabled = false;
    }

    public void DisableVisuals()
    {
        if (transform.GetChild(0)?.TryGetComponent<MeshRenderer>(out MeshRenderer childRenderer) != null)
            childRenderer.enabled = false;

    }
}
