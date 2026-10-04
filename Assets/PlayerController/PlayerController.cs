using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;
    public float turnSpeed = 12f;

    CharacterController controller;
    Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null)
        {
            return;
        }

        float x = 0f;
        float z = 0f;

        if (kb.wKey.isPressed || kb.upArrowKey.isPressed)
        {
            z = z + 1f;
        }
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed)
        {
            z = z - 1f;
        }
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed)
        {
            x = x + 1f;
        }
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)
        {
            x = x - 1f;
        }

        Vector3 input = new Vector3(x, 0f, z);
        if (input.magnitude > 1f)
        {
            input = input.normalized;
        }

        if (input.sqrMagnitude > 0.01f)
        {
            Quaternion target = Quaternion.LookRotation(input);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, turnSpeed * Time.deltaTime);
        }

        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        if (kb.spaceKey.wasPressedThisFrame && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y = velocity.y + gravity * Time.deltaTime;

        Vector3 move = input * moveSpeed;
        move.y = velocity.y;
        controller.Move(move * Time.deltaTime);
    }
}