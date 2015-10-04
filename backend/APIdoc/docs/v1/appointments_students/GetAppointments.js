/**
 * @api {get} /v1/students/:username/appointments Retrieve a list of appointments.
 * @apiVersion 1.0.0
 * @apiName GetAppointments
 * @apiGroup Appointments->User
 * @apiPermission student
 * @apiUse BaseError
 *
 * @apiDescription Retrieve a list of appointments.
 *
 * @apiParamExample Example query:
 * 		GET /students/201242273/appointments?q=created_at+gt+2015-03-12T00:00:00 HTTP/1.1
 *		Retrieve all the appointments that was created after 2015/03/12
 *
 *		Valid Operations: eq, ne, gt, ge, lt, le, like.
 *		The character '*' is a wildcard for the like operation.
 *
 * @apiParamExample Example fields:
 *      GET /students/201242273/appointments?fields=date HTTP/1.1
 *      Retrieves only the date.
 *
 * @apiParamExample Example sort:
 *      GET /students/201242273/appointments?sort=id HTTP/1.1
 *      Order the results by ascending id.
 *
 * @apiParamExample Full example:
 *      GET /students/201242273/appointments?q=created_at+gt+2015-03-12T00:00:00&fields=date&sort=id HTTP/1.1
 *
 * @apiParam {String}   access_token	The access token.
 * @apiParam {String[]} [q]           A query to filter the results.
 * @apiParam {String[]} [fields]      A list of fields to include in the results.
 * @apiParam {String[]} [sort]        A list of fields to sort the results.
 * @apiParam {Number}   [page=1]      The page number.
 * @apiParam {Number}   [limit=20]    The amount of results by page.
 *
 * @apiSuccess {Number}							        total_pages			        The total amount of pages for this query.
 * @apiSuccess {Number}	  								  current_page	          The current page number.
 * @apiSuccess {Object[]} 								  results 				        The list of appointments.
 * @apiSuccess {Number}									    results.id 		      The appointment identification.
 * @apiSuccess {Date}									      results.date 		    The appointment number.
 * @apiSuccess {Date}									      results.created_at  The creation date.
 * @apiSuccess {Date}									      results.updated_at  The last update date.
 * @apiSuccess {String="Por iniciar, Cancelada, Finalizada"}	results.state			  The appointment state.
 * @apiSuccess {Object}									    results.student 			The student data.
 * @apiSuccess {Object}                     results.laboratory		The laboratory data.
 * @apiSuccess {Object}                     results.software  		The software data.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "total_pages": 1,
 *       "current_page", 1,
 *       "results": [
 *         {
 *           "id": 87,
 *           "state": "Por iniciar",
 *           "date": "2015-08-27T13:00:00.000Z",
 *           "created_at": "2015-08-27T22:14:20.646Z",
 *           "updated_at": "2015-08-27T22:14:20.646Z",
 *           "student": {
 *            "created_at": "2015-08-27T22:14:20.646Z",
 *             "email": "gjacksonhi@squidoo.com",
 *             "gender": "Masculino",
 *             "id": 54,
 *             "last_name_1": "Lynch",
 *             "last_name_2": "Jackson",
 *             "name": "Gregory",
 *             "full_name": "Gregory Lynch Jackson",
 *             "phone": "7-(384)880-7491",
 *             "state": "Activo",
 *             "updated_at": "2015-08-27T22:14:20.646Z",
 *             "username": "201242273"
 *           },
 *           "laboratory": {
 *             "id": 1136,
 *             "name": "Laboratorio B1",
 *             "seats": 20,
 *             "state": "Activo",
 *             "created_at": "2015-08-27T22:14:20.646Z",
 *             "updated_at": "2015-08-27T22:14:20.646Z"
 *           },
 *           "software": {
 *             "id": 1136,
 *             "name": "Software #12",
 *             "code": "SF-12",
 *             "state": "Activo",
 *             "created_at": "2015-08-27T22:14:20.646Z",
 *             "updated_at": "2015-08-27T22:14:20.646Z"
 *           }
 *         }
 *       ]
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
