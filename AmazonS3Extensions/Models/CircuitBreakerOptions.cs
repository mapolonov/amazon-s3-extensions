namespace AmazonS3Extensions.Models
{ 
    public class CircuitBreakerOptions {
        /// <summary>
        /// The number of exceptions that are allowed before opening the circuit.
        /// </summary>
        public int ExceptionsAllowedBeforeBreaking { get; set; }

        /// <summary>
        /// The number of milliseconds to open the circuit.
        /// </summary>
        public int MillisecondsOfBreak { get; set; }
    }
}
