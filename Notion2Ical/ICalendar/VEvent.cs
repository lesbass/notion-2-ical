namespace Notion2Ical.ICalendar;

internal class VEvent
{
    private const string DateTimeFormat = "yyyyMMddTHHmmssZ";

    private readonly string _description;
    private readonly DateTime _end;
    private readonly bool _hasTime;
    private readonly string _itemUrl;
    private readonly DateTime _start;
    private readonly string _title;
    private readonly string _uid;

    public VEvent(string uid, DateTime start, DateTime end, bool hasTime, string title, string description, string itemUrl)
    {
        _uid = uid;
        _start = start;
        _end = end;
        _hasTime = hasTime;
        _title = title;
        _description = description;
        _itemUrl = itemUrl;
    }

    public DateTime Start => _start;

    public string ToString(string calendarId)
    {
        var sb = new StringBuilder();

        sb.Append("\r\nBEGIN:VEVENT");
        AppendDate(sb, "DTSTART", _start, _hasTime);
        AppendDate(sb, "DTEND", _end, _hasTime);
        sb.Append($"\r\nUID:{EscapeText(_uid)}@{EscapeText(calendarId)}");
        sb.Append($"\r\nURL:{_itemUrl}");
        sb.Append($"\r\nDTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
        sb.Append($"\r\nSUMMARY:{EscapeText(_title)}");
        sb.Append($"\r\nDESCRIPTION:{EscapeText(_description)}");
        sb.Append("\r\nPRIORITY:5");
        sb.Append("\r\nCLASS:PUBLIC");
        sb.Append("\r\nEND:VEVENT");

        return sb.ToString();
    }

    public static string EscapeText(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return value
            .Replace("\\", "\\\\")
            .Replace(";", "\\;")
            .Replace(",", "\\,")
            .Replace("\r\n", "\\n")
            .Replace("\n", "\\n")
            .Replace("\r", "\\n");
    }

    private static void AppendDate(StringBuilder sb, string name, DateTime value, bool hasTime)
    {
        if (hasTime)
        {
            sb.Append($"\r\n{name}:{value.ToUniversalTime().ToString(DateTimeFormat)}");
            return;
        }

        sb.Append($"\r\n{name};VALUE=DATE:{value:yyyyMMdd}");
    }
}
