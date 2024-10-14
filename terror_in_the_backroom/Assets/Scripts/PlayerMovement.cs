using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public float gravity = 9.8f;
    private Camera playerCamera;
    private CharacterController playerController;
    private Vector3 direction;
    private float verticalVelocity;

    bool isCollidingWithObject = false;
    Collider colliderOfObjectToCollect;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        //lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovement();
        LookAround();
        CheckForObjectCollecting();
    }

    void HandlePlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (playerController.isGrounded)
        {
            direction = new Vector3(horizontal, 0, vertical);
            direction = transform.TransformDirection(direction) * speed;

            if (Input.GetButton("Jump"))
            {
                verticalVelocity = jumpForce;
            }
        }

        verticalVelocity = verticalVelocity - (gravity * Time.deltaTime);
        direction.y = verticalVelocity;

        playerController.Move(direction * Time.deltaTime);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        //rotate player around the y axis
        transform.Rotate(0,mouseX,0);

        //rotate camera around the x axis
        Vector3 cameraRotation = playerCamera.transform.localEulerAngles;
        cameraRotation.x -= mouseY;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90f, 45f);  
        playerCamera.transform.localEulerAngles = cameraRotation;

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Collectible"))
        {
            isCollidingWithObject = true;
            colliderOfObjectToCollect = other;
        }
    }
    void CheckForObjectCollecting()
    {
        //only collect item if a player is colliding with is and pressing the E key
        if (isCollidingWithObject && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player is pressing E");
            CollectObject(colliderOfObjectToCollect); 
        }
    }

    void CollectObject(Collider other)
    {
        //TODO: logic to add item inventory
        other.gameObject.SetActive(false);
        //set isColliding bool back to false
        isCollidingWithObject = true;
    }

}
