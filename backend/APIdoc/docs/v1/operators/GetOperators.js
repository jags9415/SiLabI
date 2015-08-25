/**
 * @api {get} /v1/operators Retrieve a list of operators.
 * @apiVersion 1.0.0
 * @apiName GetOperators
 * @apiGroup Operators
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Retrieve a list of operators.
 *
 * @apiParamExample Example query:
 * 		/operators?q=period.year+eq+2015
 *		Retrieve all the operators of the year 2015.
 *
 *		Valid Operations: eq, ne, gt, ge, lt, le, like.
 *		The character '*' is a wildcard for the like operation.
 *
 * @apiParamExample Example fields:
 *      /operators?fields=id,name,email
 *      Retrieves only the id, name and email fields.
 *
 * @apiParamExample Example sort:
 *      /operators?sort=name,-email
 *      Order the results by ascending name and then by descending email.
 *
 * @apiParamExample Full example:
 *      /operators?q=period.year+eq+2015&fields=id,name,email&sort=name,-email
 *
 * @apiParam {String}	access_token	The access token.
 * @apiParam {String[]} [q]				A query to filter the results.
 * @apiParam {String[]} [fields]		A list of fields to include in the results.
 * @apiParam {String[]} [sort]			A list of fields to sort the results.
 * @apiParam {Number}   [page=1]		The page number.
 * @apiParam {Number}   [limit=20]		The amount of results by page.
 *
 * @apiSuccess {Number}													total_pages				The total amount of pages for this query.
 * @apiSuccess {Number}					  								current_page			The current page number.
 * @apiSuccess {Object[]} 												results 				The list of operators.
 * @apiSuccess {Number}													results.id 				The user identification.
 * @apiSuccess {String}													results.name 			The first name.
 * @apiSuccess {String}													results.last_name_1		The first last name.
 * @apiSuccess {String}													results.last_name_2		The second last name.
 * @apiSuccess {String}													results.full_name		The full name.
 * @apiSuccess {String}													results.username		The username.
 * @apiSuccess {String="Masculino, Femenino"}							results.gender			The gender.
 * @apiSuccess {String}													results.email			The email address.
 * @apiSuccess {String}													results.phone			The phone number.
 * @apiSuccess {Date}													results.created_at		The creation date.
 * @apiSuccess {Date}													results.updated_at		The last update date.
 * @apiSuccess {String="Activo, Inactivo, Bloqueado"}					results.state			The user state.
 * @apiSuccess {Object}													results.period 			The period in which the student is operating.
 * @apiSuccess {Number}													results.period.value	The period value.
 * @apiSuccess {String="Bimestre, Trimestre, Cuatrimestre, Semestre"}	results.period.type 	The period type.
 * @apiSuccess {Number}													results.period.year		The period year.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "total_pages": 23,
 *       "current_page", 1,
 *       "results": [
 *           {
 *              "created_at": "/Date(1439325374913-0600)/",
 *              "email": "gjacksonhi@squidoo.com",
 *              "gender": "Masculino",
 *              "id": 631,
 *              "last_name_1": "Lynch",
 *              "last_name_2": "Jackson",
 *              "name": "Gregory",
 *              "full_name": "Gregory Lynch Jackson",
 *              "phone": "7-(384)880-7491",
 *              "state": "Activo",
 *              "updated_at": "/Date(1439325374913-0600)/",
 *              "username": "gjacksonhi",
 *              "period": {
 *                "value": 2,
 *                "type": "Semestre",
 *                "year": 2015
 *              }
 *           },
 *           {
 *              "created_at": "/Date(1439325374913-0600)/",
 *              "email": "lturnerel@wordpress.org",
 *              "gender": "Femenino",
 *              "id": 526,
 *              "last_name_1": "George",
 *              "last_name_2": "Turner",
 *              "full_name": "Lori George Turner",
 *              "name": "Lori",
 *              "phone": "8-(226)006-5947",
 *              "state": "Activo",
 *              "updated_at": "/Date(1439325374913-0600)/",
 *              "username": "lturnerel",
 *              "period": {
 *                "value": 1,
 *                "type": "Trimestre",
 *                "year": 2015
 *              }
 *           }
 *        ]
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
