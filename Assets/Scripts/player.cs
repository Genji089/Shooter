using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    Animator animator;
    public float moveSpeed;

    bool isHold;
    int direction;
    bool isShoot;
    bool isMove;

    void Awake()
    {
        isHold = false;
        direction = 0;
        isShoot = false;
        isMove = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*float h = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime, Space.World);

        float v = Input.GetAxisRaw("Vertical");
        transform.Translate(Vector3.up * v * moveSpeed * Time.deltaTime, Space.World);

        int direction = (int)h;
        if(direction == 1 || direction == -1)
        {
            animator.SetInteger("run", direction);
            isHold = true;
        }
        if (direction == 0 && isHold == true)
        {
            animator.SetInteger("run", 0);
            isHold = false;
        }*/
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            Debug.Log("A Down");
            isMove = true;
            direction = -1;
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            isMove = true;
            direction = 1;
        }
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if(direction == -1)
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

        if (Input.GetKeyDown(KeyCode.J))
        {
            isShoot = true;
            animator.SetBool("isShoot", true);
            transform.Translate(new Vector3(0.25f * animator.GetInteger("run"), 0, 0), Space.World); //修补动画
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            isShoot = false;
            animator.SetBool("isShoot", false);
            transform.Translate(new Vector3(0.25f * animator.GetInteger("run"), 0, 0), Space.World);
        }
        
    }
}
