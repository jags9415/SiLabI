/**
 * @api {post} /v1/professors/:username/reservations Create a reservation.
 * @apiVersion 1.0.0
 * @apiName CreateReservation
 * @apiGroup Reservations_Professors
 * @apiPermission professor
 * @apiUse BaseError
 *
 * @apiDescription Create a reservation.
 *
 * @apiExample Example:
 *     POST /professors/gjackson/reservations HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "reservation": {
 *         "laboratory": "Laboratorio A",
 *         "software": "SF-35",
 *         "group": 345,
 *         "start_time": "2015-08-27T13:00:00.000Z",
 *         "end_time": "2015-08-27T14:00:00.000Z",
 *       },
 *       "access_token": "xxx.yyy.zzz"
 *     }
 *
 * @apiParam {Object}    reservation                The reservation data.
 * @apiParam {Date}      reservation.start_time     The reservation start time.
 * @apiParam {Date}      reservation.end_time       The reservation end time.
 * @apiParam {String}    [reservation.laboratory]   The laboratory name.
 * @apiParam {Number}    [reservation.group]        The group identity number.
 * @apiParam {String}    [reservation.software]     The software code.
 * @apiParam {String}    access_token               The access token.
 *
 * @apiSuccess {Number}                                         id              The reservation identity.
 * @apiSuccess {Date}                                           start_time      The reservation start time.
 * @apiSuccess {Date}                                           end_time        The reservation end time.
 * @apiSuccess {Boolean}                                        attendance      The professor attendance.
 * @apiSuccess {Date}                                           created_at      The creation date.
 * @apiSuccess {Date}                                           updated_at      The last update date.
 * @apiSuccess {String="Por iniciar, Cancelada, Finalizada"}    state           The reservation state.
 * @apiSuccess {Object}                                         professor       The professor data.
 * @apiSuccess {Object}                                         laboratory      The laboratory data.
 * @apiSuccess {Object}                                         software        The software data.
 * @apiSuccess {Object}                                         group           The group data.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "id": 1,
 *       "state": "Por iniciar",
 *       "start_time": "2015-08-27T13:00:00.000Z",
 *       "end_time": "2015-08-27T14:00:00.000Z",
 *       "attendance": false,
 *       "created_at": "2015-08-27T22:14:20.646Z",
 *       "updated_at": "2015-08-27T22:14:20.646Z",
 *       "professor": {
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
 *         "username": "gjackson"
 *       },
 *       "laboratory": {
 *         "id": 1136,
 *         "name": "Laboratorio A",
 *         "seats": 20,
 *         "state": "Activo",
 *         "created_at": "2015-08-27T22:14:20.646Z",
 *         "updated_at": "2015-08-27T22:14:20.646Z"
 *       },
 *       "software": {
 *         "id": 1136,
 *         "name": "Software #35",
 *         "code": "SF-35",
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
 *           "username": "gjackson"
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
