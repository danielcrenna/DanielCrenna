using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DanielCrenna.Shared;
using Xunit;
using Xunit.Abstractions;

namespace DanielCrenna.Tests
{
    public class SerializationTests
    {
        private readonly ITestOutputHelper _output;

        public SerializationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Can_deserialize_posts()
        {
            var data = File.OpenRead("InputFiles/posts.json");
            var posts = await JsonSerializer.DeserializeAsync<BlogPosts>(data);

            Assert.NotNull(posts);
            Assert.NotEmpty(posts.Posts);

            foreach(var post in posts.Posts)
                _output.WriteLine(post.Title);
        }
    }
}
