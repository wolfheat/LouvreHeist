using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public Interactable ActiveInteractable { get; set; }
    public Wall Wall { get; set; }
    public Door Door { get; set; }
    public Vehicle Vehicle { get; set; }
    public InfoPanel InfoPanel { get; set; }
    public Altar Altar { get; set; }
    public LockPickable LockPickable { get; set; }
    public Breakable Breakable { get; set; }
    public Grindable Grindable { get; set; }
    public EnemyController Enemy { get; set; }
    public Mock Mockup { get; set; } = null;
    public Stair Stair { get; set; } = null;

    private LayerMask enemyLayerMask;
    private LayerMask mockupLayerMask;
    private LayerMask wallAndDoorLayerMask;
    private LayerMask doorLayerMask;
    private LayerMask itemLayerMask;


    public static PickUpController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    private void Start()
    {
        doorLayerMask = LayerMask.GetMask("Door");
        wallAndDoorLayerMask = LayerMask.GetMask("Wall") | doorLayerMask;
        enemyLayerMask = LayerMask.GetMask("Enemy");
        mockupLayerMask = LayerMask.GetMask("Mock");
        itemLayerMask = LayerMask.GetMask("Items","ItemsSeeThrough") ;
        Restart();
    }

    private void OnEnable()
    {
        Restart();
    }

    public void Restart()
    {
        UpdateColliders();
        StopAllCoroutines();
        StartCoroutine(UpdateCollidersInterval());
    }

    private WaitForSeconds wait = new WaitForSeconds(0.1f);
    private IEnumerator UpdateCollidersInterval()
    {
        while (true)
        {
            UpdateColliders();
            yield return wait;
        }
    }

    public void UpdateColliders(bool wait = false)
    {
        //Debug.Log("* Updating Colliders "+(wait?" after waiting *":"*"));
        UpdateWall();        
        UpdateInteractables();
        UpdateEnemy();        
    }

    public IEnumerator UpdateCollidersWait()
    {
        yield return null;
        yield return null;
        UpdateColliders(true);
        PlayerController.Instance.MotionActionCompleted();
    }

    public void UpdateEnemy()
    {
        // Get list of interactable items
        Collider[] colliders = Physics.OverlapBox(Convert.Align(transform.position), Game.PickupDetectionBoxSize,Quaternion.identity, enemyLayerMask);

        //UIController.Instance.UpdateShownItemsUI(colliders.Select(x => x.GetComponentInParent<EnemyController>().EnemyData as ItemData).ToList());

        if (colliders.Length == 0)
            Enemy = null;
        else
        {
            EnemyController enemy = colliders[0].gameObject.GetComponentInParent<EnemyController>();
            if (!enemy.Dead)
                Enemy = enemy;
            else 
                Enemy = null;
        }


        // Get enemy mockup
        //Mockup = colliders.Where(x => x.GetComponentInParent<Interactable>() == null).ToArray().Length > 0?true:false;
        colliders = Physics.OverlapBox(Convert.Align(transform.position), Game.PickupDetectionBoxSize, Quaternion.identity, mockupLayerMask);

        Mock candidate = colliders.Where(x => x.GetComponent<Mock>() != null).ToArray().FirstOrDefault()?.GetComponent<Mock>();
        if (candidate != null && !candidate.IsPlayer)
        {
            //Debug.Log("Found a Mock at position:" + candidate.transform.position + " pos:" + candidate.pos + " name:" + candidate.name);
            Mockup = candidate;
        }
        else
            Mockup = null;
    }
    
    public void UpdateWall()
    {
        // Align box with grid before casting

        Vector3 alignedPos = Convert.Align(transform.position);
        Vector3 posBetweenPLayerAndPickupPosition = transform.position + (PlayerController.Instance.transform.position - transform.position)/2;

        // Get list of interactable items
        Collider[] colliders = Physics.OverlapBox(alignedPos, Game.PickupDetectionBoxSize,transform.rotation, wallAndDoorLayerMask);

        // Also Include close Doors in separate ColliderOverlap?
        Collider[] collidersDoors = Physics.OverlapBox(posBetweenPLayerAndPickupPosition, Game.PickupDetectionBoxSize,transform.rotation, wallAndDoorLayerMask);

        colliders = colliders.Concat(collidersDoors).ToArray();
        /*
        bool hadStair = Stair != null;
        bool hadVehicle = Vehicle != null;
        bool hadLockPick = LockPickable != null;
        bool hadBreakable = Breakable != null;
        bool hadGrindable = Grindable != null;
        */
        // Unset all
        Wall = null;
        Door = null;
        Vehicle = null;
        Stair = null;
        LockPickable = null;
        Breakable = null;
        Grindable = null;
        InfoPanel = null;
        Altar = null;

        if(colliders.Length > 0) {

            int AnythingButWallFound = 0;

            string foundItems = "";

            foreach (Collider collider in colliders) {

                if (collider.gameObject.TryGetComponent(out Wall FoundWall)) {
                    // Found A wall
                    foundItems += " Wall";
                    this.Wall = FoundWall;
                    Door = null;
                }
                else if(collider.gameObject.TryGetComponent(out Door FoundDoor)) {
                    foundItems +=  " Door";
                    this.Door = FoundDoor;
                    AnythingButWallFound++;
                }
                else if (collider.gameObject.TryGetComponent(out Vehicle FoundVehicle)) {
                    // Found A wall
                    foundItems +=  " Vehicle";
                    this.Vehicle = FoundVehicle;
                   AnythingButWallFound++;
                }
                else if (collider.gameObject.TryGetComponent(out LockPickable FoundLockpickable)) {
                    // Found A wall
                    foundItems +=  " LockPickable";
                    this.LockPickable = FoundLockpickable;
                   AnythingButWallFound++;
                }
                else if (collider.gameObject.TryGetComponent(out Breakable FoundBreakable)) {
                    // Found A wall
                    foundItems +=  " Breakable";
                    this.Breakable = FoundBreakable;
                   AnythingButWallFound++;
                }
                else if (collider.gameObject.TryGetComponent(out Grindable FoundGrindable)) {
                    // Found A wall
                    foundItems +=  " Grindable";
                    this.Grindable = FoundGrindable;
                   AnythingButWallFound++;
                }
                else if (collider.gameObject.TryGetComponent(out Stair FoundStair)) {
                    // Found A wall
                    foundItems +=  " Stair";
                    this.Stair = FoundStair;
                   AnythingButWallFound++;
                }
                else if (collider.gameObject.TryGetComponent(out InfoPanel FoundInfoPanel)) {
                    // Found A wall
                    foundItems +=  " InfoPanel";
                    this.InfoPanel = FoundInfoPanel;
                    AnythingButWallFound++;
                }else if (collider.gameObject.TryGetComponent(out Altar FoundAltar)) {
                    // Found A wall
                    foundItems += " FoundAltar";
                    this.Altar = FoundAltar;
                    AnythingButWallFound++;
                }
            }
            if (AnythingButWallFound > 0)
                Wall = null;

            SeatedUIDisplayer.Instance?.ShowPickupType(foundItems);
        }
        /*
        // Show Help text for stairs
        if (Stair != null) {
            Debug.Log("Stair Present");
            HelpInstructions.Instance.ShowInstruction("Climb Stairs");

        }else if (hadStair) {
            Debug.Log("Clearinstructions");
            HelpInstructions.Instance.ClearInstructions();
        }
        
        // Show Help text for vehicle
        if (LockPickable != null) {
            
            HelpInstructions.Instance.ShowInstruction(LockPickable.IsUnLocked?"Open/Close": "Pick Lock");

        }else if (hadLockPick) {
            HelpInstructions.Instance.ClearInstructions();
        }
        
        // Show Help text for vehicle
        if (Grindable != null) {
            HelpInstructions.Instance.ShowInstruction("Grind");

        }else if (hadGrindable) {
            HelpInstructions.Instance.ClearInstructions();
        }
        
        // Show Help text for vehicle
        if (Breakable != null) {
            HelpInstructions.Instance.ShowInstruction("Break");

        }else if (hadBreakable) {
            HelpInstructions.Instance.ClearInstructions();
        }
        
        // Show Help text for vehicle
        if (Vehicle != null) {
            Debug.Log("Vehicle Present");
            HelpInstructions.Instance.ShowInstruction("Enter Vehicle");

        }else if (hadVehicle) {
            Debug.Log("Clearinstructions");
            HelpInstructions.Instance.ClearInstructions();
        }
        */

    }

    public void UpdateInteractables()
    {
        //Debug.Log("Loot Update");   
        bool hadInteractable = ActiveInteractable != null;
        // If there is an lockpickable Item that is not open disregard any attemp to get an item inside
        if (LockPickable != null && !LockPickable.IsOpen) {
            //Debug.Log("Lockpickable ahead that is not open, unset any loot");
            ActiveInteractable = null;
            return;
        }
        // Also if there is a grindable Item that is not open disregard any attemp to get an item inside
        if (Grindable != null && !Grindable.IsOpen) {
            //Debug.Log("Grindable ahead that is not open, unset any loot");
            ActiveInteractable = null;
            return;
        }

        //Debug.Log("Updating Loot");

        // Get list of interactable items
        Collider[] colliders = Physics.OverlapBox(Convert.Align(transform.position), Game.PickupDetectionBoxSize,Quaternion.identity, itemLayerMask);
        
        //UIController.Instance.UpdateShownItemsUI(colliders.Select(x => x.GetComponent<InteractableItem>()?.Data).ToList(),true);
        if (colliders.Length == 0)
        {
            //Debug.Log("No Loot");
            //Debug.LogError("No Interactable found.");
            ActiveInteractable = null;
        }
        else {
            //Debug.Log("Active Interactable set to: " + colliders[0].name);
            //Debug.Log("Loot " + colliders[0].name);
            ActiveInteractable = colliders[0].gameObject.GetComponent<Interactable>();
        }
    }

    public bool InteractWithEnemy()
    {
        if (Enemy == null && Mockup == null) return false;

        if (Enemy != null)
            Enemy.TakeDamage(Stats.Instance.Damage);
        else if (Mockup.owner.TryGetComponent(out EnemyController enemy))
            enemy.TakeDamage(Stats.Instance.Damage);
        
        UpdateColliders();

        return true;
    }
    public bool InteractWithWall()
    {
        if (Wall == null) return false;

        if (Wall.WallData == null)
        {
            Debug.Log("Interact with Wall without Data = Bedrock");
            return false;
        }
        
        // Lets player hit and destroy walls

        //if(Wall.Damage(Stats.Instance.Damage))
        //    UpdateColliders();

        return true;
    }
}
