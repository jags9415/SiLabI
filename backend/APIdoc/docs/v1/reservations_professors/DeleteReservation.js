/**
 * @api {delete} /v1/professors/:username/reservations/:id Delete a reservation.
 * @apiVersion 1.0.0
 * @apiName DeleteReservation
 * @apiGroup Reservations_Professors
 * @apiPermission professor
 * @apiUse BaseError
 *
 * @apiDescription Delete a reservation.
 *
 * @apiExample Example:
 *     DELETE /professors/gjackson/reservations/87 HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "access_token": "xxx.yyy.zzz"
 *     }
 *
 * @apiParam {String}   access_token    The access token.
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
