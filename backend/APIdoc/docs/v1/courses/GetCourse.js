/**
 * @api {get} /v1/courses/:id Retrieve a course.
 * @apiVersion 1.0.0
 * @apiName GetCourse
 * @apiGroup Courses
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Retrieve a course.
 *
 * @apiExample Example:
 * 		GET /courses/87
 *
 * @apiParam {String}	access_token	The access token.
 *
 * @apiSuccess {Number}									    id 		      The course identification.
 * @apiSuccess {String}									    name 			  The course name.
 * @apiSuccess {String}                     code		    The course code.
 * @apiSuccess {Date}									      created_at  The creation date.
 * @apiSuccess {Date}									      updated_at  The last update date.
 * @apiSuccess {String="Activo, Inactivo"}	state			  The course state.
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
