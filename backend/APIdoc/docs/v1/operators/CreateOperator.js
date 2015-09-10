/**
 * @api {post} /v1/operators/:id Create an operator.
 * @apiVersion 1.0.0
 * @apiName CreateOperator
 * @apiGroup Operators
 * @apiPermission administrator
 * @apiUse BaseError
 *
 * @apiDescription Create an operator.
 *
 * @apiExample Example:
 *     POST /operators/87 HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "period": {
 *         "value": 1,
 *         "type": "Semestre",
 *         "year": 2015
 *       },
 *       "access_token": "..."
 *     }
 *
 * @apiParam {String}												access_token	The access token.
 * @apiParam {Object}												period 			The period in which the student will be operating.
 * @apiParam {Number}												period.value	The period value.
 * @apiParam {String="Bimestre, Trimestre, Cuatrimestre, Semestre"}	period.type 	The period type.
 * @apiParam {Number}												period.year		The period year.
 *
 * @apiSuccess {Number}													id 				The user identification.
 * @apiSuccess {String}													name 			The first name.
 * @apiSuccess {String}													last_name_1		The first last name.
 * @apiSuccess {String}													last_name_2		The second last name.
 * @apiSuccess {String}													full_name		The full name.
 * @apiSuccess {String}													username		The username.
 * @apiSuccess {String="Masculino, Femenino"}							gender			The gender.
 * @apiSuccess {String}													email			The email address.
 * @apiSuccess {String}													phone			The phone number.
 * @apiSuccess {Date}													created_at		The creation date.
 * @apiSuccess {Date}													updated_at		The last update date.
 * @apiSuccess {String="Activo, Inactivo, Bloqueado"}					state			The user state.
 * @apiSuccess {Object}													period 			The period in which the student is operating.
 * @apiSuccess {Number}													period.value	The period value.
 * @apiSuccess {String="Bimestre, Trimestre, Cuatrimestre, Semestre"}	period.type 	The period type.
 * @apiSuccess {Number}													period.year		The period year.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "created_at": "2015-08-27T22:14:20.646Z",
 *       "email": "gjacksonhi@squidoo.com",
 *       "gender": "Masculino",
 *       "id": 54,
 *       "last_name_1": "Lynch",
 *       "last_name_2": "Jackson",
 *       "name": "Gregory",
 *       "full_name": "Gregory Lynch Jackson",
 *       "phone": "7-(384)880-7491",
 *       "state": "Activo",
 *       "updated_at": "2015-08-27T22:14:20.646Z",
 *       "username": "gjacksonhi",
 *       "period": {
 *         "value": 2,
 *         "type": "Semestre",
 *         "year": 2015
 *       }
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
