/**
 * @api {get} /v1/software Retrieve a list of software.
 * @apiVersion 1.0.0
 * @apiName GetSoftwares
 * @apiGroup Software
 * @apiPermission any
 * @apiUse BaseError
 *
 * @apiDescription Retrieve a list of software.
 *
 * @apiParamExample Example:
 *      GET /software?access_token=xxx.yyy.zzz HTTP/1.1
 *
 * @apiParam {String}       access_token    The access token.
 * @apiParam {String[]}     [q]             A query to filter the results.
 * @apiParam {String[]}     [fields]        A list of fields to include in the results.
 * @apiParam {String[]}     [sort]          A list of fields to sort the results.
 * @apiParam {Number}       [page=1]        The page number.
 * @apiParam {Number}       [limit=20]      The amount of results by page.
 *
 * @apiSuccess {Number}                     total_pages             The total amount of pages for this query.
 * @apiSuccess {Number}                     current_page            The current page number.
 * @apiSuccess {Object[]}                   results                 The list of softwares.
 * @apiSuccess {Number}                     results.id              The software identity.
 * @apiSuccess {String}                     results.name            The software name.
 * @apiSuccess {String}                     results.code            The software code.
 * @apiSuccess {Date}                       results.created_at      The creation date.
 * @apiSuccess {Date}                       results.updated_at      The last update date.
 * @apiSuccess {String="Activo, Inactivo"}  results.state           The software state.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *       "total_pages": 1,
 *       "current_page", 1,
 *       "results": [
 *         {
 *           "id": 1136,
 *           "name": "Software #12",
 *           "code": "SF-12",
 *           "state": "Activo",
 *           "created_at": "2015-08-27T22:14:20.646Z",
 *           "updated_at": "2015-08-27T22:14:20.646Z"
 *         },
 *         {
 *           "id": 1137,
 *           "name": "TOEFL Practice",
 *           "code": "SF-134",
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
