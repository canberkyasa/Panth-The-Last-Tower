using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHPBar : MonoBehaviour
{
    //private RectTransform rectTransform;
    private Image healthBarImage;
    private float maxHealth = 0f;
    // Start is called before the first frame update
    void Start()
    {
        healthBarImage = GetComponent<Image>();
        maxHealth = GameManager.baseHitPoint;
        //rectTransform = GetComponent<RectTransform>(); //caching
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        UpdateHealthBarStatus();
    }

    void UpdateHealthBarStatus()
    {
        healthBarImage.fillAmount = GameManager.baseHitPoint / maxHealth;
        
}
}
