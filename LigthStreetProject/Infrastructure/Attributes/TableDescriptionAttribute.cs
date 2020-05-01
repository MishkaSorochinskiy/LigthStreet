using System;

namespace Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class TableDescriptionAttribute : Attribute
    {
        public TableDescriptionAttribute(string fullName)
        {
            FullName = fullName;
        }
        public virtual string FullName { get; }
    }
}
