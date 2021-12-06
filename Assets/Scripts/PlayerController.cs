using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("General")]
    public float gravityScale = -20f;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Rotation")]
    public float rotationSensibility = 10f;

    [Header("Jump")]
    public float jumpHeight = 1.9f;

    [Header("Datos salto")]
    public float prev_y = 0.0f;
    public int contadorGrounded = 0;        //Contador para permitir saltar un poco despu�s de no tocar el suelo (ayuda a la jugabilidad)
    public int retardoJump = 6;

    private float cameraVerticalAngle;
    Vector3 moveInput = Vector3.zero;
    Vector3 rotationinput = Vector3.zero;
    CharacterController characterController;
    /*
    [Header("GameObjects y esas cosas")]
    public DisparoArma DisparoArma;
    public GameObject escopeta;
    */
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
        Move();
       // PressedButtons();
    }

    private void Move()
    {
        if (characterController.isGrounded)
        {
            contadorGrounded = 0;
            moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            moveInput = Vector3.ClampMagnitude(moveInput, 1f);
            prev_y = moveInput.y;   //Esto esta puesto para poder visualizar cuando para de caer. No hace falta en si.

            if (Input.GetButton("Sprint") && (1 < this.GetComponent<Stamina>().ReturnStamina()))
            {
                moveInput = transform.TransformDirection(moveInput) * runSpeed;
                this.GetComponent<Stamina>().UsarStamina(0.40f);
            }
            else
            {
                moveInput = transform.TransformDirection(moveInput) * walkSpeed;
            }

            if (Input.GetButtonDown("Jump"))
            {
                moveInput.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);
                contadorGrounded = retardoJump + 1;
            }
        }
        //Poder cambiar de direccion en el aire (sprint incluido).
        else if (contadorGrounded < retardoJump) {
            if (Input.GetButtonDown("Jump"))
            {
                moveInput.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);
                contadorGrounded = retardoJump + 1;
            } }

        else
        {
            contadorGrounded++;
            prev_y = moveInput.y;
            moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            moveInput = Vector3.ClampMagnitude(moveInput, 1f);

            if (Input.GetButton("Sprint") && (1 < this.GetComponent<Stamina>().ReturnStamina()))
            {
                moveInput = transform.TransformDirection(moveInput) * runSpeed;
                this.GetComponent<Stamina>().UsarStamina(0.40f);
            }
            else
            {
                moveInput = transform.TransformDirection(moveInput) * walkSpeed;
            }

            moveInput.y = prev_y;
        }

        moveInput.y += gravityScale * Time.deltaTime;
        characterController.Move(moveInput * Time.deltaTime);
    }

    private void Look()
    {
        rotationinput.x = Input.GetAxis("Mouse X") * rotationSensibility * Time.deltaTime;
        rotationinput.y = Input.GetAxis("Mouse Y") * rotationSensibility * Time.deltaTime;

        cameraVerticalAngle = cameraVerticalAngle + rotationinput.y;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -70, 70);

        transform.Rotate(Vector3.up * rotationinput.x);
        playerCamera.transform.localRotation = Quaternion.Euler(-cameraVerticalAngle, 0f, 0f);
    }
    /*
    private void PressedButtons()
    {
       // if (Input.GetKeyDown(KeyCode.R)) { DisparoArma.Recharge(); } //Recargar Arma
       // if (Input.GetKeyDown(KeyCode.X)) { escopeta.SetActive(!escopeta.activeSelf); }   //Ocultar-Mostar Arma 
        if (Input.GetKeyDown(KeyCode.P)) {if (Time.timeScale != 0) Time.timeScale = 0; else Time.timeScale = 1;}   //Pausa
        if (Input.GetKeyDown(KeyCode.E)) {; }   //Abrir Inventario
        if (Input.GetKeyDown(KeyCode.F)) {; }   //Acciona con el mapa. Sirve para recoger elementos y/o comerciar.
        if (Input.GetKeyDown(KeyCode.Escape)) {; }  //Menu opciones y/o salir
    }*/
}
