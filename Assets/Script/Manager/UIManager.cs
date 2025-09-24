using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.OnMoneyChanged += UpdateMoneyUI;
        UpdateMoneyUI(player.Money); 
    }

    private void UpdateMoneyUI(int money)
    {
        moneyText.text = money.ToString();
    }
}