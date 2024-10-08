using FlightControl.Commands;
using FlightControl.Models;
using FlightControl.Services;
using FlightControl.Utility.CustomAttributes;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

namespace FlightControl.ViewModels
{
	/// <summary>
	/// View model for the main view.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {
        private readonly IControlTower _controlTower;

        public MainViewModel(IControlTower controlTower)
        {
            _controlTower = controlTower ?? throw new ArgumentNullException(nameof(controlTower));
            Airplanes = new ObservableCollection<Airplane>();
            ActivityLog = new ObservableCollection<string>();

            _controlTower.PlaneAdded += OnPlaneAdded;
            _controlTower.PlaneUpdated += OnPlaneUpdated;

            AddPlaneCommand = new RelayCommand(_ => AddPlane());
            TakeOffCommand = new RelayCommand(TakeOff, _ => CanTakeOff());
            AltitudeChangeCommand = new RelayCommand(ChangeAltitude, _ => CanChangeAltitude());

            _controlTower.AddTestValues();
        }

		/// <summary>
		/// The currently selected airplane in the UI.
		/// </summary>
		public Airplane? SelectedPlane { get; set; }

		/// <summary>
		/// Maintains a collection of airplanes monitored by the control tower.
		/// </summary>
		public ObservableCollection<Airplane> Airplanes { get; set; }

		/// <summary>
		/// Logs activities performed via the control tower.
		/// </summary>
		public ObservableCollection<string> ActivityLog { get; set; }

		/// <summary>
		/// Command to add a new airplane.
		/// </summary>
		public ICommand AddPlaneCommand { get; }

		/// <summary>
		/// Command to initiate takeoff for the selected airplane.
		/// </summary>
		public ICommand TakeOffCommand { get; }

		/// <summary>
		/// Command to change altitude for the selected airplane.
		/// </summary>
		public ICommand AltitudeChangeCommand { get; }

        [AlphaNumeric]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name may not exceed 50 characters.")]
        public string Name { get; set; }

        [AlphaNumeric]
        [Required(ErrorMessage = "Flight ID is required.")]
        [StringLength(20, ErrorMessage = "Flight ID may not exceed 20 characters.")]
        public string FlightId { get; set; }

        [AlphaNumeric]
        [Required(ErrorMessage = "Destination is required.")]
        [StringLength(50, ErrorMessage = "Destination may not exceed 50 characters.")]
        public string Destination { get; set; }

        [Required(ErrorMessage = "Flight time is required.")]
        [Range(0.1, 48.0, ErrorMessage = "Flight time must be between 1 and 48 hours.")]
        public double FlightTime { get; set; }

        public int ChangeInAltitude { get; set; }


		/// <summary>
		/// Handles the PlaneAdded event from the control tower. Adds the new airplane to the collection and logs the event message.
		/// </summary>
		private void OnPlaneAdded(object? sender, AirplaneEventArgs e)
        {
            Airplanes.Add(e.Airplane);
            ActivityLog.Add(e.Message);
        }

		/// <summary>
		/// Handles the PlaneUpdated event from the control tower. Updates the properties of the existing airplane
		/// if found in the collection and logs the update message.
		/// </summary>
		private void OnPlaneUpdated(object? sender, AirplaneEventArgs e)
        {
            Airplane? selectedPlane = SelectedPlane;
            Airplane? plane = Airplanes.FirstOrDefault(p => p.FlightId == e.Airplane.FlightId);
            if (plane != null)
            {
                plane.CruiseAltitude = e.Airplane.CruiseAltitude;
                plane.Destination = e.Airplane.Destination;
                ActivityLog.Add(e.Message);
                SelectedPlane = selectedPlane;
            }
            else
            {
                MessageBox.Show($"Could not update {e.Airplane.Name}'s status.");
            }
        }

		/// <summary>
		/// Adds a new airplane to the control tower's management.
		/// </summary>
		private void AddPlane()
        {
            _controlTower.AddPlane(Name, FlightId, Destination, FlightTime);
            ClearTempProperties();
        }

		/// <summary>
		/// Clears the temporary user input properties used for creating a new airplane.
		/// </summary>
		private void ClearTempProperties()
        {
            Name = string.Empty;
            FlightId = string.Empty;
            Destination = string.Empty;
            FlightTime = 0;
        }

		/// <summary>
		/// Determines whether the selected airplane can take off based on its current state.
		/// </summary>
		private bool CanTakeOff() => SelectedPlane != null;

		/// <summary>
		/// Executes the takeoff command for the selected airplane.
		/// </summary>
		private void TakeOff(object parameter)
        {
            if (parameter is Airplane airplane)
            {
                _controlTower.OrderTakeOff(airplane);
            }
        }

		/// <summary>
		/// Determines whether the selected airplane can change its altitude.
		/// </summary>
		private bool CanChangeAltitude() => SelectedPlane != null;

		/// <summary>
		/// Executes the altitude change command for the selected airplane, using the specified change in altitude.
		/// </summary>
		private void ChangeAltitude(object parameter)
        {
            if (parameter is Airplane airplane)
            {
                _controlTower.OrderAltitudeChange(airplane, ChangeInAltitude);
            }
        }
    }
}