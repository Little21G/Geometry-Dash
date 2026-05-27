using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Speeds { Slow = 0, Normal = 1, Fast = 2, Faster = 3, Fastest = 4 };
public enum Gamemodes { Cube = 0, Ship = 1, Ball = 2, UFO = 3, Wave = 4, Robot = 5, Spider = 6 };
 
public class Movement : MonoBehaviour
{
    public Speeds CurrentSpeed;
    public Gamemodes CurrentGamemode;
    float[] SpeedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };
 
    public float GroundCheckRadius;
    public LayerMask GroundMask;
    public Transform Sprite; 

    [Header("Visuals (Sprite Swapping)")]
    public SpriteRenderer playerSpriteRenderer;
    public Sprite cubeSprite;
    public Sprite shipSprite;
 
    Rigidbody2D rb;
    public int Gravity = 1;
    public bool clickProcessed = false;

    [Header("Trail & Ambient Settings")]
    public ParticleSystem movementTrail; 
    public ParticleSystem ambientSpeedParticles; 

    [Header("Dynamic Background Particle Progression")]
    [Tooltip("How often (in seconds) the particles scale up and accelerate.")]
    [SerializeField] private float progressionInterval = 7.0f;

    [Tooltip("How much faster the particles fly to the left per interval step.")]
    [SerializeField] private float speedIncreasePerStep = 5.0f;

    [Tooltip("How much larger the speed line streaks grow per interval step.")]
    [SerializeField] private float sizeIncreasePerStep = 0.05f;

    [Tooltip("How many more speed particles spawn per second per interval step.")]
    [SerializeField] private float spawnRateIncreasePerStep = 8.0f;

    // Track original starting points so the modifiers stack smoothly on top of them
    private float progressionTimer = 0f;
    private int currentStepMultiplier = 0;
    private float baseMinSpeed;
    private float baseMaxSpeed;
    private float baseMinSize;
    private float baseMaxSize;
    private float baseSpawnRate;

    [Header("Death Settings")]
    public GameObject deathParticlesPrefab; 
    public AudioClip deathSound; 
    public AudioSource levelMusic; 
    [SerializeField] private float respawnDelay = 1.0f; 
    
    private bool isDead = false; 
 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (levelMusic != null)
        {
            levelMusic.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        }

        // Cache initial background particle values so we know where our baseline is
        if (ambientSpeedParticles != null)
        {
            var velocityModule = ambientSpeedParticles.velocityOverLifetime;
            baseMinSpeed = velocityModule.x.constantMin; // Usually around -25
            baseMaxSpeed = velocityModule.x.constantMax; // Usually around -40

            var mainModule = ambientSpeedParticles.main;
            baseMinSize = mainModule.startSize.constantMin; // Usually around 0.05
            baseMaxSize = mainModule.startSize.constantMax; // Usually around 0.15

            var emissionModule = ambientSpeedParticles.emission;
            baseSpawnRate = emissionModule.rateOverTime.constant; // Usually around 30
        }
    }

    void Update()
    {
        if (isDead) return;

        // Run progression math if background particles are hooked up
        if (ambientSpeedParticles != null)
        {
            progressionTimer += Time.deltaTime;

            if (progressionTimer >= progressionInterval)
            {
                progressionTimer = 0f;
                currentStepMultiplier++;
                ApplyParticleProgression();
            }
        }
    }
 
    void FixedUpdate()
    {
        if (isDead) return;

        transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
        Invoke(CurrentGamemode.ToString(), 0);
    }

    private void ApplyParticleProgression()
    {
        // 1. Scale Velocity Over Lifetime (Make them zoom faster to the left)
        var velocityModule = ambientSpeedParticles.velocityOverLifetime;
        float speedOffset = currentStepMultiplier * speedIncreasePerStep;
        // Keep values negative so they continue moving from right -> left
        velocityModule.x = new ParticleSystem.MinMaxCurve(baseMinSpeed - speedOffset, baseMaxSpeed - speedOffset);

        // 2. Scale Main Module Sizes (Make them visually thicker/longer)
        var mainModule = ambientSpeedParticles.main;
        float sizeOffset = currentStepMultiplier * sizeIncreasePerStep;
        mainModule.startSize = new ParticleSystem.MinMaxCurve(baseMinSize + sizeOffset, baseMaxSize + sizeOffset);

        // 3. Scale Emission Module (Make the air look denser with particles)
        var emissionModule = ambientSpeedParticles.emission;
        float totalNewRate = baseSpawnRate + (currentStepMultiplier * spawnRateIncreasePerStep);
        emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(totalNewRate);
    }
 
    public bool OnGround()
    {
        return Physics2D.OverlapBox(transform.position + Vector3.down * Gravity * 0.5f, Vector2.right * 1.1f + Vector2.up * GroundCheckRadius, 0, GroundMask);
    }
 
    bool TouchingWall()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right * 0.55f), Vector2.up * 0.8f + (Vector2.right * GroundCheckRadius), 0, GroundMask);
    }
 
    void Cube() { Generic.createGamemode(rb, this, true, 22.0f, 9.057f, true, false, 380f); }
    void Ship()
    {
        rb.gravityScale = 2.93f * (Input.GetMouseButton(0) ? -1 : 1) * Gravity;
        Generic.VelocityLimit(9.95f, rb);
        transform.rotation = Quaternion.Euler(0, 0, rb.linearVelocity.y * 2);
    }
    void Ball() { Generic.createGamemode(rb, this, true, 0, 6.2f, false, true); }
    void UFO() { Generic.createGamemode(rb, this, false, 12.5f, 4.1483f, false, false, 0, 12.5f); }
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
        if (!Input.GetMouseButton(0)) clickProcessed = false;
        if(OnGround() && !clickProcessed && Input.GetMouseButton(0))
        {
            gravityFlipped = false;
            clickProcessed = true;
            robotXstart = transform.position.x;
            onGroundProcessed = true;
        }
        if (Mathf.Abs(robotXstart - transform.position.x) <= 3)
        {
            if (Input.GetMouseButton(0) && onGroundProcessed && !gravityFlipped)
            {
                rb.gravityScale = 0;
                rb.linearVelocity = Vector2.up * 11.5f * Gravity;
                return;
            }
        }
        else if (Input.GetMouseButton(0)) onGroundProcessed = false;
        rb.gravityScale = 8.62f * Gravity;
        Generic.VelocityLimit(23.66f, rb);
    }
 
    void Spider() { Generic.createGamemode(rb, this, true, 238.29f, 6.2f, false, true, 0, 238.29f); }
 
    public void ChangeThroughPortal(Gamemodes Gamemode, Speeds Speed, int gravity, int State)
    {
        switch (State)
        {
            case 0: 
                CurrentSpeed = Speed; 
                break;
            case 1: 
                CurrentGamemode = Gamemode; 
                
                // Swap the picture based on the Gamemode!
                if (playerSpriteRenderer != null)
                {
                    if (Gamemode == Gamemodes.Cube && cubeSprite != null) playerSpriteRenderer.sprite = cubeSprite;
                    else if (Gamemode == Gamemodes.Ship && shipSprite != null) playerSpriteRenderer.sprite = shipSprite;
                }
                break;
            case 2: 
                Gravity = gravity; 
                rb.gravityScale = Mathf.Abs(rb.gravityScale) * gravity; 
                gravityFlipped = true; 
                break;
        }
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PortalScript portal = collision.gameObject.GetComponent<PortalScript>();
        if (portal) portal.initiatePortal(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.x < -0.5f)
            {
                Die(); 
            }
        }
    }

    public void Die()
    {
        if (isDead) return; 

        isDead = true; 
        CancelInvoke(); 

        if (movementTrail != null) movementTrail.Stop(); 
        if (ambientSpeedParticles != null) ambientSpeedParticles.Stop();

        if (levelMusic != null) levelMusic.Pause(); 

        if (deathSound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
        }

        if (Camera.main != null)
        {
            CameraFollow camScript = Camera.main.GetComponent<CameraFollow>();
            if (camScript != null) camScript.TriggerShake(); 
        }

        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        }

        if (Sprite != null) Sprite.gameObject.SetActive(false);
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        this.enabled = false; 

        AttemptManager.RegisterDeath();

        Invoke("ReloadScene", respawnDelay);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}