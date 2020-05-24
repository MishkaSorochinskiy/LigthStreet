using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigthStreet.WebApi.Identity.Token
{
    public interface ITokenProvider
    {
        Task<string> GetToken(string subjectId, string deviceId, string secret);
    }
}
