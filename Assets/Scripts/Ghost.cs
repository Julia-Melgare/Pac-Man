using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehaviour initialBehavior;
    public Transform target;
    public bool playerControl { get; private set; } = false;

    public AudioClip eatGhostClip;
    public AudioClip sirenClip;
    public AudioClip ghostRetreatingClip;

    public int points = 200;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightenedAI>();
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();
        if (!playerControl)
        {
            frightened.Disable();
            chase.Disable();
            scatter.Enable();
            if (home != initialBehavior)
            {
                home.Disable();
            }
            if (initialBehavior != null)
            {
                initialBehavior.Enable();
            }
        }        
    }

    public void EnablePlayerInput()
    {
        CancelInvoke();
        Destroy(home);
        Destroy(scatter);
        Destroy(chase);
        frightened = gameObject.GetComponent<GhostFrightenedPlayer>();
        gameObject.GetComponent<PlayerInput>().enabled = true;
        playerControl = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ghost"), LayerMask.NameToLayer("Door"), true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled && !frightened.eaten)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else
            {
                FindObjectOfType<GameManager>().PacmanCaught();
            }
        }
    }
}
