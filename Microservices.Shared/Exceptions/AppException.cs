namespace Microservices.Shared.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class AppException : Exception
    {
        public int Code { get; } = 400;

        public AppException()
        {
        }

        public AppException(int code)
        {
            Code = code;
        }

        public AppException(int code, string message) : base(message)
        {
            Code = code;
        }

        public AppException(int code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
        public override string ToString()
        {
            return $"Exception message: {Message}\n" +
                $"Exception code: {Code}\n" +
                $"Inner exception: {InnerException}";
        }

        protected AppException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
    }
}
