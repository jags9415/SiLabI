/**
 * @api {put} /v1/students/:username/appointments/:id Update an appointment.
 * @apiVersion 1.0.0
 * @apiName UpdateAppointment
 * @apiGroup Appointments->User
 * @apiPermission student
 * @apiUse BaseError
 *
 * @apiDescription Update an appointment.
 *
 * @apiExample Example:
 *     PUT /students/201242273/appointments/87 HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "appointment": {
 *         "software": "SF-01",
 *         "date": "2015-08-27T15:00:00.000Z"
 *       },
 *       "access_token": "..."
 *     }
 *
 * @apiParam {Object}						appointment 		      The appointment data.
 * @apiParam {String}						[appointment.software] The software code.
 * @apiParam {Date}					    [appointment.date]  The date.
 * @apiParam {String}						access_token    The access token.
 *
 * @apiSuccess {Number}									    id 		      The appointment identification.
 * @apiSuccess {Date}									    date 		    The appointment number.
 * @apiSuccess {Date}									      created_at  The creation date.
 * @apiSuccess {Date}									      updated_at  The last update date.
 * @apiSuccess {String="Por iniciar, Cancelada, Finalizada"}	state			  The appointment state.
 * @apiSuccess {Object}									    student 			The student data.
 * @apiSuccess {Object}                     laboratory		The laboratory data.
 * @apiSuccess {Object}                     software  		The software data.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "id": 87,
 *       "state": "Por iniciar",
 *       "date": 2015-08-27T15:00:00.000Z,
 *       "created_at": "2015-08-27T22:14:20.646Z",
 *       "updated_at": "2015-08-27T22:14:20.646Z",
 *       "student": {
 *         "created_at": "2015-08-27T22:14:20.646Z",
 *         "email": "gjacksonhi@squidoo.com",
 *         "gender": "Masculino",
 *         "id": 54,
 *         "last_name_1": "Lynch",
 *         "last_name_2": "Jackson",
 *         "name": "Gregory",
 *         "full_name": "Gregory Lynch Jackson",
 *         "phone": "7-(384)880-7491",
 *         "state": "Activo",
 *         "updated_at": "2015-08-27T22:14:20.646Z",
 *         "username": "201242273"
 *       },
 *       "laboratory": {
 *         "id": 1136,
 *         "name": "Laboratorio B1",
 *         "seats": 20,
 *         "state": "Activo",
 *         "created_at": "2015-08-27T22:14:20.646Z",
 *         "updated_at": "2015-08-27T22:14:20.646Z"
 *       },
 *       "software": {
 *         "id": 1136,
 *         "name": "Software #12",
 *         "code": "SF-12",
 *         "state": "Activo",
 *         "created_at": "2015-08-27T22:14:20.646Z",
 *         "updated_at": "2015-08-27T22:14:20.646Z"
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
