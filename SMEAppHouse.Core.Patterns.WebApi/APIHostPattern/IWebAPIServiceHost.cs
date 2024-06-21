using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces;
using SMEAppHouse.Core.Patterns.Repo.Repository.Abstractions;

namespace SMEAppHouse.Core.Patterns.WebApi.APIHostPattern
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPk"></typeparam>
    public interface IWebApiServiceHost<TEntity, TPk>
        where TPk : struct
        where TEntity : class, IEntityKeyed<TPk>

    {
        /// <summary>
        /// Reference to the entity's data repository for handling backend data
        /// </summary>
        IRepositoryForKeyedEntity<TEntity, TPk> Repository { get; set; }

        /// <summary>
        /// Create instance of the <see cref="TEntity"/>
        /// </summary>
        /// <param name="entity"></param>
        Task<IActionResult> CreateSingleAsync(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<IActionResult> CreateManyAsync(List<TEntity> entities);

        /// <summary>
        /// https://stackoverflow.com/questions/24129919/web-api-complex-parameter-properties-are-all-null/24131860#24131860
        /// </summary>
        /// <param name="jsonOfEntity"></param>
        /// <returns></returns>
        Task<IActionResult> CreateFromJsonAsync(object jsonOfEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<IActionResult> UpdateSingleAsync(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<IActionResult> UpdateManyAsync(List<TEntity> entities);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonOfEntity"></param>
        /// <returns></returns>
        Task<IActionResult> UpdateFromJsonAsync(object jsonOfEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        Task<IActionResult> RemoveByIdAsync(TPk id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IActionResult> RemoveManyAsync(TPk[] ids);

        /// <summary>
        /// Obliterate all of the records in this table or collection
        /// </summary>
        Task<IActionResult> ZapAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> CountAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IActionResult> GetByIdAsync(TPk id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> GetAllAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitiesToInclude"></param>
        /// <returns></returns>
        Task<IActionResult> GetAndIncludeAsync(string entitiesToInclude);

        /// <summary>
        /// Important: [FromBody] must be prefixed inside web method 
        /// definition as part of the complexQuery parameter    
        /// </summary>
        /// <param name="whereStr"></param>
        /// <param name="entityParam"></param>
        /// <returns></returns>
        Task<IActionResult> GetAndConditionalAsync([FromBody]string whereStr, [FromBody]TEntity entityParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns></returns>
        string GetHeader(string headerName);

        DateTime GetLocalTz([FromBody] DateTime utcDateTime, [FromBody] int timeZoneOffset);
        DateTime ToClientTz(DateTime utcDateTime, int timeZoneOffset);
    }


}

