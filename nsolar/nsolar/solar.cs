using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsolar
{
    public static class Solar
    {
        public static void get_altitude(double latitude_deg, double longitude_deg, DateTime when,
                        double elevation = 0 , double temperature = Constants.standard_temperature,
                        double pressure = Constants.standard_pressure)
        {
            // location-dependent calculations
            var projected_radial_distance = get_projected_radial_distance(elevation, latitude_deg);
            var projected_axial_distance = get_projected_axial_distance(elevation, latitude_deg);

            // time-dependent calculations
            double jd = Time.get_julian_solar_day(when);
            double jde = Time.get_julian_ephemeris_day(when);


        }
            /*

    # time-dependent calculations
    jd = time.get_julian_solar_day(when)
    jde = time.get_julian_ephemeris_day(when)
    jce = time.get_julian_ephemeris_century(jde)
    jme = time.get_julian_ephemeris_millennium(jce)
    geocentric_latitude = get_geocentric_latitude(jme)
    geocentric_longitude = get_geocentric_longitude(jme)
    sun_earth_distance = get_sun_earth_distance(jme)
    aberration_correction = get_aberration_correction(sun_earth_distance)
    equatorial_horizontal_parallax = get_equatorial_horizontal_parallax(sun_earth_distance)
    nutation = get_nutation(jce)
    apparent_sidereal_time = get_apparent_sidereal_time(jd, jme, nutation)
    true_ecliptic_obliquity = get_true_ecliptic_obliquity(jme, nutation)

    # calculations dependent on location and time
    apparent_sun_longitude = get_apparent_sun_longitude(geocentric_longitude, nutation, aberration_correction)
    geocentric_sun_right_ascension = get_geocentric_sun_right_ascension(apparent_sun_longitude, true_ecliptic_obliquity, geocentric_latitude)
    geocentric_sun_declination = get_geocentric_sun_declination(apparent_sun_longitude, true_ecliptic_obliquity, geocentric_latitude)
    local_hour_angle = get_local_hour_angle(apparent_sidereal_time, longitude_deg, geocentric_sun_right_ascension)
    parallax_sun_right_ascension = get_parallax_sun_right_ascension(projected_radial_distance, equatorial_horizontal_parallax, local_hour_angle, geocentric_sun_declination)
    topocentric_local_hour_angle = get_topocentric_local_hour_angle(local_hour_angle, parallax_sun_right_ascension)
    topocentric_sun_declination = get_topocentric_sun_declination(geocentric_sun_declination, projected_axial_distance, equatorial_horizontal_parallax, parallax_sun_right_ascension, local_hour_angle)
    topocentric_elevation_angle = get_topocentric_elevation_angle(latitude_deg, topocentric_sun_declination, topocentric_local_hour_angle)
    refraction_correction = get_refraction_correction(pressure, temperature, topocentric_elevation_angle)
    return topocentric_elevation_angle + refraction_correction
        */

        public static double get_projected_radial_distance(double elevation , double latitude)
        {
            var flattened_latitude_rad = Utils.ConvertToRadians(get_flattened_latitude(latitude));
            var latitude_rad = Utils.ConvertToRadians(latitude);
            return System.Math.Cos(flattened_latitude_rad) + (elevation*Math.Cos(latitude_rad)/Constants.earth_radius);
            
        }

        public static double get_projected_axial_distance(double elevation , double latitude)
        {
            var flattened_latitude_rad = Utils.ConvertToRadians(get_flattened_latitude(latitude));
            var latitude_rad = Utils.ConvertToRadians(latitude);
            return 0.99664719*Math.Sin(flattened_latitude_rad) +
                   (elevation*Math.Sin(latitude_rad)/Constants.earth_radius);

        }

        public static double get_flattened_latitude(double latitude)
        {
            var latitude_rad = Utils.ConvertToRadians(latitude);
            return Utils.ConvertToDegrees(Math.Atan(0.99664719*Math.Tan(latitude_rad)));
        }


    }
}
