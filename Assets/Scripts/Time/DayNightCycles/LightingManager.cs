using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Lighting : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    // Start is called before the first frame update
    private void OnValidate(){
        if(DirectionalLight!=null)
        return;

        if(RenderSettings.sun!=null){
            DirectionalLight=RenderSettings.sun;
        }else{
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights){
                if(light.type == LightType.Directional){
                    DirectionalLight=light;
                    return;
                }
            }
        }
    }

    private void UpdateLighting(float timePercent){
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if(DirectionalLight!=null){
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent); 
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent*360f)-90f, 170f, 0));

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Preset==null){
            return; 
        }
        if(Application.isPlaying){
            TimeOfDay= TimeOfDay+Time.deltaTime;
            TimeOfDay%=24;
        }else{
            UpdateLighting(TimeOfDay/24f);
        }
    }
}
