/**
 * @api {delete} /v1/students/:username/appointments/:id Delete an appointment.
 * @apiVersion 1.0.0
 * @apiName DeleteAppointment
 * @apiGroup Appointments->User
 * @apiPermission student
 * @apiUse BaseError
 *
 * @apiDescription Delete an appointment.
 *
 * @apiExample Example:
 *     DELETE /students/201242273/appointments/83 HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "access_token": "..."
 *     }
 *
 * @apiParam {String}	access_token	The access token.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *
 * @apiErrorExample {json} Error-Response:
 *     HTTP/1.1 401 Unauthorized
 *     {
 *       "code": 401,
 *       "error": "Unauthorized",
 *       "description": "You don't have permissions to perform this operation."
 *     }
 */
