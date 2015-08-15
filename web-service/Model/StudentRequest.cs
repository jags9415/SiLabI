using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    [DataContract]
    public class StudentRequest : BaseRequest
    {
        [DataMember(EmitDefaultValue = false, Name="student")]
        public Student Student { get; set; }
    }
}