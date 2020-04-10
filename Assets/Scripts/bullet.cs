using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float changeSpeed;
    public float flySpeed;
    public float maxScaleX;
    bool nothing;
    int direction;
    // Start is called before the first frame update
    void Start()
    {
        nothing = true;
        direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.rotation.y);
        //Debug.Log(transform.eulerAngles);
        if(transform.localScale.x < maxScaleX)
        {
            transform.localScale = new Vector3(transform.localScale.x + (changeSpeed * Time.deltaTime), 2.0f, 1.0f);
        }
        if(transform.eulerAngles.y == 180)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        transform.Translate(Vector3.right * flySpeed * Time.deltaTime * direction, Space.World);
        if (nothing)
        {
            Destroy(gameObject, 2.0f);
            nothing = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "man":
                break;
            case "enemy":
                collider.SendMessage("Hit",direction);
                Destroy(gameObject);
                break;
            case "wall":
                break;
            case "platform":
                Destroy(gameObject);
                break;
            default:
                Debug.Log("unknown thing");
                break;
        }
    }
}
