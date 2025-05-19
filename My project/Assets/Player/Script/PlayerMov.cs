using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    private CharacterController controller;
    private Transform myCamera;
    private Animator animator;

    private float forcaY;
    private bool caindoDoPulo;

    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask colisaoLayer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        myCamera = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(horizontal, 0, vertical);
        movimento = myCamera.TransformDirection(movimento);
        movimento.y = 0;

        controller.Move(movimento * Time.deltaTime * 10);

        if (movimento.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movimento), Time.deltaTime * 10);
            animator.SetInteger("transition", 1); // andando
        }
        else
        {
            animator.SetInteger("transition", 0); // parado
        }

        caindoDoPulo = Physics.CheckSphere(peDoPersonagem.position, 0.3f, colisaoLayer);
        animator.SetBool("Está no chão", caindoDoPulo);

        if (Input.GetKeyDown(KeyCode.Space) && caindoDoPulo)
        {
            forcaY = 5f;
            animator.SetTrigger("Saltar");
        }

        if (forcaY > -9.81f)
        {
            forcaY += -9.81f * Time.deltaTime;
        }

        controller.Move(new Vector3(0, forcaY, 0) * Time.deltaTime);
    }
}
