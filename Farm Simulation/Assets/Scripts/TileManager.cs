using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class TileManager : MonoBehaviour, Save
{
    [SerializeField] private TileAttributes[] tileAttributesArr = null;
    private static Dictionary<string, TileAttributeDetail> tileAttributeDetailDict;
    private static Grid grid;

    [SerializeField] private PlantList editablePlantList = null;
    private static PlantList plantList = null;

    private static Transform plantGroupTransform;
    private static Tilemap surroundings1;
    private static Tilemap surroundings2;

    [SerializeField] private Tile[] editableDigTile = null;
    [SerializeField] private Tile[] editableWaterTile = null;
    private static Tile[] digTile = null;
    private static Tile[] waterTile = null;

    public const float gridSize = 1f;

    private string saveID;
    private SaveObject saveObject;
    private bool isSceneLoadInit = true;

    public string SaveID { get { return saveID; } set { saveID = value; } }
    public SaveObject SaveObject { get { return saveObject; } set { saveObject = value; } }
    public void OnEnable()
    {
        Register();
        EventHandler.AfterLoad += AfterLoad;
        EventHandler.DayPass += DayPass;
    }

    public void OnDisable()
    {
        Unregister();
        EventHandler.AfterLoad -= AfterLoad;
        EventHandler.DayPass -= DayPass;
    }
    private void AfterLoad()
    {
        grid = GameObject.FindObjectOfType<Grid>();
        surroundings1 = GameObject.FindGameObjectWithTag("Surroundings1").GetComponent<Tilemap>();
        surroundings2 = GameObject.FindGameObjectWithTag("Surroundings2").GetComponent<Tilemap>();
        if (GameObject.FindGameObjectWithTag("PlantsGroup") != null)
        {
            plantGroupTransform = GameObject.FindGameObjectWithTag("PlantsGroup").transform;
        }
        else plantGroupTransform = null;
    }
    
    private void DayPass(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        ClearShowTileAttributeDetail();
        foreach(TileAttributes tileAttributes in tileAttributesArr)
        {
            if(SaveObject.scene.TryGetValue(tileAttributes.sceneType.ToString(), out SaveScene saveScene))
            {
                if(saveScene.tileAttributeDetailData != null)
                {
                    for (int i = saveScene.tileAttributeDetailData.Count - 1; i >= 0; i--)
                    {
                        KeyValuePair<string, TileAttributeDetail> item = saveScene.tileAttributeDetailData.ElementAt(i);
                        TileAttributeDetail tileAttributeDetail = item.Value;

                        if (tileAttributeDetail.age > -1 && tileAttributeDetail.ageDig == -1) tileAttributeDetail.age += 1;
                        else if (tileAttributeDetail.age > -1 && tileAttributeDetail.ageDig > -1 && tileAttributeDetail.ageWater > -1) tileAttributeDetail.age += 1;
                        if (tileAttributeDetail.ageWater > -1) tileAttributeDetail.ageWater = -1;
                        SetTileAttributeDetail(tileAttributeDetail.x, tileAttributeDetail.y, tileAttributeDetail, saveScene.tileAttributeDetailData);
                    }
                }
            }
        }
        ShowTileAttributeDetail();
    }
    private void Awake()
    {
        saveID = GetComponent<GUIDGenerator>().GUID;
        saveObject = new SaveObject();
        digTile = editableDigTile;
        waterTile = editableWaterTile;
        plantList = editablePlantList;
    }

    private void Start()
    {
        foreach(TileAttributes tileAttributes in tileAttributesArr)
        {
            Dictionary<string, TileAttributeDetail> tileAttributeDict = new Dictionary<string, TileAttributeDetail>();
            foreach(TileAttribute tileAttribute in tileAttributes.tileAttributes)
            {
                TileAttributeDetail tileAttributeDetail;
                tileAttributeDetail = GetTileAttributeDetail(tileAttribute.tilePosition.xPos, tileAttribute.tilePosition.yPos, tileAttributeDict);

                if (tileAttributeDetail == null) tileAttributeDetail = new TileAttributeDetail();

                switch(tileAttribute.tileBoolean)
                {
                    case TileBoolean.isDroppable:
                        tileAttributeDetail.isDroppable = tileAttribute.tileBool;
                        break;

                    case TileBoolean.isBlock:
                        tileAttributeDetail.isBlock = tileAttribute.tileBool;
                        break;

                    case TileBoolean.isPath:
                        tileAttributeDetail.isPath = tileAttribute.tileBool;
                        break;

                    case TileBoolean.isDiggable:
                        tileAttributeDetail.isDiggable = tileAttribute.tileBool;
                        break;

                    case TileBoolean.isFurniturable:
                        tileAttributeDetail.isFurniturable = tileAttribute.tileBool;
                        break;

                    default:
                        break;
                }
                SetTileAttributeDetail(tileAttribute.tilePosition.xPos, tileAttribute.tilePosition.yPos, tileAttributeDetail, tileAttributeDict);
            }
            SaveScene saveScene = new SaveScene();
            saveScene.tileAttributeDetailData = tileAttributeDict;

            if(tileAttributes.sceneType.ToString() == SceneController.sceneName.ToString())
            {
                tileAttributeDetailDict = tileAttributeDict;
            }

            saveScene.boolData = new Dictionary<string, bool>();
            saveScene.boolData.Add("isSceneLoadInit", true);
            SaveObject.scene.Add(tileAttributes.sceneType.ToString(), saveScene);
        }
    }

    public static TileAttributeDetail GetTileAttributeDetail(int x, int y, Dictionary<string, TileAttributeDetail> tileAttributeDetailDict)
    {
        TileAttributeDetail tileAttributeDetail;

        return tileAttributeDetailDict.TryGetValue("x" + x + "y" + y, out tileAttributeDetail) ? tileAttributeDetail : null;
        
    }
    public static void SetTileAttributeDetail(int x, int y, TileAttributeDetail tileAttributeDetail, Dictionary<string, TileAttributeDetail> tileAttributeDict)
    {
        tileAttributeDetail.x = x;
        tileAttributeDetail.y = y;

        tileAttributeDict["x" + x + "y" + y] = tileAttributeDetail;
    }

    public static TileAttributeDetail GetTileAttributeDetail(int x, int y)
    {
        return GetTileAttributeDetail(x, y, tileAttributeDetailDict);
    }

    public static void SetTileAttributeDetail(int x, int y, TileAttributeDetail tileAttributeDetail)
    {
        SetTileAttributeDetail(x, y, tileAttributeDetail, tileAttributeDetailDict);
    }

    public static PlantInfo GetPlantInfo(int seedNo)
    {
        return plantList.GetPlantInfo(seedNo);
    }

    public static Plant GetPlantInPosition(TileAttributeDetail tileAttributeDetail)
    {
        Vector3 point = grid.GetCellCenterWorld(new Vector3Int(tileAttributeDetail.x, tileAttributeDetail.y, 0));
        Collider2D[] colliders = Physics2D.OverlapPointAll(point);
        Plant plant = null;

        for(int i=0; i<colliders.Length; i++)
        {
            plant = colliders[i].gameObject.GetComponentInParent<Plant>();
            if (plant != null && plant.plantTileLocation == new Vector2Int(tileAttributeDetail.x, tileAttributeDetail.y)) break;

            plant = colliders[i].gameObject.GetComponentInChildren<Plant>();
            if (plant != null && plant.plantTileLocation == new Vector2Int(tileAttributeDetail.x, tileAttributeDetail.y)) break;
        }
        return plant;
    }

    private void ClearShowTileAttributeDetail()
    {
        surroundings1.ClearAllTiles();
        surroundings2.ClearAllTiles();

        Plant[] plants;
        plants = FindObjectsOfType<Plant>();
        foreach (Plant plant in plants) Destroy(plant.gameObject);
    }

    public static void ShowDigTile(TileAttributeDetail tileAttributeDetail)
    {
        if(tileAttributeDetail.ageDig != -1)
        {
            Tile dig0 = DigTile(tileAttributeDetail.x, tileAttributeDetail.y);
            surroundings1.SetTile(new Vector3Int(tileAttributeDetail.x, tileAttributeDetail.y, 0), dig0);

            TileAttributeDetail neighborTileAttributeDetail;
            neighborTileAttributeDetail = GetTileAttributeDetail(tileAttributeDetail.x, tileAttributeDetail.y + 1);
            if(neighborTileAttributeDetail != null && neighborTileAttributeDetail.ageDig != -1)
            {
                Tile dig1 = DigTile(tileAttributeDetail.x, tileAttributeDetail.y + 1);
                surroundings1.SetTile(new Vector3Int(tileAttributeDetail.x, tileAttributeDetail.y + 1, 0), dig1);
            }
            neighborTileAttributeDetail = GetTileAttributeDetail(tileAttributeDetail.x, tileAttributeDetail.y - 1);
            if (neighborTileAttributeDetail != null && neighborTileAttributeDetail.ageDig != -1)
            {
                Tile dig2 = DigTile(tileAttributeDetail.x, tileAttributeDetail.y - 1);
                surroundings1.SetTile(new Vector3Int(tileAttributeDetail.x, tileAttributeDetail.y - 1, 0), dig2);
            }
            neighborTileAttributeDetail = GetTileAttributeDetail(tileAttributeDetail.x + 1, tileAttributeDetail.y);
            if (neighborTileAttributeDetail != null && neighborTileAttributeDetail.ageDig != -1)
            {
                Tile dig3 = DigTile(tileAttributeDetail.x + 1, tileAttributeDetail.y);
                surroundings1.SetTile(new Vector3Int(tileAttributeDetail.x + 1, tileAttributeDetail.y, 0), dig3);
            }

            neighborTileAttributeDetail = GetTileAttributeDetail(tileAttributeDetail.x - 1, tileAttributeDetail.y);
            if (neighborTileAttributeDetail != null && neighborTileAttributeDetail.ageDig != -1)
            {
                Tile dig4 = DigTile(tileAttributeDetail.x - 1, tileAttributeDetail.y);
                surroundings1.SetTile(new Vector3Int(tileAttributeDetail.x - 1, tileAttributeDetail.y, 0), dig4);
            }
            
        }
    }

    public static void ShowWaterTile(TileAttributeDetail tileAttributeDetail)
    {
        if (tileAttributeDetail.ageWater != -1)
        {
            Tile water0 = WaterTile(tileAttributeDetail.x, tileAttributeDetail.y);
            surroundings2.SetTile(new Vector3Int(tileAttributeDetail.x, tileAttributeDetail.y, 0), water0);

            TileAttributeDetail neighborTileAttributeDetail;
            neighborTileAttributeDetail = GetTileAttributeDetail(tileAttributeDetail.x, tileAttributeDetail.y + 1);
            if (neighborTileAttributeDetail != null && neighborTileAttributeDetail.ageWater != -1)
            {
                Tile water1 = WaterTile(tileAttributeDetail.x, tileAttributeDetail.y + 1);
                surroundings2.SetTile(new Vector3Int(tileAttributeDetail.x, tileAttributeDetail.y + 1, 0), water1);
            }
            neighborTileAttributeDetail = GetTileAttributeDetail(tileAttributeDetail.x, tileAttributeDetail.y - 1);
            if (neighborTileAttributeDetail != null && neighborTileAttributeDetail.ageWater != -1)
            {
                Tile water2 = WaterTile(tileAttributeDetail.x, tileAttributeDetail.y - 1);
                surroundings2.SetTile(new Vector3Int(tileAttributeDetail.x, tileAttributeDetail.y - 1, 0), water2);
            }
            neighborTileAttributeDetail = GetTileAttributeDetail(tileAttributeDetail.x + 1, tileAttributeDetail.y);
            if (neighborTileAttributeDetail != null && neighborTileAttributeDetail.ageWater != -1)
            {
                Tile water3 = WaterTile(tileAttributeDetail.x + 1, tileAttributeDetail.y);
                surroundings2.SetTile(new Vector3Int(tileAttributeDetail.x + 1, tileAttributeDetail.y, 0), water3);
            }

            neighborTileAttributeDetail = GetTileAttributeDetail(tileAttributeDetail.x - 1, tileAttributeDetail.y);
            if (neighborTileAttributeDetail != null && neighborTileAttributeDetail.ageWater != -1)
            {
                Tile water4 = WaterTile(tileAttributeDetail.x - 1, tileAttributeDetail.y);
                surroundings2.SetTile(new Vector3Int(tileAttributeDetail.x - 1, tileAttributeDetail.y, 0), water4);
            }

        }
    }

    public static void ShowPlantTile(TileAttributeDetail tileAttributeDetail)
    {
        if (tileAttributeDetail.seedNo != -1)
        {
            PlantInfo plantInfo = plantList.GetPlantInfo(tileAttributeDetail.seedNo);

            GameObject plantObject;

            int currentAgeStep = 0;
            int ageSteps = plantInfo.age.Length;

            for (int i = ageSteps - 1; i >= 0; i--)
            {
                if (tileAttributeDetail.age >= plantInfo.age[i])
                {
                    currentAgeStep = i;
                    break;
                }
            }

            plantObject = plantInfo.agingObject[currentAgeStep];
            Sprite ageSprite = plantInfo.ageSprite[currentAgeStep];

            Vector3 point = surroundings2.CellToWorld(new Vector3Int(tileAttributeDetail.x, tileAttributeDetail.y, 0));
            point = new Vector3(point.x + gridSize / 2, point.y + gridSize / 2, point.z);

            GameObject plantGenerated = Instantiate(plantObject, point, Quaternion.identity);

            plantGenerated.GetComponentInChildren<SpriteRenderer>().sprite = ageSprite;
            plantGenerated.transform.SetParent(plantGroupTransform);
            plantGenerated.GetComponent<Plant>().plantTileLocation = new Vector2Int(tileAttributeDetail.x, tileAttributeDetail.y);

        }
    }
    private static Tile DigTile(int x, int y)
    {
        bool up = CheckDig(x, y + 1);
        bool down = CheckDig(x, y - 1);
        bool right = CheckDig(x + 1, y);
        bool left = CheckDig(x - 1, y);

        if(!up && !down && !right && !left)
        {
            return digTile[0];
        }
        else if (!up && down && right && !left)
        {
            return digTile[1];
        }
        else if (!up && down && right && left)
        {
            return digTile[2];
        }
        else if (!up && down && !right && left)
        {
            return digTile[3];
        }
        else if (!up && down && !right && !left)
        {
            return digTile[4];
        }
        else if (up && down && right && !left)
        {
            return digTile[5];
        }
        else if (up && down && right && left)
        {
            return digTile[6];
        }
        else if (up && down && !right && left)
        {
            return digTile[7];
        }
        else if (up && down && !right && !left)
        {
            return digTile[8];
        }
        else if (up && !down && right && !left)
        {
            return digTile[9];
        }
        else if (up && !down && right && left)
        {
            return digTile[10];
        }
        else if (up && !down && !right && left)
        {
            return digTile[11];
        }
        else if (up && !down && !right && !left)
        {
            return digTile[12];
        }
        else if (!up && !down && right && !left)
        {
            return digTile[13];
        }
        else if (!up && !down && right && left)
        {
            return digTile[14];
        }
        else if (!up && !down && !right && left)
        {
            return digTile[15];
        }

        return null;
    }

    private static Tile WaterTile(int x, int y)
    {
        bool up = CheckWater(x, y + 1);
        bool down = CheckWater(x, y - 1);
        bool right = CheckWater(x + 1, y);
        bool left = CheckWater(x - 1, y);

        if (!up && !down && !right && !left)
        {
            return waterTile[0];
        }
        else if (!up && down && right && !left)
        {
            return waterTile[1];
        }
        else if (!up && down && right && left)
        {
            return waterTile[2];
        }
        else if (!up && down && !right && left)
        {
            return waterTile[3];
        }
        else if (!up && down && !right && !left)
        {
            return waterTile[4];
        }
        else if (up && down && right && !left)
        {
            return waterTile[5];
        }
        else if (up && down && right && left)
        {
            return waterTile[6];
        }
        else if (up && down && !right && left)
        {
            return waterTile[7];
        }
        else if (up && down && !right && !left)
        {
            return waterTile[8];
        }
        else if (up && !down && right && !left)
        {
            return waterTile[9];
        }
        else if (up && !down && right && left)
        {
            return waterTile[10];
        }
        else if (up && !down && !right && left)
        {
            return waterTile[11];
        }
        else if (up && !down && !right && !left)
        {
            return waterTile[12];
        }
        else if (!up && !down && right && !left)
        {
            return waterTile[13];
        }
        else if (!up && !down && right && left)
        {
            return waterTile[14];
        }
        else if (!up && !down && !right && left)
        {
            return waterTile[15];
        }

        return null;
    }

 
    private static bool CheckDig(int x, int y)
    {
        TileAttributeDetail tileAttributeDetail = GetTileAttributeDetail(x, y);
        if (tileAttributeDetail == null) return false;
        else if (tileAttributeDetail.ageDig != -1) return true;
        else return false;
    }

    private static bool CheckWater(int x, int y)
    {
        TileAttributeDetail tileAttributeDetail = GetTileAttributeDetail(x, y);
        if (tileAttributeDetail == null) return false;
        else if (tileAttributeDetail.ageWater != -1) return true;
        else return false;
    }

    private void ShowTileAttributeDetail()
    {
        foreach(KeyValuePair<string, TileAttributeDetail> item in tileAttributeDetailDict)
        {
            TileAttributeDetail tileAttributeDetail = item.Value;
            ShowDigTile(tileAttributeDetail);
            ShowWaterTile(tileAttributeDetail);
            ShowPlantTile(tileAttributeDetail);
        }
    }

    public void Recover(string scene)
    {
        if(SaveObject.scene.TryGetValue(scene, out SaveScene saveScene))
        {
            if (saveScene.tileAttributeDetailData != null) tileAttributeDetailDict = saveScene.tileAttributeDetailData;
        }

        if (saveScene.boolData != null && saveScene.boolData.TryGetValue("isSceneLoadInit", out bool isSceneLoadInitInDict)) isSceneLoadInit = isSceneLoadInitInDict;
        if (isSceneLoadInit) EventHandler.CallGeneratePlant();

        if(tileAttributeDetailDict.Count > 0)
        {
            ClearShowTileAttributeDetail();
            ShowTileAttributeDetail();
        }

        if (isSceneLoadInit) isSceneLoadInit = false;
    }

    public void Register()
    {
        SaveManager.saveList.Add(this);
    }

    public void Store(string scene)
    {
        SaveObject.scene.Remove(scene);
        SaveScene saveScene = new SaveScene();

        saveScene.tileAttributeDetailData = tileAttributeDetailDict;
        saveScene.boolData = new Dictionary<string, bool>();
        saveScene.boolData.Add("isSceneLoadInit", isSceneLoadInit);
        SaveObject.scene.Add(scene, saveScene);
    }

    public void Unregister()
    {
        SaveManager.saveList.Remove(this);
    }

    public void Load(SaveGame saveGame)
    {
        if(saveGame.saveObjectDict.TryGetValue(SaveID, out SaveObject saveObject))
        {
            SaveObject = saveObject;
            Recover(SceneManager.GetActiveScene().name);
        }
    }

    public SaveObject Save()
    {
        Store(SceneManager.GetActiveScene().name);
        return SaveObject;
    }
}
