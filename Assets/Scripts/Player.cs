using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Require these components when using this script
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [System.NonSerialized]
    public float lookWeight;					// the amount to transition when using head look

    [System.NonSerialized]
    public Transform enemy;						// a transform to Lerp the camera to during head look

    public float animSpeed = 1.5f;				// a public setting for overall animator animation speed
    public float lookSmoother = 3f;				// a smoothing setting for camera motion
    public bool useCurves;						// a setting for teaching purposes to show use of curves


    private Animator anim;							// a reference to the animator on the character
    private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer
    private AnimatorStateInfo layer2CurrentState;	// a reference to the current state of the animator, used for layer 2
    private CapsuleCollider col;					// a reference to the capsule collider of the character

    private MazeCell currentCell;
    private Text bombText;
    private Text scoreText;
    private int numBombs;
    private int score;


    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int locoState = Animator.StringToHash("Base Layer.Locomotion");			// these integers are references to our animator's states
    static int jumpState = Animator.StringToHash("Base Layer.Jump");				// and are used to check state for various actions to occur
    static int jumpDownState = Animator.StringToHash("Base Layer.JumpDown");		// within our FixedUpdate() function below
    static int fallState = Animator.StringToHash("Base Layer.Fall");
    static int rollState = Animator.StringToHash("Base Layer.Roll");
    static int waveState = Animator.StringToHash("Layer2.Wave");

    void Start()
    {
        // initialising reference variables
        numBombs = 0;
        score = 0;
        bombText = GameObject.Find("In-game Canvas(Clone)/Bombs").GetComponent<Text>();
        scoreText = GameObject.Find("In-game Canvas(Clone)/Score").GetComponent<Text>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
    }


    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");				// setup h variable as our horizontal input axis
        float v = Input.GetAxis("Vertical");				// setup v variables as our vertical input axis
        anim.SetFloat("Speed", v);							// set our animator's float parameter 'Speed' equal to the vertical input axis				
        anim.SetFloat("Direction", h); 						// set our animator's float parameter 'Direction' equal to the horizontal input axis		
        anim.speed = animSpeed;								// set the speed of our animator to the public variable 'animSpeed'
        anim.SetLookAtWeight(lookWeight);					// set the Look At Weight - amount to use look at IK vs using the head's animation
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// set our currentState variable to the current state of the Base Layer (0) of animation

    }

    public void SetLocation(MazeCell cell)
    {
        currentCell = cell;
        transform.localPosition = cell.transform.localPosition;
        transform.localPosition += new Vector3(0f, 0.5f, 0f);
    }

    void OnCollisionEnter(Collision hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Collectible":
                if (hit.gameObject.name == "Bomb(Clone)")
                {
                    numBombs++;
                    bombText.text = "Bombs: " + numBombs;
                }
                else if (hit.gameObject.name == "Cheese(Clone)")
                {
                    score += 5;
                    scoreText.text = "Score: " + score;
                }

                if (hit.gameObject.audio != null)
                {
                    AudioSource.PlayClipAtPoint(hit.gameObject.audio.clip, hit.transform.position);
                }
                Destroy(hit.gameObject);
                break;
            case "Trap":
                if (numBombs > 0)
                {
                    numBombs--;
                    bombText.text = "Bombs: " + numBombs;
                }
                else { GameManager.gameState = GameState.Lose; }

                if (hit.gameObject.audio != null)
                {
                    AudioSource.PlayClipAtPoint(hit.gameObject.audio.clip, hit.transform.position);
                }
                Destroy(hit.gameObject);
                break;
            case "End":
                Destroy(hit.gameObject);
                GameManager.gameState = GameState.Win;
                break;
            default:
                break;
        }
    }
}
