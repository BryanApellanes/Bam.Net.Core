using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A schedule defines a repeating time period used to describe a regularly occurring Event. At a minimum a schedule will specify repeatFrequency which describes the interval between occurences of the event. Additional information can be provided to specify the schedule more precisely.       This includes identifying the day(s) of the week or month when the recurring event will take place, in addition to its start and end time. Schedules may also      have start and end dates to indicate when they are active, e.g. to define a limited calendar of events.</summary>
	public class Schedule: Intangible
	{
		///<summary>Defines the day(s) of the week on which a recurring Event takes place</summary>
		public DayOfWeek ByDay {get; set;}
		///<summary>Defines the month(s) of the year on which a recurring Event takes place. Specified as an Integer between 1-12. January is 1.</summary>
		public Integer ByMonth {get; set;}
		///<summary>Defines the day(s) of the month on which a recurring Event takes place. Specified as an Integer between 1-31.</summary>
		public Integer ByMonthDay {get; set;}
		///<summary>Associates an Event with a Schedule. There are circumstances where it is preferable to share a schedule for a series of      repeating events rather than data on the individual events themselves. For example, a website or application might prefer to publish a schedule for a weekly      gym class rather than provide data on every event. A schedule could be processed by applications to add forthcoming events to a calendar. An Event that      is associated with a Schedule using this property should not have startDate or endDate properties. These are instead defined within the associated      Schedule, this avoids any ambiguity for clients using the data. The propery might have repeated values to specify different schedules, e.g. for different months      or seasons.</summary>
		public Duration EventSchedule {get; set;}
		///<summary>Defines a Date or DateTime during which a scheduled Event will not take place. The property allows exceptions to      a Schedule to be specified. If an exception is specified as a DateTime then only the event that would have started at that specific date and time      should be excluded from the schedule. If an exception is specified as a Date then any event that is scheduled for that 24 hour period should be      excluded from the schedule. This allows a whole day to be excluded from the schedule without having to itemise every scheduled event.</summary>
		public OneOfThese<Bam.Net.Schema.Org.DataTypes.Date,Bam.Net.Schema.Org.DataTypes.Date> ExceptDate {get; set;}
		///<summary>Defines the number of times a recurring Event will take place</summary>
		public Integer RepeatCount {get; set;}
		///<summary>Defines the frequency at which Events will occur according to a schedule Schedule. The intervals between      events should be defined as a Duration of time.</summary>
		public OneOfThese<Duration,Text> RepeatFrequency {get; set;}
	}
}
