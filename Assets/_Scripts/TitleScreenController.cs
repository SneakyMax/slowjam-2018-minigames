using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class TitleScreenController : MonoBehaviour
    {
        public Image Fade;

        public float FadeTime;

        private bool starting;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            if (!starting && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || InputManager.Start))
            {
                StartCoroutine(StartGame());
            }
        }

        private IEnumerator StartGame()
        {
            starting = true;
            Fade.enabled = true;
            Fade.color = new Color(Fade.color.r, Fade.color.g, Fade.color.b, 0);
            Fade.DOFade(1, FadeTime);

            yield return new WaitForSeconds(FadeTime);
            SceneManager.LoadScene(1);
        }
    }
}