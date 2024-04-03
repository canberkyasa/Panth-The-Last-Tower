using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    [Header("Tower Attributes")]
    public int towerHitPoint = 500;
    public GameObject projectilePrefab; 
    public float shootCooldown = 2f;
    public float detectionDistance = 10f;
    public GameObject target;
    public float lastShotTime;
    [SerializeField] AudioClip fireBall;
    private AudioSource _audioSource;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        //gameObject.transform.Find("TowerCanvas").gameObject.SetActive(true);
    }
    
    
    
    void Update()
    {
        pickTarget();
        //float distanceToTarget = Vector2.Distance(transform.position,target.transform.position);
        
        if(target!=null)
        {   
            shootTowardsTarget();
        }
    }
    void pickTarget()
    {   
        Collider2D[] collidersInDistance = Physics2D.OverlapCircleAll(transform.position, detectionDistance);

        float closestDistance=Mathf.Infinity;
        GameObject closestEnemy=null;

        foreach(Collider2D collider in collidersInDistance)
        {   
            if(collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position,collider.transform.position);
                float distanceToTower = Vector2.Distance(transform.position, collider.transform.position);
                
                if(distance<closestDistance)
                {  
                    closestDistance=distance;
                    closestEnemy=collider.gameObject;
     
                }

            }

        }
        if (closestEnemy != null && closestDistance <= detectionDistance)
        {
            target = closestEnemy;
        }
        else
        {
            target = null;
        }

    }
    void shootTowardsTarget()
    {   
        if (target!=null&&Time.time - lastShotTime >= shootCooldown)
        {

            PlayFireBallSound();
            
            float distanceToTarget = Vector2.Distance(transform.position,target.transform.position);
            FollowProjectile followProjectileScript=projectilePrefab.GetComponent<FollowProjectile>();
            if (followProjectileScript != null)
            {   
                followProjectileScript.SetTowerAndTarget(this.transform,detectionDistance,this.name,target.transform,target.name);
                
            }
            
            Transform topEdgeTransform= transform.Find("topEdge");
            Vector3 topEdgePosition = topEdgeTransform.position;
            
            GameObject homingProjectile = Instantiate(projectilePrefab, topEdgePosition, Quaternion.identity);
            lastShotTime = Time.time;
        }
            
    }
    
    
    private void OnDrawGizmosSelected() //builtin method, Objeyi sectiginde gizmoyu gosteriyor. 
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }

    public void sellOnClick()
    {
        
        float percentage = towerHitPoint / 500f;
        GameManager.gold += Convert.ToInt32(percentage * 50);
        Destroy(gameObject);
    }

    public void ignoreOnClick()
    {
        transform.Find("BuildingCanvas").gameObject.SetActive(false);
    }
    
    void PlayFireBallSound()
    {
        _audioSource.clip = fireBall;
        _audioSource.Play();
    }
    /*void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && canShoot)
        {
            
            Debug.Log("shootinggg");
            Shoot(other.transform);
            canShoot=false;
            
            Debug.Log("Waiting");
            InvokeRepeating("EnableShooting",shootCooldown,0.0f);
            //StartCoroutine(StartCooldown());
            
        }
    }
    void Shoot(Transform target)
    {
        Transform topEdgeTransform= transform.Find("topEdge");
        Vector3 topEdgePosition = topEdgeTransform.position;
        Vector3 direction = (target.position - topEdgePosition).normalized;
        

        GameObject projectile = Instantiate(projectilePrefab,topEdgePosition, Quaternion.identity);

        Rigidbody2D rb= projectile.GetComponent<Rigidbody2D>();

        rb.velocity = direction * projectileSpeed;


    }

    void EnableShooting()
    {
        canShoot = true;
        CancelInvoke("EnableShooting");
        
    }
    */
    
}
    

