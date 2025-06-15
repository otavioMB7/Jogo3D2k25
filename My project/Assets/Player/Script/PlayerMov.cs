using System.Collections;
using UnityEngine;

public class PlayerNovo : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 5f;
    public float jumpForce = 8f;
    public float gravity = -9.81f;
    private float verticalVelocity;
    private Animator anim;
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    private float xRotation = 0f;
    private bool isAttacking = false;
    private bool isJumping = false;
    private bool coroutineRunning = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();

        // Pulo
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            verticalVelocity = jumpForce;
            anim.SetTrigger("Jump");  // Trigger com J maiúsculo
            isJumping = true;
            Debug.Log("Pulou! Trigger Jump acionado!");
        }

        // Aplica gravidade
        verticalVelocity += gravity * Time.deltaTime;
        Vector3 verticalMove = Vector3.up * verticalVelocity;
        controller.Move(verticalMove * Time.deltaTime);

        // Ataque
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("Attack");  // Trigger com A maiúsculo
            Debug.Log("Ataque iniciado!");
            StartCoroutine(ResetAttackCooldown());
        }

        // Rotaciona o player com o mouse (câmera)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (controller.isGrounded && !isJumping)
        {
            if (move != Vector3.zero)
            {
                controller.Move(speed * Time.deltaTime * move);
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }
    }

    private IEnumerator ResetAttackCooldown()
    {
        if (coroutineRunning)
            yield break;

        coroutineRunning = true;
        // Ajuste o tempo para a duração da animação de ataque
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        coroutineRunning = false;
        Debug.Log("Ataque liberado para novo ataque.");
    }

    public void EndAttack()
    {
        isAttacking = false;
        Debug.Log("EndAttack chamado");
    }
}
