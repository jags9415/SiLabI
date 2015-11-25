/**
 * @api {get} /v1/groups/:id Retrieve a group.
 * @apiVersion 1.0.0
 * @apiName GetGroup
 * @apiGroup Groups
 * @apiPermission any
 * @apiUse BaseError
 *
 * @apiDescription Retrieve a group.
 *
 * @apiExample Example:
 *   GET /groups/87?access_token=xxx.yyy.zzz HTTP/1.1
 *
 * @apiParam {String}       access_token    The access token.
 * @apiParam {String[]}     [fields]        A list of fields to include in the results.
 *
 * @apiSuccess {Number}                     id              The group identity.
 * @apiSuccess {Number}                     number          The group number.
 * @apiSuccess {Object}                     course          The course data.
 * @apiSuccess {Object}                     professor       The professor data.
 * @apiSuccess {Object}                     period          The period data.
 * @apiSuccess {Date}                       created_at      The creation date.
 * @apiSuccess {Date}                       updated_at      The last update date.
 * @apiSuccess {String="Activo, Inactivo"}  state           The group state.
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
