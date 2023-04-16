namespace Integration.EskomSePush
{
    [Serializable]
    public  class EksdomException : Exception
    {
        public EksdomException() 
        { 
        }

        public EksdomException(string message)
            : base(message)
        {
        }

        public EksdomException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
