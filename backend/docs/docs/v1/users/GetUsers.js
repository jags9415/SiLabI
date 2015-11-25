/**
 * @api {get} /v1/users Retrieve a list of users.
 * @apiVersion 1.0.0
 * @apiName GetUsers
 * @apiGroup Users
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Retrieve a list of users.
 *
 * @apiParamExample Example:
 *      GET /users?access_token=xxx.yyy.zzz HTTP/1.1
 *
 * @apiParam {String}       access_token    The access token.
 * @apiParam {String[]}     [q]             A query to filter the results.
 * @apiParam {String[]}     [fields]        A list of fields to include in the results.
 * @apiParam {String[]}     [sort]          A list of fields to sort the results.
 * @apiParam {Number}       [page=1]        The page number.
 * @apiParam {Number}       [limit=20]      The amount of results by page.
 *
 * @apiSuccess {Number}                                                 total_pages             The total amount of pages for this query.
 * @apiSuccess {Number}                                                 current_page            The current page number.
 * @apiSuccess {Object[]}                                               results                 The list of administrators.
 * @apiSuccess {Number}                                                 results.id              The user identity.
 * @apiSuccess {String}                                                 results.name            The first name.
 * @apiSuccess {String}                                                 results.last_name_1     The first last name.
 * @apiSuccess {String}                                                 results.last_name_2     The second last name.
 * @apiSuccess {String}                                                 results.full_name       The full name.
 * @apiSuccess {String}                                                 results.username        The username.
 * @apiSuccess {String="Masculino, Femenino"}                           results.gender          The gender.
 * @apiSuccess {String}                                                 results.email           The email address.
 * @apiSuccess {String}                                                 results.phone           The phone number.
 * @apiSuccess {Date}                                                   results.created_at      The creation date.
 * @apiSuccess {Date}                                                   results.updated_at      The last update date.
 * @apiSuccess {String="Activo, Inactivo, Bloqueado"}                   results.state           The user state.
 * @apiSuccess {String="Estudiante, Docente, Operador, Administrador"}  results.type            The user type.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "total_pages": 23,
 *       "current_page", 1,
 *       "results": [
 *           {
 *              "created_at": "2015-08-27T22:14:20.646Z",
 *              "email": "gjacksonhi@squidoo.com",
 *              "gender": "Masculino",
 *              "id": 631,
 *              "last_name_1": "Lynch",
 *              "last_name_2": "Jackson",
 *              "name": "Gregory",
 *              "full_name": "Gregory Lynch Jackson",
 *              "phone": "7-(384)880-7491",
 *              "state": "Activo",
 *              "updated_at": "2015-08-27T22:14:20.646Z",
 *              "username": "gjacksonhi",
 *              "type": "Estudiante"
 *           },
 *           {
 *              "created_at": "2015-08-27T22:14:20.646Z",
 *              "email": "lturnerel@wordpress.org",
 *              "gender": "Femenino",
 *              "id": 526,
 *              "last_name_1": "George",
 *              "last_name_2": "Turner",
 *              "name": "Lori",
 *              "full_name": "Lori George Turner",
 *              "phone": "8-(226)006-5947",
 *              "state": "Activo",
 *              "updated_at": "2015-08-27T22:14:20.646Z",
 *              "username": "lturnerel",
 *              "type": "Docente"
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
