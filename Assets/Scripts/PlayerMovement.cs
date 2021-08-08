// using System.Numerics;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.InputSystem;
// using UnityEngine;
// using Vector3 = UnityEngine.Vector3;
// using Vector2 = UnityEngine.Vector2;
// //
// [RequireComponent(typeof(CharacterController))]
// public class PlayerMovement : MonoBehaviour
// {
//     [SerializeField] InputActionAsset playerControls;
//     InputAction movement;

//     CharacterController character;
//     Vector3 moveVector;
//     [SerializeField] float speed = 10f;
//     // Start is called before the first frame update
//     void Start()
//     {
//         var gameplayActionMap = playerControls.FindActionMap("Default", true);
//         movement = gameplayActionMap.FindAction("Move");
//         movement.performed += OnMovementChanged;
//         movement.canceled += OnMovementChanged;
//         movement.Enable();
//         character = GetComponent<CharacterController>();
//     }

//     // Update is called once per frame
//     void FixedUpdate()
//     {
//         character.Move(moveVector * speed * Time.fixedDeltaTime);
//     }

//     public void OnMovementChanged(InputAction.CallbackContext context)
//     {
//         Vector2 direction = context.ReadValue<Vector2>();
//         moveVector = new Vector3(direction.x, 0, direction.y);
//     }
// }
