/**
 * @api {delete} /v1/laboratories/:id Delete a laboratory.
 * @apiVersion 1.0.0
 * @apiName DeleteLaboratory
 * @apiGroup Laboratory
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Delete a laboratory.
 *
 * @apiExample Example:
 *     DELETE /laboratories/87 HTTP/1.1
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
