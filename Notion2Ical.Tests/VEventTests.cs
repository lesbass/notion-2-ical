namespace Notion2Ical.Tests;

public class VEventTests
{
    [Fact]
    public void ToString_EscapesICalendarTextFields()
    {
        var vEvent = new VEvent(
            "task-1",
            new DateTime(2030, 1, 2),
            new DateTime(2030, 1, 3),
            false,
            "Pay, send; close\\done",
            "Line 1\nLine 2",
            "https://www.notion.so/task-1");

        var serialized = vEvent.ToString("Calendar, One");

        Assert.Contains("UID:task-1@Calendar\\, One", serialized);
        Assert.Contains("SUMMARY:Pay\\, send\\; close\\\\done", serialized);
        Assert.Contains("DESCRIPTION:Line 1\\nLine 2", serialized);
    }
}
