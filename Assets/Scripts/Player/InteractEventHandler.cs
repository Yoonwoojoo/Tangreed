using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEventHandler : MonoBehaviour
{
    [Header("UI")]
    public ItemPrompt itemPrompt;
    public GameObject prompt;

    [SerializeField] private LayerMask targetLayer;

    private ItemSO curItemSO;

    [SerializeField] float waitTime;

    // 임의설정
    public GameObject inventoryObj;
    private bool activeInventory = false;

    private void Start()
    {
        prompt.SetActive(false);
        inventoryObj.SetActive(activeInventory); // 임의설정
    }

    // 임의설정
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("인벤토리창");
            activeInventory = !activeInventory;
            inventoryObj.SetActive(activeInventory);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("충돌");
        GameObject itemObject = collision.gameObject;

        if ((targetLayer.value & (1 << itemObject.layer)) != 0)
        {
            // TryGetComponent로 먼저 객체를 가져옴
            if (itemObject.TryGetComponent(out IInteractable curinteractable))
            {
                curItemSO = curinteractable.GetItemData();
                GameManager.Instance.itemSO = curItemSO;
                GameManager.Instance.addItem?.Invoke();
                itemPrompt.SetItemPrompt(curItemSO.itemName, curItemSO);
                Destroy(itemObject);
                StartCoroutine(ItemInteractionHandle());
            }
        }
    }

    private IEnumerator ItemInteractionHandle()
    {
        yield return new WaitForSeconds(waitTime);
        prompt.SetActive(false);
        GameManager.Instance.itemSO = null;
    }
}
