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
    /// An user data.
    /// </summary>
    [DataContract]
    public class User
    {
        private static List<Field> _ValidFields;

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
        public DateTime? CreatedAt { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "state")]
        public string State { get; set; }

        public static List<Field> ValidFields
        {
            get
            {
                if (User._ValidFields == null)
                {
                    FieldBuilder builder = new FieldBuilder("Users");
                    User._ValidFields = new List<Field>();
                    
                    User._ValidFields.Add(builder.Int("id", "PK_User_Id"));
                    User._ValidFields.Add(builder.VarChar("name", "Name"));
                    User._ValidFields.Add(builder.VarChar("last_name_1", "Last_Name_1"));
                    User._ValidFields.Add(builder.VarChar("last_name_2", "Last_Name_2"));
                    User._ValidFields.Add(builder.VarChar("username", "Username"));
                    User._ValidFields.Add(builder.VarChar("email", "Email"));
                    User._ValidFields.Add(builder.VarChar("phone", "Phone"));
                    User._ValidFields.Add(builder.VarChar("gender", "Gender"));
                    User._ValidFields.Add(builder.DateTime("created_at", "Created_At"));
                    User._ValidFields.Add(builder.DateTime("updated_at", "Updated_At"));
                    
                    builder.Table = "States";
                    User._ValidFields.Add(builder.VarChar("state", "Name"));
                }
                return User._ValidFields;
            }
        }
    }
}