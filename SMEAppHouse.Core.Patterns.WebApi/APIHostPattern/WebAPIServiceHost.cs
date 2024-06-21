// ReSharper disable InheritdocConsiderUsage

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces;
using SMEAppHouse.Core.Patterns.Repo.Repository.Abstractions;

namespace SMEAppHouse.Core.Patterns.WebApi.APIHostPattern
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WebApiServiceHost<TEntity, TPk> : WebApiServiceHostExt, IWebApiServiceHost<TEntity, TPk>
        where TPk : struct
        where TEntity : class, IEntityKeyed<TPk>
    {
        ///<inheritdoc cref="IIdentifiableEntity{TPk}"/>
        /// <summary>
        /// blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! 
        /// </summary>
        public IRepositoryForKeyedEntity<TEntity, TPk> Repository { get; set; }

        #region constructors

        /// <summary>
        /// Initiates this base class
        /// </summary>
        /// <param name="repository"></param>
        protected WebApiServiceHost(IRepositoryForKeyedEntity<TEntity, TPk> repository)
        {
            this.Repository = repository;
        }

        #endregion

        #region  IWebAPIServiceHost implementations

        /// <inheritdoc cref="IIdentifiableEntity{TPk}"/>
        /// <summary>
        /// blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Action]")]
        public virtual async Task<IActionResult> CreateSingleAsync(TEntity entity)
        {
            if (entity == null)
                return BadRequest($"Entity {nameof(entity)} received is null.");

            var result = await ExecuteAsync(async () =>
            {
                try
                {
                    await Repository.AddAsync(entity);
                    await Repository.DbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }

                return Ok(entity);
                //return Content(HttpStatusCode.OK, $"Entity added or saved: [ID:{entity.Id}]");
            });
            return result;
        }

        

        [HttpPost]
        [Route("[Action]")]
        public virtual async Task<IActionResult> CreateFromJsonAsync(object jsonOfEntity)
        {
            var jsonString = jsonOfEntity.ToString();
            var targetEntity = JsonConvert.DeserializeObject<TEntity>(jsonString);
            return await CreateSingleAsync(targetEntity);
        }

        [HttpPut]
        [Route("[Action]")]
        public virtual async Task<IActionResult> UpdateSingleAsync(TEntity entity)
        {
            if (entity == null)
                return BadRequest($"Entity {nameof(entity)} received is null.");


            return await ExecuteAsync(async () =>
            {
                try
                {
                    await Repository.UpdateAsync(entity);
                    await Repository.DbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest($"Operation error occured for {nameof(entity)}: {ex.Message}");
                }
                return Ok(entity);
                //return Content(HttpStatusCode.OK, "Entity updated.");

            });
        }

        [HttpPut]
        [Route("[Action]")]
        public virtual Task<IActionResult> UpdateManyAsync(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("[Action]")]
        public virtual async Task<IActionResult> UpdateFromJsonAsync(object jsonOfEntity)
        {
            var jsonString = jsonOfEntity.ToString();
            var targetEntity = JsonConvert.DeserializeObject<TEntity>(jsonString);
            return await UpdateSingleAsync(targetEntity);
        }

        [HttpDelete]
        [Route("[Action]")]
        public virtual async Task<IActionResult> RemoveByIdAsync(TPk id)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await Repository.GetSingleAsync(p => p.Id.Equals(id));

                if (entity == null)
                {
                    // Returns a NotFoundResult
                    //return Content(HttpStatusCode.NotFound, "Entity not found.");
                    return NotFound(id);
                }

                await Repository.DeleteAsync(entity);
                await Repository.DbContext.SaveChangesAsync();

                return Ok("Success");
            });
        }

        [HttpDelete]
        [Route("[Action]")]
        public virtual async Task<IActionResult> RemoveManyAsync(TPk[] ids)
        {
            return await ExecuteAsync(async () =>
            {
                var many = await Repository.GetListAsync(p => ids.Contains(p.Id));
                var genericEntityBases = many.ToList();
                if (!genericEntityBases.Any())
                    return Ok($"IDs {string.Join(",", ids)} seem to not exist.");
                Repository.DbContext.RemoveRange(genericEntityBases);
                await Repository.DbContext.SaveChangesAsync();
                return Ok("Success");
            });
        }

        [HttpDelete]
        [Route("[Action]")]
        public virtual async Task<IActionResult> ZapAsync()
        {
            return await ExecuteAsync(async () =>
            {
                var entities = await Repository.GetListAsync();
                var genericEntityBases = entities.ToList();

                if (!genericEntityBases.Any())
                    return Ok("Success but none to remove");

                Repository.DbContext.RemoveRange(genericEntityBases);
                await Repository.DbContext.SaveChangesAsync();

                // Returns an OkNegotiatedContentResult
                return Ok("Success");
            });
        }

        [HttpGet]
        [Route("[Action]")]
        public virtual async Task<IActionResult> CountAsync()
        {
            return await ExecuteAsync(async () =>
            {
                var entities = await Repository.GetListAsync();
                return Ok(entities?.Count() ?? 0);
            });
        }

        [HttpGet]
        [Route("[Action]")]
        public virtual async Task<IActionResult> GetByIdAsync(TPk id)
        {
            return await ExecuteAsync(async () =>
            {
                var result = await Repository.GetSingleAsync(p => p.Id.Equals(id));

                // Returns a NotFoundResult
                if (result == null) return NotFound();

                // Returns an OkNegotiatedContentResult
                return Ok(result);
            });
        }

        [HttpGet]
        [Route("[Action]")]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            return await ExecuteAsync(async () =>
            {
                var all = await Repository.GetListAsync();

                // Returns an OkNegotiatedContentResult
                return Ok(all);
            });
        }

        [HttpGet]
        [Route("[Action]")]
        public virtual string GetHeader(string headerName)
        {
            return Request.Headers.TryGetValue(headerName, out var headerValues)
                ? headerValues.FirstOrDefault()
                : "";
        }

        [HttpGet]
        [Route("[Action]")]
        public virtual DateTime GetLocalTz(DateTime utcDateTime, int timeZoneOffset)
        {
            try
            {
                var utcOffset = TimeSpan.FromMinutes(timeZoneOffset);
                return utcDateTime.Add(utcOffset);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Route("[Action]")]
        public virtual DateTime ToClientTz(DateTime utcDateTime, int timeZoneOffset)
        {
            try
            {
                var utcOffset = TimeSpan.FromMinutes(timeZoneOffset);
                return utcDateTime.Add(utcOffset);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Route("[Action]")]
        public virtual Task<IActionResult> CreateManyAsync(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        protected override async Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> executeActionAsync)
        {
            try
            {
                return await executeActionAsync();
            }
            catch (AggregateException ae)
            {
                var errs = ae.Flatten().InnerExceptions.Aggregate(string.Empty, (current, innerException) => current + innerException.Message);
                throw new Exception($"Errors: {errs}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public Task<IActionResult> GetAndIncludeAsync(string entitiesToInclude)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetAndConditionalAsync([FromBody] string whereStr, [FromBody] TEntity entityParam)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}




#region others

///// <summary>
///// 
///// </summary>
///// <param name="conditions"></param>
///// <param name="entitiesToInclude"></param>
///// <returns></returns>
//public virtual IActionResult Get(Expression<Func<TEntity, bool>> conditions, string entitiesToInclude)
//{
//    return Execute(() =>
//    {
//        var all = Repository.GetAll(conditions, null, 0, entitiesToInclude);

//        // Returns an OkNegotiatedContentResult
//        return Ok(all);
//    });
//}



//#region associated user context

//private ApplicationUser _member;

//public ApplicationUserManager UserManager => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

//public string UserIdentityId
//{
//    get
//    {
//        var claimsIdent = (ClaimsIdentity)HttpContext.Current.User.Identity;
//        //string username = identity.Claims.First().Value;
//        var user = UserManager.FindByName(claimsIdent.Name);
//        return user.Id;
//    }
//}

//public ApplicationUser UserRecord
//{
//    get
//    {
//        if (_member != null)
//            return _member;

//        _member = UserManager.FindByEmail(Thread.CurrentPrincipal.Identity.Name);
//        return _member;
//    }
//    set => _member = value;
//}

//#endregion

#endregion