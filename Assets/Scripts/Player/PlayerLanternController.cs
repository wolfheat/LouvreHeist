using UnityEngine;

public class PlayerLanternController : MonoBehaviour
{
    private const float NormalIntensity = 0.7f;
    private const float VehicleIntensity = 0.05f;


    [SerializeField] private Light spotLight;

    public void SetVehicleIntensity() => spotLight.intensity = VehicleIntensity;

    public void SetNormalIntensity() => spotLight.intensity = NormalIntensity;
}
