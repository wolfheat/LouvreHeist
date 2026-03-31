using UnityEngine;
using Wolfheat.StartMenu;

public class Breakable : MonoBehaviour
{
    // Breakable box, when broken generates its content at its place? From Data file SO?

    // Have specifik shatter material when breaking?

    [SerializeField] private Animator animator;
    
    [SerializeField] private Collider collider;

    protected bool isOpen = false;

    public bool IsOpen => isOpen;

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
        animator?.SetBool("Shatter", true);

        isOpen = true;

        collider.enabled = false;
    }
}
