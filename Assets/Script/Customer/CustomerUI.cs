using UnityEngine;
using TMPro;

public class CustomerUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Balloon; 
    
    public GameObject Want;    
    public TMP_Text textMesh;      

    public GameObject Cashier;  
    public GameObject Eat;   
    public GameObject Like; 
    
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        Balloon.SetActive(false);
    }

    void LateUpdate()
    {
        Balloon.transform.forward = mainCamera.transform.forward;
        Like.transform.forward = mainCamera.transform.forward;
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
        
        obj.SetActive(true);
    }
    
    
    /// <summary>
    /// 행동 종료 -> Like
    /// </summary>
    public void OnLike()
    {
        Balloon.SetActive(false);
        Like.SetActive(true);
    }
}