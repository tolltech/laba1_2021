﻿using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Laba1;
using Laba2;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace ServerApp
{
    [ApiController]
    [Route("study")]
    public class StudyController : Controller
    {
        private static readonly ConcurrentDictionary<string, KeyValue> repo = new ConcurrentDictionary<string, KeyValue>();
        private readonly Serializer serializer;

        public StudyController()
        {
            serializer = new Serializer();
        }

        [Route("ping")]
        [HttpGet]
        public IActionResult Index()
        {
            return Content("Ok");
        }
        
        [Route("find")]
        [HttpGet]
        public IActionResult Find(string key)
        {
            if (repo.TryGetValue(key, out var keyValue))
            {
                return Content(serializer.SerializeJson(keyValue));
            }

            return Content(serializer.SerializeJson<KeyValue>(null));
        }
        
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            using var streamReader = new StreamReader(Request.Body);
            var body = await streamReader.ReadToEndAsync();
            var keyValue = serializer.DeserializeJson<KeyValue>(body);

            if (repo.ContainsKey(keyValue.Key))
            {
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = $"Key {keyValue.Key} is already presented in store.";
                return StatusCode((int)HttpStatusCode.BadRequest, string.Empty);
            }
            else
            {
                repo[keyValue.Key] = keyValue;
            }

            return Ok();
        }

        [Route("update")]
        [HttpPost]
        public IActionResult Update(string key, string value)
        {
            if (repo.TryGetValue(key, out var keyValue))
            {
                keyValue.Value = value;
            }
            else
            {
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = $"Key {key} is not presented in store.";
                return StatusCode((int)HttpStatusCode.BadRequest, string.Empty);
            }

            return Ok();
        }
    }
    
    
}