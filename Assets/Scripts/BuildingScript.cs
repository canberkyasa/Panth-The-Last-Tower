using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    [Header("Attributes of the object")]
    [SerializeField] private int coinPer5Seconds = 3;
    public int buildingHitPoint = 200;

    private void Start()
    {
        InvokeRepeating("EarnGold",0f,5f);
    }

    void EarnGold()
    {
        GameManager.gold += coinPer5Seconds;
        
    }
    public void sellOnClick()
    {

        float percentage = buildingHitPoint / 200f;
        GameManager.gold += Convert.ToInt32(percentage * 25);
        Destroy(gameObject);
    }

    public void ignoreOnClick()
    {
        transform.Find("BuildingCanvas").gameObject.SetActive(false);
    }
}
