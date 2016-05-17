using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsolar
{
    public static class Time
    {
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
            return \
        (
                (timestamp(when) + get_leap_seconds(when) + tt_offset - get_delta_t(when))
            /
                seconds_per_day
        +
            gregorian_day_offset
        +
            julian_day_offset
        )

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
            return \
        (
                (timestamp(when) + get_leap_seconds(when) + tt_offset)
            /
                seconds_per_day
        +
            gregorian_day_offset
        +
            julian_day_offset
        )
        }

 

    }
}
