/**
 * @api {get} /v1/courses Retrieve a list of courses.
 * @apiVersion 1.0.0
 * @apiName GetCourses
 * @apiGroup Courses
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Retrieve a list of courses.
 *
 * @apiParamExample Example query:
 * 		GET /courses?q=created_at+gt+2015-03-12T00:00:00 HTTP/1.1
 *		Retrieve all the courses that was created after 2015/03/12
 *
 *		Valid Operations: eq, ne, gt, ge, lt, le, like.
 *		The character '*' is a wildcard for the like operation.
 *
 * @apiParamExample Example fields:
 *      GET /courses?fields=code,name HTTP/1.1
 *      Retrieves only the code and name fields.
 *
 * @apiParamExample Example sort:
 *      GET /courses?sort=id HTTP/1.1
 *      Order the results by ascending id.
 *
 * @apiParamExample Full example:
 *      GET /courses?q=created_at+gt+2015-03-12T00:00:00&fields=code,name&sort=id HTTP/1.1
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
 * @apiSuccess {Object[]} 								  results 				        The list of courses.
 * @apiSuccess {Number}									    results.id 		          The course identification.
 * @apiSuccess {String}									    results.name 			      The course name.
 * @apiSuccess {String}                     results.code		        The course code.
 * @apiSuccess {Date}									      results.created_at      The creation date.
 * @apiSuccess {Date}									      results.updated_at      The last update date.
 * @apiSuccess {String="Activo, Inactivo"}	results.state			      The course state.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "total_pages": 1,
 *       "current_page", 1,
 *       "results": [
 *         {
 *           "id": 1136,
 *           "name": "Inglés I Para Computación",
 *           "code": "CI-1311",
 *           "state": "Activo",
 *           "created_at": "2015-08-27T22:14:20.646Z",
 *           "updated_at": "2015-08-27T22:14:20.646Z"
 *         },
 *         {
 *           "id": 1137,
 *           "name": "Inglés II Para Computación",
 *           "code": "CI-1312",
 *           "state": "Activo",
 *           "created_at": "2015-08-27T22:14:20.646Z",
 *           "updated_at": "2015-08-27T22:14:20.646Z"
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
