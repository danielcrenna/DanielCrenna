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
            var posts = new List<BlogPost>
            {
                new BlogPost
                {
                    Title = "Hello, World!",
                    Body = "This is a text with some *emphasis*.",
                    PublishedAt = new DateTimeOffset(2020, 10, 4, 16, 40, 0, DateTimeOffset.Now.Offset)
                }
            };

            return Ok(posts);
        }
    }
}