using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    public UnLock seat;
    public ParticleSystem cleanUp;
    public GameObject cheir;
    public GameObject table;
    public GameObject trash;
    public bool isCleared = true;
    
    private Quaternion originalRotation; 
    
    private void Awake()
    {
        seat.SetOccupied(false); 
        if(cheir != null)
            originalRotation = cheir.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null && !isCleared)
        {
            Debug.Log($"청소합니다 ! {isCleared}");
            isCleared = true;
            trash.SetActive(false);
            seat.SetOccupied(false); 
            cleanUp.gameObject.SetActive(true);
            cleanUp.Play();

            if(cheir != null)
                cheir.transform.rotation = originalRotation;
        }
    }

    public void Dirty()
    {
        Debug.Log($"더러워 졌어요 {isCleared}");
        isCleared = false;
        cleanUp.gameObject.SetActive(false);
        Quaternion rotated = originalRotation * Quaternion.Euler(0, 110, 0);
        cheir.transform.rotation = rotated;
        trash.SetActive(true);
    }
}