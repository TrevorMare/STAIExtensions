using System;

namespace STAIExtensions.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DataContractFieldAttribute : Attribute
    {

        #region Properties
        public string DeserializeFieldName { get; private set; }
        
        public Serialization.IFieldDeserializer? FieldDeserializer { get; private set; }
        #endregion

        #region ctor

        public DataContractFieldAttribute(string deserializeFieldName, Type? fieldDeserializer = default)
        {
            this.DeserializeFieldName = deserializeFieldName;

            if (fieldDeserializer != null)
            {
                if (!typeof(Serialization.IFieldDeserializer).IsAssignableFrom(fieldDeserializer))
                    throw new ArgumentException($"IFieldDeserializer is not assignable from {fieldDeserializer.Name}.");
                FieldDeserializer = (Serialization.IFieldDeserializer)Activator.CreateInstance(fieldDeserializer);
            }
        }
        #endregion
        
    }
}