using SiLabI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI
{
    public class StudentDataAccess
    {
        public List<Student> GetStudents(QueryString request)
        {
            return new List<Student>();
        }

        public Student GetStudent(int id)
        {
            return new Student();
        }

        public Student CreateStudent(Student student)
        {
            return new Student();
        }

        public Student UpdateStudent(int id, Student student)
        {
            return new Student();
        }

        public void DeleteStudent(int id)
        {
        }
    }
}