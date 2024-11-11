using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int health = 3;
    
    private bool isTurnActive = false;
    private GameManager gameManager;

    public bool IsAlive => health > 0;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void StartTurn()
    {
        isTurnActive = true;
    }

    public void EndTurn()
    {
        isTurnActive = false;
    }

    private void Update()
    {
        if (isTurnActive && IsAlive)
        {
            HandleMovement();
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
                gameManager.EndTurn();
            }
        }
    }

    private void HandleMovement()
    {
        float move = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.Translate(move, 0, 0);
    }

    private void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameManager.CheckGameOver();
        }
    }
}
