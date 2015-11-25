/**
 * @api {post} /v1/appointments Create an appointment.
 * @apiVersion 1.0.0
 * @apiName CreateAppointment
 * @apiGroup Appointments
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Create an appointment.
 *
 * @apiExample Example:
 *     POST /appointments HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "appointment": {
 *         "student": "201502142",
 *         "laboratory": "Laboratorio B",
 *         "software": "SF-35",
 *         "date": "2015-08-27T13:00:00.000Z",
 *         "group": 87
 *       },
 *       "access_token": "xxx.yyy.zzz"
 *     }
 *
 * @apiParam {Object}       appointment               The appointment data.
 * @apiParam {String}       appointment.student       The student username.
 * @apiParam {String}       appointment.laboratory    The laboratory name.
 * @apiParam {String}       appointment.software      The software code.
 * @apiParam {Number}       appointment.group         The group identity.
 * @apiParam {Date}         appointment.date          The date.
 * @apiParam {String}       access_token              The access token.
 *
 * @apiSuccess {Number}                                         id              The appointment identity.
 * @apiSuccess {Date}                                           date            The appointment number.
 * @apiSuccess {Boolean}                                        attendance      The student attendance.
 * @apiSuccess {Date}                                           created_at      The creation date.
 * @apiSuccess {Date}                                           updated_at      The last update date.
 * @apiSuccess {String="Por iniciar, Cancelada, Finalizada"}    state           The appointment state.
 * @apiSuccess {Object}                                         student         The student data.
 * @apiSuccess {Object}                                         laboratory      The laboratory data.
 * @apiSuccess {Object}                                         software        The software data.
 * @apiSuccess {Object}                                         group           The group data.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "id": 1,
 *       "state": "Por iniciar",
 *       "attendance": false,
 *       "date": "2015-08-27T13:00:00.000Z",
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
 *       },
 *       "group": {
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
