using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Newtonsoft.Json;

namespace RoleplayToolSet
{
    [JsonObject(MemberSerialization.Fields)]
    public class WorldTime
    {
        [JsonProperty("time")]
        public LocalDateTime Time { get; private set; }
        
        [field: JsonIgnore]
        public event EventHandler TimeChanged; // Triggered if the time is changed

        public WorldTime()
        {
            Time = new LocalDateTime(1, 1, 1, 12, 0);
        }

        [JsonConstructor]
        public WorldTime(LocalDateTime time)
        {
            Time = time;
        }

        /// <summary>
        /// Sets the time held in this world time
        /// </summary>
        /// <param name="time">The time to set the time to</param>
        public void SetTime(LocalDateTime time)
        {
            Time = time;
            TimeChanged?.Invoke(this, new EventArgs());
        }

        public void Add(Period period)
        {
            try
            {
                Time = Time.Plus(period);
            }
            catch (IndexOutOfRangeException)
            {
                string message = "Time out of range";
                if (Time.Calendar != CalendarSystem.Iso)
                {
                    message += ". Try changing calendar to ISO to fix";
                }
                MessageBox.Show(message, "Time error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            TimeChanged?.Invoke(this, new EventArgs());
        }
        
        public void AddTicks(long value)
        {
            Add(Period.FromTicks(value));
        }

        public void AddMilliseconds(long value)
        {
            Add(Period.FromMilliseconds(value));
        }

        public void AddSeconds(long value)
        {
            Add(Period.FromSeconds(value));
        }

        public void AddMinutes(long value)
        {
            Add(Period.FromMinutes(value));
        }

        public void AddHours(long value)
        {
            Add(Period.FromHours(value));
        }

        public void AddDays(int value)
        {
            Add(Period.FromDays(value));
        }

        public void AddMonths(int value)
        {
            Add(Period.FromMonths(value));
        }

        public void AddYears(int value)
        {
            Add(Period.FromYears(value));
        }

        public int CompareTo(WorldTime t1)
        {
            return Time.CompareTo(t1.Time);
        }

        public static bool Equals(WorldTime t1, WorldTime t2)
        {
            return t1.CompareTo(t2) == 0;
        }

        public WorldTime Clone()
        {
            return new WorldTime(new LocalDateTime(Time.Year, Time.Month, Time.Day, Time.Hour, Time.Minute, Time.Second, Time.Millisecond));
        }

        /// <summary>
        /// Gets the time through a dat as a percentage
        /// </summary>
        /// <returns>A double from 0 to 1</returns>
        public double GetPercentThroughDay()
        {
            return (double)Time.TimeOfDay.NanosecondOfDay / LocalTime.MaxValue.NanosecondOfDay;
        }

        public void ChangeCalendar(CalendarSystem calendar)
        {
            if (calendar != Time.Calendar)
            {
                Time = new LocalDateTime(Time.Year, Time.Month, Time.Day, Time.Hour, Time.Minute, Time.Second, Time.Millisecond, calendar);
            }
        }

        public override string ToString()
        {
            return ToString("dd/MMM/yyyyg h:mm tt");
        }

        public string ToString(string format)
        {
            return Time.ToString(format, null);
        }
    }
}
