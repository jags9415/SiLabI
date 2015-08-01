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

namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service : IService
    {
        private AuthenticationController authController;

        public Service()
        {
            this.authController = new AuthenticationController();
        }

        public BaseResponse authenticate(AuthenticationRequest request)
        {
            return authController.authenticate(request);
        }
    }
}
