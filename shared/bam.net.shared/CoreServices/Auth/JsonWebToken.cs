using System;
using Bam.Net;
using Renci.SshNet.Security.Cryptography;

namespace Bam.Net.CoreServices.Auth
{
    public class JsonWebToken
    {
        public JsonWebToken(string token)
        {
            Token = token;
        }

        private string _header;
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                string encoded = _header.ToBase64UrlEncoded();
                if (Base64UrlEncodedHeader != encoded)
                {
                    Base64UrlEncodedHeader = encoded;
                } 
            }
        }

        private string _payload;
        public string Payload
        {
            get => _payload;
            set
            {
                _payload = value;
                string encoded = _payload.ToBase64UrlEncoded();
                if (Base64UrlEncodedPayload != encoded)
                {
                    Base64UrlEncodedPayload = encoded;
                }
            }
        }

        public string Signature { get; set; }

        private string _base64UrlEncodedHeader;
        public string Base64UrlEncodedHeader
        {
            get => _base64UrlEncodedHeader;
            set
            {
                _base64UrlEncodedHeader = value;
                string decoded = _base64UrlEncodedHeader.FromBase64UrlEncoded().FromBytes();
                if (Header != decoded)
                {
                    Header = decoded;
                }
            }
        }

        private string _base64UrlEncodedPayload;

        public string Base64UrlEncodedPayload
        {
            get => _base64UrlEncodedPayload;
            set
            {
                _base64UrlEncodedPayload = value;
                string decoded = _base64UrlEncodedPayload.FromBase64UrlEncoded().FromBytes();
                if (Payload != decoded)
                {
                    Payload = decoded;
                }
            }
        }

        private string _token;
        public string Token
        {
            get => _token;
            private set
            {
                _token = value;
                InitFromBearerToken();
            }
        }

        /// <summary>
        /// Regenerates the signature with the specified key and returns true if the generated value matches the
        /// current signature.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsValid(string key)
        {
            return Signature.Equals(GetSignature(key));
        }

        private string _key;
        public void SetSignature()
        {
            Args.ThrowIf<ArgumentException>(string.IsNullOrEmpty(_key), "Key not set");
            SetSignature(_key);
        }

        public void SetSignature(string key)
        {
            Args.ThrowIf<ArgumentException>(string.IsNullOrEmpty(key), "Key not specified");
            Signature = GetSignature(key);
        }

        public void SetKey(string key)
        {
            _key = key;
        }

        public string GetSignature()
        {
            Args.ThrowIf<ArgumentException>(string.IsNullOrEmpty(_key), "Key not set");
            return GetSignature(_key);
        }
        
        public string GetSignature(string key)
        {
            Args.ThrowIf<ArgumentException>(string.IsNullOrEmpty(key), "Key not specified");
            return $"{Base64UrlEncodedHeader}.{Base64UrlEncodedPayload}".HmacSha256Base64UrlEncoded(key);
        }

        public string GetBearerToken()
        {
            return $"Bearer {GetToken()}";
        }

        public string GetBearerToken(string key)
        {
            return $"Bearer {GetToken(key)}";
        }
        
        public string GetToken()
        {
            Args.ThrowIf<ArgumentException>(string.IsNullOrEmpty(_key), "Key not set");
            return GetToken(_key);
        }

        public string GetToken(string key)
        {
            Args.ThrowIf<ArgumentException>(string.IsNullOrEmpty(key), "Key not specified");
            SetKey(key);
            return $"{Base64UrlEncodedHeader}.{Base64UrlEncodedPayload}.{GetSignature(key)}";
        }
        
        private void InitFromBearerToken()
        {
            string[] segments = Token.DelimitSplit(".");
            if (segments.Length != 3)
            {
                throw new ArgumentException("Invalid bearer token");
            }

            Base64UrlEncodedHeader = segments[0];
            Base64UrlEncodedPayload = segments[1];
            Signature = segments[2];
        }
    }
}