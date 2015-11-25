/**
 * @api {put} /v1/laboratories/:id Update a laboratory.
 * @apiVersion 1.0.0
 * @apiName UpdateLaboratory
 * @apiGroup Laboratories
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Update a laboratory.
 *
 * @apiExample Example:
 *     PUT /laboratories/1136 HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "laboratory": {
 *         "seats": 25,
 *       },
 *       "access_token": "xxx.yyy.zzz"
 *     }
 *
 * @apiParam {Object}                       laboratory                          The laboratory data.
 * @apiParam {String}                       [laboratory.name]                   The laboratory name.
 * @apiParam {Number}                       [laboratory.seats]                  The laboratory available seats.
 * @apiParam {Number}                       [laboratory.appointment_priority]   The priority of appointments in the laboratory. (1: High, 2: Medium, 3: Low)
 * @apiParam {Number}                       [laboratory.reservation_priority]   The priority of reservations in the laboratory. (1: High, 2: Medium, 3: Low)
 * @apiParam {String="Activo, Inactivo"}    [laboratory.state]                  The laboratory state.
 * @apiParam {String[]}                     [laboratory.software]               The software list.
 * @apiParam {String}                       access_token                        The access token.
 *
 * @apiSuccess {Number}                     id                      The laboratory identity.
 * @apiSuccess {String}                     name                    The laboratory name.
 * @apiSuccess {Number}                     seats                   The laboratory available seats.
 * @apiSuccess {Number}                     appointment_priority    The priority of appointments in the laboratory. (1: High, 2: Medium, 3: Low)
 * @apiSuccess {Number}                     reservation_priority    The priority of reservations in the laboratory. (1: High, 2: Medium, 3: Low)
 * @apiSuccess {Date}                       created_at              The creation date.
 * @apiSuccess {Date}                       updated_at              The last update date.
 * @apiSuccess {String="Activo, Inactivo"}  state                   The laboratory state.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "id": 1136,
 *       "name": "Laboratorio B1",
 *       "seats": 25,
 *       "appointment_priority": 1,
 *       "reservation_priority": 3,
 *       "state": "Activo",
 *       "created_at": "2015-08-27T22:14:20.646Z",
 *       "updated_at": "2015-08-27T22:14:20.646Z"
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
