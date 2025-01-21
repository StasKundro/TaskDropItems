using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public Joystick movementJoystick; // Джойстик для движения
    public Joystick cameraJoystick;   // Джойстик для вращения камеры
    public float moveSpeed = 5f;      // Скорость движения персонажа
    public float rotationSpeed = 100f; // Скорость вращения камеры

    private Transform cameraTransform; // Ссылка на камеру
    private CharacterController characterController;

    private float cameraPitch = 0f; // Угол наклона камеры по вертикали

    void Start()
    {
        cameraTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("На объекте персонажа отсутствует CharacterController!");
        }
    }

    void Update()
    {
        // Движение персонажа
        float horizontal = movementJoystick.Horizontal;
        float vertical = movementJoystick.Vertical;

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
        moveDirection = transform.TransformDirection(moveDirection) * moveSpeed;

        characterController.SimpleMove(moveDirection);

        // Вращение персонажа по горизонтали
        float cameraHorizontal = cameraJoystick.Horizontal;
        transform.Rotate(0f, cameraHorizontal * rotationSpeed * Time.deltaTime, 0f);

        // Вращение камеры по вертикали
        float cameraVertical = cameraJoystick.Vertical;
        cameraPitch -= cameraVertical * rotationSpeed * Time.deltaTime; // Уменьшаем, так как движем вверх
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f); // Ограничиваем угол наклона камеры

        cameraTransform.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
    }
}
