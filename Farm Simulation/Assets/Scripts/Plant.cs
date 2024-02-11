using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Plant : MonoBehaviour
{
    [SerializeField] private SpriteRenderer plantPulledSprite = null;
    [SerializeField] private Transform farmEffectTransform = null;
    public Vector2Int plantTileLocation;
    private int farmActionsCnt = 0;

    public void ToolFarm(ItemInfo itemInfo, bool isToolUp, bool isToolDown, bool isToolRight, bool isToolLeft)
    {

        TileAttributeDetail tileAttributeDetail = TileManager.GetTileAttributeDetail(plantTileLocation.x, plantTileLocation.y);
        if (tileAttributeDetail == null) return;

        ItemInfo seedInfo = InventoryManager.GetItemInfo(tileAttributeDetail.seedNo);
        if (seedInfo == null) return;

        PlantInfo plantInfo = TileManager.GetPlantInfo(seedInfo.itemNo);
        if (plantInfo == null) return;

        Animator animator = this.GetComponentInChildren<Animator>();

        int farmActionsNeeded = plantInfo.ToolFarmAction(itemInfo.itemNo);
        farmActionsCnt += 1;

        if (animator!= null && farmActionsCnt < farmActionsNeeded)
        {
            if (isToolRight || isToolUp) animator.SetTrigger("toolRight");
            else if (isToolLeft || isToolDown) animator.SetTrigger("toolLeft");
        }

        if (plantInfo.isFarmEffect) EventHandler.CallFarmEffect(farmEffectTransform.position, plantInfo.farmEffectType);
 
        if (farmActionsNeeded == -1) return;

        if (farmActionsCnt >= farmActionsNeeded) FarmPlant(plantInfo, tileAttributeDetail, isToolRight, isToolUp, animator);
    }
    
    private void FarmPlant(PlantInfo plantInfo, TileAttributeDetail tileAttributeDetail, bool isToolRight, bool isToolUp, Animator animator)
    {
        if(plantInfo.isFarmAnimation && animator != null)
        {
            if(plantInfo.farmSprite != null)
            {
                if (plantPulledSprite != null) plantPulledSprite.sprite = plantInfo.farmSprite;
            }

            if (isToolRight || isToolUp) animator.SetTrigger("farmRight");

            else animator.SetTrigger("farmLeft");

        }


        tileAttributeDetail.age = -1;
        tileAttributeDetail.ageWater = -1;
        tileAttributeDetail.ageFarm = -1;
        tileAttributeDetail.seedNo = -1;
        if (plantInfo.disablePlant) GetComponentInChildren<SpriteRenderer>().enabled = false;
        if(plantInfo.disablePlantCollider)
        {
            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D collider in colliders) collider.enabled = false;
        }

        TileManager.SetTileAttributeDetail(tileAttributeDetail.x, tileAttributeDetail.y, tileAttributeDetail);
        if (plantInfo.isFarmAnimation && animator != null) StartCoroutine(FarmGoods(plantInfo, tileAttributeDetail, animator));
        else
        {
            GenerateFarmGoods(plantInfo);
            if (plantInfo.plantChangeNo != 0)
            {
                GenerateChangedPlant(plantInfo, tileAttributeDetail);
            }

            Destroy(gameObject);
        }
    }

    private void GenerateFarmGoods(PlantInfo plantInfo)
    {
        for(int i=0; i<plantInfo.goodsNo.Length; i++)
        {
            int farmPlant;

            if (plantInfo.plantMin[i] == plantInfo.plantMax[i] || plantInfo.plantMax[i] < plantInfo.plantMax[i]) farmPlant = plantInfo.plantMin[i];
            else farmPlant = Random.Range(plantInfo.plantMin[i], plantInfo.plantMax[i] + 1);
            
            for (int j = 0; j < farmPlant; j++)
            {
                Vector3 location;
                if(plantInfo.generatePlantInInventory)
                {
                    InventoryManager.AddItemInInventory(InventoryType.Player, plantInfo.goodsNo[i]);
                }
                else
                {
                    location = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), 0f);
                    ItemSave.SpawnItemInScene(plantInfo.goodsNo[i], location);
                }
            }
        }

    }

    private IEnumerator FarmGoods(PlantInfo plantInfo, TileAttributeDetail tileAttributeDetail, Animator animator)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Farmed"))
        {
            yield return null;
        }
        
        GenerateFarmGoods(plantInfo);
        if(plantInfo.plantChangeNo != 0)
        {
            GenerateChangedPlant(plantInfo, tileAttributeDetail);
        }

        Destroy(gameObject);
    }

    private void GenerateChangedPlant(PlantInfo plantInfo, TileAttributeDetail tileAttributeDetail)
    {
        tileAttributeDetail.seedNo = plantInfo.plantChangeNo;
        tileAttributeDetail.ageWater = -1;
        tileAttributeDetail.ageFarm = -1;
        tileAttributeDetail.age = 0;

        TileManager.SetTileAttributeDetail(tileAttributeDetail.x, tileAttributeDetail.y, tileAttributeDetail);
        TileManager.ShowPlantTile(tileAttributeDetail);
    }
}


