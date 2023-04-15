namespace Integration.EskomSePush
{
    /// <summary>
    /// Used for developer testing the API. Using a value of 'Current' or 'Future' will return test sample data.
    /// As per ESP documentation:
    /// <para>
    /// "The schedule returned with testing data is NOT accurate data; but only for testing purposes.
    /// The area name and source is updated to identify that this is testing data. 
    /// This test request will not count towards your quota."
    /// </para>
    /// </summary>
    public enum ApiTestModes
    {
        /// <summary>
        /// Use Live data that will count towards your quote
        /// </summary>
        None = 0,
        /// <summary>
        /// <paramref name="Current"></paramref> will return a loadshedding event which is occurring right now
        /// </summary>
        Current = 1,
        /// <summary>
        /// <paramref name="Future"></paramref> will return an event starting on the next hour
        /// </summary>
        Future = 2
    }
}
