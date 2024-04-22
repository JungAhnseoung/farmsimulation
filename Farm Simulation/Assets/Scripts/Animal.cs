using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Animal : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private BoxCollider2D playerBoxCollider;
    private SpriteRenderer spriteRenderer;

    private string[] animationTriggers = { "isItching", "isLaying", "isMeowing", "isLicking", "isWalking", "isRunning", "isStretching" };
    private string randomTrigger = null;

    private bool isAnimationPlaying = false;
    private bool isMovingRight = false;
    private bool isMovingLeft = false;
    private bool isMoving = false;
    
    private float speed = 2f;
    private bool isInAnimalZone = false;
    public int itemNo;
    
    private void OnEnable()
    {
        EventHandler.AnimalItemDropEvent += SpawnAnimalItem;
    }

    private void OnDisable()
    {
        EventHandler.AnimalItemDropEvent -= SpawnAnimalItem;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = false;
        isInAnimalZone = false;

        InitalizeCollider();
        StartCoroutine(RandomAnimation());
    }


    void FixedUpdate()
    {
        if (isMoving)
        {
            if(isMovingRight)
            {
                rigidbody.MovePosition(transform.position + Vector3.right * speed * Time.deltaTime);
            }
            else if(isMovingLeft)
            {
                rigidbody.MovePosition(transform.position + Vector3.left * speed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "AnimalZone")
        {
            isInAnimalZone = true;
        }
    }

    private void InitalizeCollider()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        boxCollider.isTrigger = false;
        boxCollider.size = new Vector2(0.3f, 0.3f);

        playerBoxCollider = FindObjectOfType<Player>().GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerBoxCollider, boxCollider);
    }

    private void SpawnAnimalItem()
    {
        if(isInAnimalZone)
        {
            if(itemNo == 1031) ItemSave.SpawnItemInScene(1001, new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), 0f));
            else if(itemNo == 1032) ItemSave.SpawnItemInScene(1009, new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), 0f));
            AnimalManager.isItemReady = false;
        }
    }

    IEnumerator RandomAnimation()
    {
        while(true)
        {
            if(!isAnimationPlaying)
            {
                randomTrigger = animationTriggers[Random.Range(0, animationTriggers.Length)];
                animator.SetTrigger("isWalking");
                isAnimationPlaying = true;
                int randomDirection = Random.Range(0, 2);

                if(randomDirection == 1)
                {
                    spriteRenderer.flipX = true;
                }
                else if(randomDirection == 0)
                {
                    spriteRenderer.flipX = false;
                }

                if (spriteRenderer.flipX)
                {
                    isMovingLeft = true;
                    isMovingRight = false;

                }
                else if (!spriteRenderer.flipX)
                { 
                    isMovingRight = true;
                    isMovingLeft = false;
                }
                if (randomTrigger == "isWalking" || randomTrigger == "isRunning") isMoving = true;
                isMoving = true;

            }
            yield return new WaitForSeconds(3f);
            animator.SetTrigger("isIdle");

            isMoving = false;
            isMovingRight = false;
            isMovingLeft = false;
            
            yield return new WaitForSeconds(5f);
            isAnimationPlaying=false; 
        }
    }
}
