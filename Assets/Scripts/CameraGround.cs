using UnityEngine;

public class CameraGround : MonoBehaviour
{
    public static GameObject playerSpawnPointGO;
    public static Transform player;
    [SerializeField] public float smoothSpeed = 0.3f;

    void Start()
    {
        playerSpawnPointGO = GameObject.Find("PlayerSpawnPoint");
    }
    void FixedUpdate()
    {
        Vector3 desiredPosition = new Vector3(player.transform.position.x + 3f, player.transform.position.y + 1, -10);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public static void FindPlayer()
    {
        player = playerSpawnPointGO.transform.Find("Player(Clone)");
    }
}
