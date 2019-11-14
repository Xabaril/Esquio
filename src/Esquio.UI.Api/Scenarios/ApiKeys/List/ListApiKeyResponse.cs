using System;
using System.Collections.Generic;

namespace Esquio.UI.Api.Features.ApiKeys.List
{
    public class ListApiKeyResponse
    {
        public int Total { get; set; }

        public int PageIndex { get; set; }

        public int Count { get; set; }

        public List<ListApiKeyResponseDetail> Result { get; set; }
    }

    public class ListApiKeyResponseDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
