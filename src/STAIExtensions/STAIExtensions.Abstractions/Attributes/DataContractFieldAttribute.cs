using System;

namespace STAIExtensions.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DataContractFieldAttribute : Attribute
    {

        #region Properties
        public string DeserializeFieldName { get; private set; }
        #endregion

        #region ctor

        public DataContractFieldAttribute(string deserializeFieldName)
        {
            this.DeserializeFieldName = deserializeFieldName;
        }
        #endregion
        
    }
}