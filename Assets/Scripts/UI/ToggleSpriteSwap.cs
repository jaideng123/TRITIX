using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggleSpriteSwap : MonoBehaviour
{

    public Toggle targetToggle;
    public Sprite onSprite;
    public Sprite offSprite;

    // Use this for initialization
    void Start()
    {
        targetToggle.toggleTransition = Toggle.ToggleTransition.None;
        targetToggle.onValueChanged.AddListener(OnTargetToggleValueChanged);
        Image targetImage = targetToggle.targetGraphic as Image;
        if (targetToggle.isOn)
        {
            targetImage.overrideSprite = onSprite;
        }
        else
        {
            targetImage.overrideSprite = offSprite;
        }
    }

    void OnTargetToggleValueChanged(bool newValue)
    {
        Image targetImage = targetToggle.targetGraphic as Image;
        if (targetImage != null)
        {
            if (newValue)
            {
                targetImage.overrideSprite = onSprite;
            }
            else
            {
                targetImage.overrideSprite = offSprite;
            }
        }
    }
}
