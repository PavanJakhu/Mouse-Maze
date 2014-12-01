using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float rotateSpeed = 3.0F;
	public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
	public CharacterController controller;

    private Text scoreText;
    private Vector3 moveDirection = Vector3.zero;
    private MazeCell currentCell;
    private int numBombs;

    public void SetLocation(MazeCell cell)
    {
        currentCell = cell;
        transform.localPosition = cell.transform.localPosition;
        transform.localPosition += new Vector3(0f, 0.5f, 0f);
    }

	void Start () 
	{
		//controller = GetComponent<CharacterController>();
        numBombs = 0;
		//DontDestroyOnLoad(transform.gameObject);
        scoreText = GameObject.Find("In-game Canvas(Clone)/Bombs").GetComponent<Text>();
	}

    void Update()
    {

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

        if (controller.isGrounded)
        {
            //moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));

            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= speed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Bomb":
                numBombs++;
                scoreText.text = "Bombs: " + numBombs;
                Destroy(hit.gameObject);
                break;
            case "Trap":
                Destroy(hit.gameObject);
                if (numBombs > 0) { numBombs--; }
                else { GameManager.gameState = GameState.Lose; }
                break;
            case "End":
                Destroy(hit.gameObject);
                Debug.Log("You win!");
                break;
            default:
                break;
        }
    }
}
