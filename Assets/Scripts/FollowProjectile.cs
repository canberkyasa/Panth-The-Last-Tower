using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowProjectile : MonoBehaviour
{
    public float speed = 5f;
    public Transform target;
    public Transform tower;
    public string nameTower;
    public float detectionRange;
    public string nameTarget;



    private void Start()
    {
        
    }

    void Update()
    {   
        
        if (target != null)
        {   
            HomingMove();
        }
        else
        {   
            Destroy(gameObject);
        }
        
        
    }
    public void SetTowerAndTarget(Transform newTower, float newDetectionRange, string towerName, Transform newTarget, string targetName)
    {
        // Set instance-specific tower information
        tower = newTower;
        detectionRange = newDetectionRange;
        nameTower = towerName;

        // Set instance-specific target information
        target = newTarget;
        nameTarget = targetName;
    }
    void HomingMove()
    {
        
        
        float distanceToTower = Vector2.Distance(tower.position, target.position);
        if(target!=null&&distanceToTower<detectionRange)
        {   
            
            //Debug.Log(tower.position+" shooting at to"+target.name+"at position"+target.position);
            Vector2 direction = (target.position - transform.position).normalized;
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            //rotation to target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            //move to target
            transform.Translate(direction * speed * Time.deltaTime,Space.World);
            

            //transform.position = Vector2.MoveTowards(transform.position,target.transform.position,speed * Time.deltaTime);
      
            if (distanceToTower > detectionRange)
            {   
                //Debug.Log("destroying and distance is "+distanceToTower);
                Destroy(gameObject);
            }
            
            
            
        }
        
        
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Destroy the projectile and the enemy
            EnemyScript2 _enemyScript2 = other.gameObject.GetComponent<EnemyScript2>();

            if (_enemyScript2.hitPoint > 0)
            {
                _enemyScript2.hitPoint -= 50;
            }
            if (_enemyScript2.hitPoint <= 0)
            {
                Destroy(other.gameObject);
            }
            //Destroy(other.gameObject);
            Destroy(gameObject);
        }

    }
    
    
}

