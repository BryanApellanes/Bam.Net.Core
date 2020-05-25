namespace Bam.Net
{
    public class GeneratedPassword
    {
        public GeneratedPassword()
        {
        }

        public GeneratedPassword(string value)
        {
            _value = value;
        }

        public double? JulianDate { get; set; }
        
        private string _value;
        public string Value
        {
            get => "******";
            set => _value = value;
        }
        
        public string Show()
        {
            return _value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}