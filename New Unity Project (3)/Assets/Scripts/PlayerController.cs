using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   //public int coins = 0; 
   
   public float moveSpeed;
   public float maxVelocity;
   
   
   private Gamecontrol _gameControl;
   private PlayerInput _playerInput;
   private Camera _mainCamera;
   private Rigidbody _rigidbody;

   private Vector2 _moveInput;
   

   private void OnEnable()
   {
      // inicializacao
      _gameControl = new Gamecontrol();
      
      // referencias dos componentes no mesmo objeto da unity
      _playerInput = GetComponent<PlayerInput>();
      _rigidbody = GetComponent<Rigidbody>();
      
      // Referencia para a camera main guardada na classe Camera
      _mainCamera = Camera.main;
      
      // atribuindo ao delegate do action triggered no player input
      _playerInput.onActionTriggered += OnActionTriggered;


   }

   private void OnDisable()
   {
      _playerInput.onActionTriggered -= OnActionTriggered;
   }

   private void OnActionTriggered(InputAction.CallbackContext obj)
   {
      // comparando o nome do action que esta chegando com o nome da action de mover
      if (obj.action.name.CompareTo(_gameControl.Gameplay.Movement.name) == 0)
      {
         // atribuir ao moveInput o valor proveninente do input do jogador como um Vector.
         _moveInput = obj.ReadValue<Vector2>();
      }
   }

   private void Move()
   {
      // Pega o Vector que aponta na direção em que a camera está olhando e zeramos o componente Y
      Vector3 camFoward = _mainCamera.transform.forward;
      camFoward.y = 0; 
      // Calcula o movimento no eixo da camera para o movimento frente/tras
      Vector3 moveVertical =  camFoward * _moveInput.y;

      // pega o vector que aponta para o lado direito da camera e zeramos componente Y
      Vector3 camRight = _mainCamera.transform.right;
      camRight.y = 0;
      // Calcula o movimento no eixo da camera para o movimento esquerda/direita
      Vector3 moverHorizontal = camRight * _moveInput.x;
         
      // Adiciona a força no objeto atraves do rigidbody, com intensidade definida por moveSpeed
      _rigidbody.AddForce((moveVertical + moverHorizontal) * moveSpeed * Time.fixedDeltaTime);
   }

   private void FixedUpdate()
   {
      Move();
      LimiteVelocity();
   }

   private void LimiteVelocity()
   {
      //pegar a velocidade do player
      Vector3 velocity = _rigidbody.velocity;

      // checar se a velocidade está dentro dos limites nos diferentes eixos
      // limitando o eixo x usando ifs, Abs e Sign
      if (Mathf.Abs(velocity.x) > maxVelocity) velocity.x = Mathf.Sign(velocity.x) * maxVelocity;
      
      // -maxVelocity < velocity.z < maxVelocity
      velocity.z = Mathf.Clamp(value: velocity.z, -maxVelocity, maxVelocity);

      // alterar a velocidade do player para ficar dentro dos limites 
      _rigidbody.velocity = velocity;
   }
   
   /*
    * 1 - Checar se o jogador está no chão
    * --
    * 2 - Jogador preciso apertar o botão de pulo
    * 3 - Realizar o pulo através da física
   */

   private void OnDrawGizmos()
   {
      //Debug.DrawRay(start:transform.position, dir:Vector3.down * rayDistance );
   }
   

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Coin"))
      {
         coins++;
         Destroy(other.gameObject);
      }
   }
   
}
