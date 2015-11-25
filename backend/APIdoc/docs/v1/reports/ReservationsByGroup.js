/**
 * @api {get} /v1/reports/reservations/groups/:id Get the reservations report of a group.
 * @apiVersion 1.0.0
 * @apiName GetReservationsByGroupReport
 * @apiGroup Reports
 * @apiPermission administrator
 * @apiUse BaseError
 *
 * @apiDescription Get the reservations report of a group.
 *
 * @apiExample Example:
 *   GET /reports/reservations/groups/312?access_token=xxx.yyy.zzz HTTP/1.1
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
