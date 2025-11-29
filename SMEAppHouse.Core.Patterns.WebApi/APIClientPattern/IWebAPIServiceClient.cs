// ***********************************************************************
// Assembly         : SMEAppHouse.Core.Patterns.WebApi
// Author           : jcman
// Created          : 07-04-2018
//
// Last Modified By : jcman
// Last Modified On : 08-01-2018
// ***********************************************************************
// <copyright file="IWebAPIServiceClient.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;

namespace SMEAppHouse.Core.Patterns.WebApi.APIClientPattern
{
    public interface IWebApiServiceClient<TEntity, in TPk>
        where TEntity : class, IEntity
    {
        HttpClient HttpClient { get; }
        string BaseServiceAddress { get;  }

        TEntity Create(TEntity entity);
        TEntity Update(TEntity entity);

        void RemoveById(TPk id);
        void RemoveAll();

        int Count();

        TEntity GetById(TPk id);

        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllWithEntities(params string[] entities);

    }
}