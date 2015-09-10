/**
 * @api {delete} /v1/groups/:id Delete a group.
 * @apiVersion 1.0.0
 * @apiName DeleteGroup
 * @apiGroup Groups
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Delete a group.
 *
 * @apiExample Example:
 *     DELETE /groups/87 HTTP/1.1
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
