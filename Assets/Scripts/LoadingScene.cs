using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject lightObj;
    [SerializeField] Transform loadingText;
    [SerializeField] Animator loadingTextAnim;
    [SerializeField] Slider loadingBar;
    [SerializeField] CanvasGroup hintContainer;
    [SerializeField] List<GameObject> textList;
    [SerializeField] float loadingSpeed;
    [SerializeField] float percent;
    [SerializeField] float seconds;
    [SerializeField] TweenSettings<float> alpha;
    [SerializeField] TweenSettings<float> alphaGroup;

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

    int currentIndex = 0;
    bool hasPoweredDown;

    public void SetPowerDown(bool value) { hasPoweredDown = value; }
    public bool HasPoweredDown() { return hasPoweredDown; } 

    void Start()
    {
        if(texts.Count != 0) {
            foreach (var t in texts)
            {
                if (t != null)
                    _originalColors[t] = t.color;
            }

            if(SequenceManager.Instance.GetCurrentScene() == CurrentScene.LOADING4 ||
               SequenceManager.Instance.GetCurrentScene() == CurrentScene.LOADING5)  StartCoroutine(BlinkLoop());
        }
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

    public void PlayTextAnim()
    {
        loadingTextAnim.enabled = true;
    }

    public void PauseTextAnim()
    {
        loadingTextAnim.speed = 0f;
    }

    public void ResumeTextAnim()
    {
        loadingTextAnim.speed = 1f;
    }

    public void PlayLoadingBar()
    {
        StartCoroutine(PlayLoadingBarCoroutine());
    }

    public void PlayLoading2()
    {
        StartCoroutine(PlayLoading2Coroutine());
    }

    IEnumerator PlayLoading2Coroutine()
    {
        yield return null;

        while(loadingBar.value < 100)
        {
            yield return new WaitForSeconds(seconds);
            percent = loadingBar.value;
            loadingBar.value += loadingSpeed;
        }

        yield return new WaitForSeconds(1f);
        //SequenceManager.Instance.NextScene(CurrentScene.ROOM2);
        if (SequenceManager.Instance.GetCurrentScene() == CurrentScene.LOADING2) SequenceManager.Instance.NextScene(CurrentScene.ROOM2);
        else if (SequenceManager.Instance.GetCurrentScene() == CurrentScene.LOADING4) SequenceManager.Instance.NextScene(CurrentScene.ROOM3);
        else if (SequenceManager.Instance.GetCurrentScene() == CurrentScene.LOADING5) SequenceManager.Instance.NextScene(CurrentScene.ROOM4);
    }

    IEnumerator PlayLoadingBarCoroutine()
    {
        yield return null;

        while(loadingBar.value <= 25)
        {
            yield return new WaitForSeconds(seconds);
            percent = loadingBar.value;
            loadingBar.value += loadingSpeed;
        }

        SetPowerDown(true);
        SFXManager.Instance.PauseMusic();
        SFXManager.Instance.PlaySFX("PowerDown");
        yield return new WaitForSeconds(1f);
        SFXManager.Instance.PlayCustomMusic(2);

        PauseTextAnim();
    }

    public void ResumeLoadingBar()
    {
        StartCoroutine(ResumeLoadingBarCoroutine());
    }

    IEnumerator ResumeLoadingBarCoroutine()
    {
        yield return null;

        SFXManager.Instance.StopCustomMusic();
        yield return new WaitForSeconds(0.5f);
        SFXManager.Instance.PlaySFX("PowerOn");
        yield return new WaitForSeconds(0.5f);
        SFXManager.Instance.ResumeMusic();

        ResumeTextAnim();
        
        while(loadingBar.value < 100)
        {
            yield return new WaitForSeconds(seconds / 2);
            percent = loadingBar.value;
            loadingBar.value += loadingSpeed;
        }

        yield return new WaitForSeconds(1f);
        SFXManager.Instance.StopSFX();
        if (SequenceManager.Instance.GetCurrentScene() == CurrentScene.LOADING1) SequenceManager.Instance.NextScene(CurrentScene.ROOM1);
        else if (SequenceManager.Instance.GetCurrentScene() == CurrentScene.LOADING3) SequenceManager.Instance.NextScene(CurrentScene.TITLE2);
        else if (SequenceManager.Instance.GetCurrentScene() == CurrentScene.LOADING4) SequenceManager.Instance.NextScene(CurrentScene.ROOM3);
    }

    public void ShowHintBar()
    {
        Tween.Alpha(hintContainer, alpha);
    }

    public void NextHint()
    {
        currentIndex = (currentIndex + 1) % textList.Count;
        HideHints();
        ShowHint(currentIndex);
    }

    public void PreviousHint()
    {
        currentIndex = (currentIndex - 1 + textList.Count) % textList.Count;
        HideHints();
        ShowHint(currentIndex);
    }

    void HideHints()
    {
        for(int i = 0; i < textList.Count; i++)
        {
            textList[i].SetActive(false);
        }
    }

    void ShowHint(int index)
    {
        textList[index].SetActive(true);
    }

    public void TurnTextToNormal()
    {
        textList[2].GetComponent<TextMeshProUGUI>().text = "Let there be <color=#696969>LIGHT</color>!";
    }

    public void RevealUI()
    {
        Tween.Alpha(canvasGroup, alphaGroup);
    }

    public void RevealLight()
    {
        lightObj.SetActive(true);
    }
}
