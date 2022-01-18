using STAIExtensions.Data.AzureDataExplorer.Serialization;

namespace STAIExtensions.Data.AzureDataExplorer.Attributes
{
    /// <summary>
    /// A helper class that can be used during deserialization to map json record fields
    /// to entity properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DataContractFieldAttribute : Attribute
    {

        #region Properties
        /// <summary>
        /// Gets the json record field name to use for deserialization
        /// </summary>
        public string DeserializeFieldName { get; private set; }
        
        /// <summary>
        /// Gets an optional field deserializer to convert the object into a known type
        /// </summary>
        public IFieldDeserializer? FieldDeserializer { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="deserializeFieldName"><see cref="DeserializeFieldName"/></param>
        /// <param name="fieldDeserializer"><see cref="FieldDeserializer"/></param>
        /// <exception cref="ArgumentException"></exception>
        public DataContractFieldAttribute(string deserializeFieldName, Type? fieldDeserializer = default)
        {
            this.DeserializeFieldName = deserializeFieldName;

            if (fieldDeserializer != null)
            {
                if (!typeof(IFieldDeserializer).IsAssignableFrom(fieldDeserializer))
                    throw new ArgumentException($"IFieldDeserializer is not assignable from {fieldDeserializer.Name}.");
                FieldDeserializer = (IFieldDeserializer)Activator.CreateInstance(fieldDeserializer);
            }
        }
        #endregion
        
    }
}