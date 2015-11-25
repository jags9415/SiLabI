using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// Base data model object.
    /// </summary>
    [DataContract]
    public class BaseObject
    {
        /// <summary>
        /// Check if all the object properties are null.
        /// </summary>
        /// <returns>True if all the object properties are null.</returns>
        public virtual bool isEmpty()
        {
            return this.GetType().GetProperties().All(pi => pi.GetValue(this, null) == null);
        }
    }
}