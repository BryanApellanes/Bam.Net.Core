using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Bam.Net.Data.Repositories;

namespace Bam.Net
{
    /// <summary>
    /// Represents an array of values that are read and written from a string.
    /// </summary>
    public class ArrayString : RepoData
    {
        public static implicit operator string[](ArrayString value)
        {
            return value.AsArray;
        }

        public static implicit operator string(ArrayString value)
        {
            return value.Value;
        }

        public static implicit operator ArrayString(string value)
        {
            return new ArrayString(value);
        }
        
        public ArrayString(string value) : this(value, ",", ";")
        {
        }

        public ArrayString(string value, params string[] delimiters)
        {
            if (delimiters == null || delimiters.Length == 0)
            {
                delimiters = new string[] {",", ";"};
            }
            _delimiters = delimiters;
            Value = value;
            AsArray = value.DelimitSplit(delimiters);
        }

        private readonly string[] _delimiters;
        
        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                _asArray = _value?.DelimitSplit(_delimiters);
            }
        }

        private string[] _asArray;
        [Exclude]
        [JsonIgnore]
        public string[] AsArray
        {
            get => _asArray;
            set
            {
                _asArray = value;
                _value = string.Join(",", _asArray);
            }
        }
    }
}