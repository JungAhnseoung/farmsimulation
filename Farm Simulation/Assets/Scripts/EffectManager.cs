using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FarmEffectType
{
    Wood,
    BroadLeafLeaves,
    NeedieLeaves,
    Stone,
    Grass,
    Animal,
    None
}
public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject grass = null;
    [SerializeField] private GameObject needieLeaves = null;
    [SerializeField] private GameObject broadLeafLeaves = null;
    [SerializeField] private GameObject wood = null;
    [SerializeField] private GameObject stone = null;
    [SerializeField] private GameObject animal = null;
    private void OnEnable()
    {
        EventHandler.FarmEffect += ShowFarmEffect;
    }

    private void OnDisable()
    {
        EventHandler.FarmEffect -= ShowFarmEffect; 
    }

    private void ShowFarmEffect(Vector3 effectLocation, FarmEffectType farmEffectType)
    {
        switch(farmEffectType)
        {
            case FarmEffectType.Grass:
                GameObject scytheEffect = ReusableManager.Reuse(grass, effectLocation, Quaternion.identity);
                scytheEffect.SetActive(true);
                StartCoroutine(DisableFarmEffect(scytheEffect));
                break;
            case FarmEffectType.BroadLeafLeaves:
                GameObject blTreeEffect = ReusableManager.Reuse(broadLeafLeaves, effectLocation, Quaternion.identity);
                blTreeEffect.SetActive(true);
                StartCoroutine(DisableFarmEffect(blTreeEffect));
                break;
            case FarmEffectType.NeedieLeaves:
                GameObject nTreeEffect = ReusableManager.Reuse(needieLeaves, effectLocation, Quaternion.identity);
                nTreeEffect.SetActive(true);
                StartCoroutine(DisableFarmEffect(nTreeEffect));
                break;
            case FarmEffectType.Wood:
                GameObject trunkEffect = ReusableManager.Reuse(wood, effectLocation, Quaternion.identity);
                trunkEffect.SetActive(true);
                StartCoroutine(DisableFarmEffect(trunkEffect));
                break;
            case FarmEffectType.Stone:
                GameObject stoneEffect = ReusableManager.Reuse(stone, effectLocation, Quaternion.identity);
                stoneEffect.SetActive(true);
                StartCoroutine(DisableFarmEffect(stoneEffect));
                break;
            case FarmEffectType.Animal:
                GameObject animalEffect = ReusableManager.Reuse(animal, effectLocation, Quaternion.identity);
                animalEffect.SetActive(true);
                StartCoroutine(DisableFarmEffect(animalEffect));
                break;
            default:
                break;
        }
    }

    private IEnumerator DisableFarmEffect(GameObject effect)
    {
        yield return new WaitForSeconds(2f);
        effect.SetActive(false);
    }
}
