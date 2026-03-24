using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

public class RoomScript : MonoBehaviour
{
    [SerializeField] Transform objective;
    [SerializeField] CanvasGroup canvas;
    [SerializeField] TweenSettings<float> yPos;
    [SerializeField] TweenSettings<float> alpha;
    [SerializeField] AudioMixer master;
    [SerializeField] List<GameObject> uiObjects;
    [SerializeField] GameObject overlay;
    [SerializeField] List<Light2D> lights;

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

    void Start()
    {
        if(texts.Count != 0) {
            foreach (var t in texts)
            {
                if (t != null)
                    _originalColors[t] = t.color;
            }

            if(SequenceManager.Instance.GetCurrentScene() == CurrentScene.ROOM3 ||
               SequenceManager.Instance.GetCurrentScene() == CurrentScene.ROOM4)  StartCoroutine(BlinkLoop());
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

    public void AnimateObjective()
    {
        Sequence.Create(1)
            .Group(Tween.Alpha(canvas, alpha))
            .Group(Tween.PositionY(objective, yPos));   
    }

    public void AdjustMusicVolume(float volume)
    {
        master.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void AdjustSFXVolume(float volume)
    {
        master.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    public void AdjustVoiceVolume(float volume)
    {
        master.SetFloat("VoiceVolume", Mathf.Log10(volume) * 20);
    }

    void DisableInteractableUI()
    {
        for(int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].SetActive(false);
        }

        overlay.SetActive(false);
    }

    void EnableInteractableUI()
    {
        overlay.SetActive(true);
    }

    IEnumerator Scare1()
    {
        yield return null;

        DisableInteractableUI();
        yield return new WaitForSeconds(0.15f);
        DimLights();
        SFXManager.Instance.PlaySFX("LightsBreak");
        SFXManager.Instance.PlayMusic("Setter");
        yield return new WaitForSeconds(12f);
        SFXManager.Instance.PlayMusic("Unsettle");
        yield return new WaitForSeconds(4f);
        SFXManager.Instance.PlaySFX("WoodCreaking");
        yield return new WaitForSeconds(10f);
        SequenceManager.Instance.SetActiveFilter(3);
        SFXManager.Instance.StopMusic();
        yield return new WaitForSeconds(1.5f);
        SFXManager.Instance.PlaySFX("DoorOpen");
        yield return new WaitForSeconds(5f);

        EnableInteractableUI();
        NormalLights();
        SequenceManager.Instance.SetActiveFilter(4);
        //Play Room 2 music

    }

    void DimLights()
    {
        
        // lights[0].intensity = 0.1f;
        // lights[1].intensity = 0.01f;
        // lights[2].intensity = 0.01f;
    }

    void NormalLights()
    {
        lights[0].intensity = 0.5f;
        lights[1].intensity = 1f;
        lights[2].intensity = 1f;
    }
}
