using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    private CharacterController controller;
    private Transform Mycamera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Mycamera = Camera.main.transform;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(horizontal, 0, vertical);

        movimento  = Mycamera.TransformDirection(movimento);
        movimento.y = 0;

        controller.Move(movimento * Time.deltaTime * 5  );
        controller.Move(new Vector3 (0, - 9.81f, 0) * Time.deltaTime);
    }
}