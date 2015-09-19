/**
 * @api {delete} /v1/laboratories/:id/software Remove software from a laboratory.
 * @apiVersion 1.0.0
 * @apiName RemoveSoftwareFromLaboratory
 * @apiGroup Laboratory/Software
 * @apiPermission operator
 * @apiUse BaseError
 *
 * @apiDescription Remove software from a laboratory.
 *
 * @apiExample Example:
 *     DELETE /laboratories/45/software HTTP/1.1
 *     Content-Type: application/json
 *     {
 *       "software": ["SF-01", "SF-02", "SF-03"],
 *       "access_token": "..."
 *     }
 *
 * @apiParam {Object[]}				  software  The software list.
 * @apiParam {String}						access_token    The access token.
 *
 * @apiSuccessExample {json} Success-Response:
 *     HTTP/1.1 200 OK
 *
 * @apiErrorExample {json} Error-Response:
 *     HTTP/1.1 401 Unauthorized
 *     {
 *       "code": 401,
 *       "error": "Unauthorized",
 *       "description": "You don't have permissions to perform this operation."
 *     }
 */
