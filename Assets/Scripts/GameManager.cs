using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public static int gold = 150;
    public static int baseHitPoint = 1000;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] float holdDuration = 1.5f;
    private bool isHolding = false;
    private float holdStartTime;
    public static float gameTime;
    public static float score;
    public static bool isAlive;


    private void Start()
    {
        gold = 150;
        gameTime = Time.time;
        score = 0;
        isAlive = true;
        baseHitPoint = 1000;
        
    }


    private void Update()
    {
        
        goldText.text = gold.ToString();
        
        if (Input.GetMouseButtonDown(0))
        {
            isHolding = true;
            holdStartTime = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;
        }

        if(isHolding && Time.time - holdStartTime > holdDuration)
        {

            buildingMenu();
            isHolding = false;
        }

        //Restart Scene veya Sonraki Scene geçme gelmeli aþaðýdaki son iki kod bloðu çalýþmuýyor þuan

        if (isAlive == false || baseHitPoint == 0)
        {
            EndGameScript.finalScore = score;
            gameTime = Time.time - gameTime;

            SceneManager.LoadScene(2);
        }
    }

    private void buildingMenu()
    {
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

        if(hit.collider != null && hit.collider.CompareTag("Building"))
        {
            hit.collider.gameObject.transform.Find("BuildingCanvas").gameObject.SetActive(true);
        }
    }
    
    
}
