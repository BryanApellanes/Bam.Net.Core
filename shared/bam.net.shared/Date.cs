using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net
{
    public class Date
    {
        public int Month { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }

        public static Date FromString(string value)
        {
            string[] segments = value.DelimitSplit("/");
            Args.ThrowIf<ArgumentException>(segments.Length != 3, "Unrecognized date value specified: {0}", value);
            return new Date
                {Month = int.Parse(segments[0]), Day = int.Parse(segments[1]), Year = int.Parse(segments[2])};
        }
        
        public static Date FromInstant(Instant instant)
        {
            return FromDateTime(instant.ToDate());
        }

        public static Date FromDateTime(DateTime dateTime)
        {
            return new Date { Month = dateTime.Month, Day = dateTime.Day, Year = dateTime.Year };
        }

        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day);
        }

        public override bool Equals(object obj)
        {
            if (obj is Date compareTo)
            {
                return compareTo.Month == Month && compareTo.Day == Day && compareTo.Year == Year;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Month, Day, Year);
        }

        public override string ToString()
        {
            return $"{Month}/{Day}/{Year}";
        }
    }
}
