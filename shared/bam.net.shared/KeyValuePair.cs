namespace Bam.Net
{
    public class KeyValuePair<K, V>
    {
        public KeyValuePair()
        {
        }

        public KeyValuePair(K key, V value)
        {
            this.Key = key;
            this.Value = value;
        }
        public K Key { get; set; }
        public V Value { get; set; }
    }
    
    public class KeyValuePair
    {
        public KeyValuePair()
        {
        }
        
        public KeyValuePair(string key, object value)
        {
            this.Key = key;
            this.Value = value;
        }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}