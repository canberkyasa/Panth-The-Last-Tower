using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [SerializeField] float speed = 500.0f;
    private Rigidbody2D _rigidbody2D;
    private float horizantal;
    private float vertical;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 movement;
    public int healthPoint = 300;
    private float maxHealth;
    private float lastdamageTime;
    [SerializeField] float recoverTimeInSeconds = 10;
    [SerializeField] int recoveryRate = 5;
    [SerializeField] float timeBetweenIncreases = 0.2f;
    [SerializeField] Image heroHPBar;
    

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>(); //caching
        _animator = GetComponent<Animator>(); //caching
        _spriteRenderer = GetComponent<SpriteRenderer>();

        maxHealth = healthPoint;

        
    }


    // Update is called once per frame
    void Update()
    {
        healthBarUpdate();
        
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            ResetAllAnimatorParameters();
            _animator.SetBool("runSide",true);
            if(!_spriteRenderer.flipX == true)
            {
                _spriteRenderer.flipX = true;
            }
            
        } else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            ResetAllAnimatorParameters();
            _animator.SetBool("runSide", true);
            
            if (!_spriteRenderer.flipX == false)
            {
                _spriteRenderer.flipX = false;
            }
        } else if (Input.GetAxisRaw("Vertical") > 0)
        {
            ResetAllAnimatorParameters();
            _animator.SetBool("runUp", true);
        }else if (Input.GetAxisRaw("Vertical") < 0)
        {
            ResetAllAnimatorParameters();
            _animator.SetBool("runDown", true);
        }
        else
        {
            ResetAllAnimatorParameters(); //idle durum
        }
        horizantal = Input.GetAxisRaw("Horizontal"); // Klavyedeki Input deðerlerini alýyor (yatayda)
        vertical = Input.GetAxisRaw("Vertical"); // Donus yerine gore -1 , 1 veya 0 basar

        movement = new Vector2(horizantal, vertical).normalized; //normalize edilmezse carpraz hizlarda daha hizli gidiyor karakter.

    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = new Vector2(movement.x * speed * Time.deltaTime, movement.y * speed * Time.deltaTime); //K
    }
    

    void ResetAllAnimatorParameters()
    {
        AnimatorControllerParameter[] parameters = _animator.parameters;

        foreach(var parameter in parameters)
        {
            if(parameter.type == AnimatorControllerParameterType.Bool)
            {
                _animator.SetBool(parameter.name, false);
            }
        }
    }

    public void takeDamage(int damage)
    {
        if (healthPoint > damage)
        {
            healthPoint -= damage;
            lastdamageTime = Time.time;
        }
        else
        {
            GameManager.isAlive = false;
        }
    }

    void healthBarUpdate()
    {
        float currentHealth = healthPoint / maxHealth;
        float timeSinceLastDamage = Time.time - lastdamageTime;

        //
        if (healthPoint==maxHealth)
        {
            
            transform.Find("HeroCanvas").gameObject.SetActive(false);
            return;
        }
        else
        {
            transform.Find("HeroCanvas").gameObject.SetActive(true); 
            heroHPBar.fillAmount = currentHealth;
            //transform.Find("HeroHp").GetComponent<Image>().fillAmount = currentHealth;
        }

        if(timeSinceLastDamage > recoverTimeInSeconds) // belirli bir sure hasar alinmadiysa cani artsin.
        {
            if(Time.time >= lastdamageTime + timeBetweenIncreases)
            {
                recoverHealth();
                lastdamageTime += timeBetweenIncreases;
            }
        }

    }

    void recoverHealth()
    {
        if((healthPoint + recoveryRate) > maxHealth)
        {
            healthPoint = System.Convert.ToInt32(maxHealth);
        }
        else
        {
            healthPoint += recoveryRate;
        }
    }

    
}
