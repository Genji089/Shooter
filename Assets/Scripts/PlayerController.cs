using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2D;
    public float moveSpeed;
    public float jumpSpeed;
    public float floatHeight;
    public float distFootToGround;
    public Transform bullet;
    public Transform fire;
    public Transform myCamera;
    public float cameraSpeed;
    public float cameraOffsetX;
    public float cameraOffsetY;
    public float attackSpeed; //攻击速度 
    public float attackOffsetX;
    public float attackOffsetY;
    public AudioClip audioShoot;
    
    bool isHold;
    int direction;
    bool isShoot;
    bool isMove;
    bool isInSky;
    bool jump;
    float cd;
    float fireTime;
    bool isFire; //射击，火花出现
    Vector3 targetPosition;

    void Awake()
    {
        isHold = false;
        direction = 0;
        isShoot = false;
        isMove = false;
        jump = false;
        cd = 0.0f;
        fireTime = 0.0f;
        isFire = false;
        targetPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isMove = true;
            direction = -1;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            isMove = true;
            direction = 1;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (direction == -1)
            {
                direction = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (direction == 1)
            {
                direction = 0;
            }
        }

        animator.SetInteger("run", direction);
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime, Space.World);

        if (direction == -1)
        {
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        if(direction == 1)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }

        //相机移动
        if (transform.position.x < 0)
        {
            targetPosition = new Vector3(0.0f, transform.position.y, myCamera.position.z);
        }
        else
        {
            //人物转向相机变化
            /*if(transform.eulerAngles.y == 180.0f)
            {
                targetPosition = new Vector3(transform.position.x - cameraOffsetX, transform.position.y, myCamera.position.z);
            }
            else
            {
                targetPosition = new Vector3(transform.position.x + cameraOffsetX, transform.position.y, myCamera.position.z);
            }*/
            targetPosition = new Vector3(transform.position.x + cameraOffsetX, transform.position.y, myCamera.position.z);
        }
        CameraMove(targetPosition, Vector3.Distance(targetPosition, myCamera.position));


        if (Input.GetKeyDown(KeyCode.J))
        {
            isShoot = true; //触发射击
            animator.SetBool("isShoot", true);
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            isShoot = false;
            animator.SetBool("isShoot", false);
        }

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isInSky == false)  
        {
            jump = true; //用于触发跳跃
            isInSky = true;
        }
        animator.SetBool("isJump", isInSky);
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetTrigger("die");
        }
        //Debug.Log(direction);
    }

    void FixedUpdate()
    {
        //Debug.Log(Time.fixedDeltaTime);
        if(cd <= 1 / attackSpeed)
        {
            cd += Time.fixedDeltaTime;
        }

        if (jump)
        {
            rb2D.velocity = new Vector3(0.0f, jumpSpeed, 0.0f);
            jump = false;
        }

        //脚下射线判断是否在地面
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y - floatHeight), -Vector2.up);
        if (hit.collider != null)
        {
            float distance = Mathf.Abs(hit.point.y - transform.position.y + floatHeight);
            //Debug.Log(distance);
            if(distance <= distFootToGround)
            {
                isInSky = false;
                //Debug.Log("on the ground");
            }
        }

        //Attack
        if (isShoot)
        {
            if(cd >= 1 / attackSpeed)
            {
                //Debug.Log(transform.rotation.y);
                if(transform.eulerAngles.y == 180.0f)
                {
                    Instantiate(bullet, new Vector3(transform.position.x - attackOffsetX, transform.position.y + attackOffsetY, 0.0f), transform.rotation);
                    //Instantiate(fire, new Vector3(transform.position.x - attackOffsetX - 0.6f, transform.position.y + attackOffsetY, 0.0f), transform.rotation);
                    fire.transform.position = new Vector3(transform.position.x - attackOffsetX - 0.6f, transform.position.y + attackOffsetY, 0.0f);
                    fire.transform.rotation = transform.rotation;
                }
                else
                {
                    Instantiate(bullet, new Vector3(transform.position.x + attackOffsetX, transform.position.y + attackOffsetY, 0.0f), transform.rotation);
                    //Instantiate(fire, new Vector3(transform.position.x + attackOffsetX + 0.6f, transform.position.y + attackOffsetY, 0.0f), transform.rotation);
                    fire.transform.position = new Vector3(transform.position.x + attackOffsetX + 0.6f, transform.position.y + attackOffsetY, 0.0f);
                    fire.transform.rotation = transform.rotation;
                }
                fire.transform.GetComponent<SpriteRenderer>().enabled = true;
                isFire = true;

                AudioSource.PlayClipAtPoint(audioShoot, transform.localPosition);
                cd = 0.0f;
            }
        }

        //火花计时，fireTime最大值就是火花存在的时间
        if (isFire)
        {
            fireTime += Time.fixedDeltaTime;
            if(fireTime >= 0.06f)
            {
                fire.transform.GetComponent<SpriteRenderer>().enabled = false;
                fireTime = 0.0f;
                isFire = false;
            }
        }
    }

    void Attack()
    {
        
    }

    void CameraMove(Vector3 targetPosition, float journeyLength)
    {
        float distCovered = Time.deltaTime * cameraSpeed;
        float fracJourney = 0.0f;
        if(journeyLength == 0)
        {
            return;
        }
        else
        {
            fracJourney = distCovered / journeyLength;
        }
        myCamera.position = Vector3.Lerp(myCamera.position, targetPosition, fracJourney);
    }
}
