/**
 * @api {put} /v1/laboratories/:id Update a laboratory.
 * @apiVersion 1.0.0
 * @apiName UpdateLaboratory
 * @apiGroup Laboratory
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Update a laboratory.
 *
 * @apiExample Example:
 *     PUT /laboratories/1136 HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "laboratory": {
 *         "seats": 25,
 *       },
 *       "access_token": "..."
 *     }
 *
 * @apiParam {Object}						             laboratory 		      The laboratory data.
 * @apiParam {String}						             [laboratory.name]    The laboratory name.
 * @apiParam {Number}						             [laboratory.seats]   The laboratory available seats.
 * @apiParam {String="Activo, Inactivo"}	   [laboratory.state]	  The laboratory state.
 * @apiParam {String}						             access_token     The access token.
 *
 * @apiSuccess {Number}									    id 		      The laboratory identification.
 * @apiSuccess {String}									    name 			  The laboratory name.
 * @apiSuccess {Number}                     seats       The laboratory available seats.
 * @apiSuccess {Date}									      created_at  The creation date.
 * @apiSuccess {Date}									      updated_at  The last update date.
 * @apiSuccess {String="Activo, Inactivo"}	state			  The laboratory state.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "id": 1136,
 *       "name": "Laboratorio B1",
 *       "seats": 25,
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
