using System.Collections.Generic;
using System.Linq;
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
            sb.Append("\r\nPRODID:-//notion-2-ical//EN");
            sb.Append("\r\nCALSCALE:GREGORIAN");
            sb.Append($"\r\nNAME:{VEvent.EscapeText(_name)}");
            sb.Append($"\r\nX-WR-CALNAME:{VEvent.EscapeText(_name)}");
            sb.Append("\r\nMETHOD:PUBLISH");

            foreach (var vEvent in _events.OrderBy(ev => ev.Start))
            {
                sb.Append(vEvent.ToString(_name));
            }

            sb.Append("\r\nEND:VCALENDAR");

            return sb.ToString();
        }
    }
}
