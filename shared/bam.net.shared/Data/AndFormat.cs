namespace Bam.Net.Data
{
    public class AndFormat: SetFormat
    {
        public AndFormat() { }

        public AndFormat(IQueryFilter filter)
        {
            foreach (IParameterInfo param in filter.Parameters)
            {
                this.AddParameter(param);
            }
            this.Filter = filter;
        }

        private IQueryFilter Filter;

        public override string Parse()
        {
            AssignNumbers();
            SetColumnNameFormatter();
            SetParameterPrefixes();
            string value = string.Empty;
            if (Filter != null)
            {
                value = string.Format("AND {0} ", Filter.Parse(this.StartNumber));
            }
            else
            {
                if(this.Parameters[0] != null)
                {
                    value = string.Format("AND {0} ", this.Parameters[0].ToString());
                }
            }

            return value;
        }
    }
}