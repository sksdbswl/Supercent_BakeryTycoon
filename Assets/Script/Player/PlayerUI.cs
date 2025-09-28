using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject MaxIcon;
    
    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        MaxIcon.transform.forward = mainCamera.transform.forward;
    }
}