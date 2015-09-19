/**
 * @api {put} /v1/software/:id Update a software.
 * @apiVersion 1.0.0
 * @apiName UpdateSoftware
 * @apiGroup Software
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Update a software.
 *
 * @apiExample Example:
 *     PUT /software/34 HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "software": {
 *         "code": "SF-13",
 *         "name": "Software #13",
 *         "state": "Inactivo"
 *       },
 *       "access_token": "..."
 *     }
 *
 * @apiParam {Object}						             software 		      The software data.
 * @apiParam {String}						             [software.code]    The software code.
 * @apiParam {String}						             [software.name]    The software name.
 * @apiParam {String="Activo, Inactivo"}	   [software.state]	  The software state.
 * @apiParam {String}						             access_token     The access token.
 *
 * @apiSuccess {Number}									    id 		      The software identification.
 * @apiSuccess {String}									    name 			  The software name.
 * @apiSuccess {String}                     code		    The software code.
 * @apiSuccess {Date}									      created_at  The creation date.
 * @apiSuccess {Date}									      updated_at  The last update date.
 * @apiSuccess {String="Activo, Inactivo"}	state			  The software state.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "id": 34,
 *       "name": "Software #13",
 *       "code": "SF-13",
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
