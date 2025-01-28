using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller_s : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 4f;
    public float min_x, max_x, min_y, max_y;
    [SerializeField]
    private GameObject Player_bullet;


    public AudioClip laserSound;
    private AudioSource audioSource;

    [SerializeField]
    private Transform Spawn_point;


    public float ShootInterval = 2f;
    private float timeSinceLastShot = 0f;  // Timer to track intervals between shots
    private GameManagerScript gm;
    private Player_collision_handler pc;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gm = FindObjectOfType<GameManagerScript>();
        pc = FindObjectOfType<Player_collision_handler>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Shoot_time();
    }
    void MovePlayer()
    {
        if (gm != null && gm.isGameOver)
        {
            return; // Stop spawning when the game is over

        }
        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            Vector3 temp = transform.position;
            temp.x += speed * Time.deltaTime;

            if (temp.x > max_x)
                temp.x = max_x;
            transform.position = temp;

        }
        else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            Vector3 temp = transform.position;
            temp.x -= speed * Time.deltaTime;

            if (temp.x < min_x)
                temp.x = min_x;
            transform.position = temp;
        }
        if (Input.GetAxisRaw("Vertical") > 0f)
        {
            Vector3 temp = transform.position;
            temp.y += speed * Time.deltaTime;

            if (temp.y > max_y)
                temp.y = max_y;
            transform.position = temp;
        }
        else if (Input.GetAxisRaw("Vertical") < 0f)
        {
            Vector3 temp = transform.position;
            temp.y -= speed * Time.deltaTime;

            if (temp.y < min_y)
                temp.y = min_y;
            transform.position = temp;
        }
    }
    void Shoot_time()
    {
        if (gm != null && gm.isGameOver)
        {
            return; // Stop spawning when the game is over

        }
        // Track time passed
        timeSinceLastShot += Time.deltaTime;

        // Check if it's time to shoot
        if (timeSinceLastShot >= ShootInterval)
        {
            if (!pc.IsBlinking)
            {
                Attack();
                timeSinceLastShot = 0f;  // Reset timer after shooting
            }
        }
    }
    void Attack()
    {
        GameObject Laser = Instantiate(Player_bullet, Spawn_point.position, Quaternion.identity);
        // Destroy laser after 5 seconds to prevent clutter
        if (audioSource != null && laserSound != null)
        {
            audioSource.PlayOneShot(laserSound);
        }
        Destroy(Laser, 3.5f);
    }
    public void DestroyPlayer()
    {
        Destroy(gameObject);
    }

}
