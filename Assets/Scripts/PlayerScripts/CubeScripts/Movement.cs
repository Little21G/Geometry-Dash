using System;
using Unity.VisualScripting;
using UnityEngine;

public enum Speeds { Slow = 0, Normal = 1, Fast = 2, Faster = 3, Fastest = 4 };
public enum Gamemodes { Cube = 0, Ship = 1}

public class Movement : MonoBehaviour
{
    public Speeds CurrentSpeed;
    public Gamemodes CurrentGamemode;
    //.                      0.     1.      2.     3.     4
    float[] SpeedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };

    public Transform GroundCheckTransform;
    public float GroundCheckRadius;
    public LayerMask GroundMask;
    public Transform Sprite;

Rigidbody2D rb;

int Gravity = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
      
        if(rb.linearVelocity.y < -24.2f)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -24.2f);

        Invoke(CurrentGamemode.ToString(), 0);
    } 

    bool OnGround()
    {
        return Physics2D.OverlapBox(GroundCheckTransform.position + Vector3.up - Vector3.up*(Gravity - 1/ -2), Vector2.right * 1.1f + Vector2.up * GroundCheckRadius, 0, GroundMask);
    }

    bool TouchingWall()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right * 0.55f), Vector2.up *0.8f + (Vector2.right * GroundCheckRadius), 0, GroundMask);
    }

    void Cube()
    {
       if(OnGround())
        {
            Vector3 Rotation = Sprite.rotation.eulerAngles;
            Rotation.z = Mathf.Round(Rotation.z/90)*90;
            Sprite.rotation = Quaternion.Euler(Rotation);

            //Jump
            if(Input.GetMouseButton(0))
            {
               rb.linearVelocity = Vector2.zero;
               rb.AddForce(Vector2.up * 26.6581f * Gravity, ForceMode2D.Impulse);
            }
        }
        else
        {
            Sprite.Rotate(Vector3.back * 452.4152186f * Time.deltaTime * Gravity);
        } 
    }

    public void ChangeThroughPortal( Gamemodes Gamemode, Speeds Speed, int gravity, int State)
    {
       switch(State)
        {
            case 0: 
                CurrentSpeed = Speed;
                break;
            case 1:
                CurrentGamemode = Gamemode;
                break;
            case 2:
                Gravity = gravity;
                rb.gravityScale = Mathf.Abs(rb.gravityScale) * (int)gravity;
                break;
        } 
    }
}
