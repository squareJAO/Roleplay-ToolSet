using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NodaTime;

namespace RoleplayToolSet
{
    public class WorldTime
    {
        private LocalDateTime _time;

        // Triggered if the time is changed
        public event EventHandler TimeChanged;

        public WorldTime()
        {
            _time = new LocalDateTime(1, 1, 1, 12, 0);
        }

        private WorldTime(LocalDateTime time)
        {
            _time = new LocalDateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, time.Millisecond);
        }

        public void Add(Period period)
        {
            try
            {
                _time = _time.Plus(period);
                TimeChanged?.Invoke(this, new EventArgs());
            }
            catch (IndexOutOfRangeException)
            {
                string message = "Time out of range";
                if (_time.Calendar != CalendarSystem.Iso)
                {
                    message += ". Try changing calendar to ISO to fix";
                }
                MessageBox.Show(message, "Time error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            return _time.CompareTo(t1._time);
        }

        public static bool Equals(WorldTime t1, WorldTime t2)
        {
            return t1.CompareTo(t2) == 0;
        }

        /// <summary>
        /// Gets the time through a dat as a percentage
        /// </summary>
        /// <returns>A double from 0 to 1</returns>
        public double GetPercentThroughDay()
        {
            return (double)_time.TimeOfDay.NanosecondOfDay / LocalTime.MaxValue.NanosecondOfDay;
        }

        public void ChangeCalendar(CalendarSystem calendar)
        {
            if (calendar != _time.Calendar)
            {
                _time = new LocalDateTime(_time.Year, _time.Month, _time.Day, _time.Hour, _time.Minute, _time.Second, _time.Millisecond, calendar);
            }
        }

        public override string ToString()
        {
            return ToString("dd/MMM/yyyyg h:mm tt");
        }

        public string ToString(string format)
        {
            return _time.ToString(format, null);
        }
    }
}
