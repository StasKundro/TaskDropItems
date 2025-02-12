using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupPC : MonoBehaviour
{
    public GameObject interactHint; // UI-объект "Press E to take"
    public GameObject dropHint; // UI-объект "Press E to drop / Q to throw"
    public Transform holdPoint; // Точка, где фиксируется предмет
    public float maxPickupDistance = 5f; // Максимальное расстояние для подбора
    public float throwForce = 10f; // Сила броска

    private GameObject heldItem = null; // Текущий удерживаемый предмет

    void Start()
    {
        interactHint.SetActive(false); // Скрываем подсказку подбора
        dropHint.SetActive(false); // Скрываем подсказку сброса
    }

    void Update()
    {
        if (heldItem == null)
        {
            CheckForItem(); // Проверяем наличие предмета
        }
        else
        {
            HandleHeldItem(); // Обрабатываем действия с удерживаемым предметом
        }
    }

    private void CheckForItem()
    {
        // Выпускаем луч из центра экрана
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxPickupDistance))
        {
            if (hit.collider.CompareTag("Drag"))
            {
                interactHint.SetActive(true); // Показываем подсказку "Press E to take"

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickupItem(hit.collider.gameObject); // Подбираем предмет
                }
                return;
            }
        }

        interactHint.SetActive(false); // Скрываем подсказку
    }

    private void PickupItem(GameObject item)
    {
        heldItem = item;

        // Отключаем физику у объекта
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Фиксируем объект перед камерой
        heldItem.transform.position = holdPoint.position;
        heldItem.transform.rotation = holdPoint.rotation;
        heldItem.transform.parent = holdPoint;

        interactHint.SetActive(false); // Скрываем подсказку подбора
        dropHint.SetActive(true); // Показываем подсказку сброса/броска
    }

    private void HandleHeldItem()
    {
        // Нажимаем E, чтобы отпустить предмет
        if (Input.GetKeyDown(KeyCode.E))
        {
            DropItem();
        }

        // Нажимаем Q, чтобы бросить предмет с импульсом
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ThrowItem();
        }
    }

    private void DropItem()
    {
        if (heldItem != null)
        {
            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            heldItem.transform.parent = null;
            heldItem = null;

            dropHint.SetActive(false); // Скрываем подсказку сброса/броска
        }
    }

    private void ThrowItem()
    {
        if (heldItem != null)
        {
            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                heldItem.transform.parent = null;

                // Применяем силу вперед от позиции holdPoint
                rb.AddForce(holdPoint.forward * throwForce, ForceMode.VelocityChange);
            }

            heldItem = null;
            dropHint.SetActive(false); // Скрываем подсказку сброса/броска
        }
    }
}
