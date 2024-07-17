using UnityEngine;
using UnityEngine.Tilemaps;

public class InGame_GM : MonoBehaviour
{
    public PlayerController player1;
    public PlayerController player2;
    public PlayerManager playerManager;
    public PlayerController playerController;

    private bool isPlayer1Turn = true;


    [SerializeField] private Bomb bulletPrefab; // Bombのprefab
    public Transform firePoint; // 射撃位置
    [SerializeField] private Tilemap destroyTile;

    private Bomb bomb;

    void Start()
    {
        player1.SetPlayer(true); // 最初はPlayer1のターン
        player2.SetPlayer(false);
        destroyTile = GetComponent<Tilemap>();
    }

    void Update()
    {
        if (isPlayer1Turn)
        {
            player1.HandlePlayerInput();
        }
        else
        {
            player2.HandlePlayerInput();
        }
    }
    public void FireBullet()
    {
        Debug.Log(bulletPrefab);
        Debug.Log(firePoint.position);
        Debug.Log(firePoint.rotation);
        bomb = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * playerController.currentPower;

        bomb.SetUpTile(destroyTile);
    }
    public void SwitchTurn()
    {
        playerManager.SwitchToNextTurn();
    }
}
