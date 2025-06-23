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
    private bool isJumping = false;

    [Header("Configuração de Ataque")]
    public GameObject attackHitbox; // Objeto filho que é a hitbox
    public float attackHitboxDelay = 0.2f; // tempo até ligar a hitbox
    public float attackHitboxDuration = 0.3f; // tempo ativa
    private bool isAttacking = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;

        // Garante que a hitbox começa desativada
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    void Update()
    {
        Move();
        HandleJump();
        HandleAttack();
        HandleMouseLook();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (controller.isGrounded && !isJumping)
        {
            anim.SetBool("isWalking", move != Vector3.zero);
        }

        controller.Move(speed * Time.deltaTime * move);
    }

    void HandleJump()
    {
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            verticalVelocity = jumpForce;
            anim.SetTrigger("Jump");
            isJumping = true;
            Debug.Log("Pulou!");
        }

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 verticalMove = Vector3.up * verticalVelocity;
        controller.Move(verticalMove * Time.deltaTime);
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            anim.SetTrigger("Attack");
            StartCoroutine(AttackRoutine());
            Debug.Log("Ataque animado iniciado!");
        }
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // Espera o tempo para bater com o momento do golpe na animação
        yield return new WaitForSeconds(attackHitboxDelay);

        EnableHitbox();
        Debug.Log("Hitbox ativada!");

        // Mantém a hitbox ativa pelo tempo necessário
        yield return new WaitForSeconds(attackHitboxDuration);

        DisableHitbox();
        Debug.Log("Hitbox desativada!");

        // Pode colocar um cooldown aqui se quiser
        isAttacking = false;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void EnableHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }
}
