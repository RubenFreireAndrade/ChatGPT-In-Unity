using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public GameObject chatgptUi;
    public ChatGPT chatgptObj;

    private Rigidbody rigidBody;
    private bool isPlayerTalking;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerTalking = false;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isPlayerTalking)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            chatgptUi.SetActive(false);

            float xDirection = Input.GetAxis("Horizontal");
            float zDirection = Input.GetAxis("Vertical");

            Vector3 direction = (transform.right * xDirection + transform.forward * zDirection).normalized;

            Vector3 newPosition = rigidBody.position + direction * (speed * Time.fixedDeltaTime);

            rigidBody.MovePosition(newPosition);
        }
        else
        {
            // Do not move
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            chatgptUi.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            chatgptObj.SetNPCInfo(other.GetComponent<NpcInfo>());
            isPlayerTalking = true;
            // ChatGPT UI opens.
            Debug.Log("Entering Trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isPlayerTalking = false;
            // ChatGPT UI closes.
            Debug.Log("Exiting Trigger");
        }
    }

    public void SetTalkingState(bool talkingState)
    {
        isPlayerTalking = talkingState;
    }
}
