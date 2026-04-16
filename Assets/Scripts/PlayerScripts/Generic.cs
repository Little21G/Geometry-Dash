using UnityEngine;

static public class Generic
{
    static public void VelocityLimit(float limit, Rigidbody2D rb)
    {
        int gravityMultiplyer = (int)(Mathf.Abs(rb.linearVelocity.y));
        if (rb.linearVelocity.y * -gravityMultiplyer > limit)
            rb.linearVelocity = Vector2.up * -limit * gravityMultiplyer;
    }
    static public void createGamemode(Rigidbody2D rb, Movement host, bool onGroundRequired, float initialVelocity, float gravityScale, bool canHold = false, bool flipOnClick = false, float rotationMod = 0, float yVelocityLimit = Mathf.Infinity)
    {
        if(!Input.GetMouseButton(0) || canHold && host.OnGround())
            host.clickProcessed = false;

        rb.gravityScale = gravityScale * host.Gravity;
        VelocityLimit(yVelocityLimit, rb);

        if(Input.GetMouseButton(0))
        {
            if(host.OnGround() && !host.clickProcessed|| !onGroundRequired && !host.clickProcessed)
            {
                host.clickProcessed = true;
                rb.linearVelocity = Vector2.up * initialVelocity * host.Gravity;
                host.Gravity *= flipOnClick ? -1 : 1;
            }
        }
        if(host.OnGround() || !onGroundRequired)
        host.Sprite.rotation = Quaternion.Euler(0, 0, Mathf.Round(host.Sprite.rotation.z/90)*90);
        else
            host.Sprite.Rotate(Vector3.back, rotationMod * Time.deltaTime * host.Gravity);
    }
}
