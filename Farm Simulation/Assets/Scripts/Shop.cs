using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject shop = null;
    private Player player = null;
    private Animator shopAnimator = null;
    private string[] animationTriggers = { "isStretching", "isThinking" };
    private bool isAnimationPlaying = false;

    private static bool isShopOpen = false;
    public static bool ShopOpen { get => isShopOpen; set => isShopOpen = value; }

    private void OnMouseDown()
    {
        if(!ShopOpen && IsPlayerClose())
        {
            OpenShop();
        }
    }
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        shop.SetActive(false);
    }

    private void Start()
    {
        shopAnimator = GetComponent<Animator>();
        StartCoroutine(IsThinkingOrStretching());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(ShopOpen)
            {
                CloseShop();
            }
        }
    }
    private bool IsPlayerClose()
    {
        Vector3 playerLocation = player.GetCenter();
        Vector3 chestLocation = this.transform.position;
        float distance = Vector3.Distance(playerLocation, chestLocation);
        return (distance > 2f) ? false : true;
    }

    public void OpenShop()
    {
        ShopOpen = true;
        Player.InputDisabled = true;
        shop.SetActive(true);
    }
    public void CloseShop()
    {
        ShopOpen = false;
        Player.InputDisabled = false;
        shop.SetActive(false);
        InventoryUIManager.CloseInventory();
    }

    IEnumerator IsThinkingOrStretching()
    {
        while (true)
        {
            if(!isAnimationPlaying)
            {
                string randomTrigger = animationTriggers[Random.Range(0, animationTriggers.Length)];
                shopAnimator.SetTrigger(randomTrigger);
                isAnimationPlaying = true;
            }
            yield return new WaitForSeconds(3f);
            shopAnimator.SetTrigger("isIdle");
            yield return new WaitForSeconds(7f);
            isAnimationPlaying = false;
        }
    }

}
