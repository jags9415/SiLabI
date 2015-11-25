/**
 * @api {get} /v1/courses/:id Retrieve a course.
 * @apiVersion 1.0.0
 * @apiName GetCourse
 * @apiGroup Courses
 * @apiPermission any
 * @apiUse BaseError
 *
 * @apiDescription Retrieve a course.
 *
 * @apiExample Example:
 *   GET /courses/87?access_token=xxx.yyy.zzz HTTP/1.1
 *
 * @apiParam {String}       access_token    The access token.
 * @apiParam {String[]}     [fields]        A list of fields to include in the results.
 *
 * @apiSuccess {Number}                     id          The course identity.
 * @apiSuccess {String}                     name        The course name.
 * @apiSuccess {String}                     code        The course code.
 * @apiSuccess {Date}                       created_at  The creation date.
 * @apiSuccess {Date}                       updated_at  The last update date.
 * @apiSuccess {String="Activo, Inactivo"}  state       The course state.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "id": 1136,
 *       "name": "Inglés II Para Computación",
 *       "code": "CI-1312",
 *       "state": "Activo",
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
