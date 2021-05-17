using System;
using System.Text;

namespace Notion.Models
{
    public class VEvent
    {
        private readonly string _description;
        private readonly string _end;
        private readonly string _itemUrl;
        private readonly string _start;
        private readonly string _title;

        public VEvent(string start, string end, string title, string description, string itemUrl)
        {
            _start = start;
            _end = end;
            _title = title;
            _description = description;
            _itemUrl = itemUrl;
        }

        public string ToString(string calendarId)
        {
            var sb = new StringBuilder();

            sb.Append("\r\nBEGIN:VEVENT");
            sb.Append($"\r\n{_start}");
            sb.Append($"\r\n{_end}");
            sb.Append($"\r\nUID:{GetHashCode()}@{calendarId}");
            sb.Append($"\r\nURL:{_itemUrl}");
            sb.Append($"\r\nDTSTAMP:{DateTime.Now.ToUniversalTime():yyyyMMddTHHmmssZ}");
            sb.Append($"\r\nSUMMARY:{_title}");
            sb.Append($"\r\nDESCRIPTION:{_description}");
            sb.Append("\r\nPRIORITY:5");
            sb.Append("\r\nCLASS:PUBLIC");
            sb.Append("\r\nEND:VEVENT");

            return sb.ToString();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _start != null ? _start.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (_end != null ? _end.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_itemUrl != null ? _itemUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_title != null ? _title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_description != null ? _description.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}