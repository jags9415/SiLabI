/**
 * @api {post} /v1/groups Create a group.
 * @apiVersion 1.0.0
 * @apiName CreateGroup
 * @apiGroup Groups
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Create a group.
 *
 * @apiExample Example:
 *     POST /groups HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "group": {
 *         "course": "CI-1312",
 *         "number": 40,
 *         "professor": "emarin",
 *         "students": ["201242273", "201390652", "201503214"],
 *         "period": {
 *           "value": 1,
 *           "type": "Semestre",
 *           "year": 2015
 *         }
 *       },
 *       "access_token": "..."
 *     }
 *
 * @apiParam {Object}						group 		      The group data.
 * @apiParam {String}						group.course    The course code.
 * @apiParam {Number}						group.number    The group number.
 * @apiParam {String}						group.professor The professor username.
 * @apiParam {Object[]}						[group.students]  The students list.
 * @apiParam {Object}						group.period    The period.
 * @apiParam {String}						access_token    The access token.
 *
 * @apiSuccess {Number}									    id 		      The group identification.
 * @apiSuccess {Number}									    number 		  The group number.
 * @apiSuccess {Object}									    course 			The course data.
 * @apiSuccess {Object}                     professor		The professor data.
 * @apiSuccess {Object}                     period  		The period data.
 * @apiSuccess {Date}									      created_at  The creation date.
 * @apiSuccess {Date}									      updated_at  The last update date.
 * @apiSuccess {String="Activo, Inactivo"}	state			  The group state.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "id": 1136,
 *       "number": 40,
 *       "course": {
 *         "id": 1136,
 *         "name": "Inglés II Para Computación",
 *         "code": "CI-1312",
 *         "state": "Activo",
 *         "created_at": "2015-08-27T22:14:20.646Z",
 *         "updated_at": "2015-08-27T22:14:20.646Z"
 *       },
 *       "professor": {
 *         "id": 54,
 *         "created_at": "2015-08-27T22:14:20.646Z",
 *         "email": "gjacksonhi@squidoo.com",
 *         "gender": "Masculino",
 *         "last_name_1": "Lynch",
 *         "last_name_2": "Jackson",
 *         "name": "Gregory",
 *         "full_name": "Gregory Lynch Jackson",
 *         "phone": "7-(384)880-7491",
 *         "state": "Activo",
 *         "updated_at": "2015-08-27T22:14:20.646Z",
 *         "username": "gjacksonhi"
 *       },
 *       "period": {
 *         "value": 1,
 *         "type": "Semestre",
 *         "year": 2015
 *       }
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
