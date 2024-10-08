using PropertyChanged;
using System.Windows.Threading;

namespace FlightControl.Models
{
	/// <summary>
	/// Represents an airplane and manages its flight and operational states.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
    public class Airplane
    {
		// Event declarations for takeoff, landing, and altitude changes
		public event EventHandler<AirplaneEventArgs> TookOff;
        public event EventHandler<AirplaneEventArgs> Landed;
        public event EventHandler<AirplaneEventArgs> ChangedAltitude;

        /// <summary>
        /// Simulates the flight time
        /// </summary>
        private DispatcherTimer _flightTimer;

        public Airplane(string name, string flightId, string destination, double flightTime)
        {
            Name = name;
            FlightId = flightId;
            Destination = destination;
            OriginalDestination = destination;
            FlightTime = flightTime;
            OriginalFlightTime = flightTime;
            CruiseAltitude = 10000;           // Initial cruise altitude in meters
            SetupTimer();
        }

		/// <summary>
		/// The cruising altitude of the airplane in meters.
		/// </summary>
		public int CruiseAltitude { get; set; }

		/// <summary>
		/// Indicating whether the airplane is currently in flight.
		/// </summary>
		public bool InFlight { get; set; }

		/// <summary>
		/// The current destination of the airplane.
		/// </summary>
		public string Destination { get; set; }

		/// <summary>
		/// The original destination of the airplane.
		/// </summary>
		public string OriginalDestination { get; set; }

		/// <summary>
		/// Indicating whether the airplane is returning home after reaching its destination.
		/// </summary>
		public bool HomeBound { get; set; } = false;

		public string FlightId { get; set; }

		/// <summary>
		/// The flight time left in hours.
		/// </summary>
		public double FlightTime { get; set; }

		/// <summary>
		/// The original flight time in hours.
		/// </summary>
		public double OriginalFlightTime { get; }

		/// <summary>
		/// The local time at the airplane's current location.
		/// </summary>
		public TimeOnly LocalTime { get; set; } = TimeOnly.FromDateTime(DateTime.Now);

        public string Name { get; set; }

		/// <summary>
		/// Responds to the timer's tick event, updating the flight time and potentially initiating a landing.
		/// </summary>
		private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            if (FlightTime > 0.0)
            {
                FlightTime -= 0.1;
            }
            if (FlightTime <= 0.0)
            {
                Land();
            }
        }

		/// <summary>
		/// Initiates the airplane's takeoff sequence.
		/// </summary>
		public void TakeOff()
        {
            if (!InFlight)
            {
                SetupTimer();
                StartTimer();
                OnTakeOff();
                InFlight = true;

            }
        }

		/// <summary>
		/// Handles the airplane's landing sequence.
		/// </summary>
		public void Land()
        {
            if (InFlight)
            {
                StopTimer();
                OnLanding();
                InFlight = false;
            }
        }

		/// <summary>
		/// Raises the TookOff event with details about the takeoff.
		/// </summary>
		private void OnTakeOff()
        {
            string messageString = $"The aircraft {Name} is taking off, heading for {Destination}. Estimated arrival time: {LocalTime}";
            AirplaneEventArgs args = new AirplaneEventArgs(this, messageString);
            TookOff?.Invoke(this, args);
        }

		/// <summary>
		/// Raises the Landed event when the airplane reaches its destination or returns home.
		/// </summary>
		private void OnLanding()
        {
            string messageString = $"{Name} has landed in {Destination}, at {LocalTime}.";
            HomeBound = !HomeBound;
            Destination = HomeBound ? "Home" : OriginalDestination;
            AirplaneEventArgs args = new AirplaneEventArgs(this, messageString);
            Landed?.Invoke(this, args);
            FlightTime = OriginalFlightTime;
        }

		/// <summary>
		/// Changes the airplane's altitude and raises the ChangedAltitude event.
		/// </summary>
		public int OnChangeAltitude(int changeInAltitude)
        {
            CruiseAltitude += changeInAltitude;
            string message = $"Flight {Name} changed altitude by {changeInAltitude} to {CruiseAltitude} meters.";
            AirplaneEventArgs args = new AirplaneEventArgs(this, message);
            ChangedAltitude?.Invoke(this, args);
            return CruiseAltitude;
        }

		/// <summary>
		/// Sets up the flight timer for the airplane.
		/// </summary>
		private void SetupTimer()
        {
            if (_flightTimer != null)
            {
                _flightTimer.Tick -= DispatcherTimer_Tick;
            }
            _flightTimer = new DispatcherTimer();
            _flightTimer.Tick += DispatcherTimer_Tick;
            _flightTimer.Interval = TimeSpan.FromSeconds(0.1); // One second represents one hour of flight time
        }

		/// <summary>
		/// Stops the flight timer.
		/// </summary>
		private void StopTimer()
        {
            _flightTimer.Stop();
        }

		/// <summary>
		/// Starts the flight timer.
		/// </summary>
		private void StartTimer()
        {
            _flightTimer.Start();
        }

        public override string ToString() => $"{Name}, {FlightId}, heading for {Destination}";
    }
}