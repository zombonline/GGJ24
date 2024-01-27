using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;


    private void Awake()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (transform.position.x > 8.5f)
        {
            transform.position = new Vector2(8.5f, transform.position.y);
        }
        else if( transform.position.x < -8.5f)
        {
            transform.position = new Vector2(-8.5f, transform.position.y);
        }
    }

}
