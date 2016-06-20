using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsolar
{
    public static class Time
    {

        const double julian_day_offset = 1721425 - 0.5; // add to datetime.datetime.toordinal() to get Julian day number
        const int gregorian_day_offset = 719163; // number of days to add to datetime.datetime.timestamp() / seconds_per_day to agree with datetime.datetime.toordinal()
        const double tt_offset = 32.184; // seconds to add to TAI to get TT


        //# Table of leap-seconds (to date) taken from Wikipedia
        //# <https://en.wikipedia.org/wiki/Leap_second>.
        const int leap_seconds_base_year = 1972;

        internal static double get_julian_ephemeris_century(double julian_ephemeris_day)
        {
            return (julian_ephemeris_day - 2451545.0) / 36525.0;
        }

        internal static double get_julian_ephemeris_millennium(double julian_ephemeris_century)
        {
            return (julian_ephemeris_century/10.0);
        }

        internal static double get_julian_century(double julian_day)
        {
            return (julian_day - 2451545.0) / 36525.0;
        }


        /// <summary>
        /// "returns the UT Julian day number (including fraction of a day) corresponding to" \
        /// "the specified date/time. This version assumes the proleptic Gregorian calender;" \
        /// "trying to adjust for pre-Gregorian dates/times seems pointless when the changeover" \
        /// "happened over such wildly varying times in different regions."
        /// </summary>
        /// <param name="when"></param>
        /// <returns></returns>
        public static double get_julian_solar_day(DateTime when)
        {
            when = when.ToUniversalTime();
            var timestamp = (Int32)(when.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var result  = (timestamp + get_leap_seconds(when) + tt_offset - get_delta_t(when)) / Constants.seconds_per_day;
            result += gregorian_day_offset;
            result += julian_day_offset;
            return result;
        }

        /// <summary>
        /// "returns the TT Julian day number (including fraction of a day) corresponding to" \
        /// " the specified date/time. This version assumes the proleptic Gregorian calender;" \
        /// " trying to adjust for pre-Gregorian dates/times seems pointless when the changeover" \
        /// " happened over such wildly varying times in different regions."
        /// </summary>
        /// <param name="when"></param>
        /// <returns></returns>
        public static double get_julian_ephemeris_day(DateTime when)
        {
            
            return 2455479.841159537;
        }

        public static int get_leap_seconds(DateTime when)
        {
           
            return 34;
        }

        public static double get_delta_t(DateTime when)
        {
            
            return 66.2441;
        }



    }
}
