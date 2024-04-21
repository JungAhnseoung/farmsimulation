using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVisualEffect : MonoBehaviour
{
    private bool isInAnimation = false;
    private float wobbleSpeed;
    public float wobbleWalkSpeed = 1.5f;
    public float wobbleRunSpeed = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isInAnimation == false)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                wobbleSpeed = wobbleRunSpeed;
            }
            else
            {
                wobbleSpeed = wobbleWalkSpeed;
            }
            if (Random.Range(0, 2) == 0)
            {
                if (this.gameObject.activeInHierarchy) StartCoroutine(WobbleRight());
            }
            else
            {
                if (this.gameObject.activeInHierarchy) StartCoroutine(WobbleLeft());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isInAnimation == false)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                wobbleSpeed = wobbleRunSpeed;
            }
            else
            {
                wobbleSpeed = wobbleWalkSpeed;
            }
            if (Random.Range(0, 2) == 0)
            {
                if (this.gameObject.activeInHierarchy) StartCoroutine(WobbleRight());
            }
            else
            {
                if (this.gameObject.activeInHierarchy)  StartCoroutine(WobbleLeft());
            }
        }
    }

    private IEnumerator WobbleRight()
    {
        isInAnimation = true;

        for(int i= 0; i < 3; i++)
        {
            gameObject.transform.Rotate(0f, 0f, wobbleSpeed);
            yield return new WaitForSeconds(0.03f);
        }

        for(int i=0; i< 5; i++)
        {
            gameObject.transform.Rotate(0f, 0f, -wobbleSpeed);
            yield return new WaitForSeconds(0.03f);
        }

        for (int i = 0; i < 2; i++)
        {
            gameObject.transform.Rotate(0f, 0f, wobbleSpeed);
        }

        yield return new WaitForSeconds(0.03f);

        isInAnimation = false;
    }
    private IEnumerator WobbleLeft()
    {
        isInAnimation = true;

        for (int i = 0; i < 3; i++)
        {
            gameObject.transform.Rotate(0f, 0f, -wobbleSpeed);
            yield return new WaitForSeconds(0.03f);
        }

        for (int i = 0; i < 5; i++)
        {
            gameObject.transform.Rotate(0f, 0f, wobbleSpeed);
            yield return new WaitForSeconds(0.03f);
        }

        for (int i = 0; i < 2; i++)
        {
            gameObject.transform.Rotate(0f, 0f, -wobbleSpeed);
        }

        yield return new WaitForSeconds(0.03f);

        isInAnimation = false;
    }
}
