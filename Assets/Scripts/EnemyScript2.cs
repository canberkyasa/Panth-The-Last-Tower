using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript2 : MonoBehaviour
{
    [Header("Enemy Attributes")]
    [SerializeField] float speed = 2f;
    [SerializeField] float detectionDistance = 10f;
    public int hitPoint = 100;
    public int attackPoint = 20;
    private GameObject target;
    private bool canDamage = true;
    [SerializeField] float secondsBetweenAttacks = 1f;
    [SerializeField] int enemyGold = 5;
    [SerializeField] AudioClip[] attackSounds;
    private AudioSource _audioSource;
    [SerializeField] int enemyPoint = 5;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        PickTarget();
        
        MoveTowardsTarget();
    }

    void PickTarget()
    {
        Collider2D[] collidersInDistance = Physics2D.OverlapCircleAll(transform.position, detectionDistance); //Cember icerisindeki collider'lari array'e atiyor.

        float closestDistance = Mathf.Infinity;
        
        foreach(Collider2D collider in collidersInDistance)
        {

            if (collider.CompareTag("Hero") || collider.CompareTag("Building"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = collider.gameObject;
                }
            }
            
        }
    }

    void MoveTowardsTarget()
    {
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime); //Update Methodu icinde diye *Time.deltaTime
        }
        else //Eger saldiracak ilgili tagli obje yoksa base'e doðru yürüsün.
        {
            Vector2 baseLocation = new Vector2(0f, 0f);
            transform.position = Vector2.MoveTowards(transform.position, baseLocation, speed * Time.deltaTime);
        }
        

        /* Gerekirse
        //Objenin Hedefe dogru rotasyonu
        Vector2 direction = target.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        */
    }

    private void OnDrawGizmosSelected() //builtin method, Objeyi sectiginde gizmoyu gosteriyor. 
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Hero") || collision.gameObject.CompareTag("Building"))
        {
            BuildingScript _buildingScript = collision.gameObject.GetComponent<BuildingScript>();
            TowerScript _towerScript = collision.gameObject.GetComponent<TowerScript>();
            BaseScript _baseScript = collision.gameObject.GetComponent<BaseScript>();
            CharacterController _characterController = collision.gameObject.GetComponent<CharacterController>();
            if (canDamage)
            {
                StartCoroutine(damage2Object(_towerScript, _buildingScript,_baseScript,_characterController));
            }

        }
    }
    
    IEnumerator damage2Object(TowerScript _towerScript, BuildingScript _buildingScript, BaseScript _baseScript,CharacterController _characterController)
    {
        canDamage = false;

        PlayAttackSound();
        if (_buildingScript != null)
        {
            if (_buildingScript.buildingHitPoint > attackPoint)
            {
                _buildingScript.buildingHitPoint -= attackPoint;
            }
            else
            {
                Destroy(target.gameObject);
            }

        }
        else if (_towerScript != null)
        {
            if (_towerScript.towerHitPoint > attackPoint)
            {
                _towerScript.towerHitPoint -= attackPoint;
            }
            else
            {
                Destroy(target.gameObject);
            }
        }else if (_baseScript != null)
        {
            if (GameManager.baseHitPoint >= attackPoint)
            {
                GameManager.baseHitPoint -= attackPoint;
            }
        }else if(_characterController != null)
        {
            if (_characterController.healthPoint >= attackPoint)
            {
                _characterController.takeDamage(attackPoint);
                //_characterController.healthPoint -= attackPoint;
            } //olme kismi daha yok eklenmesi gerek.
        }
        yield return new WaitForSeconds(secondsBetweenAttacks);

        canDamage = true;
    }

    private void OnDestroy()
    {
        GameManager.gold += enemyGold;
        GameManager.score += enemyPoint;
        
    }
    
    private void PlayAttackSound()
    {
        _audioSource.clip = attackSounds[Random.Range(0, attackSounds.Length)];
        _audioSource.Play();
    }
}
