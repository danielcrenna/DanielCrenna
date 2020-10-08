// Copyright (c) Daniel Crenna. All rights reserved.
// 
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, you can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using DanielCrenna.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DanielCrenna.Server.Controllers
{
    public class PostController : ControllerBase
    {
        [HttpGet("posts")]
        public IActionResult GetPosts()
        {
            #region For clarity, writing material is not covered under an Open Source License

            var posts = new List<BlogPost>
            {
                new BlogPost
                {
                    Title = "Hello, World!",
                    Body = "<pre><code class=\"csharp\">public static void Main(params string[] args) { }</code></pre>",
                    PublishedAt = new DateTimeOffset(2020, 10, 4, 16, 40, 0, DateTimeOffset.Now.Offset)
                },
                new BlogPost
                {
                    Title = "The inability to change is the heart of change.",
                    Body = "Immutability, or the inability for something to change once created, is a principle worth following far beyond double-entry accounting. " +
                           "An unused notebook seems full of promise, but the attachment to 'starting over' keeps the page empty. " +
                           "Embracing scribbles or strike-outs is one way to accept immutability, and when we get over the fascination we have with _starting over_," +
                           "then we can _start into_ the work we want to create.",
                    PublishedAt = new DateTimeOffset(2020, 10, 8, 0, 33, 0, DateTimeOffset.Now.Offset)
                },
            };

            #endregion

            return Ok(posts);
        }
    }
}