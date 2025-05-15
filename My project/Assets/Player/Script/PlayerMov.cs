using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerMov : MonoBehaviour
{
    private CharacterController controller;
    private Transform Mycamera;
    private Animator animator;

    private float forcaY;

    private bool caindodopulo;
    [SerializeField]private Transform pedopersonagem;
    [SerializeField] private LayerMask colisaoLayer;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        Mycamera = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(horizontal, 0, vertical);

        movimento  = Mycamera.TransformDirection(movimento);
        movimento.y = 0;

        controller.Move(movimento * Time.deltaTime * 10  );

        if(movimento != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movimento), Time.deltaTime * 10);
        }

        animator.SetBool("Mover", movimento != Vector3.zero);

        caindodopulo = Physics.CheckSphere(pedopersonagem.position, 0.3f, colisaoLayer);
        animator.SetBool("Está no chão", caindodopulo);

        if (Input.GetKeyDown(KeyCode.Space) && caindodopulo)

        {
            forcaY = 5f;
            animator.SetTrigger("Saltar");
        }

        if(forcaY > -9.81f)
        {
            forcaY += -9.81f * Time.deltaTime;
        }

        controller.Move(new Vector3(0, forcaY, 0) * Time.deltaTime);
    }
}