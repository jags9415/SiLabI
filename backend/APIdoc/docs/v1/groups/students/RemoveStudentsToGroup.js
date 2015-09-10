/**
 * @api {delete} /v1/groups/:id/students Remove students from a group.
 * @apiVersion 1.0.0
 * @apiName RemoveStudentsFromGroup
 * @apiGroup Groups/Students
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Remove students from a group.
 *
 * @apiExample Example:
 *     POST /groups/45/students HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "students": ["201242273", "201390652", "201503214"],
 *       "access_token": "..."
 *     }
 *
 * @apiParam {Object[]}				  students  The students list.
 * @apiParam {String}						access_token    The access token.
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
