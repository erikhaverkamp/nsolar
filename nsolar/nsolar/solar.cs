using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsolar
{
    public static class Solar
    {
        public static double get_altitude(double latitude_deg, double longitude_deg, DateTime when,
                        double elevation = 0 , double temperature = Constants.standard_temperature,
                        double pressure = Constants.standard_pressure)
        {
            // location-dependent calculations
            var projected_radial_distance = get_projected_radial_distance(elevation, latitude_deg);
            var projected_axial_distance = get_projected_axial_distance(elevation, latitude_deg);

            // time-dependent calculations
            double jd = Time.get_julian_solar_day(when);
            double jde = Time.get_julian_ephemeris_day(when);
            double jce = Time.get_julian_ephemeris_century(jde);
            double jme = Time.get_julian_ephemeris_millennium(jce);
            double geocentric_latitude = get_geocentric_latitude(jme);
            double sun_earth_distance = get_sun_earth_distance(jme);
            double geocentric_longitude = get_geocentric_longitude(jme);
            double aberration_correction = get_aberration_correction(sun_earth_distance);
            double equatorial_horizontal_parallax = get_equatorial_horizontal_parallax(sun_earth_distance);
            var nutation = get_nutation(jce);
            return 0;

        }

        private static Nutation get_nutation(double jce)
        {
            var abcd = Constants.nutation_coefficients;
            var nutation_long = new List<double>();
            var nutation_oblique = new List<double>();
            var p = Constants.get_aberration_coeffs();
    //x = list \
    //  (
    //    p[k](jce)
    //    for k in
    //        ( # order is important
    //            'MeanElongationOfMoon',
    //            'MeanAnomalyOfSun',
    //            'MeanAnomalyOfMoon',
    //            'ArgumentOfLatitudeOfMoon',
    //            'LongitudeOfAscendingNode',
    //        )
    //  )
            var y = Constants.aberration_sin_terms;
    //for i in range(len(abcd)):
    //    sigmaxy = 0.0
    //    for j in range(len(x)):
    //        sigmaxy += x[j] * y[i][j]
    //    #end for
    //    nutation_long.append((abcd[i][0] + (abcd[i][1] * jce)) * math.sin(math.radians(sigmaxy)))
    //    nutation_oblique.append((abcd[i][2] + (abcd[i][3] * jce)) * math.cos(math.radians(sigmaxy)))

            // 36000000 scales from 0.0001 arcseconds to degrees

            var nutation = new Nutation();
            nutation.longitude = nutation_long.Sum()/36000000.0;
            nutation.obliquity = nutation_oblique.Sum()/36000000.0;
            return nutation;
        }


        private static double get_equatorial_horizontal_parallax(double sunEarthDistance)
        {
            return 8.794 / (3600 / sunEarthDistance);
        }

        private static double get_aberration_correction(double sunEarthDistance)
        {
            return -20.4898 / (3600.0 * sunEarthDistance);
        }

        private static double get_sun_earth_distance(double jme)
        {
            return get_coeff(jme, Constants.sun_earth_distance_coeffs) / 1E8;
        }

        private static double get_geocentric_longitude(double jme)
        {
            return (get_heliocentric_longitude(jme) + 180)%360;
        }

        private static double get_heliocentric_longitude(double jme)
        {
            return Utils.ConvertToDegrees(get_coeff(jme, Constants.heliocentric_longitude_coeffs)/1e8)%360;
        }

        private static double get_geocentric_latitude(double jme)
        {
            return -1.0*get_heliocentric_latitude(jme);
        }

        private static double get_heliocentric_latitude(double jme)
        {
            var intermediate = get_coeff(jme, Constants.heliocentric_latitude_coeffs)/1e8;
            // 6.31390387746413e-07
            
            return Utils.ConvertToDegrees(intermediate);

        }

        private static double get_coeff(double jme, string coeffs)
        {
            //    computes a polynomial with time-varying coefficients from the given constant" \
            //    coefficients array and the current Julian millennium."
            var result = 0.0;
            var x = 1.0;
            coeffs = coeffs.Replace("\n", "");
            coeffs = coeffs.Replace("\r", "");
            coeffs = coeffs.Replace(" ", "");
            //coeffs = coeffs.Replace("[", "");
            //coeffs = coeffs.Replace("]", "");
            var cols = coeffs.Split(';');
            

            foreach (var line in cols)
            {
                var s =  line.Substring(1, line.Length - 2);
                s = s.Replace("],[", ";");
                s = s.Substring(1, s.Length - 2);
                //Debug.WriteLine(s);
                var c = 0.0;
                var pols = s.Split(';');
                foreach (var l in pols)
                {
                    var ll = l.Split(',');

                    var a = Convert.ToDouble(ll[0]);
                    var b = Convert.ToDouble(ll[1]);
                    var d = Convert.ToDouble(ll[2]);


                    c += a * System.Math.Cos(b + (d * jme));
                    //Debug.WriteLine("{0} {1} {2}", a,b,d);
                }

                result += c * x;
                Debug.WriteLine(c);
                x *= jme;
            }

            return result;

        }


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
