using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Localization
{
    public class DescriptionAttributes<T>
    {
        protected List<DescriptionAttribute> Attributes = new List<DescriptionAttribute>();
        public List<string> Descriptions { get; set; }

        public DescriptionAttributes()
        {
            RetrieveAttributes();
            Descriptions = Attributes.Select(x => x.Description).ToList();
        }

        private void RetrieveAttributes()
        {
            foreach (var attribute in typeof(T).GetMembers().SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>()))
            {
                Attributes.Add(attribute);
            }
        }
    }
}
