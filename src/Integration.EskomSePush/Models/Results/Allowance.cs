namespace Integration.EskomSePush.Models.Results
{
    /// <summary>
    /// Check API allowance
    /// </summary>
    public class Allowance
    {
        /// <summary>
        /// Credit used
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Credit limit
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Credit balance
        /// </summary>
        public int Balance => Math.Max(Limit - Count, 0);

        /// <inheritdoc/>
        public override string ToString() => $"Used: {Count} Balance: {Balance}";

        /// <summary>
        /// Constructor
        /// </summary>
        public Allowance(int count, int limit) 
        {
            Count = count;
            Limit = limit; 
        }
    }
}
