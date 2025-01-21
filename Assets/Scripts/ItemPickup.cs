using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    public Button takeButton; // Кнопка подбора предмета
    public Button dropButton; // Кнопка сброса предмета
    public float maxPickupDistance = 5f; // Максимальное расстояние до объекта для подбора
    public Transform holdPoint; // Точка, где фиксируется предмет (например, перед камерой)

    private GameObject heldItem = null; // Текущий удерживаемый предмет

    void Start()
    {
        // Скрываем кнопки по умолчанию
        takeButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);

        // Подписываемся на события кнопок
        takeButton.onClick.AddListener(PickupItem);
        dropButton.onClick.AddListener(DropItem);
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
                // Если нашли объект с тегом "Drag", показываем кнопку подбора
                takeButton.gameObject.SetActive(true);
                return;
            }
        }

        // Если ничего не нашли, скрываем кнопку подбора
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

                // Показываем кнопку сброса, скрываем кнопку подбора
                dropButton.gameObject.SetActive(true);
                takeButton.gameObject.SetActive(false);
            }
        }
    }

    private void DropItem()
    {
        if (heldItem != null)
        {
            // Включаем физику у объекта
            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Отпускаем объект
            heldItem.transform.parent = null;
            heldItem = null;

            // Скрываем кнопку сброса
            dropButton.gameObject.SetActive(false);
        }
    }
}
