using SiLabI.Model;
using SiLabI.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;

namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    public partial class Service
    {
        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            return _AuthController.Authenticate(request);
        }
    }
}
