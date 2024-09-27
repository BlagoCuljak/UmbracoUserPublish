using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Web.Common.Controllers;
using System;

namespace UmbracoUserPublish.Controllers
{
    [Route("api/newpost")]
    public class PostController : ControllerBase
    {
        private readonly IContentService _contentService;

        public PostController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpPost]
        public IActionResult CreatePost([FromBody] PostRequest postRequest)
        {
            if (postRequest == null || string.IsNullOrEmpty(postRequest.Name) || string.IsNullOrEmpty(postRequest.Text))
            {
                return BadRequest("Invalid input data.");
            }

            // The GUID of the parent node "posts"
            Guid contentGuid = new Guid("e11d3218-5e98-4dce-85c1-9151a3e85688");

            // Retrieve the parent content using the GUID
            var parentContent = _contentService.GetById(contentGuid);
            if (parentContent == null)
            {
                return NotFound("Parent content not found.");
            }

            // Create a new content item under the "posts" node
            var post = _contentService.Create(postRequest.Name, parentContent, "post");

            // Set the values for the new content
            post.Name = postRequest.Name;
            post.SetValue("text", postRequest.Text);

            // Save and publish the new post
            _contentService.Save(post);
            // Using real user Id to make the publish work
            //var publishResult = _contentService.Publish(post, new[] { "*" }, -1);

            //Using 0 to publish the content as non-exising user 
            var publishResult = _contentService.Publish(post, new[] { "*" }, 0);

            if (!publishResult.Success)
            {
                return StatusCode(500, "Error publishing the post.");
            }

            return Ok("Post created and published successfully.");
        }
    }

    public class PostRequest
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }
}
