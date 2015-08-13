using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    [DataContract]
    public class User
    {
        [DataMember(EmitDefaultValue = false, Name = "id")]
        public int Id { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "name")]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "last_name_1")]
        public string LastName1 { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "last_name_2")]
        public string LastName2 { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "username")]
        public string Username { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "email")]
        public string Email { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "phone")]
        public string Phone { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "gender")]
        public string Gender { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}