/**
 * @api {get} /v1/laboratories/:id/software Get the laboratory software.
 * @apiVersion 1.0.0
 * @apiName GetLaboratorySoftware.
 * @apiGroup Laboratory/Software
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Get the laboratory software.
 *
 * @apiExample Example:
 *     GET /laboratories/45/software HTTP/1.1
 *
 * @apiParam {String}						access_token    The access token.
 * @apiParam {String[]} [q]				A query to filter the results.
 * @apiParam {String[]} [fields]		A list of fields to include in the results.
 * @apiParam {String[]} [sort]			A list of fields to sort the results.
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
 *     [
 *       {
 *         "id": 1,
 *         "name": "Software #11",
 *         "code": "SF-11",
 *         "state": "Activo",
 *         "created_at": "2015-08-27T22:14:20.646Z",
 *         "updated_at": "2015-08-27T22:14:20.646Z"
 *       },
 *       {
 *         "id": 2,
 *         "name": "Software #12",
 *         "code": "SF-12",
 *         "state": "Activo",
 *         "created_at": "2015-08-27T22:14:20.646Z",
 *         "updated_at": "2015-08-27T22:14:20.646Z"
 *       }
 *    ]
 *
 * @apiErrorExample {json} Error-Response:
 *     HTTP/1.1 401 Unauthorized
 *     {
 *       "code": 401,
 *       "error": "Unauthorized",
 *       "description": "You don't have permissions to perform this operation."
 *     }
 */
