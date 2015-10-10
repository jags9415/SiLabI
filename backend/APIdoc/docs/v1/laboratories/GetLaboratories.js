/**
 * @api {get} /v1/laboratories Retrieve a list of laboratories.
 * @apiVersion 1.0.0
 * @apiName GetLaboratories
 * @apiGroup Laboratory
 * @apiPermission any
 * @apiUse BaseError
 *
 * @apiDescription Retrieve a list of laboratories.
 *
 * @apiParamExample Example query:
 * 		GET /laboratories?q=created_at+gt+2015-03-12T00:00:00 HTTP/1.1
 *		Retrieve all the laboratories that was created after 2015/03/12
 *
 *		Valid Operations: eq, ne, gt, ge, lt, le, like.
 *		The character '*' is a wildcard for the like operation.
 *
 * @apiParamExample Example fields:
 *      GET /laboratories?fields=seats,name HTTP/1.1
 *      Retrieves only the seats and name fields.
 *
 * @apiParamExample Example sort:
 *      GET /laboratories?sort=id HTTP/1.1
 *      Order the results by ascending id.
 *
 * @apiParamExample Full example:
 *      GET /laboratories?q=created_at+gt+2015-03-12T00:00:00&fields=seats,name&sort=id HTTP/1.1
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
 * @apiSuccess {Object[]} 								  results 				        The list of laboratories.
 * @apiSuccess {Number}									    results.id 		          The laboratory identification.
 * @apiSuccess {String}									    results.name 			      The laboratory name.
 * @apiSuccess {Number}                     results.seats		        The laboratory available seats.
 * @apiSuccess {Date}									      results.created_at      The creation date.
 * @apiSuccess {Date}									      results.updated_at      The last update date.
 * @apiSuccess {String="Activo, Inactivo"}	results.state			      The laboratory state.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "total_pages": 1,
 *       "current_page", 1,
 *       "results": [
 *         {
 *           "id": 1,
 *           "name": "Laboratorio B1",
 *           "seats": 20,
 *           "state": "Activo",
 *           "created_at": "2015-08-27T22:14:20.646Z",
 *           "updated_at": "2015-08-27T22:14:20.646Z"
 *         },
 *         {
 *           "id": 2,
 *           "name": "Laboratorio A1",
 *           "seats": 35,
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
