using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe do jogador
public class Player : MonoBehaviour
{
    // Componentes do jogador
    [Header("Componentes")]
    Rigidbody playerBody; // Corpo do jogador (Rigidbody)

    // Movimenta��o do jogador
    [Header("Movimenta��o")]
    float inputX; // Input horizontal do jogador
    float inputZ; // Input vertical do jogador
    public float speed; // Velocidade do jogador
    Vector3 movement; // Vetor de movimenta��o do jogador

    // Pulo do jogador
    [Header("Pulo")]
    float inputY; // Input de pulo do jogador
    public float jumpForce; // For�a do pulo do jogador
    bool isGrounded; // Verifica se o jogador est� no ch�o
    public GameObject checkGround; // Objeto que verifica se o jogador est� no ch�o
    public float checkRadius; // Raio de verifica��o do ch�o
    public LayerMask whatIsGround; // Layer do ch�o

    // C�mera do jogador
    [Header("Camera")]
    Transform cameraT; // Transform da c�mera
    float verticalLookRotation; // Rota��o vertical da c�mera
    float mouseX; // Input de movimento horizontal da c�mera
    float mouseY; // Input de movimento vertical da c�mera
    public float mouseSensitivityX; // Sensibilidade de movimento horizontal da c�mera
    public float mouseSensitivityY; // Sensibilidade de movimento vertical da c�mera
    float mouse2;

    //Atirar
    [Header("Tiros")]
    float inputShoot;

    // Inicializa��o do jogador
    void Start()
    {
        // Obter o componente Rigidbody do jogador
        playerBody = GetComponent<Rigidbody>();
        // Obter a transform da c�mera
        cameraT = Camera.main.transform;
        // Tirar o cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Atualiza��o do jogador (chamada a cada frame)
    void Update()
    {
        // Obter os inputs da c�mera
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        // Verificar se o jogador est� no ch�o
        isGrounded = Physics.CheckSphere(checkGround.transform.position, checkRadius, whatIsGround);

        // Rotacionar o jogador com a c�mera
        RotateWithCamera();

        // Movimentar o jogador para frente baseado na rota��o
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        //Checar se o jogador apertou o bot�o de pular
        inputY = Input.GetAxis("Jump");

        // Converter a movimenta��o para o sistema de coordenadas do jogador
        movement = transform.TransformDirection(new Vector3(inputX * speed, 0, inputZ * speed));        //Mostrar e Sumir mouse
        
        inputShoot = Input.GetAxis("Fire1");
        mouse2 = Input.GetAxis("Fire2");
        if(Cursor.visible == false && mouse2 !=0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if(Cursor.visible == true && inputShoot != 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Atualiza��o f�sica do jogador (chamada a cada frame f�sico)
    void FixedUpdate()
    {
        // Atualizar a velocidade do jogador
        playerBody.velocity = new Vector3(movement.x, playerBody.velocity.y, movement.z);
        if (isGrounded && inputY != 0)
        {
            // Adicionar for�a de pulo ao jogador
            playerBody.AddForce(new Vector3(movement.x, jumpForce, movement.z));
        }
    }

    // Rotacionar o jogador com a c�mera
    public void RotateWithCamera()
    {
        // Rotacionar o jogador com o input da c�mera
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
        // Atualizar a rota��o vertical da c�mera
        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
        // Limitar a rota��o vertical da c�mera entre -60 e 60 graus
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        // Atualizar a rota��o da c�mera
        cameraT.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    // Desenhar gizmos para debug
    private void OnDrawGizmos()
    {
        // Desenhar uma esfera para representar a �rea de verifica��o do ch�o
        Gizmos.DrawSphere(checkGround.transform.position, checkRadius);
        // Mudar a cor dos gizmos para vermelho
        Gizmos.color = Color.red;
    }
}