//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Net;

namespace Rock.DataTransferObjects.Checkin
{
	/// <summary>
	/// This represents the main datagram of a checkin request.  It's the object
	/// that is passed through the check-in system's filtering mechanism
	/// and over to the client for displaying (as appropriate) for the entire
	/// set of check-in steps being performed.
	/// </summary>
	public class RequestData
	{
		/// <summary>
		/// The kiosk that is performing the checkin operation.
		/// </summary>
		public Kiosk Kiosk = new Kiosk();

		/// <summary>
		/// The inital family search value (barcode, phone number, name, etc.).  
		/// </summary>
		public string SearchValue = string.Empty;

		/// <summary>
		/// The list of families that match the given search value.
		/// Inside each family is a person who could be (and then later who is being)
		/// checked in.
		/// </summary>
		public List<Family> Families = new List<Family>();
	}

	/// <summary>
	/// This represents a person who is a candidate for possible checkin.
	/// </summary>
	public class CheckinPerson : CRM.DTO.Person
	{
		/// <summary>
		/// Flag to control whether or not someone should not be allowed to check-in.
		/// Use case is when a volunteer has committed some violation and is no longer being allowed to check-in (?).
		/// </summary>
		public bool IsAllowedToCheckIn = true;
		/// <summary>
		/// This represents the list of events that could possibly be checked into.
		/// </summary>
		public List<PossibleEvent> PossibleEvents = new List<PossibleEvent>();
		/// <summary>
		/// This represents the list of events that the person is checked into.
		/// </summary>
		public List<Event> CheckedInEvents = new List<Event>();
	}

	/// <summary>
	/// This represents the list of events that could possibly be checked into.
	/// </summary>
	public class PossibleEvent : Event
	{
		/// <summary>
		/// 
		/// </summary>
		public bool CanCheckIn; // not 100% why this is/was needed. If it is, when we figure it out, then it needs to be documented here.
	}

	/// <summary>
	/// This represents the list of events someone is attempting to be checked in to.
	/// </summary>
	public class CheckedInEvent : Event
	{
		/// <summary>
		/// This flag indicates whether or not the person was actually checked in.
		/// </summary>
		public bool IsCheckedIn = true;

		/// <summary>
		/// Indicates whether or not there are errors that need to be handled/displayed/relayed/etc.
		/// </summary>
		public bool HasErrors = false;
	}

	/// <summary>
	/// A pseudo class that represents the family group.  Not sure yet
	/// if this is going to extend the "Group" or just be a simplified
	/// representation (more concrete) of the family unit.
	/// </summary>
	public class Family : Groups.DTO.Group
	{
		public List<CheckinPerson> CheckinPerson;
	}

	/// <summary>
	/// A pseudo class that represents the check-in event for a person.
	/// </summary>
	public class Event
	{
		public EventType EventType = new EventType();
		/// <summary>
		/// This bit is used to control whether or not this event is active.
		/// </summary>
		public bool IsActive = true;
		/// <summary>
		/// 'Disabled' option for things that need to be displayed but not made clickable.  Use case would be checking in for a volunteer opportunity when you don't have a background check.  The error message could be used for the description I suppose.
		/// </summary>
		public bool IsDisabled = false;
		/// <summary>
		/// This is the public "start time" for the event (NOT the time for which check-in can start for the event).
		/// </summary>
		public DateTime StartTime;
		/// <summary>
		/// This is the list of rooms/locations where this event is taking place.
		/// </summary>
		public List<Room> Rooms = new List<Room>();

		/// <summary>
		/// Holds any errors or error messages that have occurred for this event and person.
		/// </summary>
		public List<Error> Errors;

		/// <summary>
		/// This class represents any error that occurs during the checkin.
		/// </summary>
		public class Error
		{
			public int Code;
			public string Message;
		}
	}

	/// <summary>
	/// A pseudo class that represents the event type.  Not sure yet
	/// if this is going to extend the <see cref="*thing*" /> that represents an event type
	/// or just be a simplified representation (more concrete) of it.
	/// </summary>
	public class EventType
	{
		public string Name;
		public string MinistryCategory;
		public bool UseLoadBalancing = false;
		public int LoadBalancingType; // possibly replace with enum
	}

	/// <summary>
	/// This is to be replaced with the actual class that represents the room/location.
	/// </summary>
	public class Room
	{
		public bool IsOpen = false;
	}
	/// <summary>
	/// This is to be replaced with the actual class that represents the printer.
	/// </summary>
	public struct Printer
	{
	}

	/// <summary>
	/// This represents the checkin Kiosk.
	/// </summary>
	public class Kiosk
	{
		/// <summary>
		/// Represents the possible kiosk modes
		/// </summary>
		public enum KioskMode
		{
			/// <summary>
			/// Unattended, self serve kiosk
			/// </summary>
			SelfServe,
			/// <summary>
			/// Attended by a kiosk volunteer; non-self serve.
			/// </summary>
			Attended
		}

		/// <summary>
		/// The list of <see cref="EventType" /> for which this kiosk can perform checkin
		/// It is used to filter. If empty, the kiosk should not filter by event type.
		/// </summary>
		public List<EventType> EventTypes = new List<EventType>();

		/// <summary>
		/// The list of <see cref="Room" /> for which this kiosk can perform checkin
		/// It is used to filter. If empty, the kiosk should not filter by room/location.
		/// </summary>
		public List<Room> Rooms = new List<Room>();

		/// <summary>
		/// The list of <see cref="Room" /> for which this kiosk can perform checkin
		/// It is used to filter. If empty, the kiosk should not filter by room/location.
		/// </summary>
		public Printer Printer = new Printer();

		/// <summary>
		/// Holds the state of the kiosk's Mode. The Mode is used to control 
		/// behavior of the kiosk.
		/// </summary>
		public KioskMode Mode; 
		public double Latitude = 0.0;
		public double Longitude = 0.0;
		public IPAddress IpAddress = null;
	}
}
