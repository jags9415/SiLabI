/**
 * @api {get} /v1/reports/reservations/professors/:username Get the reservations report of a professor.
 * @apiVersion 1.0.0
 * @apiName GetReservationsByProfessorReport
 * @apiGroup Reports
 * @apiPermission administrator
 * @apiUse BaseError
 *
 * @apiDescription Get the reservations report of a professor.
 *
 * @apiExample Example:
 *   GET /reports/reservations/professors/emarin?access_token=xxx.yyy.zzz HTTP/1.1
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
