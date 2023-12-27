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


        private DateTime begin_liturgical_season = DateTime.MaxValue;

        public PsalterDay() : this(DateTime.Today){}
        public PsalterDay(DateTime date)
        {
            this.date = date;
        }

        public LiturgicalSeason calculateLiturgicalSeason()
        {
            DateTime christmas_eve = new DateTime(date.Year, 12, 24);
            Func<DateTime, DateTime> lastSunday = (date) => date.AddDays(-(int)date.DayOfWeek);
            //The first one will be the sunday, three weeks ago (21 days before) from the 4th advent sunday
            DateTime first_advent_sunday = lastSunday(christmas_eve).AddDays(-21);
            if (date >= first_advent_sunday && date <= christmas_eve)
            {
                begin_liturgical_season = first_advent_sunday;
                return LiturgicalSeason.Advent;
            }

            if (date > christmas_eve)
            {
                begin_liturgical_season = christmas_eve;
                return LiturgicalSeason.Christmas;
            }

            /*
             * epiphany sunday is celebrated between 2nd and 8th of january, being the day, in which it is sunday.
             * The baptism of the lord is celebrated on monday if the epiphany sunday is celebrated the 7th or 8th
             * otherwise it is celebrated on the next sunday of the epiphany sunday
             */
            Func<DateTime, DateTime> nextSunday = (date) => date.AddDays(7 - (int)date.DayOfWeek);
            DateTime epiphany_sunday = lastSunday(new DateTime(date.Year, 1, 8));
            DateTime baptism_of_the_lord = epiphany_sunday.Day > 6 ? epiphany_sunday.AddDays(1) : nextSunday(epiphany_sunday);

            if (date <= baptism_of_the_lord) //baptism of the lord has the Sunday I psalms
            {
                //The whole christmas octave we celebrate the first sunday, so after it we are still on the first week.
                //Weird case: what about if we end the octave on saturday? We continue with the second week? Probably
                
                begin_liturgical_season = new DateTime(date.Year - 1, 12, 31); //Will be for sure the first week of the psaltery "Sunday like"
                return LiturgicalSeason.Christmas;
            }

            DateTime easter = computus_easter_sunday(date.Year);
            const int DAYS_BETWEEN_EASTER_AND_ASH = 46;
            DateTime ash_wednesday = easter.AddDays(-DAYS_BETWEEN_EASTER_AND_ASH);

            if (date >= ash_wednesday && date < easter)
            {
                begin_liturgical_season = ash_wednesday; //Consider that for this case you will need A LOT of special considerations
                return LiturgicalSeason.Lent;
            }
                

            DateTime pentecost = easter.AddDays(49);
            if (date >= easter && date <= pentecost)
            {
                begin_liturgical_season = easter;
                return LiturgicalSeason.Easter;
            }

            if (date < ash_wednesday)
                begin_liturgical_season = baptism_of_the_lord.AddDays(1);//The next sunday is already the Second week of the Ordinary time
            if(date > pentecost)
            {
                //Maybe there is another way to have the week set at two and use the difference to calculate for the days before. Consider this
                begin_liturgical_season = pentecost.AddDays(1);//This will be a Monday, take care
                //Calculate the week, to do so calculate the week in which we would be on the tuesday before ash wednesday
                DateTime week2_ordinary = nextSunday(baptism_of_the_lord);
                DateTime last_ordinary_week_before_lent = lastSunday(ash_wednesday);
                int week_last_ordinary_week_before_lent = ((last_ordinary_week_before_lent - week2_ordinary).Days / 7) + 2
                weekNr = week_last_ordinary_week_before_lent + 1;
            }

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
