using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Drone.UI {
    public class RacingEffects : MonoBehaviour {
        [SerializeField] CanvasGroup faderElement;
        [SerializeField] float timeToFade = 1f;
        [SerializeField] TextMeshProUGUI countdownText;

        public IEnumerator CountdownEffects() {
            countdownText.text = "3";
            yield return new WaitForSeconds(1f);
            countdownText.text = "2";
            yield return new WaitForSeconds(1f);
            countdownText.text = "1";
            yield return new WaitForSeconds(1f);
            countdownText.text = "Go!";
            yield return new WaitForSeconds(1f);
            countdownText.text = "";
        }

        public IEnumerator FadeOut() {
            while (faderElement.alpha < 1) {
                faderElement.alpha += Time.deltaTime / timeToFade;
                yield return null;
            }
        }

        public IEnumerator FadeIn() {
            while (faderElement.alpha > 0) {
                faderElement.alpha -= Time.deltaTime / timeToFade;
                yield return null;
            }
        }
    }
}

