using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 direction;

    private int desiredLine = 1; // 0 left, 1 middle and 3 right
    public float laneDistance = 4; // the distance between two lanes

    public float forwardSpeed;

    public float jumpForce;
    public float Gravity = -20;

    public Animator animator;

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float maxSpeed;

    public bool isSliding = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        // if game is not started, stop game
        if (!PlayerManager.isGameStarted)
            return;
        // increase speed
        if (forwardSpeed < maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;

        animator.SetBool("isGameStarted", true);

        direction.z = forwardSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
        // if controller is in ground
        //if (controller.isGrounded)
        if (isGrounded)
        {
            direction.y = -1;
            // if user push up arrow and controller is in ground
            if (Input.GetKey("space"))
            {
                jump();
            }
        }
        else
        {
            // set gravity to jump
            direction.y += Gravity * Time.deltaTime;
        }

        // gather the inputs on which lane we should be
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLine++;
            if (desiredLine == 3)
                desiredLine = 2;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLine--;
            if (desiredLine == -1)
                desiredLine = 0;
        }
        else if (Input.GetKey("down") && !isSliding)
        {
            StartCoroutine(Slide());
        }

        // calculate where we should be in the future

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLine == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLine == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        //transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.deltaTime);
        //transform.position = targetPosition;
        //controller.center = controller.center;

        if (transform.position == targetPosition)
            return;

        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;

        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);


    }

    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }


    private void jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }


    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;
        yield return new WaitForSeconds(1.3f);
        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }

}
