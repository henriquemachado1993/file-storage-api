using System;
using System.Collections.Generic;
using System.Text;

namespace SkinkiDriverApi.ValueObjects
{
    public class PagingResult<T> : BusinessResult<T>
    {
        public PageResult Paging { get; set; }

        public PagingResult(T data) : base(data)
        {
            Data = data;
            Messages = new List<MessageResult>();
            Paging = new PageResult();
        }

        public static PagingResult<T> CreateValidResultPaging(T model, PageResult pageResult)
        {
            var pagination = new PagingResult<T>(model);
            pagination.Paging = pageResult;
            return pagination;
        }

        public static PagingResult<T> CreateValidResultPaging()
        {
            return new PagingResult<T>(default);
        }

        public static PagingResult<T> CreateInvalidResultPaging(List<MessageResult> message)
        {
            var bo = new PagingResult<T>(default);
            bo.WithErrors(message);
            return bo;
        }

        public static PagingResult<T> CreateInvalidResultPaging(MessageResult message)
        {
            var bo = new PagingResult<T>(default);
            bo.WithErrors(message);
            return bo;
        }
    }
}
