using SiLabI.Exceptions;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// Represent a period of a year.
    /// </summary>
    /// <example>1 Semestre 2015</example>
    [DataContract]
    public class Period : BaseObject
    {
        private int? _value;
        private string _type;
        private int? _year;

        /// <summary>
        /// The period value.
        /// </summary>
        /// <example>1</example>
        [DataMember(EmitDefaultValue = false, Name = "value", Order = 1)]
        public int? Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// The period type.
        /// </summary>
        /// <example>Semestre</example>
        [DataMember(EmitDefaultValue = false, Name = "type", Order = 2)]
        public string Type
        {
            set
            {
                if (!Validator.IsValidPeriod(value))
                {
                    throw new InvalidParameterException("period.type", "Ingrese 'Bimestre', 'Trimestre', 'Cuatrimestre' o 'Semestre'");
                }

                _type = value;
            }
            get { return _type; }
        }

        /// <summary>
        /// The period year.
        /// </summary>
        /// <example>2015</example>
        [DataMember(EmitDefaultValue = false, Name = "year", Order = 3)]
        public int? Year
        {
            get { return _year; }
            set { _year = value; }
        }

        /// <summary>
        /// Check if the period is valid.
        /// </summary>
        /// <returns>True if the period is valid.</returns>
        public bool isValidForCreate()
        {
            bool valid = true;

            valid &= Type != null && Validator.IsValidPeriod(Type);
            valid &= Year != null && Year > 0;
            valid &= Value != null && Value > 0;

            return valid;
        }

        /// <summary>
        /// Check if the period is valid for an update operation.
        /// </summary>
        /// <returns>True if the period is valid.</returns>
        public bool isValidForUpdate()
        {
            bool valid = true;

            if (Type != null || Value != null)
            {
                valid &= Type != null && Validator.IsValidPeriod(Type);
                valid &= Value != null && Value > 0;
            }
            
            if (Year != null) valid &= Year > 0;

            return valid;
        }

        public static Period Parse(DataRow row, string prefix = "")
        {
            prefix = prefix.Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += ".";
            }

            Period period = new Period();

            period.Type = row.Table.Columns.Contains(prefix + "type") ? Converter.ToString(row[prefix + "type"]) : null;
            period.Value = row.Table.Columns.Contains(prefix + "value") ? Converter.ToNullableInt32(row[prefix + "value"]) : null;
            period.Year = row.Table.Columns.Contains(prefix + "year") ? Converter.ToNullableInt32(row[prefix + "year"]) : null;

            if (period.isEmpty())
            {
                period = null;
            }

            return period;
        }
    }
}