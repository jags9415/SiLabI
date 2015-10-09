/**
 * @api {get} /v1/students/:username/groups Retrieve the group list of a student.
 * @apiVersion 1.0.0
 * @apiName GetStudentGroups
 * @apiGroup Students
 * @apiPermission student
 * @apiUse BaseError
 *
 * @apiDescription Retrieve the group list of a student.
 *
 * @apiParamExample Example query:
 * 		GET /students/201242273/groups?q=created_at+gt+2015-03-12T00:00:00 HTTP/1.1
 *		Retrieve all the groups that was created after 2015/03/12
 *
 *		Valid Operations: eq, ne, gt, ge, lt, le, like.
 *		The character '*' is a wildcard for the like operation.
 *
 * @apiParamExample Example fields:
 *      GET /students/201242273/groups?fields=number,course HTTP/1.1
 *      Retrieves only the number and course fields.
 *
 * @apiParamExample Example sort:
 *      GET /students/201242273/groups?sort=id HTTP/1.1
 *      Order the results by ascending id.
 *
 * @apiParamExample Full example:
 *      GET /students/201242273/groups?q=created_at+gt+2015-03-12T00:00:00&fields=number,course&sort=id HTTP/1.1
 *
 * @apiParam {String}   access_token	The access token.
 * @apiParam {String[]} [q]           A query to filter the results.
 * @apiParam {String[]} [fields]      A list of fields to include in the results.
 * @apiParam {String[]} [sort]        A list of fields to sort the results.
 *
 * @apiSuccess {Object[]} 								  results 				    The list of groups.
 * @apiSuccess {Number}									    results.id 		      The group identification.
 * @apiSuccess {Number}									    results.number 		  The group number.
 * @apiSuccess {Object}									    results.course 			The course data.
 * @apiSuccess {Object}                     results.professor		The professor data.
 * @apiSuccess {Object}                     results.period  		The period data.
 * @apiSuccess {Date}									      results.created_at  The creation date.
 * @apiSuccess {Date}									      results.updated_at  The last update date.
 * @apiSuccess {String="Activo, Inactivo"}	results.state			  The group state.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     [
 *       {
 *         "id": 1136,
 *         "number": 40,
 *         "course": {
 *           "id": 1136,
 *           "name": "Inglés II Para Computación",
 *           "code": "CI-1312",
 *           "state": "Activo",
 *           "created_at": "2015-08-27T22:14:20.646Z",
 *           "updated_at": "2015-08-27T22:14:20.646Z"
 *         },
 *         "professor": {
 *           "id": 54,
 *           "created_at": "2015-08-27T22:14:20.646Z",
 *           "email": "gjacksonhi@squidoo.com",
 *           "gender": "Masculino",
 *           "last_name_1": "Lynch",
 *           "last_name_2": "Jackson",
 *           "name": "Gregory",
 *           "full_name": "Gregory Lynch Jackson",
 *           "phone": "7-(384)880-7491",
 *           "state": "Activo",
 *           "updated_at": "2015-08-27T22:14:20.646Z",
 *           "username": "gjacksonhi"
 *         },
 *         "period": {
 *           "value": 1,
 *           "type": "Semestre",
 *           "year": 2015
 *         }
 *         "state": "Activo",
 *         "created_at": "2015-08-27T22:14:20.646Z",
 *         "updated_at": "2015-08-27T22:14:20.646Z"
 *       }
 *     ]
 *
 * @apiErrorExample {json} Error-Response:
 *     HTTP/1.1 401 Unauthorized
 *     {
 *       "code": 401,
 *       "error": "Unauthorized",
 *       "description": "You don't have permissions to perform this operation."
 *     }
 */
