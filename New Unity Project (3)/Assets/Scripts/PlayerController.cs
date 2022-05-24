using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   public float moveSpeed; 
   
   
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
      // Calcula o movimento no eixo da camera para o movimento frente/tras
      Vector3 moveVertical = _mainCamera.transform.forward * _moveInput.y;

      // Calcula o movimento no eixo da camera para o movimento esquerda/direita
      Vector3 moverHorizontal = _mainCamera.transform.right * _moveInput.x;
         
      // Adiciona a força no objeto atraves do rigidbody, com intensidade definida por moveSpeed
      _rigidbody.AddForce((_mainCamera.transform.forward * _moveInput.y +
                           _mainCamera.transform.right * _moveInput.x) *
                          moveSpeed * Time.fixedDeltaTime);
   }

   private void FixedUpdate()
   {
      Move();
   }
}
