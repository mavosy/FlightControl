using FlightControl.Interfaces;
using FlightControl.Models;

namespace FlightControl.Services
{
	/// <summary>
	/// Manages and coordinates the airplane operations, maintaining a list of airplanes and providing methods for their control.
	/// </summary>
	public interface IControlTower : IListManager<Airplane>
    {
		/// <summary>
		/// Event triggered when a new airplane is added.
		/// </summary>
		event EventHandler<AirplaneEventArgs> PlaneAdded;

		/// <summary>
		/// Event triggered when an existing airplane is updated (e.g., took off, changed altitude, landed).
		/// </summary>
		event EventHandler<AirplaneEventArgs> PlaneUpdated;

		/// <summary>
		/// Delegate for handling changes in airplane altitude.
		/// </summary>
		delegate int ChangeAltitudeDelegate(int changeInAltitude);

		/// <summary>
		/// Provides a copy of the list of airplanes managed by the control tower.
		/// </summary>
		IEnumerable<Airplane> Airplanes { get; }

		/// <summary>
		/// Adds a new airplane with specified details to the control list.
		/// </summary>
		void AddPlane(string name, string flightId, string destination, double flightTime);

        void OnAirplaneTookOff(object sender, AirplaneEventArgs e);
        void OnAirplaneLanded(object sender, AirplaneEventArgs e);

		/// <summary>
		/// Adds some test values to the control tower for demonstration and testing purposes.
		/// </summary>
		void AddTestValues();

		/// <summary>
		/// Orders the specified airplane to take off.
		/// </summary>
		void OrderTakeOff(Airplane airplane);

		/// <summary>
		/// Orders a change in altitude for the specified airplane using the provided delegate.
		/// </summary>
		int OrderAltitudeChange(Airplane airplane, int change);
    }
}