using UnityEngine;
using Wolfheat.StartMenu;

public class AlertArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter " +other.name);
        if(other.TryGetComponent(out PlayerColliderController player)) {
            // PLayer entered an Alert area
            PoliceTimer.Instance.TryAlert();

            HelpInstructions.Instance.ShowInstruction("Alarm Triggered! Police incoming, get out in time!", HelpButtonType.Info);

            SoundMaster.Instance.PlaySound(SoundName.Teleport);
        }
    }
}
