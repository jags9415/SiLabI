/**
 * @api {put} /v1/courses/:id Update a course.
 * @apiVersion 1.0.0
 * @apiName UpdateCourse
 * @apiGroup Courses
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Update a course.
 *
 * @apiExample Example:
 *     PUT /courses/34 HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "course": {
 *         "code": "CI-1313",
 *         "name": "Inglés III Para Computación",
 *         "state": "Inactivo"
 *       },
 *       "access_token": "xxx.yyy.zzz"
 *     }
 *
 * @apiParam {Object}                       course           The course data.
 * @apiParam {String}                       [course.code]    The course code.
 * @apiParam {String}                       [course.name]    The course name.
 * @apiParam {String="Activo, Inactivo"}    [course.state]   The course state.
 * @apiParam {String}                       access_token     The access token.
 *
 * @apiSuccess {Number}                     id         The course identity.
 * @apiSuccess {String}                     name        The course name.
 * @apiSuccess {String}                     code        The course code.
 * @apiSuccess {Date}                       created_at  The creation date.
 * @apiSuccess {Date}                       updated_at  The last update date.
 * @apiSuccess {String="Activo, Inactivo"}  state       The course state.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "id": 34,
 *       "name": "Inglés III Para Computación",
 *       "code": "CI-1313",
 *       "state": "Inactivo",
 *       "created_at": "2015-08-27T22:14:20.646Z",
 *       "updated_at": "2015-08-27T22:14:20.646Z"
 *     }
 *
 * @apiErrorExample {json} Error-Response:
 *     HTTP/1.1 401 Unauthorized
 *     {
 *       "code": 401,
 *       "error": "Unauthorized",
 *       "description": "You don't have permissions to perform this operation."
 *     }
 */
