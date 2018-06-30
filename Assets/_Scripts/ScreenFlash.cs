using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class ScreenFlash : MonoBehaviour
    {
        public static ScreenFlash Instance { get; private set; }
        
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            Instance = this;
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
            spriteRenderer.enabled = true;
        }

        public static void Flash(float time, Ease ease = Ease.OutQuad)
        {
            Instance.spriteRenderer.color = new Color(Instance.spriteRenderer.color.r, Instance.spriteRenderer.color.g, Instance.spriteRenderer.color.b, 1);
            Instance.spriteRenderer.DOFade(0, time).SetEase(ease);
        }
    }
}