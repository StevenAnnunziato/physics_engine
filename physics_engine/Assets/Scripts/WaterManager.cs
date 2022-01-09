using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterManager : MonoBehaviour
{

    private static WaterManager instance;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private float amplitude;
    [SerializeField] private float wavelength;
    [SerializeField] private float waveSpeed;

    public Slider ampSlider;
    public Slider wavelengthSlider;
    public Slider speedSlider;

    public void SetAmplitude(float v) { amplitude = v; }
    public void SetWavelength(float v) { wavelength = v; }
    public void SetWaveSpeed(float v) { waveSpeed = v; }

    public static WaterManager GetInstance() { return instance; }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        meshFilter.mesh.MarkDynamic();
    }

    // Update is called once per frame
    void Update()
    {
        // update settings from the sliders
        amplitude = ampSlider.value;
        wavelength = wavelengthSlider.value;
        waveSpeed = speedSlider.value;

        // make vertices wave
        Vector3[] verts = meshFilter.mesh.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i].z = getWaterLevel(verts[i]); // changing z to account for blender's z-axis pointing up
        }

        // reassign vertices to mesh
        meshFilter.mesh.vertices = verts;
        meshFilter.mesh.RecalculateNormals();
    }

    public float getWaterLevel(Vector3 pos)
    {
        return amplitude * Mathf.Sin(pos.x * wavelength + Time.time * waveSpeed);
    }
}
