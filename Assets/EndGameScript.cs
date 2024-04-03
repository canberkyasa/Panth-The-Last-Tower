using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class EndGameScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeAndWaveText;
    public static float finalScore;
    public static int lastWave = 1;

    private void Start()
    {
        scoreText.text = string.Format("Your Score: {0}",finalScore.ToString());
        timeAndWaveText.text = string.Format("You last {0} seconds, and died at wave {1}", GameManager.gameTime.ToString(), lastWave.ToString());

    }

    void Update()
    {
        
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(0);
        }
    }

    

}
