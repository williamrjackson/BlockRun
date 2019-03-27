using UnityEngine;

public class ColorTransition : MonoBehaviour
{
    [SerializeField]
    [Range(.0001f, .1f)]
    private float cycleSpeed = .006f;
    [SerializeField]
    LaserLine[] lasers = null;

    void Update()
    {
        foreach(LaserLine line in lasers)
        {
            float h;
            float s;
            float v;
            float hnull;
            Color.RGBToHSV(line.GetColor(), out h, out s, out v);
            h = (h + cycleSpeed < 1) ? h + cycleSpeed : h - 1 + cycleSpeed;
            line.SetColor(Color.HSVToRGB(h, s, v));
            Color.RGBToHSV(RenderSettings.fogColor, out hnull, out s, out v);
            RenderSettings.fogColor = Color.HSVToRGB(h, s, v);
        }
    }
}
