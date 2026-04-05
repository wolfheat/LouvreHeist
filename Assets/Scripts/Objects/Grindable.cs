using System.Linq;
using UnityEngine;
using Wolfheat.StartMenu;

public class Grindable : MonoBehaviour, ISavable
{
    protected bool isOpen = false;

    [SerializeField] private Animator animator;
    [SerializeField] private bool walkableWhenBroken = false;

    public bool IsOpen => isOpen;

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

    public object GetState()
    {
        return new GrindableState()
        {
            IsOpen = isOpen
        };
    }

    public void RestoreState(object stateObject)
    {
        if (stateObject is not GrindableState) {
            Debug.Log("Unable to Read Object as GrindableState");
            return;
        }
        GrindableState state = (GrindableState)stateObject;

        isOpen = state.IsOpen;

        if (isOpen) {
            SetToOpenState();
        }

    }

    #endregion


    private void SetToOpenState()
    {
        // Run Glass destroy animator? Show Shattered glass particles?
        animator.SetBool("break", true);

        if (walkableWhenBroken && TryGetComponent<Collider>(out Collider collider))
            collider.enabled = false;
    }

    public void GrindCompleteOpen()
    {
        isOpen = true;

        Debug.Log("Glass was successfully Opened ");

        SoundMaster.Instance.PlaySound(SoundName.GlassBreak);

        SetToOpenState();
    }

}
