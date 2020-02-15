using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Data
{
    [Serializable]
    public class DataProperty: KeyedAuditRepoData
    {
        [CompositeKey]
        public string TypeNamespace { get; set; }
        [CompositeKey]
        public string TypeName { get; set; }
        [CompositeKey]
        public string InstanceIdentifier { get; set; }
        [CompositeKey]
        public string Name { get; set; }
        public object Value { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is DataProperty dataProp)
            {
                return dataProp.Name.Equals(Name) && Value.Equals(dataProp.Value) && InstanceIdentifier.Equals(dataProp.InstanceIdentifier);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return this.GetHashCode(InstanceIdentifier, Name, Value);
        }
        
        public virtual ulong CreateOperationId { get; set; }
        public virtual CreateOperation CreateOperation { get; set; }
        public virtual ulong DeleteEventId { get; set; }
        public virtual DeleteEvent DeleteEvent { get; set; }
        public virtual ulong DeleteOperationId { get; set; }
        public virtual DeleteOperation DeleteOperation { get; set; }
        public virtual ulong UpdateOperationId { get; set; }
        public virtual UpdateOperation UpdateOperation { get; set; }
        public virtual ulong WriteEventId { get; set; }
        public virtual WriteEvent WriteEvent { get; set; }
        public virtual ulong SaveOperationId { get; set; }
        public virtual SaveOperation SaveOperation { get; set; }

        static Func<object, string> _valueConverter;
        static readonly object _valueConverterLock = new object();
        public static Func<object, string> ValueConverter
        {
            get
            {
                return _valueConverterLock.DoubleCheckLock<Func<object, string>>(ref _valueConverter, () => { return (o) => o?.ToString() ?? "NULL"; });
            }
            set => _valueConverter = value;
        }

        private static Func<object, string[]> _arrayConverter;
        private static readonly object _arrayConverterLock = new object();

        public static Func<object, string[]> ArrayConverter
        {
            get { return _arrayConverterLock.DoubleCheckLock<Func<object, string[]>>(ref _arrayConverter, () =>
                {
                    return (o) =>
                    {
                        if (o is Array array)
                        {
                            List<string> stringValues = new List<string>();
                            foreach (object val in array)
                            {
                                stringValues.Add(ValueConverter(val));
                            }

                            return stringValues.ToArray();
                        }
                        else
                        {
                            return new string[] {ValueConverter(o)};
                        }
                    };
                }); 
            }
            set => _arrayConverter = value;
        }

        public static DataPropertyFilter Where(object property, QueryOperator queryOperator, object value)
        {
            return FilterWhere(property, queryOperator, value);
        }
        
        public static DataPropertyFilter FilterWhere(object property, QueryOperator queryOperator, object value)
        {
            Args.ThrowIfNull(property, "property");
            return new DataPropertyFilter(){Name = property.ToString(), Operator = queryOperator, Value = value};
        }
    }
}
