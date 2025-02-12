using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    public Button takeButton; // Кнопка подбора предмета
    public Button dropButton; // Кнопка сброса предмета
    public Button throwButton; // Кнопка броска предмета
    public float maxPickupDistance = 5f; // Максимальное расстояние до объекта для подбора
    public Transform holdPoint; // Точка, где фиксируется предмет (например, перед камерой)
    public float throwForce = 10f; // Сила броска

    private GameObject heldItem = null; // Текущий удерживаемый предмет

    void Start()
    {
        // Скрываем кнопки по умолчанию
        takeButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
        throwButton.gameObject.SetActive(false);

        // Подписываемся на события кнопок
        takeButton.onClick.AddListener(PickupItem);
        dropButton.onClick.AddListener(DropItem);
        throwButton.onClick.AddListener(ThrowItem);
    }

    void Update()
    {
        if (heldItem == null)
        {
            CheckForItem();
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
                takeButton.gameObject.SetActive(true);
                return;
            }
        }

        takeButton.gameObject.SetActive(false);
    }

    private void PickupItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxPickupDistance))
        {
            if (hit.collider.CompareTag("Drag"))
            {
                heldItem = hit.collider.gameObject;

                Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                heldItem.transform.position = holdPoint.position;
                heldItem.transform.rotation = holdPoint.rotation;
                heldItem.transform.parent = holdPoint;

                dropButton.gameObject.SetActive(true);
                throwButton.gameObject.SetActive(true);
                takeButton.gameObject.SetActive(false);
            }
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

            dropButton.gameObject.SetActive(false);
            throwButton.gameObject.SetActive(false);
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
            dropButton.gameObject.SetActive(false);
            throwButton.gameObject.SetActive(false);
        }
    }
}
