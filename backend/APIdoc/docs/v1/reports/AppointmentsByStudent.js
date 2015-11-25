/**
 * @api {get} /v1/reports/appointments/students/:username Get the appointments report of a student.
 * @apiVersion 1.0.0
 * @apiName GetAppointmentsByStudentReport
 * @apiGroup Reports
 * @apiPermission administrator
 * @apiUse BaseError
 *
 * @apiDescription Get the appointments report of a student.
 *
 * @apiExample Example:
 *   GET /reports/appointments/students/201242273?access_token=xxx.yyy.zzz HTTP/1.1
 *
 * @apiParam {String}   access_token    The access token.
 *
 * @apiSuccess {Byte[]}    bytes   The report in PDF format.
 *
 * @apiErrorExample {json} Error-Response:
 *     HTTP/1.1 401 Unauthorized
 *     {
 *       "code": 401,
 *       "error": "Unauthorized",
 *       "description": "You don't have permissions to perform this operation."
 *     }
 */
