using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Room5Controller : MonoBehaviour
    {
        public SpriteRenderer FadeOverlay;

        private void OnEnable()
        {
            StartCoroutine(EndGameSequence());
        }

        private IEnumerator EndGameSequence()
        {
            var player = PlayerController.Instance;
            player.IsControlled = true;

            player.ForceMovement = PlayerController.ControlledState.Idle;
            yield return new WaitForSeconds(5);

            player.ForceMovement = PlayerController.ControlledState.MoveLeft;
            yield return new WaitForSeconds(1);

            player.ForceMovement = PlayerController.ControlledState.Idle;
            yield return new WaitForSeconds(1);

            player.ForceMovement = PlayerController.ControlledState.MoveRight;
            yield return new WaitForSeconds(2);

            player.ForceMovement = PlayerController.ControlledState.Idle;
            yield return new WaitForSeconds(1);

            player.ForceMovement = PlayerController.ControlledState.MoveLeft;
            yield return new WaitForSeconds(4);

            FadeOverlay.enabled = true;
            FadeOverlay.color = new Color(FadeOverlay.color.r, FadeOverlay.color.g, FadeOverlay.color.b, 0);
            FadeOverlay.DOFade(1, 2);

            yield return new WaitForSeconds(2);

            SceneManager.LoadScene(0);
        }
    }
}