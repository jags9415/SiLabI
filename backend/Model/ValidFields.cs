using SiLabI.Model.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SiLabI.Model
{
    public class ValidFields
    {
        private static List<Field> _GenericUserValidFields;
        private static List<Field> _UserValidFields;
        private static List<Field> _OperatorValidFields;
        private static List<Field> _CourseValidFields;
        private static List<Field> _GroupValidFields;
        private static List<Field> _PeriodValidFields;
        private static List<Field> _SoftwareValidFields;
        private static List<Field> _LaboratoryValidFields;

        private static List<Field> GenericUser
        {
            get
            {
                if (_GenericUserValidFields == null)
                {
                    _GenericUserValidFields = new List<Field>();
                    _GenericUserValidFields.Add(new Field("id", SqlDbType.Int));
                    _GenericUserValidFields.Add(new Field("name", SqlDbType.VarChar));
                    _GenericUserValidFields.Add(new Field("last_name_1", SqlDbType.VarChar));
                    _GenericUserValidFields.Add(new Field("last_name_2", SqlDbType.VarChar));
                    _GenericUserValidFields.Add(new Field("full_name", SqlDbType.VarChar));
                    _GenericUserValidFields.Add(new Field("username", SqlDbType.VarChar));
                    _GenericUserValidFields.Add(new Field("email", SqlDbType.VarChar));
                    _GenericUserValidFields.Add(new Field("phone", SqlDbType.VarChar));
                    _GenericUserValidFields.Add(new Field("gender", SqlDbType.VarChar));
                    _GenericUserValidFields.Add(new Field("created_at", SqlDbType.DateTime));
                    _GenericUserValidFields.Add(new Field("updated_at", SqlDbType.DateTime));
                    _GenericUserValidFields.Add(new Field("state", SqlDbType.VarChar));
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
                    _UserValidFields.Add(new Field("type", SqlDbType.VarChar));
                }
                return _UserValidFields;
            }
        }

        public static List<Field> Operator
        {
            get
            {
                if (_OperatorValidFields == null)
                {
                    _OperatorValidFields = new List<Field>(GenericUser);
                    _OperatorValidFields.Add(new Field("period", ValidFields.Period));
                }
                return _OperatorValidFields;
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

        public static List<Field> Administrator
        {
            get { return GenericUser; }
        }

        public static List<Field> Period
        {
            get
            {
                if (_PeriodValidFields == null)
                {
                    _PeriodValidFields = new List<Field>();
                    _PeriodValidFields.Add(new Field("value", SqlDbType.Int));
                    _PeriodValidFields.Add(new Field("type", SqlDbType.VarChar));
                    _PeriodValidFields.Add(new Field("year", SqlDbType.Int));
                }
                return _PeriodValidFields;
            }
        }

        public static List<Field> Course
        {
            get
            {
                if (_CourseValidFields == null)
                {
                    _CourseValidFields = new List<Field>();
                    _CourseValidFields.Add(new Field("id", SqlDbType.Int));
                    _CourseValidFields.Add(new Field("name", SqlDbType.VarChar));
                    _CourseValidFields.Add(new Field("code", SqlDbType.VarChar));
                    _CourseValidFields.Add(new Field("state", SqlDbType.VarChar));
                    _CourseValidFields.Add(new Field("created_at", SqlDbType.DateTime));
                    _CourseValidFields.Add(new Field("updated_at", SqlDbType.DateTime));
                }
                return _CourseValidFields;
            }
        }

        public static List<Field> Group
        {
            get
            {
                if (_GroupValidFields == null)
                {
                    _GroupValidFields = new List<Field>();
                    _GroupValidFields.Add(new Field("id", SqlDbType.Int));
                    _GroupValidFields.Add(new Field("number", SqlDbType.VarChar));
                    _GroupValidFields.Add(new Field("state", SqlDbType.VarChar));
                    _GroupValidFields.Add(new Field("created_at", SqlDbType.DateTime));
                    _GroupValidFields.Add(new Field("updated_at", SqlDbType.DateTime));
                    _GroupValidFields.Add(new Field("period", ValidFields.Period));
                    _GroupValidFields.Add(new Field("course", ValidFields.Course));
                    _GroupValidFields.Add(new Field("professor", ValidFields.Professor));
                }
                return _GroupValidFields;
            }
        }

        public static List<Field> Software
        {
            get
            {
                if (_SoftwareValidFields == null)
                {
                    _SoftwareValidFields = new List<Field>();
                    _SoftwareValidFields.Add(new Field("id", SqlDbType.Int));
                    _SoftwareValidFields.Add(new Field("name", SqlDbType.VarChar));
                    _SoftwareValidFields.Add(new Field("code", SqlDbType.VarChar));
                    _SoftwareValidFields.Add(new Field("state", SqlDbType.VarChar));
                    _SoftwareValidFields.Add(new Field("created_at", SqlDbType.DateTime));
                    _SoftwareValidFields.Add(new Field("updated_at", SqlDbType.DateTime));
                }
                return _SoftwareValidFields;
            }
        }

        public static List<Field> Laboratory
        {
            get
            {
                if (_LaboratoryValidFields == null)
                {
                    _LaboratoryValidFields = new List<Field>();
                    _LaboratoryValidFields.Add(new Field("id", SqlDbType.Int));
                    _LaboratoryValidFields.Add(new Field("name", SqlDbType.VarChar));
                    _LaboratoryValidFields.Add(new Field("seats", SqlDbType.Int));
                    _LaboratoryValidFields.Add(new Field("state", SqlDbType.VarChar));
                    _LaboratoryValidFields.Add(new Field("created_at", SqlDbType.DateTime));
                    _LaboratoryValidFields.Add(new Field("updated_at", SqlDbType.DateTime));
                }
                return _LaboratoryValidFields;
            }
        }
    }
}