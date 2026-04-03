using UnityEngine;

public enum HelpButtonType{LMB,RMB,Info};

public class HelpInstructions : MonoBehaviour
{
    [SerializeField] private ButtonInstruction LMB; 
    [SerializeField] private ButtonInstruction RMB; 
    [SerializeField] private ButtonInstruction Info; 

    private float infoTimer = 0f;
    private float updateTimer = 0f;
    private const float InfoTime = 5f;
    private const float UpdateTime = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("HelpInstructions: Timed Update Of Help Instructions");
        ClearInstructions();
    }

    public void ClearInstructions(HelpButtonType button)
    {
        Debug.Log("HelpInstructions: Clearing Instruction");
        if(button == HelpButtonType.LMB)
            LMB.gameObject.SetActive(false);
        if(button == HelpButtonType.RMB)
            RMB.gameObject.SetActive(false);
        if(button == HelpButtonType.Info)
            Info.gameObject.SetActive(false);
    }
    
    public void ClearInstructions()
    {
        Debug.Log("HelpInstructions: Clearing Instructions all");
        LMB.gameObject.SetActive(false);
        RMB.gameObject.SetActive(false);
    }

    public void ShowInstruction(string stringText, HelpButtonType type = HelpButtonType.LMB)
    {
        Debug.Log("HelpInstructions: show "+stringText);
        if(type == HelpButtonType.LMB) {
            LMB.gameObject.SetActive(true);
            LMB.HelpText.text = stringText;
        }
        else if (type == HelpButtonType.RMB) {
            
            RMB.gameObject.SetActive(true);
            RMB.HelpText.text = stringText;
        }
        else {
            // This should have a Timer
            infoTimer = InfoTime;
            Info.gameObject.SetActive(true);
            Info.HelpText.text = stringText;
        }
    }
    private void Update()
    {
        if(infoTimer > 0) {
            infoTimer -= Time.deltaTime;
            if(infoTimer <= 0) {
                Info.gameObject.SetActive(false);
            }
        }

        updateTimer -= Time.deltaTime;
        if(updateTimer <= 0) {
            updateTimer += UpdateTime;
            TimedUpdateOfKeys();
        }
    }
    private void TimedUpdateOfKeys()
    {
        Debug.Log("Timed Update Of Help Instructions");
        Stair stair = PickUpController.Instance.Stair;
        LockPickable lockPickable = PickUpController.Instance.LockPickable;
        Grindable grindable = PickUpController.Instance.Grindable;
        Breakable breakable = PickUpController.Instance.Breakable;
        Vehicle vehicle = PickUpController.Instance.Vehicle;
        Interactable interactable = PickUpController.Instance.ActiveInteractable;

        bool anyAvailableleft = stair != null || lockPickable != null || grindable != null || breakable != null || vehicle != null ;
        bool anyAvailableright = vehicle != null || interactable != null;

        if (stair != null) {
            ShowInstruction("Climb Stairs");
        }
        if (lockPickable != null) {
            ShowInstruction(lockPickable.IsUnLocked ? "Open/Close" : "Pick Lock");
        }
        if (grindable != null) {
            ShowInstruction("Grind");
        }
        if (breakable != null) {
            ShowInstruction("Break");
        }
        
        if (interactable != null) {
            ShowInstruction("Grab",HelpButtonType.RMB);
        }
        if (vehicle != null) {
            if (PlayerController.Instance.IsSeated) {
                ShowInstruction("Exit Vehicle");
                ShowInstruction(vehicle.Engaged? vehicle.GetDisEngageInstructions : vehicle.GetEngageInstructions,HelpButtonType.RMB);
            }
            else
                ShowInstruction("Enter Vehicle");
        }

        if (!anyAvailableleft) {
            ClearInstructions(HelpButtonType.LMB);
        }
        if (!anyAvailableright) {
            ClearInstructions(HelpButtonType.RMB);
        }



    }


    public static HelpInstructions Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


}
