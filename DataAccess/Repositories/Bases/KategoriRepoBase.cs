﻿using AppCore.DataAccess.EntityFramework.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace DataAccess.Repositories.Bases
{
    public abstract class KategoriRepoBase : RepoBase<Kategori, ETicaretContext>
    {
        protected KategoriRepoBase() : base()
        {

        }

        protected KategoriRepoBase(ETicaretContext eTicaretContext) : base(eTicaretContext)
        {

        }
    }
}
