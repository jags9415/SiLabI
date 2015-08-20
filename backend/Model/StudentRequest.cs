﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Web;

namespace SiLabI.Model
{
    [DataContract]
    public class StudentRequest : BaseRequest
    {
        protected Student _student;

        [DataMember(Name="student")]
        public Student Student
        {
            set
            {
                if (value == null)
                {
                    ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "MissingParameter", "El parámetro 'student' es obligatorio.");
                    throw new WebFaultException<ErrorResponse>(error, error.Code);
                }
                _student = value;
            }
            get { return _student; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return base.IsValid() && Student != null;
        }
    }
}