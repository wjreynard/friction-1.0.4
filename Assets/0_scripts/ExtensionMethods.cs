using UnityEngine;

public static class ExtensionMethods
{

    public static float LinearRemap(this float value,
                                     float valueRangeMin, float valueRangeMax,
                                     float newRangeMin, float newRangeMax)
    {
        return (value - valueRangeMin) / (valueRangeMax - valueRangeMin) * (newRangeMax - newRangeMin) + newRangeMin;
    }

}