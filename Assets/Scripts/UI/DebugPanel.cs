using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{

    // Debug Panel has 10 messages active
    Queue<DebugMessage> queue = new ();

    [SerializeField] private DebugMessage messagePrefab; 
    [SerializeField] private GameObject messageHolder;
    private const int MaxMessages = 15;

    public static DebugPanel Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    public void AddMessage(string messageText)
    {
        DebugMessage message = Instantiate(messagePrefab, messageHolder.transform);
        message.SetText(messageText);
        queue.Enqueue(message);
        if(queue.Count > MaxMessages)
            queue.Dequeue();
    }

}
