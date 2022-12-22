using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Api.Dtos
{
    public class ResponseDto
    {
        public int Code { get; set; }
        public string ShortMessage { get; set; }
        public object Result { get; set; }
    }
}