/**
 * @api {put} /me Update the user profile.
 * @apiVersion 1.0.0
 * @apiName UpdateProfile
 * @apiGroup Profile
 * @apiPermission any
 * @apiUse BaseError
 *
 * @apiDescription Update the user profile.
 *
 * @apiExample Example:
 *     PUT /me HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "user": {
 *         "email": "ataylor4a@moonfruit.com",
 *         "phone": "83492144"
 *       },
 *       "access_token":"..."
 *     }
 *
 * @apiParam {Object}							             	user 			      	The user data.
 * @apiParam {String}							            	[user.password]		The password.
 * @apiParam {String="Masculino, Femenino"}			[user.gender]		  The gender.
 * @apiParam {String}								            [user.email]			The email address. [To delete this field send an empty string]
 * @apiParam {String}							             	[user.phone]			The phone number. [To delete this field send an empty string]
 * @apiParam {String}							            	access_token			The access token.
 *
 * @apiSuccess {Number}																			 		  				id 				    The user identification.
 * @apiSuccess {String}																			 		  				name 			    The first name.
 * @apiSuccess {String}																			 		  				last_name_1		The first last name.
 * @apiSuccess {String}																			 		  				last_name_2		The second last name.
 * @apiSuccess {String}																			 			  			full_name		  The full name.
 * @apiSuccess {String}																			 			  			username		  The username.
 * @apiSuccess {String="Masculino, Femenino"}									 			  	  gender			  The gender.
 * @apiSuccess {String}																				   					email		    	The email address.
 * @apiSuccess {String}																				 	  				phone			    The phone number.
 * @apiSuccess {Date}																			  			 			  created_at		The creation date.
 * @apiSuccess {Date}														  								 			  updated_at		The last update date.
 * @apiSuccess {String="Activo, Inactivo, Bloqueado"}						          state		    	The user state.
 * @apiSuccess {String="Estudiante, Docente, Operador, Administrador"} 		type 		    	The user type.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "created_at": "2015-08-27T22:14:20.646Z",
 *       "email": "ataylor4a@moonfruit.com",
 *       "gender": "Masculino",
 *       "id": 1136,
 *       "last_name_1": "Lewis",
 *       "last_name_2": "Taylor",
 *       "name": "Arthur",
 *       "full_name": "Arthur Lewis Taylor",
 *       "phone": "83492144",
 *       "state": "Activo",
 *       "updated_at": "2015-08-27T22:14:20.646Z",
 *       "username": "201242273"
 *     }
 *
 * @apiErrorExample {json} Error-Response:
 *     HTTP/1.1 401 Unauthorized
 *     {
 *       "code": 401,
 *       "error": "Unauthorized",
 *       "description": "You don't have permissions to perform this operation."
 *     }
 */
