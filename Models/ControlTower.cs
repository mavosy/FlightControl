using FlightControl.Services;

namespace FlightControl.Models
{
	/// <summary>
	/// Manages and coordinates the airplane operations, maintaining a list of airplanes and providing methods for their control.
	/// </summary>
	public class ControlTower : ListManager<Airplane>, IControlTower
    {
        private readonly ListManager<Airplane> _airplanes;

		/// <summary>
		/// Event triggered when a new airplane is added.
		/// </summary>
		public event EventHandler<AirplaneEventArgs> PlaneAdded;

		/// <summary>
		/// Event triggered when an existing airplane is updated (e.g., took off, changed altitude, landed).
		/// </summary>
		public event EventHandler<AirplaneEventArgs> PlaneUpdated;

		/// <summary>
		/// Delegate for handling changes in airplane altitude.
		/// </summary>
		public delegate int ChangeAltitudeDelegate(int changeInAltitude);

        public ControlTower()
        {
            _airplanes = new ListManager<Airplane>();
        }

		/// <summary>
		/// Provides a copy of the list of airplanes managed by the control tower.
		/// </summary>
		public IEnumerable<Airplane> Airplanes => _airplanes.CopyList();

		/// <summary>
		/// Adds a new airplane with specified details to the control list.
		/// </summary>
		public void AddPlane(string name, string flightId, string destination, double flightTime)
          {
            Airplane airplane = new Airplane(name, flightId, destination, flightTime);
            airplane.TookOff += OnAirplaneTookOff;
            airplane.Landed += OnAirplaneLanded;
            airplane.ChangedAltitude += OnAirplaneChangedAltitude;
            PlaneAdded?.Invoke(this, new AirplaneEventArgs(airplane, $"Flight {airplane.Name} headed for runway."));
        }

        private void OnAirplaneChangedAltitude(object? sender, AirplaneEventArgs e)
        {
            PlaneUpdated?.Invoke(this, e);
        }

        public void OnAirplaneTookOff(object? sender, AirplaneEventArgs e)
        {
            PlaneUpdated?.Invoke(this, e);
        }

        public void OnAirplaneLanded(object? sender, AirplaneEventArgs e)
        {
            PlaneUpdated?.Invoke(this, e);
        }

		/// <summary>
		/// Adds some test values to the control tower for demonstration and testing purposes.
		/// </summary>
		public void AddTestValues()
        {
            AddPlane("Boeing 737", "FL100", "New York", 5);
            AddPlane("Airbus A320", "FL200", "Los Angeles", 3);
            AddPlane("Boeing 777", "FL300", "Chicago", 7);
            AddPlane("Airbus A380", "FL400", "San Francisco", 6);
            AddPlane("Embraer 190", "FL500", "Seattle", 10);
        }

		/// <summary>
		/// Orders the specified airplane to take off.
		/// </summary>
		public void OrderTakeOff(Airplane airplane)
        {
            airplane.TakeOff();
        }

		/// <summary>
		/// Orders a change in altitude for the specified airplane using the provided delegate.
		/// </summary>
		public int OrderAltitudeChange(Airplane airplane, int change)
        {
            ChangeAltitudeDelegate changeAltitudeDelegate = airplane.OnChangeAltitude;
            return changeAltitudeDelegate(change);
        }
    }
}