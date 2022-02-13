using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StarDisplay : MonoBehaviour
{
    public int numStarsInPuzzle;
    public StarCounterSO starCounterSO;

    public Sprite outlineSprite;
    public Sprite filledSprite;
    public Color outlineColor;
    public Color filledColor;

    private List<Image> starImages;

    private void Awake() {
        starImages = GetComponentsInChildren<Image>().ToList();
        while (starImages.Count > numStarsInPuzzle) {
            // delete stars
            Image s = starImages[starImages.Count - 1];
            starImages.RemoveAt(starImages.Count - 1);
            Destroy(s.gameObject);
        }
        while (starImages.Count < numStarsInPuzzle) {
            // create stars
            Image s = starImages[starImages.Count - 1];
            Image newS = Instantiate(s.gameObject, s.rectTransform.position, Quaternion.identity, transform).GetComponent<Image>();
            starImages.Add(newS);
        }

        UpdateDisplay();
    }

    private void Update() {
        UpdateDisplay();
    }

    private void UpdateDisplay() {
        for (int i = 0; i < starImages.Count; i++) {
            starImages[i].sprite = starCounterSO.currentCount > i ? filledSprite : outlineSprite;
            starImages[i].color = starCounterSO.currentCount > i ? filledColor : outlineColor;
        }
    }
}
