using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Dywq.Infrastructure.Core.Extensions;
namespace Dywq.Infrastructure.Core.Utilitiy
{


    public interface IMd5
    {
        string Md5(string input);
    }


    public class MyMd5 : IMd5
    {
        readonly IOptions<Md5Options> _options;

        public MyMd5(IOptions<Md5Options> options)
        {
            _options = options;
        }

        public virtual string Md5(string input)
        {
            return $"{_options?.Value?.Prefix}-{input}".Md5();
        }
    }
}
