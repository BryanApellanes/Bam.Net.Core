using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A series of Events. Included events can relate with the series using the superEvent property.An EventSeries is a collection of events that share some unifying characteristic. For example, "The Olympic Games" is a series, whichis repeated regularly. The "2012 London Olympics" can be presented both as an Event in the series "Olympic Games", and as anEventSeries that included a number of sporting competitions as Events.The nature of the association between the events in an EventSeries can vary, but typical examples couldinclude a thematic event series (e.g. topical meetups or classes), or a series of regular events that share a location, attendee group and/or organizers.EventSeries has been defined as a kind of Event to make it easy for publishers to use it in an Event context withoutworrying about which kinds of series are really event-like enough to call an Event. In general an EventSeriesmay seem more Event-like when the period of time is compact and when aspects such as location are fixed, butit may also sometimes prove useful to describe a longer-term series as an Event.</summary>
	public class EventSeriesSeries: Series
	{
	}
}
