using System;
using UnityEngine;

public static class SunPosition 
{
    private static float latitude = 37.9838f; // Athens latitude
    private static float longitude = 23.7275f; // Athens longitude

    public static Quaternion UpdateSunPosition(Light directionalLight)
    {
        // Get current local time in Athens (adjust for daylight saving time automatically)
        TimeZoneInfo athensTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GTB Standard Time"); // Athens Time Zone
        DateTime athensTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, athensTimeZone);

        // Calculate the fractional year in radians (used for approximation)
        float dayOfYear = athensTime.DayOfYear;
        float fractionalYear = (2f * Mathf.PI / 365f) * (dayOfYear - 1f + (athensTime.Hour - 12f) / 24f);

        // Declination angle (approximation in degrees)
        float declination = 23.45f * Mathf.Sin(fractionalYear);

        // Time correction for solar angle
        float equationOfTime = 7.5f * Mathf.Sin(2 * fractionalYear) - 1.9f * Mathf.Sin(fractionalYear);
        float solarTime = athensTime.Hour * 60f + athensTime.Minute + equationOfTime + 4f * longitude;

        // Hour angle (degrees)
        float hourAngle = (solarTime / 4f) - 180f;

        // Elevation angle (degrees)
        float elevation = Mathf.Asin(
            Mathf.Sin(latitude * Mathf.Deg2Rad) * Mathf.Sin(declination * Mathf.Deg2Rad) +
            Mathf.Cos(latitude * Mathf.Deg2Rad) * Mathf.Cos(declination * Mathf.Deg2Rad) * Mathf.Cos(hourAngle * Mathf.Deg2Rad)
        ) * Mathf.Rad2Deg;

        // Azimuth angle (degrees)
        float azimuth = Mathf.Atan2(
            -Mathf.Sin(hourAngle * Mathf.Deg2Rad),
            Mathf.Tan(declination * Mathf.Deg2Rad) * Mathf.Cos(latitude * Mathf.Deg2Rad) -
            Mathf.Sin(latitude * Mathf.Deg2Rad) * Mathf.Cos(hourAngle * Mathf.Deg2Rad)
        ) * Mathf.Rad2Deg;
        azimuth = (azimuth + 360f) % 360f; // Normalize to 0-360 degrees

        // Apply rotation to the directional light
        return directionalLight.transform.rotation = Quaternion.Euler(90f - elevation, azimuth, 0f);
    }
}
