using System;

namespace STAIExtensions.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataContractTableAttribute : Attribute
    {

        #region Properties
        public string TableName { get; private set; }
        #endregion

        #region ctor

        public DataContractTableAttribute(string tableName)
        {
            this.TableName = tableName;
        }
        #endregion
        
    }
}