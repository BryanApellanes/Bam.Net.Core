namespace Bam.Net.Data
{
    /// <summary>
    /// Class that represents the identifiers for a specific instance of Data.
    /// </summary>
    public class Identifiers
    {
        public object Data { get; set; }
        
        public ulong LocalId { get; }
        
        public string[] CompositeKeyProperties { get; set; }
        
        public ulong CompositeKey { get; set; }
        
        public ulong DeterministicId { get; set; }

        /*public static Identifiers For(object data)
        {
            return new Identifiers()
            {
                
            }
        }*/

        /*protected static ulong GetLocalId(object data)
        {
            if (data is Dao dao)
            {
                //return dao.Get
            }
        }*/
    }
}