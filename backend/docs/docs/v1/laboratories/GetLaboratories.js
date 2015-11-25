/**
 * @api {get} /v1/laboratories Retrieve a list of laboratories.
 * @apiVersion 1.0.0
 * @apiName GetLaboratories
 * @apiGroup Laboratories
 * @apiPermission any
 * @apiUse BaseError
 *
 * @apiDescription Retrieve a list of laboratories.
 *
 * @apiParamExample Example:
 *      GET /laboratories?access_token=xxx.yyy.zzz HTTP/1.1
 *
 * @apiParam {String}       access_token    The access token.
 * @apiParam {String[]}     [q]             A query to filter the results.
 * @apiParam {String[]}     [fields]        A list of fields to include in the results.
 * @apiParam {String[]}     [sort]          A list of fields to sort the results.
 * @apiParam {Number}       [page=1]        The page number.
 * @apiParam {Number}       [limit=20]      The amount of results by page.
 *
 * @apiSuccess {Number}                     total_pages                         The total amount of pages for this query.
 * @apiSuccess {Number}                     current_page                        The current page number.
 * @apiSuccess {Object[]}                   results                             The list of laboratories.
 * @apiSuccess {Number}                     results.id                          The laboratory identity.
 * @apiSuccess {String}                     results.name                        The laboratory name.
 * @apiSuccess {Number}                     results.seats                       The laboratory available seats.
 * @apiSuccess {Number}                     results.appointment_priority        The priority of appointments in the laboratory. (1: High, 2: Medium, 3: Low)
 * @apiSuccess {Number}                     results.reservation_priority        The priority of reservations in the laboratory. (1: High, 2: Medium, 3: Low)
 * @apiSuccess {Date}                       results.created_at                  The creation date.
 * @apiSuccess {Date}                       results.updated_at                  The last update date.
 * @apiSuccess {String="Activo, Inactivo"}  results.state                       The laboratory state.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "total_pages": 1,
 *       "current_page", 1,
 *       "results": [
 *         {
 *           "id": 1,
 *           "name": "Laboratorio B1",
 *           "seats": 20,
 *           "appointment_priority": 1,
 *           "reservation_priority": 3,
 *           "state": "Activo",
 *           "created_at": "2015-08-27T22:14:20.646Z",
 *           "updated_at": "2015-08-27T22:14:20.646Z"
 *         },
 *         {
 *           "id": 2,
 *           "name": "Laboratorio A1",
 *           "seats": 35,
 *           "appointment_priority": 3,
 *           "reservation_priority": 1,
 *           "state": "Activo",
 *           "created_at": "2015-08-27T22:14:20.646Z",
 *           "updated_at": "2015-08-27T22:14:20.646Z"
 *         }
 *       ]
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
