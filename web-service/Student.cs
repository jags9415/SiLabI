using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// A student data.
    /// </summary>
    [DataContract]
    public class Student : User
    {
        public static List<Field> ValidFields = new List<Field>()
        {
            new Field("id", "INT", true),
            new Field("name", "STRING", false),
            new Field("last_name_1", "STRING", false),
            new Field("last_name_2", "STRING", false),
            new Field("username", "STRING", true),
            new Field("email", "STRING", true),
            new Field("phone", "STRING", true),
            new Field("gender", "STRING", true),
            new Field("student_id", "INT", true),
            new Field("created_at", "DATETIME", true),
            new Field("updated_at", "DATETIME", true)
        };
        
        /// <summary>
        /// The number that identifies the student in the university.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "student_id")]
        public string StudentId { get; set; }
    }
}