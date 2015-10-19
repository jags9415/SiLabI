/**
 * @api {get} /v1/students/:username/appointments/:id/available Retrieve the available appointment update dates.
 * @apiVersion 1.0.0
 * @apiName GetAvailableAppointmentsForUpdate
 * @apiGroup Appointments->User
 * @apiPermission student
 * @apiUse BaseError
 *
 * @apiDescription Retrieve the available appointment update dates.
 *
 * @apiExample Example:
 * 		GET /students/201242273/appointments/34/available HTTP/1.1
 *
 * @apiParam {String}	access_token	 The access token.
 * @apiParam {String[]} [q]           A query to filter the results.
 * @apiParam {String[]} [fields]      A list of fields to include in the results.
 * @apiParam {String[]} [sort]        A list of fields to sort the results.
 *
 * @apiSuccess {Date}						date 		      The appointment date.
 * @apiSuccess {Object}         laboratory		The laboratory data.
 * @apiSuccess {Number}         spaces  		  The available spaces in the laboratory.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     [
 *       {
 *         "date": "2015-10-04T08:00:00.000",
 *         "laboratory": {
 *           "created_at": "2015-09-23T07:35:33.667",
 *           "id": 2,
 *           "state": "Activo",
 *           "updated_at": "2015-09-23T07:35:33.667",
 *           "appointment_priority": 1,
 *           "name": "Laboratorio B",
 *           "reservation_priority": 2,
 *           "seats": 20
 *         },
 *         "spaces": 20
 *       },
 *       {
 *         "date": "2015-10-04T09:00:00.000",
 *         "laboratory": {
 *           "created_at": "2015-09-23T07:35:33.667",
 *           "id": 2,
 *           "state": "Activo",
 *           "updated_at": "2015-09-23T07:35:33.667",
 *           "appointment_priority": 1,
 *           "name": "Laboratorio B",
 *           "reservation_priority": 2,
 *           "seats": 20
 *         },
 *         "spaces": 20
 *       }
 *     ]
 *
 * @apiErrorExample {json} Error-Response:
 *     HTTP/1.1 401 Unauthorized
 *     {
 *       "code": 401,
 *       "error": "Unauthorized",
 *       "description": "You don't have permissions to perform this operation."
 *     }
 */
