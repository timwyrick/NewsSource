using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

using NewsSource.Server.Services.Interfaces;
using NewsSource.Shared.Enums;
using NewsSource.Shared.Models;

namespace NewsSource.Server.Services
{
    public class NewsService : INewsService
    {
        private readonly IConfiguration _configuration;
        private readonly string _newsApiBaseUrl;
        private readonly string _newsApiKey;
        private readonly string _newsApiDefaultPageSize;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;

        public NewsService(IHttpClientFactory clientFactory,
                           IConfiguration configuration)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient();
            _newsApiBaseUrl = _configuration.GetValue<string>("NewsApiBaseUrl");
            _newsApiKey = _configuration.GetValue<string>("NewsApiKey");
            _newsApiDefaultPageSize = _configuration.GetValue<string>("NewsApiDefaultPageSize");
        }

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
            StringBuilder requestUri = new StringBuilder();

            requestUri.Append($"{_newsApiBaseUrl}everything?");

            if (!string.IsNullOrEmpty(keyword))
                requestUri.Append($"q={HttpUtility.UrlEncode(keyword)}&");

            if (!string.IsNullOrEmpty(titleKeyword))
                requestUri.Append($"qlnTitle={HttpUtility.UrlEncode(titleKeyword)}&");

            if (!string.IsNullOrEmpty(sources))
                requestUri.Append($"sources={sources}&");

            if (!string.IsNullOrEmpty(domains))
                requestUri.Append($"domains={domains}&");

            if (!string.IsNullOrEmpty(excludedDomains))
                requestUri.Append($"excludeDomains={excludedDomains}&");

            if (fromDate != null)
            {
                var fDate = fromDate?.ToUniversalTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture);
                requestUri.Append($"from={fDate}&");
            }

            if (toDate != null)
            {
                var tDate = toDate?.ToUniversalTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture);
                requestUri.Append($"to={tDate}&");
            }

            if (language != null)
                requestUri.Append($"language={language.ToString()}&");

            if (language != null)
                requestUri.Append($"language={language.ToString()}&");

            if (sortBy != null)
                requestUri.Append($"language={sortBy.ToString()}&");

            if(pageSize != null)
            {
                requestUri.Append($"pageSize={pageSize}&");
            }
            else
            {
                requestUri.Append($"pageSize={_newsApiDefaultPageSize}&");
            }

            if(page != null)
            {
                requestUri.Append($"page={page}&");
            }
            else
            {
                requestUri.Append("page=1&");
            }

            requestUri.Append($"apiKey={_newsApiKey}");

            var response = await _client.GetAsync(requestUri.ToString()).ConfigureAwait(false);
            if(response.IsSuccessStatusCode)
            {
                var newsData = JsonConvert.DeserializeObject<NewsResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                return newsData.Articles;
            }

            return null;
        }

        public async Task<IEnumerable<Article>> GetTopHeadlinesAsync(Country? country,
                                                                     Category? category,
                                                                     string sources,
                                                                     string keyword,
                                                                     int? pageSize,
                                                                     int? page)
        {
            StringBuilder requestUri = new StringBuilder();

            requestUri.Append($"{_newsApiBaseUrl}top-headlines?");

            if (country != null)
            {
                requestUri.Append($"country={country.ToString()}&");
            }
            else
            {
                requestUri.Append($"country={Country.US.ToString()}&");
            }

            if(category != null)
                requestUri.Append($"category={category.ToString()}&");

            if (!string.IsNullOrEmpty(sources))
                requestUri.Append($"sources={sources}&");

            if (!string.IsNullOrEmpty(keyword))
                requestUri.Append($"q={HttpUtility.UrlEncode(keyword)}&");

            if (pageSize != null)
            {
                requestUri.Append($"pageSize={pageSize}&");
            }
            else
            {
                requestUri.Append($"pageSize={_newsApiDefaultPageSize}&");
            }

            if (page != null)
            {
                requestUri.Append($"page={page}&");
            }
            else
            {
                requestUri.Append("page=1&");
            }

            requestUri.Append($"apiKey={_newsApiKey}");

            var response = await _client.GetAsync(requestUri.ToString()).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var newsData = JsonConvert.DeserializeObject<NewsResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                return newsData.Articles;
            }

            return null;
        }
    }
}
