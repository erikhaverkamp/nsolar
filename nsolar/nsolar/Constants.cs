using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsolar
{
    public static class Constants
    {
        public const double earth_radius = 6378140.0;// # meters
        public const double earth_axis_inclination = 23.45;//  # degrees
        public const double seconds_per_day = 86400;

        public const double standard_pressure = 101325.00;//  # pascals
        public const double standard_temperature = 288.15;//  # kelvin
        public const double celsius_offset = 273.15;//  # subtract from kelvin to get deg C, add to deg C to get kelvin
        public const double earth_temperature_lapse_rate = -0.0065;//  # change in temperature with height, kelvin/metre
        public const double air_gas_constant = 8.31432;//  # N*m/s^2
        public const double earth_gravity = 9.80665;//  # m/s^2 or N/kg
        public const double earth_atmosphere_molar_mass = 0.0289644;//  # kg/mol
    }
}
