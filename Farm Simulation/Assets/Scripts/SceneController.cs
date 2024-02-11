using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneType
{
    Farm,
    Home,
    Ground
}
public class SceneController : MonoBehaviour
{
    [SerializeField] private static float duration = 0.2f;
    [SerializeField] private CanvasGroup canvasParent = null;
    [SerializeField] private static CanvasGroup canvasGroup = null;
    [SerializeField] private Image image = null;
    private static bool isFade;
    public SceneType scene;
    public static SceneType sceneName;
    private static SceneController instance;

    public void Awake()
    {
        canvasGroup = canvasParent;
        instance = this;
        sceneName = scene;
    }
    public IEnumerator Start()
    {
        image.color = new Color(0f, 0f, 0f, 1f);
        canvasGroup.alpha = 1f;

        yield return StartCoroutine(Load(sceneName.ToString()));
        EventHandler.CallAfterLoad();
        SaveManager.RecoverScene();

        StartCoroutine(FadeOut(0f));
    }

    public static void FadeOutLoad(string scene, Vector3 location)
    {
        if (!isFade)
        {
            instance.StartCoroutine(FadeOutChange(scene, location));
        }
    }

    private static IEnumerator FadeOutChange(string scene, Vector3 location)
    {
        EventHandler.CallBeforeFadeOut();
        yield return instance.StartCoroutine(FadeOut(1f));

        SaveManager.StoreScene();

        Player.go.transform.position = location;

        EventHandler.CallBeforeUnload();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return instance.StartCoroutine(Load(scene));

        EventHandler.CallAfterLoad();

        SaveManager.RecoverScene();
        
        yield return instance.StartCoroutine(FadeOut(0f));
        EventHandler.CallAfterFadeIn();

    }

    private static IEnumerator FadeOut(float alp)
    {
        isFade = true;
        canvasGroup.blocksRaycasts = true;

        float speed = Mathf.Abs(canvasGroup.alpha - alp) / duration;
        while (!Mathf.Approximately(canvasGroup.alpha, alp))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alp, speed * Time.deltaTime);
            yield return null;
        }

        isFade = false;
        canvasGroup.blocksRaycasts = false;
    }

    private static IEnumerator Load(string scene)
    {
        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        SceneManager.SetActiveScene(newScene);
    }
}
