using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Enemy Details")]
    public List<Enemy> enemies = new List<Enemy>();

    [Header("Wave Details")]
    [SerializeField] int currWave;
    private int waveValue;
    [SerializeField] TextMeshProUGUI currentWave;


    public Transform[] spawnLocations;
    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateWave();
        InvokeRepeating("nextWaveControl", 1f, 5f);
    }


    void FixedUpdate()
    {
        if(spawnTimer <= 0)
        {
            //spawn an enemy
            if(enemiesToSpawn.Count > 0)
            {
                Instantiate(enemiesToSpawn[0], spawnLocations[Random.Range(0,spawnLocations.Length)]); //spawn first enemy in our list
                enemiesToSpawn.RemoveAt(0);//and remove it
                spawnTimer = spawnInterval;
            }
            else
            {
                waveTimer = 0;
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
    }

    public void GenerateWave()
    {
        currentWave.text = string.Format("Wave: {0}", currWave.ToString());
        EndGameScript.lastWave = currWave;
        if (currWave <= 2)
        {
            waveValue = currWave * 15;
        }
        else if(currWave <=5)
        {
            waveValue = currWave * 20;
        }
        else if(currWave <=10)
        {
            waveValue = currWave * 30;
        }
        else
        {
            waveValue = currWave * 100;
        }
        
        GenerateEnemies();


        if (enemiesToSpawn.Count > 0)
        {
            if (waveDuration / enemiesToSpawn.Count >= 0.25f)
            {
                spawnInterval = waveDuration / enemiesToSpawn.Count; // interval between spawns. t/enemies
            }
            else
            {
                spawnInterval = 0.25f; // default to 0.25f 
            }
        }
        
        waveTimer = waveDuration; //wave duration is read only
    }

    public void GenerateEnemies()
    {

        //Create a temporary list of enemies to generate
        //
        // in a loop grab a random enemy, see if we can afford it
        // if we can, add it to our list, and deduct the cost ((REPEATTT)) 
        // -> if we have no points left, leave the loop

        

        List<GameObject> generatedEnemies = new List<GameObject>();
        
        if (currWave < 3)
        {
            while (waveValue > 0)
            {
                int randEnemyId = 0;
                int randEnemyCost = enemies[randEnemyId].cost;

                if (waveValue - randEnemyCost >= 0)
                {
                    generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                    waveValue -= randEnemyCost;

                }
                else if (waveValue <= 1)
                {
                    break;
                }
            }
        }
        else
        {
            while (waveValue > 0)
            {
                int randEnemyId = Random.Range(0, enemies.Count);
                int randEnemyCost = enemies[randEnemyId].cost;

                if (waveValue - randEnemyCost >= 0)
                {
                    generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                    waveValue -= randEnemyCost;

                }
                else if (waveValue <= 1)
                {
                    break;
                }
            }
        }

        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;

    }

    [System.Serializable]
    public class Enemy
    {
        public GameObject enemyPrefab;
        public int cost;
    }

    private void nextWaveControl()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            currWave++;
            StartCoroutine(DenemeCoroutine());
            GenerateWave();

        }
    }
    IEnumerator DenemeCoroutine()
    {
        yield return new WaitForSeconds(15);
    }
}
