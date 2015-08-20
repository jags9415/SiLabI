/**
 * @api {post} /v1/administrators/:id Create an administrator.
 * @apiVersion 1.0.0
 * @apiName CreateAdministrator
 * @apiGroup Administrators
 * @apiPermission administrator
 * @apiUse BaseError
 *
 * @apiDescription Create an administrator.
 *
 * @apiExample Example:
 *     POST /administrators/87
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
