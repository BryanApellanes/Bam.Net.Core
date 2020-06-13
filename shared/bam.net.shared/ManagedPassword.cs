namespace Bam.Net
{
    public class ManagedPassword
    {
        public ManagedPassword()
        {
        }

        public ManagedPassword(string value)
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

        public void Set(string password)
        {
            Value = password;
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