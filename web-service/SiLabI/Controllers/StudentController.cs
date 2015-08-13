using SiLabI.Model;
using System;
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
        [DataMember(EmitDefaultValue = false)]
        public Student student { get; set; }
    }
}

namespace SiLabI.Controllers
{
    public class StudentController
    {
        private StudentDataAccess dao;

        public StudentController()
        {
            this.dao = new StudentDataAccess();
        }

        public List<Student> GetStudents(QueryString request)
        {
            Token.CheckToken(request.AccessToken, UserType.Operator);
            return dao.GetStudents(request);
        }

        public Student GetStudent(int id, string token)
        {
            Token.CheckToken(token, UserType.Operator);
            return dao.GetStudent(id);
        }

        public Student CreateStudent(StudentRequest request)
        {
            Token.CheckToken(request.AccessToken, UserType.Operator);
            return dao.CreateStudent(request.student);
        }

        public Student UpdateStudent(int id, StudentRequest request)
        {
            Token.CheckToken(request.AccessToken, UserType.Operator);
            return dao.UpdateStudent(id, request.student);
        }

        public void DeleteStudent(int id, BaseRequest request)
        {
            Token.CheckToken(request.AccessToken, UserType.Operator);
            dao.DeleteStudent(id);
        }
    }
}