using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using NewsSource.Shared.Enums;
using NewsSource.Shared.Models;

namespace NewsSource.Server.Services.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<Article>> GetAllArticlesAsync(string keyword,
                                                       string titleKeyword,
                                                       string sources,
                                                       string domains,
                                                       string excludedDomains,
                                                       DateTime? fromDate,
                                                       DateTime? toDate,
                                                       Language? language,
                                                       Sort? sortBy,
                                                       int? pageSize,
                                                       int? page);

        Task<IEnumerable<Article>> GetTopHeadlinesAsync(Country? country,
                                                        Category? category,
                                                        string sources,
                                                        string keyword,
                                                        int? pageSize,
                                                        int? page);
    }
}
