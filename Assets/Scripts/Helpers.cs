using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public static class Helpers {

    public static float RoundToNearest(this float value, float unit, float offset = 0f) {
        return (Mathf.Round((value - offset) / unit) * unit) + offset;
    }

    public static float Mod(this float a, float mod)
    {
        return a - mod * Mathf.Floor(a / mod);
    }

    public static int Mod(this int a, int mod)
    {
        return a - mod * Mathf.FloorToInt((float)a / (float)mod);
    }

    public static Coroutine Invoke(this MonoBehaviour mb, Action f, float delay, bool useUnscaled = false)
    {
        return mb.StartCoroutine(DoInvoke(f, delay, useUnscaled));
    }

    private static IEnumerator DoInvoke(Action f, float delay, bool useUnscaled)
    {
        if (useUnscaled) {
            yield return new WaitForSecondsRealtime(delay);
        }
        else {
            yield return new WaitForSeconds(delay);
        }
        
        f();
    }

    public static Vector2 XY(this Vector3 v) {
		return new Vector2(v.x, v.y);
	}

    public static Vector2 XZ(this Vector3 v) {
        return new Vector2(v.x, v.z);
    }

    public static Vector3 XYZ(this Vector2 v, float z = 0f) {
        return new Vector3(v.x, v.y, z);
    }

	public static Vector3 WithX(this Vector3 v, float x) {
		return new Vector3(x, v.y, v.z);
	}

	public static Vector3 WithY(this Vector3 v, float y) {
		return new Vector3(v.x, y, v.z);
	}

	public static Vector3 WithZ(this Vector3 v, float z) {
		return new Vector3(v.x, v.y, z);
	}

	public static Vector2 WithX(this Vector2 v, float x) {
		return new Vector2(x, v.y);
	}
	
	public static Vector2 WithY(this Vector2 v, float y) {
		return new Vector2(v.x, y);
	}
	
	public static Vector3 WithZ(this Vector2 v, float z) {
		return new Vector3(v.x, v.y, z);
    }

    public static Vector3 Round(this Vector3 vector) => new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
    public static Vector2 Round(this Vector2 vector) => new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));

    public static Vector3 RoundToNearest(this Vector3 vector, Vector3 unit) => new Vector3(Helpers.RoundToNearest(vector.x, unit.x), Helpers.RoundToNearest(vector.y, unit.y), Helpers.RoundToNearest(vector.z, unit.z));
    public static Vector3 RoundToNearest(this Vector3 vector, Vector3 unit, Vector3 offset) => new Vector3(Helpers.RoundToNearest(vector.x, unit.x, offset.x), Helpers.RoundToNearest(vector.y, unit.y, offset.y), Helpers.RoundToNearest(vector.z, unit.z, offset.z));
    public static Vector2 RoundToNearest(this Vector2 vector, Vector2 unit) => new Vector2(Helpers.RoundToNearest(vector.x, unit.x), Helpers.RoundToNearest(vector.y, unit.y));
    public static Vector2 RoundToNearest(this Vector2 vector, Vector2 unit, Vector3 offset) => new Vector2(Helpers.RoundToNearest(vector.x, unit.x, offset.x), Helpers.RoundToNearest(vector.y, unit.y, offset.y));

    public static Vector3 Abs(this Vector3 vector) => new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
	public static Vector2 Abs(this Vector2 vector) => new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));

	public static Vector3 Max(Vector3 vector1, Vector3 vector2) => new Vector3(Mathf.Max(vector1.x, vector2.x), Mathf.Max(vector1.y, vector2.y), Mathf.Max(vector1.z, vector2.z));
	public static Vector3 Min(Vector3 vector1, Vector3 vector2) => new Vector3(Mathf.Min(vector1.x, vector2.x), Mathf.Min(vector1.y, vector2.y), Mathf.Min(vector1.z, vector2.z));

	public static Rect RectFromCenter(Vector2 center, Vector2 size) => new Rect(center - size / 2, size);
	public static Vector2 ClampInRect(Vector2 vector, Rect rect) => new Vector2(Mathf.Clamp(vector.x, rect.xMin, rect.xMax), Mathf.Clamp(vector.y, rect.yMin, rect.yMax));

	public static Vector2 ViewportSize(this Camera camera) => camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - camera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));

    public static float GetH(this Color c) {
        Color.RGBToHSV(c, out float h, out _, out _);
        return h;
    }

    public static float GetS(this Color c) {
        Color.RGBToHSV(c, out _, out float s, out _);
        return s;
    }

    public static float GetV(this Color c) {
        Color.RGBToHSV(c, out _, out _, out float v);
        return v;
    }

    public static Color WithH(this Color c, float h) {
        float S, V;
        Color.RGBToHSV(c, out _, out S, out V);
        return Color.HSVToRGB(h, S, V);
    }

    public static Color WithS(this Color c, float s) {
        float H, V;
        Color.RGBToHSV(c, out H, out _, out V);
        return Color.HSVToRGB(H, s, V);
    }

    public static Color WithV(this Color c, float v) {
        float H, S;
        Color.RGBToHSV(c, out H, out S, out _);
        return Color.HSVToRGB(H, S, v);
    }

    public static Color WithA(this Color c, float a) {
        return new Color(c.r, c.g, c.b, a);
    }

    public static Color2 ToColor2(this Color c) {
        return new Color2(c, c);
    }

    public static void SetWidth(this LineRenderer line, float w) {
        line.startWidth = w;
        line.endWidth = w;
    }

    public static void SetColor(this LineRenderer line, Color c) {
        line.startColor = c;
        line.endColor = c;
    }

    public static void SetColor(this ParticleSystem particleSystem, Color color) {
        var colorModule = particleSystem.colorOverLifetime;
        colorModule.enabled = true;

        // set color gradient to match color
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(color.WithA(1), 0.0f),
                new GradientColorKey(color.WithA(1), 1.0f)
            }, new GradientAlphaKey[] {
                new GradientAlphaKey(color.a, 0.0f),
                new GradientAlphaKey(color.a, 1.0f)
            }
        );

        colorModule.color = grad;
    }

    public static float DBToVolume(float db) {
        return Mathf.Pow(10f, db / 20f);
    }

    public static float VolumeToDB(float volume) {
        return Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
    }


    public static bool LayerMaskContains(int mask, int layer) {
        return mask == (mask | (1 << layer));
    }
}