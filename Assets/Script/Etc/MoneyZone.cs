using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyZone : MonoBehaviour
{
    public Transform zonePoint;
    public GameObject moneyPrefab;

    private Stack<GameObject> moneyStack = new Stack<GameObject>();

    public float Xspacing = 0.8f;    // x간격
    public float Yspacing = 0.5f;    // y간격
    public float height = 0.2f;     // 층 높이
    public int maxPerLayer = 9;     // 3x3 = 9개
    public float collectDelay = 0.1f; // 흡수 간격

    public void SpawnMoney(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int index = moneyStack.Count;
            int layer = index / maxPerLayer;
            int slot = index % maxPerLayer;

            int row = slot / 3;
            int col = slot % 3;

            Vector3 offset = new Vector3(col * Xspacing, layer * height, -row * Yspacing);
            
            GameObject money = GenericPoolManager.Instance.Get(moneyPrefab, zonePoint.position + offset, Quaternion.identity, zonePoint);
            
            money.transform.Rotate(0, 90f, 0);
            money.SetActive(true);
            
            moneyStack.Push(money);
        }
    }

    public GameObject PopMoney()
    {
        if (moneyStack.Count == 0) return null;
        return moneyStack.Pop();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        
        if (player)
        {
            StartCoroutine(CollectMoneyCoroutine(player));
        }
    }

    private IEnumerator CollectMoneyCoroutine(Player player)
    {
        while (moneyStack.Count > 0)
        {
            GameObject money = PopMoney();
            if (money != null)
            {
                player.AddMoney(1);
                StartCoroutine(GameManager.Instance.MoveTo(money, player.transform, 0.5f));
                yield return new WaitForSeconds(collectDelay);
            }
        }
    }
}