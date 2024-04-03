using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    [Header("Attributes of the object")]
    [SerializeField] private int coinPer5Seconds = 10;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("EarnGold", 5f,5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EarnGold()
    {
        GameManager.gold += coinPer5Seconds;

    }
}
