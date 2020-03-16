using System;

namespace Bam.Net.Schema.Json
{
    public class UnsupportedFormatException: Exception
    {
        public UnsupportedFormatException(SerializationFormat format) : base(
            $"The specified format is not supported: {format.ToString()}")
        {
        }
    }
}