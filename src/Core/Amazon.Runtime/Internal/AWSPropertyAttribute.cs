using System;

namespace Amazon.Runtime.Internal
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AWSPropertyAttribute : Attribute
    {
        private long min;
        private long max;
        public bool Required { get; set; }

        public bool IsMinSet { get; private set; }
        public long Min
        {
            get
            {
                return IsMinSet ? min : long.MinValue;
            }
            set
            {
                IsMinSet = true;
                min = value;
            }
        }
        public bool IsMaxSet { get; private set; }
        public long Max
        {
            get
            {
                return IsMaxSet ? max : long.MaxValue;
            }
            set
            {
                IsMaxSet = true;
                max = value;
            }
        }
    }
}
