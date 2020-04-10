using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2D;
    BoxCollider2D bc2D;

    public float moveSpeed;
    public float hp;
    public float hitTime;
    public float hitSpeed; //被击退的速度
    public float hitBackLength;
    public float DestroyTime; //相当于die后尸体存留时间
    public AudioClip audioHit;

    int direction;
    int directionLast; //存放上一次的direction
    float timeCounter;
    bool isHit;
    bool isDie;
    Vector3 targetPosition;

    void Awake()
    {
        direction = -1;
        timeCounter = 0.0f;
        directionLast = 0;
        isHit = false;
        isDie = false;
        targetPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (hp == 0)
        {
            Die();
        }
        else
        {
            if (isHit)
            {
                animator.SetBool("isHit", true);
                if (timeCounter < hitTime)//被打中，停下，进入isHit状态
                {
                    timeCounter += Time.fixedDeltaTime;
                    if (directionLast == 0)
                    {
                        directionLast = direction;
                    }
                    direction = 0;
                    HitBack(transform.position, targetPosition);
                }
                else
                {
                    timeCounter = 0.0f;
                    direction = directionLast;
                    directionLast = 0;
                    isHit = false; //解除状态
                    animator.SetBool("isHit", false);
                }
            }
            //Debug.Log("direction:" + direction);
            //Debug.Log("directionLast:" + directionLast);

            //模型转向
            if (direction == -1 || directionLast == -1)
            {
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
            if (direction == 1 || directionLast == 1)
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }

            transform.Translate(Vector3.right * moveSpeed * direction * Time.fixedDeltaTime, Space.World);
        }
    }

    void Die()
    {
        isDie = true;
        animator.SetBool("isDie", true);
        Destroy(rb2D);
        Destroy(bc2D);
        Destroy(gameObject, DestroyTime);
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    void Hit(int bulletDirection)
    {
        isHit = true;
        //AudioSource.PlayClipAtPoint(audioHit,transform.localPosition);
        hp -= 1;
        if(bulletDirection == 1)
        {
            targetPosition = new Vector3(transform.position.x + hitBackLength, transform.position.y, transform.position.z);
        }
        else
        {
            targetPosition = new Vector3(transform.position.x - hitBackLength, transform.position.y, transform.position.z);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        direction = -direction;
    }

    void HitBack(Vector3 startPosition, Vector3 endPosition)
    {
        float JourneyLength = Vector3.Distance(endPosition, startPosition);
        float distCover = Time.deltaTime * hitSpeed;
        float fracJourney = 0.0f;
        if(JourneyLength == 0)
        {
            Debug.LogError("JourneyLength(hitBackLength) is 0.");
            return;
        }
        else
        {
            fracJourney = distCover / JourneyLength;
        }
        transform.position = Vector3.Lerp(startPosition, endPosition, fracJourney);
    }
}
