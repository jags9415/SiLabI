/**
 * @api {delete} /v1/courses/:id Delete a course.
 * @apiVersion 1.0.0
 * @apiName DeleteCourse
 * @apiGroup Courses
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Delete a course.
 *
 * @apiExample Example:
 *     DELETE /courses/87 HTTP/1.1
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
