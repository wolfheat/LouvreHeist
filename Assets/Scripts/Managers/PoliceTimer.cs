using System;
using TMPro;
using UnityEngine;

public class PoliceTimer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject timerHolder;

    private float timer;

    // If police alerted once dont restart timer
    public bool PoliceAlerted { get; private set; }


    public static PoliceTimer Instance { get; private set; }

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
        // Start Deactivated
        Reset();
    }

    public void Reset()
    {
        timerHolder.SetActive(false);        
        PoliceAlerted = false;
    }

    public void TryAlert()
    {
        if (PoliceAlerted) return;
        PoliceAlerted = true;
        timerHolder.SetActive(true);
        // Set the Time due to the base timer and any additional dragon
        timer = Stats.Instance.CurrentAlertTime;
    }

    private void Update()
    {
        if (PoliceAlerted) {
            timer -= Time.deltaTime;
            int min = (int)timer / 60;
            int sec = (int)timer - min * 60;
            timerText.text = min + ":" + sec;

            if(timer <= 0) {
                Debug.Log("Player was arrested");
                // Show Lose screen
                UIController.Instance.ShowDeathScreenInstant();
                Stats.Instance.IsDead = true;
            }

        }
    }
}
