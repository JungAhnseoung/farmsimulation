using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class Others
{
    public static bool GetObjectInPositionBox<T>(out List<T> objectsInPosition, Vector2 position, Vector2 scale, float rotation)
    {
        List<T> objects = new List<T>();
        bool isThere = false;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, scale, rotation);
        
        for(int i=0; i<colliders.Length; i++)
        {
            T tObject = colliders[i].gameObject.GetComponentInParent<T>();
            if(tObject != null)
            {
                isThere = true;
                objects.Add(tObject);
            }
            else
            {
                tObject = colliders[i].gameObject.GetComponentInChildren<T>();
                if(tObject != null)
                {
                    isThere = true;
                    objects.Add(tObject);
                }
            }
        }

        objectsInPosition = objects;
        return isThere;
    }
    public static T[] GetObjectInPositionBoxNon<T>(int collidersCount, Vector2 pivot, Vector2 scale, float rotation)
    {
        Collider2D[] colliders = new Collider2D[collidersCount];
        Physics2D.OverlapBoxNonAlloc(pivot, scale, rotation, colliders);

        T tObject = default(T);
        T[] objects = new T[colliders.Length];

        for (int i = colliders.Length - 1; i >= 0; i--)
        {
            if (colliders[i] != null)
            {
                tObject = colliders[i].gameObject.GetComponent<T>(); 
                if(tObject != null) objects[i] = tObject;
            }
        }

        return objects;
    }

    public static bool GetObjectInPosition<T>(out List<T> objectsInPosition, Vector3 position)
    {
        List<T> objects = new List<T>();
        bool isThere = false;
        Collider2D[] colliders = Physics2D.OverlapPointAll(position);

        T tObject = default(T);
        for (int i = 0; i < colliders.Length; i++)
        {
            tObject = colliders[i].gameObject.GetComponentInParent<T>();
            if (tObject != null)
            {
                isThere = true;
                objects.Add(tObject);
            }
            else
            {
                tObject = colliders[i].gameObject.GetComponentInChildren<T>();
                if (tObject != null)
                {
                    isThere = true;
                    objects.Add(tObject);
                }
            }
        }

        objectsInPosition = objects;
        return isThere;
    }
}
