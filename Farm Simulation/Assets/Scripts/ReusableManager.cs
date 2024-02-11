using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReusableManager : MonoBehaviour
{
    private static Dictionary<int, Queue<GameObject>> reusables = new Dictionary<int, Queue<GameObject>>();
    [SerializeField] private Reusable[] reusable = null;
    [SerializeField] private Transform reusableTransform = null;

    [System.Serializable]
    public struct Reusable
    {
        public GameObject reusableObject;
        public int size;
    }
    void Start()
    {
        for (int i = 0; i < reusable.Length; i++)
        {
            string reusableName = reusable[i].reusableObject.name;
            int key = reusable[i].reusableObject.GetInstanceID();

            GameObject gameObjectGroup = new GameObject(reusableName + "Group");
            gameObjectGroup.transform.SetParent(reusableTransform);

            if(!reusables.ContainsKey(key))
            {
                reusables.Add(key, new Queue<GameObject>());
                for (int j = 0; j < reusable[i].size; j++)
                {
                    GameObject newReusableObject = Instantiate(reusable[i].reusableObject, gameObjectGroup.transform) as GameObject;
                    newReusableObject.SetActive(false);
                    reusables[key].Enqueue(newReusableObject);
                }
            }

        }
    }
    
    public static GameObject Reuse(GameObject reusableObject, Vector3 location, Quaternion angle)
    {
        int key = reusableObject.GetInstanceID();
        if (reusables.ContainsKey(key))
        {
            GameObject reusingObject = reusables[key].Dequeue();
            reusables[key].Enqueue(reusingObject);
            if (reusingObject.activeSelf) reusingObject.SetActive(false);

            reusingObject.transform.position = location;
            reusingObject.transform.rotation = angle;
            reusingObject.transform.localScale = reusableObject.transform.localScale;
            return reusingObject;
        }
        else return null;
    }
}
