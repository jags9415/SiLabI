/**
 * @api {put} /v1/groups/:id/students Update the group students.
 * @apiVersion 1.0.0
 * @apiName UpdateGroupStudents
 * @apiGroup Groups_Students
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Update the group students.
 *
 * @apiExample Example:
 *     PUT /groups/45/students HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "students": ["201242273", "201390652", "201503214"],
 *       "access_token": "xxx.yyy.zzz"
 *     }
 *
 * @apiParam {String[]}     students        The students list.
 * @apiParam {String}       access_token    The access token.
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
