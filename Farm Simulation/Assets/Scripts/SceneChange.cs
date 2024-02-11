using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private SceneType sceneType = SceneType.Farm;
    [SerializeField] private Vector3 position = new Vector3();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        float xCor = Mathf.Approximately(position.x, 0f) ? player.transform.position.x : position.x;
        float yCor = Mathf.Approximately(position.y, 0f) ? player.transform.position.y : position.y;

        SceneController.FadeOutLoad(sceneType.ToString(), new Vector3(xCor, yCor, 0f));
    }
}
