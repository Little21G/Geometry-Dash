using System;
using Unity.VisualScripting;
using UnityEngine;

public enum Speeds { Slow = 0, Normal = 1, Fast = 2, Faster = 3, Fastest = 4 };
public enum Gamemodes { Cube = 0, Ship = 1, Ball = 2, UFO = 3, Wave = 4, Robot = 5, Spider = 6};

public class Movement : MonoBehaviour
{
    public Speeds CurrentSpeed;
    public Gamemodes CurrentGamemode;
    //.                      0.     1.      2.     3.     4
    float[] SpeedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };
    [System.NonSerialized] public int[] screenHeightValues = { 11, 10, 8, 10, 10, 11, 9 };
    [System.NonSerialized] public float yLastPortal = -2.3f;

    public float GroundCheckRadius;
    public LayerMask GroundMask;
    public Transform Sprite;

Rigidbody2D rb;

public int Gravity = 1;
public bool clickProcessed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
        Invoke(CurrentGamemode.ToString(), 0);
    } 

   public bool OnGround()
    {
        return Physics2D.OverlapBox(transform.position + Vector3.down * Gravity * 0.5f, Vector2.right * 1.1f + Vector2.up * GroundCheckRadius, 0, GroundMask);
    }

    bool TouchingWall()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right * 0.55f), Vector2.up *0.8f + (Vector2.right * GroundCheckRadius), 0, GroundMask);
    }

    void Cube()
    {
        Generic.createGamemode(rb, this, true, 19.5269f, 9.057f, true, false, 409.1f);
    }

    void Ship()
    {
       rb.gravityScale = 2.93f * (Input.GetMouseButton(0) ? -1 : 1) * Gravity;
       Generic.VelocityLimit(9.95f, rb);
        transform.rotation = Quaternion.Euler(0, 0, rb.linearVelocity.y * 2);
    }

    void Ball()
    {
        Generic.createGamemode(rb, this, true, 0f, 6.2f, false, true);
    }

    void UFO()
    {
        Generic.createGamemode(rb, this, false, 10.841f, 4.1483f, false, false, 0, 10.841f);
    }

    void Wave()
    {
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(0, SpeedValues[(int)CurrentSpeed] * (Input.GetMouseButton(0) ? 1 : -1) * Gravity);
    }

    float robotXstart = -100;
    bool onGroundProcessed;
    bool gravityFlipped;

    void Robot()
        {
            if (!Input.GetMouseButton(0))
            clickProcessed = false;

            if(OnGround() && !clickProcessed && Input.GetMouseButton(0))
        {
            gravityFlipped = false;
            clickProcessed = true;
            robotXstart = transform.position.x;
            onGroundProcessed = true;
        }

        if(Mathf.Abs(robotXstart - transform.position.x) <=3)
        {
            if(Input.GetMouseButton(0) && onGroundProcessed && !gravityFlipped)
            {
                rb.gravityScale = 0;
                rb.linearVelocityY = 10.4f * Gravity;
                return;

            }
        }

        else if (Input.GetMouseButton(0))
            onGroundProcessed = false;

            rb.gravityScale = 8.62f * Gravity;
            Generic.VelocityLimit(23.66f, rb);

        }

    void Spider()
    {
        Generic.createGamemode(rb, this, true, 238.29f, 6.2f, false, true, 0f, 238.29f);
    }
    public void ChangeThroughPortal( Gamemodes Gamemode, Speeds Speed, int gravity, int State, float yPortal)
    {
       switch(State)
        {
            case 0: 
                CurrentSpeed = Speed;
                break;
            case 1:
            yLastPortal = yPortal;
                CurrentGamemode = Gamemode;
                break;
            case 2:
                Gravity = gravity;
                rb.gravityScale = Mathf.Abs(rb.gravityScale) * (int)gravity;
                gravityFlipped = true;
                break;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PortalScript portal = collision.gameObject.GetComponent<PortalScript>();
        if(portal)
            portal.initiatePortal(this);
    }
}
