﻿using System;
using System.Configuration;

namespace DU.Themes.Configuration
{
    public class PropertyMappings : ConfigurationSection
    {
        [ConfigurationProperty("Properties", Options = ConfigurationPropertyOptions.IsRequired)]
        public PropertiesCollection DefinedProperties
        {
            get
            {
                return (PropertiesCollection)this["Properties"];
            }
        }
    }

    [ConfigurationCollection(typeof(PropertyElement), AddItemName = "Property")]
    public class PropertiesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PropertyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return ((PropertyElement)element).PocoName;
        }
    }

    public class PropertyElement : ConfigurationElement
    {
        [ConfigurationProperty("poco", IsRequired = true, IsKey = true)]
        public string PocoName
        {
            get { return (string)base["poco"]; }
        }

        [ConfigurationProperty("excel", IsRequired = true)]
        public string ExcelName
        {
            get { return (string)base["excel"]; }
        }
    }
}
