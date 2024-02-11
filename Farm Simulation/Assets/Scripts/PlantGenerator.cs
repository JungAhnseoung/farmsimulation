using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGenerator : MonoBehaviour
{
    [ItemNoName]
    [SerializeField] private int seedNo = 0;
    [SerializeField] private int age = 0;
    [SerializeField] private int ageDig = -1;
    [SerializeField] private int ageWater = -1;
    private Grid grid;

    private void OnEnable()
    {
        EventHandler.GeneratePlant += GeneratePlant;
    }
    private void OnDisable()
    {
        EventHandler.GeneratePlant -= GeneratePlant;
    }

    private void GeneratePlant()
    {
        grid = GameObject.FindObjectOfType<Grid>();
        Vector3Int plantTileLocation = grid.WorldToCell(transform.position);

        if(seedNo > 0)
        {
            TileAttributeDetail tileAttributeDetail;
            tileAttributeDetail = TileManager.GetTileAttributeDetail(plantTileLocation.x, plantTileLocation.y);
            if( tileAttributeDetail == null) tileAttributeDetail = new TileAttributeDetail();

            tileAttributeDetail.seedNo = seedNo;
            tileAttributeDetail.age = age;
            tileAttributeDetail.ageDig = ageDig;
            tileAttributeDetail.ageWater = ageWater;

            TileManager.SetTileAttributeDetail(plantTileLocation.x, plantTileLocation.y, tileAttributeDetail);
        }

        Destroy(gameObject);
    }
}
