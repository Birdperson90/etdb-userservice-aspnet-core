﻿using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;

namespace EntertainmentDatabase.REST.ServiceBase.Generics.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void UseGuidPrimaryKey<T>(this ModelBuilder modelBuilder) where T : class, IEntity, new()
        {
            modelBuilder.Entity<T>(builder =>
            {
                builder.HasKey(entity => entity.Id);

                builder.Property(entity => entity.Id)
                    .ForSqlServerHasDefaultValueSql("newid()");
            });
        }

        public static void UseConccurencyToken<T>(this ModelBuilder modelBuilder) where T : class, IEntity, new()
        {
            modelBuilder.Entity<T>(builder =>
            {
                builder.Property(entity => entity.RowVersion)
                    .ValueGeneratedOnAddOrUpdate()
                    .IsConcurrencyToken();
            });
        }

        public static void DisableCascadeDelete(this ModelBuilder modelBuilder)
        {
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}