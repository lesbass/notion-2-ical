using System.Collections.Generic;
using System.Text;

namespace Notion.Models
{
    public class VCalendar
    {
        private readonly List<VEvent> _events = new List<VEvent>();
        private readonly string _name;

        public VCalendar(string name)
        {
            _name = name;
        }

        public void AddEvent(VEvent vEvent)
        {
            _events.Add(vEvent);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("BEGIN:VCALENDAR");
            sb.Append("\r\nVERSION:2.0");
            sb.Append($"\r\nNAME:{_name}");
            sb.Append($"\r\nX-WR-CALNAM:{_name}");
            sb.Append("\r\nMETHOD:PUBLISH");

            _events.ForEach(ev => sb.Append(ev.ToString(_name.GetHashCode().ToString())));

            sb.Append("\r\nEND:VCALENDAR");

            return sb.ToString();
        }
    }
}