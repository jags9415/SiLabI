using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using System.Reflection;
using System.ServiceModel.Activation;
using SiLabI.Model;
using SiLabI.Controllers;
using System.Net;
using SiLabI.Model.Query;

namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        private AuthenticationController _AuthController;
        private UserController _UserController;
        private AdministratorController _AdminController;
        private StudentController _StudentController;
        private ProfessorController _ProfessorController;
        private OperatorController _OperatorController;
        private CourseController _CourseController;
        private GroupController _GroupController;
        private SoftwareController _SoftwareController;
        private LaboratoryController _LaboratoryController;
        private AppointmentController _AppointmentController;
        private StudentAppointmentController _StudentAppointmentController;
        private ReservationController _ReservationController;
        private ProfessorReservationController _ProfessorReservationController;

        /// <summary>
        /// Create a new Service.
        /// </summary>
        public Service()
        {
            this._AuthController = new AuthenticationController();
            this._UserController = new UserController();
            this._AdminController = new AdministratorController();
            this._StudentController = new StudentController();
            this._ProfessorController = new ProfessorController();
            this._OperatorController = new OperatorController();
            this._CourseController = new CourseController();
            this._GroupController = new GroupController();
            this._SoftwareController = new SoftwareController();
            this._LaboratoryController = new LaboratoryController();
            this._AppointmentController = new AppointmentController();
            this._StudentAppointmentController = new StudentAppointmentController();
            this._ReservationController = new ReservationController();
            this._ProfessorReservationController = new ProfessorReservationController();
        }
    }
}
