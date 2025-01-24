using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Rigidbody2D ballrb;
    public bool inPlay;
    public Transform paddle;
    public float speed;
    public Transform Explosion;
    private GameManager gm;
    private GamestartScript gs;
    private GameManage4S g4s;
    // private EnemyPaddleFollow es;
    private BrickType bt;
    public Transform Powerup;
    private AudioSource audioSource;
    public AudioClip bounce;

    void Start()
    {
        gm = FindObjectOfType<GameManager>(); // Check if GameManager is present
        g4s = FindObjectOfType<GameManage4S>(); // Check if GameManage4S is present
        gs = FindObjectOfType<GamestartScript>();
        ballrb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        // es=FindObjectOfType<EnemyPaddleFollow>();
        bt=FindObjectOfType<BrickType>();
    }

    void Update()
    {
        // Check if the game is over
        if ((gm != null && gm.gameOver) || (g4s != null && g4s.gameOver))
        {
            return;
        }

        // Check if the game start panel is active
        if (gs != null && gs.GameStartPanel.activeInHierarchy)
        {
            return;
        }

        // Keep the ball on the paddle before launching
        if (!inPlay)
        {
            transform.position = paddle.position;
        }

        // Launch the ball
        if (Input.GetButtonDown("Jump") && !inPlay)
        {
            inPlay = true;
            ballrb.AddForce(Vector2.up * speed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Bottom"))
        {
            // Stop the ball and reset its position
            ballrb.velocity = Vector2.zero;
            inPlay = false;

            // Update lives depending on the active manager
            if (g4s != null)
            {
                g4s.UpdateLives(-1);
            }
            else if (gm != null)
            {
                gm.UpdateLives(-1);
            }
        }
        
    }

void OnCollisionEnter2D(Collision2D other)
{
     if (audioSource != null && bounce != null && !other.collider.CompareTag("Bottom"))
        {
            audioSource.PlayOneShot(bounce);
            Debug.Log("audioClip");
        }
    if (other.transform.CompareTag("Brick"))
    {
        BrickScript brickScript = other.gameObject.GetComponent<BrickScript>();
        BrickType brickType = other.gameObject.GetComponent<BrickType>();

        if (brickScript != null)
        {
            // Handle brick breaking logic
            if (brickScript.hitsToBreak > 1)
            {
                brickScript.BreakBrick();
            }
            else
            {
                // Spawn a power-up with a random chance
                int randomfall = Random.Range(1, 200);
                if (randomfall < 20)
                {
                    Instantiate(Powerup, other.transform.position, other.transform.rotation);
                }

                // Create explosion effect
                Transform newExplosion = Instantiate(Explosion, other.transform.position, other.transform.rotation);
                Destroy(newExplosion.gameObject, 2.5f);

                // Update score based on BrickId
                if (brickType != null)
                {
                    if (brickType.BrickId == 1)
                    {
                        // Player-side brick (enemy hits this, deduct player points)
                        if (g4s != null)
                        {
                            g4s.UpdateScore(-brickScript.points); // Deduct points
                            g4s.UpdateNumberOfBricks();
                        }
                    }
                    else if (brickType.BrickId == 2)
                    {
                        // Enemy-side brick (player hits this, add points)
                        if (g4s != null)
                        {
                            g4s.UpdateScore(brickScript.points); // Add points
                            g4s.UpdateNumberOfBricks();
                        }
                      
                    }
                }
                else
                {
                    // Default behavior for bricks without BrickType
                    if (g4s != null)
                    {
                        g4s.UpdateScore(brickScript.points);
                        g4s.UpdateNumberOfBricks();
                    }
                    else if (gm != null)
                    {
                        gm.UpdateScore(brickScript.points);
                        gm.UpdateNumberOfBricks();
                    }
                }

                // Destroy the brick
                Destroy(other.gameObject);
            }
           
        }
    }
}

} 