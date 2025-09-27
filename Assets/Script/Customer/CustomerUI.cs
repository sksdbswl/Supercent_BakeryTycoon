using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CustomerUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Balloon; 
    
    public GameObject Want;    
    public TMP_Text textMesh;      

    public GameObject Cashier;  
    public GameObject Eat;   
    public ParticleSystem Like; 
    
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        Balloon.SetActive(false);
    }

    void LateUpdate()
    {
        // 항상 카메라를 향하도록
        Balloon.transform.forward = mainCamera.transform.forward;
        Like.transform.forward = mainCamera.transform.forward;

        // 거리 보정
        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
        float scaleFactor = distance * 0.1f;
        Balloon.transform.localScale = Vector3.one * scaleFactor;
        Like.transform.localScale = Vector3.one * scaleFactor;
    }

    /// <summary>
    /// 원하는 빵 개수 표기
    /// </summary>
    public void SetBreadCount(int count)
    {
        Balloon.SetActive(true);
        textMesh.text = count.ToString();
    }

    /// <summary>
    /// 상태 변경
    /// </summary>
    public void OnSprite(GameObject obj)
    {
        Want.SetActive(false);
        Cashier.SetActive(false);
        Eat.SetActive(false);
        Like.gameObject.SetActive(false);
        
        obj.SetActive(true);
    }
    
    /// <summary>
    /// 행동 종료 -> Like
    /// </summary>
    public void OnLike()
    {
        Balloon.SetActive(false);
        Like.gameObject.SetActive(true);
        Like.Play();
    }
}