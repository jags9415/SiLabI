/**
 * @api {post} /v1/professors Create a professor.
 * @apiVersion 1.0.0
 * @apiName CreateProfessor
 * @apiGroup Professors
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Create a professor.
 *
 * @apiExample Example:
 *     POST /professors HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "professor": {
 *         "email": "ataylor4a@moonfruit.com",
 *         "gender": "Masculino",
 *         "last_name_1": "Lewis",
 *         "last_name_2": "Taylor",
 *         "name": "Arthur",
 *         "phone": "83492144",
 *         "username": "ataylor",
 *         "password": "..."
 *       },
 *       "access_token": "xxx.yyy.zzz"
 *     }
 *
 * @apiParam {Object}                           professor                   The professor data.
 * @apiParam {String}                           professor.name              The first name.
 * @apiParam {String}                           professor.last_name_1       The first last name.
 * @apiParam {String}                           [professor.last_name_2]     The second last name.
 * @apiParam {String}                           professor.username          The username.
 * @apiParam {String}                           professor.password          The password.
 * @apiParam {String="Masculino, Femenino"}     professor.gender            The gender.
 * @apiParam {String}                           [professor.email]           The email address.
 * @apiParam {String}                           [professor.phone]           The phone number.
 * @apiParam {String}                           access_token                The access token.
 *
 * @apiSuccess {Number}                                 id              The user identity.
 * @apiSuccess {String}                                 name            The first name.
 * @apiSuccess {String}                                 last_name_1     The first last name.
 * @apiSuccess {String}                                 last_name_2     The second last name.
 * @apiSuccess {String}                                 full_name       The full name.
 * @apiSuccess {String}                                 username        The username.
 * @apiSuccess {String="Masculino, Femenino"}           gender          The gender.
 * @apiSuccess {String}                                 email           The email address.
 * @apiSuccess {String}                                 phone           The phone number.
 * @apiSuccess {Date}                                   created_at      The creation date.
 * @apiSuccess {Date}                                   updated_at      The last update date.
 * @apiSuccess {String="Activo, Inactivo, Bloqueado"}   state           The user state.
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
 *       "username": "ataylor"
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
