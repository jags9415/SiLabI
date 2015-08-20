using SiLabI.Model.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI.Model
{
    public class ValidFields
    {
        private static List<Field> _GenericUserValidFields;
        private static List<Field> _UserValidFields;
        private static List<Field> _StudentValidFields;

        private static List<Field> GenericUser
        {
            get
            {
                if (_GenericUserValidFields == null)
                {
                    _GenericUserValidFields = new List<Field>();
                    FieldBuilder builder;

                    builder = new FieldBuilder("Users");
                    _GenericUserValidFields.Add(builder.Int("PK_User_Id", "id"));
                    _GenericUserValidFields.Add(builder.VarChar("Name", "name"));
                    _GenericUserValidFields.Add(builder.VarChar("Last_Name_1", "last_name_1"));
                    _GenericUserValidFields.Add(builder.VarChar("Last_Name_2", "last_name_2"));
                    _GenericUserValidFields.Add(builder.VarChar("Username", "username"));
                    _GenericUserValidFields.Add(builder.VarChar("Email", "email"));
                    _GenericUserValidFields.Add(builder.VarChar("Phone", "phone"));
                    _GenericUserValidFields.Add(builder.VarChar("Gender", "gender"));
                    _GenericUserValidFields.Add(builder.DateTime("Created_At", "created_at"));
                    _GenericUserValidFields.Add(builder.DateTime("Updated_At", "updated_at"));

                    builder = new FieldBuilder("States");
                    _GenericUserValidFields.Add(builder.VarChar("Name", "state"));
                }
                return _GenericUserValidFields;
            }
        }

        public static List<Field> User
        {
            get
            {
                if (_UserValidFields == null)
                {
                    _UserValidFields = new List<Field>(GenericUser);

                    FieldBuilder builder = new FieldBuilder("Users");
                    _UserValidFields.Add(builder.VarChar("Type", "type"));
                }
                return _UserValidFields;
            }
        }

        public static List<Field> Student
        {
            get { return GenericUser; }
        }

        public static List<Field> Professor
        {
            get { return GenericUser; }
        }

        public static List<Field> Operator
        {
            get { return GenericUser; }
        }

        public static List<Field> Administrator
        {
            get { return GenericUser; }
        }
    }
}