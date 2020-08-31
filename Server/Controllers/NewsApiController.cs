using Microsoft.AspNetCore.Mvc;
using NewsSource.Server.Services.Interfaces;
using NewsSource.Shared.Enums;
using NewsSource.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsSource.Server.Controllers
{
    [ApiController]
    [Route("news")]
    public class NewsApiController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsApiController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        [Route("headline")]
        [Produces("application/json")]
        public string Get()
        {
            return "This is a news headline";
        }
        
        [HttpGet]
        [Route("all")]
        [Produces("application/json")]
        public async Task<IEnumerable<Article>> GetAllArticlesAsync(string keyword,
                                                              string titleKeyword,
                                                              string sources,
                                                              string domains,
                                                              string excludedDomains,
                                                              DateTime? fromDate,
                                                              DateTime? toDate,
                                                              Language? language,
                                                              Sort? sortBy,
                                                              int? pageSize,
                                                              int? page)
        {
            var articles = await  _newsService.GetAllArticlesAsync(keyword, titleKeyword, sources, domains, excludedDomains, fromDate, toDate, language, sortBy, pageSize, page);

            return articles;
        }

        [HttpGet]
        [Route("top")]
        public Task<IEnumerable<Article>> GetTopHeadlinesAsync(Country? country,
                                                               Category? category,
                                                               string sources,
                                                               string keyword,
                                                               int? pageSize,
                                                               int? page)
        {
            return _newsService.GetTopHeadlinesAsync(country, category, sources, keyword, pageSize, page);
        }
    }
}
