using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationOverride : MonoBehaviour
{
    [SerializeField] private GameObject player = null;
    [SerializeField] private AnimationAttribute[] animationAttributes = null;

    private Dictionary<AnimationClip, AnimationAttribute> animationAttrDict;
    private Dictionary<string, AnimationAttribute> animationAttrDict_key;

    void Start()
    {
        animationAttrDict = new Dictionary<AnimationClip, AnimationAttribute>();
        foreach (AnimationAttribute attribute in animationAttributes)
        {
            animationAttrDict.Add(attribute.animationClip, attribute);
        }    

        animationAttrDict_key = new Dictionary<string, AnimationAttribute>();
        foreach(AnimationAttribute attribute in animationAttributes)
        {
            animationAttrDict_key.Add(attribute.characterAnimationAttr.ToString() + attribute.typeAnimationAttr.ToString() + attribute.animationName.ToString(), attribute);
        }
    }

    public void OverrideCharacterAttributes(List<CharacterDetailAttribute> characterDetailAttrs)
    {
        foreach (CharacterDetailAttribute characterDetailAttr in characterDetailAttrs)
        {
            Animator animator = null;
            List<KeyValuePair<AnimationClip, AnimationClip>> keyValuePairs = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            string animatorName = characterDetailAttr.characterAnimationAttr.ToString();

            Animator[] animators = player.GetComponentsInChildren<Animator>();
            foreach(Animator item in animators)
            {
                if(item.name == animatorName)
                {
                    animator = item;
                    break;
                }
            }

            AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            List<AnimationClip> animationClips = new List<AnimationClip>(animatorOverrideController.animationClips);

            foreach(AnimationClip animationClip in animationClips)
            {
                AnimationAttribute animationAttribute;
                if(animationAttrDict.TryGetValue(animationClip, out animationAttribute))
                {
                    AnimationAttribute newAnimationAttribute;
                    if(animationAttrDict_key.TryGetValue(characterDetailAttr.characterAnimationAttr.ToString() + characterDetailAttr.typeAnimationAttr.ToString() + animationAttribute.animationName.ToString(), out newAnimationAttribute))
                    {
                        AnimationClip newAnimationClip = newAnimationAttribute.animationClip;
                        keyValuePairs.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, newAnimationClip));
                    }
                }
            }

            animatorOverrideController.ApplyOverrides(keyValuePairs);
            animator.runtimeAnimatorController= animatorOverrideController;
        }
    }
}
