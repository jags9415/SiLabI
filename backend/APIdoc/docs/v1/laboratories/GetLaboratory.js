/**
 * @api {get} /v1/laboratories/:id Retrieve a laboratory.
 * @apiVersion 1.0.0
 * @apiName GetLaboratory
 * @apiGroup Laboratory
 * @apiPermission any
 * @apiUse BaseError
 *
 * @apiDescription Retrieve a laboratory.
 *
 * @apiExample Example:
 * 		GET /laboratories/87 HTTP/1.1
 *
 * @apiParam {String}	access_token	The access token.
 * @apiParam {String[]} [fields]		A list of fields to include in the results.
 *
 * @apiSuccess {Number}									    id 		      The laboratory identification.
 * @apiSuccess {String}									    name 			  The laboratory name.
 * @apiSuccess {Number}                     seats		    The laboratory available seats.
 * @apiSuccess {Date}									      created_at  The creation date.
 * @apiSuccess {Date}									      updated_at  The last update date.
 * @apiSuccess {String="Activo, Inactivo"}	state			  The laboratory state.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "id": 1136,
 *       "name": "Laboratorio B1",
 *       "seats": 20,
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
