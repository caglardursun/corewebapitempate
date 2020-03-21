using API.Data;
using API.Data.Entity;
using API.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace API.Data.DataManager
{
    public class GeneralUserManager : DbFactoryBase, IGeneralUserManager
    {
        private ILogger<GeneralUserManager> _logger;
        public GeneralUserManager(
            IConfiguration config
            , ILogger<GeneralUserManager> logger
            ) : base(config)
        {

            _logger = logger;
        }

        public Task<long> CreateAsync(GeneralUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(object Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistAsync(object Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GeneralUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GeneralUser> GetByIdAsync(object Id)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralUser> GetGeneralUserByUserName(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<(IEnumerable<GeneralUser> GeneralUsers, Pagination Pagination)> SearchGeneralUsersAsync(UrlQuerySearchParameters urlSearchParams)
        {
            return await DbQueryPagedAsync<GeneralUser>(urlSearchParams, new SqlBuilder());
        }

        public Task<bool> UpdateAsync(GeneralUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
