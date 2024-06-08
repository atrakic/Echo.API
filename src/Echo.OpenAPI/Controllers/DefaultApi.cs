/*
 * Echo API
 *
 * A simple API to store and retrieve messages.
 *
 * The version of the OpenAPI document: 0.0.1
 *
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using Echo.OpenAPI.Attributes;
using Echo.OpenAPI.Models;

namespace Echo.OpenAPI.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiController]
    public class DefaultApiController : ControllerBase
    {
        /// <summary>
        /// Search messages
        /// </summary>
        /// <param name="query"></param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/messages")]
        [ValidateModelState]
        [SwaggerOperation("MessagesGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Message>), description: "OK")]
        public virtual IActionResult MessagesGet([FromQuery (Name = "query")]string query)
        {

            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(List<Message>));
            string exampleJson = null;
            exampleJson = "[ {\n  \"id\" : 0,\n  \"message\" : \"message\"\n}, {\n  \"id\" : 0,\n  \"message\" : \"message\"\n} ]";

            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<List<Message>>(exampleJson)
            : default(List<Message>);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// Store a new message
        /// </summary>
        /// <param name="message"></param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/messages")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("MessagesPost")]
        public virtual IActionResult MessagesPost([FromBody]Message message)
        {

            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            throw new NotImplementedException();
        }
    }
}
