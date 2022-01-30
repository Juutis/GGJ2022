using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayChanger : MonoBehaviour
{
    public static DayChanger main;

    public DayCycleConfig config;

    public Light DayLight;
    public Light NightLight;

    private float started;
    private float timeOfDay;

    void Awake() {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        started = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(-transform.forward, 360f / config.DayLength * Time.deltaTime);

        timeOfDay = (Time.time - started) / config.DayLength % 1.0f;

        if (IsNight()) {
            night();
        } else {
            day();
        }

        handleColors();
    }

    public bool IsNight() {
        return timeOfDay > 0.5f;
    }

    public float GetTimeOfDay() {
        return timeOfDay;
    }

    private void night() {
        DayLight.enabled = false;
        NightLight.enabled = true;
        RenderSettings.ambientLight = Color.red;
        RenderSettings.skybox.SetColor("_Tint", Color.red);
        MusicPlayer.main.SwitchMusic(false);
    }

    private void day() {
        DayLight.enabled = true;
        NightLight.enabled = false;
        MusicPlayer.main.SwitchMusic(true);
    }

    private void handleColors() {
        Color startAmbientColor, endAmbientColor, startSkyColor, endSkyColor;
        float k;
        if (timeOfDay < 0.1) {
            startAmbientColor = config.DawnAmbientColor;
            endAmbientColor = config.DayAmbientColor;
            startSkyColor = config.DawnSkyColor;
            endSkyColor = config.DaySkyColor;
            k = 0.1f;
        } else if (timeOfDay < 0.4) {
            startAmbientColor = config.DayAmbientColor;
            endAmbientColor = config.DayAmbientColor;
            startSkyColor = config.DaySkyColor;
            endSkyColor = config.DaySkyColor;
            k = 1.0f;
        } else if (timeOfDay < 0.5) {
            startAmbientColor = config.DayAmbientColor;
            endAmbientColor = config.DuskAmbientColor;
            startSkyColor = config.DaySkyColor;
            endSkyColor = config.DuskSkyColor;
            k = 0.1f;
        } else if (timeOfDay < 0.52) {
            startAmbientColor = config.DuskAmbientColor;
            endAmbientColor = config.NightAmbientColor;
            startSkyColor = config.DuskSkyColor;
            endSkyColor = config.NightSkyColor;
            k = 0.02f;
        } else if (timeOfDay < 0.98) {
            startAmbientColor = config.NightAmbientColor;
            endAmbientColor = config.NightAmbientColor;
            startSkyColor = config.NightSkyColor;
            endSkyColor = config.NightSkyColor;
            k = 1.0f;
        } else {
            startAmbientColor = config.NightAmbientColor;
            endAmbientColor = config.DawnAmbientColor;
            startSkyColor = config.NightSkyColor;
            endSkyColor = config.DawnSkyColor;
            k = 0.02f;
        }

        float t = timeOfDay % k * (1/k);
        Color ambientColor = Color.Lerp(startAmbientColor, endAmbientColor, t);
        Color skyColor = Color.Lerp(startSkyColor, endSkyColor, t);

        RenderSettings.ambientLight = ambientColor;
        RenderSettings.skybox.SetColor("_Tint", skyColor);
    }
}
