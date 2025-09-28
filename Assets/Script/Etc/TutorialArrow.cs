using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    [Header("Tutorial Arrow on Player")]
    [SerializeField] private Transform player;          
    [SerializeField] private GameObject playerArrow;
    
    private void Update()
    {
        if (GameManager.Instance.currentData != null && playerArrow.activeSelf)
        {
            UpdatePlayerArrow();
            transform.position = player.position;
        }
    }

    private void UpdatePlayerArrow()
    {
        Vector3 direction = GameManager.Instance.currentData.targetPos.position - player.position;
        direction.y = 0; 

        if (direction.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            playerArrow.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            Vector3 offset = Vector3.up * 0.5f; 
            transform.position = player.position + offset;
        }
    }

}