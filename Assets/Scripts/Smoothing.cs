using System;
using System.Collections.Generic;
using UnityEngine;

public static class Smoothing
{
    private static float[] smoothingKernel;

    private static float[] smoothingKernelMid;

    private static float[] smoothingKernelBig;

    private static float[] decayKernel;

    static Smoothing()
    {
        Smoothing.smoothingKernel = new float[]
        {
            0.4f,
            0.3f,
            0.2f,
            0.1f
        };
        Smoothing.smoothingKernelMid = new float[10];
        for (int i = 0; i < 10; i++)
        {
            Smoothing.smoothingKernelMid[i] = (float)(10 - i);
        }
        Smoothing.smoothingKernelBig = new float[30];
        for (int j = 0; j < 30; j++)
        {
            Smoothing.smoothingKernelBig[j] = (float)((30 - j) * (30 - j) * (j + 1));
        }
        Smoothing.decayKernel = new float[1000];
        for (int k = 0; k < 1000; k++)
        {
            Smoothing.decayKernel[k] = 1f + (float)k * 0.1f;
        }
    }

    public static float FillValue(List<float> array, float value)
    {
        return Smoothing.FillValue(array, value, Smoothing.smoothingKernel);
    }

    public static float FillValueMid(List<float> array, float value)
    {
        return Smoothing.FillValue(array, value, Smoothing.smoothingKernelMid);
    }

    public static float FillValueBig(List<float> array, float value)
    {
        return Smoothing.FillValue(array, value, Smoothing.smoothingKernelBig);
    }

    public static float FillValue(List<float> array, float value, float[] smoothingKernel)
    {
        array.Clear();
        for (int i = 0; i < smoothingKernel.Length; i++)
        {
            array.Add(value);
        }
        return value;
    }

    public static float FillValue(List<float> array, float value, int count)
    {
        array.Clear();
        for (int i = 0; i < count; i++)
        {
            array.Add(value);
        }
        return value;
    }

    public static float SmoothValue(List<float> array, float value)
    {
        return Smoothing.SmoothValue(array, value, Smoothing.smoothingKernel);
    }

    public static float SmoothValueMid(List<float> array, float value)
    {
        return Smoothing.SmoothValue(array, value, Smoothing.smoothingKernelMid);
    }

    public static float SmoothValueBig(List<float> array, float value)
    {
        return Smoothing.SmoothValue(array, value, Smoothing.smoothingKernelBig);
    }

    public static float SmoothValue(List<float> array, float value, float[] smoothingKernel)
    {
        array.Insert(0, value);
        if (array.Count > smoothingKernel.Length)
        {
            array.RemoveAt(array.Count - 1);
        }
        float num = 0f;
        float num2 = 0f;
        for (int i = 0; i < array.Count; i++)
        {
            float num3 = smoothingKernel[i];
            num += num3 * array[i];
            num2 += num3;
        }
        return num / num2;
    }

    public static Vector3 SmoothValue(List<Vector3> array, Vector3 value)
    {
        return Smoothing.SmoothValue(array, value, Smoothing.smoothingKernel);
    }

    public static Vector3 SmoothValueMid(List<Vector3> array, Vector3 value)
    {
        return Smoothing.SmoothValue(array, value, Smoothing.smoothingKernelMid);
    }

    public static Vector3 SmoothValueBig(List<Vector3> array, Vector3 value)
    {
        return Smoothing.SmoothValue(array, value, Smoothing.smoothingKernelBig);
    }

    public static Vector3 SmoothValue(List<Vector3> array, Vector3 value, float[] smoothingKernel)
    {
        array.Insert(0, value);
        if (array.Count > smoothingKernel.Length)
        {
            array.RemoveAt(array.Count - 1);
        }
        Vector3 a = Vector3.zero;
        float num = 0f;
        for (int i = 0; i < array.Count; i++)
        {
            float num2 = smoothingKernel[i];
            a += num2 * array[i];
            num += num2;
        }
        return a / num;
    }

    public static void PushValue(List<float> array, float value, int maxCount)
    {
        while (array.Count >= maxCount)
        {
            array.RemoveAt(0);
        }
        array.Add(value);
    }

    public static float Average(List<float> array, int delay, int take)
    {
        if (array.Count == 0)
        {
            return 0f;
        }
        if (delay >= array.Count)
        {
            return array[0];
        }
        float num = 0f;
        int num2 = array.Count - delay;
        int num3 = num2 - take;
        if (num3 < 0)
        {
            num3 = 0;
        }
        for (int i = num3; i < num2; i++)
        {
            num += array[i];
        }
        return num / (float)(num2 - num3);
    }

    public static void Unpush(List<float> array, int samples)
    {
        while (array.Count > 0 && samples > 0)
        {
            array.RemoveAt(array.Count - 1);
            samples--;
        }
    }
}
