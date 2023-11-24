using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private Rigidbody2D rb;

    private float original_speed = 3f;
    private float speed_x = 0f;
    private float speed_y = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isRunning){
            rb.velocity = new Vector2(speed_x, speed_y);
        }else{
            rb.velocity = Vector2.zero;
        }
    }

    public void Update_Speed(string direction)
    {
        if (direction == "top")
        {
            speed_x = 0f;
            speed_y = original_speed;
        }
        else if (direction == "bottom")
        {
            speed_x = 0f;
            speed_y = -original_speed;
        }
        else if (direction == "right")
        {
            speed_x = original_speed;
            speed_y = 0f;
        }
        else if (direction == "left")
        {
            speed_x = -original_speed;
            speed_y = 0f;
        }
        else 
        {
            speed_x = 0f;
            speed_y = 0f;
        }
    }
}
