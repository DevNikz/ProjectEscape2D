using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleAnimater : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] Transform Logo;
    [SerializeField] Transform text;
    [SerializeField] Button bgButton;
    [SerializeField] Transform popUp1, popUp2, popUp3;


    [Header("Properties")]
    [SerializeField] TweenSettings<float> yLogoPos;
    [SerializeField] TweenSettings<Color> textAlpha;
    [SerializeField] TweenSettings<float> scale1, scale2, scale3;

    [SerializeField] GameObject scareParent;

    [Header("Target Texts")]
    [Tooltip("All TMP texts that can participate in the effect.")]
    public List<TextMeshProUGUI> texts = new();
 
    [Header("Cycle Settings")]
    [Tooltip("How many texts to blink per cycle (0 = random 1 to all).")]
    public int textsPerCycle = 0;
 
    [Tooltip("Time between each full blink cycle.")]
    public float cycleInterval = 0.2f;
 
    [Tooltip("Randomness added to cycleInterval each cycle.")]
    public float cycleJitter = 0.15f;
 
    [Header("Blink Settings")]
    [Tooltip("How many times each selected text blinks per cycle.")]
    [Range(1, 10)]
    public int blinksPerCycle = 3;
 
    [Tooltip("Min duration of a single on/off blink.")]
    public float minBlinkDuration = 0.02f;
 
    [Tooltip("Max duration of a single on/off blink.")]
    public float maxBlinkDuration = 0.12f;
 
    [Tooltip("Minimum alpha when a text is 'off' (0 = fully invisible).")]
    [Range(0f, 1f)]
    public float minAlpha = 0f;

    [Header("Color Glitch (optional)")]
    [Tooltip("Chance (0–1) that a text gets a random color glitch during a blink.")]
    [Range(0f, 1f)]
    public float colorGlitchChance = 0.3f;
 
    [Tooltip("How saturated/extreme the glitch colors can be.")]
    [Range(0f, 1f)]
    public float colorGlitchIntensity = 0.8f;
 
    // Stores original colors so we can restore them
    private Dictionary<TextMeshProUGUI, Color> _originalColors = new();

    Sequence sequenceText;

    void Start()
    {
        if(texts.Count != 0) {
            foreach (var t in texts)
            {
                if (t != null)
                    _originalColors[t] = t.color;
            }
        }

        popUp1.localScale = Vector3.zero;
        popUp2.localScale = Vector3.zero;
        popUp3.localScale = Vector3.zero;
    }

    public void PlayLogo()
    {
        Sequence.Create(1)
            .Chain(Tween.PositionY(Logo, yLogoPos));
    }

    public void PlayLogo2()
    {
        Tween.UIAnchoredPositionY(Logo.GetComponent<RectTransform>(), yLogoPos);
    }

    public void PlayBlinkText()
    {
        sequenceText = Sequence.Create(10, Sequence.SequenceCycleMode.Yoyo)
            .Chain(Tween.Custom(textAlpha, onValueChange: newVal => text.GetComponent<TextMeshProUGUI>().color = newVal))
            .OnComplete(PlayBlinkText);
    }

    public void EnableButton()
    {
        bgButton.enabled = true;
    }

    public void DisableButton()
    {
        bgButton.enabled = false;
    }

    public void OpenPopUp(int value)
    {
        switch(value)
        {
            case 1:
                sequenceText.timeScale = 0f;
                popUp1.gameObject.SetActive(true);
                ShowPop1();
                ShowScareText();
                SetupScareText();
                break;
            case 2:
                popUp2.gameObject.SetActive(true);
                ShowPop2();
                break;
            case 3:
                popUp3.gameObject.SetActive(true);
                ShowPop3();
                break;
        }
    }

    public void ShowScareText() { scareParent.SetActive(true); }
    public void HideScareText() { scareParent.SetActive(false); }

    public void ShowPop1()
    {
        Sequence.Create(1)
            .Chain(Tween.Scale(popUp1, scale1));
    }

    public void ShowPop2()
    {
        Sequence.Create(1)
            .Chain(Tween.Scale(popUp2, scale2));
    }

    public void ShowPop3()
    {
        Sequence.Create(1)
            .Chain(Tween.Scale(popUp3, scale3));
    }

    public void NextScene()
    {
        if (SequenceManager.Instance.GetCurrentScene() == CurrentScene.TITLE) SequenceManager.Instance.NextScene(CurrentScene.NEXT_TITLE);
        else if (SequenceManager.Instance.GetCurrentScene() == CurrentScene.TITLE2) SequenceManager.Instance.NextScene(CurrentScene.NEXT_TITLE2);
    }

    public void DebugText()
    {
        Debug.Log("Button Clicked Here.");
        sequenceText.timeScale = 0f;
    }

    public void SetupScareText()
    {
        
        StartCoroutine(BlinkLoop());
    }

    IEnumerator BlinkLoop()
    {
        while (true)
        {
            // Pick how many texts to affect this cycle
            int count = textsPerCycle > 0
                ? Mathf.Clamp(textsPerCycle, 1, texts.Count)
                : Random.Range(1, texts.Count + 1);
 
            // Shuffle and take a subset
            List<TextMeshProUGUI> shuffled = new(texts);
            Shuffle(shuffled);
            List<TextMeshProUGUI> selected = shuffled.GetRange(0, count);
 
            // Launch a blink coroutine for each selected text
            List<Coroutine> running = new();
            foreach (var tmp in selected)
            {
                if (tmp != null)
                    running.Add(StartCoroutine(BlinkText(tmp)));
            }
 
            // Wait for all blinks to finish
            foreach (var c in running)
                yield return c;
 
            // Wait before next cycle
            float wait = cycleInterval + Random.Range(0f, cycleJitter);
            yield return new WaitForSeconds(wait);
        }
    }

    IEnumerator BlinkText(TextMeshProUGUI tmp)
    {
        Color original = _originalColors.ContainsKey(tmp) ? _originalColors[tmp] : tmp.color;

        for (int i = 0; i < blinksPerCycle; i++)
        {
            // --- OFF state ---
            float offAlpha = Random.Range(minAlpha, 0.3f);
            Color offColor = original;
            offColor.a = offAlpha;
 
            // Optional color glitch on the OFF state
            if (Random.value < colorGlitchChance)
                offColor = GetGlitchColor(original, offAlpha);
 
            tmp.color = offColor;
            yield return new WaitForSeconds(Random.Range(minBlinkDuration, maxBlinkDuration));
 
            // --- ON state ---
            Color onColor = original;
 
            // Occasionally glitch the ON state too (less likely)
            if (Random.value < colorGlitchChance * 0.5f)
                onColor = GetGlitchColor(original, original.a);
 
            tmp.color = onColor;
            yield return new WaitForSeconds(Random.Range(minBlinkDuration, maxBlinkDuration));
        }

        // Always restore to original color
        tmp.color = original;
    }

    private Color GetGlitchColor(Color baseColor, float alpha)
    {
        // Shift hue randomly, keep some of the original saturation/value
        Color.RGBToHSV(baseColor, out float h, out float s, out float v);
        h = (h + Random.Range(0.05f, 0.5f)) % 1f;
        s = Mathf.Lerp(s, 1f, colorGlitchIntensity);
        v = Mathf.Lerp(v, 1f, colorGlitchIntensity * 0.5f);
        Color glitch = Color.HSVToRGB(h, s, v);
        glitch.a = alpha;
        return glitch;
    }

    static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
