/**
 * @api {get} /v1/software/:code Retrieve a software.
 * @apiVersion 1.0.0
 * @apiName GetSoftware
 * @apiGroup Software
 * @apiPermission any
 * @apiUse BaseError
 *
 * @apiDescription Retrieve a software.
 *
 * @apiExample Example:
 * 		GET /software/SF-12 HTTP/1.1
 *
 * @apiParam {String}	access_token	The access token.
 * @apiParam {String[]} [fields]		A list of fields to include in the results.
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
 *       "id": 1136,
 *       "name": "Software #12",
 *       "code": "SF-12",
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
