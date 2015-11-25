/**
 * @api {get} /v1/users/:username Retrieve an user.
 * @apiVersion 1.0.0
 * @apiName GetUser
 * @apiGroup Users
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Retrieve an user.
 *
 * @apiExample Example:
 *   GET /users/gjacksonhi?access_token=xxx.yyy.zzz HTTP/1.1
 *
 * @apiParam {String}       access_token    The access token.
 * @apiParam {String[]}     [fields]        A list of fields to include in the results.
 *
 * @apiSuccess {Number}                                                 id              The user identity.
 * @apiSuccess {String}                                                 name            The first name.
 * @apiSuccess {String}                                                 last_name_1     The first last name.
 * @apiSuccess {String}                                                 last_name_2     The second last name.
 * @apiSuccess {String}                                                 full_name       The full name.
 * @apiSuccess {String}                                                 username        The username.
 * @apiSuccess {String="Masculino, Femenino"}                           gender          The gender.
 * @apiSuccess {String}                                                 email           The email address.
 * @apiSuccess {String}                                                 phone           The phone number.
 * @apiSuccess {Date}                                                   created_at      The creation date.
 * @apiSuccess {Date}                                                   updated_at      The last update date.
 * @apiSuccess {String="Activo, Inactivo, Bloqueado"}                   state           The user state.
 * @apiSuccess {String="Estudiante, Docente, Operador, Administrador"}  type            The user type.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "created_at": "2015-08-27T22:14:20.646Z",
 *       "email": "gjacksonhi@squidoo.com",
 *       "gender": "Masculino",
 *       "id": 631,
 *       "last_name_1": "Lynch",
 *       "last_name_2": "Jackson",
 *       "name": "Gregory",
 *       "full_name": "Gregory Lynch Jackson",
 *       "phone": "7-(384)880-7491",
 *       "state": "Activo",
 *       "updated_at": "2015-08-27T22:14:20.646Z",
 *       "username": "gjacksonhi",
 *       "type": "Estudiante"
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
