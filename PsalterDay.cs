using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhichPsalmWeek
{
    public enum LiturgicalSeason
    {
        Advent,
        Christmas,
        Ordinary,
        Lent,
        Easter,
        Unknown
    }
    public class PsalterDay
    {
        private DateTime date = DateTime.Today;
        private LiturgicalSeason season = LiturgicalSeason.Unknown;
        private int weekNr = 0;


        private DateTime first_advent_sunday, christmas_eve, baptism_of_the_lord, ash_wednesday, easter, pentecost = DateTime.MaxValue;
        private bool feasts_aready_set = false;

        public PsalterDay() : this(DateTime.Today){}
        public PsalterDay(DateTime date)
        {
            this.date = date;

        }

        public LiturgicalSeason calculateLiturgicalSeason()
        {
            christmas_eve = new DateTime(date.Year, 12, 24);
            Func<DateTime, DateTime> lastSunday = (date) => date.AddDays(-(int)date.DayOfWeek);
            //The first one will be the sunday, three weeks ago (21 days before) from the 4th advent sunday
            first_advent_sunday = lastSunday(christmas_eve).AddDays(-21);
            if (date >= first_advent_sunday && date <= christmas_eve)
                return LiturgicalSeason.Advent;
            /*
             * epiphany sunday is celebrated between 2nd and 8th of january, being the day, in which it is sunday.
             * The baptism of the lord is celebrated on monday if the epiphany sunday is celebrated the 7th or 8th
             * otherwise it is celebrated on the next sunday of the epiphany sunday
             */
            Func<DateTime, DateTime> nextSunday = (date) => date.AddDays(7 - (int)date.DayOfWeek);
            DateTime epiphany_sunday = lastSunday(new DateTime(date.Year, 1, 8));
            baptism_of_the_lord = epiphany_sunday.Day > 6 ? epiphany_sunday.AddDays(1) : nextSunday(epiphany_sunday);

            if ( date > christmas_eve || date <= baptism_of_the_lord)
                return LiturgicalSeason.Christmas;

            easter = computus_easter_sunday(date.Year);
            const int days_between_easter_and_ash = 46;
            ash_wednesday = easter.AddDays(-days_between_easter_and_ash);

            if (date >= ash_wednesday && date < easter)
                return LiturgicalSeason.Lent;

            pentecost = easter.AddDays(49);
            if (date >= easter && date <= pentecost)
                return LiturgicalSeason.Easter;

            return LiturgicalSeason.Ordinary;
        }

        public static DateTime computus_easter_sunday(int year)
        {   //According to https://www.algorithm-archive.org/contents/computus/computus.html
            int y_in_metonic_calendar = year % 19;
            int century = year / 100;
            int shift_in_metonic_cycle = (13 + (8 * century)) / 25;

            int amount_of_400s = century / 4;
            int non_observed_leap_days = century - amount_of_400s;
            int lunar_month_offset = (15 - shift_in_metonic_cycle + non_observed_leap_days) % 30;
            /*
             * Every 12 lunar months is roughly 354 days, which is 11 days shorter than 365. 
             * This means that every year in the Metonic cycle, the lunar phase will be 11 days behind. 
             * It just so happens that −11 % 30=19.
             */
            int days_from_spring_equinox_to_next_full_moon = (19 * y_in_metonic_calendar + lunar_month_offset) % 30;

            //Calculate next sunday
            int century_offset = (4 + non_observed_leap_days) % 7;
            int week_offset_leaps = 2 * (year % 4) + 4 * (year % 7);
            int offset_full_moon_to_easter_sunday = (week_offset_leaps + 6 * days_from_spring_equinox_to_next_full_moon + century_offset) % 7;

            if ((days_from_spring_equinox_to_next_full_moon == 29 && offset_full_moon_to_easter_sunday == 6) ||
                (days_from_spring_equinox_to_next_full_moon == 28 && offset_full_moon_to_easter_sunday == 6 && y_in_metonic_calendar > 10))
            {
                offset_full_moon_to_easter_sunday = -1;
            }

            int total_offset = days_from_spring_equinox_to_next_full_moon + offset_full_moon_to_easter_sunday + 1;

            DateTime spring_equinox = new DateTime(year, 3, 21);
            return spring_equinox.AddDays(total_offset);
        }
    }
}
