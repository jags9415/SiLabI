using SiLabI.Model.Query;
using System;
using System.Collections.Generic;
using System.Data;
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
        private static List<Field> _ValidFields;

        /// <summary>
        /// The number that identifies the student in the university.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "student_id")]
        public string StudentId { get; set; }

        public static List<Field> ValidFields
        {
            get
            {
                if (Student._ValidFields == null)
                {
                    FieldBuilder builder = new FieldBuilder("Students");
                    Student._ValidFields = User.ValidFields;
                    Student._ValidFields.Add(builder.VarChar("student_id", "Student_Id"));
                }
                return Student._ValidFields;
            }
        }
    }
}