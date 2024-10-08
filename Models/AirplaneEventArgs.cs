namespace FlightControl.Models
{
    /// <summary>
    /// Event arguments for airplane events, providing additional details.
    /// </summary>
    public class AirplaneEventArgs : EventArgs
    {
        public AirplaneEventArgs(Airplane airplane, string message)
        {
            Airplane = airplane;
            Message = message;
        }

        /// <summary>
        /// The airplane object involved in the event.
        /// </summary>
        public Airplane Airplane { get; }

        /// <summary>
        /// A message providing details about the event.
        /// </summary>
        public string Message { get; }
    }
}