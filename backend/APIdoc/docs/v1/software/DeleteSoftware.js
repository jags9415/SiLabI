/**
 * @api {delete} /v1/software/:id Delete a software.
 * @apiVersion 1.0.0
 * @apiName DeleteSoftware
 * @apiGroup Software
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Delete a software.
 *
 * @apiExample Example:
 *     DELETE /software/87 HTTP/1.1
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
