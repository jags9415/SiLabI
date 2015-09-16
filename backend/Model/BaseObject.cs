using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    [DataContract]
    public class BaseObject
    {
        public virtual bool isEmpty()
        {
            return this.GetType().GetProperties().All(pi => pi.GetValue(this, null) == null);
        }
    }
}