using System;
using System.Collections;
using UnityEngine;
using Wolfheat.StartMenu;
public class Vehicle : MonoBehaviour
{
    //[SerializeField] GameObject doorPart;
    [SerializeField] BoxCollider doorCollider;
    [SerializeField] Transform seatedPositionTransform;
    [SerializeField] Transform exitPositionTransform;
    [SerializeField] Animator vehicleAnimator;
    [SerializeField] string EngineUseParameterName = "StartEngine";
    [SerializeField] string EngageInstruction = "EngageInstruction";
    [SerializeField] string DisEngageInstruction = "DisEngageInstruction";
    [SerializeField] Vehicle EngageNeededToEngage;
    [SerializeField] GameObject EngagedObjectActivated;

    public string GetEngageInstructions => EngageInstruction;
    public string GetDisEngageInstructions => DisEngageInstruction;
    public bool Engaged { get; set; } = false;

    public Transform GetSeatedTransform => seatedPositionTransform;
    public Transform GetExitTransform => exitPositionTransform;

    protected bool bossDoor = false;



    public virtual bool IsBossDoor => false;

    //Vector3 seatedPosition = new Vector3(0.1f,  0.32f, -0.35f);

    public Transform EnterVehicle()
    {
        Debug.Log("Entering The Vehicle");

        Debug.Log("Lock Player to a position? -5.9  0.32 12.65");

        // Have this be local position per vehicle so it follows the vehicles rotation
        Debug.Log("Lock Player to local position? 0.1  0.32 -0.35");

        // Since rotation isnt on a spot, prohibit rotations when in vihicle?

        Debug.Log("Lower the Spotlight to intensity 0.05?");
        
        SoundMaster.Instance.PlaySound(SoundName.OpenDoor);

        // Should I send the coordinates to player for when entering? SHould not be able to move inside but rotate?
        // Or should the vehicle handle the player when it is inside?

        // Animated opening of door
        //StartCoroutine(OpenDoorCO());
        // Clear on Exit

        return  seatedPositionTransform;
    }
            
    public bool Engage()
    {
        Debug.Log("Using Engine of Vehicle "+name);

        if(EngageNeededToEngage != null && !EngageNeededToEngage.Engaged) {
            Debug.Log("Can not engage Vehicle, prerequirements not met at "+EngageNeededToEngage.name);
            return false;
        }

        SoundMaster.Instance.PlaySound(SoundName.OpenDoor);

        vehicleAnimator.SetBool(EngineUseParameterName, true);

        Engaged = true;
        if(EngagedObjectActivated != null)
            EngagedObjectActivated?.SetActive(Engaged);

        return true;
    }
    public void DisEngage()
    {
        Debug.Log("Stopping Engine of Vehicle "+name);

        SoundMaster.Instance.PlaySound(SoundName.OpenDoor);

        vehicleAnimator.SetBool(EngineUseParameterName, false);

        Engaged = false;
        if (EngagedObjectActivated != null)
            EngagedObjectActivated?.SetActive(Engaged);

    }

    public void OpenDoorAnimate()
    {
        Debug.Log("Animate opening door");

        //GetComponent<Collider>().enabled = false;
        SoundMaster.Instance.PlaySound(SoundName.OpenDoor);
        //StartCoroutine(OpenDoorCO());
        //doorPart.transform.Rotate(0, -90, 0);
    }
    /*
    private IEnumerator OpenDoorCO()
    {
        float startRotation = doorPart.transform.rotation.eulerAngles.y;
        Quaternion startRotationQuaternion = doorPart.transform.localRotation;
        float currentRotation = startRotation;
        //Quaternion endRotationQuaternion = Quaternion.LookRotation(-doorPart.transform.right, Vector3.up);
        Quaternion endRotationQuaternion = startRotationQuaternion * Quaternion.AngleAxis(-95f, Vector3.up);
        Quaternion fullyOpenedRotationQuaternion = startRotationQuaternion * Quaternion.AngleAxis(-105f, Vector3.up);
            //Quaternion.RotateTowards(startRotationQuaternion,endRotationQuaternion,90+15f);
                    
        const float RotateTime = 0.2f;
        const float OpenDoorHitTime = 0.15f;
        float rotateTimer = 0;
        bool haveSlammed = false;
        // Opening rotation fast
        while(rotateTimer < RotateTime) {

            if(!haveSlammed && rotateTimer > OpenDoorHitTime) {
                Debug.Log(".. SLAM");
                SoundMaster.Instance.PlaySound(SoundName.OpenDoorHitWall);
                CameraShakeController.Instance.Shake(ShakeDataType.Door);
                haveSlammed = true;
            }

            rotateTimer += Time.deltaTime;
            doorPart.transform.localRotation = Quaternion.Lerp(startRotationQuaternion,fullyOpenedRotationQuaternion,rotateTimer/RotateTime);
            yield return null;
        }


        rotateTimer = 0;

        // Rotate back and stop
        while(rotateTimer < RotateTime) {
            rotateTimer += Time.deltaTime;
            doorPart.transform.localRotation = Quaternion.Lerp(doorPart.transform.localRotation, endRotationQuaternion, 0.1f);
            yield return null;
        }
        doorPart.transform.localRotation = endRotationQuaternion;            
    }
    */
}
